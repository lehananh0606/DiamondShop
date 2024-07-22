using Service.Commons;
using Service.ViewModels.Request.Auctions;
using Service.ViewModels.Request.Bid;
using Service.ViewModels.Request.Order;
using Service.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IBidService
    {
        public Task<OperationResult<List<BidResponse>>> GetAll(ViewModels.Request.Bid.GetAllBidRequest request);
        public Task<OperationResult<BidResponse>> GetById(int id);

        public Task<OperationResult<bool>> CreateEntity(CreateBidRequest request);
        public Task<OperationResult<BidResponse>> UpdateBid(int id, UpdateBidRequest request);
        public Task<OperationResult<bool>> DeleteBid(int id);
        public Task<bool> RegisterAuctionAsync(int id, RegisterAuctionDTO dto);
    }
}
