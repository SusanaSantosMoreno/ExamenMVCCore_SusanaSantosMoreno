using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenMVCCore_SusanaSantosMoreno.Models {
    public class Genero {

        public int IdGenero { get; set; }
        public String NombreGenero { get; set; }

        public Genero( int idGenero, string nombreGenero ) {
            IdGenero = idGenero;
            NombreGenero = nombreGenero;
        }

        public Genero() { }
    }
}
