using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Цех : Form
    {
        public Цех()
        {
            InitializeComponent();
            Program.Цех = this;
        }
        int n = -1;
        double price, end_price, end_quantity;
        private void Цех_Load(object sender, EventArgs e)
        {
            SqlSelect();
            Sql_raw_materials_recipes();
            dataGridView1.DataSource = Conect.ds.Tables["applications"];
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (Авторизация.ueser == "Администратор")
            {
                button2.Visible = true;

            }
        }

        public void SqlSelect()
        {
            string sql;
            sql = "select applications.id as 'Системный номер', concat('(', recipes.ingredient_kode ,') ', recipes.name) as '(Номер) Название рецепта', recipes.type as 'Тип рецепта', applications.quantity as 'Количество', applications.condition as 'Состояние', applications.importance as 'Важность', applications.data as 'Дата', destination_store, sem_finished_product, recipes_id from applications inner join recipes on recipes.ingredient_kode = applications.recipes_id ORDER BY `id` ASC;";

            Conect.Table_Fill("applications", sql);

            
            sql = "select semi_finished_products.id as 'Системный номер', concat('(', recipes.ingredient_kode ,') ', recipes.name) as '(Номер) Название рецепта', recipes.type as 'Тип рецепта', semi_finished_products.quantity as 'Количество', concat(recipes.quantity,' ', recipes.unit) as 'Получаемое количество за 1 единицу.', semi_finished_products.data as 'Дата' from semi_finished_products inner join recipes on recipes.ingredient_kode = semi_finished_products.recipes_id;";

            Conect.Table_Fill("semi_finished_products", sql);

            sql = "select ingredient_kode as 'Системный номер', name as 'Название', type as 'Тип', description  as 'Описание',price as 'Себестоимость', end_price as 'Стоимость с учетом наценки', recipes.quantity as 'Получаемое количество продукции' , recipes.unit as 'Ед измерения' from recipes;";

            Conect.Table_Fill("recipes", sql);

        }

        public static void Sql_raw_materials_recipes()
        {
           string sql = "select " +
               "ingredient," +
          "`raw_materials-recipes`.id as 'Системный номер', " +
          "recipes.name as 'Название', " +
          "recipes.type as 'Тип', " +
          "`raw_materials-recipes`.`semi-finished_product`, " +
          "`raw_materials-recipes`.raw_materials_id as 'Номер', " +
          "`raw_materials-recipes`.quantity as 'Количество', " +
          "`raw_materials-recipes`.price as 'Цена' " +
          "from " +
          "`raw_materials-recipes` " +
          "inner join recipes on recipes.ingredient_kode = `raw_materials-recipes`.`semi-finished_product` " +
          "union " +
          "select " +
             " ingredient," +
          "`raw_materials-recipes`.id as 'Системный номер', " +
          "raw_materials.name as 'Название', " +
          "raw_materials.type as 'Тип', " +
          "`raw_materials-recipes`.`semi-finished_product`, " +
          "`raw_materials-recipes`.`raw_materials_id` as 'Номер', " +
          "`raw_materials-recipes`.quantity as 'Количество', " +
          "`raw_materials-recipes`.price as 'Цена' " +
          "from " +
          "`raw_materials-recipes` " +
          "inner join raw_materials on raw_materials.id = `raw_materials-recipes`.raw_materials_id; ";

            Conect.Table_Fill("raw_materials-recipes", sql);



        }

        private void button4_Click(object sender, EventArgs e)
        {
            Цех_Склад_полуфабрикатов Цех_Склад_полуфабрикатов = new Цех_Склад_полуфабрикатов();
            Цех_Склад_полуфабрикатов.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Цех_Рецепты Цех_Рецепты = new Цех_Рецепты();
            Цех_Рецепты.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Цех_заявка Цех_заявка = new Цех_заявка();
            Цех_заявка.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Закрыть заявку и отправить на указаную точку реализации, заявку с номером " + dataGridView1.Rows[n].Cells["Системный номер"].Value + "?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана запись!", "Ошибка"); return;
            }
            string caption = "Заявка";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            if(dataGridView1.Rows[n].Cells["Состояние"].Value.ToString() == "Достаточно сырья")
            {
                string sem_finished_product = dataGridView1.Rows[n].Cells["sem_finished_product"].Value.ToString();
                string shop_id = dataGridView1.Rows[n].Cells["destination_store"].Value.ToString();
                string recepes_id = dataGridView1.Rows[n].Cells[9].Value.ToString();
                double quantity = Convert.ToDouble(dataGridView1.Rows[n].Cells["Количество"].Value);

                string sql;
                if(sem_finished_product == "1")
                {
                    sql = $"INSERT INTO `semi_finished_products` (`recipes_id`, `quantity`) VALUES ('{recepes_id}', '{quantity.ToString().Replace(",",".")}');";
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
                    SqlSelect();
                }
                else
                {
                    for (int i = 0; i < Conect.ds.Tables["recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes"].Rows[i]["Системный номер"].ToString() == recepes_id)
                            quantity *= Convert.ToDouble(Conect.ds.Tables["recipes"].Rows[i]["Получаемое количество продукции"]);
                    }
                    for (int i = 0; i < Conect.ds.Tables["recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes"].Rows[i]["Системный номер"].ToString() == recepes_id)
                            price = Convert.ToDouble(Conect.ds.Tables["recipes"].Rows[i]["Себестоимость"]);
                    }
                    for (int i = 0; i < Conect.ds.Tables["recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes"].Rows[i]["Системный номер"].ToString() == recepes_id)
                            end_price = Convert.ToDouble(Conect.ds.Tables["recipes"].Rows[i]["Стоимость с учетом наценки"]);
                    }
                    for (int i = 0; i < Conect.ds.Tables["recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes"].Rows[i]["Системный номер"].ToString() == recepes_id)
                            end_quantity = Convert.ToDouble(Conect.ds.Tables["recipes"].Rows[i]["Получаемое количество продукции"]);
                    }
                    price = price / end_quantity;
                    end_price = end_price / end_quantity;
                    sql = $"INSERT INTO `finished_products` (`shop_id`, `recipes_id`, `quantity`, `price`,`end_price`) VALUES ('{shop_id}', '{recepes_id}', '{quantity.ToString().Replace(",", ".")}', '{price.ToString().Replace(",", ".")}' , '{end_price.ToString().Replace(",", ".")}');";
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
                    SqlSelect();
                }


                sql = "DELETE FROM applications WHERE id = '" + dataGridView1.Rows[n].Cells["Системный номер"].Value + "';";
                if (Conect.Modification_Execute(sql))
                {

                    Conect.ds.Tables["applications"].Rows.RemoveAt(n);
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.CurrentCell = null;
                    n = -1;
                }
                else
                {
                    MessageBox.Show("Произошла ошибка", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Невозможно закрыть заявку т.к недостаточно ресурсов для её завершения","Ошибка");
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Вы точно хотите удалить заявку с системным номером " + dataGridView1.Rows[n].Cells["Системный номер"].Value + "?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана удаляемая запись!", "Ошибка"); return;
            }
            string caption = "Удаление заявки";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = "DELETE FROM applications WHERE id = '" + dataGridView1.Rows[n].Cells["Системный номер"].Value + "';";
            if (Conect.Modification_Execute(sql))
            {
                
                Conect.ds.Tables["applications"].Rows.RemoveAt(n);
                dataGridView1.AutoResizeColumns();
                dataGridView1.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Данный тип уже используется.  Для удаления, удалите все существующие записи использующие данный параметр", "Ошибка");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                n = dataGridView1.CurrentRow.Index;
        }

        private void Цех_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
