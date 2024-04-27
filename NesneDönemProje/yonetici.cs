using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Npgsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NesneDönemProje
{
    public partial class yonetici : Form
    {
        public yonetici()
        {
            InitializeComponent();
            LoadFilmButtons();
        }
        private void LoadFilmButtons()
        {

            // PostgreSQL bağlantı dizesi
            string connectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";

            // NpgsqlConnection oluştur
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Film verilerini çek
                string query = "SELECT film_id, film_ad , resim FROM film ";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        int buttonHeight = 30; // Buton yüksekliği


                        int currentTop = 0; // İlk butonun üstünden başlamak için
                        int butonSayac = 1;
                        int asw = 0, asd = 0;

                        while (reader.Read())
                        {
                            butonSayac++;
                            if (butonSayac % 3 == 2 && butonSayac != 2)
                            {
                                asw = 0;
                                asd += 300;

                            }
                            

                            // Her bir film için bir buton oluştur
                            Button filmButton = new Button();
                            filmButton.Width = 250;
                            filmButton.Height = 250;
                            string ad = reader["film_ad"].ToString();
                            filmButton.Cursor = Cursors.Hand;


                            

                            filmButton.Tag = reader["film_ad"].ToString(); // Film ID'yi butonun Tag özelliğine ekleyebilirsiniz.
                            filmButton.Click += FilmButton_Click; // Butona tıklandığında çalışacak olayı belirle
                            byte[] imageBytes = null;

                            // "resim" sütunu null değilse okuma işlemi gerçekleştir
                            if (!reader.IsDBNull(reader.GetOrdinal("resim")))
                            {
                                imageBytes = (byte[])reader["resim"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    Image image = Image.FromStream(ms);
                                    filmButton.BackgroundImage = image;
                                    filmButton.BackgroundImageLayout = ImageLayout.Stretch;

                                    // Şimdi image nesnesini kullanabilirsiniz
                                    // Örneğin, bir PictureBox nesnesine atayabilirsiniz

                                }
                            }
                            else
                            {
                                filmButton.Text = ad;
                            }


                            



                            filmButton.Location = new Point(asw, asd);

                            // Panel içine butonu ekle
                            panel3.Controls.Add(filmButton);

                            // Sonraki butonun üstünden başlamak için değerleri güncelle

                            asw += 300;

                        }
                    }
                }
            }
        }

        private void FilmButton_Click(object sender, EventArgs e)
        {
            // Film butonlarından birine tıklandığında çalışacak olay
            Button clickedButton = (Button)sender;                       
            // Örneğin, tıklanan filmin ID'sini alabilirsiniz
            string filmId = clickedButton.Tag.ToString();
            PanelYaz(filmId);
            // Burada tıklanan filmle ilgili diğer işlemleri gerçekleştirebilirsiniz

        }

        private string connectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";
        public void PanelYaz(string ad)
        {
            string isim = ad;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                //bağlantı sağlanması
                connection.Open();
                //sorgu oluşturulması
                string sorgu = "SELECT * FROM film WHERE film_ad = @ad ";

                using (NpgsqlCommand command = new NpgsqlCommand(sorgu, connection))
                {
                    //parametreler ekleniyor
                    command.Parameters.AddWithValue("@ad", isim);
                    panel4.Visible = true;
                    label2.Visible = false;
                    panel3.Visible = false;


                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            string film_ad = reader.GetString(reader.GetOrdinal("film_ad"));

                            string yonetmen = reader.GetString(reader.GetOrdinal("yonetmen"));

                            int süre = reader.GetInt32(reader.GetOrdinal("süre"));

                            string konu = reader.GetString(reader.GetOrdinal("konu"));

                            string oyuncular = reader.GetString(reader.GetOrdinal("oyuncular"));

                            string tür = reader.GetString(reader.GetOrdinal("tur"));
                            oyuncular = oyuncular.Trim('{', '}');


                            // "," karakterine göre ayır ve boşlukları temizle
                            string[] oyuncularr = oyuncular.Split(',').Select(oyuncu => oyuncu.Trim('\"')).ToArray();

                            DateTime yayın_yılı = reader.GetDateTime(reader.GetOrdinal("yayin_yili"));
                            string yıl;
                            string ay;
                            string gün;
                            string a = yayın_yılı.Day + "/" + yayın_yılı.Month.ToString() + "/" + yayın_yılı.Year.ToString();

                            Double puan = reader.GetDouble(reader.GetOrdinal("degerlendirme_puani"));


                            byte[] imageBytes = null;

                            // "resim" sütunu null değilse okuma işlemi gerçekleştir
                            if (!reader.IsDBNull(reader.GetOrdinal("resim")))
                            {
                                imageBytes = (byte[])reader["resim"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    Image image = Image.FromStream(ms);
                                    button2.BackgroundImage = image;
                                }
                            }


                            lbltr.Text = tür;
                            lblkonu.Text = konu;
                            lbloyun.Text = oyuncular;
                            lblsüre.Text = süre.ToString();
                            lbltarih.Text = a;
                            lblynt.Text = yonetmen;
                            lblımd.Text = puan.ToString();
                            label4.Text = film_ad.ToUpper();
                            



                        }

                    }
                }
                string sorgum = "SELECT * FROM degerlendirme_tablosu WHERE film_ad = @ad ";

                using (NpgsqlCommand command = new NpgsqlCommand(sorgum, connection))
                {
                    //parametreler ekleniyor
                    command.Parameters.AddWithValue("@ad", isim);
                    panel4.Visible = true;
                    label2.Visible = false;
                    panel3.Visible = false;


                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        int buttonHeight = 30; // Buton yüksekliği


                        int currentTop = 0; // İlk butonun üstünden başlamak için

                        int asw = 500, asd = 0;

                        while (reader.Read())
                        {

                            // Her bir film için bir buton oluştur
                            Label label = new Label();

                            label.Width = 500;
                            label.Height = 100;
                            label.Font = new Font("Rockwell", 10, FontStyle.Regular);
                            label.BorderStyle = BorderStyle.FixedSingle;
                            label.Padding = new Padding(5); // İçerideki metni çerçeveden ayır
                                                            // 
                            string kisi = reader.GetString(reader.GetOrdinal("kullanici_ad_soyad"));
                            string yorum = reader.GetString(reader.GetOrdinal("yorum"));
                            label.Text = kisi + "\n" + yorum;



                            label.Location = new Point(15, asw);

                            // Panel içine butonu ekle
                            panel4.Controls.Add(label);

                            // Sonraki butonun üstünden başlamak için değerleri güncelle
                            asw += 157;
                        }

                    }
                }


                
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            button8.Visible = true;
            button7.Enabled = false;
            


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        bool move;
        int mouse_x;
        int mouse_y;

        private void yonetici_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void yonetici_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void yonetici_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel5.Visible= true;
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string film = label4.Text.ToLower();
            NpgsqlConnection baglama = new NpgsqlConnection("server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234");
            if (textBox2.Text == "")
            {
                MessageBox.Show("lütfen bir deger giriniz");
            }
            else
            {
                if (comboBox1.Text == "yönetmen")
                {
                    baglama.Open();
                    // sql syntax ı nedeniyle stringleri parçalamak ve üstünde oynamak gerekti.
                    string deger = textBox2.Text;
                    
                    NpgsqlCommand komut = new NpgsqlCommand("update film  set  yonetmen=@p1 where film_ad Like '" + film + "'", baglama);
                    komut.Parameters.AddWithValue("@p1", deger);
                    komut.ExecuteNonQuery();
                    
                    
                    MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    lblynt.Text = deger;
                    

                    baglama.Close();
                }
                if (comboBox1.Text == "konu")
                {
                    baglama.Open();
                    string deger = textBox2.Text;
                    NpgsqlCommand komut = new NpgsqlCommand("update film set  konu = @p1 where film_ad Like '" + film + "'", baglama);

                    komut.Parameters.AddWithValue("@p1", deger);
                    komut.ExecuteNonQuery();

                    MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    lblkonu.Text = deger;
                    

                    baglama.Close();
                }
                

            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            yonetici dene = new yonetici();
            dene.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NpgsqlConnection baglam = new NpgsqlConnection("server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234");
            string film = label4.Text.ToLower();
            baglam.Open();
            // sql syntax ı nedeniyle stringleri parçalamak ve üstünde oynamak gerekti.
            NpgsqlCommand kmt = new NpgsqlCommand("delete from  film where film_ad Like '" + film + "'", baglam);

            kmt.ExecuteNonQuery();

            MessageBox.Show("Bilgiler başarıyla silindi bizi tercih ettiğiniz için teşekkürler...");
            yonetici form = new yonetici();
            form.Show();
            this.Close();


            baglam.Close();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (label9.Text == "" || label8.Text == "" || label4.Text == "" || maskedTextBox1.Text == "" || maskedTextBox2.Text == "" || textBox7.Text == "") 
            {
                MessageBox.Show("lütfen bilgileri kontrol edin");
            }
            else
            {
                string connectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";
                
                
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Film eklemek için SQL sorgusu
                    string insertQuery = "INSERT INTO film (film_ad, yonetmen,oyuncular,tur,yayin_yili,degerlendirme_puani,resim,süre,konu) VALUES (@filmAd, @yonetmen,@oyuncular,@tur,@yayin_yili,@degerlendirme_puani,@resim,@süre,@konu)";

                    // SQL komut nesnesi oluştur
                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Parametreleri ekle
                        DateTime yayinYili = DateTime.Parse(maskedTextBox2.Text);

                        // PostgreSQL parametresine ekle
                        command.Parameters.AddWithValue("@yayin_yili", yayinYili);
                        command.Parameters.AddWithValue("@filmAd", textBox9.Text.ToLower());
                        command.Parameters.AddWithValue("@yonetmen", textBox8.Text.ToLower());
                        command.Parameters.AddWithValue("@oyuncular", textBox1.Text.ToLower());
                        command.Parameters.AddWithValue("@tur", textBox5.Text.ToLower());
                        
                        command.Parameters.AddWithValue("@degerlendirme_puani", numericUpDown1.Value);
                        command.Parameters.Add(new NpgsqlParameter("@resim", NpgsqlTypes.NpgsqlDbType.Bytea)).Value = DBNull.Value;
                        command.Parameters.AddWithValue("@süre", Convert.ToInt32(maskedTextBox1.Text));
                        command.Parameters.AddWithValue("@konu", textBox7.Text);


                        // Komutu çalıştır
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("İzleme listesine eklendi..");
                    
                }
            }


        }

        private void button15_Click(object sender, EventArgs e)
        {
            yonetici dene = new yonetici();
            dene.Show();
            this.Hide();
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
