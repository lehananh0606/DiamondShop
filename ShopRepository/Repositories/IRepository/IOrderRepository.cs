using ShopRepository.Models;
using ShopRepository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Repositories.IRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
    }
}
