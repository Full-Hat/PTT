using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{ 
    interface ICommands
    {
        int JustDoIt(string command);
        void ParseOperation(string opString);
    }

    public abstract class Operation
    {
        public Operation(char symbol, List<int> operands)
        {
            operationSymbol = symbol;
            this.operands = operands;
        }

        public Operation() { operands = new List<int>(); }

        public abstract void ParseOperation(string opString);

        public char operationSymbol { get; set; }
        protected List<int> operands { get; set; } 

        public static SimpleOperation.OperationType getTypeOfOperation(string operation)
        {
            foreach(char symb in operation)
            {
                switch(symb)
                {
                    case '+':
                        return SimpleOperation.OperationType.PLUS;
                    case '-':
                        return SimpleOperation.OperationType.MINUS;
                    case '*':
                        return SimpleOperation.OperationType.MULT;
                    case '/':
                        return SimpleOperation.OperationType.DIV;
                }
            }

            return SimpleOperation.OperationType.UNDEFINED;
        }
        
        abstract public int calculate();
    }

    public abstract class BynaryOperation: Operation
    {
        public BynaryOperation(char symbol, int a, int b) : base(symbol, new List<int> { a, b })
        {
        }

        public BynaryOperation() { }

        public override void ParseOperation(string opString)
        {
            string number = "";
            int i = 0;
            while (opString[i] != ' ')
            {
                number += opString[i++];
            }
            opString = opString.Remove(opString.IndexOf(number), number.Length);
            opString = Utils.TrimFirst(opString);

            int operand = 0;
            int.TryParse(number, out operand);
            operands.Add(operand);

            operationSymbol = opString[0];
            opString = opString.Remove(0, 1);
            opString = Utils.TrimFirst(opString);

            number = "";
            i = 0;
            while (i != opString.Length && opString[i] != ' ')
            {
                number += opString[i++];
            }

            int.TryParse(number, out operand);
            operands.Add(operand);
        }
    }

    public class PlusOperation : BynaryOperation
    {
        public PlusOperation(int a, int b) : base('+', a, b)
        {
        }

        public PlusOperation()
        {
        }

        public override int calculate()
        {
            return operands[0] + operands[1];
        }
    }

    public class MinusOperation : BynaryOperation
    {
        public MinusOperation(int a, int b) : base('+', a, b)
        {
        }

        public MinusOperation()
        {
        }

        public override int calculate()
        {
            return operands[0] - operands[1];
        }
    }

    public class MultOperation : BynaryOperation
    {
        public MultOperation(int a, int b) : base('*', a, b)
        {
        }

        public MultOperation()
        {
        }

        public override int calculate()
        {
            return operands[0] * operands[1];
        }
    }

    public class DivOperation : BynaryOperation
    {
        public DivOperation(int a, int b) : base('/', a, b)
        {
        }

        public DivOperation()
        {
        }

        public override int calculate()
        {
            return operands[0] / operands[1];
        }
    }

    public class Return : ICommands
    {
        private int returnValue;

        public void ParseOperation(string opString)
        {
            opString = opString.Remove(0, opString.IndexOf("return"));
            string returnString = Utils.GetWord(opString, 0);
            if(returnString != "return")
            {
                throw new Exception("Invalid return instruction.. PARSE ERROR int RETURN class.. ");
            } 
            opString = opString.Remove(opString.IndexOf(returnString), returnString.Length);
            opString = Utils.TrimFirst(opString);

            string operation = string.Empty;
            int i = 0;
            bool isSimpleDigit = true;
            while(opString[i] != ';')
            {
                if(!char.IsDigit(opString[i])) // for future: add checker if spaces are after params 
                {
                    isSimpleDigit = false;
                }

                operation += opString[i++];
            }

            if(isSimpleDigit)
            {
                returnValue = int.Parse(operation);
            }
            else // it is operation 
            {
                if (Calculator.IsValid(operation))
                {
                    returnValue = (int)Calculator.Calculate(Calculator.ToPolishNotation(operation));
                    return;
                }

                // we must understand what type we use || old version 
                SimpleOperation.OperationType opType = Operation.getTypeOfOperation(operation);
                Operation op = null;
                switch(opType)
                {
                    case SimpleOperation.OperationType.PLUS:
                        op = new PlusOperation();
                        break;
                    case SimpleOperation.OperationType.MINUS:
                        op = new MinusOperation();
                        break;
                    case SimpleOperation.OperationType.MULT:
                        op = new MultOperation();
                        break;
                    case SimpleOperation.OperationType.DIV:
                        op = new DivOperation();
                        break;
                    case SimpleOperation.OperationType.UNDEFINED:
                        throw new Exception("Undefined type of operation, error in syntaxis or program don't know this type :D");
                }
                op.ParseOperation(operation);
                returnValue = op.calculate();
            }
        }

        public int JustDoIt(string command)
        {
            ParseOperation(command);

            return returnValue;
        }
    }
}
