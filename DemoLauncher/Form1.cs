using DropdownSelect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable ds = new DataTable("dataSource");

            ds.Columns.Add(new DataColumn("aa"));
            ds.Columns.Add(new DataColumn("bb"));
            ds.Columns.Add(new DataColumn("cc"));

            var row1 = ds.NewRow();
            row1["aa"] = "很长很长很长的中文";
            row1["bb"] = "contain";
            row1["cc"] = "Colunm's";
            ds.Rows.Add(row1);

            var row2 = ds.NewRow();
            row2["aa"] = "81";
            row2["bb"] = "中山";
            row2["cc"] = "Windows8";
            ds.Rows.Add(row2);

            var row3 = ds.NewRow();
            row3["aa"] = "山泉水";
            row3["bb"] = "8";
            row3["cc"] = "window";
            ds.Rows.Add(row3);

            dropdownSelect1.DataSource = ds;
            dropdownSelect1.DisplayMember = "cc";
            dropdownSelect1.Columns[0].IsVisible = true;
            dropdownSelect1.Columns[0].Filterabled = false;
            dropdownSelect1.Columns[2].IsVisible = true;

            GridPanelDropdownColumn column = new GridPanelDropdownColumn();
            column.DataPropertyName = "cc";
            column.ListSource = ds;

            gridPanel1.Columns.Add(column);
            gridPanel1.Columns.Add(new DataGridViewTextBoxColumn());
            gridPanel1.Columns.Add(new DataGridViewTextBoxColumn());
            gridPanel1.Columns.Add(new DataGridViewTextBoxColumn());
        }

        private void dropdownSelect1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
