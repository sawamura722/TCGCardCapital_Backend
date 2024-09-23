using AutoMapper;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;

namespace TCGCardCapital.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
            CreateMap<Reward, RewardDTO>().ReverseMap();
            CreateMap<Tournament, TournamentDTO>().ReverseMap();
            CreateMap<Ranking, TournamentRegistrationDTO>().ReverseMap();
            CreateMap<Ranking, TournamentRankingDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();

            // User to UserDTO mapping, ignoring PasswordHash
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            // UserDTO to User mapping, ignoring PasswordHash since it will be handled separately
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Handle hashing separately
        }
    }
}
