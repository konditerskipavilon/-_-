using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Магазин_склад_сырья_списание : Form
    {
        public Магазин_склад_сырья_списание()
        {
            InitializeComponent();
        }
        bool delete;
        private void button2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "" && maskedTextBox2.Text != "")
            {
                Double a, b;
                Double kod;
                a = Convert.ToDouble(maskedTextBox1.Text + "," + maskedTextBox2.Text);
                b = Convert.ToDouble(Магазин_Склад_готовой_продукции.quantity);
                if(a == 0)
                {
                    MessageBox.Show("Нельзя списать 0","Ошибка");
                    return;
                }
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

                        sql = "UPDATE finished_products SET quantity = " + kod.ToString().Replace(",", ".") + " WHERE id = " + Магазин_Склад_готовой_продукции.id + ";";
                        Conect.Modification_Execute(sql);
                    }
                    else
                    {
                        string sql;

                        sql = "DELETE FROM finished_products WHERE id = " + Магазин_Склад_готовой_продукции.id + ";";
                        Conect.Modification_Execute(sql);
                    }

                    Магазин_Склад_готовой_продукции.SqlSelect();
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

        private bool Save()
        {
            string sql;
            string masked;
            masked = maskedTextBox1.Text + "." + maskedTextBox2.Text;
            if (delete == false)
            {
                masked = Магазин_Склад_готовой_продукции.quantity.Replace(",", ".");
            }
            if (maskedTextBox1.Text != "")
            {
                sql = $"INSERT INTO `write_off` (`name`, `reason`, `quantity`, `unit`) VALUES ('{textBox1.Text}', '{textBox3.Text}', '{masked}', '{Магазин_Склад_готовой_продукции.unit}');";
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

        private void Магазин_склад_сырья_списание_Load(object sender, EventArgs e)
        {
            textBox1.Text = Магазин_Склад_готовой_продукции.name;
            textBox4.Text = Магазин_Склад_готовой_продукции.unit;
        }

        private void maskedTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }
    }
}
