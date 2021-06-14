using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Авторизация : Form
    {
        public Авторизация()
        {
            InitializeComponent();
        }

        public static string ueser = null;
        private void Авторизация_Load(object sender, EventArgs e)
        {
            string sql;
            MySqlCommand command;
            MySqlDataReader datrReader;
            Conect.connection.Open();

            sql = "SELECT * from users;";
            command = new MySqlCommand(sql, Conect.connection);
            datrReader = command.ExecuteReader();
            while (datrReader.Read())
                comboBox1.Items.Add(datrReader["name"]);
            Conect.connection.Close();
            textBox1.UseSystemPasswordChar = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql;
            MySqlCommand command;
            MySqlDataReader datrReader;
            Conect.connection.Open();

            sql = "SELECT * from users where name = '" + comboBox1.Text + "';";
            command = new MySqlCommand(sql, Conect.connection);
            datrReader = command.ExecuteReader();
            datrReader.Read();

            if (comboBox1.Text != "" && textBox1.Text == datrReader["password"].ToString())
            {
                Conect.connection.Close();
                comboBox1.Text = ueser;//Для прав
                Hide();
                Производство Производство = new Производство(); Производство.ShowDialog();
                Close();
            }
            else
            {
                MessageBox.Show("Неправильный пароль");
            }

            Conect.connection.Close();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox1.UseSystemPasswordChar = false;
            else
                textBox1.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button3_Click(sender, e);

            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button3_Click(sender, e);

            }
        }
    }
}
