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

        private void Производство_Load(object sender, EventArgs e)
        {

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