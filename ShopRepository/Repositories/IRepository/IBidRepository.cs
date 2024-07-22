using ShopRepository.Models;
using ShopRepository.Repositories.GenericRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Repositories.IRepository
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task<Bid> FindByUserIdAndAuctionId(int userId, int auctionId);
        Task<Bid> FindTop1ByAuctionId(int auctionId);
        Task<bool> ExistsBidByAuctionIdAndUserIdAsync(int auctionId, int userId);
    }
}
