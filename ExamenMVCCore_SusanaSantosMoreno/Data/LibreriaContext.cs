using ExamenMVCCore_SusanaSantosMoreno.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenMVCCore_SusanaSantosMoreno.Data {
    public class LibreriaContext {

        SqlDataAdapter adapterLibros;
        SqlDataAdapter adapterGeneros;
        DataTable tablaLibros;
        DataTable tablaGeneros;

        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;

        public LibreriaContext() {
            String cadena = @"Data Source=msi\sqlserver_2019;Initial Catalog=Libreria;
                    User ID=sa;Password=MCSD2020";
            this.tablaLibros = new DataTable();
            this.tablaGeneros = new DataTable();
            this.adapterLibros = new SqlDataAdapter("SELECT * FROM Libros", cadena);
            this.adapterGeneros = new SqlDataAdapter("SELECT * FROM Generos", cadena);
            this.adapterLibros.Fill(this.tablaLibros);
            this.adapterGeneros.Fill(this.tablaGeneros);


            this.connection = new SqlConnection(cadena);
            this.command = new SqlCommand();
            this.command.Connection = this.connection;
        }

        public List<Libro> GetLibros() {
            List<Libro> libros = new List<Libro>();
            var consulta = from datos in this.tablaLibros.AsEnumerable()
                           select datos;
            foreach(var dato in consulta) {
                libros.Add(new Libro(dato.Field<int>("IdLibro"), dato.Field<String>("Titulo"),
                    dato.Field<String>("Autor"), dato.Field<String>("Sinopsis"), dato.Field<String>("Imagen"),
                    dato.Field<int>("IdGenero")));
            }
            return libros;
        }

        public List<Genero> GetGeneros() {
            List<Genero> generos = new List<Genero>();
            var consulta = from datos in this.tablaGeneros.AsEnumerable()
                           select datos;
            foreach (var dato in consulta) {
                generos.Add(new Genero(dato.Field<int>("idGenero"), dato.Field<String>("Genero")));
            }
            return generos;
        }

        public List<Libro> GetLibrosPorGenero(int idGenero) {
            List<Libro> libros = new List<Libro>();
            var consulta = from datos in this.tablaLibros.AsEnumerable()
                           where datos.Field<int>("idGenero") == idGenero
                           select datos;
            foreach (var dato in consulta) {
                libros.Add(new Libro(dato.Field<int>("IdLibro"), dato.Field<String>("Titulo"),
                    dato.Field<String>("Autor"), dato.Field<String>("Sinopsis"), 
                    dato.Field<String>("Imagen"),
                    dato.Field<int>("IdGenero")));
            }
            return libros;
        }

        public Libro GetLibro(int IdLibro) {
            Libro libro = new Libro();
            this.command.CommandType = CommandType.Text;
            this.command.CommandText = "SELECT * FROM Libros WHERE IdLibro = @IdLibro";
            this.command.Parameters.AddWithValue("@IdLibro", IdLibro);
            this.connection.Open();
            this.reader = this.command.ExecuteReader();
            this.reader.Read();
            libro = new Libro(Convert.ToInt32(this.reader["IdLibro"]), this.reader["Titulo"].ToString(), 
                this.reader["Autor"].ToString(), this.reader["Sinopsis"].ToString(), 
                this.reader["Imagen"].ToString(), 
                Convert.ToInt32(this.reader["IdGenero"]));
            this.reader.Close();
            this.command.Parameters.Clear();
            this.connection.Close();
            return libro;
        }

        public Genero GetGenero( int IdGenero ) {
            Genero genero = new Genero();
            var consulta = from datos in this.tablaGeneros.AsEnumerable()
                           where datos.Field<int>("IdGenero") == IdGenero
                           select datos;
            var dato = consulta.First();
            genero = new Genero(dato.Field<int>("IdGenero"), dato.Field<String>("Genero"));
            return genero;
        }

        /* 
        CREATE PROCEDURE CREARLIBRO (@Titulo NVARCHAR(100), @Autor NVARCHAR(100), 
	        @Sinopsis NVARCHAR(255), @Imagen NVARCHAR(100), @IdGenero INT)
        AS
	        DECLARE @IdLibro INT
	        SELECT @IdLibro = MAX(IdLibro) + 1 FROM LIBROS;
	        INSERT INTO Libros VALUES (@IdLibro, @Titulo, @Autor, @Sinopsis, @Imagen, @IdGenero);
        GO
         */
        public void CreateLibro(String titulo, String autor, 
            String sinopsis, String imagen, int idGenero ) {
            this.command.CommandType = CommandType.StoredProcedure;
            this.command.CommandText = "CREARLIBRO";
            this.command.Parameters.AddWithValue("@Titulo", titulo);
            this.command.Parameters.AddWithValue("@Autor", autor);
            this.command.Parameters.AddWithValue("@Sinopsis", sinopsis);
            this.command.Parameters.AddWithValue("@Imagen", imagen);
            this.command.Parameters.AddWithValue("@IdGenero", idGenero);
            this.connection.Open();
            this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        /* 
        CREATE PROCEDURE EDITARLIBRO (@IdLibro INT, @Titulo NVARCHAR(100), @Autor NVARCHAR(100), 
	        @Sinopsis NVARCHAR(255), @Imagen NVARCHAR(100), @IdGenero INT)
        AS
	        UPDATE Libros SET Titulo = @Titulo, Autor = @AUTOR, Sinopsis = @Sinopsis, 
		        Imagen = @Imagen, IdGenero = @IdGenero WHERE IdLibro = @IdLibro;
        GO 
         */
        public void EditLibro(int IdLibro, String titulo, String autor,
            String sinopsis, String imagen, int idGenero ) {
            this.command.CommandType = CommandType.StoredProcedure;
            this.command.CommandText = "EDITARLIBRO";
            this.command.Parameters.AddWithValue("@IdLibro", IdLibro);
            this.command.Parameters.AddWithValue("@Titulo", titulo);
            this.command.Parameters.AddWithValue("@Autor", autor);
            this.command.Parameters.AddWithValue("@Sinopsis", sinopsis);
            this.command.Parameters.AddWithValue("@Imagen", imagen);
            this.command.Parameters.AddWithValue("@IdGenero", idGenero);
            this.connection.Open();
            this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        /* 
        CREATE PROCEDURE ELIMINAR_GENERO(@IdGenero INT)
        AS
	        DECLARE @LibrosPorGenero INT
	        SELECT @LibrosPorGenero = COUNT(IdGenero) FROM Libros WHERE IdGenero = @IdGenero;
	
	        IF @LibrosPorGenero = 0  
		        DELETE FROM Generos WHERE IdGenero = @IdGenero;
	
        GO
         */
        public void DeleteGenero( int idGenero ) {
            this.command.CommandType = CommandType.StoredProcedure;
            this.command.CommandText = "ELIMINAR_GENERO";
            this.command.Parameters.AddWithValue("@IdGenero", idGenero);
            this.connection.Open();
            this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        /*
         * CREATE PROCEDURE CREARGENERO (@Genero NVARCHAR(100))
            AS
                 DECLARE @IdGenero INT
	             SELECT @IdGenero = MAX(IdGenero) + 1 FROM Generos;
	             INSERT INTO Generos VALUES (@IdGenero, @Genero);
            GO
         */
        public void CreateGenero(String genero) {
            this.command.CommandType = CommandType.StoredProcedure;
            this.command.CommandText = "CREARGENERO";
            this.command.Parameters.AddWithValue("@Genero", genero);
            this.connection.Open();
            this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public void UpdateGenero( int IdGenero, String genero ) {
            this.command.CommandType = CommandType.Text;
            this.command.CommandText = "UPDATE Generos SET Genero = @Genero WHERE IdGenero = @IdGenero";
            this.command.Parameters.AddWithValue("@Genero", genero);
            this.command.Parameters.AddWithValue("@IdGenero", IdGenero);
            this.connection.Open();
            this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public int GetIdGenero() {
            this.command.CommandType = CommandType.Text;
            this.command.CommandText = "SELECT MAX(IdGenero) AS maxId FROM Generos";
            this.connection.Open();
            this.reader = this.command.ExecuteReader();
            int maxId = 0;
            while (this.reader.Read()) {
                maxId = Convert.ToInt32(this.reader ["maxId"]);
            }
            this.reader.Close();
            this.connection.Close();
            return (maxId + 1);
        }

        public void EditGenero( int IdGenero, String genero ) {
            this.command.CommandType = CommandType.Text;
            this.command.CommandText = "UPDATE Generos SET Genero = @Genero WHERE IdGenero = @IdGenero";
            this.command.Parameters.AddWithValue("@IdGenero", IdGenero);
            this.command.Parameters.AddWithValue("@Genero", genero);
            this.connection.Open();
            this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

    }
}
