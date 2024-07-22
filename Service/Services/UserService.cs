using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Commons;
using Service.Contants;
using Service.Exceptions;
using Service.IServices;
using Service.Utils;
using Service.ViewModels.Request;
using Service.ViewModels.Request.User;
using Service.ViewModels.Response.User;
using ShopRepository.Enums;
using ShopRepository.Models;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Repositories.Repository;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService
    {

        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        public async Task<GetUserResponse> GetUserAsync(GetUserRequest getUserRequest, IEnumerable<Claim> claims)
        {
            try
            {
                // Kiểm tra xem danh sách claims có chứa phần tử nào với kiểu ClaimTypes.Email không
                var registeredEmailClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (registeredEmailClaim == null)
                {
                    throw new Exception("No email claim found.");
                }

                // Lấy email từ phần tử claim có kiểu ClaimTypes.Email
                string email = registeredEmailClaim.Value;

                // Khởi tạo các biến để lưu số lượng Users và danh sách Users
                int numberItems = 0;
                List<User> users = null;

                // Kiểm tra xem getUserRequest có giá trị null không
                if (getUserRequest == null)
                {
                    // Truy vấn tất cả người dùng từ cơ sở dữ liệu
                    numberItems = await _unitOfWork.UserRepository.GetNumberUsersAsync(null);
                    users = await _unitOfWork.UserRepository.GetUsersAsync(null, 1, 5, null, null);
                }
                else
                {
                    // Kiểm tra và lấy số lượng và danh sách Users dựa trên yêu cầu từ getUserRequest
                    if (!string.IsNullOrWhiteSpace(getUserRequest.SearchValue))
                    {
                        numberItems = await _unitOfWork.UserRepository.GetNumberUsersAsync(getUserRequest.SearchValue);

                        users = await _unitOfWork.UserRepository.GetUsersAsync(getUserRequest.SearchValue, getUserRequest.CurrentPage.Value, getUserRequest.ItemsPerPage.Value,
                                                                               GetSortBy(getUserRequest.SortBy, "asc"), GetSortBy(getUserRequest.SortBy, "desc"));
                    }
                    else
                    {
                        numberItems = await _unitOfWork.UserRepository.GetNumberUsersAsync(null);
                        users = await _unitOfWork.UserRepository.GetUsersAsync(null, getUserRequest.CurrentPage.Value, getUserRequest.ItemsPerPage.Value,
                                                                               GetSortBy(getUserRequest.SortBy, "asc"), GetSortBy(getUserRequest.SortBy, "desc"));
                    }
                }

                // Tính toán tổng số trang dựa trên số lượng Users và số lượng hiển thị trên mỗi trang
                int totalPages = numberItems > 0 ? (int)Math.Ceiling((decimal)numberItems / getUserRequest.ItemsPerPage.Value) : 0;

                // Chuyển đổi danh sách Users sang đối tượng GetUserResponse
                List<UserResponse> getUserResponses = _mapper.Map<List<UserResponse>>(users);

                // Trả về đối tượng GetUserResponse
                return new GetUserResponse()
                {
                    TotalPages = totalPages,
                    NumberItems = numberItems,
                    Users = getUserResponses
                };
            }
            catch (Exception ex)
            {
                string error = ErrorUtil.GetErrorString("Exception", ex.Message);
                throw new Exception(error);
            }
        }



        public async Task<GetUserResponse> GetUsersAsync(GetUserRequest getUsersRequest, IEnumerable<Claim> claims)
        {
            try
            {
                // Lấy thông tin Claim về email đăng ký từ danh sách claims
                Claim registeredEmailClaim = claims.First(x => x.Type == ClaimTypes.Email);
                string email = registeredEmailClaim.Value;

                // Khởi tạo các biến để lưu số lượng Users và danh sách Users
                int numberItems = 0;
                List<User> users = null;

                // Kiểm tra và lấy số lượng và danh sách Users dựa trên yêu cầu từ getUsersRequest
                if (!string.IsNullOrWhiteSpace(getUsersRequest.SearchValue))
                {
                    numberItems = await _unitOfWork.UserRepository.GetNumberUsersAsync(getUsersRequest.SearchValue);

                    users = await _unitOfWork.UserRepository.GetUsersAsync(getUsersRequest.SearchValue, getUsersRequest.CurrentPage.Value, getUsersRequest.ItemsPerPage.Value,
                                                                           GetSortBy(getUsersRequest.SortBy, "asc"), GetSortBy(getUsersRequest.SortBy, "desc"));
                }
                else
                {
                    numberItems = await _unitOfWork.UserRepository.GetNumberUsersAsync(null);
                    users = await _unitOfWork.UserRepository.GetUsersAsync(null, getUsersRequest.CurrentPage.Value, getUsersRequest.ItemsPerPage.Value,
                                                                           GetSortBy(getUsersRequest.SortBy, "asc"), GetSortBy(getUsersRequest.SortBy, "desc"));
                }

                // Tính toán tổng số trang dựa trên số lượng Users và số lượng hiển thị trên mỗi trang
                int totalPages = numberItems > 0 ? (int)Math.Ceiling((decimal)numberItems / getUsersRequest.ItemsPerPage.Value) : 0;

                // Chuyển đổi danh sách Users sang đối tượng GetUserResponse
                List<UserResponse> getUserResponses = _mapper.Map<List<UserResponse>>(users);

                // Trả về đối tượng GetUsersResponse
                return new GetUserResponse()
                {
                    TotalPages = totalPages,
                    NumberItems = numberItems,
                    Users = getUserResponses
                };
            }
            catch (Exception ex)
            {
                string error = ErrorUtil.GetErrorString("Exception", ex.Message);
                throw new Exception(error);
            }
        }

        // Phương thức hỗ trợ để lấy chuỗi sắp xếp theo chiều tăng hoặc giảm dần
        private string? GetSortBy(string? sortBy, string direction)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return null;
            }
            return sortBy.ToLower().EndsWith(direction) ? sortBy.Split("_")[0] : null;
        }


        // Phương thức hỗ trợ để lấy chuỗi sắp xếp theo chiều tăng hoặc giảm dần


        public async Task CreateUserAsync(CreateUserRequest createUserRequest, IEnumerable<Claim> claims)
        {
            try
            {
                
                var newUser = _mapper.Map<User>(createUserRequest);
                await _unitOfWork.UserRepository.CreateUserAsync(newUser);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {               
                throw new Exception("Error creating new user: " + ex.Message);
            }
        }


        public async Task UpdateUserAsync(int id, UpdateUserRequest updateUserRequest, IEnumerable<Claim> claims)
        {
            try
            {
                // Thực hiện kiểm tra xác thực, ví dụ: kiểm tra quyền truy cập của người dùng
                // ở đây có thể sử dụng claims để kiểm tra quyền hạn

                // Kiểm tra xem người dùng có tồn tại không
                var existingUser = await _unitOfWork.UserRepository.GetUserAsync(id);
                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }

                // Sử dụng AutoMapper để ánh xạ từ UpdateUserRequest sang Users
                var updatedUser = _mapper.Map(updateUserRequest, existingUser);

                // Gọi phương thức UpdateUserAsync từ UserRepository để cập nhật thông tin người dùng
                await _unitOfWork.UserRepository.UpdateUserAsync(updatedUser);

                // Các bước khác sau khi cập nhật thành công có thể được thêm ở đây

            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                throw new Exception("Error updating user: " + ex.Message);
            }
        }

        public async Task UpdateUserStatusAsync(int id, UpdateUserStatusRequest updateUserStatusRequest, IEnumerable<Claim> claims)
        {
            try
            {
                // Kiểm tra quyền hạn
                // TODO: Thêm mã kiểm tra quyền hạn ở đây

                // Lấy người dùng từ UserRepository
                var userToUpdate = await _unitOfWork.UserRepository.GetUserAsyncUpdate(id);
                if (userToUpdate == null)
                {
                    throw new NotFoundException("User not found");
                }

                // Kiểm tra trạng thái request có hợp lệ không
                if (!Enum.TryParse(updateUserStatusRequest.Status, out CustomerStatus.Status statusEnum))
                {
                    throw new BadRequestException("Invalid status value");
                }

                // Cập nhật trạng thái người dùng
                userToUpdate.Status = (int)statusEnum;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _unitOfWork.CommitAsync();
            }
            catch (NotFoundException ex)
            {
                // Xử lý nếu không tìm thấy người dùng
                throw new NotFoundException($"User not found: {ex.Message}");
            }
            catch (BadRequestException ex)
            {
                // Xử lý nếu có lỗi truyền dữ liệu không hợp lệ
                throw new BadRequestException($"Invalid status value: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi không xác định
                throw new Exception($"Failed to update user status: {ex.Message}");
            }
        }

        public async Task<UserResponse> GetAccountAsync(int idAccount, IEnumerable<Claim> claims)
        {
            try
            {
                Claim registeredEmailClaim = claims.First(x => x.Type == ClaimTypes.Email);
                string email = registeredEmailClaim.Value;

                User existedAccount = await this._unitOfWork.UserRepository.GetUserAsync(idAccount);
                if (existedAccount is null)
                {
                    throw new NotFoundException(MessageConstant.CommonMessage.NotExistAccountId);
                }
                if (existedAccount.Email.Equals(email) == false)
                {
                    throw new BadRequestException(MessageConstant.AccountMessage.AccountIdNotBelongYourAccount);
                }
                UserResponse getAccountResponse = this._mapper.Map<UserResponse>(existedAccount);
                return getAccountResponse;
            }
            catch (NotFoundException ex)
            {
                string error = ErrorUtil.GetErrorString("Account id", ex.Message);
                throw new NotFoundException(error);
            }
            catch (BadRequestException ex)
            {
                string error = ErrorUtil.GetErrorString("Account id", ex.Message);
                throw new BadRequestException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorUtil.GetErrorString("Exception", ex.Message);
                throw new Exception(error);
            }
        }

        public async Task DeleteUserAsync(int id, IEnumerable<Claim> claims)
        {
            try
            {
                // Retrieve the user from the repository
                var userToDelete = await _unitOfWork.UserRepository.GetUserAsync(id);
                if (userToDelete == null)
                {
                    throw new Exception("User not found.");
                }

                // Set the IsDeleted field to true
                userToDelete.IsDeleted = true;

                // Commit the changes to the database
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting user: " + ex.Message);
            }
        }

        public async Task<OperationResult<UserResponse>> GetUserByCreatedBy(string createdBy)
        {
            var result = new OperationResult<UserResponse>();

            try
            {
                var users = await _unitOfWork.UserRepository.GetUsersByCreatedByAsync(createdBy);
                if (users == null || !users.Any())
                {
                    result.AddError(StatusCode.NotFound, "Users", "No users found created by the specified user.");
                    return result;
                }

                var userResponses = _mapper.Map<List<UserResponse>>(users);
                result.Payload = userResponses.FirstOrDefault(); // Assuming you want to return the first user found
                result.Message = "Users retrieved successfully.";
                return result;
            }
            catch (Exception ex)
            {
                result.AddError(StatusCode.ServerError, "Exception", ex.Message);
                return result;
            }
        }

        public async Task<OperationResult<UserResponse>> GetUserByName(string name)
        {
            var result = new OperationResult<UserResponse>();

            try
            {
                // Query the user by name from the database asynchronously
                var user = await _unitOfWork.UserRepository.GetUserByNameAsync(name);

                if (user == null)
                {
                    // Handle case where user is not found
                    result.AddError(StatusCode.NotFound, "User", $"User '{name}' not found.");
                    return result;
                }

                // Map the User entity to UserResponse view model
                var userResponse = _mapper.Map<UserResponse>(user);

                // Set payload and message for successful retrieval
                result.Payload = userResponse;
                result.Message = "User retrieved successfully.";

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


    }

}
