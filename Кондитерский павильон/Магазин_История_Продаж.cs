using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Магазин_История_Продаж : Form
    {
        public Магазин_История_Продаж()
        {
            InitializeComponent();
        }

        private void Магазин_История_Продаж_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Магазин_просмотр_чека Магазин_просмотр_чека = new Магазин_просмотр_чека();
            Магазин_просмотр_чека.Show();
        }
    }
}
