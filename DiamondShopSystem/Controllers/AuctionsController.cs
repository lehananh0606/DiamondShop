using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModels.Request;
using Service.ViewModels.Request.Auctions;

namespace DiamondShopSystem.Controllers
{
    public class AuctionsController : BaseController
    {
        
        private readonly IAuctionService _auctionService;
        private readonly IBidService _bidService;

        public AuctionsController(IAuctionService auctionService, IBidService bidService)
        {
            _auctionService = auctionService;
            _bidService = bidService;
        }

        /// <summary>
        /// 
        /// Get all auctions
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("auctions")]
        // get all
        public async Task<IActionResult> GetAll([FromQuery] GetAllAuctions request)
        {
            var response = await _auctionService.GetAll(request);
            
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
        
        
        /// <summary>
        /// Get auction by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("auctions/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _auctionService.GetById(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpPost("auction")]
        public async Task<IActionResult> Create(CreateAuctionRequest request)
        {
            var response = await _auctionService.CreateEntity(request);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
            
        }

        /// <summary>
        /// Update an auction by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("auctions/set-evaluate/{id}")]
        public async Task<IActionResult> StaffUpdate(int id, StaffUpdate request)
        {
            var response = await _auctionService.StaffUpdate(id, request);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpPut("auctions/set-approve/{id}")]
        public async Task<IActionResult> AdminAprrove(int id, AdminApproveRequest request)
        {
            var response = await _auctionService.AdminAprrove(id, request);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        [HttpPut("auctions/set-confirm/{id}")]
        public async Task<IActionResult> StaffConfirm(int id, StaffConfirmRequest request)
        {
            var response = await _auctionService.StaffConfirm(id, request);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        [HttpPut("auctions/set-waiting/{id}")]
        public async Task<IActionResult> UserWaiting(int id)
        {
            var response = await _auctionService.UserWaiting(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpPut("auctions/set-comming/{id}")]
        public async Task<IActionResult> UserComming(int id)
        {
            var response = await _auctionService.UserComming(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        /// <summary>
        /// Register a user for an auction.
        /// </summary>
        /// <param name="id">Auction ID</param>
        /// <param name="dto">Registration details</param>
        /// <returns></returns>
        [HttpPost("auctions/{id}/register")]
        public async Task<IActionResult> RegisterAuction(int id, RegisterAuctionDTO dto)
        {
            var result = await _bidService.RegisterAuctionAsync(id, dto);

            if (result)
            {
                return Ok(); // Successfully registered
            }
            else
            {
                return BadRequest(new { message = "Auction registration failed." }); // Registration failed
            }
        }



    }
}
