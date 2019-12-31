using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace OCR_Tabanli_Fis_TanimaWF
{
    public partial class Form1 : Form
    {
        public Bitmap bitmap;
        private string path;
        public SqlConnection sqlConnection = new SqlConnection("server=.;database=IsletmeFis;Integrated Security=SSPI;");

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Resim seçiniz";
                dlg.Filter = "Resim (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(dlg.FileName);

                    path = dlg.FileName;
                    var imageSize = pictureBox1.Image.Size;
                    var fitSize = pictureBox1.ClientSize;
                    pictureBox1.SizeMode = imageSize.Width > fitSize.Width || imageSize.Height > fitSize.Height ?
                        PictureBoxSizeMode.Zoom : PictureBoxSizeMode.CenterImage;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txtbox = "";
            List<List<string>> blocks = new List<List<string>>();

            List<string> ls = TesseractOCR.Doit(new Bitmap(pictureBox1.Image), path, Dil.TURKISH, buttonIsle, ref txtbox, ref blocks);
            richTextBox1.Text = txtbox;

            KaydetIsletmeFisBilgilerini(blocks[0][0], ls[0], ls[1], ls[2], ls[3]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GosterKayitlari();
        }

        private void GosterKayitlari()
        {
            SqlDataAdapter oleDbDataAdapter = new SqlDataAdapter("select * from IsletmeFis", sqlConnection);
            DataTable dataTable = new DataTable();
            oleDbDataAdapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }

        private void GosterKayitlari1()
        {
            SqlDataAdapter oleDbDataAdapter = new SqlDataAdapter("select * from Isletme i inner join Fis f on f.IsletmeId=i.IsletmeId", sqlConnection);
            DataTable dataTable = new DataTable();
            oleDbDataAdapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns[4].Visible = false;
        }

        public void KaydetIsletmeBilgilerini(string isletmeAdi, string isletmeAdresi)
        {
            SqlCommand sqlCommand = new SqlCommand("insert Isletme values(@isletmeAdi,@isletmeAdresi)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@isletmeAdi", isletmeAdi);
            sqlCommand.Parameters.AddWithValue("@isletmeAdresi", isletmeAdresi);

            sqlConnection.Open();
            int etkilenen = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            if (etkilenen > 0)
            {
                Console.WriteLine("Kayıt eklendi");
                GosterKayitlari();
            }
            else
                Console.WriteLine("Kayıt ekleneMEdi!");
        }

        public void KaydetFisBilgilerini(int isletmeId, string fisNo, string tarih, string urunlerKdvlerFiyatlar, string toplamFiyat)
        {
            SqlCommand sqlCommand = new SqlCommand("insert Fis values(@isletmeId,@fisNo,@tarih,@urunlerKdvlerFiyatlar,@toplamFiyat)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@isletmeId", isletmeId);
            sqlCommand.Parameters.AddWithValue("@fisNo", fisNo);
            sqlCommand.Parameters.AddWithValue("@tarih", tarih);
            sqlCommand.Parameters.AddWithValue("@urunlerKdvlerFiyatlar", urunlerKdvlerFiyatlar);
            sqlCommand.Parameters.AddWithValue("@toplamFiyat", toplamFiyat);

            sqlConnection.Open();
            int etkilenen = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            if (etkilenen > 0)
            {
                Console.WriteLine("Kayıt eklendi");
                GosterKayitlari();
            }
            else
                Console.WriteLine("Kayıt ekleneMEdi!");
        }

        public void KaydetIsletmeFisBilgilerini(string isletmeAdi, string isletmeAdresi, string tarih, string urunlerKdvlerFiyatlar, string toplamFiyat)
        {
            SqlCommand sqlCommand = new SqlCommand("insert IsletmeFis values(@isletmeAdi,@isletmeAdresi,@tarih,@urunlerKdvlerFiyatlar,@toplamFiyat)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@isletmeAdi", isletmeAdi);
            sqlCommand.Parameters.AddWithValue("@isletmeAdresi", isletmeAdresi);
            sqlCommand.Parameters.AddWithValue("@tarih", tarih);
            sqlCommand.Parameters.AddWithValue("@urunlerKdvlerFiyatlar", urunlerKdvlerFiyatlar);
            sqlCommand.Parameters.AddWithValue("@toplamFiyat", toplamFiyat);

            sqlConnection.Open();
            int etkilenen = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            if (etkilenen > 0)
            {
                Console.WriteLine("Kayıt eklendi");
                GosterKayitlari();
            }
            else
                Console.WriteLine("Kayıt ekleneMEdi!");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter oleDbDataAdapter = new SqlDataAdapter("select * from IsletmeFis where IsletmeAdi like '%" + textBox1.Text + "%'", sqlConnection);
            DataTable dataTable = new DataTable();
            oleDbDataAdapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter oleDbDataAdapter = new SqlDataAdapter("select * from IsletmeFis where Tarih like '%" + textBox2.Text + "%'", sqlConnection);
            DataTable dataTable = new DataTable();
            oleDbDataAdapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }
    }
}