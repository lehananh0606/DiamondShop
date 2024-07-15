
using ShopRepository.Repositories.GenericRepository;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Models;
using ShopRepository.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Repositories.Repository
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(DiamondShopContext context) : base(context)
        {
        }
    }
}
