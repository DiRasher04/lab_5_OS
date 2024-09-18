using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_5_OS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = N.ToString();
            textBox2.Text = b.ToString();
            textBox3.Text = d.ToString();
            textBox4.Text = m.ToString();
            textBox5.Text = v.ToString();
            textBox6.Text = k.ToString();
            textBox7.Text = l.ToString();
        }

        int N = 42, 
            k = 266, 
            l = 95, 
            m = 277, 
            v = 42, 
            b = 624, 
            d = 35;

        //Объявляем блокирующие переменные глобально
        bool cpu = true;
        bool hdd = true;
        bool lan = true;
        //Время занятости устройств
        int cpu_busy_time = 0;
        int hdd_busy_time = 0;
        int lan_busy_time = 0;
        //Время бехдействия устройств
        int cpu_stop_time = 0;
        int hdd_stop_time = 0;
        int lan_stop_time = 0;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 10;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 100;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int cpu_current = 0;
            int hdd_current = 0;
            int lan_current = 0;
            
            //Если свободен, ищем для него процесс
            if (cpu)
            {
                for(int i = 0; i < N; i++)
                {
                    if (mas_task[i].cpu_time > 0) 
                    {
                        cpu = false; //Занимает устройство
                        cpu_current = i;
                        break;
                    }
                }
            }
            //Если ЦП занят, уменьшаем время обработки
            if(!cpu && mas_task[cpu_current].cpu_time > 0)
            {
                mas_task[cpu_current].cpu_time --;
                cpu_busy_time++;
            }

            //Если процесс выполнился на ЦП - освобождаем его
            if (!cpu && mas_task[cpu_current].cpu_time <= 0)
            {
                cpu = true;
            }

            //Если свободен, ищем для него процесс
            if (hdd && mas_task[cpu_current].cpu_time == 0)
            {
                for (int i = 0; i < N; i++)
                {
                    if (mas_task[i].hdd_time > 0)
                    {
                        hdd = false; //Занимает устройство
                        hdd_current = i;
                        break;
                    }
                }
            }
            //Если Hdd занят, уменьшаем время обработки
            if (!hdd && mas_task[hdd_current].hdd_time > 0)
            {
                mas_task[hdd_current].hdd_time--;
                hdd_busy_time++;
                hdd_stop_time = cpu_busy_time;
            }

            //Если процесс выполнился на Hdd - освобождаем его
            if (!hdd && mas_task[hdd_current].hdd_time <= 0)
            {
                hdd = true;
            }

            //Если свободен, ищем для него процесс
            if (lan && mas_task[hdd_current].hdd_time == 0)
            {
                for (int i = 0; i < N; i++)
                {
                    if (mas_task[i].lan_time > 0)
                    {
                        lan = false; //Занимает устройство
                        lan_current = i;
                        break;
                    }
                }
            }
            //Если lan занят, уменьшаем время обработки
            if (!lan && mas_task[lan_current].lan_time > 0)
            {
                mas_task[lan_current].lan_time--;
                lan_busy_time++;
            }

            //Если процесс выполнился на lan - освобождаем его
            if (!lan && mas_task[lan_current].lan_time <= 0)
            {
                lan = true;
            }
            //Выводим выполненный масив на форму
            richTextBox2.Clear();
            for(int i = 0;i < N;i++)
            {
                if (mas_task[i].id < 10)
                {
                    richTextBox2.Text += "0" + mas_task[i].id.ToString() + " " + mas_task[i].cpu_time.ToString() + " " + mas_task[i].hdd_time.ToString() + " " + mas_task[i].lan_time.ToString() + "\n";
                    richTextBox1.Text = "Выполнено процессором: " + cpu_busy_time.ToString() + "\nВыполнено диском: " + hdd_busy_time.ToString() + "\nВыполнено сетью: " + lan_busy_time.ToString() + "\nОжидание процессора: " + hdd_stop_time.ToString() + "\nОжидание диска: " + hdd_stop_time.ToString() + "\nОжидание сети: " + hdd_stop_time.ToString();
                }
                else
                {
                    richTextBox2.Text += mas_task[i].id.ToString() + " " + mas_task[i].cpu_time.ToString() + " " + mas_task[i].hdd_time.ToString() + " " + mas_task[i].lan_time.ToString() + "\n";
                    richTextBox1.Text = "Выполнено процессором: " + cpu_busy_time.ToString() + "\nВыполнено диском: " + hdd_busy_time.ToString() + "\nВыполнено сетью: " + lan_busy_time.ToString() + "\nОжидание процессора: " + hdd_stop_time.ToString() + "\nОжидание диска: " + hdd_stop_time.ToString() + "\nОжидание сети: " + hdd_stop_time.ToString();
                }
            }
        }

        public struct task
        {
            public int id;
            public int cpu_time;
            public int hdd_time;
            public int lan_time;
        }
        task[] mas_task;
        private void button3_Click(object sender, EventArgs e)//Сгенерировать список задач
        {
            //Генерируем время обработки
            Random r = new Random();
            mas_task = new task[N];
            for (int i = 0; i < N; i++)
            {
                mas_task[i].id = i + 1;
                mas_task[i].cpu_time = k - l + r.Next(l * 2);
                mas_task[i].hdd_time = m - v + r.Next(v * 2);
                mas_task[i].lan_time = b - d + r.Next(d * 2);
            }
            //Выводим на форму
            richTextBox3.Clear();
            for (int i = 0; i < N; i++)
            {
                if(i < 9)
                {
                    richTextBox3.Text += "0" + mas_task[i].id.ToString() + " " + mas_task[i].cpu_time.ToString() + " " + mas_task[i].hdd_time.ToString() + " " + mas_task[i].lan_time.ToString() + "\n";
                }
                else
                {
                    richTextBox3.Text += mas_task[i].id.ToString() + " " + mas_task[i].cpu_time.ToString() + " " + mas_task[i].hdd_time.ToString() + " " + mas_task[i].lan_time.ToString() + "\n";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//Отсортировать список задач
        {
            //Сортируем список задач по загрузке ЦП
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    if (mas_task[i].cpu_time < mas_task[j].cpu_time)
                    {
                        task temp = mas_task[i];
                        mas_task[i] = mas_task[j];
                        mas_task[j] = temp;
                    }
                }
            }
            //Выводим отсортированный на форму
            richTextBox2.Clear();
            for (int i = 0; i < N; i++)
            {
                if (mas_task[i].id < 10)
                {
                    richTextBox2.Text += "0" + mas_task[i].id.ToString() + " " + mas_task[i].cpu_time.ToString() + " " + mas_task[i].hdd_time.ToString() + " " + mas_task[i].lan_time.ToString() + "\n";
                }
                else
                {
                    richTextBox2.Text += mas_task[i].id.ToString() + " " + mas_task[i].cpu_time.ToString() + " " + mas_task[i].hdd_time.ToString() + " " + mas_task[i].lan_time.ToString() + "\n";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)//Смоделировать выполнение
        {
            timer1.Enabled = true;
        }
    }
}
