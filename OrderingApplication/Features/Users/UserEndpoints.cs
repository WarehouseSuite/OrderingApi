using OrderingApplication.Features.Users.Addresses;
using OrderingApplication.Features.Users.Delete;
using OrderingApplication.Features.Users.Profile;
using OrderingApplication.Features.Users.Authentication;
using OrderingApplication.Features.Users.Orders;
using OrderingApplication.Features.Users.Registration;
using OrderingApplication.Features.Users.Security;
using OrderingApplication.Features.Users.Sessions;

namespace OrderingApplication.Features.Users;

internal static class UserEndpoints
{
    internal static void MapUserEndpoints( this IEndpointRouteBuilder app )
    {
        app.MapAuthenticationEndpoints();
        app.MapRegistrationEndpoints();
        app.MapAccountProfileEndpoints();
        app.MapAccountSecurityEndpoints();
        app.MapAccountAddressEndpoints();
        app.MapAccountDeleteEndpoints();
        app.MapAccountOrdersEndpoints();
        app.MapAccountSessionsEndpoints();
    }
}