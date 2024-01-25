using backEnd.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.BrandService.Commands.CreateBrandService;
using static backEnd.Services.BrandService.Commands.DeleteBrandService;
using static backEnd.Services.BrandService.Commands.GetAllBrandsService;
using static backEnd.Services.BrandService.Querys.GetBrandById;

namespace backEnd.Controllers
{

    [ApiController]
    [Route("/brand")]
    public class BrandController : ControllerBase
    {

        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<BrandResponse> CreateBrand([FromBody] CreateBrandCommand request) {
            return _mediator.Send(request);
        }

        [HttpGet]
        public Task<List<BrandResponse>> GetAllBrands(){
            return _mediator.Send(new GetAllBrandQuery());
        }

        [HttpGet("{id}")]
        public Task<BrandResponse> GetBrandById(int id){
            return _mediator.Send(new GetBrandIdQuery {IdBrand = id});
        }

        [HttpDelete("{id}")]
        public Task<int> DeleteBrand(int id){
            return _mediator.Send(new DeleteBrandCommand {IdBrand = id});
        }
    }
}