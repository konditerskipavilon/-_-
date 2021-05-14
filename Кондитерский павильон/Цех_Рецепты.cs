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
    public partial class Цех_Рецепты : Form
    {
        public Цех_Рецепты()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Цех_Создание_рецепта Цех_Создание_рецепта = new Цех_Создание_рецепта();
            Цех_Создание_рецепта.Show();
        }
    }
}
