using Robust.Shared.IoC;

namespace Content.Server
{
    internal static class ServerContentIoC
    {
        public static void Register()
        {
            IoCManager.Register<IConnectionManager, ConnectionManager>();
            // DEVNOTE: IoCManager registrations for the server go here and only here.
        }
    }
}