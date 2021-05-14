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
    public partial class Склад_сырья_Добавление_на_Склад : Form
    {
        public Склад_сырья_Добавление_на_Склад()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Склад_сырья_типы Склад_сырья_добавление_типа = new Склад_сырья_типы();
            Склад_сырья_добавление_типа.Show();
        }

        private void Склад_сырья_Добавление_на_Склад_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Склад_сырья_редактирование_единицы_измерения Склад_сырья_редактирование_единицы_измерения = new Склад_сырья_редактирование_единицы_измерения();
            Склад_сырья_редактирование_единицы_измерения.Show();
        }
    }
}
