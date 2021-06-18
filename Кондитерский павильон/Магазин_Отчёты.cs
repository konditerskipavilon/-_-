using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace Кондитерский_павильон
{
    public partial class Магазин_Отчёты : Form
    {
        public Магазин_Отчёты()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy-MM-dd";
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
        }

        double sum, sum2;

        private void Магазин_Отчёты_Load(object sender, EventArgs e)
        {
            string sql;
            sql = $"select id as 'Системный номер', data as 'Дата', price as 'Себестоимость', end_price as 'Цена' from cheque where shop_id = '{Производство.id_shop}';";

            Conect.Table_Fill("cheque_report", sql);
            dataGridView1.DataSource = Conect.ds.Tables["cheque_report"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cost_price();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kod = dateTimePicker1.Text + " " + dateTimePicker2.Text;
            string kod2 = dateTimePicker3.Text + " " + dateTimePicker4.Text;
            string sql;
            sql = $"select id as 'Системный номер', data as 'Дата', price as 'Себестоимость', end_price as 'Цена' from cheque where shop_id = {Производство.id_shop} AND data BETWEEN '{kod}' AND '{kod2}';";

            Conect.Table_Fill("cheque_report", sql);
            cost_price();
        }

        private void cost_price()
        {
            sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if(dataGridView1.Rows[i].Cells["Себестоимость"].Value.ToString() != "")
                {
                    sum += Convert.ToDouble(dataGridView1.Rows[i].Cells["Себестоимость"].Value.ToString().Replace(".", ","));
                }
                
            }
            label4.Text = "Себестоимость: " + sum.ToString();

            sum2 = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if (dataGridView1.Rows[i].Cells["Цена"].Value.ToString() != "")
                {
                    sum2 += Convert.ToDouble(dataGridView1.Rows[i].Cells["Цена"].Value.ToString().Replace(".", ","));
                }
            }
            label5.Text = "Итоговая цена:  " + sum2.ToString();

            label6.Text = "Выручка: " + (sum2 - sum).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
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
            worksheet.Cells[1, 2] = "Дата";
            worksheet.Cells[1, 3] = "Себестоимость";
            worksheet.Cells[1, 4] = "Цена";
            worksheet.Columns[1].ColumnWidth = 5;
            worksheet.Columns[2].ColumnWidth = 20;
            worksheet.Columns[3].ColumnWidth = 15;
            worksheet.Columns[4].ColumnWidth = 10;
            worksheet.Columns[5].ColumnWidth = 30;
            worksheet.Cells[1, 5] = "Общая себестоимость: " + sum;
            worksheet.Cells[2, 5] = "Общая цена: " + sum2;
            worksheet.Cells[3, 5] = label6.Text;
            excel.Visible = true;
            progressBar1.Visible = false;
        }

    }
}
