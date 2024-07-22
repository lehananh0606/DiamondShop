using AutoMapper;
using Microsoft.Extensions.Logging;
using Service.Commons;
using Service.IServices;
using Service.ViewModels.Response;
using ShopRepository.Models;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(UnitOfWork unitOfWork, IMapper mapper, ILogger<TransactionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<Transaction>>> GetAll(
        bool? isAscending,
        string? orderBy = null,
        Expression<Func<Transaction, bool>>? filter = null,
        string[]? includeProperties = null,
        int pageIndex = 0,
        int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<Transaction>>();
            try
            {
                var transactions = _unitOfWork.TransactionRepository.FilterAll(
                    isAscending,
                    orderBy,
                    filter,
                    includeProperties,
                    pageIndex,
                    pageSize);

                if (!transactions.Any())
                {
                    result.StatusCode = StatusCode.NoContent;
                    result.Message = "Chưa có giao dịch nào được tạo.";
                    return result;
                }

                result.Payload = transactions;
                result.Message = "Lấy danh sách giao dịch thành công.";
                result.StatusCode = StatusCode.Ok;
                result.IsError = false;
            }
            catch (Exception e)
            {
                result.StatusCode = StatusCode.ServerError;
                result.Message = e.Message;
                result.IsError = true;
                throw;
            }
            return result;
        }

        public async Task<OperationResult<List<TransactionResponse>>> GetAllTransaction(int accountId)
        {
            var result = new OperationResult<List<TransactionResponse>>();
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetWalletByAccountIdAsync(accountId);
                if (wallet != null)
                {
                    //var listTransactions = await _unitOfWork.TransactionRepository.GetAllTransactions(wallet.Id);
                    //var listTransactionsResponse = _mapper.Map<List<TransactionResponse>>(listTransactions);
                    //if (listTransactionsResponse == null || !listTransactionsResponse.Any())
                    //{
                    //    result.AddResponseStatusCode(StatusCode.Ok, $"List Transactions with accountId: {accountId} is Empty!", listTransactionsResponse);
                    //    return result;
                    //}
                    //result.AddResponseStatusCode(StatusCode.Ok, "Get List Transactions Done.", listTransactionsResponse);

                    var listTransactions = await _unitOfWork.TransactionRepository.GetAllTransactions(wallet.WalletId);
                    var listTransactionResponses = new List<TransactionResponse>();

                    foreach (var transaction in listTransactions)
                    {
                        var payeeWallet = _unitOfWork.WalletRepository.GetById((int)transaction.WalletId);
                        var payeeAccount = await _unitOfWork.UserRepository.GetByIdAsync((int)payeeWallet.UserId);

                        var transactionResponse = _mapper.Map<TransactionResponse>(transaction);
                        // Thiết lập PayeeName dựa trên thông tin Account tìm được
                        transactionResponse.CreatedBy = payeeAccount.Name;
                        listTransactionResponses.Add(transactionResponse);
                    }

                    if (!listTransactionResponses.Any())
                    {
                        result.AddResponseStatusCode(StatusCode.Ok, $"List Transactions with accountId: {accountId} is Empty!", listTransactionResponses);
                        return result;
                    }

                    result.AddResponseStatusCode(StatusCode.Ok, "Get List Transactions Done.", listTransactionResponses);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred in GetAllTransaction Service Method");
                throw;
            }
        }
    }
}
