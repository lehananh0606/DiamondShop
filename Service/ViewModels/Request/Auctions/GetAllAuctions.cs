using System.Linq.Expressions;
using LinqKit;
using Service.Commons;
using ShopRepository.Models;

namespace Service.ViewModels.Request;

public class GetAllAuctions: PaginationRequest<ShopRepository.Models.Auction>
{
    public string? Search { get; set; }
    
    public override Expression<Func<ShopRepository.Models.Auction, bool>> GetExpressions()
    {
        
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Search = Search.Trim().ToLower();
            
            var queryExpression =  PredicateBuilder.New<ShopRepository.Models.Auction>(true);
            queryExpression.Or(cus => cus.ProductName.ToLower().Contains(Search));
            queryExpression.Or(cus => cus.ProductCode.ToLower().Contains(Search));
            queryExpression.Or(cus => cus.Description.ToLower().Contains(Search));

            Expression = Expression.And(queryExpression);
        }

        Expression = Expression.And(u => u.IsDeleted == false);
        
        return Expression;
    }
}