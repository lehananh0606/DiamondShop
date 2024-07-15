
using ShopRepository.Models;
using ShopRepository.Repositories.GenericRepository;
using ShopRepository.Repositories.IRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Repositories.Repository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(DiamondShopContext context) : base(context)
        {
        }
    }
}
