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
    public partial class Склад_сырья_Добавление_на_Склад : Form
    {
        public Склад_сырья_Добавление_на_Склад()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Склад_сырья_типы Склад_сырья_добавление_типа = new Склад_сырья_типы();
            Склад_сырья_добавление_типа.Show();
        }

        private void Склад_сырья_Добавление_на_Склад_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Conect.connection.Close();
            Склад_сырья_редактирование_единицы_измерения Склад_сырья_редактирование_единицы_измерения = new Склад_сырья_редактирование_единицы_измерения();
            Склад_сырья_редактирование_единицы_измерения.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kod = null;
            kod = maskedTextBox2.Text + "." + maskedTextBox3.Text;

            string sql;
            if (textBox1.Text != "" && maskedTextBox1.Text != "" && comboBox1.Text != "" && comboBox2.Text != "")
            {
                sql = "INSERT INTO `raw_materials` (`name`, `type`, `quantity`, `unit`, `price`) VALUES ('" + textBox1.Text + "', '" + comboBox1.Text + "', '" + maskedTextBox1.Text + "', '" + comboBox2.Text + "', " + kod + ");";
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
                Program.склад_Сырья.sql();
                this.Close();
            }
            else
            {
                MessageBox.Show("Поле название не должно быть пустым", "Ошибка");
            }
        }

        private void Склад_сырья_Добавление_на_Склад_Activated(object sender, EventArgs e)
        {
           string sql = "SELECT name_type as 'Название' FROM type;";//
            Conect.Table_Fill("type", sql);
            comboBox1.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["type"].Rows.Count; i++)
         
                comboBox1.Items.Add(Conect.ds.Tables["type"].Rows[i]["Название"].ToString());

            sql = "SELECT name_unit as 'Название' FROM unit;";//
            Conect.Table_Fill("unit", sql);
            comboBox2.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["unit"].Rows.Count; i++)
          
                comboBox2.Items.Add(Conect.ds.Tables["unit"].Rows[i]["Название"].ToString());
            
        }

        private void maskedTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }

        private void maskedTextBox2_MouseDown(object sender, MouseEventArgs e)
        {
            maskedTextBox2.Text = null;
        }

        private void maskedTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button2_Click(sender, e);

            }
        }

        private void maskedTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button2_Click(sender, e);

            }
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button2_Click(sender, e);

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }
    }
}
