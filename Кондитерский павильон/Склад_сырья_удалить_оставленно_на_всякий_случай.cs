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
    public partial class Склад_сырья_удалить_оставленно_на_всякий_случай : Form
    {
        public Склад_сырья_удалить_оставленно_на_всякий_случай()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Склад_сырья_Добавление_на_Склад Склад_сырья_Добавление_на_Склад = new Склад_сырья_Добавление_на_Склад();
            Склад_сырья_Добавление_на_Склад.Show();
        }

        private void Склад_сырья_Load(object sender, EventArgs e)
        {
            OpenForm(new Склад_сырья());
        }
        public void OpenForm(Form OF)
        {
            OF.TopLevel = false;
            OF.FormBorderStyle = FormBorderStyle.None;
            OF.Dock = DockStyle.Fill;
            panel1.Controls.Add(OF);
            panel1.Tag = OF;
            OF.BringToFront();
            OF.Show();
        }
    }
}
