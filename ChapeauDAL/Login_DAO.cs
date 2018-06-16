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
        public Employee ValidateCredentials(string password, string username)
        {
            string query = "SELECT emp_id, emp_firstName, emp_lastName, emp_occupation FROM [EMPLOYEE] WHERE user_name = @username AND pwd_Hash = @password";

            SqlParameter[] sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@username", SqlDbType.NChar)
            {
                Value = username
            };

            sqlParameters[1] = new SqlParameter("@password", SqlDbType.NChar)
            {
                Value = password
            };

            return ReadEmployee(ExecuteSelectQuery(query, sqlParameters));
        }

        private Employee ReadEmployee(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                throw new NullReferenceException();
            }
            Employee employee = new Employee();

            foreach (DataRow dr in dataTable.Rows)
            {
                employee = new Employee
                {
                    ID = (int)dr["emp_id"],
                    Name = $"{dr["emp_firstName"]}  {dr["emp_lastName"]}",
                    Occupation = (Occupation)Int16.Parse(dr["emp_occupation"].ToString())
                };
            }
            return employee;

        }
    }
}

