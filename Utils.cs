using System.Collections.Generic;

namespace Analyzer
{
    class Utils
    {
        public static List<string> Split(string data)
        {
            bool isPrWord = false;

            List<string> words = new List<string>();

            foreach (char symbol in data)
            {
                if (symbol != ' ' && !isPrWord)
                {
                    words.Add(string.Empty);
                }

                if (symbol != ' ')
                {
                    words[words.Count - 1] += symbol;
                }

                isPrWord = symbol != ' ';
            }

            return words;
        }

        public static string GetWord(string data, int index)
        {
            bool isPrWord = false;

            string word = string.Empty;
            int wordInd = 0;

            foreach (char symbol in data)
            {
                if (symbol != ' ' && !isPrWord && word != "" && wordInd == index++)
                {
                    return word;
                }

                if (symbol != ' ')
                {
                    word += symbol;
                }

                isPrWord = symbol != ' ';
            }

            return "";
        }

        public static string TrimFirst(string str)
        {
            string newString = string.Empty;

            bool isStarted = false;
            foreach (char el in str)
            {
                if (el != ' ' && el != '\n' && el != '\t')
                {
                    isStarted = true;
                }

                if (isStarted == true)
                {
                    newString += el;
                }
            }

            return newString;
        }
    }
}
