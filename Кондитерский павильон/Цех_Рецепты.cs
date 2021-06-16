using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Цех_Рецепты : Form
    {
        public Цех_Рецепты()
        {
            InitializeComponent();
            Program.Цех_Рецепты = this;
        }
        /////////////////////////////////////////
        public static int n = -1;
        public static bool editing = false;
        public static int editing_id;
        public static string name;
        //////////////////////////////////////
        private void button5_Click(object sender, EventArgs e)
        {
            Цех_Создание_рецепта Цех_Создание_рецепта = new Цех_Создание_рецепта();
            Цех_Создание_рецепта.Show();
        }

        private void Цех_Рецепты_Load(object sender, EventArgs e)
        {
            SqlSelect();
        }

        public void SqlSelect()
        {
            string sql;
            sql = "select ingredient_kode as 'Системный номер', name as 'Название', type as 'Тип', description  as 'Описание',price as 'Себестоимость', end_price as 'Стоимость с учетом наценки', recipes.quantity as 'Получаемое количество продукции' , recipes.unit as 'Ед измерения' from recipes;";

            Conect.Table_Fill("recipes", sql);
            dataGridView1.DataSource = Conect.ds.Tables["recipes"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


            private void button2_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Вы точно хотите запись с названием " + dataGridView1.Rows[n].Cells["Системный номер"].Value + "?";
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
            sql = "DELETE FROM recipes WHERE ingredient_kode = '" + dataGridView1.Rows[n].Cells["Системный номер"].Value + "';";
            if (Conect.Modification_Execute(sql))
            {
                sql = "DELETE FROM `raw_materials-recipes` WHERE ingredient = '" + dataGridView1.Rows[n].Cells["Системный номер"].Value + "';";
                Conect.Modification_Execute(sql);
                Conect.ds.Tables["recipes"].Rows.RemoveAt(n);
                dataGridView1.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Данный тип уже используется.  Для удаления, удалите все существующие записи использующие данный параметр", "Ошибка");
            }
        }

        private void Цех_Рецепты_Activated(object sender, EventArgs e)
        {
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[2].Width = 120;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[0].Width = 75;
            dataGridView1.CurrentCell = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.CurrentRow.Index != 0)
            //{
                n = dataGridView1.CurrentRow.Index;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Цех_просмотр_рецепта Цех_просмотр_рецепта = new Цех_просмотр_рецепта();
            Цех_просмотр_рецепта.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Производство Производство = new Производство();
            Производство.Show();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            editing = true;
            
            try
            {
                editing_id = Convert.ToInt32(dataGridView1.Rows[n].Cells["Системный номер"].Value);
                name = dataGridView1.Rows[n].Cells["Название"].Value.ToString();
                Цех_Создание_рецепта Цех_Создание_рецепта = new Цех_Создание_рецепта();
                Цех_Создание_рецепта.Show();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана редактируемая запись таблицы!", "Ошибка"); return;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SqlSelect();
        }
    }
}
