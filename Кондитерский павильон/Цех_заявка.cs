using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Цех_заявка : Form
    {
        public Цех_заявка()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////
        string recipes;
        string kod;
        bool test;
        //////////////////////////////////////////////
        

        private void Цех_заявка_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["shop"].Rows.Count; i++)

                comboBox2.Items.Add(Conect.ds.Tables["shop"].Rows[i]["Название"].ToString());

            string sql;
            sql = "select ingredient_kode as 'Системный номер', name as 'Название', type as 'Тип', description  as 'Описание',price as 'Себестоимость' from recipes where type = 'Готовая продукция';";
            Conect.Table_Fill("recipes_application_g", sql);

            sql = "select ingredient_kode as 'Системный номер', name as 'Название', type as 'Тип', description  as 'Описание',price as 'Себестоимость' from recipes where type != 'Готовая продукция';";
            Conect.Table_Fill("recipes_application_p", sql);
            sql = "SELECT * FROM semi_finished_products;";

            Conect.Table_Fill("semi_finished_products_p", sql);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kod = maskedTextBox1.Text + "." + maskedTextBox2.Text; string sem_finished_product = "", destination_store = ", `destination_store`";

            if (comboBox3.Text != "" && comboBox1.Text != "" && comboBox1.Text != "" && maskedTextBox1.Text != "" && comboBox2.Text != "" && comboBox4.Text != "")
            {
                string shop = null, sql;
                recipes = null;
                sql = "select * from recipes";
                Conect.Table_Fill("select_recipes", sql);

                    for (int i = 0; i < Conect.ds.Tables["select_recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["select_recipes"].Rows[i]["name"].ToString() == comboBox1.Text)
                            recipes = Conect.ds.Tables["select_recipes"].Rows[i]["ingredient_kode"].ToString();
                    }

                if (comboBox2.Text != "Склад полуфабрикатов")
                {
                    for (int i = 0; i < Conect.ds.Tables["recipes"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["recipes"].Rows[i]["Системный номер"].ToString() == recipes)
                            if (Conect.ds.Tables["recipes"].Rows[i]["Получаемое количество продукции"].ToString() == "")
                            {
                                MessageBox.Show("Невозможно создать заявку если не указано количество получаемой продукции или если оно равно '0'.", "Ошибка");
                                return;

                            }
                            else
                            {
                                if (Convert.ToDouble(Conect.ds.Tables["recipes"].Rows[i]["Получаемое количество продукции"]) <= 0)
                                {
                                    MessageBox.Show("Невозможно создать заявку если не указано количество получаемой продукции или если оно равно '0'.", "Ошибка");
                                    return;
                                }
                            }
                    }
                }

                if (comboBox2.Text == "Склад полуфабрикатов")
                {
                    sem_finished_product = "1";
                    shop = null;
                    destination_store = null;
                }
                else
                {
                    for (int i = 0; i < Conect.ds.Tables["shop"].Rows.Count; i++)
                    {
                        if (Conect.ds.Tables["shop"].Rows[i]["Название"].ToString() == comboBox2.Text)
                            shop = ", '" + Conect.ds.Tables["shop"].Rows[i]["Системный номер"].ToString() + "'";
                        sem_finished_product = "0";
                    }
                }
                test = false;

                if (Materials_Update_test(kod, recipes))
                {
                    WriteOff_Update_test(recipes);
                }
                 
                sql = "select id as 'Системный номер', name as 'Название', type as 'Тип',quantity  as 'Количество',unit  as 'Единица измерения', price as 'Цена' from raw_materials where quantity != '0';";

                Conect.Table_Fill("raw_materials", sql);

                if (test)
                {
                    MessageBox.Show("Недостаточно ресурсов", "Ошибка");
                    return;
                }

                if (Materials_Update(kod,recipes))
                {
                    WriteOff_Update(recipes);

                    sql = $"INSERT INTO `applications` (`recipes_id`, `quantity`, `importance`{destination_store}, `sem_finished_product`, `condition`) VALUES ('{recipes}', '{kod}', '{comboBox4.Text}'{shop}, '{sem_finished_product}','Достаточно сырья');";
                    MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                    Conect.connection.Open();
                    command.ExecuteNonQuery();
                    Conect.connection.Close();
                    Program.Цех.SqlSelect();
                }
                else
                {
                    

                    sql = $"INSERT INTO `applications` (`recipes_id`, `quantity`, `importance`{destination_store}, `sem_finished_product`, `condition`) VALUES ('{recipes}', '{kod}', '{comboBox4.Text}'{shop}, '{sem_finished_product}','Недостаточно сырья');";
                    MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                    Conect.connection.Open();
                    command.ExecuteNonQuery();
                    Conect.connection.Close();
                    Program.Цех.SqlSelect();
                    return;
                }
                Close();


            }
            else
            {
                MessageBox.Show("Заполните все поля.", "Ошибка");
            }
        }
        double sum2;
        string id_test2;
        string produ2;
        private void WriteOff_Update(string id)
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
            $"inner join recipes on recipes.ingredient_kode = `raw_materials-recipes`.`semi-finished_product` where type = 'Полуфабрикат' AND ingredient = '{id}' " +
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
            $"inner join raw_materials on raw_materials.id = `raw_materials-recipes`.raw_materials_id where type = 'Полуфабрикат' AND ingredient = '{id}'; ";

            Conect.Table_Fill("raw_materials-recipes_where_type_p", sql);


            sql = "SELECT * FROM semi_finished_products;";

            Conect.Table_Fill("semi_finished_products_p", sql);

            string semi_finished_product = null;
            string quantity;
            bool MaterialsUpdateTest = true;
            for (int j = 0; j < Conect.ds.Tables["raw_materials-recipes_where_type_p"].Rows.Count; j++)
            {
                 semi_finished_product = Conect.ds.Tables["raw_materials-recipes_where_type_p"].Rows[j]["semi-finished_product"].ToString();
                 quantity = Conect.ds.Tables["raw_materials-recipes_where_type_p"].Rows[j]["Количество"].ToString();
               
                if (checkBox1.Checked == true)
                {
                    for (int l = 0; l < Conect.ds.Tables["semi_finished_products_p"].Rows.Count; l++)
                    {

                        string semi_finished_product_p = Conect.ds.Tables["semi_finished_products_p"].Rows[l]["recipes_id"].ToString();
                        int semi_finished_product_id = Convert.ToInt32(Conect.ds.Tables["semi_finished_products_p"].Rows[l]["id"]);

                        if (id_test2 != id || semi_finished_product != produ2)
                        {
                            sum2 = Convert.ToDouble(kod.Replace(".", ",")) * Convert.ToDouble(quantity);
                            id_test2 = id;
                        }

                        if (semi_finished_product_p == semi_finished_product)
                        {
                            produ2 = semi_finished_product;
                            double quantity_p = Convert.ToDouble(Conect.ds.Tables["semi_finished_products_p"].Rows[l]["quantity"]);
                            sum2 = sum2 - quantity_p;
                            if (sum2 > 0)
                            {
                                sql = $"DELETE FROM semi_finished_products WHERE id = '{semi_finished_product_id}';";
                                if (Conect.Modification_Execute(sql))
                                {
                                    Conect.ds.Tables["semi_finished_products"].Rows.RemoveAt(l);
                                    Conect.ds.Tables["semi_finished_products_p"].Rows.RemoveAt(l);
                                }
                                WriteOff_Update(id);
                                MaterialsUpdateTest = false;
                                return;
                            }
                            else
                            {
                                if (sum2 < 0)
                                {
                                    sum2 = -sum2 - sum2;
                                    sum2 = sum2 / 2;
                                    sql = $"UPDATE `semi_finished_products` SET `quantity` = '{sum2.ToString().Replace(",",".")}' WHERE `id` = '{semi_finished_product_id}';";
                                    Conect.Modification_Execute(sql);
                                    Conect.ds.Tables["semi_finished_products_p"].Rows[l]["quantity"] = quantity_p;
                                    Conect.ds.Tables["semi_finished_products"].Rows[l]["Количество"] = quantity_p;
                                    MaterialsUpdateTest = false;
                                }

                                if (sum2 == 0)
                                {
                                    sql = $"DELETE FROM semi_finished_products WHERE id = '{semi_finished_product_id}';";
                                    if (Conect.Modification_Execute(sql))
                                    {
                                        Conect.ds.Tables["semi_finished_products"].Rows.RemoveAt(l);
                                        Conect.ds.Tables["semi_finished_products_p"].Rows.RemoveAt(l);
                                        MaterialsUpdateTest = false;
                                    }
                                }
                            }
                        }

                    }
                    if (MaterialsUpdateTest)
                    {
                        if (!Materials_Update(sum2.ToString(), semi_finished_product))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (!Materials_Update(sum2.ToString(), semi_finished_product))
                    {
                        return;
                    }
                }

            }
            if (semi_finished_product != null)
            {
                WriteOff_Update(semi_finished_product);
            }
        }

        private bool Materials_Update(string quantity, string id)
        {
            int n = 0;
            string Mater_Recieps_Materials = null;
            string Materials_ID = null;
            string Mater_Recieps_ID = null;

            Conect.ds.Tables["raw_materials-recipes"].DefaultView.RowFilter = "[Тип] <> 'Полуфабрикат'";
            Conect.ds.Tables["raw_materials-recipes"].DefaultView.RowFilter = $"[ingredient] = '{id}'";

            for (int j = 0; j < Conect.ds.Tables["raw_materials-recipes"].Rows.Count; j++)
            {
                for (int i = 0; i < Conect.ds.Tables["raw_materials"].Rows.Count; i++)
                {
                    Mater_Recieps_ID = Conect.ds.Tables["raw_materials-recipes"].Rows[j]["ingredient"].ToString();
                    Mater_Recieps_Materials = Conect.ds.Tables["raw_materials-recipes"].Rows[j]["Номер"].ToString();
                    Materials_ID = Conect.ds.Tables["raw_materials"].Rows[i]["Системный номер"].ToString();

                    if (Mater_Recieps_ID == id && Mater_Recieps_Materials == Materials_ID)
                    {
                        n = i;
                        double sum = Convert.ToDouble(Conect.ds.Tables["raw_materials-recipes"].Rows[j]["Количество"]);
                        sum *= Convert.ToDouble(quantity.Replace(".", ","));
                        sum = Convert.ToDouble(Conect.ds.Tables["raw_materials"].Rows[i]["Количество"]) - sum;
                        if (sum >= 0)
                        {
                            string sql = $"UPDATE `raw_materials` SET `quantity` = '{sum.ToString().Replace(",", ".")}' WHERE `id` = '{Materials_ID}';";
                            Conect.Modification_Execute(sql);
                            Conect.ds.Tables["raw_materials"].Rows[n]["Количество"] = sum;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }

            }
            return true;
        }
        double sum;
        string id_test;
        string produ;
        private void WriteOff_Update_test(string id)
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
            $"inner join recipes on recipes.ingredient_kode = `raw_materials-recipes`.`semi-finished_product` where type = 'Полуфабрикат' AND ingredient = '{id}' " +
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
            $"inner join raw_materials on raw_materials.id = `raw_materials-recipes`.raw_materials_id where type = 'Полуфабрикат' AND ingredient = '{id}'; ";

            Conect.Table_Fill("raw_materials-recipes_where_type_p", sql);


            string semi_finished_product = null;
            string quantity;
            bool MaterialsUpdateTest = true;
            for (int j = 0; j < Conect.ds.Tables["raw_materials-recipes_where_type_p"].Rows.Count; j++)
            {
                semi_finished_product = Conect.ds.Tables["raw_materials-recipes_where_type_p"].Rows[j]["semi-finished_product"].ToString();
                quantity = Conect.ds.Tables["raw_materials-recipes_where_type_p"].Rows[j]["Количество"].ToString();

                
                if (checkBox1.Checked == true)
                {
                    for (int l = 0; l < Conect.ds.Tables["semi_finished_products_p"].Rows.Count; l++)
                    {

                        string semi_finished_product_p = Conect.ds.Tables["semi_finished_products_p"].Rows[l]["recipes_id"].ToString();
                        int semi_finished_product_id = Convert.ToInt32(Conect.ds.Tables["semi_finished_products_p"].Rows[l]["id"]);

                        if (id_test != id || semi_finished_product != produ)
                        {
                            sum = Convert.ToDouble(kod.Replace(".", ",")) * Convert.ToDouble(quantity);
                            id_test = id;
                        }

                        if (semi_finished_product_p == semi_finished_product)
                        {
                            produ = semi_finished_product;
                            double quantity_p = Convert.ToDouble(Conect.ds.Tables["semi_finished_products_p"].Rows[l]["quantity"]);
                            sum = sum - quantity_p;
                            if (sum > 0)
                            {
                                    Conect.ds.Tables["semi_finished_products_p"].Rows.RemoveAt(l);
                                    WriteOff_Update_test(id);
                                    MaterialsUpdateTest = false;
                                    return;
                            }
                            else
                            {
                                if (sum < 0)
                                {
                                    sum =  -sum - sum;
                                    sum = sum / 2;
                                    Conect.ds.Tables["semi_finished_products_p"].Rows[l]["quantity"] = sum;
                                    MaterialsUpdateTest = false;
                                }

                                if (sum == 0)
                                {
                                        Conect.ds.Tables["semi_finished_products_p"].Rows.RemoveAt(l);
                                    MaterialsUpdateTest = false;
                                }
                            }
                        }

                    }
                    if (MaterialsUpdateTest)
                    {
                        if (!Materials_Update_test(sum.ToString(), semi_finished_product))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (!Materials_Update_test(sum.ToString(), semi_finished_product))
                    {
                        return;
                    }
                }



            }
            if (semi_finished_product != null)
            {
                WriteOff_Update_test(semi_finished_product);
            }
        }

      

        private bool Materials_Update_test(string quantity, string id)
        {
            int n = 0;
            string Mater_Recieps_Materials = null;
            string Materials_ID = null;
            string Mater_Recieps_ID = null;

            Conect.ds.Tables["raw_materials-recipes"].DefaultView.RowFilter = "[Тип] <> 'Полуфабрикат'";
            Conect.ds.Tables["raw_materials-recipes"].DefaultView.RowFilter = $"[ingredient] = '{id}'";

            for (int j = 0; j < Conect.ds.Tables["raw_materials-recipes"].Rows.Count; j++)
            {
                for (int i = 0; i < Conect.ds.Tables["raw_materials"].Rows.Count; i++)
                {
                    Mater_Recieps_ID = Conect.ds.Tables["raw_materials-recipes"].Rows[j]["ingredient"].ToString();
                    Mater_Recieps_Materials = Conect.ds.Tables["raw_materials-recipes"].Rows[j]["Номер"].ToString();
                    Materials_ID = Conect.ds.Tables["raw_materials"].Rows[i]["Системный номер"].ToString();

                    if (Mater_Recieps_ID == id && Mater_Recieps_Materials == Materials_ID)
                    {
                        n = i;
                        double sum = Convert.ToDouble(Conect.ds.Tables["raw_materials-recipes"].Rows[j]["Количество"]);
                        sum *= Convert.ToDouble(quantity.Replace(".", ","));
                        sum = Convert.ToDouble(Conect.ds.Tables["raw_materials"].Rows[i]["Количество"]) - sum;
                        if (sum >= 0)
                        {
                            Conect.ds.Tables["raw_materials"].Rows[n]["Количество"] = sum;
                        }
                        else
                        {
                            test = true;
                            return false;
                        }
                    }

                }

            }
            return true;
        }



        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text != "")
            {
                if (comboBox3.Text == "Готовая продукция")
                {
                    comboBox1.Items.Clear();
                    comboBox2.Items.Remove("Склад полуфабрикатов");
                    for (int i = 0; i < Conect.ds.Tables["recipes_application_g"].Rows.Count; i++)

                        comboBox1.Items.Add(Conect.ds.Tables["recipes_application_g"].Rows[i]["Название"].ToString());
                }
                else
                {
                    comboBox1.Items.Clear();
                    comboBox2.Items.Add("Склад полуфабрикатов");
                    for (int i = 0; i < Conect.ds.Tables["recipes_application_p"].Rows.Count; i++)

                        comboBox1.Items.Add(Conect.ds.Tables["recipes_application_p"].Rows[i]["Название"].ToString());
                }
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
            
        }

        private void maskedTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            maskedTextBox1.Text = null;
        }
    }
}