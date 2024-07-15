using Quartz;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Quartz
{
    public class EveryMinute : IJob
    {
        private readonly UnitOfWork _unitOfWork;
        public EveryMinute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("intro the EveryMinute");

            //var timeThresholdBefore = DateTime.Now.AddHours(-48);
            var timeThreshold = DateTime.Now;

            var statuses = new List<int?> { 5,6 };

            var auctions = _unitOfWork.AuctionRepository.Get(
               filter: u => statuses.Contains(u.Status)
               && u.IsActived == true
               && u.IsRejected == false
               && u.StartDate >= timeThreshold,

               pageSize: -1
            );

            foreach (var auction in auctions)
            {
                string msg = "update auction " + auction.AuctionId
                    + " statusfrom: " + auction.Status;

                auction.Status = auction.Status + 1;

                msg += " statusTo: " + auction.Status;

                auction.UpdateAt = DateTime.Now;

                Console.WriteLine(msg);
                _unitOfWork.AuctionRepository.Update(auction);
            }

            await _unitOfWork.SaveChangesAsync();


        }
    }
}
