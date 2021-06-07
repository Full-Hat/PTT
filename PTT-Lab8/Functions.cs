using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public struct ParamAn
    {
        public TypeAn type;
        public string name;
    };

    public class ParamsAn
    {
        public ParamsAn()
        {
            paramsAn = new List<ParamAn>();
        }
        public ParamsAn(string paramsString)
        {
            paramsAn = new List<ParamAn>();
            string type;
            while (true)
            {
                type = Utils.GetWord(paramsString, 0);
                if (!TypesAn.IsType(type))
                {
                    break;
                }

                paramsString = paramsString.Remove(paramsString.IndexOf(type), type.Length);
                paramsString = Utils.TrimFirst(paramsString);

                string name = string.Empty;
                foreach (char symbol in paramsString)
                {
                    if (symbol == ' ' || symbol == ',')
                    {
                        break;
                    }
                    name += symbol;
                }
                paramsString = paramsString.Remove(paramsString.IndexOf(name), name.Length);
                paramsString = Utils.TrimFirst(paramsString);
                if (paramsString.Length != 0 && paramsString[0] == ',')
                {
                    paramsString = paramsString.Remove(0, 1);
                    paramsString = Utils.TrimFirst(paramsString);
                }
                paramsAn.Add(new ParamAn { type = new TypeAn(type), name = name });
            }
        }

        public List<ParamAn> paramsAn;
    };

    public class Function: IGlobalElement
    {
        public Function(TypeAn returnType, string name, ParamsAn paramsAn, string code)
        {
            m_returnType = returnType;
            m_name = name;
            m_params = paramsAn;
            m_code = code;
        }
        public Function()
        {
            m_returnType = new TypeAn();
            m_name = new string("");
            m_params = new ParamsAn();
            m_code = new string("");
        }

        private TypeAn m_returnType;
        private string m_name;
        private ParamsAn m_params;

        private string m_code;

        public string Code { get { return m_code; } set { m_code = value; } } 
        public TypeAn ReturnType {  get { return m_returnType; } set { m_returnType = value; } }

        public bool IsReal()
        {
            return m_name != "";
        }

        public void Print()
        {
            Console.WriteLine($"Return type: {m_returnType.Name}");
            Console.WriteLine($"Funct name: {m_name}");
            Console.WriteLine("Params:\n{");
            foreach(ParamAn param in m_params.paramsAn)
            {
                Console.WriteLine($"\ttype: {param.type.Name}, name: {param.name}");
            }
            Console.WriteLine("}");
            Console.WriteLine($"Code: \n+{m_code}+");
        }

        public void Print(string message)
        {
            Console.WriteLine($"~~~~~~~~~~~~~~{message}~~~~~~~~~~~~~~");
            Print();
        }

        public void PrintCode()
        {
            Console.WriteLine(m_code);
        }

        public static Function TryParseFunctionHead(string str)
        {

            str = Utils.TrimFirst(str);
            string type = Utils.GetWord(str, 0);
            if (type == "")
            {
                return new Function();
            }

            // try to find return type 
            if (!TypesAn.IsType(type))
            {
                return new Function();
            }
            TypeAn _type = new TypeAn(type);

            int typeInd = str.IndexOf(type);
            str = str.Remove(typeInd, typeInd + type.Length);
            str = Utils.TrimFirst(str);


            // try to find name 
            string functName = "";
            bool isNameStarted = false;
            foreach (char el in str)
            {
                if (el != ' ' && el != '(')
                {
                    isNameStarted = true;
                }

                if ((el == ' ' || el == '(') && isNameStarted)
                {
                    break;
                }
                functName += el;
            }
            int nameInd = str.IndexOf(functName);
            str = str.Remove(nameInd, nameInd + functName.Length);
            str = Utils.TrimFirst(str);

            // try to find params 
            string paramsString = "";
            bool isParamsStarted = false;
            foreach (char symbol in str)
            {
                if (symbol == ')')
                {
                    break;
                }

                if (symbol == '(')
                {
                    isParamsStarted = true;
                    continue;
                }

                if (isParamsStarted)
                {
                    paramsString += symbol;
                }
            }
            if (!isParamsStarted)
            {
                return new Function();
            }
            int paramsInd = str.IndexOf(paramsString);
            str = str.Remove(paramsInd, paramsString.Length);
            str = Utils.TrimFirst(str);

            // try to find code  
            string codeString = "";
            bool isCodeStarted = false;
            foreach (char symbol in str)
            {
                if (symbol == '}')
                {
                    break;
                }

                if (symbol == '{')
                {
                    isCodeStarted = true;
                    continue;
                }

                if (isCodeStarted)
                {
                    codeString += symbol;
                }
            }
            if (!isCodeStarted)
            {
                return new Function();
            }
            codeString = Utils.TrimFirst(codeString);

            return new Function(_type, functName, new ParamsAn(paramsString), codeString);
        }
    }

    public class Functions
    {
        public Functions()
        {
            FunctionsAn = new List<Function>();
        }

        public List<Function> FunctionsAn { get; set; }

        public Function this[int index] => FunctionsAn[index];
    };
}
