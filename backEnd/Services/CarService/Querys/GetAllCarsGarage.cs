using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.Querys
{

    public class GetAllCarsGarage
    {
        public class GetAllCarsGarageQuery : IRequest<List<CarResponse>>
        {

        }

        public class GetAllCarsGarageHandler : IRequestHandler<GetAllCarsGarageQuery, List<CarResponse>>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly IMapper _mapper;

            public GetAllCarsGarageHandler(DbEstacionamientoContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CarResponse>> Handle(GetAllCarsGarageQuery request, CancellationToken cancellationToken)
            {
                var carsGarage = await _context.Cars
                .Include(c => c.TypeNavigation)
                .Include(c => c.BrandNavigation)
                .Include(c => c.StateNavigation)
                .Include(c => c.FormatNavigation)
                .Where(c => c.Garage == true)
                .Where(c => c.Location != "None")
                .ToListAsync();
                var carsResponse = _mapper.Map<List<CarResponse>>(carsGarage);
                return carsResponse;
            }
        }
    }
}