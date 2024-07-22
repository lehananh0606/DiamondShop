using Quartz;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Quartz
{
    public class MonthlyAt1AMOn1st : IJob
    {
        private readonly UnitOfWork _unitOfWork;
        public MonthlyAt1AMOn1st(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("intro the MonthlyAt1AMOn1st !!!");


            //var timeThresholdBefore = DateTime.Now.AddHours(-48);
            var timeThresholdAfter = DateTime.Now.AddDays(40);

            var statuses = new List<int?> { 0, 2 }; 

            var auctions = _unitOfWork.AuctionRepository.Get(
               filter: u => statuses.Contains(u.Status)
               //&& u.UpdateAt >= timeThresholdBefore
               && u.UpdateAt <= timeThresholdAfter,
               pageSize: -1
            );

            foreach (var auction in auctions)
            {
                string msg = "Expire by system form stautus: " + auction.Status;
                auction.Status = 7;
                auction.ExpiredAt = DateTime.Now;
                auction.IsExpired = true;
                auction.IsRejected = true;
                auction.RejecrReason = msg;
                auction.UpdateAt = DateTime.Now;

                Console.WriteLine(msg + " autionId " + auction.AuctionId);

                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
            }

            await _unitOfWork.SaveChangesAsync();


        }
    }
}
