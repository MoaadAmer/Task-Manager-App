using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskManagerAPI.Controllers
{
    public class Base : ControllerBase
    {
        protected Guid? GetUserId()
        {
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid result))
            {
                return result;
            }
            return null;

        }
    }
}
