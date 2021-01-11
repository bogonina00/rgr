using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        int nowLab = 0;

        public Form4()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nowLab++;
            if (nowLab == 2) 
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = false;
                label6.Visible = false;
                button2.Visible = false;
            }
            else if (nowLab == 1)
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = true;
                label6.Visible = true;
                button3.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Form3 title3 = new Form3();
            title3.Location = this.Location;
            title3.ShowDialog();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nowLab--;
            if (nowLab == 0)
            {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                button3.Visible = false;
            }
            if (nowLab == 1 )
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = true;
                label6.Visible = true;
                button2.Visible = true;
            }
        }

    }
}
