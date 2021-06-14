using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using Кондитерский_павильон.Properties;

namespace Кондитерский_павильон
{
    public partial class Настройки : Form
    {
        public Настройки()
        {
            InitializeComponent();
        }

        private void Настройки_Load(object sender, EventArgs e)
        {
            textBox3.Text = Properties.Settings.Default.ip;
            textBox4.Text = Properties.Settings.Default.name_bd;
            textBox5.Text = Properties.Settings.Default.user;
            textBox6.Text = Properties.Settings.Default.password;
            textBox6.UseSystemPasswordChar = true;

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
            textBox2.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.Default["ip"] = textBox3.Text;
            Settings.Default["name_bd"] = textBox4.Text;
            Settings.Default["user"] = textBox5.Text;
            Settings.Default["password"] = textBox6.Text;
            Settings.Default.Save();
            MessageBox.Show("Изменения сохранены, вступят в силу после перезагрузки программы.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox2.Text)
            {
                string sql = $"UPDATE `users` SET `password` = '{textBox1.Text}' WHERE `name` = '{comboBox1.Text}';";
                Conect.Modification_Execute(sql);
                MessageBox.Show("Пароль успешно изменён.","Уведомление.");

            }
            else { MessageBox.Show("Неверное подтверждение пароля"); }
        }
    }
}
