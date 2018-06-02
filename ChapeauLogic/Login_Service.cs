using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;
using System.Data;

namespace ChapeauLogic
{
    public class Login_Service
    {
        public void GetCredentials(Login login)
        {
            Login_DAO lgnDao = new Login_DAO();
            lgnDao.checkCredentials(login); //pass the parameters to the database layer
        }
    }
}
