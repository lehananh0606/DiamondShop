using AutoMapper;
using Service.ViewModels.Request;
using ShopRepository.Models;
using Service.ViewModels.Response;

using Service.Utils;
using Service.ViewModels.AccountToken;
using Service.ViewModels.Request.User;
using Service.ViewModels.Response.User;
using Service.ViewModels.Request.Auctions;
using Service.ViewModels.Request.Order;
using Service.ViewModels.Request.Bid;


namespace Service.Commons
{
    public class AutoMapperService : Profile
    {

        public AutoMapperService(
        )
        {
            CreateMap<User, AccountResponse>().ForMember(dept => dept.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<User, UserResponse>()
                .ForMember(dept => dept.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dept => dept.Status, opt => opt.MapFrom(src => StatusUtils.ChangeUserStatus((int)src.Status)));
            CreateMap<AccountRequest, User>();
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();

            // Token Mappings
            CreateMap<AccountTokenRequest, AccountToken>();
            CreateMap<AccountToken, AccountTokenRequest>();

            // Bid Mappings
            CreateMap<BidRequest, Bid>();
            CreateMap<Bid, BidResponse>();

            // Notification Mappings
            CreateMap<NotificationRequest, Notification>();
            CreateMap<Notification, NotificationResponse>();

            // Product Image Mappings
            CreateMap<ProductImageRequest, ProductImage>();
            CreateMap<ProductImage, ProductImageResponse>();

            // Transaction Mappings
            CreateMap<TransactionRequest, Transaction>();
            CreateMap<Transaction, TransactionResponse>();

            // Wallet Mappings
            CreateMap<WalletRequest, Wallet>();
            CreateMap<Wallet, WalletResponse>();

            // Auction Mappings
            CreateMap<Auction, AuctionRequest>();
            CreateMap<Auction, AuctionResponse>();
            CreateMap<CreateAuctionRequest, Auction>();
            CreateMap<StaffUpdate, Auction>();
            CreateMap<AdminApproveRequest, Auction>();
            CreateMap<StaffConfirmRequest, Auction>();
            CreateMap<UserWaitingRequest, Auction>();
            CreateMap<UserComming, Auction>();
            CreateMap<AuctionResponse, Auction>();


            // Order Mappings
            CreateMap<Order, OrderResponse>();
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<UpdateOrderRequest, Order>();

            // Bid Mappings (Duplicate, avoid redundancy)
            CreateMap<Bid, BidResponse>();
            CreateMap<CreateBidRequest, Bid>();
            CreateMap<UpdateBidRequest, Bid>();
        }
    }
}


// Example
/*CreateMap<TransactionHistoryDTO, TransactionHistory>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => _unitOfWork.CustomerRepo.GetById(src.CustomerId)))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => _unitOfWork.PaymentRepo.GetById(src.PaymentId)))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => _unitOfWork.AccountTypeRepo.GetById(src.AccountTypeId)))
                .AfterMap(async (src, dest, context) =>
                {
                    var accountType = await _unitOfWork.AccountTypeRepo.GetByIdAsync(src.AccountTypeId);
                    dest.AccountType = accountType;

                    var payment = await _unitOfWork.PaymentRepo.GetByIdAsync(src.PaymentId);
                    dest.Payment = payment;
                })
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.PaymentDate))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.PaymentDate.AddDays(30)))
                 .ReverseMap();*/