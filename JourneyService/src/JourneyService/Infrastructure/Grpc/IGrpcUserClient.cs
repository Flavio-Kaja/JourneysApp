using JourneyService.Infrastructure.Models;
using JourneyService.Services;
using JourneyService.Wrappers;

namespace JourneyService.Infrastructure.Grpc
{
    public interface IGrpcUserClient : IJourneyServiceScopedService
    {
        PagedList<UserDto> GetUsers(UserParametersDto userParams);
    }
}
