using Service.Commons;
using Service.ViewModels.Request;
using Service.ViewModels.Request.User;
using Service.ViewModels.Response;
using Service.ViewModels.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
    
{
    public interface IUserService
    {
        public Task<GetUserResponse> GetUserAsync(GetUserRequest getUserRequest, IEnumerable<Claim> claims);
        // public Task<UserResponse> GetCashierReportAsync(IEnumerable<Claim> claims);
        public Task<UserResponse> GetAccountAsync(int id, IEnumerable<Claim> claims);
        public Task<OperationResult<UserResponse>> GetUserByName(string name);
        public Task<OperationResult<UserResponse>> GetUserByCreatedBy(string createdBy);
        public Task CreateUserAsync(CreateUserRequest createUserRequest, IEnumerable<Claim> claims);
        //public Task<UserResponse> GetCashierAsync(int idCashier, IEnumerable<Claim> claims);
        public Task UpdateUserAsync(int id, UpdateUserRequest updateUserRequest, IEnumerable<Claim> claims);
        public Task UpdateUserStatusAsync(int id, UpdateUserStatusRequest updateUserStatusRequest, IEnumerable<Claim> claims);
        public Task DeleteUserAsync(int id, IEnumerable<Claim> claims);
    }
}
