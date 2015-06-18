using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMusica
{
    class Goear
    {
        /// <summary>
        /// Parser que busca Canciones en el texto
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public List<Cancion> ObtenerUrls(string[] texto)
        {
            Boolean esUrl = false;
            Boolean esNombre = false;
            List<Cancion> lista = new List<Cancion>();
            int i=-1;
            string url = "";
            string nombre = "";
            int e = 6;
            foreach (string s in texto)
            {
                    if ((s == " href") )
                    {
                        if (e <= 0)
                        {
                            esUrl = true;
                            i = 2;
                        }
                        else e--;
                    }
                    if ((esUrl) && (i == 0))
                    {
                        url = s;
                        i = 4;
                        esUrl = false;
                        esNombre = true;
                    }
                    if ((esNombre) && (i == 0))
                    {
                        nombre = s;
                        esNombre = false;
                        if ((nombre != " title") && (nombre != " type") && (nombre != ">\n    <ul class") && (nombre != ">Goear</span></a>\n       <iframe class") && (nombre != ">Letras</a></li>\n          </ul>\n          <ul>\n            <li class") && (nombre != " src"))
                        {
                            try
                            {
                                nombre = nombre.TrimStart(('>'));
                                nombre = nombre.Remove(nombre.Length - 21);
                                if (nombre.Contains("<h2>"))
                                {
                                    //No es cancion.
                                    throw new SystemException();
                                }
                                Cancion nueva = new Cancion();
                                nueva.SetNombre(nombre);
                                nueva.SetUrl(url);
                                lista.Add(nueva);
                            }
                            catch
                            {
                            }
                        }
                    }
                
                i--;
            }
            return lista;
        }

        /// <summary>
        /// Busca link de descarga
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public String ObtenerUrl(string[] texto)
        {
            int i = 0;
            string final = "";
            Boolean esUrl = false;
             foreach (string s in texto)
            {
                if (esUrl)
                {
                    final = s;
                    esUrl = false;
                }
                if (s == ("id=\"descargar\"")) esUrl = true;
                i++;
            }
           string[] final2 =final.Split(('='),('"'));
            return final2[2];
        }
    }
}
