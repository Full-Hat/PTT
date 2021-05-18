using System.Collections.Generic;

namespace Analyzer
{
    public class TypeAn
    {
        public TypeAn(string _name = "")
        {
            Name = _name;
        }

        private string name;
        public string Name 
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    };

    class TypesAn
    {
        public static List<TypeAn> Types { get; set; } = new List<TypeAn>();

        static TypesAn()
        {
            Types.Add(new TypeAn("int"));
            Types.Add(new TypeAn("double"));
            Types.Add(new TypeAn("void"));
        }
        public static bool IsType(string str)
        {
            foreach (TypeAn type in TypesAn.Types)
            {
                if (str == type.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public TypeAn this[int index] => Types[index];
    };
}
