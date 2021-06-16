using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Склад_сырья_обновление : Form
    {
        public Склад_сырья_обновление()
        {
            InitializeComponent();
        }

        int id;
        string ed, quantity;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Conect.ds.Tables["raw_materials_0"].Rows.Count; i++)
            {
                if (Conect.ds.Tables["raw_materials_0"].Rows[i]["Название"].ToString() == comboBox1.Text)
                    id = Convert.ToInt32(Conect.ds.Tables["raw_materials_0"].Rows[i]["Системный номер"]);
            }

            for (int i = 0; i < Conect.ds.Tables["raw_materials_0"].Rows.Count; i++)
            {
                if (Conect.ds.Tables["raw_materials_0"].Rows[i]["Название"].ToString() == comboBox1.Text)
                    ed = Conect.ds.Tables["raw_materials_0"].Rows[i]["Единица измерения"].ToString();
            }

            for (int i = 0; i < Conect.ds.Tables["raw_materials_0"].Rows.Count; i++)
            {
                if (Conect.ds.Tables["raw_materials_0"].Rows[i]["Название"].ToString() == comboBox1.Text)
                    quantity = Conect.ds.Tables["raw_materials_0"].Rows[i]["Количество"].ToString();
            }
            label3.Text = $"Количество в наличии  {quantity}   ({ed})";
        }

        private void Склад_сырья_обновление_Load(object sender, EventArgs e)
        {
            string sql = "select id as 'Системный номер', name as 'Название', type as 'Тип',quantity  as 'Количество',unit  as 'Единица измерения', price as 'Цена' from raw_materials;";

            Conect.Table_Fill("raw_materials_0", sql);

            comboBox1.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["raw_materials_0"].Rows.Count; i++)

                comboBox1.Items.Add(Conect.ds.Tables["raw_materials_0"].Rows[i]["Название"].ToString());
        }

        private void maskedTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            double kod = Convert.ToDouble(maskedTextBox1.Text + "," + maskedTextBox2.Text);
            kod += Convert.ToDouble(quantity);

            if (maskedTextBox1.Text != "" && comboBox1.Text != "")
            {
                string sql = $"UPDATE `raw_materials` SET `quantity` = '{kod.ToString().Replace(",",".")}' WHERE `id` = '{id}';";
                MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                Conect.connection.Open();
                command.ExecuteNonQuery();                
                Conect.connection.Close();
                Производство.Sql_raw_materials();
                this.Close();
            }
            else
            {
                MessageBox.Show("Поля не должны быть пустыми.", "Ошибка");
            }
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
           
        }
    }
}
