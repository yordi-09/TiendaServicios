﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class AutorDTO
    {

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime? Fecha { get; set; }

        public string AutorLibroGuid { get; set; }


    }
}
