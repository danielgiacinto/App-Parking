using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.PasswordService.UpdatePassword;
using static backEnd.Services.UserService.LoginUser;

namespace backEnd.Controllers {

    [ApiController]
    [Route("/login")]
    public class UserController : ControllerBase {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator){
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand request) {
            bool response =  await _mediator.Send(request);
            if(response){
                return Ok(response);
            } else {
                return NotFound();
            }
        }

        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommand request) {
            bool response =  await _mediator.Send(request);
            if(response){
                return Ok(response);
            } else {
                return NotFound();
            }
        }
    }
}