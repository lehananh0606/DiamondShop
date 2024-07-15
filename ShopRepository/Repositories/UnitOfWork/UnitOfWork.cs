using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopRepository.Repositories.IRepository;
using ShopRepository.Repositories.Repository;
using ShopRepository.Models;

namespace ShopRepository.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DiamondShopContext _dbContext;
        private IDbFactory _dbFactory;

        private AuctionRepository _auctionRepository;

        private BidRepository _bidRepository;

        private NotificationRepository _notificationRepository;

        private OrderRepository _orderRepository;

        private ProductImageRepository _productImageRepository;

        private TransactionRepository _transactionRepository;

        private UserRepository _userRepository;

        private WalletRepository _walletRepository;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
            if (this._dbContext == null)
            {
                this._dbContext = dbFactory.InitDbContext();
            }
        }


        public AuctionRepository AuctionRepository
        {
            get
            {
                if (_auctionRepository == null)
                {
                    _auctionRepository = new AuctionRepository(_dbContext);
                }
                return _auctionRepository;
            }
        }

        public BidRepository BidRepository
        {
            get
            {
                if (_bidRepository == null)
                {
                    _bidRepository = new BidRepository(_dbContext);
                }
                return _bidRepository;
            }
        }

        public NotificationRepository NotificationRepository
        {
            get
            {
                if (_notificationRepository == null)
                {
                    _notificationRepository = new NotificationRepository(_dbContext);
                }
                return _notificationRepository;
            }
        }

        public OrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_dbContext);
                }
                return _orderRepository;
            }
        }

        public ProductImageRepository ProductImageRepository
        {
            get
            {
                if (_productImageRepository == null)
                {
                    _productImageRepository = new ProductImageRepository(_dbContext);
                }
                return _productImageRepository;
            }
        }

        public TransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository = new TransactionRepository(_dbContext);
                }
                return _transactionRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_dbContext);
                }
                return _userRepository;
            }
        }

        public WalletRepository WalletRepository
        {
            get
            {
                if (_walletRepository == null)
                {
                    _walletRepository = new WalletRepository(_dbContext);
                }
                return _walletRepository;
            }
        }

        public void Commit()
        {
            this._dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await this._dbContext.SaveChangesAsync();
        }
        
        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        
    }
}
