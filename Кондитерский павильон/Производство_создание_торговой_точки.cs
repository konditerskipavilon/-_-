using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Производство_создание_торговой_точки : Form
    {
        public Производство_создание_торговой_точки()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql;
            if (textBox1.Text != "" )
            {
                sql = "INSERT INTO `shop` (`title`, `Address`) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "');";
                MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                Conect.connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    MessageBox.Show("Невозможно добавить тип с названием которое уже создано, измените название типа.", "Ошибка!");
                    Conect.connection.Close(); return;
                }
                Conect.connection.Close();
                Производство.Sql();
                this.Close();
            }
            else
            {
                MessageBox.Show("Поле название не должно быть пустым", "Ошибка");
            }
        }

        private void Производство_создание_торговой_точки_Load(object sender, EventArgs e)
        {

        }
    }
}
