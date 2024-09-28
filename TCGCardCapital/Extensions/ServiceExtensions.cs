using TCGCardCapital.Services.IService;
using TCGCardCapital.Services.ServiceImpl;

namespace TCGCardCapital.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRewardService, RewardService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITournamentRegistrationService, TournamentRegistrationService>();
            services.AddScoped<ITournamentRankingService, TournamentRankingService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
