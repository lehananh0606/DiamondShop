using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Service.Commons;
using Service.IServices;
using Service.Utils;
using Service.ViewModels.Response;
using Service.ViewModels.Response.User;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly ILogger<WalletService> _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<WalletService> logger)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<OperationResult<WalletResponse>> GetWalletByUserId(int id)
        {
            var result = new OperationResult<WalletResponse>();

            try
            {
                // Query the user by name from the database asynchronously
                var wallet = await _unitOfWork.WalletRepository.GetWalletByAccountIdAsync(id);

                if (wallet == null)
                {
                    // Handle case where user is not found
                    result.AddError(StatusCode.NotFound, "Wallet", $"Wallet '{id}' not found.");
                    return result;
                }

                // Map the User entity to UserResponse view model
                var walletResponse = _mapper.Map<WalletResponse>(wallet);

                // Set payload and message for successful retrieval
                result.Payload = walletResponse;
                result.Message = "Wallet retrieved successfully.";

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                string error = ErrorUtil.GetErrorString("Exception", ex.Message);
                // Log or debug the error
                Console.WriteLine($"Error occurred: {error}");

                // Add error to result
                result.AddError(StatusCode.ServerError, "Exception", error);
                return result;
            }
        }
        public async Task<OperationResult<WalletResponse>> UpdateWalletBalance(int walletId, float balance)
        {
            var result = new OperationResult<WalletResponse>();
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null)
                {
                    result.AddError(StatusCode.NotFound, "Wallet", $"Wallet '{walletId}' not found.");
                    return result;
                }

                wallet.Balance = balance;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);

                var walletResponse = _mapper.Map<WalletResponse>(wallet); // Mapping nếu sử dụng AutoMapper
                return OperationResult<WalletResponse>.Success(walletResponse);
            }
            catch (Exception ex)
            {
                result.AddError(StatusCode.NotFound, "Wrong", $"Wallet");
                return result;
            }
        }

        public async Task<OperationResult<WalletResponse>> GetWalletByUserIdAsync(string token)
        {
            var result = new OperationResult<WalletResponse>();

            try
            {
                var key = Encoding.ASCII.GetBytes("your_secret_key_here");
                var handler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = handler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is JwtSecurityToken jwtToken)
                {
                    var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                    if (int.TryParse(userId, out int parsedUserId))
                    {
                        var wallet = await _unitOfWork.WalletRepository.GetWalletByAccountIdAsync(parsedUserId);

                        if (wallet != null)
                        {
                            var walletResponse = _mapper.Map<WalletResponse>(wallet);
                            result.Payload = walletResponse;
                            result.Message = "Wallet retrieved successfully.";
                        }
                        else
                        {
                            result.AddError(StatusCode.NotFound, "Wallet", $"Wallet for user '{parsedUserId}' not found.");
                        }
                    }
                    else
                    {
                        result.AddError(StatusCode.BadRequest, "Token", "Invalid user ID format.");
                    }
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Token", "Invalid token format.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the wallet.");
                result.AddError(StatusCode.ServerError, "Exception", "An unexpected error occurred.");
            }

            return result;
        }

    }
}
