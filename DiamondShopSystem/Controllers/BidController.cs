using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModels.Request.Bid;

namespace DiamondShopSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : BaseController
    {
        private readonly IBidService _bidService;
        private readonly ILogger<BidController> _logger;

        public BidController(IBidService bidService, ILogger<BidController> logger)
        {
            _bidService = bidService;
            _logger = logger;
        }

        //[HttpPost]
        //[Route("create")]
        //public async Task<IActionResult> CreateBid([FromBody] CreateBidRequest request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Invalid bid request.");
        //    }

        //    var result = await _bidService.CreateEntity(request);

        //    if (result.IsError)
        //    {
        //        _logger.LogWarning("Bid creation failed: {Message}", result.Message);
        //        return StatusCode((int)result.StatusCode, result);
        //    }

        //    return Ok(result);
        //}

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllBids([FromQuery] GetAllBidRequest request)
        {
            var result = await _bidService.GetAll(request);

            if (result.IsError)
            {
                _logger.LogWarning("Failed to get all bids: {Message}", result.Message);
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetBidById(int id)
        {
            var result = await _bidService.GetById(id);

            if (result.IsError)
            {
                _logger.LogWarning("Bid not found: {Message}", result.Message);
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] UpdateBidRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid bid update request.");
            }

            var result = await _bidService.UpdateBid(id, request);

            if (result.IsError)
            {
                _logger.LogWarning("Bid update failed: {Message}", result.Message);
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            var result = await _bidService.DeleteBid(id);

            if (result.IsError)
            {
                _logger.LogWarning("Bid deletion failed: {Message}", result.Message);
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
