using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    class ErrorHandler
    {
        private List<string> errorsInfo;

        public ErrorHandler()
        { 
            ErrorCounter = 0;
        }

        private int ErrorCounter { get; set; } 

        public delegate void ErrorHandlerDel(string message);
        
        public event ErrorHandlerDel Notify;

        public void PrintErrorReport()
        {
            Console.WriteLine("~~~~~~~~~~~~Error report -- all that you don't want to see~~~~~~~~~~~~");
            Console.WriteLine($"Program found {ErrorCounter} errors, {(ErrorCounter == 0 ? "good result" : "try reboot pc")}");

            if (errorsInfo != null)
            {
                foreach (string str in errorsInfo)
                {
                    Console.WriteLine(str);
                }
            }
        }

        public void FixError(string errorMessage)
        {
            ++ErrorCounter;

            if(errorsInfo == null)
            {
                errorsInfo = new List<string>();
            }
            errorsInfo.Add(errorMessage);

            Notify?.Invoke(errorMessage);
        }
    }
}
