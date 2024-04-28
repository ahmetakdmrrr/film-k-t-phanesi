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
    public partial class AnaSayfa : Form
    {
        private kullanıcı AktifKullanıcı; // kullanıcı sınıfınızın örneği
        public AnaSayfa(kullanıcı kullanıcı)
        {

            AktifKullanıcı = kullanıcı;
            InitializeComponent();
            LoadFilmButtons();
            notifyIcon1.Icon = SystemIcons.Information;
            notifyIcon1.BalloonTipText = "Yepyeni filmler eklendi haydi aç ve izle!";
            notifyIcon1.BalloonTipTitle = "Yeni Filmler Eklendi!!!!";
            notifyIcon1.ShowBalloonTip(2000);
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
                            int asw = 0,asd=0;

                        while (reader.Read())
                        {
                            butonSayac++;
                            if (butonSayac%3 == 2 && butonSayac !=2)
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
                                }
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
        private const string ConnectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";
       

        public void PanelYaz(string ad)
        {
            string isim = ad;
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
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
                            else
                            {
                                button2.Text = ad;
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
                        
                        int asw =700, asd = 0;
                        
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


                string sorgumm = "select ROUND(avg(degerlendirme_puani),2) from  degerlendirme_tablosu where film_ad = @ad;";
                
                using (NpgsqlCommand command = new NpgsqlCommand(sorgumm, connection))
                    {
                        command.Parameters.AddWithValue("@ad", isim);

                        // ExecuteScalar kullanarak tek bir değeri alın
                        object result = command.ExecuteScalar();

                        // Eğer sonuç NULL değilse ve bir değer varsa, bu değeri kullanabilirsiniz
                        if (result != null && result != DBNull.Value)
                        {
                            double ortalamaPuan = Convert.ToDouble(result);
                        // Ortalama puanı kullanmak için ortalamaPuan değişkenini kullanabilirsiniz
                        lblımd.Text = ((Convert.ToDouble(lblımd.Text) + ortalamaPuan) / 2).ToString();
                        }
                        else
                        {
                            // Sonuç NULL veya değersizse, bir işlem yapabilirsiniz
                            Console.WriteLine("Ortalama Puan bulunamadı");
                        }
                    }              
            }

            



        }
        private void FilmButton_Click(object sender, EventArgs e)
        {
            // Film butonlarından birine tıklandığında çalışacak olay
            Button clickedButton = (Button)sender;
            panel4.BringToFront();
            label19.Visible = button10.Visible = button11.Visible = textBox3.Visible = false;
            if(AktifKullanıcı.uyelik_türü == "Premium")
            {
                label22.Visible = numericUpDown1.Visible = true;
            }
            panel3.Visible = true;
            

            // Örneğin, tıklanan filmin ID'sini alabilirsiniz
            string filmId = clickedButton.Tag.ToString();
            PanelYaz(filmId);
            



            // Burada tıklanan filmle ilgili diğer işlemleri gerçekleştirebilirsiniz

        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {

        }
        bool move;
        int mouse_x;
        int mouse_y;


        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }
        private void HeaderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;

        }

        private void HeaderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }

        }
        private void AnaSayfa_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;

        }
        private void AnaSayfa_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void AnaSayfa_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }
        }

        

        

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            deneme deneme = new deneme();
            deneme.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            label19.Visible = button10.Visible = button11.Visible = textBox3.Visible = true;
            AnaSayfa dene = new AnaSayfa(AktifKullanıcı);
            dene.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {


            if(AktifKullanıcı.uyelik_türü =="Premium")
            {
                if (textBox1.Text == "" && numericUpDown1.Value == 0)
                {
                    MessageBox.Show("boşlukarı doldur!!");
                }
                else
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                    {
                        conn.Open();
                        string k_Ad_soyad = AktifKullanıcı.ad_soyad;
                        string tc = AktifKullanıcı.tc_kimlik;
                        string film_ad = label4.Text.ToLower();
                        string yorum = textBox1.Text;
                        // Veritabanına eklenecek değerler


                        // PostgreSQL sorgusunu oluştur
                        string insertQuery = "INSERT INTO degerlendirme_tablosu (film_ad,kullanici_ad_soyad,degerlendirme_puani,tc_kimlik,yorum) VALUES (@film_ad,@kullaniciAdi,@degerlendirmepuani,@tc_kimlik,@yorum)";

                        // PostgreSQL komutunu oluştur
                        using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
                        {
                            // Parametreleri ekleyin
                            cmd.Parameters.AddWithValue("@film_ad", film_ad);
                            cmd.Parameters.AddWithValue("@kullaniciAdi", k_Ad_soyad);
                            cmd.Parameters.AddWithValue("@tc_kimlik", tc);
                            cmd.Parameters.AddWithValue("@yorum", yorum);
                            cmd.Parameters.AddWithValue("@degerlendirmepuani", numericUpDown1.Value);

                            // Komutu yürütün
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("yorumunuz başarıyla eklendi...");
                        textBox1.Text = "";
                        numericUpDown1.Value = 0;

                    }
                }

            }
            else
            {
                if (textBox1.Text == "" )
                {
                    MessageBox.Show("boşlukarı doldur!!");
                }
                else
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                    {
                        conn.Open();
                        string k_Ad_soyad = AktifKullanıcı.ad_soyad;
                        string tc = AktifKullanıcı.tc_kimlik;
                        string film_ad = label4.Text.ToLower();
                        string yorum = textBox1.Text;
                        // Veritabanına eklenecek değerler


                        // PostgreSQL sorgusunu oluştur
                        string insertQuery = "INSERT INTO degerlendirme_tablosu (film_ad,kullanici_ad_soyad,degerlendirme_puani,tc_kimlik,yorum) VALUES (@film_ad,@kullaniciAdi,@degerlendirmepuani,@tc_kimlik,@yorum)";

                        // PostgreSQL komutunu oluştur
                        using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
                        {
                            // Parametreleri ekleyin
                            cmd.Parameters.AddWithValue("@film_ad", film_ad);
                            cmd.Parameters.AddWithValue("@kullaniciAdi", k_Ad_soyad);
                            cmd.Parameters.AddWithValue("@tc_kimlik", tc);
                            cmd.Parameters.AddWithValue("@yorum", yorum);
                            cmd.Parameters.Add("@degerlendirmepuani", NpgsqlTypes.NpgsqlDbType.Double).Value = DBNull.Value;


                            // Komutu yürütün
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("yorumunuz başarıyla eklendi...");
                        textBox1.Text = "";
                        numericUpDown1.Value = 0;

                    }
                }

            }
            
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label19.Visible =button10.Visible = button11.Visible =textBox3.Visible =  false;

            panel5.BringToFront();
            
            

            panel5.Visible = true;
            string a = AktifKullanıcı.dogum_tarihi.Day.ToString() + "/" + AktifKullanıcı.dogum_tarihi.Month.ToString() + "/" + AktifKullanıcı.dogum_tarihi.Year.ToString();
            lblism.Text = AktifKullanıcı.ad_soyad;
            lblka.Text = AktifKullanıcı.kullanici_adi;
            lblkn.Text = AktifKullanıcı.tc_kimlik;
            lblth.Text = a;
            label21.Text = AktifKullanıcı.uyelik_türü;
            lblc.Text = AktifKullanıcı.cinsiyet;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label19.Visible = button10.Visible = button11.Visible = textBox3.Visible = true;
            panel5.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        NpgsqlConnection baglama = new NpgsqlConnection("server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234");
        private void button7_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == "")
            {
                MessageBox.Show("lütfen bir deger giriniz");
            }
            else
            {
                if (comboBox1.Text == "ad soyad")
                {
                    baglama.Open();
                    // sql syntax ı nedeniyle stringleri parçalamak ve üstünde oynamak gerekti.
                    string deger = textBox2.Text;
                    NpgsqlCommand komut = new NpgsqlCommand("update kullanici set ad_soyad =@p1 where tc_kimlik Like '" + AktifKullanıcı.tc_kimlik + "'", baglama);
                    NpgsqlCommand komutt = new NpgsqlCommand("update degerlendirme_tablosu set kullanici_ad_soyad =@p1 where tc_kimlik Like '" + AktifKullanıcı.tc_kimlik + "'", baglama);
                    komut.Parameters.AddWithValue("@p1", deger);
                    komut.ExecuteNonQuery();
                    komutt.Parameters.AddWithValue("@p1", deger);
                    komutt.ExecuteNonQuery();
                    MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    lblism.Text = deger;
                    AktifKullanıcı.ad_soyad = deger;

                    baglama.Close();
                }
                if (comboBox1.Text == "kullanıcı adı")
                {
                    baglama.Open();
                    string deger = textBox2.Text;
                    NpgsqlCommand komut = new NpgsqlCommand("update kullanici set kullanici_adi =@p1 where tc_kimlik Like '" + AktifKullanıcı.tc_kimlik + "'", baglama);
                    
                    komut.Parameters.AddWithValue("@p1", deger);
                    komut.ExecuteNonQuery();
                    
                    MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    lblka.Text = deger;
                    AktifKullanıcı.kullanici_adi = deger;

                    baglama.Close();
                }
                if (comboBox1.Text == "Şifre")
                {
                    baglama.Open();
                    string deger = textBox2.Text;
                    NpgsqlCommand komut = new NpgsqlCommand("update kullanici set sifre =@p1 where tc_kimlik Like '" + AktifKullanıcı.tc_kimlik + "'", baglama);
                    komut.Parameters.AddWithValue("@p1", deger);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Bilgiler başarıyla güncellendi.");
                  
                    baglama.Close();
                }

            }
            

        }

        private void SearchAndDisplay(string arananKelime)
        {
            // PostgreSQL bağlantı dizesi
            string connectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";

            // NpgsqlConnection oluştur
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Film verilerini çek ve belirli bir kelimeyi içeren satırları getir
                string query = "SELECT film_id, film_ad, resim FROM film WHERE film_ad ILIKE '%' || @arananKelime || '%' OR yonetmen ILIKE '%' || @arananKelime || '%' OR tur ILIKE '%' || @arananKelime || '%'";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@arananKelime", arananKelime);

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
                            byte[] imageBytes = (byte[])reader["resim"];

                            // Her bir film için bir buton oluştur
                            Button filmButton = new Button();
                            filmButton.Width = 250;
                            filmButton.Height = 250;
                            string ad = reader["film_ad"].ToString();
                            filmButton.Cursor = Cursors.Hand;




                            filmButton.Tag = reader["film_ad"].ToString(); // Film ID'yi butonun Tag özelliğine ekleyebilirsiniz.
                            filmButton.Click += FilmButton_Click; // Butona tıklandığında çalışacak olayı belirle




                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image image = Image.FromStream(ms);
                                filmButton.BackgroundImage = image;
                                filmButton.BackgroundImageLayout = ImageLayout.Stretch;
                                

                                // Şimdi image nesnesini kullanabilirsiniz
                                // Örneğin, bir PictureBox nesnesine atayabilirsiniz

                            }



                            filmButton.Location = new Point(asw, asd);

                            // Panel içine butonu ekle
                            panel3.Controls.Add(filmButton);

                            // Sonraki butonun üstünden başlamak için değerleri güncelle

                            asw += 300;

                        }
                    }

                }
                textBox3.Text = "";

            }
        }

        
        private void button10_Click(object sender, EventArgs e)
        {
            button11.Visible = true;
            panel3.Controls.Clear();
            string arananKelime = textBox3.Text.Trim();
            if (!string.IsNullOrEmpty(arananKelime))
            {
                SearchAndDisplay(arananKelime);
            }
            else
            {
                MessageBox.Show("Lütfen bir kelime girin.");
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            textBox3.Text = "";
            LoadFilmButtons();

        }

        private void button12_Click(object sender, EventArgs e)
        {
            baglama.Open();
            // sql syntax ı nedeniyle stringleri parçalamak ve üstünde oynamak gerekti.
            NpgsqlCommand kmt = new NpgsqlCommand("delete from  kullanici where tc_kimlik Like '" + AktifKullanıcı.tc_kimlik + "'", baglama);
            NpgsqlCommand kmt1 = new NpgsqlCommand("delete from  degerlendirme_tablosu where tc_kimlik Like '" + AktifKullanıcı.tc_kimlik + "'", baglama);
            kmt.ExecuteNonQuery();
            kmt1.ExecuteNonQuery();
            MessageBox.Show("Bilgiler başarıyla silindi bizi tercih ettiğiniz için teşekkürler...");
            Form1 form = new Form1();
            form.Show();
            this.Close();
            

            baglama.Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";
            string film_ad = label4.Text.ToLower();
            string tc = AktifKullanıcı.tc_kimlik;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Film eklemek için SQL sorgusu
                string insertQuery = "INSERT INTO izleme_listesi (film_ad, tc_kimlik) VALUES (@filmAd, @tcKimlik)";

                // SQL komut nesnesi oluştur
                using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@filmAd", film_ad);
                    command.Parameters.AddWithValue("@tcKimlik", tc);

                    // Komutu çalıştır
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("İzleme listesine eklendi..");
                button15.Enabled = false;
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel6.BringToFront();
            string sorgum = "select izleme_listesi.film_ad,izleme_listesi.tc_kimlik,film.resim FROM film INNER JOIN izleme_listesi  ON izleme_listesi.film_ad = film.film_ad where tc_kimlik = @ad ";
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {


                using (NpgsqlCommand command = new NpgsqlCommand(sorgum, connection))
            {
                //parametreler ekleniyor
                command.Parameters.AddWithValue("@ad", AktifKullanıcı.tc_kimlik);
                panel6.Visible = true;

                    connection.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        int buttonHeight = 30; // Buton yüksekliği


                        int currentTop = 0; // İlk butonun üstünden başlamak için

                        int asw = 95, asd = 0;

                        while (reader.Read())
                        {

                            // Her bir film için bir buton oluştur
                            Button btn  = new Button();
                            Button sil = new Button();
                            sil.Width = 50;
                            sil.Height = 50;
                            sil.Text = "sil";
                            sil.Font = new Font("Rockwell", 10, FontStyle.Regular);
                            sil.BackColor = Color.Red;

                            btn.Width = 500;                          
                            btn.Height = 100;
                            btn.Font = new Font("Rockwell", 10, FontStyle.Regular);      
                            btn.Padding = new Padding(5); // İçerideki metni çerçeveden ayır
                            
                                                            // 
                            string kisi = reader.GetString(reader.GetOrdinal("film_ad"));
                            sil.Tag = reader["film_ad"].ToString();
                            

                            btn.Text = kisi + "\n" ;
                            sil.Click += label_Click;


                            sil.Location = new Point(655, asw);
                            btn.Location = new Point(150, asw);

                            // Panel içine butonu ekle
                            panel6.Controls.Add(btn);
                            panel6.Controls.Add(sil);
                            byte[] imageBytess = (byte[])reader["resim"];

                            

                            // Sonraki butonun üstünden başlamak için değerleri güncelle
                            asw += 157;

                        }


                    }
                }
            }

        }

        private void label_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
           
            string filmId = clickedButton.Tag.ToString();
            Liste_sil(filmId);
            button13_Click(this, EventArgs.Empty);

        }

        private void Liste_sil(string filmId)
        {
            baglama.Open();
            // sql syntax ı nedeniyle stringleri parçalamak ve üstünde oynamak gerekti.
            NpgsqlCommand kmt = new NpgsqlCommand("delete from  izleme_listesi where tc_kimlik Like '" + AktifKullanıcı.tc_kimlik + "' and film_ad Like '" +filmId +"'", baglama);
           
            kmt.ExecuteNonQuery();
            
            baglama.Close();
            

        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            AnaSayfa dene = new AnaSayfa(AktifKullanıcı);
            dene.Show();
            this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            istatistik yeni = new istatistik();
            yeni.Show();

        }
    }
}
