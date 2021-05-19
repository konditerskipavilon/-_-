using MySql.Data.MySqlClient;
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
    public partial class Склад_сырья_редактирование_единицы_измерения : Form
    {
        public Склад_сырья_редактирование_единицы_измерения()
        {
            InitializeComponent();
        }

        int n = -1;

        private void Sql()
        {
            string sql;
            sql = "select name_unit as 'Название', description as 'Описание' from unit;";

            Conect.Table_Fill("unit", sql);
            dataGridView1.DataSource = Conect.ds.Tables["unit"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql;
            if (textBox1.Text != "")
            {
                sql = "INSERT INTO `unit` (`name_unit`, `description`) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "');";
                MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                Conect.connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                }
                catch
                {
                    MessageBox.Show("Невозможно добавить единицу измерения с названием которое уже создано, измените название ед.", "Ошибка!");
                    Conect.connection.Close(); return;
                }
                Conect.connection.Close();
                Sql();
                textBox1.Text = null;
                textBox2.Text = null;
            }
            else
            {
                MessageBox.Show("Поле название не должно быть пустым", "Ошибка");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Вы точно хотите запись с названием " + dataGridView1.Rows[n].Cells["Название"].Value + "?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана удаляемая запись таблицы!", "Ошибка"); return;
            }
            string caption = "Удаление единицы измерения";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = "DELETE FROM unit WHERE name_unit = '" + dataGridView1.Rows[n].Cells["Название"].Value + "';";
            Conect.Modification_Execute(sql);
            if (Conect.vipravlen == true)
            {
                Conect.ds.Tables["unit"].Rows.RemoveAt(n);
                dataGridView1.AutoResizeColumns();
                dataGridView1.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Данный тип уже используется.  Для удаления, удалите все существующие записи использующие данный параметр","Ошибка");
            }
        }

        private void Склад_сырья_редактирование_единицы_измерения_Load(object sender, EventArgs e)
        {
            Sql();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = dataGridView1.CurrentRow.Index;
            }
            catch (System.NullReferenceException) { }
        }

        private void Склад_сырья_редактирование_единицы_измерения_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }
    }
}
