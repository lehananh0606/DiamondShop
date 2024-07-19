using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiamondShopSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("getwalletbyuserid")]
        [Authorize]
        public async Task<IActionResult> GetWalletByUserIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Invalid user ID in token." });
            }

            var result = await _walletService.GetWalletByUserIdAsync(userId);
            if (result.IsError)
            {
                return StatusCode((int)result.StatusCode, result.Errors);
            }

            return Ok(result.Payload);
        }
    }
}
