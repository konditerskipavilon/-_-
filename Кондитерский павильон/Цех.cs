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
    public partial class Цех : Form
    {
        public Цех()
        {
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Цех_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Цех_Склад_полуфабрикатов Цех_Склад_полуфабрикатов = new Цех_Склад_полуфабрикатов();
            Цех_Склад_полуфабрикатов.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Цех_Рецепты Цех_Рецепты = new Цех_Рецепты();
            Цех_Рецепты.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Цех_заявка Цех_заявка = new Цех_заявка();
            Цех_заявка.Show();
        }
    }
}
