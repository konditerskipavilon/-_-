using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Производство : Form
    {
        public Производство()
        {
            InitializeComponent();
        }

        ////////////////////////////////////
        public static int id_shop;
        public static string name_shop;
        int n = -1;
        ////////////////////////////////////
       
        public void Производство_Load(object sender, EventArgs e)
        {
            Sql();
            Sql_raw_materials();
            dataGridView1.DataSource = Conect.ds.Tables["shop"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (Авторизация.ueser == "Администратор")
            {
                button3.Visible = true;
                button6.Visible = true;
                button5.Visible = true;
            }
            
            if (Авторизация.ueser == "Кондитер")
            {
                button2.Enabled = false;
            }

            if (Авторизация.ueser == "Продавец")
            {
                button4.Enabled = false;
                button1.Enabled = false;
            }
            string sql;
            sql = "select ingredient_kode as 'Системный номер', name as 'Название', type as 'Тип', description  as 'Описание',price as 'Себестоимость', end_price as 'Стоимость с учетом наценки', recipes.quantity as 'Получаемое количество продукции' , recipes.unit as 'Ед измерения' from recipes;";

            Conect.Table_Fill("recipes", sql);
        }
        public static void Message()
        {
            MessageBox.Show("Нет соединения с базой данных","Ошибка");
        }

        public static void Sql()
        {
            string sql;
            sql = "select id as 'Системный номер', title as 'Название', Address as 'Адрес'from shop;";

            Conect.Table_Fill("shop", sql);


        }

        public static void Sql_raw_materials()
        {
            string sql;
            sql = "select id as 'Системный номер', name as 'Название', type as 'Тип',quantity  as 'Количество',unit  as 'Единица измерения', price as 'Цена' from raw_materials where quantity != '0';";

            Conect.Table_Fill("raw_materials", sql);
        }



        private void button4_Click(object sender, EventArgs e)
        {
            Цех Цех = new Цех();
            Цех.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Склад_сырья Склад_сырья = new Склад_сырья();
            Склад_сырья.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                id_shop = Convert.ToInt32(dataGridView1.Rows[n].Cells[0].Value);
                name_shop = dataGridView1.Rows[n].Cells[1].Value.ToString();
                Магазин Магазин = new Магазин();
                Магазин.Show();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Необходимо выбрать точку реализации.", "Ошибка"); 
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
            Авторизация меню = new Авторизация(); меню.ShowDialog();
            Close();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Производство_создание_торговой_точки Производство_создание_торговой_точки = new Производство_создание_торговой_точки();
            Производство_создание_торговой_точки.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = dataGridView1.CurrentRow.Index;
            }
            catch (System.NullReferenceException) { }
        }

        private void Производство_Activated(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Настройки Настройки = new Настройки();
            Настройки.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string message;
            try
            {
                message = "Вы точно хотите запись с номером " + dataGridView1.Rows[n].Cells["Системный номер"].Value + "?";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не указана удаляемая запись таблицы!", "Ошибка"); return;
            }
            string caption = "Удаление";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql;
            sql = "DELETE FROM shop WHERE id = '" + dataGridView1.Rows[n].Cells["Системный номер"].Value + "';";
            if (Conect.Modification_Execute(sql))
            {
                Conect.ds.Tables["shop"].Rows.RemoveAt(n);
                dataGridView1.CurrentCell = null;
                n = -1;
            }
            else
            {
                MessageBox.Show("Данная точка теализации не может быть удалена т.к. кодержит готовую продукцию.", "Ошибка");
            }
        }

    }
}
