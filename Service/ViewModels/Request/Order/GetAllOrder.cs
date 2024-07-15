using LinqKit;
using Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Order
{
    public class GetAllOrder : PaginationRequest<ShopRepository.Models.Order>
    {
        public string? Search { get; set; }

        public override Expression<Func<ShopRepository.Models.Order, bool>> GetExpressions()
        {

            if (!string.IsNullOrWhiteSpace(Search))
            {
                Search = Search.Trim().ToLower();

                var queryExpression = PredicateBuilder.New<ShopRepository.Models.Order>(true);
                queryExpression.Or(cus => cus.AuctionTitle.ToLower().Contains(Search));
                queryExpression.Or(cus => cus.AuctionCode.ToLower().Contains(Search));
                queryExpression.Or(cus => cus.AuctionName.ToLower().Contains(Search));

                Expression = Expression.And(queryExpression);
            }

            Expression = Expression.And(u => u.IsDeleted == false);

            return Expression;
        }
    }
}
