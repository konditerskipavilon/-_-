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
    public partial class Цех_просмотр_рецепта : Form
    {
        public Цех_просмотр_рецепта()
        {
            InitializeComponent();
        }

        private void Цех_просмотр_рецепта_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = Conect.ds.Tables["recipes"].Rows[Цех_Рецепты.n]["Название"].ToString();
                textBox2.Text = Conect.ds.Tables["recipes"].Rows[Цех_Рецепты.n]["Тип"].ToString();
                textBox3.Text = Conect.ds.Tables["recipes"].Rows[Цех_Рецепты.n]["Описание"].ToString();
            }catch(System.IndexOutOfRangeException)
            {
                MessageBox.Show("Не выбрана строка","Ошибка");
                this.Close();
            }
        }
    }
}
