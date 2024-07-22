using ShopRepository.Models;
using ShopRepository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Transactions;

namespace ShopRepository.Repositories.IRepository
{
    public interface ITransactionRepository : IGenericRepository<Models.Transaction>
    {
        Task<List<Models.Transaction>> GetAllTransactions(int walletId);
        Task<List<Models.Transaction>> GetAllTransactionPayments();
        Task<bool> AlreadyRefundedAsync(int orderId);
    }
}
