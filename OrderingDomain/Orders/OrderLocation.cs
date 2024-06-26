using OrderingDomain.ValueTypes;

namespace OrderingDomain.Orders;

public sealed class OrderLocation
{
    public Guid Id { get; set; } = Guid.Empty;
    public WorldGridPos WorldGridPos { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
}