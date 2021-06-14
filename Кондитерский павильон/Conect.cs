using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Кондитерский_павильон
{
    class Conect
    {
        static string ip = Properties.Settings.Default.ip;
        static string name_bd = Properties.Settings.Default.name_bd;
        static string user = Properties.Settings.Default.user;
        static string password = Properties.Settings.Default.password;

        public static string myConnectionString = $"server={ip} ;user={user};database={name_bd};password={password};";
        public static MySqlConnection connection = new MySqlConnection(myConnectionString);

        public static DataSet ds = new DataSet();

        public static void Table_Fill(string name, string sql)
        {
            if (ds.Tables[name] != null)
                ds.Tables[name].Clear();
            MySqlDataAdapter da;
            da = new MySqlDataAdapter(sql, connection);
            try
            {
                da.Fill(ds, name);
            } catch (MySql.Data.MySqlClient.MySqlException)
            {
                Производство.Message();

            }
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
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                connection.Close(); return false;
            }
            connection.Close();
            return true;
        }
        

    }
}
