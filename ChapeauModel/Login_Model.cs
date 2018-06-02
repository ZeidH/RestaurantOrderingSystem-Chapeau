using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapeauModel
{
    public class Login
    {
        public string username { get; set; }
        public string pwdhash { get; set; }
    }

    public class Employee
    {
        public int id;
        public string name;
        public string surname;
        public Occupation occupation;
        public bool status;

        public Employee()
        {
            //emplty constructor 
        }

        public Employee(int id, string name, string surname, Occupation occupation, bool status)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.occupation = occupation;
            this.status = status;
        }

    }

    public class Employer : Employee
    {

    }
}
