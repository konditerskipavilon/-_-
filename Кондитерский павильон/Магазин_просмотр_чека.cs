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
    public partial class Магазин_просмотр_чека : Form
    {
        public Магазин_просмотр_чека()
        {
            InitializeComponent();
        }

        private void Магазин_просмотр_чека_Load(object sender, EventArgs e)
        {
            string sql;
            sql = $"select cheque_fill.id as 'Системный номер', recipes.name as 'Название', cheque_fill.quantity as 'Количество', recipes.unit as 'ед измерения', cheque_fill.price as 'Себестоимость', cheque_fill.end_price as 'Цена' from cheque_fill inner join recipes on recipes.ingredient_kode = cheque_fill.recepes_id where cheque_id = {Магазин_История_Продаж.id};";

            Conect.Table_Fill("cheque_fill2", sql);
            dataGridView1.DataSource = Conect.ds.Tables["cheque_fill2"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
