using Robust.Client.UserInterface;

namespace Content.Client.UI
{
    public interface IUIStyleManager
    {
        Stylesheet Stylesheet { get; }

        void Initialize();
    }
}