using System;
using System.Threading.Tasks;
using Robust.Shared.Interfaces.Configuration;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.IoC;
using Robust.Shared.Network;

#nullable enable

namespace Content.Server
{
    public interface IConnectionManager
    {
        void Initialize();
    }

    public sealed class ConnectionManager : IConnectionManager
    {
        [Dependency] private readonly IServerNetManager _netMgr = default!;
        [Dependency] private readonly IConfigurationManager _cfg = default!;

        public void Initialize()
        {
            _netMgr.Connecting += NetMgrOnConnecting;
            _netMgr.AssignUserIdCallback = AssignUserIdCallback;
        }

        // NOTE: Async becomes more important when you start interfacing with an external db.
        private async Task NetMgrOnConnecting(NetConnectingArgs e)
        {
            return;
        }

        private async Task<NetUserId?> AssignUserIdCallback(string name)
        {
            return null; 
        }
    }
}