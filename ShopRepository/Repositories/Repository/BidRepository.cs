using ShopRepository.Models;
using ShopRepository.Repositories.GenericRepository;
using ShopRepository.Repositories.IRepository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShopRepository.Repositories.Repository
{
    public class BidRepository : GenericRepository<Bid>, IBidRepository
    {
        public BidRepository(DiamondShopContext context) : base(context)
        {
        }

        public async Task<Bid> FindByUserIdAndAuctionId(int userId, int auctionId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.UserId == userId && b.AuctionId == auctionId);
        }

        public async Task<Bid> FindTop1ByAuctionId(int auctionId)
        {
            return await _dbSet
                .Where(b => b.AuctionId == auctionId && b.IsTop1 == true)
                .OrderByDescending(b => b.BiddingPrice)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> ExistsBidByAuctionIdAndUserIdAsync(int auctionId, int userId)
        {
            return await _dbSet
                .AnyAsync(bid => bid.AuctionId == auctionId && bid.UserId == userId);
        }
    }
}
