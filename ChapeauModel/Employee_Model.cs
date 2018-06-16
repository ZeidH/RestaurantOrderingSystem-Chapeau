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
        public string Name { get; set; }
        public Occupation? Occupation { get; set; }
    }
}
