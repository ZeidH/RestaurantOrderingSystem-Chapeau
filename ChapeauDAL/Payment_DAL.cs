using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ChapeauDAL
{
    class Payment_DAL : Connection
    {
        private void Db_Payment()
        {
            SqlConnection connection = OpenConnectionDB();

            //Query - Get all the names and which room they are in
            sb.Append("SELECT r.room_id, r.IS_LECTURER_ROOM, concat(s.student_first_name, ' ' , s.student_last_name) as student_name, concat(l.teacher_first_name, ' ' , l.teacher_last_name) as teacher_name " +
                "FROM(ROOM AS r LEFT JOIN STUDENT AS s ON r.room_id=s.room_id)LEFT JOIN LECTURER AS l ON r.room_id = l.room_id");

            string sql = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ReadInfoQuery(reader);
                }
                reader.Close();
            }
            connection.Close();
        }
        private void ReadInfoQuery(SqlDataReader reader)
        {
            //Receive data from database with query and send it to Model layer
            //Example: setStudentOrTeacher((bool)reader["IS_LECTURER_ROOM"]);

        }
    }
}
