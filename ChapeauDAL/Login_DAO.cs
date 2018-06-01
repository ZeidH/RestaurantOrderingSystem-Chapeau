using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ChapeauModel;

namespace ChapeauDAL
{
    public class Login_DAO : Base
    {
        public void checkCredentials(Login login)
        {
            string query = "SELECT user_name, pwdHash FROM [LOGIN] WHERE user_name = @username, pwdHash = @password";

            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@username", SqlDbType.NChar)
            {
                Value = login.username
            };

            sqlParameters[1] = new SqlParameter("@password", SqlDbType.NChar)
            {
               Value = login.pwdhash
            };

            ExecuteSelectQuery(query, sqlParameters);

            getId(login);
        }

        public Employee getId(Login login)
        {
            string query = "SELECT emp_id FROM [LOGIN] WHERE user_name = @username, pwdHash = @password";

            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@username", SqlDbType.NChar)
            {
                Value = login.username
            };

            sqlParameters[1] = new SqlParameter("@password", SqlDbType.NChar)
            {
                Value = login.pwdhash
            };

            Employee employee = new Employee();
            employee.id = Convert.ToInt32(ExecuteSelectQuery(query, sqlParameters));

            return employee;
        }
    }
}
