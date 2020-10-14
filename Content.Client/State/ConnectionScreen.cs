
using System;
using System.Text.RegularExpressions;
using Content.Client.UI;
using Robust.Client;
using Robust.Client.Interfaces;
using Robust.Client.Interfaces.ResourceManagement;
using Robust.Client.Interfaces.UserInterface;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Interfaces.Configuration;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Log;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace Content.Client.State
{
    public class ConnectionScreen : Robust.Client.State.State
    {
        [Dependency] private readonly IBaseClient _client = default!;
        [Dependency] private readonly IClientNetManager _netManager = default!;
        [Dependency] private readonly IConfigurationManager _configurationManager = default!;
        [Dependency] private readonly IGameController _controllerProxy = default!;
        [Dependency] private readonly IResourceCache _resourceCache = default!;
        [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

        private ConnectionMenuControl _connectionMenu;

        private static readonly Regex IPv6Regex = new Regex(@"\[(.*:.*:.*)](?::(\d+))?");

        /// <inheritdoc />
        public override void Startup()
        {
            _connectionMenu = new ConnectionMenuControl(_resourceCache, _configurationManager);
            _connectionMenu.DirectConnectButton.OnPressed += DirectConnectButtonPressed;
            _client.RunLevelChanged += RunLevelChanged;
            _userInterfaceManager.StateRoot.AddChild(_connectionMenu);
        }

        public override void Shutdown()
        {
            _connectionMenu.Dispose();
        }

        private void DirectConnectButtonPressed(BaseButton.ButtonEventArgs args)
        {
            var input = _connectionMenu.AddressBox;
            TryConnect(input.Text);
        }

        private void TryConnect(string address)
        {
            _setConnectingState(true);
            _netManager.ConnectFailed += _onConnectFailed;
            try
            {
                ParseAddress(address, out var ip, out var port);
                _client.ConnectToServer(ip, port);
            }
            catch (ArgumentException e)
            {
                _userInterfaceManager.Popup($"Unable to connect: {e.Message}", "Connection error.");
                Logger.Warning(e.ToString());
                _netManager.ConnectFailed -= _onConnectFailed;
                _setConnectingState(false);
            }
        }

        private void _onConnectFailed(object _, NetConnectFailArgs args)
        {
            _userInterfaceManager.Popup($"Failed to connect:\n{args.Reason}");
            _netManager.ConnectFailed -= _onConnectFailed;
            _setConnectingState(false);
        }

        private void _setConnectingState(bool state)
        {
            _connectionMenu.DirectConnectButton.Disabled = state;
        }

        private void RunLevelChanged(object obj, RunLevelChangedEventArgs args)
        {
            if (args.NewLevel == ClientRunLevel.Initialize)
            {
                _setConnectingState(false);
                _netManager.ConnectFailed -= _onConnectFailed;
            }
        }

        private void ParseAddress(string address, out string ip, out ushort port)
        {
            var match6 = IPv6Regex.Match(address);
            if (match6 != Match.Empty)
            {
                ip = match6.Groups[1].Value;
                if (!match6.Groups[2].Success)
                {
                    port = _client.DefaultPort;
                }
                else if (!ushort.TryParse(match6.Groups[2].Value, out port))
                {
                    throw new ArgumentException("Not a valid port.");
                }

                return;
            }

            // See if the IP includes a port.
            var split = address.Split(':');
            ip = address;
            port = _client.DefaultPort;
            if (split.Length > 2)
            {
                throw new ArgumentException("Not a valid Address.");
            }

            // IP:port format.
            if (split.Length == 2)
            {
                ip = split[0];
                if (!ushort.TryParse(split[1], out port))
                {
                    throw new ArgumentException("Not a valid port.");
                }
            }
        }

        private sealed class ConnectionMenuControl : Control
        {
            private readonly IResourceCache _resourceCache;
            private readonly IConfigurationManager _configurationManager;

            public LineEdit UsernameBox { get; private set; }
            public LineEdit AddressBox { get; private set; }
            public Button JoinServerButton { get; private set; }
            public Button QuitButton { get; private set; }
            public Button DirectConnectButton { get; private set; }
            public ConnectionMenuControl(IResourceCache resc, IConfigurationManager cfgman)
            {
                _resourceCache = resc;
                _configurationManager = cfgman;
                PerformLayout();
            }

            private void PerformLayout()
            {
                LayoutContainer.SetAnchorPreset(this, LayoutContainer.LayoutPreset.Wide);

                var layout = new LayoutContainer();
                AddChild(layout);

                var vBox = new VBoxContainer
                {
                    //StyleIdentifier = "connectionMenuVBox"
                };

                layout.AddChild(vBox);
                LayoutContainer.SetAnchorPreset(vBox, LayoutContainer.LayoutPreset.Center);
                //LayoutContainer.SetMarginRight(vBox, -25);
                //LayoutContainer.SetMarginTop(vBox, 30);
                LayoutContainer.SetGrowHorizontal(vBox, LayoutContainer.GrowDirection.Both);
                LayoutContainer.SetGrowVertical(vBox, LayoutContainer.GrowDirection.Both);

                vBox.AddChild(new Label {Text = "Robust Example Project"});
                var testTexture = _resourceCache.GetResource<TextureResource>("/Textures/robust.png");
                var test = new TextureRect
                {
                    Texture = testTexture,
                    Stretch = TextureRect.StretchMode.KeepCentered,
                    TextureScale = new Robust.Shared.Maths.Vector2(0.3f, 0.3f)
                };
                vBox.AddChild(test);

                var usernameHBox = new HBoxContainer {SeparationOverride = 4};
                vBox.AddChild(usernameHBox);
                usernameHBox.AddChild(new Label {Text = "Username"});

                UsernameBox = new LineEdit
                {
                    Text = "Baa", PlaceHolder = "Username",
                    SizeFlagsHorizontal = SizeFlags.FillExpand
                };

                usernameHBox.AddChild(UsernameBox);

                var addressHBox = new HBoxContainer {SeparationOverride = 4};
                vBox.AddChild(addressHBox);
                addressHBox.AddChild(new Label {Text = "Address"});

                AddressBox = new LineEdit
                {
                    Text = "localhost", PlaceHolder = "Address",
                    SizeFlagsHorizontal = SizeFlags.FillExpand
                };
                
                addressHBox.AddChild(AddressBox);

                DirectConnectButton = new Button
                {
                    Text = "Direct Connect",
                    TextAlign = Label.AlignMode.Center,
                };

                vBox.AddChild(DirectConnectButton);


            }
        }
    }

    
}