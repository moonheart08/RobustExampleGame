using Robust.Shared.ContentPack;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Client.Interfaces.State;
using Robust.Client.Interfaces.Input;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Content.Client.UI;
using Content.Client.State;

namespace Content.Client
{
    public class EntryPoint: GameClient
    {
        [Dependency] private readonly IStateManager _stateManager = default!;

        public override void Init()
        {
            var factory = IoCManager.Resolve<IComponentFactory>();
            var prototypes = IoCManager.Resolve<IPrototypeManager>();
            
            factory.DoAutoRegistrations();

            foreach (var ignoreName in IgnoredComponents.List)
            {
                factory.RegisterIgnore(ignoreName);
            }

            foreach (var ignoreName in IgnoredPrototypes.List)
            {
                prototypes.RegisterIgnore(ignoreName);
            }

            ClientContentIoC.Register();

            IoCManager.BuildGraph();

            IoCManager.Resolve<IUIStyleManager>().Initialize();

            // DEVNOTE: This is generally where you'll be setting up the IoCManager further.

            IoCManager.InjectDependencies(this);
        }
        public override void PostInit()
        {
            base.PostInit();

            // DEVNOTE: Further setup, this is the spot you should start trying to connect to the server from.
            _stateManager.RequestStateChange<ConnectionScreen>();
        }

        public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
        {
            base.Update(level, frameEventArgs);
            // DEVNOTE: Game update loop goes here. Usually you'll want some independent GameTicker.
        }
    }

    
}