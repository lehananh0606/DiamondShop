using Service.Commons;
using Service.ViewModels.Response;
using Service.ViewModels.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IWalletService
    {
        Task<OperationResult<WalletResponse>> GetWalletByUserId(int id);
        Task<OperationResult<WalletResponse>> UpdateWalletBalance(int walletId, float balance);
        Task<OperationResult<WalletResponse>> GetWalletByUserIdAsync(string userId);
    }
}
