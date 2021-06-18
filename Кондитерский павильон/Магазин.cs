using System;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Магазин : Form
    {
        public Магазин()
        {
            InitializeComponent();
        }

        private void Магазин_Load(object sender, EventArgs e)
        {
            Text = Производство.name_shop;
            button3.Enabled = false;
            OpenForm(new Магазин_Склад_готовой_продукции());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenForm(new Магазин_Продажа());
            enable();
            button1.Enabled = false;

        }

        private void enable()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private Form OFF = null;
        public void OpenForm(Form OF)
        {
            if (OFF != null)
            {
                OFF.Close();
            }
            OFF = OF;
            OF.TopLevel = false;
            OF.FormBorderStyle = FormBorderStyle.None;
            OF.Dock = DockStyle.Fill;
            panel1.Controls.Add(OF);
            panel1.Tag = OF;
            OF.BringToFront();
            OF.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            enable();
            button3.Enabled = false;
            OpenForm(new Магазин_Склад_готовой_продукции());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            enable();
            button2.Enabled = false;
           OpenForm(new Магазин_Отчёты());
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
