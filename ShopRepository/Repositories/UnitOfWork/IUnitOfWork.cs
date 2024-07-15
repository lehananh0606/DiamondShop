
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage;
using ShopRepository.Repositories.IRepository;


namespace ShopRepository.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        public void Commit();
        public Task CommitAsync();
    }
}
