using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Склад_сырья_списание : Form
    {
        public Склад_сырья_списание()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
        }

        private void Склад_сырья_списание_Load(object sender, EventArgs e)
        {
            textBox1.Text = Склад_сырья.name;
            textBox4.Text = Склад_сырья.unit;
        }

        public bool delete;
        private bool Save()
        {
            string kod = dateTimePicker1.Text +" "+ dateTimePicker2.Text;
            string kod2 = kod.Replace(".", "/");
            string sql;
            string masked;
            masked = maskedTextBox1.Text + "." + maskedTextBox2.Text;
            if(delete == false)
            {
                masked = Склад_сырья.quantity.Replace(",",".");
            }
            if (maskedTextBox1.Text != "")
            {
                sql = "INSERT INTO `write_off` (`name`, `reason`, `quantity`, `unit`, `time`) VALUES ('" + textBox1.Text + "', '" + textBox3.Text + "', " + masked + ", '" + textBox4.Text + "' , '" + kod2.Replace(":", ".") + "');";
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "" && maskedTextBox2.Text != "")
            {
                Double a, b;
                Double kod;
                a = Convert.ToDouble(maskedTextBox1.Text + "," + maskedTextBox2.Text);
                b = Convert.ToDouble(Склад_сырья.quantity);
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

                        sql = "UPDATE raw_materials SET quantity = " + kod.ToString().Replace(",", ".") + " WHERE id = " + Склад_сырья.id + ";";
                        Conect.Modification_Execute(sql);
                    }
                    else
                    {
                        string sql;
                        
                        sql = "UPDATE raw_materials SET quantity = 0 WHERE id = " + Склад_сырья.id + ";";
                        Conect.Modification_Execute(sql);
                    }

                    Производство.Sql_raw_materials();
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

        private void maskedTextBox2_MouseDown(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }
    }
}
