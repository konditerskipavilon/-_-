using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Магазин_Склад_готовой_продукции : Form
    {
        public Магазин_Склад_готовой_продукции()
        {
            InitializeComponent();
        }

        int n;
        public static string id, name, quantity, unit;

        private void Склад_готовой_продукции_Load(object sender, EventArgs e)
        {
            SqlSelect();
            dataGridView1.DataSource = Conect.ds.Tables["finished_products"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public static void SqlSelect()
        {
            string sql;
            sql = $"select finished_products.id as 'Системный номер', recipes.name as 'Название', recipes.type as 'Тип', finished_products.quantity as 'Количество', recipes.unit as 'Единица измерения', finished_products.price as 'Себестоимость' , finished_products.end_price as 'Цена' from finished_products inner join recipes on recipes.ingredient_kode = finished_products.recipes_id where finished_products.shop_id = {Производство.id_shop};";

            Conect.Table_Fill("finished_products", sql);

        }

        private void Магазин_Склад_готовой_продукции_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            n = dataGridView1.CurrentRow.Index;
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                id = dataGridView1.Rows[n].Cells["Системный номер"].Value.ToString();
                name = dataGridView1.Rows[n].Cells["Название"].Value.ToString();
                quantity = dataGridView1.Rows[n].Cells["Количество"].Value.ToString();
                unit = dataGridView1.Rows[n].Cells[4].Value.ToString();
                Магазин_склад_сырья_списание Магазин_склад_сырья_списание = new Магазин_склад_сырья_списание();
                Магазин_склад_сырья_списание.Show();
                n = -1;

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана запись !", "Ошибка");
            }

        }
    }
}
