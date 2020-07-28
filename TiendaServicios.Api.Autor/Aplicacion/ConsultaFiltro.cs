using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class ConsultaFiltro
    {
        public class AutorUnico : IRequest<AutorDTO>
        {
            public int AutorLibroId { get; set; }


        }


        public class Manejador : IRequestHandler<AutorUnico, AutorDTO>
        {

            private readonly ContextoAutor _contexto;
            private readonly IMapper _mapper;


            public Manejador(ContextoAutor contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }
            public async Task<AutorDTO> Handle(AutorUnico request, CancellationToken cancellationToken)
            {
                var autorLibro = await _contexto.AutorLibro.Where(x => x.AutorLibroId == request.AutorLibroId).FirstOrDefaultAsync();
                

                if (autorLibro == null)
                {
                    throw new Exception("No se encontro el autor");
                }

                var autorDto = _mapper.Map<AutorLibro, AutorDTO>(autorLibro);
                return autorDto;
            }
        }
    }
}
