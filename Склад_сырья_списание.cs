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

        public bool delete = true;

        private void save()
        {
            string kod = null;
            kod = dateTimePicker1.Text +" "+ dateTimePicker2.Text;
            string kod2 = kod.Replace(".", "/");
            string sql;
            string masked;
            masked = maskedTextBox2.Text;
            if(delete == false)
            {
                masked = Склад_сырья.quantity;
            }
            if (maskedTextBox2.Text != "")
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
                    MessageBox.Show("Неверный тип данных.", "Ошибка!");
                    Conect.connection.Close(); return;
                }
                Conect.connection.Close();

            }
            else
            {
                MessageBox.Show("Поле количество не должно быть пустым", "Ошибка");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            int a, b;
            int kod;
            a = Convert.ToInt32(maskedTextBox2.Text);
            b = Convert.ToInt32(Склад_сырья.quantity);


            if (a < b)
            {
                string sql;

                kod = b - a;

                sql = "UPDATE raw_materials SET quantity = " + kod + " WHERE id = " + Склад_сырья.id + ";";
                Conect.Modification_Execute(sql);
                //delite
                //update
            }
            else
            {
                string sql;
                delete = false;
                sql = "DELETE FROM raw_materials WHERE id = " + Склад_сырья.id + ";";
                Conect.Modification_Execute(sql);
                //delite
            }
            save();
            Program.склад_Сырья.sql();
            this.Close();
        }

        private void maskedTextBox2_MouseDown(object sender, MouseEventArgs e)
        {
            maskedTextBox2.Text = null;
        }
    }
}
