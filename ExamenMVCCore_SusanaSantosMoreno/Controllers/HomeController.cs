using ExamenMVCCore_SusanaSantosMoreno.Data;
using ExamenMVCCore_SusanaSantosMoreno.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenMVCCore_SusanaSantosMoreno.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        LibreriaContext context;

        public HomeController( ILogger<HomeController> logger ) {
            _logger = logger;
            context = new LibreriaContext();
        }

        public IActionResult Index(int IdGenero, String genero) {
            List<Libro> libros = new List<Libro>();
            if (IdGenero > 0) {
                libros = context.GetLibrosPorGenero(IdGenero);
            } else {
                libros = context.GetLibros();
            }
            
            List<Genero> generos = context.GetGeneros();
            ViewData ["Generos"] = generos;
            return View(libros);
        }

        [HttpPost]
        public IActionResult Index(int IdGenero) {
            List<Libro> libros = context.GetLibrosPorGenero(IdGenero);
            List<Genero> generos = context.GetGeneros();
            ViewData ["Generos"] = generos;
            return View(libros);
        }

        public IActionResult CreateLibros() {
            List<Genero> generos = context.GetGeneros();
            ViewData ["Generos"] = generos;
            return View();
        }

        

        [HttpPost]
        public IActionResult CreateLibros(Libro libro) {
            this.context.CreateLibro(libro.Titulo, libro.Autor, 
                libro.Sinopsis, libro.Imagen, libro.IdGenero);
            return RedirectToAction("Index");
        }

        public IActionResult EditLibros(int IdLibro) {
            Libro libro = this.context.GetLibro(IdLibro);
            List<Genero> generos = context.GetGeneros();
            ViewData ["Generos"] = generos;
            Genero genero = context.GetGenero(libro.IdGenero);
            ViewData ["Genero"] = genero;
            return View(libro);
        }

        [HttpPost]
        public IActionResult EditLibros(Libro libro) {
            this.context.EditLibro(libro.IdLibro, libro.Titulo, libro.Autor,
                libro.Sinopsis, libro.Imagen, libro.IdGenero);
            return RedirectToAction("Index");
        }

        public IActionResult DetalleLibros( int IdLibro ) {
            Libro libro = this.context.GetLibro(IdLibro);
            Genero genero = this.context.GetGenero(libro.IdGenero);
            ViewData ["Genero"] = genero;
            return View(libro);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
