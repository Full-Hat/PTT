using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    class Program
    {
        public static List<StringBuilder> SplitString(StringBuilder baseString) 
        {
            List<StringBuilder> words = new List<StringBuilder>(0);
            StringBuilder currWord = new StringBuilder("");
            bool isInWordCurr = false;
            bool isInWordPriv = false;
            baseString.Append(' ');

            for(int i = 0; i < baseString.Length; ++i)
            {
                isInWordCurr = !Char.IsWhiteSpace(baseString[i]);

                if (isInWordCurr) //  word started or continue
                {
                    currWord.Append(baseString[i]);
                }
                else if(isInWordPriv && !isInWordCurr) // word end 
                {
                    words.Add(currWord);
                    currWord = new StringBuilder("");
                }
                isInWordPriv = isInWordCurr; 
            }

            return words;
        }
        public static StringBuilder ReverseWords(StringBuilder baseString, List<StringBuilder> words)
        {
            StringBuilder reverseString = new StringBuilder("");
            bool isWordAdded = false;
            int wordCounter = words.Count;

            for (int i = 0; i < baseString.Length; ++i)
            {
                if(Char.IsWhiteSpace(baseString[i]))
                {
                    reverseString.Append(' ');
                    isWordAdded = false;
                }
                else if(!isWordAdded)
                {
                    reverseString.Append(words[--wordCounter]);
                    isWordAdded = true;
                }
            }

            return reverseString;
        }
        public static int Main(string[] args)
        {
            Console.Write("Enter the text: ");
            StringBuilder str;

            while((str = new StringBuilder(Console.ReadLine())).Length == 0)
            {
                Console.Write("Input string is empty, try again: ");
            }

            Console.WriteLine(ReverseWords(str, SplitString(str)));

            return 0;
        }
    }
}