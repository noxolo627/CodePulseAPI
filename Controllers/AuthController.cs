
using CodePulse.Repositories.Implementations;
using CodePulse. Repositories.Interfaces;
using CodePulse.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodePulse.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager,
        ITokenRepository tokenRepository)
    {
      this.userManager = userManager;
      this.tokenRepository = tokenRepository;
    }

    // POST: {apibaseurl}/api/auth/login
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
      // Check Email
      var identityUser = await userManager.FindByEmailAsync(request.Email);

      if (identityUser is not null)
      {
        // Check Password
        var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

        if (checkPasswordResult)
        {
          var roles = await userManager.GetRolesAsync(identityUser);

          // Create a Token and Response
          var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

          var response = new LoginResponseDto()
          {
            Email = request.Email,
            Roles = roles.ToList(),
            Token = jwtToken
          };

          //await pdateAdminPassword().Wait;
          return Ok(response);
        }
      }
      var errorMessage = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
      ModelState.AddModelError("", $"{errorMessage}");


      return ValidationProblem(ModelState);
    }


    // POST: {apibaseurl}/api/auth/register
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
      // Create IdentityUser object
      var user = new IdentityUser
      {
        UserName = request.Email?.Trim(),
        Email = request.Email?.Trim()
      };

      // Create User
      var identityResult = await userManager.CreateAsync(user, request.Password);

      if (identityResult.Succeeded)
      {
        // Add Role to user (Reader)
        identityResult = await userManager.AddToRoleAsync(user, "Reader");

        if (identityResult.Succeeded)
        {
          return Ok();
        }
        else
        {
          if (identityResult.Errors.Any())
          {
            foreach (var error in identityResult.Errors)
            {
              ModelState.AddModelError("", error.Description);
            }
          }
        }
      }
      else
      {
        if (identityResult.Errors.Any())
        {
          foreach (var error in identityResult.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
        }
      }

      return ValidationProblem(ModelState);
    }

  }
}
