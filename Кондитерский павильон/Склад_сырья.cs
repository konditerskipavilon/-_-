using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

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
        public static string name, quantity, unit, price;
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
                MessageBox.Show("Не указана запись!", "Ошибка");
            }



        }

        private void button3_Click(object sender, EventArgs e)
        { 
            Склад_сырья_история_списаний склад_сырья_списание = new Склад_сырья_история_списаний();
            склад_сырья_списание.Show();
        }

        public void Склад_сырья_Load(object sender, EventArgs e)
        {
            Sql();
            comboBox1.Text = "Нет фильтра";
            comboBox1.Items.Clear();
            for (int i = 0; i < Conect.ds.Tables["raw_materials"].Rows.Count; i++)
            {
                comboBox1.Items.Add(Conect.ds.Tables["raw_materials"].Rows[i]["Тип"].ToString());
            }
            object[] items = comboBox1.Items.OfType<String>().Distinct().ToArray();
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Нет фильтра");
            comboBox1.Items.AddRange(items);

        }
        #region 1
        public void Sql()
        {

            dataGridView1.DataSource = Conect.ds.Tables["raw_materials"];
            dataGridView1.Columns["Цена"].DefaultCellStyle.Format = "C2";
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Склад_сырья_обновление Склад_сырья_обновление = new Склад_сырья_обновление();
            Склад_сырья_обновление.Show();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text == "")
            {
                Conect.ds.Tables["raw_materials"].DefaultView.RowFilter = "";
                dataGridView1.CurrentCell = null;
            }
            else
            {
                Conect.ds.Tables["raw_materials"].DefaultView.RowFilter = $"Название Like '%" + textBox1.Text + "%'";
                dataGridView1.CurrentCell = null;

            }
        }
        #endregion
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Нет фильтра")
            {
                Conect.ds.Tables["raw_materials"].DefaultView.RowFilter = "";
                dataGridView1.CurrentCell = null;
            }
            else
            {
                Conect.ds.Tables["raw_materials"].DefaultView.RowFilter = $"Тип = '" + comboBox1.Text + "'";
                dataGridView1.CurrentCell = null;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = dataGridView1.Rows.Count;
            progressBar1.Visible = true;
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

            for (int i = 1, l = 2; i < dataGridView1.RowCount + 1; i++, l++)
            {
                for (int j = 1; j < dataGridView1.ColumnCount + 1; j++)
                {
                    worksheet.Rows[l].Columns[j] = dataGridView1.Rows[i - 1].Cells[j - 1].Value;
                    progressBar1.Value = i;
                }
            }
            worksheet.Cells[1, 1] = "№";
            worksheet.Cells[1, 2] = "Название";
            worksheet.Cells[1, 3] = "Тип";
            worksheet.Cells[1, 4] = "Количество";
            worksheet.Cells[1, 5] = "Ед";
            worksheet.Cells[1, 6] = "Цена";
            worksheet.Columns[1].ColumnWidth = 5;
            worksheet.Columns[2].ColumnWidth = 30;
            worksheet.Columns[3].ColumnWidth = 20;
            worksheet.Columns[4].ColumnWidth = 25;
            worksheet.Columns[5].ColumnWidth = 5;
            worksheet.Columns[6].ColumnWidth = 10;
            excel.Visible = true;
            progressBar1.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                id = Convert.ToInt32(dataGridView1.Rows[n].Cells["Системный номер"].Value);
                name = dataGridView1.Rows[n].Cells["Название"].Value.ToString();
                price = dataGridView1.Rows[n].Cells["Цена"].Value.ToString();
                Склад_сырья_редактирование Склад_сырья_редактирование = new Склад_сырья_редактирование();
                Склад_сырья_редактирование.Show();

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана запись !", "Ошибка");
            }
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
