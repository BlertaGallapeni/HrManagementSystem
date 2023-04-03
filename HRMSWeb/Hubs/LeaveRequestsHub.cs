using Microsoft.AspNetCore.SignalR;

namespace HRMSWeb.Hubs
{
    public class LeaveRequestsHub : Hub
    {
        public async Task GetLeaveRequest()
        {
            await Clients.All.SendAsync("LeaveRequest");
        }
    }
}
