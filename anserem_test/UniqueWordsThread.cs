using System;
using System.IO;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace UniqueWordsThread
{
    public class UniqueThread
    {
        private ConcurrentDictionary<string, int> uniqueDictionary;
        private object locker = new();
        private string? fileContent;
        private int processedFiles;
        public int ProcessedFiles {get {return processedFiles;} set{processedFiles = value;}}
        /* for thread attempt
        private int ThreadCount;
        private int ThreadNum;
        
        public UniqueThread()
        {
            UniqueDictionary = new ConcurrentDictionary<string, int>();
            ThreadCount = 0;
            ThreadNum = 0;
        }
        */
        public UniqueThread()
        {
            uniqueDictionary = new ConcurrentDictionary<string, int>();
            ProcessedFiles = 0;
        }
        public ConcurrentDictionary<string, int> GetUniqueDictionary()
        {
            return uniqueDictionary;
        }
        public void AddWord(string word)
        {
            uniqueDictionary.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
        }
        /* for thread attempt

        public int GetThreadCount()
        {
            return ThreadCount;
        }
        public void SetThreadCount(int tc)
        {
            ThreadCount = tc;
        }
        public int GetThreadNum()
        {
            return ThreadNum;
        }
        public void SetThreadNum(int tn)
        {
            ThreadNum = tn;
        }
        public void FillDicitonaryThread(object? obj)
        {
            if(obj is string path)
            {
                    
                if(!(Path.GetExtension(path) == ".txt"))
                    Console.WriteLine("{0}\nFile extension mismatch\nReading only .txt files", path);
                else 
                {
                    if(new FileInfo(path).Length == 0)
                        Console.WriteLine("{0} is an empty file", path);
                    else
                    {
                        lock (locker)
                        {
                            char[] separator = {' ', '\n', '\r', ',', '.', '/', '?', '!', '<', '>', '\"', ':', ';', '-', '^', '_', '(', ')', '[', ']', '{', '}'};
                            fileContent = File.ReadAllText(path);
                            
                            //var sb = new StringBuilder();
                            //foreach (char c in fileContent)
                            //{
                            //    if (!char.IsPunctuation(c))
                            //    sb.Append(Char.ToLower(c));
                            //    else sb.Append(' ');
                            //}
                            //fileContent = sb.ToString();
                            
                            var sb = new StringBuilder();
                            foreach (char c in fileContent)
                            {
                                sb.Append(Char.ToLower(c));
                            }
                            fileContent = sb.ToString();
                            string[] words = fileContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        
                            foreach(string word in words)
                            {
                                AddWord(word);
                            }
                            //Console.WriteLine("\n{0}\n", Thread.CurrentThread.Name);
                            if(GetThreadNum() == GetThreadCount()-1)
                                Console.WriteLine(string.Join("\n", GetUniqueDictionary().Select(x => $"{x.Key} {x.Value}").ToArray()));
                        }
                    }
                }
            }
            SetThreadNum(Interlocked.Increment(ref ThreadNum));
        }
        */
        public void FillDicitonaryTask(string path)
        {
            if(!(Path.GetExtension(path) == ".txt"))
                Console.WriteLine("{0}\nFile extension mismatch\nReading only .txt files", path);
            else 
            {
                if(new FileInfo(path).Length == 0)
                {
                    Console.WriteLine("{0} is an empty file", path);
                }
                else
                {
                    lock (locker)
                    {
                        char[] separator = {' ', '\n', '\r', ',', '.', '/', '?', '!', '<', '>', '\"', ':', ';', '-', '^', '_', '(', ')', '[', ']', '{', '}'};
                        fileContent = File.ReadAllText(path);
                            /*
                            var sb = new StringBuilder();
                            foreach (char c in fileContent)
                            {
                                if (!char.IsPunctuation(c))
                                sb.Append(Char.ToLower(c));
                                else sb.Append(' ');
                            }
                            fileContent = sb.ToString();
                            */
                        var sb = new StringBuilder();
                        foreach (char c in fileContent)
                        {
                            sb.Append(Char.ToLower(c));
                        }
                        fileContent = sb.ToString();
                        string[] words = fileContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        
                        foreach(string word in words)
                        {
                            AddWord(word);
                        }
                    }
                }
                ProcessedFiles++;
            }
        }
    }
}