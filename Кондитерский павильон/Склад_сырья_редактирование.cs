using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Склад_сырья_редактирование : Form
    {
        public Склад_сырья_редактирование()
        {
            InitializeComponent();
        }
        int id;
        private void Склад_сырья_редактирование_Load(object sender, EventArgs e)
        {
            this.Text = $"Редактирование сырья: '{Склад_сырья.name}'";
            textBox1.Text = Склад_сырья.name;
            id = Склад_сырья.id;
            string price = Склад_сырья.price.Substring(Склад_сырья.price.Length - 2);
            maskedTextBox2.Text = null;
            maskedTextBox2.Text = Склад_сырья.price.Remove(Склад_сырья.price.Length - 2);
            maskedTextBox3.Text = price;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Склад_сырья_типы Склад_сырья_типы = new Склад_сырья_типы();
            Склад_сырья_типы.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Склад_сырья_редактирование_единицы_измерения Склад_сырья_редактирование_единицы_измерения = new Склад_сырья_редактирование_единицы_измерения();
            Склад_сырья_редактирование_единицы_измерения.Show();
        }

        private void Склад_сырья_редактирование_Activated(object sender, EventArgs e)
        {
            string sql = "SELECT name_type as 'Название' FROM type;";
            Conect.Table_Fill("type", sql);
            comboBox1.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["type"].Rows.Count; i++)

                comboBox1.Items.Add(Conect.ds.Tables["type"].Rows[i]["Название"].ToString());

            sql = "SELECT name_unit as 'Название' FROM unit;";
            Conect.Table_Fill("unit", sql);
            comboBox2.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["unit"].Rows.Count; i++)

                comboBox2.Items.Add(Conect.ds.Tables["unit"].Rows[i]["Название"].ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kod = null;
            kod = maskedTextBox2.Text + "." + maskedTextBox3.Text;
            string sql;
            if (textBox1.Text != "" && comboBox1.Text != "" && comboBox2.Text != "" && maskedTextBox2.Text != "")
            {
                sql = $"UPDATE `raw_materials` SET `name` = '{textBox1.Text}', `type` = '{comboBox1.Text}', `unit` = '{comboBox2.Text}', `price` = '{kod}' WHERE `id` = '{id}';";
                MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                Conect.connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    MessageBox.Show("Ошибка.", "Ошибка!");
                    Conect.connection.Close(); return;
                }
                Conect.connection.Close();
                Производство.Sql_raw_materials();
                this.Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля.", "Ошибка");
            }
        }
    }
}
