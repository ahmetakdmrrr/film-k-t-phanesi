using System;
using Npgsql;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234");

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            button3.Visible = true;
        }
        private const string ConnectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";
       
        

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            button3.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //kullanıcı adı ve şifre bilgileri alınıyor.
            string kulanici_adi = textBox1.Text;
            string Sifre = textBox2.Text;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                //bağlantı sağlanması
                connection.Open();
                //sorgu oluşturulması
                string sorgu = "SELECT * FROM kullanici WHERE kullanici_adi = @kullaniciAdi AND sifre = @sifre";

                using (NpgsqlCommand command = new NpgsqlCommand(sorgu, connection))
                {
                    //parametreler ekleniyor
                    command.Parameters.AddWithValue("@kullaniciAdi", kulanici_adi);
                    command.Parameters.AddWithValue("@sifre", Sifre);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string üyelik_türü = reader.GetString(reader.GetOrdinal("uyelik_turu"));

                            if (üyelik_türü == "Premium")
                            {
                                Premium Premium = new Premium(reader.GetString(reader.GetOrdinal("ad_soyad")), üyelik_türü, reader.GetDateTime(reader.GetOrdinal("dogum_tarihi")), reader.GetString(reader.GetOrdinal("kullanici_adi")), reader.GetString(reader.GetOrdinal("sifre")), reader.GetString(reader.GetOrdinal("tc_kimlik")), reader.GetString(reader.GetOrdinal("cinsiyet")));

                                AnaSayfa sayfa = new AnaSayfa(Premium);
                                this.Hide();
                                sayfa.Show();
                            }
                            else
                            {
                                Standart standart = new Standart(reader.GetString(reader.GetOrdinal("ad_soyad")), üyelik_türü, reader.GetDateTime(reader.GetOrdinal("dogum_tarihi")), reader.GetString(reader.GetOrdinal("kullanici_adi")), reader.GetString(reader.GetOrdinal("sifre")), reader.GetString(reader.GetOrdinal("tc_kimlik")), reader.GetString(reader.GetOrdinal("cinsiyet")));

                                AnaSayfa sayfa = new AnaSayfa(standart);
                                this.Hide();
                                sayfa.Show();

                            }

                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bulunamadı veya şifre hatalı.");
                        }
                    }
                }
            }
        }

        bool move;
        int mouse_x;
        int mouse_y;
        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Y_giris giris = new Y_giris();
            giris.Show();
            this.Hide();
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            label5.ForeColor = Color.White;

        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Maroon;
        }
    }
}
