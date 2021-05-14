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
    public partial class Склад_сырья : Form
    {
        public Склад_сырья()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Склад_сырья_Добавление_на_Склад Склад_сырья_Добавление_на_Склад = new Склад_сырья_Добавление_на_Склад();
            Склад_сырья_Добавление_на_Склад.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Склад_сырья_списание Склад_сырья_списание = new Склад_сырья_списание();
            Склад_сырья_списание.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        { 
            Склад_сырья_история_списаний склад_сырья_списание = new Склад_сырья_история_списаний();
            склад_сырья_списание.Show();
        }

        private void Склад_сырья_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "select * from raw_materials;";

            Conect.Table_Fill("raw_materials", sql);
            dataGridView1.DataSource = Conect.ds.Tables["raw_materials"];
            //dataGridView1.Columns["Марка и модель ТС"].Visible = false;
            //dataGridView1.Columns["Собственник ТС"].Visible = false;
            //dataGridView1.Columns["Место регистрации ТС"].Visible = false;
            //dataGridView1.Columns["Сумма штрафа"].DefaultCellStyle.Format = "C2";
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }
    }
}
