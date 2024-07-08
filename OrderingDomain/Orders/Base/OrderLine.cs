namespace OrderingDomain.Orders.Base;

public sealed class OrderLine
{
    public OrderLine() { }
    public OrderLine( Guid orderId, Guid warehouseId )
    {
        OrderId = orderId;
        WarehouseId = warehouseId;
    }

    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid OrderGroupId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid UnitId { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public int Quantity { get; set; }
}