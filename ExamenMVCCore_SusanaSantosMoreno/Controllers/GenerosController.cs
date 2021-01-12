using ExamenMVCCore_SusanaSantosMoreno.Data;
using ExamenMVCCore_SusanaSantosMoreno.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenMVCCore_SusanaSantosMoreno.Controllers {
    public class GenerosController : Controller {
        LibreriaContext context;

        public GenerosController() {
            context = new LibreriaContext();
        }

        public IActionResult Index() {
            List<Genero> generos = context.GetGeneros();
            return View(generos);
        }

        public IActionResult CreateGeneros() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGeneros(String genero) {
            this.context.CreateGenero(genero);
            return RedirectToAction("Index");
        }

        public IActionResult EditGeneros(int IdGenero) {
            Genero genero = this.context.GetGenero(IdGenero);
            return View(genero);
        }

        [HttpPost]
        public IActionResult EditGeneros( int idGenero, String genero ) {
            this.context.EditGenero(idGenero, genero);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteGeneros(int IdGenero ) {
            this.context.DeleteGenero(IdGenero);
            return RedirectToAction("Index");
        }
    }
}
