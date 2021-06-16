using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Магазин_История_Продаж : Form
    {
        public Магазин_История_Продаж()
        {
            InitializeComponent();
        }

        int n = -1;
        public static int id;

        private void Магазин_История_Продаж_Load(object sender, EventArgs e)
        {
            
            string sql;
            sql = $"select id as 'Системный номер', data as 'Дата', price as 'Цена' from cheque where shop_id = '{Производство.id_shop}';";

            Conect.Table_Fill("cheque", sql);
            dataGridView1.DataSource = Conect.ds.Tables["cheque"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            try
            {
                id = Convert.ToInt32(dataGridView1.Rows[n].Cells[0].Value);
                Магазин_просмотр_чека Магазин_просмотр_чека = new Магазин_просмотр_чека();
                Магазин_просмотр_чека.Show();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана запись!", "Ошибка");
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            n = dataGridView1.CurrentRow.Index;
        }

        private void Магазин_История_Продаж_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
