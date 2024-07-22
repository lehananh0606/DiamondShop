using ShopRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IFirebaseService<T>
    {
        public Task<string> Save(object saveObj, long id, string collectionName);
        public Task<T> GetByKey(string key, string collectionName);
        public Task<bool> Delete(string key, string collectionName);
        public Task<List<T>> Get(string collectionName);
        public Task<Auction> GetAuctionByKey(string key, string collectionName);
        public Task<List<Auction>> GetAuctions(string collectionName, int? status = -1, DateTime? time = null);
    }
}
