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
    public partial class istatistik : Form
    {
        public istatistik()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            string connectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";

            // NpgsqlConnection oluştur
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Film verilerini çek
                string query = "SELECT film_id, film_ad , resim FROM film order by degerlendirme_puani DESC limit 10 ";
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
                            
                            filmButton.Cursor = Cursors.Hand;
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
                            panel1.Controls.Add(filmButton);

                            // Sonraki butonun üstünden başlamak için değerleri güncelle

                            asw += 300;

                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            string connectionString = "server=localHost; port=5432; Database=NesneProje; user ID=postgres; password=1234";

            // NpgsqlConnection oluştur
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Film verilerini çek
                string query = "SELECT film.film_ad,film.resim,COUNT(*) FROM degerlendirme_tablosu INNER JOIN  film ON film.film_ad = degerlendirme_tablosu.film_ad GROUP BY film.resim, film.film_ad ORDER BY COUNT(*) DESC";
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

                            filmButton.Cursor = Cursors.Hand;
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
                            panel1.Controls.Add(filmButton);

                            // Sonraki butonun üstünden başlamak için değerleri güncelle

                            asw += 300;

                        }
                    }
                }
            }
        }
    }
}
