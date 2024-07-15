using AutoMapper;
using DiamondShopSystem.Authorization;
using DiamondShopSystem.Constants;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Service.Commons;
using Service.Contants;
using Service.IServices;
using Service.Services;
using Service.ViewModels.Request.User;
using Service.ViewModels.Response.User;
using ShopRepository.Repositories.UnitOfWork;
using System.Security.Claims;

namespace DiamondShopSystem.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class UserController : ControllerBase
        {
            private readonly IUserService _userService;

            public UserController(IUserService userService)
            {
                _userService = userService;
            }

        [HttpGet]
        //[ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        //[Produces(MediaTypeConstant.ApplicationJson)]
        //[HttpGet(APIEndPointConstant.Customer.Cu)]
        //[PermissionAuthorize(RoleConstants.Admin)]
        public async Task<ActionResult<GetUserResponse>> GetUsersAsync([FromQuery] GetUserRequest getUsersRequest)
        {
            try
            {
                // Lấy danh sách Claims từ HttpContext.User.Claims
                IEnumerable<Claim> claims = HttpContext.User.Claims;

                var result = await _userService.GetUserAsync(getUsersRequest, claims);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
            public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest) 
            {
                try
                {
                    // Lấy danh sách Claims từ HttpContext.User.Claims
                    IEnumerable<Claim> claims = HttpContext.User.Claims;

                    await _userService.CreateUserAsync(createUserRequest, claims);
                    return Ok("User created successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateUserAsync(int id, [FromBody] UpdateUserRequest updateUserRequest)
            {
                try
                {
                    // Lấy danh sách Claims từ HttpContext.User.Claims
                    IEnumerable<Claim> claims = HttpContext.User.Claims;

                    await _userService.UpdateUserAsync(id, updateUserRequest, claims);
                    return Ok("User updated successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            [HttpPut("{id}/status")]
            public async Task<ActionResult> UpdateUserStatusAsync(int id, [FromBody] UpdateUserStatusRequest updateUserStatusRequest)
            {
                try
                {
                    // Lấy danh sách Claims từ HttpContext.User.Claims
                    IEnumerable<Claim> claims = HttpContext.User.Claims;

                    await _userService.UpdateUserStatusAsync(id, updateUserStatusRequest, claims);
                    return Ok("User status updated successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteUserAsync(int id)
            {
                try
                {
                // Lấy danh sách Claims từ HttpContext.User.Claims
                    IEnumerable<Claim> claims = HttpContext.User.Claims;
              

                // Check in the database to ensure the user's IsDeleted field is set to true

                await _userService.DeleteUserAsync(id, claims);
                    return Ok("User deleted successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }
