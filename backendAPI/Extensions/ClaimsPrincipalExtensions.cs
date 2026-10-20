using System.Security.Claims;

namespace backendAPI.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetMemberId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get the memberId from token");
    }   // ova go vrakja id-to na memberot preku tokenot pri auth


}