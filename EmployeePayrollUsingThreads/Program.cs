using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollUsingThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Employee Payroll System Using Threads!");

            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParellelTaks      // #region is used like a comment to collapse or minimize the code
            //Perform three tasks in parallel on the source array
            Parallel.Invoke(                                         // using System.Threading.Tasks
                () =>
                {
                    Console.WriteLine("Begin first task...");
                    GetLongestWord(words);
                },  // Close first Action

                () =>
                {
                    Console.WriteLine("Begin second task...");
                    GetMostCommonWords(words);
                },  // Close first Action

                () =>
                {
                    Console.WriteLine("Begin third task...");
                    GetCountForWord(words, "sleep");
                }  //Close third Action
            ); //Close parallel.invoke

            Console.WriteLine("Returned from Parallel.Invoke");
            #endregion

            Console.WriteLine("Press any key to exit");
        }

        private static string GetLongestWord(string[] words)
        {
            var longestWord = (from w in words
                               orderby w.Length descending
                               select w).First();
            Console.WriteLine($"Task 1 -- The longest word is {longestWord}.");
            return longestWord;
        }

        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words                        // using System.Linq namespace
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;
            var commonWords = frequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();                        // using System.text namespace
            sb.AppendLine("Task 2 -- The most common words are:");
            foreach (var v in commonWords)
            {
                sb.AppendLine(" " + v);
            }
            Console.WriteLine(sb.ToString());
        }

        private static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words
                           where word.ToUpper().Contains(term.ToUpper())
                           select word;
            Console.WriteLine($@"Task 3 -- The word ""{term}"" occurs {findWord.Count()} times.");
        }

        static string[] CreateWordArray(string uri)
        {
            Console.WriteLine($"Retrieving from {uri}");

            //Download a web page the easy way.
            string s = new WebClient().DownloadString(uri);  // using System.Net

            return s.Split(
                new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
    
}
