using OrderingDomain.Cart;
using OrderingDomain.ReplyTypes;

namespace OrderingInfrastructure.Features.Cart;

public interface ICartRepository
{
    Task<Reply<List<CartItem>>> PostGet( string userId, List<CartItem> itemsFromClient );
    Task<Reply<bool>> Clear( string userId );
}