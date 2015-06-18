using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace DMusica
{
    public partial class Form1 : Form
    {

        String cadenaUrl = "http://www.goear.com/search.php?q=";
        String proveedorUrl = "http://dowint.net/?site=1&url=http://www.goear.com/";

        List<Cancion> lista = new List<Cancion>();
        List<Cancion> PlayList = new List<Cancion>();
        Conexion conectar = new Conexion();
        public Form1()
        {
            InitializeComponent();
            linkLabel1.Links.Add(0, 0, "");
        }

        /// <summary>
        /// Realizamos la Busqueda segun los parametros de frontend y listamos
        /// el resultado en listBox2.
        /// </summary>
        private void Buscar()
        {
            string[] palabras = textBox2.Text.Split((' '));
            string consulta = "";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            lista = new List<Cancion>();
            int i = 0;
            foreach (string s in palabras)
            {
                consulta = consulta + "+" + s;
            }
            if (comboBox1.Text == "Busqueda Rapida") i = 0;
            if (comboBox1.Text == "Busqueda Normal") i = 4;
            if (comboBox1.Text == "Busqueda Completa") i = 8;
            for (int y = 0; y <= i; y++)
            {
                textBox1.Text = conectar.DescargarUrl(cadenaUrl + consulta + "&p=" + y);
                //Parte de Busqueda 
                string[] words = conectar.Busqueda(textBox1.Text);
                Goear goear = new Goear();
                foreach (Cancion cancion in goear.ObtenerUrls(words))
                {
                    lista.Add(cancion);
                }
            }

            foreach (Cancion cancion in lista)
            {

                listBox2.Items.Add(cancion.GetNombre());

            }
        }

        /// <summary>
        /// Realizamos la funcioón de buscar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Reproduce el mp3 en el reproductor por defecto del sistema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(linkLabel1.Text);
        }

        /// <summary>
        /// Guardamos fichero .mp3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog ruta = new SaveFileDialog();

            ruta.Filter = "MP3 | *.mp3";
            ruta.FileName = listBox2.SelectedItem.ToString() + ".mp3";
            ruta.ShowDialog();
            descargarFichero(linkLabel1.Text, ruta.FileName);
            progressBar1.Visible = true;
            label1.Visible = true;
        }

        /// <summary>
        /// Función para la descarga del fichero.
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="ruta"></param>
        private void descargarFichero(string URL, string ruta){
            string DireccionWEB = URL;
            string RutaEnElDisco = ruta;

            WebClient wc = new WebClient();
            //Le asigno un manejador al evento. DownloadProgressChanged se produce
            //cuando una operación de descarga asincrónica transfiere correctamente
            //una parte o la totalidad de los datos.
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            //Se produce cuando todos los datos son transmitidos.
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompletedCallback);
            Uri uri = new Uri(DireccionWEB);
            wc.DownloadFileAsync(uri, RutaEnElDisco);  

        }

        /// <summary>
        /// Callback para actualiar el estado del a descarga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.BytesReceived + "/" + e.TotalBytesToReceive;
        }

        /// <summary>
        /// Callback para aviso de fichero completado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DownloadFileCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Completado!");
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Visible = false;
            button3.Visible = false;
            progressBar1.Visible = false;
            label1.Visible = false;
            obtenerURL();
        }

        /// <summary>
        /// Busca la ruta del MP3 partiendo de la ruta de Goear.
        /// </summary>
        private void obtenerURL()
        {
            int i = listBox2.SelectedIndex;
            if (lista.Count >= i)
            {
                textBox3.Text = lista[listBox2.SelectedIndex].GetUrl();
                textBox4.Text = conectar.DescargarUrl(proveedorUrl + lista[listBox2.SelectedIndex].GetUrl() + "&download=Descargar");
                string[] palabras = textBox4.Text.Split((' '));
                Goear goear = new Goear();

                string url = goear.ObtenerUrl(palabras);
                linkLabel1.Text = url;
                linkLabel1.Links.RemoveAt(0);
                linkLabel1.Links.Add(0, url.Length, url);

                //Hacemos visibles los dos botones.
                button2.Visible = true;
                button3.Visible = true;
            }
        }

        /// <summary>
        /// Si tecla Enter entonces buscar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Buscar();
            }

        }
    }
}
