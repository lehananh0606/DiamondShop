using System.Linq.Expressions;
using LinqKit;
using Service.Commons.Enum;
using System.Linq.Dynamic.Core;

namespace Service.Commons;

public abstract class FilterRequest<T> where T : class
{
    public string? SortColumn { get; set; }

    public SortDirection SortDir { get; set; } = SortDirection.Asc;
    
    protected Expression Expression = PredicateBuilder.New<T>(true);

    public abstract Expression<Func<T, bool>> GetExpressions();

    public Func<IQueryable<T>, IOrderedQueryable<T>>? GetOrder()
    {
        if (string.IsNullOrWhiteSpace(SortColumn)) return null;

        return query => query.OrderBy($"{SortColumn} {SortDir.ToString().ToLower()}");
    }
}