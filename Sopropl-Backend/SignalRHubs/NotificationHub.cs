using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sopropl_Backend.SignalRHubs
{
    [Authorize]
    public class NotificationHub : Hub
    {

    }
}