using Microsoft.AspNetCore.Mvc;
using OrderingApplication.Extentions;
using OrderingApplication.Features.Users.Addresses.Types;
using OrderingApplication.Utilities;

namespace OrderingApplication.Features.Users.Addresses;

internal static class UserAddressEndpoints
{
    const string CookiesOrJwt = "CookiesOrJwt";
    
    internal static void MapAccountAddressEndpoints( this IEndpointRouteBuilder app )
    {
        app.MapGet( "api/account/addresses/view",
            static async ( [FromQuery] int page, [FromQuery] int pageSize, HttpContext http, UserAddressManager manager ) =>
            await GetAddresses( page, pageSize, http, manager ) ).RequireAuthorization( CookiesOrJwt );

        app.MapPut( "api/account/addresses/add",
            static async ( [FromBody] AddressDto address, HttpContext http, UserAddressManager manager ) =>
            await AddAddress( address, http, manager ) ).RequireAuthorization( CookiesOrJwt );

        app.MapPut( "api/account/addresses/update",
            static async ( [FromBody] AddressDto address, HttpContext http, UserAddressManager manager ) =>
            await UpdateAddress( address, http, manager ) ).RequireAuthorization( CookiesOrJwt );

        app.MapDelete( "api/account/addresses/delete",
            static async ( [FromQuery] Guid addressId, HttpContext http, UserAddressManager manager ) =>
            await DeleteAddress( addressId, http, manager ) ).RequireAuthorization( CookiesOrJwt );
    }

    static async Task<IResult> GetAddresses( int page, int pageSize, HttpContext http, UserAddressManager manager )
    {
        EndpointLogger.EndpointHit( nameof( GetAddresses ) );
        var getReply = await manager.ViewAddresses( http.UserId(), page, pageSize );
        return getReply.GetIResult();
    }
    static async Task<IResult> AddAddress( AddressDto address, HttpContext http, UserAddressManager manager )
    {
        var getReply = await manager.AddAddress( http.UserId(), address );
        
        return getReply.GetIResult();
    }
    static async Task<IResult> UpdateAddress( AddressDto address, HttpContext http, UserAddressManager manager )
    {
        var getReply = await manager.UpdateAddress( http.UserId(), address );
        return getReply.GetIResult();
    }
    static async Task<IResult> DeleteAddress( Guid addressId, HttpContext http, UserAddressManager manager )
    {
        var getReply = await manager.DeleteAddress( http.UserId(), addressId );
        return getReply.GetIResult();
    }
}