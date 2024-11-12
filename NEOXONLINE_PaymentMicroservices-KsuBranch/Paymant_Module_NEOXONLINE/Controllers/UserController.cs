using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Payment.Application.Payment_DAL.Contracts;
using Payment.BLL.Contracts.Identity;
using Payment.Domain.DTOs;
using Payment.Domain.ECommerce;
using Payment.Domain.Identity;
using Payment.Domain.PayProduct;
using System.Net;

namespace Paymant_Module_NEOXONLINE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public UserController(ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .Include(u => u.Basket)
            .ToListAsync();

            return Ok(users);
        }

        [HttpGet("GetUserByFirstName")]
        public async Task<IActionResult> GetUserByFirstName(string userName)
        {
            var user = _unitOfWork.GetRepository<User>().AsQueryable().Where(u => u.FirstName.Equals(userName)).Include(u => u.Basket).First();
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound($"user with name {userName} not found ");
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreationDto userCreationDto)
        {
            var newUser = new User
            {
                FirstName = userCreationDto.FirstName,
                LastName = userCreationDto.LastName,
                Email = userCreationDto.Email,
                Сountry = userCreationDto.Сountry,
                Address = userCreationDto.Address,
                PhoneNumber = userCreationDto.PhoneNumber,
                //Basket = new List<Basket>(),
            };
            _unitOfWork.GetRepository<User>().Create(newUser);
            _unitOfWork.GetRepository<Basket>().Create(new Basket() { User = newUser });

            await _unitOfWork.SaveShangesAsync();

            return Ok(newUser);
        }

        [HttpPost("CreateUserByToken")]

        public async Task<IActionResult> CreateUserByToken(string token)
        {
            try
            {
                var claimsList = _tokenService.DecryptToken(token);
                var user = _tokenService.GetUser(claimsList);

                _unitOfWork.GetRepository<User>().Create(user);
                _unitOfWork.GetRepository<Basket>().Create(new Basket() { User = user });

                await _unitOfWork.SaveShangesAsync();
                return Ok(user);
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest("token is invalid");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("token does not contain reguired fields");
            }
        }

        [HttpPut("UpdateUser")]

        public async Task<IActionResult> UpdateUser([FromForm]UserCreationDto userDto)
        {
            var user = await _unitOfWork.GetRepository<User>()
                .AsReadOnlyQueryable()
                .FirstOrDefaultAsync(u => u.FirstName.Equals(userDto.FirstName));
            if (user != null)
            {
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Email = userDto.Email;
                user.Сountry = userDto.Сountry;
                user.Address = userDto.Address;
                user.PhoneNumber = userDto.PhoneNumber;

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveShangesAsync();

                return Ok(user);
            }
            else
            {
                return NotFound($"user with name {userDto.FirstName} not found");
            }
        }


        [HttpDelete("DeleteUser")]

        public async Task<IActionResult> DeleteUser(string userName)
        {
            var deletedUser = await _unitOfWork.GetRepository<User>()
                .AsQueryable()
                .FirstOrDefaultAsync(u => u.FirstName.Equals(userName));
            if (deletedUser == null)
            {
                return BadRequest(new { message = $"Invalid source data. Not Found {userName}" });
            }
            else
            {
                _unitOfWork.GetRepository<User>().Delete(deletedUser);
                await _unitOfWork.SaveShangesAsync();
                return Ok($"{deletedUser} has been deleted");
            }
        }
    }
}
