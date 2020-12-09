using NIST_STS.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestScryptOG
{
    class Utilities
    {
        private List<ITest> _runList;
        private int _sequencesCount;
        private int _sequenceLength;
        private IBitSequenceGenerator _generator;
        private bool _firstRun;

        public Utilities()
        {
            this._runList = new List<ITest>
            {
                new FrequencyTest(),
                new BlockFrequencyTest(),
                new CusumTest(),
                new RunsTest(),
                new ApproximateEntropyTest(),
                new SerialTest()
            };

            this._sequencesCount = 100;
            this._sequenceLength = 10000;
            this._generator = new GeneratorOnRandomPassAndSalt();
            this._firstRun = true;
        }

        public void ShowMain()
        {
            if (this._firstRun)
            {
                Help();
                this._firstRun = false;
            }

            while (true)
            {
                Console.Write("Enter command:> ");

                switch (Console.ReadLine().Replace(" ", "").ToLower())
                {
                    case "add":
                        ShowAddTestMenu();
                        break;
                    case "chg":
                        ChangeGenerator();
                        break;
                    case "chsc":
                        ChangeSeqCount();
                        break;
                    case "chsl":
                        ChangeSeqLength();
                        break;
                    case "clear":
                        ClearRunList();
                        break;
                    case "del":
                        ShowDeleteTestMenu();
                        break;
                    case "exit":
                        return;
                    case "help":
                        Help();
                        break;
                    case "run":
                        ShowResults(RunTesting());
                        break;
                    case "shconf":
                        ShowInfo();
                        break;
                    case "sht":
                        PrintTests();
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private (ITest tets, int success)[] RunTesting()
        {
            (ITest test, int success)[] results = new (ITest, int)[this._runList.Count];

            for (int i = 0; i < results.Length; i++)
            {
                results[i].test = this._runList[i];
            }

            if (this._generator is GeneratorWherePassAndSaltFromConsole)
            {
                for (int i = 0; i < this._sequencesCount; i++)
                {
                    Console.WriteLine("{0})", i);
                    BitArray sequence = this._generator.Generate(this._sequenceLength);

                    for (int k = 0; k < results.Length; k++)
                    {
                        if (results[k].test.Run(sequence)) results[k].success++;
                    }
                }
            }
            else
            {
                Console.WriteLine("Testing... ");
                using (ProgressBar progress = new ProgressBar())
                {
                    for (int i = 0; i < this._sequencesCount; i++)
                    {
                        BitArray sequence = this._generator.Generate(this._sequenceLength);

                        for (int k = 0; k < results.Length; k++)
                        {
                            if (results[k].test.Run(sequence)) results[k].success++;
                            progress.Report(((double)i * results.Length + k + 1) /
                                (this._sequencesCount * results.Length));
                        }
                    }
                }
                Console.WriteLine("Done.");
            }

            return results;
        }

        private void ShowResults((ITest test, int success)[] results)
        {
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t R E S U L T S \n");
            Console.WriteLine(Enumerable.Repeat('#', 90).ToArray());
            Console.WriteLine("Count os sequences: {0} \n", this._sequencesCount);
            Console.WriteLine(string.Format("{0,45}{1,10}{2,10}{3,10}\n",
                "Test", "Success", "Percent", "Warning"));

            foreach ((ITest test, int success) in results)
            {
                double percent = (double)success / this._sequencesCount;
                string mark = (percent < 0.99) ? "*" : "";
                string row = String.Format("{0,45}{1,10}{2,10}{3,10}\n", test, success,
                    (double)success / this._sequencesCount, mark);
                Console.WriteLine(row);
            }

            Console.WriteLine(Enumerable.Repeat('#', 90).ToArray());
            Console.WriteLine("\n\n");
        }

        private void Help()
        {
            Console.WriteLine();
            Console.WriteLine("\t C O M M A N D S \n");
            Console.WriteLine("##########################################");
            Console.WriteLine("[add] - add test to runlist");
            Console.WriteLine("[chg] - change generator");
            Console.WriteLine("[chsc] - change sequences count");
            Console.WriteLine("[chsl] - change sequence length");
            Console.WriteLine("[clear] - cleare runlist");
            Console.WriteLine("[del] - delete test from runlist");
            Console.WriteLine("[exit] - close programm");
            Console.WriteLine("[help] - show help");
            Console.WriteLine("[run] - run testing");
            Console.WriteLine("[shconf] - show current config");
            Console.WriteLine("[sht] - show available tests");
            Console.WriteLine("##########################################\n");
        }


        private void ShowInfo()
        {
            Console.WriteLine();
            Console.WriteLine("\t C U R R E N T  C O N F I G");
            Console.WriteLine("##########################################");
            PrintRunList();
            Console.WriteLine("Generator type: {0}", this._generator.ToString());
            Console.WriteLine("Sequences count: {0}", this._sequencesCount);
            Console.WriteLine("Sequence length: {0}", this._sequenceLength);
            Console.WriteLine("##########################################\n\n");
        }

        private void ChangeGenerator()
        {
            Console.WriteLine("Available generators:");
            Console.WriteLine("[0] {0}", nameof(GeneratorOnRandomPassAndSalt));
            Console.WriteLine("[1] {0}", nameof(GeneratorOnZeroPassAndSalt));
            Console.WriteLine("[2] {0} Warning! You will have to enter pass and salt" +
                " {1} times in this case.\n",
                nameof(GeneratorWherePassAndSaltFromConsole), this._sequencesCount);
            Console.Write("Enter new type of generator: ");

            switch (Console.ReadLine().Replace(" ", ""))
            {
                case "0":
                    _generator = new GeneratorOnRandomPassAndSalt();
                    break;
                case "1":
                    _generator = new GeneratorOnZeroPassAndSalt();
                    break;
                case "2":
                    _generator = new GeneratorWherePassAndSaltFromConsole();
                    break;
                default:
                    Console.WriteLine("Invalid input. Change canceled.\n");
                    return;
            }

            Console.WriteLine("Change successful.\n");
        }

        private void ChangeSeqCount()
        {
            Console.Write("Enter new count of sequences (min 1): ");

            if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
            {
                this._sequencesCount = count;
                Console.WriteLine("Change successful.\n");
            }
            else
            {
                Console.WriteLine("Invalid input. Change canceled.\n");
            }
        }

        private void ChangeSeqLength()
        {
            Console.Write("Enter new length of sequence (min 100): ");

            if (int.TryParse(Console.ReadLine(), out int length) && length >= 100)
            {
                this._sequencesCount = length;
                Console.WriteLine("Change successful.\n");
            }
            else
            {
                Console.WriteLine("Invalid input. Change canceled.\n");
            }
        }

        private void ShowAddTestMenu()
        {
            PrintTests();
            Console.Write("Enter the test number that you want to add to runlist: ");
            string number = Console.ReadLine();
            AddTest(number);
        }

        private void AddTest(string number)
        {
            ITest test;

            switch (number)
            {
                case ("1"):
                    test = new FrequencyTest();
                    break;

                case ("2"):
                    Console.WriteLine("Enter Block Frequency Test Parameters.");
                    Console.Write("Enter a block size (default 10): ");
                    string inputBF = Console.ReadLine();
                    if (string.IsNullOrEmpty(inputBF))
                    {
                        test = new BlockFrequencyTest();
                        break;
                    }
                    else if (int.TryParse(inputBF, out int blockSize))
                    {
                        test = new BlockFrequencyTest(blockSize);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Addition canceled.\n");
                        return;
                    }

                case ("3"):
                    Console.WriteLine("Enter Cumulative Sums Test Parameters.");
                    Console.Write("Forward mode? [yes/no] (default yes): ");
                    string inputCT = Console.ReadLine();
                    if (string.IsNullOrEmpty(inputCT) || inputCT == "yes")
                    {
                        test = new CusumTest();
                        break;
                    }
                    else if (inputCT == "no")
                    {
                        test = new CusumTest(forwardMode: false);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Addition canceled.\n");
                        return;
                    }

                case ("4"):
                    test = new RunsTest();
                    break;

                case ("5"):
                    Console.WriteLine("Enter Approximate Entropy Test Parameters.");
                    Console.Write("Enter a block size (default 3): ");
                    string inputAT = Console.ReadLine();
                    if (string.IsNullOrEmpty(inputAT))
                    {
                        test = new ApproximateEntropyTest();
                        break;
                    }
                    else if (int.TryParse(inputAT, out int blockSize))
                    {
                        test = new ApproximateEntropyTest(blockSize);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Addition canceled.\n");
                        return;
                    }

                case ("6"):
                    Console.WriteLine("Enter Serial Test Parameters.");
                    Console.Write("Enter a block size (default 3): ");
                    string inputST = Console.ReadLine();
                    if (string.IsNullOrEmpty(inputST))
                    {
                        test = new SerialTest();
                        break;
                    }
                    else if (int.TryParse(inputST, out int blockSize))
                    {
                        test = new SerialTest(blockSize);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Addition canceled.\n");
                        return;
                    }

                default:
                    Console.WriteLine("Invalid input. Addition canceled.\n");
                    return;
            }

            if (_runList.Contains(test))
                Console.WriteLine("Test has already been added earlier. Addition is canceled.\n");
            else
            {
                _runList.Add(test);
                Console.WriteLine("The test was successfully added to the runlist.\n");
            }

        }

        private void ShowDeleteTestMenu()
        {
            PrintRunList();
            Console.Write("Enter the test number that you want to delete from runlist: ");
            string number = Console.ReadLine();
            DeleteTest(number);
        }

        private void DeleteTest(string number)
        {
            if (int.TryParse(number, out int index) && index < _runList.Count)
            {
                _runList.RemoveAt(index);
                Console.WriteLine("The test was successfully deleted from the runlist.\n");
            }
            else
            {
                Console.WriteLine("Invalid input. Deletion canceled.\n");
            }
        }

        private void ClearRunList()
        {
            PrintRunList();
            Console.Write("Are you sure you want to clear the startup list? [yes/no]: ");
            if (Console.ReadLine().Replace(" ", "").ToLower() == "yes")
            {
                _runList.Clear();
                Console.WriteLine("Runlist was cleared successfully.\n");
            }
            else
            {
                Console.WriteLine("Runlist clearing canceled.\n");
            }
        }

        private void PrintRunList()
        {
            Console.WriteLine();
            Console.WriteLine("\t SELECTED TESTS");
            Console.WriteLine("_______________________________________ \n");

            for (int i = 0; i < _runList.Count; i++)
            {
                Console.WriteLine("[{0}] {1}", i, _runList[i].ToString());
            }

            Console.WriteLine("_______________________________________ \n");
        }

        private static void PrintTests()
        {
            Console.WriteLine();
            Console.WriteLine("\t S T A T I S T I C A L   T E S T S");
            Console.WriteLine("\t ____________________________________ \n");
            Console.WriteLine("\t [1] Frequency \n");
            Console.WriteLine("\t [2] Block Frequency \n");
            Console.WriteLine("\t [3] Cumulative Sums \n");
            Console.WriteLine("\t [4] Runs \n");
            Console.WriteLine("\t [5] Approximate Entropy \n");
            Console.WriteLine("\t [6] Serial \n");
            Console.WriteLine("\t ____________________________________ \n");
        }
    }
}
