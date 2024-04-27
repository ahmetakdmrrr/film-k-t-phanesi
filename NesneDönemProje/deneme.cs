using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace NesneDönemProje
{
    public partial class deneme : Form
    {
        private Panel filmPanel;

        public deneme()
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
                string query = "SELECT film_id, film_ad FROM film";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        int buttonHeight = 30; // Buton yüksekliği
                        int buttonSpacing = 5; // Butonlar arasındaki boşluk

                        int currentTop = 0; // İlk butonun üstünden başlamak için
                        while (reader.Read())
                        {
                            // Her bir film için bir buton oluştur
                            Button filmButton = new Button();
                            filmButton.Text = reader["film_ad"].ToString();
                            filmButton.Tag = reader["film_id"].ToString(); // Film ID'yi butonun Tag özelliğine ekleyebilirsiniz.
                            filmButton.Click += FilmButton_Click; // Butona tıklandığında çalışacak olayı belirle

                            filmButton.Location = new Point(0, currentTop);

                            // Panel içine butonu ekle
                            panel1.Controls.Add(filmButton);

                            // Sonraki butonun üstünden başlamak için değerleri güncelle
                            currentTop += buttonHeight + buttonSpacing;
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

            // Burada tıklanan filmle ilgili diğer işlemleri gerçekleştirebilirsiniz
            MessageBox.Show($"Tıklanan Film ID: {filmId}");
        }




    }
}
