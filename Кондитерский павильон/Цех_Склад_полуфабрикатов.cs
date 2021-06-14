using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Цех_Склад_полуфабрикатов : Form
    {
        int n = -1;
        public static string id, name, quantity;
        public Цех_Склад_полуфабрикатов()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            Склад_сырья_история_списаний Склад_сырья_история_списаний = new Склад_сырья_история_списаний();
            Склад_сырья_история_списаний.Show();
        }

        private void Цех_Склад_полуфабрикатов_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Conect.ds.Tables["semi_finished_products"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public static void SqlSelect()
        {
            string sql;
            sql = "select semi_finished_products.id as 'Системный номер', concat('(', recipes.ingredient_kode ,') ', recipes.name) as '(Номер) Название рецепта', recipes.type as 'Тип рецепта', semi_finished_products.quantity as 'Количество', concat(recipes.quantity,' ', recipes.unit) as 'Получаемое количество за 1 единицу.', semi_finished_products.data as 'Дата' from semi_finished_products inner join recipes on recipes.ingredient_kode = semi_finished_products.recipes_id;";

            Conect.Table_Fill("semi_finished_products", sql);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                id = dataGridView1.Rows[n].Cells["Системный номер"].Value.ToString();
                name = dataGridView1.Rows[n].Cells["(Номер) Название рецепта"].Value.ToString();
                quantity = dataGridView1.Rows[n].Cells["Количество"].Value.ToString();

                Цех_списание Цех_списание = new Цех_списание();
                Цех_списание.Show();
                n = -1;

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана запись !", "Ошибка");
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            n = dataGridView1.CurrentRow.Index;
        }

        private void Цех_Склад_полуфабрикатов_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }
    }
}
