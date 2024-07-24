
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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DiamondShopContext context) : base(context)
        {
        }
        public async Task<Order> GetByAuctionIdAsync(int auctionId)
        {
            return await _dbSet
                 .FirstOrDefaultAsync(b => b.AuctionId == auctionId);
        }
    }
}
