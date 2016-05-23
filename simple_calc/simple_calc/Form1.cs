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
        static bool error; 

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

        public void kiszamol()
        {
            //itt végezzük a kiszámolást, ha egyenlőségjelet nyomott és a textbox1.text nem üres
            //az utolsó tárolt műveleti jel =, ezt töröljük
            muv_tagok.RemoveAt(muv_tagok.Count() - 1);

            //bejárjuk a muv_tagok listát és megkeressük, az éppen aktuális műveletet, amíg hibát nem kapunk vissza, vagy el nem fogy a lista
            error = false;
            while (next_muv() != 0 && !error)
            {
                szamol(next_muv() - 1);
            }
            if (!error)
            {
                //nincs hiba, az eredmény kiírása
                textBox1.Text = muv_tagok[0];
                akt_szam = textBox1.Text;
                akt_gomb = muv_tagok[0].Substring(muv_tagok[0].Length - 1, 1); //a szövegdoboz utolsó karaktere
                muv_tagok.Clear();
            }
            else
            {
                //hibaüzenet megjelenítése
                label1.Text = "Érvénytelen művelet!";
            }
        }
        public bool szamol(int i)
        {
            //ez a függvény végzi a számolást: kivesz 3 elemet a listából, majd a művelet elvégzése után törli azokat 
            // és i helyére visszaírja az eredményt (ez lesz e következő művelet egyik tagja, vagy a végeredmény
            int range = 3;
            double eredmeny;
            string a, muvelet, b;
            double szam1, szam2;
            //három elem kivétele a muv_tagok listából
            a = muv_tagok[i];
            muvelet = muv_tagok[i + 1];
            b = muv_tagok[i + 2];

            szam1 = Convert.ToDouble(a);
            szam2 = Convert.ToDouble(b);
            switch (muvelet)
            {
                case "+": eredmeny = szam1 + szam2; break;
                case "-": eredmeny = szam1 - szam2; break;
                case "/": if (szam2 == 0) { error = true; eredmeny = 0; } else { eredmeny = szam1 / szam2; } break;
                case "*": eredmeny = szam1 * szam2; break;
                default: error = true; eredmeny = 0; break;
            }
            if (!error)
            {
                //a 3 elem törlése a listából és az eredmény visszaírása a lista i. helyére
                muv_tagok.RemoveRange(i, range);
                muv_tagok.Insert(i, Convert.ToString(eredmeny));
                return error;
            }
            else
            {
                // hiba volt a feldolgozás során
                return error = true;
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
                kiszamol();
            }


        }
    }
}
