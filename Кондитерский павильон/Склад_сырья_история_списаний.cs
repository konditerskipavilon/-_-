using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Склад_сырья_история_списаний : Form
    {
        public Склад_сырья_история_списаний()
        {
            InitializeComponent();
        }
        int n = -1;

        
        private void склад_сырья_списание_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "select id as 'Системный номер', name as 'Название', reason as 'Описание', quantity  as 'Количество',unit  as 'Единица измерения', time as 'Время' from write_off;";

            Conect.Table_Fill("write_off", sql);
            dataGridView1.DataSource = Conect.ds.Tables["write_off"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if(Авторизация.ueser == "Администратор")
            {
                button2.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Вы точно хотите запись с cистемным номером " + dataGridView1.Rows[n].Cells["Системный номер"].Value + "?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана удаляемая запись таблицы!", "Ошибка"); return;
            }
            string caption = "Удаление записи";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = "DELETE FROM write_off WHERE id = '" + dataGridView1.Rows[n].Cells["Системный номер"].Value + "';";
            if (Conect.Modification_Execute(sql))
            {
                Conect.ds.Tables["write_off"].Rows.RemoveAt(n);
                dataGridView1.AutoResizeColumns();
                dataGridView1.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Ошибка.", "Ошибка");
            }
        }

        private void Склад_сырья_история_списаний_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = dataGridView1.CurrentRow.Index;
            }
            catch(System.NullReferenceException) 
            {
                MessageBox.Show("Пелемени");
            }
        }

    }
}
