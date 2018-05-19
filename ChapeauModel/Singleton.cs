using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapeauModel
{
    public class Singleton
    {
        private static Singleton firstInstance = null;

        private Singleton() { }

        public static Singleton getInstance() {
            if(firstInstance == null) {
                firstInstance = new Singleton();

            }

            return firstInstance;
        }
    }
}
