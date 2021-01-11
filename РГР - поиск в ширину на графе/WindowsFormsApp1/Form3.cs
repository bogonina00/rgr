using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        bool initPoint = false;                     // Активировано ли создание вершин
        bool initLine = false;                      // Активировано ли создание рёбер
        bool auto = true;                           // Автоматический или ручной режим

        int countVer = 0;                           // Кол-во вершин
        int countLine = 0;                          // Кол-во рёбер
        int begin;                                  // Начальная вершина
        int time;                                   // Время одного шага

        int left, right = 0;                        // Счётчик для отрисовки рёбер
        int saveIforLeft, saveIforRight = -1;       // Счётчик для отрисовки рёбер
        int numberStep = 1;                         // Счётчик шагов визуализатора

        Queue<int> q = new Queue<int>();            // Очередь в ожидании
        Queue<int> ender = new Queue<int>();        // Очередь помеченных вершин

        Point[] arrVer = new Point[50];                 // Массив Вершин
        Point[,] arrLine = new Point[500, 2];       // Массив Рёбер
        int[,] smejnost = new int[200,2];           // связь между вершинами
        Label[] label = new Label[50];              // Индексы вершин

        int[,] offspringVer = new int[50, 50];      // Наследование вершин

        string[] level = new string[50];            // Строки для уровней

        // 
        Pen pnBlueElips = new Pen(Color.Blue, 3);
        Pen pnRedElips = new Pen(Color.Red, 3);
        Pen pnBlackElips = new Pen(Color.Black, 3);  
        SolidBrush brRed = new SolidBrush(Color.Red);           // Кисти для графики
        SolidBrush brWhite = new SolidBrush(Color.White);
        Pen pnRedRect = new Pen(Color.Red, 1);
        Pen pnBaseRect = new Pen(Color.FromArgb(255, 229, 229, 229), 1);

        //


        public Form3() // Конструктор формы
        {
            InitializeComponent();
        } 
        private bool isValInRange(string str, int a, int b) // Проверка, является ли данная строка числом между границами
        {
            
            int num;
            bool isNum = int.TryParse(str, out num);
            if (isNum)
            {
                if ((Convert.ToInt32(str) >= a) & (Convert.ToInt32(str) <= b))
                    return true;
                else
                    return false;
            }
            else
                return false;
           
        }   

        private void levels(int a, int k) // Рекурсивный метод для определение минимального кол-ва рёбер до вершин
        {
            for (int i = 1; i <= offspringVer[a, 0]; i++)
            {
                if(offspringVer[offspringVer[a, i], 0] != 0)
                {
                    levels(offspringVer[a, i], k + 1);
                }
                level[k] += offspringVer[a, i] + " ";
            }
        }   

        private void _pause(int value)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < value)
                Application.DoEvents();
        }       // Пауза
        
        private void stepUp() {                   
            string str1 = labelStepString.Text;
            string str2 = labelEndString.Text;
            Graphics g = CreateGraphics();

            while ((q.Count != 0))
            {
                str1 = "";

                int ind = q.Dequeue();                                                 // УДАЛЕНИЕ
                ender.Enqueue(ind);
                numberStep++;
                labelStep.Text = numberStep + "-й шаг";
                g.FillEllipse(brRed, arrVer[ind].X - 40, arrVer[ind].Y - 40, 40, 40);
                label[ind].BackColor = Color.Red;
                label[ind].Enabled = false;
                foreach (int number in q)
                {
                    str1 += " " + number.ToString() + " ";
                    g.DrawRectangle(pnBaseRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
                    labelStepString.Text = str1;
                    g.DrawRectangle(pnRedRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
                }
                if (str1.Equals(""))
                {
                    g.DrawRectangle(pnBaseRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
                    labelStepString.Text = str1;
                }
                str1 = "";
                str2 = "";
                foreach (int number in ender)
                {
                    str2 += " " + number.ToString() + " ";
                    g.DrawRectangle(pnBaseRect, labelEndString.Location.X - 1, labelEndString.Location.Y - 1, labelEndString.Width + 1, labelEndString.Height + 1);
                    labelEndString.Text = str2;
                    g.DrawRectangle(pnRedRect, labelEndString.Location.X - 1, labelEndString.Location.Y - 1, labelEndString.Width + 1, labelEndString.Height + 1);
                }
                str2 = "";
                int sup = 1;
                for (int i = 0; i < countLine; i++)
                {
                    if (smejnost[i, 0] == ind)
                    {
                        if ((!ender.Contains(smejnost[i, 1])) & (!q.Contains(smejnost[i, 1])))
                        {
                            offspringVer[ind, sup] = smejnost[i, 1];
                            sup++;

                                q.Enqueue(smejnost[i, 1]);
                                g.DrawEllipse(pnRedElips, arrVer[smejnost[i, 1]].X - 40, arrVer[smejnost[i, 1]].Y - 40, 40, 40);
                        }

                    }
                    else if (smejnost[i, 1] == ind)
                    {
                        if ((!ender.Contains(smejnost[i, 0])) & (!q.Contains(smejnost[i, 0])))
                        {
                            offspringVer[ind, sup] = smejnost[i, 0];
                            sup++;

                                q.Enqueue(smejnost[i, 0]);
                                g.DrawEllipse(pnRedElips, arrVer[smejnost[i, 0]].X - 40, arrVer[smejnost[i, 0]].Y - 40, 40, 40);
 
                        }
                    }
                }
                offspringVer[ind, 0] = sup - 1;
                str1 = "";
                foreach (int number in q)
                {
                    str1 += " " + number.ToString() + " ";
                    g.DrawRectangle(pnBaseRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
                    labelStepString.Text = str1;
                    g.DrawRectangle(pnRedRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
                }
                str1 = "";
                if (!auto)
                {
                    button7.Visible = true;
                    button8.Visible = true;
                    button4.Visible = true;
                    linkLabel1.Visible = true;
                    break;
                }
                else
                {
                    button4.Visible = false;
                    linkLabel1.Visible = false;
                    _pause(time);
                }
            }
            if(q.Count == 0)
            {
                button7.Visible = false;
                button8.Visible = true;
                button6.Visible = false;
                button6.Text = "Пуск";
                button4.Visible = true;
                linkLabel1.Visible = true;
                button5.Visible = true;
            }
            else
            {
                button7.Visible = true;
                button8.Visible = true;
                button6.Visible = true;
                button5.Visible = false;
            }
        }           // Шаг вперёд по алгоритму 
        private void stepDown()
        {
            button5.Visible = false;
            auto = false;
            numberStep -= 1;
            labelStep.Text = numberStep + "-й шаг";
            if(numberStep == 1)
            {
                button8.Visible = false;
            }
            else
            {
                button7.Visible = true;
                button6.Visible = true;
            }
            string str1 = labelStepString.Text;
            string str2 = labelEndString.Text;
            Graphics g = CreateGraphics();
         
            str1 = "";
            int ind = 0;

            int k = 0;
            foreach (int number in ender)
            {
                ind = number;
                k++;
            }
            int[] arr = new int[30];
            arr = ender.ToArray();
            ender.Clear();
            for(int i = 0; i < k-1; i++)
            {
                ender.Enqueue(arr[i]);
            }
            str2 = "";
            foreach (int number in ender)
            {
                str2 += " " + number.ToString() + " ";
                g.DrawRectangle(pnBaseRect, labelEndString.Location.X - 1, labelEndString.Location.Y - 1, labelEndString.Width + 1, labelEndString.Height + 1);
                labelEndString.Text = str2;
                g.DrawRectangle(pnRedRect, labelEndString.Location.X - 1, labelEndString.Location.Y - 1, labelEndString.Width + 1, labelEndString.Height + 1);
            }
            if (str2.Equals(""))
            {
                g.DrawRectangle(pnBaseRect, labelEndString.Location.X - 1, labelEndString.Location.Y - 1, labelEndString.Width + 1, labelEndString.Height + 1);
                labelEndString.Text = str2;
            }
            str2 = "";
            int[] brr = new int[30];
            brr = q.ToArray();
            k = 0;
            foreach (int number in q)
            {
                k++;
            }
            q.Clear();
            q.Enqueue(ind);
            for (int i = 0; i < k - offspringVer[ind, 0]; i++)
            {
                q.Enqueue(brr[i]);
            }
            str1 = "";
            foreach (int number in q)
            {
                str1 += " " + number.ToString() + " ";
                g.DrawRectangle(pnBaseRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
                labelStepString.Text = str1;
                g.DrawRectangle(pnRedRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
            }
   
           
            
            
            for (int i = 1; i <= offspringVer[ind, 0]; i++)
            {
                g.FillEllipse(brWhite, arrVer[offspringVer[ind, i]].X - 40, arrVer[offspringVer[ind, i]].Y - 40, 40, 40);
                g.DrawEllipse(pnBlackElips, arrVer[offspringVer[ind, i]].X - 40, arrVer[offspringVer[ind, i]].Y - 40, 40, 40);
                label[offspringVer[ind, i]].BackColor = Color.White;
            }
            g.FillEllipse(brWhite, arrVer[ind].X - 40, arrVer[ind].Y - 40, 40, 40);
            g.DrawEllipse(pnRedElips, arrVer[ind].X - 40, arrVer[ind].Y - 40, 40, 40);
            label[ind].BackColor = Color.White;


        } 
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (initPoint)
            {
                base.OnMouseClick(e);
                Point pLoc = e.Location;
                bool position = true;
                bool grance = ((pLoc.X > 55) & (pLoc.X < 585) & (pLoc.Y > 55) & (pLoc.Y < 545));
                for (int i = 0; i < countVer; i++)
                {
                    if ((Math.Abs(arrVer[i].X - pLoc.X - 20) < 50) & (Math.Abs(arrVer[i].Y - pLoc.Y - 20 ) < 50))
                    {
                        position = false;
                        break;
                    }
                }
                if ((position) & (grance))
                {
                    Pen pn = new Pen(Color.Black, 3);
                    Graphics g = CreateGraphics();
                    g.DrawEllipse(pn, pLoc.X - 20, pLoc.Y - 20, 40, 40);

                    Point pointforLabel = new Point(pLoc.X - 10, pLoc.Y - 10);

                    label[countVer] = new Label();
                    label[countVer].Text = Convert.ToString(countVer);
                    label[countVer].Location = pointforLabel;
                    label[countVer].BackColor = System.Drawing.Color.Transparent;
                    label[countVer].AutoSize = true;
                    label[countVer].Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    label[countVer].Enabled = false;
                    Controls.Add(label[countVer]);

                    arrVer[countVer] = new Point(pLoc.X + 20, pLoc.Y + 20);
                    countVer++;
                }

            }
            else if (initLine)
            {
                
                base.OnMouseClick(e);
                Graphics g = CreateGraphics();
                Point pLoc = e.Location;
                for (int i = 0; i < countVer; i++)
                {
                    if ((pLoc.X >= arrVer[i].X-40) & (pLoc.X <= arrVer[i].X) & (pLoc.Y >= arrVer[i].Y - 40) & (pLoc.Y <= arrVer[i].Y))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (saveIforRight == i)
                            {
                                right = 0;
                            }
                            left = 1;
                            arrLine[countLine, 0].X = arrVer[i].X - 20;
                            arrLine[countLine, 0].Y = arrVer[i].Y - 20;
                            if (saveIforLeft != -1)
                                g.DrawEllipse(pnBlackElips, arrVer[saveIforLeft].X - 40, arrVer[saveIforLeft].Y - 40, 40, 40);
                            saveIforLeft = i;
                            g.DrawEllipse(pnBlueElips, arrVer[i].X - 40, arrVer[i].Y - 40, 40, 40);
                            break;
                        }
                        else
                        {
                            if (saveIforLeft == i)
                            {
                                left = 0;
                            }
                            right = 1;
                            arrLine[countLine, 1].X = arrVer[i].X - 20;
                            arrLine[countLine, 1].Y = arrVer[i].Y - 20;
                            if(saveIforRight != -1)
                                g.DrawEllipse(pnBlackElips, arrVer[saveIforRight].X - 40, arrVer[saveIforRight].Y - 40, 40, 40);
                            saveIforRight = i;
                            g.DrawEllipse(pnRedElips, arrVer[i].X - 40, arrVer[i].Y - 40, 40, 40);
                            break;
                        }
                    }
                }
                if ((left == 1) & (right == 1))
                {
                    double k = (Math.Sqrt(Math.Pow(Math.Abs(arrLine[countLine, 1].X - arrLine[countLine, 0].X),2) + Math.Pow(Math.Abs(arrLine[countLine, 1].Y - arrLine[countLine, 0].Y), 2)))/20;
                    int diffX = Convert.ToInt32(Math.Abs(arrLine[countLine, 1].X - arrLine[countLine, 0].X) / k);
                    int diffY = Convert.ToInt32(Math.Abs(arrLine[countLine, 1].Y - arrLine[countLine, 0].Y) / k);
                    Pen pn = new Pen(Color.Black, 3);
                    if (arrLine[countLine, 0].Y <= arrLine[countLine, 1].Y)
                    {
                        arrLine[countLine, 0].Y += diffY;
                        arrLine[countLine, 1].Y -= diffY;
                    }
                    else
                    {
                        arrLine[countLine, 0].Y -= diffY;
                        arrLine[countLine, 1].Y += diffY;
                    }
                    if(arrLine[countLine, 0].X >= arrLine[countLine, 1].X)
                    {
                        arrLine[countLine, 0].X -= diffX;
                        arrLine[countLine, 1].X += diffX;
                    }
                    else
                    {
                        arrLine[countLine, 0].X += diffX;
                        arrLine[countLine, 1].X -= diffX;
                    }
                    g.DrawLine(pn, arrLine[countLine, 0].X, arrLine[countLine, 0].Y, arrLine[countLine, 1].X, arrLine[countLine, 1].Y);
                    smejnost[countLine, 0] = saveIforLeft;
                    smejnost[countLine, 1] = saveIforRight;
                    g.DrawEllipse(pnBlackElips, arrVer[smejnost[countLine, 0]].X - 40, arrVer[smejnost[countLine, 0]].Y - 40, 40, 40);
                    g.DrawEllipse(pnBlackElips, arrVer[smejnost[countLine, 1]].X - 40, arrVer[smejnost[countLine, 1]].Y - 40, 40, 40);
                    countLine++;
                    left = 0;
                    right = 0;
                    saveIforRight = -1;
                    saveIforLeft = -1;
                }

            }
            
        }       // Реализация создания вершин и рёбер

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           for(int i = 0; i < countVer; i++)
            {
                Controls.Remove(label[i]);
            }
            countVer = 0;
            countLine = 0;

            Pen pn = new Pen(Color.White, 2);
            Pen pnW = new Pen(Color.FromArgb(255, 229, 229, 229), 1);
            Graphics g = CreateGraphics();
            SolidBrush br = new SolidBrush(Color.White);
            g.FillRectangle(br,23,23,595,555);
            g.DrawRectangle(pnW, labelEndString.Location.X - 1, labelEndString.Location.Y - 1, labelEndString.Width + 1, labelEndString.Height + 1);
            g.DrawRectangle(pnW, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
            labelEndString.Text = "";
            labelStep.Text = "";
            labelStepString.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            label6.Text = "";
            q.Clear();
            ender.Clear();
            for (int i = 0; i < level.Length; i++)
            {
                level[i] = "";
            }
            button5.Visible = false;
            button4.Visible = true;
            button6.Visible = false;
            button6.Text = "Стоп";
            button7.Visible = false;
            button8.Visible = false;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            //button4.Enabled = false;
            countVer--;
            if (!isValInRange(textBox2.Text, 0, countVer))
            {
                MessageBox.Show("Введите пожалуйста существующую вершину");
                countVer++;

            }
            else if (!isValInRange(textBox4.Text, 0, 10))
            {
                MessageBox.Show("Введите пожалуйста время от 0 до 10 сек \n(рекомендуется от 2-х до 3-х сек)");

            }
            else
            {
                countVer++;
                auto = true;
                button6.Text = "Стоп";

                for (int i = 0; i < level.Length; i++)
                {
                    level[i] = "";
                }
                label6.Text = "";
                
                q.Clear();
                ender.Clear();

                button7.Visible = false;
                button8.Visible = false;



                Graphics g = CreateGraphics();

                for (int i = 0; i < countVer; i++)
                {
                    g.FillEllipse(brWhite, arrVer[i].X - 40, arrVer[i].Y - 40, 40, 40);
                    g.DrawEllipse(pnBlackElips, arrVer[i].X - 40, arrVer[i].Y - 40, 40, 40);
                    label[i].BackColor = Color.White;

                }
                
                g.DrawRectangle(pnBaseRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);
                g.DrawRectangle(pnBaseRect, labelEndString.Location.X - 1, labelEndString.Location.Y - 1, labelEndString.Width + 1, labelEndString.Height + 1);
                numberStep = 1;

                time = Convert.ToInt32(textBox4.Text) * 1000;



                labelEndString.Text = "";
                labelStep.Text = "";
                labelStepString.Text = "";

                labelEndString.AutoSize = true;
                labelStep.AutoSize = true;
                labelStepString.AutoSize = true;


                labelStep.Text = numberStep.ToString() + "-й шаг";
                begin = Convert.ToInt32(textBox2.Text);
                q.Enqueue(begin);
                labelStepString.Text = begin.ToString();
                g.DrawRectangle(pnRedRect, labelStepString.Location.X - 1, labelStepString.Location.Y - 1, labelStepString.Width + 1, labelStepString.Height + 1);

                g.DrawEllipse(pnRedElips, arrVer[begin].X - 40, arrVer[begin].Y - 40, 40, 40);
                button6.Visible = true;
                _pause(time);
                stepUp();
                
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form4 title4 = new Form4();
            title4.Location = this.Location;
            title4.ShowDialog();
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < level.Length; j++)
            {
                level[j] = "";
            }
            string str = "";
            levels(begin, 1);
            str += "Начальная вершина - " + begin + "\n";
            int i = 1;
            while (!level[i].Equals(""))
            {
                str += "Вершины на расстоянии " + i + " от начальной:  " + level[i] + "\n";
                i++;
            }
            MessageBox.Show(str);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            auto = !auto;
            if (auto)
            {
                button6.Text = "Стоп";
                button7.Visible = false;
                button8.Visible = false;
                stepUp();
 
            }
            else
            {
                button6.Text = "Пуск";
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            stepDown();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            stepUp();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            initPoint = true;
            initLine = false;
       
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            initPoint = false;
            initLine = true;

        }


    }
}
