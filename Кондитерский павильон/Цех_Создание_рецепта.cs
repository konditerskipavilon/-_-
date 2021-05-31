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
    public partial class Цех_Создание_рецепта : Form
    {
        public Цех_Создание_рецепта()
        {
            InitializeComponent();
        }

        //////////////////
        int IdReciepes;
        int n = -1;
        //////////////////
        private void Цех_Создание_рецепта_Load(object sender, EventArgs e)
        {
            
        }

        private bool IfNameExists(string name)
        {
            Conect.connection.Open();
            var sql = $"select * from recipes where name = '{name}';";

            MySqlCommand command = new MySqlCommand(sql, Conect.connection);
            var result = command.ExecuteReader().HasRows;
            Conect.connection.Close();
            return result;
        }

        private void Save_Click(object sender, EventArgs e)
        {

            if (IfNameExists(textBox1.Text))
            {
                MessageBox.Show("Невозможно добавить технологическую карту с названием которое уже создано, измените название.", "Ошибка!");
                return;
            }

            string sql = "select ingredient_kode as 'Системный номер', name as 'Название', type as 'Тип', description  as 'Описание' from recipes where type = 'Полуфабрикат';";
            Conect.Table_Fill("recipes_box", sql);


            if (textBox1.Text != "" && comboBox1.Text != "")
            {
                sql = "INSERT INTO `recipes` (`name`, `type`, `description`) VALUES ('" + textBox1.Text + "','" + comboBox1.Text + "','" + textBox3.Text + "');";
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
                groupBox2.Visible = true;

                sql = "select * from recipes;";

                Conect.Table_Fill("recipes_kod", sql);
                for (int i = 0; i < Conect.ds.Tables["recipes_kod"].Rows.Count; i++)
                {
                    if (Conect.ds.Tables["recipes_kod"].Rows[i]["name"].ToString() == textBox1.Text)
                        IdReciepes = Convert.ToInt32(Conect.ds.Tables["recipes_kod"].Rows[i]["ingredient_kode"]);
                }

                SqlSelect();
            }
            else
            {
                MessageBox.Show("Поле название не должно быть пустым", "Ошибка");
            }

        }

        private void ComboBoxType_Clic(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Полуфабрикат")
            {

                comboBox2.Items.Clear();
                for (int i = 0; i < Conect.ds.Tables["recipes_box"].Rows.Count; i++)

                    comboBox2.Items.Add(Conect.ds.Tables["recipes_box"].Rows[i]["Название"].ToString());
            }
            else
            {
                string sql = "select id as 'Системный номер', name as 'Название', type as 'Тип',quantity  as 'Количество',unit  as 'Единица измерения', price as 'Цена' from raw_materials where quantity != '0';";
                Conect.Table_Fill("raw_materials_box", sql);
                comboBox2.Items.Clear();
                for (int i = 0; i < Conect.ds.Tables["raw_materials_box"].Rows.Count; i++)

                    comboBox2.Items.Add(Conect.ds.Tables["raw_materials_box"].Rows[i]["Название"].ToString());
            }
        }

        private void AddIngredient_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                string id = null;
                string kod_quantity = maskedTextBox1.Text + "." + maskedTextBox2.Text;

                if (comboBox3.Text == "Полуфабрикат")
                {
                    for (int i = 0; i < Conect.ds.Tables["recipes_box"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes_box"].Rows[i]["Название"].ToString() == comboBox2.Text)
                            id = Conect.ds.Tables["recipes_box"].Rows[i]["Системный номер"].ToString();
                    }
                }
                else
                {
                    for (int i = 0; i < Conect.ds.Tables["raw_materials_box"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["raw_materials_box"].Rows[i]["Название"].ToString() == comboBox2.Text)
                            id = Conect.ds.Tables["raw_materials_box"].Rows[i]["Системный номер"].ToString();
                    }
                }

                if (comboBox3.Text == "Полуфабрикат")
                {
                    string sql = $"INSERT INTO `kondit`.`raw_materials-recipes` (`ingredient`, `semi-finished_product`, `quantity`, `price`) VALUES ('{IdReciepes}', '{id}', '{kod_quantity}', '999');";
                    MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                    Conect.connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();

                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        Conect.connection.Close();

                    }
                    Conect.connection.Close();

                }
                else
                {
                    string sql = $"INSERT INTO `kondit`.`raw_materials-recipes` (`ingredient`, `raw_materials_id`, `quantity`, `price`) VALUES ('{IdReciepes}', '{id}', '{kod_quantity}', '998');";
                    MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                    Conect.connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();

                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        Conect.connection.Close();

                    }
                    Conect.connection.Close();

                }
                SqlSelect();
            }
            else
            {
                MessageBox.Show("Ингридиент не может быть пустым", "Ошибка");
            }
        }

        private void SqlSelect()
        {
            string sql;

            sql = "select " +
            "`raw_materials-recipes`.id as 'Системный номер', "+
            "recipes.name as 'Название', "+
            "recipes.type as 'Тип', "+
            "`raw_materials-recipes`.quantity as 'Количество', "+
            "`raw_materials-recipes`.price as 'Цена' "+
            "from "+
            "`raw_materials-recipes` "+
            "inner join recipes on recipes.ingredient_kode = `raw_materials-recipes`.`semi-finished_product` "+
            "where "+
            $"ingredient = '{ IdReciepes}' "+
            "union "+
            "select "+
            "`raw_materials-recipes`.id as 'Системный номер', "+
            "raw_materials.name as 'Название', "+
            "raw_materials.type as 'Тип', "+
            "`raw_materials-recipes`.quantity as 'Количество', "+
            "`raw_materials-recipes`.price as 'Цена' "+
            "from "+
            "`raw_materials-recipes` "+
            "inner join raw_materials on raw_materials.id = `raw_materials-recipes`.raw_materials_id " +
            "where " +
            $"ingredient = '{ IdReciepes}';";

            Conect.Table_Fill("raw_materials-recipes", sql);
            dataGridView2.DataSource = Conect.ds.Tables["raw_materials-recipes"];
            dataGridView2.BackgroundColor = SystemColors.Control;
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Вы точно хотите удалить все записи ?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана удаляемая запись таблицы!", "Ошибка"); return;
            }
            string caption = "Удаление ";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = $"DELETE FROM `raw_materials-recipes` WHERE ingredient = '{IdReciepes}';";
            if (Conect.Modification_Execute(sql))
            {
                SqlSelect();
                dataGridView2.AutoResizeColumns();
                dataGridView2.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Данный  уже используется.  Для удаления, удалите все существующие записи использующие данный параметр", "Ошибка");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Вы точно хотите запись с названием " + dataGridView2.Rows[n].Cells["Системный номер"].Value + "?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана удаляемая запись таблицы!", "Ошибка"); return;
            }
            string caption = "Удаление единицы измерения";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = "DELETE FROM `raw_materials-recipes` WHERE id = '" + dataGridView2.Rows[n].Cells["Системный номер"].Value + "';";
            if (Conect.Modification_Execute(sql))
            {
                Conect.ds.Tables["raw_materials-recipes"].Rows.RemoveAt(n);
                dataGridView2.AutoResizeColumns();
                dataGridView2.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Данный  уже используется.  Для удаления, удалите все существующие записи использующие данный параметр", "Ошибка");
            }
        }

        private void Цех_Создание_рецепта_Activated(object sender, EventArgs e)
        {
            dataGridView2.AutoResizeColumns();
            dataGridView2.CurrentCell = null;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = dataGridView2.CurrentRow.Index;
            }
            catch (System.NullReferenceException) { }
        }

        private void maskedTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }

        private void maskedTextBox2_MouseClick(object sender, MouseEventArgs e)
        {
            maskedTextBox2.Text = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
