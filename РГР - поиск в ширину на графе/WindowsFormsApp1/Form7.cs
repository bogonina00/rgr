using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form7 : Form
    {
        public Form7(int [,] arr, int countLine, int countVer)
        {

          
                InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            for (int i = 0; i < countVer; i++)
            {
                dataGridView1.Columns.Add("column 1" , i.ToString());
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }

            int[,] matrix = new int[countVer, countVer];

            for (int i = 0; i < countLine; i++)
            {
                matrix[arr[i, 0], arr[i, 1]] = 1;
                matrix[arr[i, 1], arr[i, 0]] = 1;
            }

            for (int i = 0; i < countVer; i++)
            {
                for (int j = 0; j < countVer; j++)
                {
                    if (matrix[i, j] != 1)
                    {
                        matrix[i, j] = 0;
                    }
                }
            }

            for (int i = 0; i < countVer; i++)
            {
                for (int j = 0; j < countVer; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = matrix[i, j];
                }
            }

        }

    }
}
