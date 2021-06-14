using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Цех_списание : Form
    {
        public Цех_списание()
        {
            InitializeComponent();
        }

        string id = Цех_Склад_полуфабрикатов.id;
        private void Цех_списание_Load(object sender, EventArgs e)
        {
            textBox1.Text = Цех_Склад_полуфабрикатов.name;
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "" && maskedTextBox2.Text != "")
            {
                Double a, b;
                Double kod;
                a = Convert.ToDouble(maskedTextBox1.Text + "," + maskedTextBox2.Text);
                b = Convert.ToDouble(Цех_Склад_полуфабрикатов.quantity);
                if (a < b)
                {
                    delete = true;
                }
                else
                {
                    delete = false;
                }
                if (Save())
                {
                    if (delete)
                    {
                        string sql;

                        kod = b - a;

                        sql = "UPDATE semi_finished_products SET quantity = " + kod.ToString().Replace(",", ".") + " WHERE id = " + id + ";";
                        Conect.Modification_Execute(sql);
                    }
                    else
                    {
                        string sql;

                        sql = $"DELETE FROM `semi_finished_products` WHERE `id` = '{id}';";
                        Conect.Modification_Execute(sql);
                    }

                    Цех_Склад_полуфабрикатов.SqlSelect();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный тип данных", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Укажите количество.", "Ошибка");
            }
        }

        public bool delete;
        private bool Save()
        {
            string sql;
            string masked;
            masked = maskedTextBox1.Text + "." + maskedTextBox2.Text;
            if (delete == false)
            {
                masked = Цех_Склад_полуфабрикатов.quantity.Replace(",", ".");
            }
            if (maskedTextBox1.Text != "")
            {
                sql = $"INSERT INTO `write_off` (`name`, `reason`, `quantity`) VALUES ('{textBox1.Text}', '{textBox2.Text}', '{masked}');";
                MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                Conect.connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    Conect.connection.Close();
                    return false;
                }
                Conect.connection.Close();
                return true;
            }
            else
            {

                return false;
            }
        }

        private void maskedTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }
    }
}
