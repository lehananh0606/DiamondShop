using ShopRepository.Models;
using ShopRepository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Repositories.IRepository
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet> GetWalletByAccountIdAsync(int accountId);
        Task<Wallet> UpdateBalance(int walletId, decimal balance);
        Wallet GetById(int walletId);
    }
}
