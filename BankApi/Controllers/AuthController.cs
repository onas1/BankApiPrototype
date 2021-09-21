using AutoMapper;
using BankApi.DTOS;
using BankApi.Models;
using BankApi.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("register_new_user")]
        public async Task<IActionResult> Create(RegisterUserDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();

            //var address = _mapper.Map<Address>(model.Address);
            var user = _mapper.Map<AppUser>(model);

            var result = await _authService.Register(user, model.Password);
            if (result.ResponseCode != "00") return NotFound(result);
            return Ok(result);



        }

        [HttpPost]
        [Route("login_to_authorize_system")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _authService.SignIn(model);
            if (result.ResponseCode != "00") return NotFound(result);
            return Ok(result);

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.SignOut();
            if (result.ResponseCode != "01") return BadRequest(result);
            return Ok(result);
        }

    }
}
