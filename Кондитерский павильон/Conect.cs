using MySql.Data.MySqlClient;
using System.Data;

namespace Кондитерский_павильон
{
    class Conect
    {
        public static bool vipravlen;
        public static string myConnectionString = "server=192.168.1.10 ;user=root;database=kondit;password=Qq414213543;"; 
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
                vipravlen = true;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                vipravlen = false;
                connection.Close(); return false;
            }
            connection.Close();
            return true;
        }
    }
}
