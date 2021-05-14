using MySql.Data.MySqlClient;
using System.Data;

namespace Кондитерский_павильон
{
    class Conect
    {
        public static string myConnectionString = "server=185.154.75.232 ;user=root;database=kondit1;password=Qq414213543;"; // Database = rudenko; Data Source = 127.0.0.1; UserId = root; Password = Qwerty123;
        public static MySqlConnection connection = new MySqlConnection(myConnectionString);

        public static DataSet ds = new DataSet();

        public static void Table_Fill(string name, string sql)
        {
            if (ds.Tables[name] != null)
                ds.Tables[name].Clear();
            MySqlDataAdapter da;
            da = new MySqlDataAdapter(sql, connection);
            da.Fill(ds, name);
            connection.Close();
        }

        public static bool Modification_Execute(string sql)
        {
            MySqlCommand command;
            command = new MySqlCommand(sql, connection);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                connection.Close(); return false;
            }
            connection.Close();
            return true;
        }
    }
}
