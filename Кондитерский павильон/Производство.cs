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
    public partial class Производство : Form
    {
        public Производство()
        {
            InitializeComponent();
        }

        public void Производство_Load(object sender, EventArgs e)
        {
            Sql();
        }
        public static void Message()
        {
            MessageBox.Show("Нет соединения с базой данных","Ошибка");
        }

        public void Sql()
        {
            string sql;
            sql = "select id as 'Системный номер', title as 'Название', Address as 'Адрес'from shop;";

            Conect.Table_Fill("shop", sql);
            dataGridView1.DataSource = Conect.ds.Tables["shop"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var insertButton = new Button();
            if (sender is Button button)
            {
                button.Text = "Магазин №2";
                Controls.Add(insertButton);
            }
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
            Магазин Магазин = new Магазин();
            Магазин.Show();
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

    }
}