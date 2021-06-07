using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Analyzer
{
    public interface IGlobalElement
    {
        void PrintCode();
        void Print();
        void Print(string message);
    };

    public class Withe
    {
        private List<IGlobalElement> elements = new List<IGlobalElement>();

        public void Add(Functions functions)
        {
            foreach (Function funct in functions.FunctionsAn)
            {
                elements.Add(funct);
            }
        }

        public void PrintCode()
        {
            foreach (IGlobalElement funct in elements)
            {
                funct.PrintCode();
                Console.WriteLine();
            }
        }

        public void Print()
        {
            foreach (IGlobalElement funct in elements)
            {
                funct.Print("Function");
                Console.WriteLine();
            }
        }
    }

    public class Program
    {
        public static Functions findAllFunctions()
        { 
            Functions functions = new Functions();

            File.Open("code.txt", FileMode.OpenOrCreate).Dispose();
            string code;
            string prRow = string.Empty;
            string currRow;
            string[] data = File.ReadLines("code.txt").ToArray();
            for(int i = 0; i < data.Length; ++i)
            {
                currRow = data[i];

                code = prRow + currRow;

                Function funct = Function.TryParseFunctionHead(code);
                if (funct.IsReal())
                {
                    while (currRow.IndexOf("}") == -1)
                    {
                        currRow = data[++i]; 
                        code += currRow;
                    }
                    funct = Function.TryParseFunctionHead(code);
                    functions.FunctionsAn.Add(funct);
                    Console.WriteLine("We found function! :D\n");
                }

                prRow = currRow;
            }

            return functions;
        }  

        static void Main(string[] args)
        {
            ErrorHandler handler = new ErrorHandler();
            handler.Notify += (string mess) => { Console.WriteLine($"\nAhtung!!!\n{mess}"); };

            Withe withe = new Withe();


            Functions functions = findAllFunctions();
            ICommands returnCommand = new Return(); 
            if(functions.FunctionsAn == null)
            {
                Console.WriteLine("There is no functions..");
            }
            else
            {
                withe.Add(functions);
                withe.Print();

                foreach(Function funct in functions.FunctionsAn)
                {
                    if(funct.ReturnType.Name != "void")
                    {
                        try
                        {
                            int functionResult = returnCommand.JustDoIt(funct.Code);
                            Console.WriteLine($"Function return part : {functionResult}");
                        }
                        catch(Exception ex)
                        {
                            handler.FixError(ex.Message);
                            Console.WriteLine("Funct isn't have only return operation, so it is very very hard to parse :D\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Oops, this function return... Nothing, +- nothing ");
                    }
                }
            }

            Console.WriteLine("\n\n");
            handler.PrintErrorReport();
        }
    }
};
