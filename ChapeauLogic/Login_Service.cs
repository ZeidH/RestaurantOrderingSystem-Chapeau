using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ChapeauDAL;
using ChapeauModel;
using System.Security.Cryptography;

namespace ChapeauLogic
{
    public class Login_Service
    {
        private readonly string password;
        private readonly string username;
        private Login_DAO login_DAO = new Login_DAO();

        public Login_Service(string password, string username)
        {
            this.username = username;
            this.password = GetStringSha256Hash(password);
        }

        internal static string GetStringSha256Hash(string pw)
        {
            if (String.IsNullOrEmpty(pw))
                return String.Empty;

            using (SHA256Managed sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(pw);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public Employee Validation()
        {
            try
            {
                return login_DAO.ValidateCredentials(password, username);
            }
            catch (SqlException)
            {
                throw new Exception("I can't connect to Chapeau!");
            }
            catch(NullReferenceException)
            {
                throw new Exception("Wrong Password or Username");
            }
        }
    }
}
