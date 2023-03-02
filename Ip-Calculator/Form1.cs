using System;
using System.Windows.Forms;

namespace ip_calc_by_DAR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            string data = "10.10.10.10/25";

            data = textBox1.Text;   //входные данные
            if(data.Length > 18)    //проверка на количество исходных данных
            {
                textBox2.Text += "Слишком много данных, что то пошло не по плану!";
                return;
            }
            int[] ip_int = { 0, 0, 0, 0, 0 };   //для числовых представлений данных

            string tmp = "";                    //вспомогательная строка
            int j = 0; int i = 0;

            short sig_check_d = 0;              //проверка формата
            short sig_check_s = 0;
            for (j = 0; j < data.Length; j++)
            {
                if (data[j] == '.')
                    sig_check_d++;
                else if (data[j] == '/')
                    sig_check_s++;
            }
            if (sig_check_d != 3 && sig_check_s != '/')
            {
                textBox2.Text += "Нет, это так не работает, попробуйте как-то так: 10.10.10.10/25";
                return;
            }

            for (j = 0; j < 4; j++)   //заполнение четырёх байтов
            {
                while (true)
                {
                    if (data[i] == '.' || data[i] == '/')
                        break;
                    tmp += data[i];
                    i++;
                }
                i++;
                try
                {
                    ip_int[j] = Int32.Parse(tmp);
                }
                catch(Exception)
                {
                    textBox2.Text += "Вы ввели некорректные данные, не надо так!";
                    return;
                }
                tmp = "";
                
                if(ip_int[j] > 255)   //проверка на величину байта
                {
                    textBox2.Text += "Не надо вводить числа больше 255, мы такое осуждаем!";
                    return;
                }
            }
            while (i < data.Length)   //заполнение маски
            {
                tmp += data[i];
                i++;
            }
            ip_int[4] = Int32.Parse(tmp);

            if (ip_int[4] > 32)   //проверка на величину маски
            {
                textBox2.Text += "Маска не может быть больше 32, переделывайте!";
                return;
            }

            int mask = ip_int[4];
            int[] m = { 0, 0, 0, 0 };
            for (i = 0; i < 4; i++)
            {
                if (mask >= 8)
                    m[i] = 255;
                else
                {
                    switch (mask)
                    {
                        case 7:
                            m[i] = 254;
                            break;
                        case 6:
                            m[i] = 252;
                            break;
                        case 5:
                            m[i] = 248;
                            break;
                        case 4:
                            m[i] = 240;
                            break;
                        case 3:
                            m[i] = 224;
                            break;
                        case 2:
                            m[i] = 192;
                            break;
                        case 1:
                            m[i] = 128;
                            break;
                    }
                }
                mask = mask - 8;
            }

            
            textBox2.Text += "Mask: ";
            //вывод данных
            for (j = 0; j < 4; j++)
            {
                tmp = Convert.ToString(m[j]);
                textBox2.Text += tmp;
                if (j != 3)
                    textBox2.Text += ".";
                else
                    textBox2.Text += "";
            }
            textBox2.Text += "                                                                                                                                                    ";
            
            textBox2.Text += "Network: ";
            //расчёты Network
            int[] net = { 0, 0, 0, 0 };
            for (i = 0; i < 4; i++)
            {
                net[i] = ip_int[i] & m[i];
            }
            //вывод данных
            for (j = 0; j < 4; j++)
            {
                tmp = Convert.ToString(net[j]);
                textBox2.Text += tmp;
                if (j != 3)
                    textBox2.Text += ".";
                else
                    textBox2.Text += "";
            }
            textBox2.Text += "                                                                                                                                                    ";

            textBox2.Text += "Broadcast: ";
            //расчёты Broadcast
            int[] broadcast = { 0, 0, 0, 0 };
            for (i = 0; i < 4; i++)
                broadcast[i] = 256 + (ip_int[i] | ~m[i]);
            //вывод данных
            for (j = 0; j < 4; j++)
            {
                tmp = Convert.ToString(broadcast[j]);
                textBox2.Text += tmp;
                if (j != 3)
                    textBox2.Text += ".";
                else
                    textBox2.Text += "";
            }
            textBox2.Text += "                                                                                                                                               ";

            textBox2.Text += "Hosts: ";
            //расчёты Hosts
            double hosts_count = Math.Pow(2, (32 - ip_int[4]));
            //вывод данных
            tmp = Convert.ToString(hosts_count);
            textBox2.Text += tmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}