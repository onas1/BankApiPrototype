using BankApi.DTOS;
using BankApi.Models;
using BankApi.Service.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BankApi.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager,
            IConfiguration config, IJwtTokenGenerator jwtTokenGenerator,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _config = config;
            _jwtTokenGenerator = jwtTokenGenerator;
            _signInManager = signInManager;
        }

        public async Task<Response> SignIn(LoginDTO model)
        {
            var response = new Response
            {
                ResponseCode = "02",
                ResponseMessage = "SignIn Unsuccessful",
                Data = null
            };

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var token = _jwtTokenGenerator.GenerateToken(user.UserName, user.Id, user.Email, _config);
                await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);


                response.ResponseCode = "00";
                response.ResponseMessage = "SignInSuccessful";
                response.Data = token;
                return response;

            }
            return response;
        }

        public async Task<Response> SignOut()
        {
            await _signInManager.SignOutAsync();
            return new Response
            {
                ResponseCode = "00",
                ResponseMessage = "Logout successful",
                Data = true
            };
        }

        public async Task<Response> Register(AppUser model, string password)
        {
            var response = new Response();


            var result = await _userManager.CreateAsync(model, password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                response.ResponseMessage = "Successful";
                response.ResponseCode = "00";
                response.Data = user;

                return response;
            }

            string Errors = "";
            foreach (var error in result.Errors)
            {
                Errors += error.Description.ToString() + "/n";
            }

            response.ResponseCode = "02";
            response.ResponseMessage = "Unsuccessful";
            response.Data = Errors;

            return response;

        }
    }
}
