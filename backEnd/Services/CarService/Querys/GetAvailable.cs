using backEnd.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.CarService.Querys
{
    public class GetAvailable
    {
        public class GetAvailableCarQuery : IRequest<AvailableResponse>
        {

        }

        public class GetAllBrandHandler : IRequestHandler<GetAvailableCarQuery, AvailableResponse>
        {
            private readonly DbEstacionamientoContext _context;

            public GetAllBrandHandler(DbEstacionamientoContext context)
            {
                _context = context;
            }


            public async Task<AvailableResponse> Handle(GetAvailableCarQuery request, CancellationToken cancellationToken)
            {
                int busy = await _context.Cars.Where(c => c.Garage == true).CountAsync();
                int available = 30 - busy;
                AvailableResponse availableResponse = new AvailableResponse();
                availableResponse.Available = available;
                availableResponse.Busy = busy;
                return availableResponse;
            }
        }
    }
}