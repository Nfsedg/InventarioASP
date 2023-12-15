﻿using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public CategoriaRepositorio(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        public void Actuializar(Categoria categoria)
        {
            var categoriaDb = _db.Bodegas.FirstOrDefault(b => b.Id == categoria.Id);
            if (categoriaDb != null)
            {
                categoriaDb.Nombre = categoria.Nombre;
                categoriaDb.Descripcion = categoria.Descripcion;
                categoriaDb.Estado = categoria.Estado;
                _db.SaveChanges();
            }
        }
    }
}
