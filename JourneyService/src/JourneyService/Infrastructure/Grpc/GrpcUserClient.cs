using Grpc.Net.Client;
using JourneyService.Infrastructure.Models;
using JourneyService.Wrappers;

namespace JourneyService.Infrastructure.Grpc
{
    public class GrpcUserClient : IGrpcUserClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GrpcUserClient> _logger;
        public GrpcUserClient(IConfiguration configuration, ILogger<GrpcUserClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public PagedList<UserDto> GetUsers(UserParametersDto userParams)
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcUserService"]);
            var client = new UserService.Protos.UserService.UserServiceClient(channel);
            var request = new UserService.Protos.UserParametersRequest();
            request.PageSize = userParams.PageSize;
            request.FirstName = userParams.FirstName;
            request.LastName = userParams.LastName;
            request.Email = userParams.Email;
            request.Descending = userParams.Descending;
            request.MaxDailyGoal = userParams.MaxDailyGoal;
            request.MinDailyGoal = userParams.MinDailyGoal;
            request.PageNumber = userParams.PageNumber;
            request.SortBy = request.SortBy;
            request.UserIds.AddRange(userParams.UserIds.Select(id => id.ToString()).ToList());
            try
            {
                _logger.LogInformation("Sending grpc request to retrieve users {@request}", request);
                var reply = client.GetUserList(request);
                var response = new PagedList<UserDto>(reply.Users.Select(pu => new UserDto()
                {

                    Id = Guid.Parse(pu.Id),
                    FirstName = pu.FirstName,
                    LastName = pu.LastName,
                    Email = pu.Email,
                    UserName = pu.UserName,
                    DailyGoal = pu.DailyGoal

                }).ToList(), reply.TotalCount, reply.PageNumber, reply.PageSize);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Sending grpc request to retrieve users failed, see exception for more info {@ex}", ex);
                throw;

            }
        }
    }
}
