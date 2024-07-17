using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.IServices;
using Service.Library;
using Service.ViewModels.Request;
using Service.ViewModels.Response;
using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Exceptions;
using Service.Commons;
using ShopRepository.Models;
using ShopRepository.Repositories.UnitOfWork;

namespace Service.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IAuctionService _auctionService;
        private readonly IWalletService _walletService;
        private readonly UnitOfWork _unitOfWork;

        public VnPayService(IConfiguration config, IUserService userService, IAuctionService auctionService, IWalletService walletService, IUnitOfWork unitOfWork)
        {
            _config = config;
            _userService = userService;
            _auctionService = auctionService;
            _walletService = walletService;
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            try
            {
                var time = DateTime.Now;
                var vnpay = new VnPayLibrary();

                // Get auction details asynchronously
                var auctionResult = await _auctionService.GetById(model.AuctionId);

                if (auctionResult.IsError || auctionResult.Payload == null)
                {
                    throw new NotFoundException("Auction not found.");
                }

                var auction = auctionResult.Payload;

                // Find User by createdBy field of Auction
                var userResult = await _userService.GetUserByName(auction.CreatedBy);

                if (userResult.IsError || userResult.Payload == null)
                {
                    throw new NotFoundException("User not found.");
                }

                var user = userResult.Payload;

                // Retrieve wallet by user ID
                var walletResult = await _walletService.GetWalletByUserId(user.UserID);

                if (walletResult.IsError || walletResult.Payload == null)
                {
                    throw new NotFoundException("Wallet not found.");
                }

                var wallet = walletResult.Payload;

                // Add VnPay request data
                vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
                vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
                vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
                vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
                vnpay.AddRequestData("vnp_CreateDate", time.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
                vnpay.AddRequestData("vnp_IpAddr", Utilss.GetIpAddress(context));
                vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
                vnpay.AddRequestData("vnp_OrderInfo", wallet.WalletId.ToString());
                vnpay.AddRequestData("vnp_OrderType", PaymentMethod.DEPOSIT.ToString());
                vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
                vnpay.AddRequestData("vnp_TxnRef", time.Ticks.ToString());

                // Create payment URL
                var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
                return paymentUrl;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw;
            }
        }

        public async Task<VnPaymentResponseModel> PaymentExecute(IQueryCollection collections)
        {
            try
            {
                var vnpay = new VnPayLibrary();

                foreach (var (key, value) in collections)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value.ToString());
                    }
                }

                var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
                if (!checkSignature)
                {
                    return new VnPaymentResponseModel { Success = false };
                }

                // Extract wallet ID and amount from the response
                var walletId = int.Parse(vnpay.GetResponseData("vnp_OrderInfo"));
                var amount = Convert.ToSingle(vnpay.GetResponseData("vnp_Amount")) / 100f;

                // Get auction details asynchronously
                var auctionResult = await _auctionService.GetById(walletId);

                if (auctionResult.IsError || auctionResult.Payload == null)
                {
                    throw new NotFoundException("Auction not found.");
                }

                var auction = auctionResult.Payload;

                // Find User by createdBy field of Auction
                var userResult = await _userService.GetUserByName(auction.CreatedBy);

                if (userResult.IsError || userResult.Payload == null)
                {
                    throw new NotFoundException("User not found.");
                }

                var user = userResult.Payload;

                // Retrieve wallet by user ID
                var walletResult = await _walletService.GetWalletByUserId(user.UserID);

                if (walletResult.IsError || walletResult.Payload == null)
                {
                    throw new NotFoundException("Wallet not found.");
                }

                var wallet = walletResult.Payload;

                // Update Wallet balance
                wallet.Balance += amount;

                // Save the updated wallet balance
                var updateResult = await _walletService.UpdateWalletBalance(wallet.WalletId, wallet.Balance);
                if (updateResult.IsError)
                {
                    throw new Exception("Failed to update wallet balance.");
                }

                return new VnPaymentResponseModel
                {
                    Success = true,
                    WalletId = wallet.WalletId,
                    BankCode = vnpay.GetResponseData("vnp_BankCode"),
                    BankTranNo = vnpay.GetResponseData("vnp_BankTranNo"),
                    CardType = vnpay.GetResponseData("vnp_CardType"),
                    Amount = amount,
                    Token = vnp_SecureHash,
                    VnPayResponseCode = vnpay.GetResponseData("vnp_ResponseCode")
                };
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw;
            }
        }

        public async Task<OperationResult<Transaction>> DepositPayment(VnPaymentResponseModel response)
        {
            var result = new OperationResult<Transaction>();
            var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(response.WalletId);

                if (wallet == null)
                {
                    result.Message = "Wallet not found";
                    result.IsError = true;
                    result.Errors.Add(new Error()
                    {
                        StatusCode = (int)StatusCode.NotFound,
                        Message = new List<ErrorDetail>
                        {
                            new ErrorDetail { FieldNameError = "Wallet", DescriptionError = new List<string> { "Wallet not found" } }
                        }
                    });
                    return result;
                }

                var payment = new Transaction
                {
                    WalletId = response.WalletId,
                    Amount = response.Amount,
                    Status = PaymentEnum.SUCCESS.ToString(),
                    TransactionType = PaymentMethod.DEPOSIT.ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Resource = response.BankCode,
                    TransactionCode = "DEP" + Utilss.RandomString(7)
                };

                // Save the payment
                await _unitOfWork.TransactionRepository.AddAsync(payment);
                var count = await _unitOfWork.SaveChangesAsync();

                if (count == 0)
                {
                    await transaction.RollbackAsync();
                    result.Message = "Payment failed";
                    result.IsError = true;
                    result.Errors.Add(new Error()
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = new List<ErrorDetail>
                        {
                            new ErrorDetail { FieldNameError = "Payment", DescriptionError = new List<string> { "Payment fail" } }
                        }
                    });
                    return result;
                }

                // Update wallet balance
                wallet.Balance += payment.Amount;
                wallet.UpdatedAt = DateTime.Now;

                await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                var countUpdate = await _unitOfWork.SaveChangesAsync();

                if (countUpdate == 0)
                {
                    await transaction.RollbackAsync();
                    result.Message = "Payment failed";
                    result.IsError = true;
                    result.Errors.Add(new Error()
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = new List<ErrorDetail>
                        {
                            new ErrorDetail { FieldNameError = "Payment", DescriptionError = new List<string> { "Payment fail" } }
                        }
                    });
                    return result;
                }

                await transaction.CommitAsync();
                result.Payload = payment;
                result.IsError = false;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.Message = e.Message;
                result.IsError = true;
                result.Errors.Add(new Error()
                {
                    StatusCode = (int)StatusCode.ServerError,
                    Message = new List<ErrorDetail>
                    {
                        new ErrorDetail { FieldNameError = "Exception", DescriptionError = new List<string> { e.Message } },
                        new ErrorDetail { FieldNameError = "Additional detail", DescriptionError = new List<string> { "Additional error information" } }
                    }
                });
            }

            return result;
        }
    }
}
