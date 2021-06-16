using MySql.Data.MySqlClient;
using System;
using System.Drawing;
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
        double price;
        double sum;
        bool clouse = false;
        bool сancel = false;
        string name;
        //////////////////
        
        private void Цех_Создание_рецепта_Load(object sender, EventArgs e)
        {

            if(Цех_Рецепты.editing)
            {
                IdReciepes = Цех_Рецепты.editing_id;
                name = Цех_Рецепты.name;
                groupBox2.Visible = true;
                SqlSelect();
                cost_price();
            }
            else
            {
                IdReciepes = 0;
                groupBox2.Visible = false;
            }
            cost_price();
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
                MessageBox.Show("Невозможно добавить технологическую карту с уже использующимся названием, измените название.", "Ошибка!");
                return;
            }

            name = textBox1.Text;


            if (textBox1.Text != "" && comboBox1.Text != "")
            {
                string sql = "INSERT INTO `recipes` (`name`, `type`, `description`) VALUES ('" + textBox1.Text + "','" + comboBox1.Text + "','" + textBox3.Text + "');";
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
                clouse = true;
            }
            else
            {
                MessageBox.Show("Тип не должен быть пустым", "Ошибка");
            }

        }

        private void ComboBoxType_Clic(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Полуфабрикат")
            {
                string sql = $"select ingredient_kode as 'Системный номер', name as 'Название', type as 'Тип', description  as 'Описание' from recipes where type = 'Полуфабрикат' AND name != '{name}';";
                Conect.Table_Fill("recipes_box", sql);
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
            if (comboBox2.Text != "" && maskedTextBox1.Text != "")
            {
                string id = null;
                string kod_quantity = maskedTextBox1.Text + "," + maskedTextBox2.Text;
                price = 0;

                if (comboBox3.Text == "Полуфабрикат")
                {
                    for (int i = 0; i < Conect.ds.Tables["recipes_box"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes_box"].Rows[i]["Название"].ToString() == comboBox2.Text)
                            id = Conect.ds.Tables["recipes_box"].Rows[i]["Системный номер"].ToString();
                    }
                    for (int i = 0; i < Conect.ds.Tables["recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes"].Rows[i]["Название"].ToString() == comboBox2.Text)
                            price = Convert.ToDouble(Conect.ds.Tables["recipes"].Rows[i]["Себестоимость"]);
                    }
                }
                else
                {
                    for (int i = 0; i < Conect.ds.Tables["raw_materials_box"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["raw_materials_box"].Rows[i]["Название"].ToString() == comboBox2.Text)
                            id = Conect.ds.Tables["raw_materials_box"].Rows[i]["Системный номер"].ToString();

                    }
                    for (int i = 0; i < Conect.ds.Tables["raw_materials_box"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["raw_materials_box"].Rows[i]["Название"].ToString() == comboBox2.Text)
                            price = Convert.ToDouble(Conect.ds.Tables["raw_materials_box"].Rows[i]["Цена"]);

                    }
                }

                price *= Convert.ToDouble(kod_quantity);

                if (comboBox3.Text == "Полуфабрикат")
                {
                    string sql = $"INSERT INTO `kondit`.`raw_materials-recipes` (`ingredient`, `semi-finished_product`, `quantity`, `price`) VALUES ('{IdReciepes}', '{id}', '{kod_quantity.ToString().Replace(",", ".")}', '{price.ToString().Replace(",", ".")}');";
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
                    string sql = $"INSERT INTO `kondit`.`raw_materials-recipes` (`ingredient`, `raw_materials_id`, `quantity`, `price`) VALUES ('{IdReciepes}', '{id}', '{kod_quantity.ToString().Replace(",", ".")}', '{price.ToString().Replace(",", ".")}');";
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
                Цех.Sql_raw_materials_recipes();
                cost_price();
            }
            else
            {
                MessageBox.Show("Ингридиент не может быть пустым", "Ошибка");
            }
        }

        private void SqlSelect()
        {
            Conect.ds.Tables["raw_materials-recipes"].DefaultView.RowFilter = "[ingredient] = " + IdReciepes + "";
            dataGridView2.DataSource = Conect.ds.Tables["raw_materials-recipes"];
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Visible = false;
            dataGridView2.BackgroundColor = SystemColors.Control;
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Delete_all();
            cost_price();
        }

        private void Delete_all()
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
                Цех.Sql_raw_materials_recipes();
                dataGridView2.AutoResizeColumns();
                dataGridView2.CurrentCell = null;
                n = -1;
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
                Цех.Sql_raw_materials_recipes();
            }
            else
            {
                MessageBox.Show("Данный  уже используется.  Для удаления, удалите все существующие записи использующие данный параметр", "Ошибка");
            }
            cost_price();
        }

        private void Цех_Создание_рецепта_Activated(object sender, EventArgs e)
        {
            dataGridView2.AutoResizeColumns();
            dataGridView2.CurrentCell = null;

            string sql = "SELECT name_unit as 'Название' FROM unit;";//
            Conect.Table_Fill("unit", sql);
            comboBox4.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["unit"].Rows.Count; i++)

                comboBox4.Items.Add(Conect.ds.Tables["unit"].Rows[i]["Название"].ToString());
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = dataGridView2.CurrentRow.Index;
            }
            catch (System.NullReferenceException)
            {
            }
        }

        private void maskedTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kod_price = maskedTextBox4.Text + "." + maskedTextBox3.Text;
            string kod = maskedTextBox6.Text + "." + maskedTextBox5.Text;
            Цех_Рецепты.editing = false;
            Цех_Рецепты.editing_id = 0;
            string sql = $"UPDATE `recipes` SET `price` = '{sum.ToString().Replace(",", ".")}', `end_price` = '{kod_price}', `quantity` = '{kod}', `unit` = '{comboBox4.Text}' WHERE `ingredient_kode` = '{IdReciepes}';";
            MySqlCommand command = new MySqlCommand(sql, Conect.connection);
            Conect.connection.Open();
            command.ExecuteNonQuery();
            Conect.connection.Close();
            Program.Цех_Рецепты.SqlSelect();
            сancel = true;
            this.Close();
        }
        private void cost_price()
        {
            sum = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; ++i)
            {
                sum += Convert.ToDouble(dataGridView2.Rows[i].Cells["Цена"].Value.ToString().Replace(".", ","));
            }
            label7.Text = "Общая себестоимость: " + sum.ToString();
        }

        private void Цех_Создание_рецепта_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clouse && !сancel)
            {
                    var result = MessageBox.Show("Вы не сохранили рецепт!\nУдалить рецепт?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {

                        string sql;
                        sql = $"DELETE FROM `raw_materials-recipes` WHERE ingredient = '{IdReciepes}';";
                        Conect.Modification_Execute(sql);
                        sql = $"DELETE FROM recipes WHERE ingredient_kode = '{IdReciepes}';";
                        Conect.Modification_Execute(sql);
                        Цех_Рецепты.editing = false;
                        Цех_Рецепты.editing_id = 0;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                
            }
            Цех_Рецепты.editing = false;
            Цех_Рецепты.editing_id = 0;
        }

        private void maskedTextBox4_MouseClick(object sender, MouseEventArgs e)
        {
            maskedTextBox4.Text = null;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text == "Сырье")
            {
                for (int i = 0; i < Conect.ds.Tables["raw_materials_box"].Rows.Count; i++)
                {
                    if (Conect.ds.Tables["raw_materials_box"].Rows[i]["Название"].ToString() == comboBox2.Text)
                        label9.Text = "(" + Conect.ds.Tables["raw_materials_box"].Rows[i]["Единица измерения"].ToString() + ")";
                }
            }
        }

        private void maskedTextBox6_MouseDown(object sender, MouseEventArgs e)
        {
            maskedTextBox6.Text = null;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Склад_сырья_редактирование_единицы_измерения Склад_сырья_редактирование_единицы_измерения = new Склад_сырья_редактирование_единицы_измерения();
            Склад_сырья_редактирование_единицы_измерения.Show();
            cost_price();
        }
    }
}
