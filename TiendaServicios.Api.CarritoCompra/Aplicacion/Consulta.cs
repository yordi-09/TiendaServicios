﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {

        public class Ejecuta : IRequest<CarritoDto>{
            
            public int CarritoSesionId { get; set; }

        }


        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto _contexto;
            private readonly ILibrosService _librosService;

            public Manejador(CarritoContexto contexto, ILibrosService librosService)
            {
                _contexto = contexto;
                _librosService = librosService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await _contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var ListaCarritoDto = new List<CarritoDetalleDto>();

                foreach(var libro in carritoSesionDetalle)
                {
                    var response = await _librosService.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if (response.resultado)
                    {
                        var objeto_libro = response.Libro;
                        var carritoDetalle = new CarritoDetalleDto
                        {
                            TituloLibro = objeto_libro.Titulo,
                            FechaPublicacion = objeto_libro.FechaPublicacion,
                            LibroId = objeto_libro.LibreriaMaterialId

                        };
                        ListaCarritoDto.Add(carritoDetalle);

                        
                    }
                    
                }

                var carritoSesionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCrecion,
                    ListaProductos = ListaCarritoDto
                };
                return carritoSesionDto;
            }
        }

    }
}
