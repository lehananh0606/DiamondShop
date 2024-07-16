
using ShopRepository.Repositories.GenericRepository;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Models;
using ShopRepository.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShopRepository.Repositories.Repository
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(DiamondShopContext context) : base(context)
        {
            
        }


        public new Wallet Remove(Wallet entity)
        { 
            _dbSet.Update(entity);
            return entity;
        }

        public async Task<Wallet> GetWalletByAccountIdAsync(int accountId)
        {
            return await _dbSet
                .Where(a => a.UserId == accountId)
                .FirstOrDefaultAsync();
        }

        public async Task<Wallet> UpdateBalance(int walletId, decimal balance)
        {
            var wallet = await _dbSet.FindAsync(walletId);
            if (wallet == null)
            {
                throw new KeyNotFoundException("Wallet not found.");
            }
            wallet.Balance = (float?)balance;
            _dbSet.Update(wallet);
            return wallet;
        }

        public Wallet GetById(int walletId)
        {
            return _dbSet.FirstOrDefault(w => w.WalletId == walletId);
        }
    }
}
