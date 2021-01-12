using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenMVCCore_SusanaSantosMoreno.Models {
    public class Libro {
        public int IdLibro { get; set; }
        public String Titulo { get; set; }
        public String Autor { get; set; }
        public String Sinopsis { get; set; }
        public String Imagen { get; set; }
        public int IdGenero { get; set; }

        public Libro() {
            /*this.IdLibro = 0;
            this.Titulo = "";
            this.Autor = "";
            this.Sinopsis = "";
            this.Imagen = "";
            this.IdGenero = 0;*/
        }

        public Libro( int idLibro, string titulo, string autor, 
            string sinopsis, string imagen, int idGenero ) {
            this.IdLibro = idLibro;
            this.Titulo = titulo;
            this.Autor = autor;
            this.Sinopsis = sinopsis;
            this.Imagen = imagen;
            this.IdGenero = idGenero;
        }
    }
}
