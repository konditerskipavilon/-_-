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
            Program.склад_Сырья = this;
        }

        ///////////////////////////////////////////////////
        public static string name, quantity, unit;
        public static int id;
        int n = -1;
        ///////////////////////////////////////////////////
        
        private void button2_Click(object sender, EventArgs e)
        {
            Склад_сырья_Добавление_на_Склад Склад_сырья_Добавление_на_Склад = new Склад_сырья_Добавление_на_Склад();
            Склад_сырья_Добавление_на_Склад.Show();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            name = null;
            try
            {
                id = Convert.ToInt32(dataGridView1.Rows[n].Cells["Системный номер"].Value);
                name = dataGridView1.Rows[n].Cells["Название"].Value.ToString();
                quantity = dataGridView1.Rows[n].Cells["Количество"].Value.ToString();
                unit = dataGridView1.Rows[n].Cells["Единица измерения"].Value.ToString();
                Склад_сырья_списание Склад_сырья_списание = new Склад_сырья_списание();
                Склад_сырья_списание.Show();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана запись таблицы!", "Ошибка");
            }



        }

        private void button3_Click(object sender, EventArgs e)
        { 
            Склад_сырья_история_списаний склад_сырья_списание = new Склад_сырья_история_списаний();
            склад_сырья_списание.Show();
        }

        public void Склад_сырья_Load(object sender, EventArgs e)
        {
            sql();
        }

        public void sql()
        {
            string sql;
            sql = "select id as 'Системный номер', name as 'Название', type as 'Тип',quantity  as 'Количество',unit  as 'Единица измерения', price as 'Цена' from raw_materials;";

            Conect.Table_Fill("raw_materials", sql);
            dataGridView1.DataSource = Conect.ds.Tables["raw_materials"];
            dataGridView1.Columns["Цена"].DefaultCellStyle.Format = "C2";
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = dataGridView1.CurrentRow.Index;
            }
            catch (System.NullReferenceException) { }
        }

        private void Склад_сырья_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }
    }
}
