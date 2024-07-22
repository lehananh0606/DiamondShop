using Service.Commons;
using Service.ViewModels.Response;
using ShopRepository.Models;
using System.Linq.Expressions;


namespace Service.IServices
{

    public interface ITransactionService
    {


        Task<OperationResult<IEnumerable<Transaction>>> GetAll(bool? isAscending, string? orderBy = null,
           Expression<Func<Transaction, bool>>? filter = null, string[]? includeProperties = null, int pageIndex = 0,
           int pageSize = 10);

        Task<OperationResult<List<TransactionResponse>>> GetAllTransaction(int accountId);

    }
}