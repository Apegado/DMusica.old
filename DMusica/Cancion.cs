using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMusica
{
    class Cancion
    {
        private string nombre = "";
        private string url = "";

        public string GetNombre()
        {
            return this.nombre;
        }

        public void SetNombre(string newNombre)
        {
            this.nombre = newNombre;
        }

        public string GetUrl()
        {
            return this.url;
        }

        public void SetUrl(string newUrl)
        {
            this.url = newUrl;
        }
    }
}
