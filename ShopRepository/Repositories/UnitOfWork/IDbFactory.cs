

using ShopRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Repositories.UnitOfWork
{
    public interface IDbFactory : IDisposable
    {
        public DiamondShopContext InitDbContext();
      //  public Task<RedisConnectionProvider> InitRedisConnectionProvider();
    }
}
