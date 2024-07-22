using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Commons;
using Service.ViewModels.Request;
using Service.ViewModels.Request.Auctions;
using Service.ViewModels.Response;

namespace Service.IServices
{
    public interface IAuctionService
    {
        public Task<OperationResult<List<AuctionResponse>>> GetAll(GetAllAuctions request);
        public Task<OperationResult<AuctionResponse>> GetById(int id);
       
        public Task<OperationResult<bool>> CreateEntity(CreateAuctionRequest request);

        public Task<OperationResult<bool>> StaffUpdate(int id, StaffUpdate request);
        public Task<OperationResult<bool>> AdminAprrove(int id, AdminApproveRequest request);
        public Task<OperationResult<bool>> StaffConfirm(int id, StaffConfirmRequest request);
        public Task<OperationResult<bool>> UserWaiting(int id);
        public Task<OperationResult<bool>> UserComming(int id);

    }
}
