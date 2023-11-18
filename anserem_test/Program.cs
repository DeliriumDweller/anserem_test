using System;
using System.IO;
using System.Text;
using System.Collections.Concurrent;
using System.Diagnostics;

using UniqueWordsThread;

namespace anserem_test
{
    class Program
    {
        static void Main(string[] args)
        {
            UniqueThread Dictionary = new UniqueThread();
            try
            {
                if(Directory.EnumerateFileSystemEntries(Environment.CurrentDirectory + "//files//").Any())
                {
                    string[] Documents = Directory.GetFiles(Environment.CurrentDirectory + "//files//");
                    if(Documents.Length == 0) 
                        Console.WriteLine("Directory {0} contains no files", Environment.CurrentDirectory + "//files//");
                    else
                    {
                        /* 
                        foreach(string s in Documents)
                        {  
                            Console.WriteLine("Found: " + s);
                        }
                        */

                        /* thread attempt
                        Dictionary.SetThreadCount(Documents.Length);
                        foreach(string f in Documents)
                        {
                            Thread uThread = new Thread(Dictionary.FillDicitonaryThread);
                            //pThread.Name = f;
                            uThread.Start(f);
                        }
                        */

                        List<string> doclist = new List<string>();
                        foreach(string f in Documents){
                            doclist.Add(f);
                        }

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        
                        ParallelLoopResult result1 = Parallel.ForEach<string>(doclist, Dictionary.FillDicitonaryTask);
                        Console.WriteLine(string.Join("\n", Dictionary.GetUniqueDictionary().Select(x => $"{x.Key} {x.Value}").ToArray()));
                        
                        stopwatch.Stop();
                        TimeSpan t = stopwatch.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
                        Console.WriteLine("\nProcessed {0} .txt files in {1}", Dictionary.ProcessedFiles, elapsedTime);
                    }
                }
                else Console.WriteLine("Directory is empty");
            }
            catch(Exception e)
            {
                Console.WriteLine($"\nDirectory not found: {e.Message}");
            }
        }
    }
}
