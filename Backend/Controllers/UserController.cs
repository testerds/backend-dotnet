using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Backend.Criptografia;
using Backend.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICriptografia _criptografia;
        private readonly IDataAccess _dataAccess;

        public UserController(IConfiguration configuration, IDataAccess dataAccess, ICriptografia criptografia)
        {
            _configuration = configuration;
            _criptografia = criptografia;
            _dataAccess = dataAccess;
        }

        public static bool IsValidEmail(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetUsers(string userName, string password)
        {
            try
            {
                if(null == userName)
                {
                    var users = await _dataAccess.GetAllUsers();
                    foreach (var user in users)
                    {
                        user.Password = _criptografia.Decrypt(user.Password);
                    }
                    return Ok(users);
                }
                else
                {
                    if(!IsValidEmail(userName))
                    {
                        return BadRequest("Email não valido!");
                    }
                    var user = await _dataAccess.GetUserByUsername(userName);
                    if (_criptografia.Decrypt(user.Password) != password)
                    {
                        return Unauthorized();
                    }
                    user.Password = null;
                    return Ok(user);
                }
            }
            catch(UnauthorizedAccessException e)
            {
                return Unauthorized();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Models.User user)
        {
            try
            {
                if (!IsValidEmail(user.Email))
                {
                    return BadRequest("Email não valido!");
                }
                user.CreationDate = DateTime.UtcNow;
                user.Password = _criptografia.Encrypt(user.Password);
                user.UserId = Guid.NewGuid();
                return Ok(await _dataAccess.CreateUser(user));
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                return Ok(await _dataAccess.DeleteUserById(userId));
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
