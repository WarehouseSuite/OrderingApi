using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderingDomain.Orders;
using OrderingDomain.ReplyTypes;

namespace OrderingInfrastructure.Features.Ordering.Repositories;

internal sealed class OrderingRepository( OrderingDbContext database, ILogger<OrderingRepository> logger ) 
    : DatabaseService<OrderingRepository>( database, logger ), IOrderingRepository
{
    readonly OrderingDbContext _database = database;
    
    public async Task<Reply<bool>> InsertOrder( Order order )
    {
        try {
            await _database.ActiveOrders.AddAsync( order );
            return await SaveAsync();
        }
        catch ( Exception e ) {
            return ProcessDbException<bool>( e );
        }
    }
    public async Task<Reply<bool>> InsertOrderLines( IEnumerable<OrderLine> orderLines )
    {
        try {
            await _database.AddRangeAsync( orderLines );
            return await SaveAsync();
        }
        catch ( Exception e ) {
            return ProcessDbException<bool>( e );
        }
    }
    public async Task<Reply<bool>> InsertOrderItems( IEnumerable<OrderItem> orderItems )
    {
        try {
            await _database.ActiveOrderItems.AddRangeAsync( orderItems );
            return await SaveAsync();
        }
        catch ( Exception e ) {
            return ProcessDbException<bool>( e );
        }
    }
    public async Task<Reply<bool>> DeleteOrderData( Guid orderId )
    {
        try {
            List<OrderItem> orderItems = await _database.ActiveOrderItems.Where( i => i.OrderId == orderId ).ToListAsync();
            List<OrderLine> orderLines = await _database.ActiveOrderLines.Where( l => l.OrderId == orderId ).ToListAsync();
            Order? order = await _database.ActiveOrders.FirstOrDefaultAsync( o => o.Id == orderId );

            _database.ActiveOrderItems.RemoveRange( orderItems );
            _database.ActiveOrderLines.RemoveRange( orderLines );
            if (order is not null)
                _database.ActiveOrders.Remove( order );

            return await SaveAsync();
        }
        catch ( Exception e ) {
            return ProcessDbException<bool>( e );
        }
    }
    public async Task<Reply<Order>> GetOrderById( Guid orderId )
    {
        try {
            Order? order = await _database.ActiveOrders.FirstOrDefaultAsync( o => o.Id == orderId );
            return order is not null
                ? Reply<Order>.Success( order )
                : Reply<Order>.Failure( $"Order {orderId} not found in db." );
        }
        catch ( Exception e ) {
            return ProcessDbException<Order>( e );
        }
    }
    public async Task<Replies<OrderLine>> GetOrderLinesByOrderId( Guid orderId )
    {
        try {
            return Replies<OrderLine>.Success(
                await _database.ActiveOrderLines.Where( l => l.OrderId == orderId ).ToListAsync() );
        }
        catch ( Exception e ) {
            return HandleDbExceptionReplies<OrderLine>( e );
        }
    }
    public async Task<Replies<OrderItem>> GetItemsForLineById( Guid orderId, Guid orderLineId )
    {
        try {
            return Replies<OrderItem>.Success(
                await _database.ActiveOrderItems.Where( i => i.OrderId == orderId && i.OrderLineId == orderLineId ).ToListAsync() );
        }
        catch ( Exception e ) {
            return HandleDbExceptionReplies<OrderItem>( e );
        }
    }
    public async Task<Reply<Dictionary<OrderLine, IEnumerable<OrderItem>>>> GetItemsForOrderLines( IEnumerable<OrderLine> lines )
    {
        try {
            Dictionary<OrderLine, IEnumerable<OrderItem>> items = [];

            foreach ( OrderLine l in lines )
                if ((await GetItemsForLineById( l.OrderId, l.Id )).OutFailure( out Replies<OrderItem> itemsResult ))
                    return Reply<Dictionary<OrderLine, IEnumerable<OrderItem>>>.Failure( itemsResult );
                else
                    items.TryAdd( l, itemsResult.Enumerable );

            return Reply<Dictionary<OrderLine, IEnumerable<OrderItem>>>.Success( items );
        }
        catch ( Exception e ) {
            return ProcessDbException<Dictionary<OrderLine, IEnumerable<OrderItem>>>( e );
        }
    }
}