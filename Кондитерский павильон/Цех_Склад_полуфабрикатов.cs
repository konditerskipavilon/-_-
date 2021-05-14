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
    public partial class Цех_Склад_полуфабрикатов : Form
    {
        public Цех_Склад_полуфабрикатов()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Цех_списание Цех_списание = new Цех_списание();
            Цех_списание.Show();
        }
    }
}
