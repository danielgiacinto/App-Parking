using System.Reflection;
using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.CarTypeService
{
    public class GetCarTypeService
    {

        public class GetCarTypeIdQuery : IRequest<List<CarTypeResponse>>
        {

        }


        public class GetAllBrandHandler : IRequestHandler<GetCarTypeIdQuery, List<CarTypeResponse>>
        {
            private readonly DbEstacionamientoContext _context;

            private readonly IMapper _mapper;

            public GetAllBrandHandler(DbEstacionamientoContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CarTypeResponse>> Handle(GetCarTypeIdQuery request, CancellationToken cancellationToken)
            {

                var carType = await _context.CarTypes.ToListAsync();

                List<CarTypeResponse>? response = _mapper.Map<List<CarTypeResponse>>(carType);
                return response;

            }
        }
    }
}