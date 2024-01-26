using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.Querys
{

    public class GetAllCarService
    {
        public class GetAllCarQuery : IRequest<List<CarResponse>>
        {

        }

        public class GetAllBrandHandler : IRequestHandler<GetAllCarQuery, List<CarResponse>>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly IMapper _mapper;

            public GetAllBrandHandler(DbEstacionamientoContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CarResponse>> Handle(GetAllCarQuery request, CancellationToken cancellationToken)
            {

                var cars = await _context.Cars
                .Include(c => c.TypeNavigation)
                .Include(c => c.BrandNavigation)
                .Include(c => c.StateNavigation)
                .Include(c => c.FormatNavigation)
                .Where(c => c.Garage == false)
                .Where(c => c.Location == "None")
                .OrderByDescending(c => c.DischargeDate)
                .ToListAsync();
                var carsDto = _mapper.Map<List<CarResponse>>(cars);
                return carsDto;
            }
        }
    }
}