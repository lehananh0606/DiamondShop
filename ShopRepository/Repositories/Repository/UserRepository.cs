using Microsoft.EntityFrameworkCore;

using ShopRepository.Enums;
using ShopRepository.Models;
using ShopRepository.Repositories.GenericRepository;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Repositories.IRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Repositories.Repository
{
    public class UserRepository
    {
        private readonly DiamondShopContext _dbContext;

        public UserRepository(DiamondShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserAsync(int accountId)
        {
            try
            {
                return await this._dbContext.Users.Include(x => x.Role)
                                                     .SingleOrDefaultAsync(x => x.UserId == accountId && x.Status != (int)CustomerStatus.Status.DISABLE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            try
            {
                return await _dbContext.Users
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Name == name && x.Status != (int)CustomerStatus.Status.DISABLE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<User>> GetUsersByCreatedByAsync(string createdBy)
        {
            try
            {
                return await _dbContext.Users
                                       .Where(x => x.CreatedBy == createdBy)
                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserAsyncUpdate(int accountId)
        {
            try
            {
                return await this._dbContext.Users.Include(x => x.Role)
                                                     .SingleOrDefaultAsync(x => x.UserId == accountId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> GetByIdAsync(int id, params Expression<Func<User, object>>[] includeProperties)
        {
            IQueryable<User> query = _dbContext.Users;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.SingleOrDefaultAsync(e => e.UserId == id);
        }

        public async Task<User> GetUserAsync(string email)
        {
            try
            {
                return await this._dbContext.Users.Include(x => x.Role)
                                                     .SingleOrDefaultAsync(x => x.Email == email && x.Status != (int)CustomerStatus.Status.DISABLE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetActiveAccountAsync(string email)
        {
            try
            {
                return await this._dbContext.Users.Include(x => x.Role)
                                                     .SingleOrDefaultAsync(x => x.Email.Equals(email) && x.Status == (int)CustomerStatus.Status.ACTIVE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetNumberUsersAsync(string? searchValue)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    return await _dbContext.Users
                        .Where(u => EF.Functions.Like(u.Name, $"%{searchValue}%"))
                        .CountAsync();
                }
                else
                {
                    return await _dbContext.Users.CountAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                await this._dbContext.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserAsync(User updatedUser)
        {
            try
            {
                // Kiểm tra xem người dùng có tồn tại trong cơ sở dữ liệu không
                var existingUser = await _dbContext.Users.FindAsync(updatedUser.UserId);
                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }

                // Cập nhật thông tin người dùng
                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;
                existingUser.ImageUrl = updatedUser.ImageUrl;
                existingUser.Dob = updatedUser.Dob;
                existingUser.Status = updatedUser.Status;
                //existingUser.RoleName = updatedUser.RoleName;
                existingUser.IsBanned = updatedUser.IsBanned;
                existingUser.ExpiredAt = updatedUser.ExpiredAt;
                existingUser.CreatedAt = updatedUser.CreatedAt;
                existingUser.CreatedBy = updatedUser.CreatedBy;
                existingUser.ModifiedBy = updatedUser.ModifiedBy;
                existingUser.UpdatedAt = updatedUser.UpdatedAt;
                existingUser.IsDeleted = updatedUser.IsDeleted;
                existingUser.ModifiedVersion = updatedUser.ModifiedVersion;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                throw new Exception("Error updating user: " + ex.Message);
            }
        }

        

        public async Task DeleteUserAsync(int id, IEnumerable<Claim> claims)
        {
            try
            {
                // Kiểm tra quyền truy cập
                if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
                {
                    throw new Exception("Unauthorized access.");
                }

                // Truy vấn người dùng cần xóa
                var userToDelete = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (userToDelete == null)
                {
                    throw new Exception("User not found.");
                }

                // Xóa người dùng
                _dbContext.Users.Remove(userToDelete);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                throw new Exception("Error deleting user: " + ex.Message);
            }
        }

        public async Task<List<User>> GetUsersAsync(string? searchValue, int currentPage, int itemsPerPage, string? sortByASC, string? sortByDESC)
        {
            try
            {
                IQueryable<User> query = _dbContext.Users;

                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    query = query.Where(u => EF.Functions.Like(u.Name, $"%{searchValue}%"));
                }

                if (!string.IsNullOrWhiteSpace(sortByASC))
                {
                    query = query.OrderByProperty(sortByASC, "asc");
                }

                if (!string.IsNullOrWhiteSpace(sortByDESC))
                {
                    query = query.OrderByProperty(sortByDESC, "desc");
                }

                return await query.Skip((currentPage - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, string order)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = order == "asc" ? "OrderBy" : "OrderByDescending";
            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), property.Type },
                source.Expression,
                lambda
            );

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }

}
