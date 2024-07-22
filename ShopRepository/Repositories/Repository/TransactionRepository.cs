
using ShopRepository.Repositories.GenericRepository;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Models;
using ShopRepository.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopRepository.Enums;
using Microsoft.EntityFrameworkCore;

namespace ShopRepository.Repositories.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DiamondShopContext context) : base(context)
        {
        }
        public async Task<bool> AlreadyRefundedAsync(int orderId)
        {
            return await Task.FromResult(AlreadyRefunded(orderId));
        }

        public async Task<List<Transaction>> GetAllTransactionPayments()
        {
            return await _dbSet
                .Where(pst => pst.TransactionType == TransactionEnum.PAYMENT.ToString())
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetAllTransactions(int walletId)
        {
            return await _dbSet
                .Where(t => t.WalletId == walletId || t.WalletId == walletId)
                .Include(w => w.Wallet)
                .ThenInclude(a => a.User)
                .ToListAsync();
        }

        private bool AlreadyRefunded(int orderId)
        {
            return _dbSet.Any(t => t.OrderId == orderId && t.TransactionType == TransactionEnum.REFUND.ToString());
        }
    }
}
