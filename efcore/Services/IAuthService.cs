
namespace efcore.Services
{
    public interface IAuthService
    {
        Task<UserOutputDto?> RegisterUserAsync(UserInputDto request);
        Task<UserOutputDto?> RegisterManagerAsync(UserInputDto request);
        Task<string?> LoginAsync(UserInputDto request);
    }
}