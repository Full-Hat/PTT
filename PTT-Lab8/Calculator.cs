using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public abstract class SimpleOperation : IComparable<SimpleOperation>
    {
        public SimpleOperation(OperationType type)
        {
            OpSymbol = type;
        }
        public SimpleOperation()
        {
        }

        public abstract int CompareTo(SimpleOperation other);

        public enum OperationType
        {
            PLUS,
            MINUS,
            MULT,
            DIV,
            UNDEFINED,
        }

        public OperationType OpSymbol { get; set; }

        public static bool IsOperator(char ch)
        {
            return ch == '/' || ch == '*' || ch == '-' || ch == '+' || ch == '^';
        }
    }

    public class Minus: SimpleOperation
    { 
        public Minus() : base(OperationType.MINUS)
        {
        }

        public override int CompareTo(SimpleOperation other)
        {
            return (other.OpSymbol == OperationType.MINUS || other.OpSymbol == OperationType.PLUS) == true ? 0 : -1;
        }
    }

    public class Plus : SimpleOperation
    {
        public Plus() : base(OperationType.PLUS)
        {
        }

        public override int CompareTo(SimpleOperation other)
        {
            return (other.OpSymbol == OperationType.MINUS || other.OpSymbol == OperationType.PLUS) == true ? 0 : -1;
        }
    }

    public class Mult : SimpleOperation
    {
        public Mult() : base(OperationType.MULT)
        {
        }

        public override int CompareTo(SimpleOperation other)
        {
            if(other.OpSymbol == OperationType.MULT || other.OpSymbol == OperationType.DIV)
            {
                return 0;
            }

            if(other.OpSymbol == OperationType.MINUS || other.OpSymbol == OperationType.PLUS)
            {
                return 1;
            }

            return -1;
        }
    }

    public class Div : SimpleOperation
    {
        public Div() : base(OperationType.MULT)
        {
        }

        public override int CompareTo(SimpleOperation other)
        {
            if (other.OpSymbol == OperationType.MULT || other.OpSymbol == OperationType.DIV)
            {
                return 0;
            }

            if (other.OpSymbol == OperationType.MINUS || other.OpSymbol == OperationType.PLUS)
            {
                return 1;
            }

            return -1;
        }
    }

    public class PowO : SimpleOperation
    {
        public PowO() : base(OperationType.MULT)
        {
        }

        public override int CompareTo(SimpleOperation other)
        {
            if (other.OpSymbol == OperationType.MULT || other.OpSymbol == OperationType.DIV)
            {
                return 1;
            }

            if (other.OpSymbol == OperationType.MINUS || other.OpSymbol == OperationType.PLUS)
            {
                return 1;
            }

            return 0;
        }
    }

    static class Calculator
    {

        private static void AddSignedOperands(string expr)
        {
            for (int i = 0; i < expr.Length; i++)
            {
                char current = expr[i];
                if (current == '+' || current == '-')
                    if ((i == 0 || expr[i - 1] == '(') && (char.IsDigit(expr[i + 1]) || expr[i + 1] == '('))
                        expr = expr.Insert(i, "0");
            }
        }

        public static bool IsValid(string expr)
        {
            if (expr.Length < 1)
            {
                return false;
            }
            if (SimpleOperation.IsOperator(expr[expr.Length - 1]) || SimpleOperation.IsOperator(expr[0]) || expr[0] == ')' || expr[expr.Length - 1] == '(')
            {
                return false;
            }
            for (int i = 0; i < expr.Length; i++)
            {
                if (i != expr.Length - 1)
                {
                    if (expr[i] == ')' && expr[i + 1] == '(')
                    {
                        return false;
                    }
                    if (expr[i] == '(' && expr[i + 1] == ')')
                    {
                        return false;
                    }
                }
                if (!(char.IsDigit(expr[i]) || expr[i] == '.' || expr[i] == '(' || expr[i] == ')' || SimpleOperation.IsOperator(expr[i])))
                {
                    return false;
                }
                if (((expr[i] == '.' || expr[i] == ')') && i == 0) || ((expr[i] == '.' || expr[i] == '(') && i == expr.Length - 1))
                {
                    return false;
                }
                if ((expr[i] == '.' && !char.IsDigit(expr[i - 1])) || (expr[i] == '.' && !char.IsDigit(expr[i + 1])))
                {
                    return false;
                }
                if (SimpleOperation.IsOperator(expr[i]) && (!(char.IsDigit(expr[i - 1]) || expr[i - 1] == ')') || !(char.IsDigit(expr[i + 1]) || expr[i + 1] == '(')))
                {
                    return false;
                }
            }
            int bracketsCounter = 0;
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == '(')
                {
                    bracketsCounter++;
                }
                else if (expr[i] == ')')
                {
                    bracketsCounter--;
                }
            }
            if (bracketsCounter != 0)
            {
                return false;
            }

            return true;
        }

        public static List<string> SplitExpression(string expr)
        {
            List<string> terms = new();
            string currentTerm = string.Empty;
            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (char.IsDigit(c) || c == '.') currentTerm += c;
                else if (SimpleOperation.IsOperator(c) || c == ')' || c == '(')
                {
                    terms.Add(currentTerm);
                    currentTerm = "";
                    currentTerm += c;
                    terms.Add(currentTerm);
                    currentTerm = "";
                }
            }
            terms.Add(currentTerm);
            List<string> termsFiltered = new();
            foreach (var i in terms)
            {
                if (i != "")
                {
                    termsFiltered.Add(i);
                }
            }
            return termsFiltered;
        }

        private static int GetPriority(char o)
        {
            if (o == '+' || o == '-') return 0;
            if (o == '*' || o == '/') return 1;
            if (o == '^') return 2;
            return 0;
        } 
        
        public static List<string> ToPolishNotation(string expr)
        {
            List<string> terms = new();
            List<string> infixNotationTerms = SplitExpression(expr);
            Stack<string> operationsStack = new();
            foreach (var i in infixNotationTerms)
            {
                string current = i;
                if (char.IsDigit(current[0]))
                    terms.Add(current);
                else if (current[0] == '(')
                    operationsStack.Push(current);
                else if (current[0] == ')')
                {
                    string currentOp = operationsStack.Peek();
                    while (currentOp[0] != '(')
                    {
                        terms.Add(currentOp);
                        operationsStack.Pop();
                        currentOp = operationsStack.Peek();
                    }
                    operationsStack.Pop();
                }
                else if (SimpleOperation.IsOperator(current[0]))
                {
                    if (operationsStack.Count == 0) operationsStack.Push(current);
                    else
                    {
                        string lastOperand = operationsStack.Peek();
                        if (lastOperand[0] == '(') operationsStack.Push(current);
                        else
                        {
                            SimpleOperation simpOp = null;
                            SimpleOperation currOp = null;
                            if (lastOperand[0] == '+') { simpOp = new Plus(); }
                            else if (lastOperand[0] == '-') { simpOp = new Minus(); }
                            else if (lastOperand[0] == '*') { simpOp = new Mult(); }
                            else if (lastOperand[0] == '/') { simpOp = new Div(); }
                            else if (lastOperand[0] == '^') { simpOp = new PowO(); }
                            if (current[0] == '+') { currOp = new Plus(); }
                            else if (current[0] == '-') { currOp = new Minus(); }
                            else  if (current[0] == '*') { currOp = new Mult(); }
                            else if (current[0] == '/') { currOp = new Div(); }
                            else if (current[0] == '^') { simpOp = new PowO(); }
                            if (simpOp.CompareTo(currOp) == 0 || simpOp.CompareTo(currOp) > 0)
                            {
                                operationsStack.Pop();
                                operationsStack.Push(current);
                                terms.Add(lastOperand);
                            }
                            else if (simpOp.CompareTo(currOp) < 0) operationsStack.Push(current);
                        }
                    }
                }
            }
            while (operationsStack.Count > 0)
            {
                string lastOperand = operationsStack.Peek();
                terms.Add(lastOperand);
                operationsStack.Pop();
            }
            return terms;
        }

        public static double Calculate(List<string> polishNotationTerms)
        {
            Stack<string> countingStack = new();
            foreach (var i in polishNotationTerms)
            {
                if (SimpleOperation.IsOperator(i[0]))
                {
                    double a = double.Parse(countingStack.Peek());
                    countingStack.Pop();
                    double b = double.Parse(countingStack.Peek());
                    countingStack.Pop();
                    if (i[0] == '+') countingStack.Push((a + b).ToString());
                    else if ((i)[0] == '-') countingStack.Push((b - a).ToString());
                    else if ((i)[0] == '*') countingStack.Push((a * b).ToString());
                    else if ((i)[0] == '/') countingStack.Push((b / a).ToString());
                    else if ((i)[0] == '^') countingStack.Push(Math.Pow(b, a).ToString());
                }
                else countingStack.Push(i);
            }
            return double.Parse(countingStack.Peek());
        }

    }
}
