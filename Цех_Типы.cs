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
    public partial class Цех_Типы : Form
    {
        public Цех_Типы()
        {
            InitializeComponent();
        }
        int n = -1;
        private void Цех_Типы_Load(object sender, EventArgs e)
        {
            Sql();
        }

        private void Sql()
        {
            string sql;
            sql = "select name as 'Название', recipes_typecol as 'Описание' from recipes_type;";

            Conect.Table_Fill("recipes_type", sql);
            dataGridView1.DataSource = Conect.ds.Tables["recipes_type"];
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
                sql = "INSERT INTO `recipes_type` (`name`, `recipes_typecol`) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "');";
                MySqlCommand command = new MySqlCommand(sql, Conect.connection);
                Conect.connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    MessageBox.Show("Невозможно добавить тип с названием которое уже создано, измените название типа.", "Ошибка!");
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
            string caption = "Удаление типа";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = "DELETE FROM recipes_type WHERE name = '" + dataGridView1.Rows[n].Cells["Название"].Value + "';";
            Conect.Modification_Execute(sql);
            if (Conect.vipravlen == true)
            {
                Conect.ds.Tables["recipes_type"].Rows.RemoveAt(n);
                dataGridView1.AutoResizeColumns();
                dataGridView1.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Данный тип уже используется.  Для удаления, удалите все существующие записи использующие данный параметр", "Ошибка");
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

        private void Цех_Типы_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }
    }
}
