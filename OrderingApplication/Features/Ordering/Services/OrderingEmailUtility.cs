using System.Text;
using OrderingDomain.Orders.Base;
using OrderingDomain.Orders.Meta;

namespace OrderingApplication.Features.Ordering.Services;

internal static class OrderingEmailUtility
{
    internal static string GenerateOrderPlacedEmail(Order order)
    {
        var emailHtml = new StringBuilder();

        emailHtml.AppendLine("""
        <html>
        <head>
        <style>
        body { font-family: Arial, sans-serif; }
        .order-details { width: 100%; border-collapse: collapse; margin: 20px 0; }
        .order-details th, .order-details td { border: 1px solid #dddddd; text-align: left; padding: 8px; }
        .order-details th { background-color: #f2f2f2; }
        .order-header { font-size: 18px; font-weight: bold; margin-bottom: 10px; }
        .order-section { margin-bottom: 20px; }
        </style>
        </head>
        <body>
        <div class='order-header'>Order Confirmation</div>
        <div class='order-section'>Order ID: {order.Id}</div>
        <div class='order-section'>Date Placed: {order.DatePlaced:MMMM dd, yyyy}</div>
        <div class='order-section'>Total Quantity: {order.TotalQuantity}</div>
        <div class='order-section'>Order Status: {order.State}</div>
        <div class='order-header'>Contact Information</div>
        <div class='order-section'>Name: {order.Contact.Name}</div>
        <div class='order-section'>Email: {order.Contact.Email}</div>
        <div class='order-section'>Phone: {order.Contact.Phone}</div>
        <div class='order-header'>Shipping Address</div>
        <div class='order-section'>{order.ShippingAddress.Street}</div>
        <div class='order-section'>{order.ShippingAddress.City}, {order.ShippingAddress.State} {order.ShippingAddress.ZipCode}</div>
        <div class='order-section'>{order.ShippingAddress.Country}</div>
        <div class='order-header'>Billing Address</div>
        <div class='order-section'>{order.BillingAddress.Street}</div>
        <div class='order-section'>{order.BillingAddress.City}, {order.BillingAddress.State} {order.BillingAddress.ZipCode}</div>
        <div class='order-section'>{order.BillingAddress.Country}</div>
        <div class='order-header'>Order Details</div>
        <table class='order-details'>
        <tr>
        <th>Group ID</th>
        <th>Warehouse ID</th>
        <th>State</th>
        <th>Last Updated</th>
        </tr>
        """);

        foreach (var group in order.OrderGroups)
        {
            emailHtml.AppendLine($"""
            <tr>
            <td>{group.Id}</td>
            <td>{group.WarehouseId}</td>
            <td>{group.State}</td>
            <td>{group.LastUpdated:MMMM dd, yyyy}</td>
            </tr>
            <tr>
            <td colspan='4'>
            <table class='order-details' style='margin-left: 20px;'>
            <tr>
            <th>Order Line ID</th>
            <th>Unit ID</th>
            <th>Unit Price</th>
            <th>Quantity</th>
            <th>Discount</th>
            <th>Tax</th>
            </tr>
            """);

            foreach (var line in group.OrderLines)
            {
                emailHtml.AppendLine($"""
                <tr>
                <td>{line.Id}</td>
                <td>{line.UnitId}</td>
                <td>{line.UnitPrice:C}</td>
                <td>{line.Quantity}</td>
                <td>{line.Discount:C}</td>
                <td>{line.Tax:C}</td>
                </tr>
                """);
            }

            emailHtml.AppendLine("""
            </table>
            </td>
            </tr>
            """);
        }

        emailHtml.AppendLine("""
        </table>
        </body>
        </html>
        """);

        return emailHtml.ToString();
    }

    internal static string GenerateOrderGroupUpdateEmail( OrderGroup group, OrderState newState )
    {
        var emailHtml = new StringBuilder();

        emailHtml.AppendLine( """
                              <html>
                              <head>
                              <style>
                              body { font-family: Arial, sans-serif; }
                              .order-details { width: 100%; border-collapse: collapse; margin: 20px 0; }
                              .order-details th, .order-details td { border: 1px solid #dddddd; text-align: left; padding: 8px; }
                              .order-details th { background-color: #f2f2f2; }
                              .order-header { font-size: 18px; font-weight: bold; margin-bottom: 10px; }
                              .order-section { margin-bottom: 20px; }
                              </style>
                              </head>
                              <body>
                              <div class='order-header'>Order Update</div>
                              <div class='order-section'>Order Group ID: {group.Id}</div>
                              <div class='order-section'>Warehouse ID: {group.WarehouseId}</div>
                              <div class='order-section'>New State: {newState}</div>
                              <div class='order-section'>Last Updated: {DateTime.Now:MMMM dd, yyyy}</div>
                              <div class='order-header'>Order Lines</div>
                              <table class='order-details'>
                              <tr>
                              <th>Order Line ID</th>
                              <th>Unit ID</th>
                              <th>Unit Price</th>
                              <th>Quantity</th>
                              <th>Discount</th>
                              <th>Tax</th>
                              </tr>
                              """ );

        foreach ( var line in group.OrderLines )
        {
            emailHtml.AppendLine( $"""
                                   <tr>
                                   <td>{line.Id}</td>
                                   <td>{line.UnitId}</td>
                                   <td>{line.UnitPrice:C}</td>
                                   <td>{line.Quantity}</td>
                                   <td>{line.Discount:C}</td>
                                   <td>{line.Tax:C}</td>
                                   </tr>
                                   """ );
        }

        emailHtml.AppendLine( """
                              </table>
                              </body>
                              </html>
                              """ );

        return emailHtml.ToString();
    }

    internal static string GenerateOrderUpdateEmail( Order order, OrderState newState )
    {
        return @"
            <html>
            <head>
            <style>
            body { font-family: Arial, sans-serif; }
            .order-header { font-size: 18px; font-weight: bold; margin-bottom: 10px; }
            .order-section { margin-bottom: 20px; }
            </style>
            </head>
            <body>
            <div class='order-header'>Order Status Update</div>
            <div class='order-section'>Order ID: " + order.Id + @"</div>
            <div class='order-section'>New Order Status: " + newState + @"</div>
            <div class='order-section'>Date: " + DateTime.Now.ToString( "MMMM dd, yyyy" ) + @"</div>
            <p>Dear " + order.Contact.Name + @",</p>
            <p>We wanted to inform you that the status of your order has changed to <strong>" + newState + @"</strong>. Please find the updated details below:</p>
            <p>If you have any questions or need further assistance, feel free to contact us at support@example.com.</p>
            <p>Thank you for shopping with us!</p>
            <p>Best regards,<br />The YourShop Team</p>
            </body>
            </html>";
    }
    internal static string GenerateOrderCancelledEmail( Order order, string cancellationReason )
    {
        return @"
            <html>
            <head>
            <style>
            body { font-family: Arial, sans-serif; }
            .order-header { font-size: 18px; font-weight: bold; margin-bottom: 10px; }
            .order-section { margin-bottom: 20px; }
            </style>
            </head>
            <body>
            <div class='order-header'>Order Cancellation</div>
            <div class='order-section'>Order ID: " + order.Id + @"</div>
            <div class='order-section'>Cancellation Reason:</div>
            <p>" + cancellationReason + @"</p>
            <div class='order-section'>Date: " + DateTime.Now.ToString( "MMMM dd, yyyy" ) + @"</div>
            <p>Dear " + order.Contact.Name + @",</p>
            <p>We regret to inform you that your order has been cancelled. The details are as follows:</p>
            <p>If you have any questions or need further assistance, feel free to contact us at support@example.com.</p>
            <p>Thank you for your understanding.</p>
            <p>Best regards,<br />The YourShop Team</p>
            </body>
            </html>";
    }
}