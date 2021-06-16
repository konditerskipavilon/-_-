using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Магазин_Продажа : Form
    {
        public Магазин_Продажа()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        int n = -1, m = -1;
        string name, quantity, sql;
        double price, sum;
        int id, id_cheque;
        bool test = true;
        

        private void Магазин_Продажа_Load(object sender, EventArgs e)
        {
            SqlSelect();

            dataGridView2.DataSource = Conect.ds.Tables["cheque_fill"];
            dataGridView2.BackgroundColor = SystemColors.Control;
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.DataSource = Conect.ds.Tables["finished_products"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        public void SqlSelect()
        {
            string sql;
            sql = $"select finished_products.id as 'Системный номер', recipes.name as 'Название', recipes.type as 'Тип', finished_products.quantity as 'Количество', recipes.unit as 'Единица измерения', finished_products.price as 'Себестоимость' , finished_products.end_price as 'Цена' from finished_products inner join recipes on recipes.ingredient_kode = finished_products.recipes_id where finished_products.shop_id = {Производство.id_shop};";

            Conect.Table_Fill("finished_products", sql);



            sql = $"select cheque_fill.id as 'Системный номер', recipes.name as 'Название', cheque_fill.quantity as 'Количество', cheque_fill.price as 'Цена' from cheque_fill inner join recipes on recipes.ingredient_kode = cheque_fill.recepes_id where cheque_id = '0';";

            Conect.Table_Fill("cheque_fill", sql);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Магазин_История_Продаж Магазин_История_Продаж = new Магазин_История_Продаж();
            Магазин_История_Продаж.Show();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text == "")
            {
                Conect.ds.Tables["finished_products"].DefaultView.RowFilter = "";
                dataGridView1.CurrentCell = null;
            }
            else
            {
                Conect.ds.Tables["finished_products"].DefaultView.RowFilter = $"Название Like '%" + textBox1.Text + "%'";
                dataGridView1.CurrentCell = null;

            }
        }

        private void cost_price()
        {
            sum = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; ++i)
            {
                sum += Convert.ToDouble(dataGridView2.Rows[i].Cells["Цена"].Value.ToString().Replace(".", ","));
            }
            label1.Text = "Итого: " + sum.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (maskedTextBox1.Text != "" && maskedTextBox2.Text != "" && n >= 0)
                {

                    if (test)
                    {
                        sql = "select cheque_fill.id as 'Системный номер', recipes.name as 'Название', cheque_fill.quantity as 'Количество', cheque_fill.price as 'Цена' from cheque_fill inner join recipes on recipes.ingredient_kode = cheque_fill.recepes_id where cheque_id = '0';";
                        Conect.Table_Fill("cheque_fill", sql);
                        sql = $"select id as 'Системный номер', data as 'Дата', price as 'Цена' from cheque;";
                        Conect.Table_Fill("select_cheque", sql);

                        for (int i = 0; i < Conect.ds.Tables["select_cheque"].Rows.Count; i++)
                        {
                            id_cheque = Convert.ToInt32(Conect.ds.Tables["select_cheque"].Rows[i]["Системный номер"]);
                        }

                        id_cheque++;
                        sql = $"INSERT INTO `cheque` (`id`, `shop_id`) VALUES ('{id_cheque}', '{Производство.id_shop}');";
                        MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                        Conect.connection.Open();
                        try
                        {
                            command.ExecuteNonQuery();

                        }
                        catch (MySql.Data.MySqlClient.MySqlException)
                        {
                            button3_Click(null, null);
                            Conect.connection.Close(); return;
                        }
                        Conect.connection.Close();
                        test = false;
                    }
                    string kod = maskedTextBox1.Text + "," + maskedTextBox2.Text;
                    name = dataGridView1.Rows[n].Cells["Название"].Value.ToString();
                    quantity = dataGridView1.Rows[n].Cells["Количество"].Value.ToString();
                    price = Convert.ToDouble(dataGridView1.Rows[n].Cells["Цена"].Value);
                    price = price * Convert.ToDouble(kod);

                    for (int i = 0; i < Conect.ds.Tables["recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes"].Rows[i]["Название"].ToString() == name)
                            id = Convert.ToInt32(Conect.ds.Tables["recipes"].Rows[i]["Системный номер"]);
                    }
                    if(Convert.ToDouble(quantity) >= Convert.ToDouble(kod))
                    {
                        if (Convert.ToDouble(quantity) > Convert.ToDouble(kod))
                        {
                            quantity = (Convert.ToDouble(quantity) - Convert.ToDouble(kod)).ToString();
                            sql = $"UPDATE `finished_products` SET `quantity` = '{quantity.Replace(",", ".")}' WHERE `id` = '{dataGridView1.Rows[n].Cells["Системный номер"].Value}';";
                            MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                            Conect.connection.Open();
                            try
                            {
                                command.ExecuteNonQuery();

                            }
                            catch (MySql.Data.MySqlClient.MySqlException)
                            {
                                Conect.connection.Close(); return;
                            }
                            Conect.connection.Close();
                            sql = $"select finished_products.id as 'Системный номер', recipes.name as 'Название', recipes.type as 'Тип', finished_products.quantity as 'Количество', recipes.unit as 'Единица измерения', finished_products.price as 'Себестоимость' , finished_products.end_price as 'Цена' from finished_products inner join recipes on recipes.ingredient_kode = finished_products.recipes_id where finished_products.shop_id = {Производство.id_shop};";

                            Conect.Table_Fill("finished_products", sql);
                        }
                        else
                        {
                            sql = "DELETE FROM finished_products WHERE id = '" + dataGridView1.Rows[n].Cells["Системный номер"].Value + "';";
                            if (Conect.Modification_Execute(sql))
                            {
                                Conect.ds.Tables["finished_products"].Rows.RemoveAt(n);
                                dataGridView1.AutoResizeColumns();
                                dataGridView1.CurrentCell = null;
                                n = -1;
                            }
                            else
                            {
                                MessageBox.Show("Ошибка.", "Ошибка");
                                return;
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Количество ингредиентов добавляемое на продажу не должно быть более чем имеется на складе.", "Ошибка");
                        return;
                    }

                    sql = $"INSERT INTO `cheque_fill` (`cheque_id`, `recepes_id`, `quantity`, `price`) VALUES ('{id_cheque}', '{id}', '{kod.Replace(",",".")}', '{price.ToString().Replace(",",".")}');";
                    MySqlCommand command2 = new MySqlCommand(sql, Conect.connection);
                    Conect.connection.Open();
                    try
                    {
                        command2.ExecuteNonQuery();

                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        Conect.connection.Close(); return;
                    }
                    Conect.connection.Close();
                    sql = $"select cheque_fill.id as 'Системный номер', recipes.name as 'Название', cheque_fill.quantity as 'Количество', cheque_fill.price as 'Цена' from cheque_fill inner join recipes on recipes.ingredient_kode = cheque_fill.recepes_id where cheque_id = '{id_cheque}';";
                    Conect.Table_Fill("cheque_fill", sql);
                    n = -1;
                }

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана запись!", "Ошибка");
            }
            cost_price();
        }

        private void Магазин_Продажа_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            maskedTextBox1.Text = null;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            sql = $"select finished_products.id as 'Системный номер', recipes.name as 'Название', recipes.type as 'Тип', finished_products.quantity as 'Количество', recipes.unit as 'Единица измерения', finished_products.price as 'Себестоимость' , finished_products.end_price as 'Цена' from finished_products inner join recipes on recipes.ingredient_kode = finished_products.recipes_id where finished_products.shop_id = {Производство.id_shop};";

            Conect.Table_Fill("finished_products", sql);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            test = true;
            string sql = $"UPDATE `cheque` SET `price` = '{sum.ToString().Replace(",",".")}' WHERE (`id` = '{id_cheque}');";
            MySqlCommand command = new MySqlCommand(sql, Conect.connection);
            Conect.connection.Open();
            try
            {
                command.ExecuteNonQuery();

            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                Conect.connection.Close(); return;
            }
            Conect.connection.Close();
            sql = "select cheque_fill.id as 'Системный номер', recipes.name as 'Название', cheque_fill.quantity as 'Количество', cheque_fill.price as 'Цена' from cheque_fill inner join recipes on recipes.ingredient_kode = cheque_fill.recepes_id where cheque_id = '0';";
            Conect.Table_Fill("cheque_fill", sql);
            cost_price();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Удалить запись с номером  " + dataGridView2.Rows[m].Cells["Системный номер"].Value + "?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана удаляемая запись таблицы!", "Ошибка"); return;
            }
            string caption = "Удаление";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = "DELETE FROM cheque_fill WHERE id = '" + dataGridView2.Rows[m].Cells["Системный номер"].Value + "';";
            if (Conect.Modification_Execute(sql))
            {
                Conect.ds.Tables["cheque_fill"].Rows.RemoveAt(m);
                dataGridView2.AutoResizeColumns();
                dataGridView2.CurrentCell = null;
                m = -1;
            }
            else
            {
                MessageBox.Show("Ошибка.", "Ошибка");
            }
            cost_price();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m = dataGridView2.CurrentRow.Index;
        }

        private void maskedTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             n = dataGridView1.CurrentRow.Index;
        }
    }
}
