using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NesneDönemProje
{
    public partial class Y_giris : Form
    {
        public Y_giris()
        {
            InitializeComponent();
        }

        private void Y_giris_Load(object sender, EventArgs e)
        {

        }

        

        private void label5_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }


        bool move;
        int mouse_x;
        int mouse_y;
        private void Y_giris_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }
        private void Y_giris_MouseUp_1(object sender, MouseEventArgs e)
        {
            move = false;
        }
        private void Y_giris_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string k_adi = "A";
            string sifre = "2";
            if(textBox1.Text ==k_adi && textBox2.Text == sifre)
            {
                yonetici yonetici = new yonetici();
                yonetici.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("giriş bilgilerinde hata var");
            }





        }
    }
}
