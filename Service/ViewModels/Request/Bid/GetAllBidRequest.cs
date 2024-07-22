using LinqKit;
using Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Bid
{
    public class GetAllBidRequest : PaginationRequest<ShopRepository.Models.Bid>
    {
        public string? Search { get; set; }

        public override Expression<Func<ShopRepository.Models.Bid, bool>> GetExpressions()
        {

            if (!string.IsNullOrWhiteSpace(Search))
            {
                Search = Search.Trim().ToLower();

                var queryExpression = PredicateBuilder.New<ShopRepository.Models.Bid>(true);            
                queryExpression.Or(cus => cus.UserName.ToLower().Contains(Search));

                Expression = Expression.And(queryExpression);
            }

            Expression = Expression.And(u => u.IsDeleted == false);

            return Expression;
        }
    }
}
