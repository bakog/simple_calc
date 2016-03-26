using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace simple_calc
{
    public partial class Form1 : Form
    {
        static string akt_gomb, elozo_gomb;
        static List<string> muv_tagok = new List<string>();
        static string akt_szam;
        public int next_muv()
        {
            int index1, index2;
            if (muv_tagok.Contains("*") && muv_tagok.Contains("/"))
            {
                index1 = muv_tagok.IndexOf("*");
                index2 = muv_tagok.IndexOf("/");
                if (index1 < index2) return index1;
                else return index2;
            }
            else if (muv_tagok.Contains("*"))
            {
                index1 = muv_tagok.IndexOf("*");
                return index1;
            }
            else if (muv_tagok.Contains("/"))
            {
                index1 = muv_tagok.IndexOf("/");
                return index1;
            }
            else if (muv_tagok.Contains("+") && muv_tagok.Contains("-"))
            {
                index1 = muv_tagok.IndexOf("+");
                index2 = muv_tagok.IndexOf("-");
                if (index1 < index2) return index1;
                else return index2;
            }
            else if (muv_tagok.Contains("+"))
            {
                index1 = muv_tagok.IndexOf("+");
                return index1;
            }
            else if (muv_tagok.Contains("-"))
            {
                index1 = muv_tagok.IndexOf("-");
                return index1;
            }
            else
            {
                return 0;
            }
        }
        public void tarol(string s)
        {
            if (akt_gomb == null || akt_gomb == "") //első bevitel
            {
                if (s != "+" && s != "-" && s != "/" && s != "*" && s != "=") //az első bevitel szám
                {
                    akt_szam += s;
                    akt_gomb = s;
                    textBox1.Text += s;
                }
                else if (s == "-") //az első bevitel - (negatív számmal kezdünk)
                {
                    akt_szam += s;
                    akt_gomb = s;
                    textBox1.Text += s;
                }
            }
            //2. bevitel az előző bevitel - volt
            else if ((elozo_gomb == null || elozo_gomb == "") && akt_gomb == "-")
            {
                if (s != "+" && s != "-" && s != "/" && s != "*" && s != "=") //a bevitel negatív szám számjegye
                {
                    elozo_gomb = akt_gomb;
                    akt_szam += s;
                    akt_gomb = s;
                    textBox1.Text += s;
                }
            }
            //2. bevitel az előző bevitel számjegy volt
            else if ((elozo_gomb == null || elozo_gomb == "") && (akt_gomb != "+" && akt_gomb != "-" && akt_gomb != "/" && akt_gomb != "*" && akt_gomb != "="))
            {
                if (s == "-" || s == "+" || s == "/" || s == "*" || s == "=") //a bevitel műveleti jel lesz
                {
                    muv_tagok.Add(akt_szam); // letároljuk az előzőleg bevitt számot 
                    muv_tagok.Add(s); // letároljuk a műveleti jelet
                                      //MessageBox.Show("letároljuk: akt_szam:"+akt_szam+" muveleti_jel: "+s);
                    elozo_gomb = akt_gomb;
                    akt_szam = "";
                    akt_gomb = s;
                    textBox1.Text += s;
                }
                else if (s != "-" && s != "+" && s != "/" && s != "*" && s != "=") // a bevitel számjegy lesz, folytatjuk a számot
                {
                    elozo_gomb = akt_gomb;
                    akt_gomb = s;
                    akt_szam += s;
                    textBox1.Text += s;
                }
            }
            //3. bevitel az előző bevitel számjegy volt
            else if ((elozo_gomb != null && elozo_gomb != "") && (akt_gomb != "+" && akt_gomb != "-" && akt_gomb != "/" && akt_gomb != "*" && akt_gomb != "="))
            {
                if (s != "-" && s != "+" && s != "/" && s != "*" && s != "=") // a bevitel számjegy lesz, folytatjuk a számot
                {
                    elozo_gomb = akt_gomb;
                    akt_gomb = s;
                    akt_szam += s;
                    textBox1.Text += s;
                }
                // a bevitel műveleti jel
                else if (s == "-" || s == "+" || s == "/" || s == "*" || s == "=")
                {
                    muv_tagok.Add(akt_szam); // letároljuk az előzőleg bevitt számot 
                    muv_tagok.Add(s); // letároljuk a műveleti jelet
                                      //MessageBox.Show("3szam --- letároljuk: akt_szam:" + akt_szam + " muveleti_jel: " + s);
                    elozo_gomb = akt_gomb;
                    akt_gomb = s;
                    akt_szam = "";
                    textBox1.Text += s;
                }
            }
            // 3. bevitel műveleti jel volt 
            else if ((elozo_gomb != null && elozo_gomb != "") && (akt_gomb == "+" || akt_gomb == "-" || akt_gomb == "/" || akt_gomb == "*" || akt_gomb != "="))
            {
                if (s != "+" && s != "-" && s != "/" && s != "*" && s != "=") //a bevitel szám (pozitív szám a kivonandó)
                {
                    elozo_gomb = akt_gomb;
                    akt_szam += s;
                    akt_gomb = s;
                    textBox1.Text += s;
                }
                else if (s == "-") //a bevitel - (negatív szám lesz majd a kivonandó)
                {
                    elozo_gomb = akt_gomb;
                    akt_szam += s;
                    akt_gomb = s;
                    textBox1.Text += s;
                }
            }

        }



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            tarol("6");

        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            elozo_gomb = "";
            akt_gomb = "";
            akt_szam = "";
            muv_tagok.Clear();
            label1.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //textBox1.Text = "1";
            tarol("1");

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            tarol("2");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tarol("3");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tarol("4");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            tarol("5");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            tarol("7");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            tarol("8");

        }

        private void button9_Click(object sender, EventArgs e)
        {
            tarol("9");

        }

        private void button10_Click(object sender, EventArgs e)
        {
            tarol("0");

        }

        private void button13_Click(object sender, EventArgs e)
        {
            tarol("+");

        }

        private void button15_Click(object sender, EventArgs e)
        {
            tarol("-");

        }

        private void button11_Click(object sender, EventArgs e)
        {
            tarol("*");

        }

        private void button14_Click(object sender, EventArgs e)
        {
            tarol("/");

        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                label1.Text = "Műveleti tagok listája: ";

                tarol("=");
                // a kiszamolás elindítása
                foreach (string tag in muv_tagok)
                {
                    label1.Text += tag + ", ";
                }
                label2.Text = "Az első művelet indexe: "+Convert.ToString (next_muv());
            }


        }
    }
}
