﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Кондитерский_павильон
{
    public partial class Магазин_Склад_готовой_продукции : Form
    {
        public Магазин_Склад_готовой_продукции()
        {
            InitializeComponent();
        }

        private void Склад_готовой_продукции_Load(object sender, EventArgs e)
        {
            SqlSelect();
        }

        public void SqlSelect()
        {
            string sql;
            sql = $"select finished_products.id as 'Системный номер', concat('(', recipes.ingredient_kode ,') ', recipes.name) as '(Номер) Название', recipes.type as 'Тип рецепта', finished_products.quantity as 'Количество', recipes.price as 'Себестоимость' from finished_products inner join recipes on recipes.ingredient_kode = finished_products.recipes_id where finished_products.shop_id = {Производство.id_shop};";

            Conect.Table_Fill("finished_products", sql);
            dataGridView1.DataSource = Conect.ds.Tables["finished_products"];
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
