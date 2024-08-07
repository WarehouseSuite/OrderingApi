using OrderingDomain.ReplyTypes;
using OrderingDomain.Users;

namespace OrderingInfrastructure.Features.Users.Repositories;

public interface ISessionRepository : IEfCoreRepository
{
    public Task<Reply<int>> CountUserSessions( string userId );
    public Task<Reply<List<UserSession>>> GetPaginatedUserSessions( string userId, int page, int pageSize );
    public Task<Reply<UserSession>> GetSession( string sessionId, string userId );
    public Task<IReply> AddSession( UserSession session );
    public Task<IReply> DeleteSession( string sessionId, string userId );
}