using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapeauModel
{
    public class Employee
    {
        public int ID { get; set; }
        private string name;
        public string Name
        {
            get
            {
                string[] splitName = name.Split(' ');
                char[] charArray = splitName[1].ToCharArray();
                return $"{splitName[0]} {charArray[0]}.";
            }
            set
            {
                name = value;
            }
        }
        public Occupation? Occupation { get; set; }

    }
}
