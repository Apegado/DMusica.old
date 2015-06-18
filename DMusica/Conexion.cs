using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DMusica
{
    class Conexion
    {
        /// <summary>
        /// Intenta descargar la url dada.
        /// </summary>
        /// <param name="url">Url a descargar</param>
        /// <returns></returns>
        public string DescargarUrl(string url)
        {
            string result = "";
            int intentos = 0;

            while (intentos++ < 10)
            {
                try
                {
                    result = DescargarURLaux(url);
                    intentos = 11;
                }
                catch
                {
                    //no hacer nada
                }
            }

            return result;
        }

        /// <summary>
        /// Función auxiliar para la descarga de la dirección
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string DescargarURLaux(string url)
        {
            string final = "";
            System.Net.WebClient Client = new WebClient();
            Stream strm = Client.OpenRead(url);
            StreamReader sr = new StreamReader(strm);
            string line;
            int y = 0;
            do
            {
                y++;
                line = sr.ReadLine();
                final = final + '\n' + line;

            }
            while (line != null);
            strm.Close();

            return final;
        }

        /// <summary>
        /// Separar palabras para parsear.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public string[] Busqueda(string texto)
        {
            char[] delimiterChars = { '"' , '='};

            string[] words = texto.Split(delimiterChars);

            return words;
        }

    }
}
