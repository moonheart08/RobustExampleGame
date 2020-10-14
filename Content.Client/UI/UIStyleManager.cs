using System.Linq;
using Robust.Client.Graphics;
using Robust.Client.Graphics.Drawing;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;
using static Robust.Client.UserInterface.StylesheetHelpers;
using Robust.Client.ResourceManagement;
using static Content.Client.StaticIoC;
using Robust.Client.Interfaces.UserInterface;
using Robust.Shared.IoC;
using Content.Client.UI;


namespace Content.Client.UI
{
    public class UIStyleManager: IUIStyleManager {
        [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

        public const string ClassHighDivider = "HighDivider";
        public const string StyleClassLabelHeading = "LabelHeading";
        public const string StyleClassLabelSubText = "LabelSubText";

        public const string ButtonOpenRight = "OpenRight";
        public const string ButtonOpenLeft = "OpenLeft";
        public const string ButtonOpenBoth = "OpenBoth";

        public const string ButtonCaution = "Caution";


        public static readonly Color ButtonColorDefault = Color.FromHex("#464966");
        public static readonly Color ButtonColorHovered = Color.FromHex("#575b7f");
        public static readonly Color ButtonColorPressed = Color.FromHex("#3e6c45");
        public static readonly Color ButtonColorDisabled = Color.FromHex("#30313c");

        public static readonly Color ButtonColorCautionDefault = Color.FromHex("#ab3232");
        public static readonly Color ButtonColorCautionHovered = Color.FromHex("#cf2f2f");
        public static readonly Color ButtonColorCautionPressed = Color.FromHex("#3e6c45");
        public static readonly Color ButtonColorCautionDisabled = Color.FromHex("#602a2a");

        public Stylesheet Stylesheet { get; private set; }

        public void Initialize()
        {
            var notoSans12 = new VectorFont(ResC.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Regular.ttf"), 12);
            var notoSansBold15 = new VectorFont(ResC.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Bold.ttf"), 15);

            var buttonTexture = ResC.GetResource<TextureResource>("/Textures/UI/button.png");
            var windowBackgroundTex = ResC.GetResource<TextureResource>("/Textures/UI/button.png");
            var windowBackground = new StyleBoxTexture
            {
                Texture = windowBackgroundTex,
            };
            var buttonBase = new StyleBoxTexture
            {
                Texture = buttonTexture,
            };

            var lineEditTex = ResC.GetResource<TextureResource>("/Textures/UI/edit.png");
            var lineEdit = new StyleBoxTexture
            {
                Texture = lineEditTex,
            };
            lineEdit.SetPatchMargin(StyleBox.Margin.All, 3);
            lineEdit.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);


            buttonBase.SetPatchMargin(StyleBox.Margin.All, 10);
            buttonBase.SetPadding(StyleBox.Margin.All, 1);
            buttonBase.SetContentMarginOverride(StyleBox.Margin.Vertical, 2);
            buttonBase.SetContentMarginOverride(StyleBox.Margin.Horizontal, 14);

            var openRightButtonBase = new StyleBoxTexture(buttonBase)
            {
                Texture = new AtlasTexture(buttonTexture, UIBox2.FromDimensions((0, 0), (14, 24))),
            };
            openRightButtonBase.SetPatchMargin(StyleBox.Margin.Right, 0);
            openRightButtonBase.SetContentMarginOverride(StyleBox.Margin.Right, 8);
            openRightButtonBase.SetPadding(StyleBox.Margin.Right, 2);

            var openLeftButtonBase = new StyleBoxTexture(buttonBase)
            {
                Texture = new AtlasTexture(buttonTexture, UIBox2.FromDimensions((10, 0), (14, 24))),
            };
            openLeftButtonBase.SetPatchMargin(StyleBox.Margin.Left, 0);
            openLeftButtonBase.SetContentMarginOverride(StyleBox.Margin.Left, 8);
            openLeftButtonBase.SetPadding(StyleBox.Margin.Left, 1);

            var openBothButtonBase = new StyleBoxTexture(buttonBase)
            {
                Texture = new AtlasTexture(buttonTexture, UIBox2.FromDimensions((10, 0), (3, 24))),
            };
            openBothButtonBase.SetPatchMargin(StyleBox.Margin.Horizontal, 0);
            openBothButtonBase.SetContentMarginOverride(StyleBox.Margin.Horizontal, 8);
            openBothButtonBase.SetPadding(StyleBox.Margin.Right, 2);
            openBothButtonBase.SetPadding(StyleBox.Margin.Left, 1);

            Stylesheet = new Stylesheet(new StyleRule[] {
                new StyleRule(
                    new SelectorElement(null, null, null, null),
                    new[]
                    {
                        new StyleProperty("font", notoSans12),
                    }),
                new StyleRule(new SelectorElement(typeof(LineEdit), null, null, null),
                    new[]
                    {
                        new StyleProperty(LineEdit.StylePropertyStyleBox, lineEdit),
                    }),
                Element<Label>().Class(StyleClassLabelHeading)
                    .Prop(Label.StylePropertyFont, notoSansBold15),

                Element<Label>().Class(StyleClassLabelSubText)
                    .Prop(Label.StylePropertyFont, notoSans12)
                    .Prop(Label.StylePropertyFontColor, Color.White),

                Element<PanelContainer>().Class(ClassHighDivider)
                    .Prop(PanelContainer.StylePropertyPanel, new StyleBoxFlat
                    {
                        BackgroundColor = Color.Gray, ContentMarginBottomOverride = 2, ContentMarginLeftOverride = 2
                    }),
                // Shapes for the buttons.
                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Prop(ContainerButton.StylePropertyStyleBox, buttonBase),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Class(ButtonOpenRight)
                    .Prop(ContainerButton.StylePropertyStyleBox, openRightButtonBase),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Class(ButtonOpenLeft)
                    .Prop(ContainerButton.StylePropertyStyleBox, openLeftButtonBase),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Class(ButtonOpenBoth)
                    .Prop(ContainerButton.StylePropertyStyleBox, openBothButtonBase),

                // Colors for the buttons.
                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Pseudo(ContainerButton.StylePseudoClassNormal)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorDefault),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Pseudo(ContainerButton.StylePseudoClassHover)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorHovered),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Pseudo(ContainerButton.StylePseudoClassPressed)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorPressed),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton)
                    .Pseudo(ContainerButton.StylePseudoClassDisabled)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorDisabled),

                // Colors for the caution buttons.
                Element<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(ButtonCaution)
                    .Pseudo(ContainerButton.StylePseudoClassNormal)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorCautionDefault),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(ButtonCaution)
                    .Pseudo(ContainerButton.StylePseudoClassHover)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorCautionHovered),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(ButtonCaution)
                    .Pseudo(ContainerButton.StylePseudoClassPressed)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorCautionPressed),

                Element<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(ButtonCaution)
                    .Pseudo(ContainerButton.StylePseudoClassDisabled)
                    .Prop(Control.StylePropertyModulateSelf, ButtonColorCautionDisabled),


                Element<Label>().Class(ContainerButton.StyleClassButton)
                    .Prop(Label.StylePropertyAlignMode, Label.AlignMode.Center),

                Child()
                    .Parent(Element<Button>().Class(ContainerButton.StylePseudoClassDisabled))
                    .Child(Element<Label>())
                    .Prop("font-color", Color.FromHex("#FFFFFF")),
            });

            _userInterfaceManager.Stylesheet = Stylesheet;
        }
    }
}