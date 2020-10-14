using Robust.Shared.IoC;
using Content.Client.UI;

namespace Content.Client
{
    internal static class ClientContentIoC
    {
        public static void Register()
        {
            IoCManager.Register<IUIStyleManager, UIStyleManager>();
            // DEVNOTE: IoCManager registrations for the client go here and only here.
        }
    }
}