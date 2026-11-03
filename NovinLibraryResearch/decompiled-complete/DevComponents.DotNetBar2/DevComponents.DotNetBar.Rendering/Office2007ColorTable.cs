using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Schedule;

namespace DevComponents.DotNetBar.Rendering;

public class Office2007ColorTable : IElementStyleClassProvider, IDisposable
{
	private Office2007ButtonItemColorTableCollection ቈ = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection _1249 = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection ቊ = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection _ቋ = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection _ቌ = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection _ቍ = new Office2007ButtonItemColorTableCollection();

	private Office2007RibbonTabItemColorTableCollection _124E = new Office2007RibbonTabItemColorTableCollection();

	private Office2007RibbonTabGroupColorTableCollection _124F = new Office2007RibbonTabGroupColorTableCollection();

	private eOffice2007ColorScheme լ;

	private Hashtable ቐ = new Hashtable();

	private Dictionary<string, object> ቑ = new Dictionary<string, object>();

	private ColorFactory ቒ = new ColorFactory();

	public Office2007RibbonBarColorTable RibbonBar = new Office2007RibbonBarColorTable();

	private static Office2007ButtonItemColorTable ቓ;

	public Office2007ItemGroupColorTable ItemGroup = new Office2007ItemGroupColorTable();

	public Office2007BarColorTable Bar = new Office2007BarColorTable();

	public Office2007RibbonColorTable RibbonControl = new Office2007RibbonColorTable();

	public Office2007ColorItemColorTable ColorItem = new Office2007ColorItemColorTable();

	public Office2007MenuColorTable Menu = new Office2007MenuColorTable();

	public Office2007ComboBoxColorTable ComboBox = new Office2007ComboBoxColorTable();

	public Office2007DialogLauncherColorTable DialogLauncher = new Office2007DialogLauncherColorTable();

	public ColorScheme LegacyColors = new ColorScheme(eDotNetBarStyle.Office2007);

	public Office2007SystemButtonColorTable SystemButton = new Office2007SystemButtonColorTable();

	public Office2007SystemButtonColorTable SystemButtonClose;

	public Office2007FormColorTable Form = new Office2007FormColorTable();

	public Office2007QuickAccessToolbarColorTable QuickAccessToolbar = new Office2007QuickAccessToolbarColorTable();

	public Office2007TabColorTable TabControl = new Office2007TabColorTable();

	public Office2007TabColorTable DockTabStrip;

	public Office2007KeyTipsColorTable KeyTips = new Office2007KeyTipsColorTable();

	public Office2007CheckBoxColorTable CheckBoxItem = new Office2007CheckBoxColorTable();

	public Office2007ScrollBarColorTable ScrollBar = new Office2007ScrollBarColorTable();

	public Office2007ScrollBarColorTable AppScrollBar = new Office2007ScrollBarColorTable();

	public Office2007ProgressBarColorTable ProgressBarItem = new Office2007ProgressBarColorTable();

	public Office2007ProgressBarColorTable ProgressBarItemPaused = new Office2007ProgressBarColorTable();

	public Office2007ProgressBarColorTable ProgressBarItemError = new Office2007ProgressBarColorTable();

	public Office2007GalleryColorTable Gallery = new Office2007GalleryColorTable();

	public Office2007NavigationPaneColorTable NavigationPane = new Office2007NavigationPaneColorTable();

	public Office2007SliderColorTable Slider = new Office2007SliderColorTable();

	public Office2010RangeSliderColorTable RangeSlider = new Office2010RangeSliderColorTable();

	public Office2007SuperTooltipColorTable SuperTooltip = new Office2007SuperTooltipColorTable();

	public Office2007ListViewColorTable ListViewEx = new Office2007ListViewColorTable();

	public Office2007DataGridViewColorTable DataGridView = new Office2007DataGridViewColorTable();

	public Office2007SideBarColorTable SideBar = new Office2007SideBarColorTable();

	public TreeColorTable AdvTree;

	public CrumbBarItemViewColorTable CrumbBarItemView;

	public Office2007WarningBoxColorTable WarningBox = new Office2007WarningBoxColorTable();

	public Office2007CalendarViewColorTable CalendarView = new Office2007CalendarViewColorTable();

	public SuperTabColorTable SuperTab = new SuperTabColorTable();

	public SuperTabItemColorTable SuperTabItem = new SuperTabItemColorTable();

	public SuperTabPanelColorTable SuperTabPanel = new SuperTabPanelColorTable();

	public SuperTabColorTable Backstage = new SuperTabColorTable();

	public SuperTabItemColorTable BackstageItem = new SuperTabItemColorTable();

	public SuperTabPanelColorTable BackstagePanel = new SuperTabPanelColorTable();

	public SwitchButtonColors SwitchButton;

	public Dictionary<string, LabelColors> LabelItemColors = new Dictionary<string, LabelColors>();

	public StepIndicatorColorTable StepIndicator = new StepIndicatorColorTable();

	public OfficeStepItemColorTable StepItem = new OfficeStepItemColorTable();

	public OfficeListBoxItemColorTable ListBoxItem = new OfficeListBoxItemColorTable();

	public RadialMenuColorTable RadialMenu = new RadialMenuColorTable();

	public TokenEditorColorTable TokenEditor = new TokenEditorColorTable();

	public FlyoutColorTable Flyout = new FlyoutColorTable();

	public SideNavColorTable SideNav = new SideNavColorTable();

	public TabFormColorTable TabForm = new TabFormColorTable();

	public Dictionary<string, TabFormItemColorTable> TabFormItemColorTables = new Dictionary<string, TabFormItemColorTable>();

	private static bool _ቔ;

	public Office2007ButtonItemColorTableCollection ButtonItemColors => ቈ;

	public Office2007ButtonItemColorTableCollection RibbonButtonItemColors => _1249;

	public Office2007ButtonItemColorTableCollection MenuButtonItemColors => ቊ;

	public Office2007ButtonItemColorTableCollection ApplicationButtonColors => _ቋ;

	public Office2007ButtonItemColorTableCollection BackstageButtonItemColors => _ቌ;

	public Office2007ButtonItemColorTableCollection NavigationPaneButtonItemColors => _ቍ;

	public Office2007RibbonTabItemColorTableCollection RibbonTabItemColors => _124E;

	public Office2007RibbonTabGroupColorTableCollection RibbonTabGroupColors => _124F;

	public eOffice2007ColorScheme InitialColorScheme => լ;

	public Hashtable StyleClasses => ቐ;

	public static bool CloneImagesOnAccess
	{
		get
		{
			return _ቔ;
		}
		set
		{
			_ቔ = value;
		}
	}

	public Dictionary<string, object> ContextualTables => ቑ;

	public Office2007ColorTable()
	{
		ቕ(ቒ);
	}

	public Office2007ColorTable(ColorFactory colorFactory)
	{
		ቒ = colorFactory;
		ቕ(ቒ);
	}

	private void ቕ(ColorFactory ቖ)
	{
		RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonNormal.png");
		RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonHot.png");
		RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonPressed.png");
		RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9286371), ቖ.GetColor(8954306));
		RibbonControl.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15200248), ቖ.GetColor(12646911));
		RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(12573695), Color.Empty);
		RibbonControl.TabDividerBorder = ቖ.GetColor(11455216);
		RibbonControl.TabDividerBorderLight = ቖ.GetColor(13951989);
		RibbonControl.CornerSize = 3;
		RibbonControl.PanelTopBackgroundHeight = 15f;
		RibbonControl.PanelTopBackground = new LinearGradientColorTable(ቖ.GetColor(14411508), ቖ.GetColor(13622767));
		RibbonControl.PanelBottomBackground = new LinearGradientColorTable(ቖ.GetColor(13228525), ቖ.GetColor(15201023));
		ItemGroup.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10073824), ቖ.GetColor(7574717));
		ItemGroup.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14017521), ቖ.GetColor(14937595));
		ItemGroup.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13163502), ቖ.GetColor(13229558));
		ItemGroup.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12374249), ቖ.GetColor(13689335));
		ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, ቖ.GetColor(12110044));
		ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, ቖ.GetColor(16777215));
		RibbonBar.Default = ዂ.ዎ(ቖ);
		RibbonBar.MouseOver = ዂ.ዏ(ቖ);
		RibbonBar.Expanded = ዂ.ዐ(ቖ);
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዂ._12C7(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቈ.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ወ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቈ.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዉ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቈ.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዊ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቈ.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዋ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቈ.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዌ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቈ.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ው(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቈ.Add(office2007ButtonItemColorTable);
		ቈ.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(ቖ));
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዂ.ዑ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		_124E.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዂ.ዒ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		_124E.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዂ.ዓ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		_124E.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዂ.ዔ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		_124E.Add(office2007RibbonTabItemColorTable);
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = ዂ.ዕ(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		_124F.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዂ.ዖ(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		_124F.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዂ._12D7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		_124F.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዂ.ዘ(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		_124F.Add(office2007RibbonTabGroupColorTable);
		Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		Bar.ToolbarBottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		Bar.ToolbarBottomBorder = ቖ.GetColor(12244215);
		Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(16448250), Color.Empty);
		Bar.PopupToolbarBorder = ቖ.GetColor(8816262);
		Bar.StatusBarTopBorder = ቖ.GetColor(5668272);
		Bar.StatusBarTopBorderLight = ቖ.GetColor(Color.FromArgb(148, Color.White));
		Bar.StatusBarAltBackground.Clear();
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(12967160), 0f));
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(11127543), 0.4f));
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(9484010), 0.4f));
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(7640514), 1f));
		Menu.Background = new LinearGradientColorTable(ቖ.GetColor(16448250), Color.Empty);
		Menu.Border = new LinearGradientColorTable(ቖ.GetColor(8816262), Color.Empty);
		Menu.Side = new LinearGradientColorTable(ቖ.GetColor(15331054), Color.Empty);
		Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(12961221), Color.Empty);
		Menu.SideBorderLight = new LinearGradientColorTable(ቖ.GetColor(16119285), Color.Empty);
		Menu.SideUnused = new LinearGradientColorTable(ቖ.GetColor(15066597), Color.Empty);
		Menu.FileBackgroundBlend.Clear();
		Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(215, 229, 247), 0f),
			new BackgroundColorBlend(Color.FromArgb(218, 231, 247), 4f),
			new BackgroundColorBlend(Color.FromArgb(207, 223, 243), 4f),
			new BackgroundColorBlend(Color.FromArgb(189, 211, 239), 1f)
		});
		Menu.FileContainerBorder = Color.White;
		Menu.FileContainerBorderLight = Color.FromArgb(155, 175, 202);
		Menu.FileColumnOneBackground = ቖ.GetColor(16448250);
		Menu.FileColumnOneBorder = ቖ.GetColor(12961221);
		Menu.FileColumnTwoBackground = ቖ.GetColor(15330030);
		Menu.FileBottomContainerBackgroundBlend.Clear();
		Menu.FileBottomContainerBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(189, 211, 239), 0f),
			new BackgroundColorBlend(Color.FromArgb(190, 212, 240), 0.4f),
			new BackgroundColorBlend(Color.FromArgb(176, 201, 234), 0.4f),
			new BackgroundColorBlend(Color.FromArgb(207, 224, 245), 1f)
		});
		ComboBox.Default.Background = ቖ.GetColor(15397627);
		ComboBox.Default.Border = ቖ.GetColor(11256286);
		ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ComboBox.Default.ExpandText = ቖ.GetColor(1393291);
		ComboBox.DefaultStandalone.Background = ቖ.GetColor(16777215);
		int color = 11782113;
		ComboBox.DefaultStandalone.Border = ቖ.GetColor(color);
		ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(15003902), ቖ.GetColor(13623283), 90);
		ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable();
		ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(11519199), Color.Empty, 90);
		ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(1393291);
		ComboBox.MouseOver.Background = ቖ.GetColor(16777215);
		ComboBox.MouseOver.Border = ቖ.GetColor(color);
		ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16776418), ቖ.GetColor(16770981), 90);
		ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(16776435), 90);
		ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(14405273), Color.Empty, 90);
		ComboBox.MouseOver.ExpandText = ቖ.GetColor(1393291);
		ComboBox.DroppedDown.Background = ቖ.GetColor(16777215);
		ComboBox.DroppedDown.Border = ቖ.GetColor(color);
		ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(15392959), ቖ.GetColor(16766038), 90);
		ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(15854549), ቖ.GetColor(16770708), 90);
		ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(10129251), Color.Empty, 90);
		ComboBox.DroppedDown.ExpandText = ቖ.GetColor(1393291);
		DialogLauncher.Default.DialogLauncher = ቖ.GetColor(6721199);
		DialogLauncher.Default.DialogLauncherShade = ቖ.GetColor(16777215);
		DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(6721199);
		DialogLauncher.MouseOver.DialogLauncherShade = Color.FromArgb(192, ቖ.GetColor(16777215));
		DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776415), ቖ.GetColor(16773031));
		DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767349), ቖ.GetColor(16769944));
		DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(16776178));
		DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14405273), Color.Empty);
		DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(6721199);
		DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(192, ቖ.GetColor(16777215));
		DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15261373), ቖ.GetColor(15386253));
		DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16754488), ቖ.GetColor(16763982));
		DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15788756), ቖ.GetColor(16769937));
		DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10129251), ቖ.GetColor(11576434));
		SystemButton.Default = new Office2007SystemButtonStateColorTable();
		SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(7966119), ቖ.GetColor(9152460));
		SystemButton.Default.LightShade = ቖ.GetColor(16316922);
		SystemButton.Default.DarkShade = ቖ.GetColor(6645093);
		SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(7966119), ቖ.GetColor(9152460));
		SystemButton.MouseOver.LightShade = ቖ.GetColor(16316922);
		SystemButton.MouseOver.DarkShade = ቖ.GetColor(6645093);
		SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16120062), ቖ.GetColor(15397631));
		SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13821182), ቖ.GetColor(13755131));
		SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(ቖ.GetColor(16514303), Color.Transparent);
		SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(ቖ.GetColor(16514303), Color.Transparent);
		SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12702958), ቖ.GetColor(11191529));
		SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16711423), ቖ.GetColor(15135998));
		SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(7966119), ቖ.GetColor(9152460));
		SystemButton.Pressed.LightShade = ቖ.GetColor(16316922);
		SystemButton.Pressed.DarkShade = ቖ.GetColor(6645093);
		SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12244472), ቖ.GetColor(10404589));
		SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(ቖ.GetColor(12111593), Color.Transparent);
		SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(8696553), ቖ.GetColor(11523831));
		SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(ቖ.GetColor(13036285), Color.Transparent);
		SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10600421), ቖ.GetColor(11717351));
		SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13887231), ቖ.GetColor(14477819));
		Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(3889794),
			ቖ.GetColor(11650785),
			ቖ.GetColor(12769783),
			ቖ.GetColor(12573695),
			ቖ.GetColor(12573695)
		};
		Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(12633806),
			ቖ.GetColor(13424354),
			ቖ.GetColor(13950700),
			ቖ.GetColor(12573695),
			ቖ.GetColor(12573695)
		};
		Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(15002614), ቖ.GetColor(14346749));
		Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(13295351), ቖ.GetColor(15003645));
		Form.Active.CaptionBottomBorder = new Color[2]
		{
			ቖ.GetColor(14480638),
			ቖ.GetColor(11587575)
		};
		Form.Active.CaptionText = ቖ.GetColor(4090538);
		Form.Active.CaptionTextExtra = ቖ.GetColor(6910073);
		Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(14936044), ቖ.GetColor(14607853));
		Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(14213612), ቖ.GetColor(14936303));
		Form.Inactive.CaptionText = ቖ.GetColor(10526880);
		Form.Inactive.CaptionTextExtra = ቖ.GetColor(10526880);
		Form.BackColor = ቖ.GetColor(12769783);
		Form.TextColor = ቖ.GetColor(1393291);
		QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14608372), ቖ.GetColor(15134457));
		QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14411767), ቖ.GetColor(13228526));
		QuickAccessToolbar.Active.OutterBorderColor = ቖ.GetColor(16185852);
		QuickAccessToolbar.Active.MiddleBorderColor = ቖ.GetColor(10138581);
		QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15133939), ቖ.GetColor(13555942));
		QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13555942), ቖ.GetColor(13161443));
		QuickAccessToolbar.Inactive.OutterBorderColor = ቖ.GetColor(16185852);
		QuickAccessToolbar.Inactive.MiddleBorderColor = ቖ.GetColor(10138581);
		QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11718125), ቖ.GetColor(11191786));
		QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(8298957);
		QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		QuickAccessToolbar.Standalone.InnerBorderColor = ቖ.GetColor(14477559);
		QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(14542830);
		QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(5486);
		QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(ቖ.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(ቖ.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		TabControl.Default = new Office2007TabItemStateColorTable();
		TabControl.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		TabControl.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		TabControl.Default.InnerBorder = ቖ.GetColor(15988733);
		TabControl.Default.OuterBorder = ቖ.GetColor(9610695);
		TabControl.Default.Text = ቖ.GetColor(1395347);
		TabControl.MouseOver = new Office2007TabItemStateColorTable();
		TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776683), ቖ.GetColor(16772264));
		TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767577), ቖ.GetColor(16770701));
		TabControl.MouseOver.InnerBorder = ቖ.GetColor(16777211);
		TabControl.MouseOver.OuterBorder = ቖ.GetColor(11967859);
		TabControl.MouseOver.Text = ቖ.GetColor(1395347);
		TabControl.Selected = new Office2007TabItemStateColorTable();
		TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(Color.White), ቖ.GetColor(16645630));
		TabControl.Selected.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16645630), ቖ.GetColor(16645630));
		TabControl.Selected.InnerBorder = ቖ.GetColor(Color.White);
		TabControl.Selected.OuterBorder = ቖ.GetColor(9610695);
		TabControl.Selected.Text = ቖ.GetColor(1395347);
		TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(14938111), ቖ.GetColor(11588351));
		TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(16645630), ቖ.GetColor(10337507));
		TabControl.TabPanelBorder = ቖ.GetColor(9610695);
		Office2007CheckBoxColorTable checkBoxItem = CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = ቖ.GetColor(11256286);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(10661049)), Color.FromArgb(164, ቖ.GetColor(16185078)));
		checkBoxItem.Default.CheckInnerBorder = ቖ.GetColor(10661049);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.Default.Text = ቖ.GetColor(1393291);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(14609146), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = ቖ.GetColor(5601187);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(16438650)), Color.FromArgb(128, ቖ.GetColor(16709863)));
		checkBoxItem.MouseOver.CheckInnerBorder = ቖ.GetColor(16438650);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.MouseOver.Text = ቖ.GetColor(1393291);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(12703989), Color.Empty);
		checkBoxItem.Pressed.CheckBorder = ቖ.GetColor(5601187);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(15894822)), Color.FromArgb(164, ቖ.GetColor(16774357)));
		checkBoxItem.Pressed.CheckInnerBorder = ቖ.GetColor(15894822);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.Pressed.Text = ቖ.GetColor(1393291);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = ቖ.GetColor(11448757);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(14738149)), Color.FromArgb(164, ቖ.GetColor(16514043)));
		checkBoxItem.Disabled.CheckInnerBorder = ቖ.GetColor(14738149);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(9276813), Color.Empty);
		checkBoxItem.Disabled.Text = ቖ.GetColor(9276813);
		ዂ.ዱ(this, ቖ);
		ዂ.ዳ(this, ቖ);
		Office2007ProgressBarColorTable progressBarItem = ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = ቖ.GetColor(14607084);
		progressBarItem.InnerBorder = ቖ.GetColor(7640770);
		progressBarItem.Chunk = new GradientColorTable(6918698, 15200980, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = ቖ.GetColor(14607084);
		progressBarItem.InnerBorder = ቖ.GetColor(7640770);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = ቖ.GetColor(14607084);
		progressBarItem.InnerBorder = ቖ.GetColor(7640770);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		Office2007GalleryColorTable gallery = Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(14542830);
		gallery.GroupLabelText = ቖ.GetColor(5486);
		gallery.GroupLabelBorder = ቖ.GetColor(12961221);
		LegacyColors.BarBackground = ቖ.GetColor(14938111);
		LegacyColors.BarBackground2 = ቖ.GetColor(11128574);
		LegacyColors.BarStripeColor = ቖ.GetColor(7314905);
		LegacyColors.BarCaptionBackground = ቖ.GetColor(6656975);
		LegacyColors.BarCaptionBackground2 = ቖ.GetColor(3630240);
		LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(14938111);
		LegacyColors.BarCaptionInactiveBackground2 = ቖ.GetColor(11522815);
		LegacyColors.BarCaptionInactiveText = ቖ.GetColor(538482);
		LegacyColors.BarCaptionText = ቖ.GetColor(16777215);
		LegacyColors.BarFloatingBorder = ቖ.GetColor(3630240);
		LegacyColors.BarPopupBackground = ቖ.GetColor(16185078);
		LegacyColors.BarPopupBorder = ቖ.GetColor(6656975);
		LegacyColors.ItemBackground = Color.Empty;
		LegacyColors.ItemBackground2 = Color.Empty;
		LegacyColors.ItemCheckedBackground = ቖ.GetColor(16569720);
		LegacyColors.ItemCheckedBackground2 = ቖ.GetColor(16500815);
		LegacyColors.ItemCheckedBorder = ቖ.GetColor(12276995);
		LegacyColors.ItemCheckedText = ቖ.GetColor(0);
		LegacyColors.ItemDisabledBackground = Color.Empty;
		LegacyColors.ItemDisabledText = ቖ.GetColor(9276813);
		LegacyColors.ItemExpandedShadow = Color.Empty;
		LegacyColors.ItemExpandedBackground = ቖ.GetColor(14938110);
		LegacyColors.ItemExpandedBackground2 = ቖ.GetColor(10076144);
		LegacyColors.ItemExpandedText = ቖ.GetColor(0);
		LegacyColors.ItemHotBackground = ቖ.GetColor(16774604);
		LegacyColors.ItemHotBackground2 = ቖ.GetColor(16767861);
		LegacyColors.ItemHotBorder = ቖ.GetColor(16760169);
		LegacyColors.ItemHotText = ቖ.GetColor(0);
		LegacyColors.ItemPressedBackground = ቖ.GetColor(16553789);
		LegacyColors.ItemPressedBackground2 = ቖ.GetColor(16758878);
		LegacyColors.ItemPressedBorder = ቖ.GetColor(16485436);
		LegacyColors.ItemPressedText = ቖ.GetColor(0);
		LegacyColors.ItemSeparator = Color.FromArgb(80, ቖ.GetColor(10143487));
		LegacyColors.ItemSeparatorShade = Color.FromArgb(250, ቖ.GetColor(16777215));
		LegacyColors.ItemText = ቖ.GetColor(0);
		LegacyColors.MenuBackground = ቖ.GetColor(16185078);
		LegacyColors.MenuBackground2 = Color.Empty;
		LegacyColors.MenuBarBackground = ቖ.GetColor(12573695);
		LegacyColors.MenuBorder = ቖ.GetColor(6656975);
		LegacyColors.ItemExpandedBorder = LegacyColors.MenuBorder;
		LegacyColors.MenuSide = ቖ.GetColor(15331054);
		LegacyColors.MenuSide2 = Color.Empty;
		LegacyColors.MenuUnusedBackground = LegacyColors.MenuBackground;
		LegacyColors.MenuUnusedSide = ቖ.GetColor(14342874);
		LegacyColors.MenuUnusedSide2 = Color.Empty;
		LegacyColors.ItemDesignTimeBorder = Color.Black;
		LegacyColors.BarDockedBorder = ቖ.GetColor(7314905);
		LegacyColors.DockSiteBackColor = ቖ.GetColor(12573695);
		LegacyColors.DockSiteBackColor2 = Color.Empty;
		LegacyColors.CustomizeBackground = ቖ.GetColor(14149887);
		LegacyColors.CustomizeBackground2 = ቖ.GetColor(7314905);
		LegacyColors.CustomizeText = ቖ.GetColor(0);
		LegacyColors.PanelBackground = ቖ.GetColor(14938111);
		LegacyColors.PanelBackground2 = ቖ.GetColor(11522815);
		LegacyColors.PanelText = ቖ.GetColor(538482);
		LegacyColors.PanelBorder = ቖ.GetColor(6656975);
		LegacyColors.ExplorerBarBackground = ቖ.GetColor(12896468);
		LegacyColors.ExplorerBarBackground2 = ቖ.GetColor(11645896);
		NavigationPane.ButtonBackground = new GradientColorTable();
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14938111), 0f));
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12901887), 0.4f));
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(11391487), 0.4f));
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12639231), 1f));
		SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(13228527));
		SuperTooltip.TextColor = ቖ.GetColor(5000268);
		Office2007SliderColorTable slider = Slider;
		slider.Default.LabelColor = ቖ.GetColor(663677);
		slider.Default.SliderLabelColor = ቖ.GetColor(1980763);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15857404), 0.15f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(11454958), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(6985936), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15857404), 1f));
		slider.Default.PartBorderColor = ቖ.GetColor(3102605);
		slider.Default.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.Default.PartForeColor = ቖ.GetColor(5661557);
		slider.Default.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(15200505));
		slider.Default.TrackLineColor = ቖ.GetColor(7640770);
		slider.Default.TrackLineLightColor = ቖ.GetColor(14607084);
		slider.MouseOver.LabelColor = ቖ.GetColor(598113);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(1980763);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776693), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16768899), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14526221), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 1f));
		slider.MouseOver.PartBorderColor = ቖ.GetColor(3102605);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.MouseOver.PartForeColor = ቖ.GetColor(6775370);
		slider.MouseOver.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(15200505));
		slider.MouseOver.TrackLineColor = ቖ.GetColor(7640770);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(14607084);
		slider.Pressed.LabelColor = ቖ.GetColor(598113);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(1980763);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14382360), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16224303), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15561728), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16761224), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15913653), 1f));
		slider.Pressed.PartBorderColor = ቖ.GetColor(3102605);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.Pressed.PartForeColor = ቖ.GetColor(6771006);
		slider.Pressed.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(15200505));
		slider.Pressed.TrackLineColor = ቖ.GetColor(7640770);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(14607084);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(9276813);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(9276813);
		slider.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color2 in slider.Default.PartBackground.Colors)
		{
			slider.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color2.Color), color2.Position));
		}
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		ቮ(this, ቖ);
		ListViewEx.Border = ቖ.GetColor(color);
		ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(12901887));
		ListViewEx.ColumnSeparator = ቖ.GetColor(10143487);
		ListViewEx.ColumnForeColor = ቖ.GetColor(1393291);
		ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(10997232), Color.Empty);
		ListViewEx.SelectionBorder = ቖ.GetColor(14938111);
		DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(10401486);
		DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(16383229), ቖ.GetColor(13884393), 90);
		DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16374175), ቖ.GetColor(15843679), 90);
		DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(15897910);
		DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(16766349), ቖ.GetColor(15897146), 90);
		DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(15897910);
		DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(14672612), ቖ.GetColor(12371410), 90);
		DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(8888247);
		DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(12371410), ቖ.GetColor(14672612), 90);
		DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(16777215);
		DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15002871));
		DataGridView.RowNormalBorder = ቖ.GetColor(10401486);
		DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16766349));
		DataGridView.RowSelectedBorder = ቖ.GetColor(15897910);
		DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(15897910);
		DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		DataGridView.RowMouseOverBorder = ቖ.GetColor(15897910);
		DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(12305617));
		DataGridView.RowPressedBorder = ቖ.GetColor(16777215);
		DataGridView.GridColor = ቖ.GetColor(13686757);
		DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(11125993));
		DataGridView.SelectorBorder = ቖ.GetColor(10401486);
		DataGridView.SelectorBorderDark = ቖ.GetColor(11587575);
		DataGridView.SelectorBorderLight = ቖ.GetColor(14017778);
		DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(16382715), ቖ.GetColor(14146274));
		DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(9150645));
		DataGridView.SelectorMouseOverBorder = ቖ.GetColor(10401486);
		DataGridView.SelectorMouseOverBorderDark = ቖ.GetColor(11587575);
		DataGridView.SelectorMouseOverBorderLight = ቖ.GetColor(14017778);
		DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(16382715), ቖ.GetColor(14146274));
		SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		SideBar.Border = ቖ.GetColor(6656975);
		SideBar.SideBarPanelItemText = ቖ.GetColor(1393291);
		SideBar.SideBarPanelItemDefault = new GradientColorTable();
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14938111), 0f));
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12901887), 0.4f));
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(11391487), 0.4f));
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12639231), 1f));
		SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16505781), 0f));
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16697208), 0.4f));
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16692310), 0.4f));
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16640927), 1f));
		SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776409), 0f));
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770957), 0.4f));
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16766792), 0.4f));
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770963), 1f));
		SideBar.SideBarPanelItemPressed = new GradientColorTable();
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16300137), 0f));
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16622433), 0.4f));
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16484924), 0.4f));
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16694112), 1f));
		AdvTree = new TreeColorTable();
		_135B._135C(AdvTree, ቖ);
		CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1393291);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1393291);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFCD9"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFE78D"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFD748"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFE793"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FFB8A98E");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1393291);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1393291);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFC59B61"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFEEB469"), 0.1f),
			new BackgroundColorBlend(ቖ.GetColor("FFFCA060"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFB8E3D"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFEBB60"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("408B7654");
		WarningBox.BackColor = ቖ.GetColor(Color.FromArgb(255, 196, 219, 249));
		WarningBox.WarningBorderColor = ቖ.GetColor(Color.FromArgb(255, 162, 188, 213));
		WarningBox.WarningBackColor1 = ቖ.GetColor(Color.FromArgb(255, 255, 255, 255));
		WarningBox.WarningBackColor2 = ቖ.GetColor(Color.FromArgb(255, 229, 244, 254));
		CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(6130889)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(15002870),
				ቖ.GetColor(14082801),
				ቖ.GetColor(12768491),
				ቖ.GetColor(13688559)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(14017009)),
			new ColorDef(ቖ.GetColor(2706554)),
			new ColorDef(ቖ.GetColor(6130889)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12308457),
				ቖ.GetColor(9285337)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(16119285)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(14938111)),
			new ColorDef(ቖ.GetColor(6656975)),
			new ColorDef(ቖ.GetColor(6656975)),
			new ColorDef(ቖ.GetColor(6656975)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14805492),
				ቖ.GetColor(14082801),
				ቖ.GetColor(12768491),
				ቖ.GetColor(13688559)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(6130889)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14805492),
				ቖ.GetColor(14082801),
				ቖ.GetColor(12768491),
				ቖ.GetColor(13688559)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }, 0f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(6130889)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14805492),
				ቖ.GetColor(14082801),
				ቖ.GetColor(12768491),
				ቖ.GetColor(13688559)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(6130889)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12308457),
				ቖ.GetColor(9285337)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215))
		};
		CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(4944290)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16645887),
				ቖ.GetColor(12702698)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(11650540),
				ቖ.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12773564),
				ቖ.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16369568),
				ቖ.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12957158),
				ቖ.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(15837868),
				ቖ.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16776362),
				ቖ.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(-1)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(8388736))
		};
		SuperTab = ዂ.ዙ(ቖ);
		SuperTabItem = ዂ.ዚ(ቖ);
		SuperTabPanel = ዂ.ዛ(ቖ);
		Backstage = ዂ.ዜ(ቖ);
		BackstageItem = ዂ.ዝ(ቖ);
		BackstagePanel = ዂ.ዞ(ቖ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(11256286);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(14345715);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(1393291);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(9621584);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(1393291);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(12703984);
		switchButtonColorTable.SwitchBorderColor = ቖ.GetColor(8824271);
		switchButtonColorTable.TextColor = ቖ.GetColor(1393291);
		SwitchButton = new SwitchButtonColors();
		SwitchButton.Default = switchButtonColorTable;
		SwitchButton.Disabled.BorderColor = CheckBoxItem.Disabled.CheckBorder;
		SwitchButton.Disabled.SwitchBorderColor = SwitchButton.Disabled.BorderColor;
		SwitchButton.Disabled.OffTextColor = CheckBoxItem.Disabled.Text;
		SwitchButton.Disabled.OnTextColor = SwitchButton.Disabled.OffTextColor;
		SwitchButton.Disabled.TextColor = SwitchButton.Disabled.OffTextColor;
		SwitchButton.Disabled.SwitchBackColor = CheckBoxItem.Disabled.CheckInnerBackground.Start;
		SwitchButton.Disabled.OffBackColor = CheckBoxItem.Disabled.CheckInnerBackground.Start;
		SwitchButton.Disabled.OnBackColor = SwitchButton.Disabled.OffBackColor;
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(12177645);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = ቖ.GetColor(13952760);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዸ(this);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዹ(this);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዺ(this);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዻ(this);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዼ(this);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(color));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(color));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(color));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(color));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(color), SystemColors.Window);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጌ(ቖ, eOffice2007ColorScheme.Blue);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ListViewEx);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(Bar);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(color), Color.Empty);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(16383229), ቖ.GetColor(13884393), ቖ.GetColor(10401486));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(16383229), ቖ.GetColor(13884393), ቖ.GetColor(10401486));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(0));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(Color.White), ቖ.GetColor("FF567DB0"), ቖ.GetColor("FF2F578D"));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጙ(CheckBoxItem.Default.CheckBorder);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(Color.White));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(Color.DarkGray));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(SystemColors.Window);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(this);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(this);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(ቖ.GetColor(16119543));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(LegacyColors.PanelBorder), Color.Empty);
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(1393291));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(1393291), ቖ.GetColor(15002614));
		ቐ.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(Color.White), ቖ.GetColor(3889794));
		ቐ.Add(elementStyle.Class, elementStyle);
		Office2010ColorTable._21B0(this, ColorFactory.Empty);
		StepIndicator.BackgroundColor = ቖ.GetColor(ColorFunctions.GetShade(Form.BackColor, 10));
		StepIndicator.IndicatorColor = ቖ.GetColor(10800168);
		RadialMenu = new RadialMenuColorTable();
		StepItem.Default = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14938111),
			ቖ.GetColor(12901887),
			ቖ.GetColor(11391487),
			ቖ.GetColor(12639231)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(1393291), new Color[2]
		{
			ቖ.GetColor(7836601),
			ቖ.GetColor(16777207)
		});
		StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16776409),
			ቖ.GetColor(16770957),
			ቖ.GetColor(16766792),
			ቖ.GetColor(16770963)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(1393291), new Color[2]
		{
			ቖ.GetColor(12625782),
			ቖ.GetColor(16773822)
		});
		StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16300137),
			ቖ.GetColor(16622433),
			ቖ.GetColor(16484924),
			ቖ.GetColor(16694112)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(5486), new Color[2]
		{
			ቖ.GetColor(6258614),
			ቖ.GetColor(11636829)
		});
		ListBoxItem.Default = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0));
		ListBoxItem.MouseOver = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0), new Color[2]
		{
			ቖ.GetColor(16776409),
			ቖ.GetColor(16770963)
		}, 90, new float[2] { 0f, 1f });
		ListBoxItem.Selected = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0), new Color[2]
		{
			ቖ.GetColor(16569720),
			ቖ.GetColor(16500815)
		}, 90, new float[2] { 0f, 1f });
		Flyout.BorderColor = Form.Active.BorderColors[0];
		SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(1393291));
		SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(1393291), new Color[4]
		{
			ቖ.GetColor(16776409),
			ቖ.GetColor(16770957),
			ቖ.GetColor(16766792),
			ቖ.GetColor(16770963)
		}, new Color[2]
		{
			ቖ.GetColor(12625782),
			ቖ.GetColor(16773822)
		}, 2);
		SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(1393291), new Color[4]
		{
			ቖ.GetColor(16300137),
			ቖ.GetColor(16622433),
			ቖ.GetColor(16484924),
			ቖ.GetColor(16694112)
		}, new Color[2]
		{
			ቖ.GetColor(6258614),
			ቖ.GetColor(16624913)
		}, 2);
		SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(1393291), new Color[1] { Form.BackColor }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(9286371),
			Color.Empty,
			ቖ.GetColor(9286371)
		});
		SideNav.BorderColors = new Color[1] { ቖ.GetColor(9286371) };
		SideNav.TitleBackColor = ቖ.GetColor(14411509);
		SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(9286371)
		};
		SideNav.PanelBackColor = Form.BackColor;
		TabForm.Active.CaptionText = ቖ.GetColor(4090538);
		TabForm.Inactive.CaptionText = ቖ.GetColor(10526880);
		TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(Color.White) };
		TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(Color.White) };
		TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, _1257(ቖ, 1393291));
		TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, _125E(ቖ));
		TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, _125F(ቖ));
		TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, ቡ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, ቢ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, ቦ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, ቧ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, ቨ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, ቩ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, ቪ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, ቫ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, ቭ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, ቬ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, ባ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, ቤ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, ብ(ቖ));
		TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, በ(ቖ));
	}

	internal static TabFormItemColorTable _1257(ColorFactory ቖ, int ໆ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BorderColors = new Color[2]
		{
			ቖ.GetColor(12763842),
			ቖ.GetColor(64, Color.White)
		};
		tabFormItemColorTable.Default.BackColors = new Color[2]
		{
			ቖ.GetColor(14607079),
			ቖ.GetColor(13488856)
		};
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(ໆ);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.MouseOver = tabFormItemColorTable.Default;
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[2]
		{
			ቖ.GetColor(Color.White),
			ቖ.GetColor(16119543)
		};
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(11711154) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(ໆ);
		tabFormItemColorTable.Name = TabFormItem.DefaultColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable _1257(ColorFactory ቖ, Color[] ቘ, Color[] _1259, Color ቚ, Color[] ቛ, Color[] ቜ, Color ቝ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BorderColors = _1259;
		tabFormItemColorTable.Default.BackColors = ቘ;
		tabFormItemColorTable.Default.TextColor = ቚ;
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BorderColors = _1259;
		tabFormItemColorTable.MouseOver.BackColors = ቘ;
		tabFormItemColorTable.MouseOver.TextColor = ቝ;
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = ቛ;
		tabFormItemColorTable.Selected.BorderColors = ቜ;
		tabFormItemColorTable.Selected.TextColor = ቝ;
		tabFormItemColorTable.Name = TabFormItem.DefaultColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable _125E(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 10141926) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(9090786) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(10141926) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(9090786) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(13426674) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(12112621) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.BlueColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable _125F(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 15979125) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(15845981) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(15979125) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(15845981) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(16378297) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(16244381) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.YellowColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable በ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 16033446) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(15965083) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(16033446) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(15965083) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(16437713) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(16302786) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.RedColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቡ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 14067411) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(13867471) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(14067411) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(13867471) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(15389416) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(14991585) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.PurpleColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቢ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 10080479) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(8899033) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(10080479) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(8899033) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(13363183) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(12050152) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.CyanColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ባ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 15841407) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(15839343) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(15841407) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(15839343) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(16309182) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(16240552) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.OrangeColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቤ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 9097875) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(8047763) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(9097875) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(8047763) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(12970441) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(11395260) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.GreenColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ብ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 15902929) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(13867471) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(15902929) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(13867471) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(16372456) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(15975902) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.MagentaColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቦ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 10466017) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(9874398) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(10466017) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(9874398) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(13556208) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(12635627) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.BlueMistColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቧ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 11841503) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(11183579) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(11841503) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(11183579) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(14210799) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(13421033) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.PurpleMistColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቨ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 13939352) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(13476997) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(13939352) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(13476997) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(15325387) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(14731446) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.TanColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቩ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 12704386) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(11850599) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(12704386) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(11850599) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(14740928) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(13821091) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.LemonLimeColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቪ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 11261323) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(10342008) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(11261323) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(10342008) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(14019270) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(12771756) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.AppleColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቫ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 8967362) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(7720376) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(8967362) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(7720376) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(12905185) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(11330005) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.TealColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቬ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 12831183) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(12173511) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(12831183) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(12173511) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(14738407) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(14014941) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.SilverColorTableKey;
		return tabFormItemColorTable;
	}

	internal static TabFormItemColorTable ቭ(ColorFactory ቖ)
	{
		TabFormItemColorTable tabFormItemColorTable = new TabFormItemColorTable();
		tabFormItemColorTable.Default.BackColors = new Color[1] { ቖ.GetColor(128, 15902929) };
		tabFormItemColorTable.Default.BorderColors = new Color[1] { ቖ.GetColor(15441865) };
		tabFormItemColorTable.Default.TextColor = ቖ.GetColor(4473924);
		tabFormItemColorTable.Disabled = tabFormItemColorTable.Default.Clone();
		tabFormItemColorTable.Disabled.TextColor = ቖ.GetColor(11053224);
		tabFormItemColorTable.Selected = new TabFormItemStateColorTable();
		tabFormItemColorTable.Selected.BackColors = new Color[1] { ቖ.GetColor(15902929) };
		tabFormItemColorTable.Selected.BorderColors = new Color[1] { ቖ.GetColor(15441865) };
		tabFormItemColorTable.Selected.TextColor = ቖ.GetColor(2236962);
		tabFormItemColorTable.MouseOver = new TabFormItemStateColorTable();
		tabFormItemColorTable.MouseOver.BackColors = new Color[1] { ቖ.GetColor(16372431) };
		tabFormItemColorTable.MouseOver.BorderColors = new Color[1] { ቖ.GetColor(15975902) };
		tabFormItemColorTable.MouseOver.TextColor = tabFormItemColorTable.Selected.TextColor;
		tabFormItemColorTable.Name = TabFormItem.RedChalkColorTableKey;
		return tabFormItemColorTable;
	}

	internal static void ቮ(Office2007ColorTable ቯ, ColorFactory ቖ)
	{
		Office2010RangeSliderColorTable rangeSlider = ቯ.RangeSlider;
		Office2007SliderColorTable slider = ቯ.Slider;
		rangeSlider.TickLineColor = slider.Default.TrackLineColor;
		rangeSlider.RangeBackground = slider.Default.PartBackground.Clone();
		rangeSlider.RangeLineColor = slider.Default.PartBorderColor;
		rangeSlider.RangeValueBackground = new GradientColorTable();
		rangeSlider.RangeValueBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14809300), 0f));
		rangeSlider.RangeValueBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(9823594), 0.8f));
		rangeSlider.RangeValueBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(9823594), 0.85f));
		rangeSlider.RangeValueBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(11857557), 1f));
		rangeSlider.RangeValueLineColor = rangeSlider.RangeLineColor;
		rangeSlider.MinRangeSlider.Default.Background = slider.Default.PartBackground.Clone();
		rangeSlider.MinRangeSlider.Default.BorderColor = slider.Default.PartBorderColor;
		rangeSlider.MinRangeSlider.Default.BorderLightColor = slider.Default.PartBorderLightColor;
		rangeSlider.MinRangeSlider.Disabled.Background = slider.Disabled.PartBackground.Clone();
		rangeSlider.MinRangeSlider.Disabled.BorderColor = slider.Disabled.PartBorderColor;
		rangeSlider.MinRangeSlider.Disabled.BorderLightColor = slider.Disabled.PartBorderLightColor;
		rangeSlider.MinRangeSlider.MouseOver.Background = slider.MouseOver.PartBackground.Clone();
		rangeSlider.MinRangeSlider.MouseOver.BorderColor = slider.MouseOver.PartBorderColor;
		rangeSlider.MinRangeSlider.MouseOver.BorderLightColor = slider.MouseOver.PartBorderLightColor;
		rangeSlider.MinRangeSlider.Pressed.Background = slider.Pressed.PartBackground.Clone();
		rangeSlider.MinRangeSlider.Pressed.BorderColor = slider.Pressed.PartBorderColor;
		rangeSlider.MinRangeSlider.Pressed.BorderLightColor = slider.Pressed.PartBorderLightColor;
		rangeSlider.MaxRangeSlider = rangeSlider.MinRangeSlider;
	}

	public Office2007ColorTable(eOffice2007ColorScheme scheme)
	{
		switch (scheme)
		{
		case eOffice2007ColorScheme.Black:
			ዂ.ዶ(this, ቒ);
			break;
		case eOffice2007ColorScheme.Silver:
			_21C4._21B2(this, ቒ);
			break;
		case eOffice2007ColorScheme.VistaGlass:
			Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, ቒ);
			break;
		default:
			ቕ(ቒ);
			break;
		}
		լ = scheme;
	}

	public Office2007ColorTable(eOffice2007ColorScheme scheme, ColorFactory colorFactory)
	{
		ቒ = colorFactory;
		switch (scheme)
		{
		case eOffice2007ColorScheme.Black:
			ዂ.ዶ(this, ቒ);
			break;
		case eOffice2007ColorScheme.Silver:
			_21C4._21B2(this, ቒ);
			break;
		case eOffice2007ColorScheme.VistaGlass:
			Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, ቒ);
			break;
		default:
			ቕ(ቒ);
			break;
		}
		լ = scheme;
	}

	public Office2007ColorTable(eOffice2007ColorScheme scheme, Color baseSchemeColor)
	{
		if (baseSchemeColor.IsEmpty)
		{
			ቒ = ColorFactory.Empty;
		}
		else
		{
			ቒ = new ColorBlendFactory(baseSchemeColor);
		}
		switch (scheme)
		{
		case eOffice2007ColorScheme.Black:
			ዂ.ዶ(this, ቒ);
			break;
		case eOffice2007ColorScheme.Silver:
			_21C4._21B2(this, ቒ);
			break;
		case eOffice2007ColorScheme.VistaGlass:
			Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, ቒ);
			break;
		default:
			ቕ(ቒ);
			break;
		}
		լ = scheme;
	}

	internal static Office2007ButtonItemColorTable ተ()
	{
		if (ቓ != null)
		{
			return ቓ;
		}
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዂ._12C7(ColorFactory.Empty);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቓ = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	public ElementStyle GetClass(string className)
	{
		if (ቐ.ContainsKey(className))
		{
			return (ElementStyle)ቐ[className];
		}
		return null;
	}

	public void Dispose()
	{
		if (RibbonControl.StartButtonDefault != null)
		{
			RibbonControl.StartButtonDefault.Dispose();
			RibbonControl.StartButtonDefault = null;
		}
		if (RibbonControl.StartButtonMouseOver != null)
		{
			RibbonControl.StartButtonMouseOver.Dispose();
			RibbonControl.StartButtonMouseOver = null;
		}
		if (RibbonControl.StartButtonPressed != null)
		{
			RibbonControl.StartButtonPressed.Dispose();
			RibbonControl.StartButtonPressed = null;
		}
	}

	public static string GetContextualKey(Type colorTableType, Type containerType)
	{
		return colorTableType.Name + "+" + containerType.FullName;
	}

	public static string GetContextualKey(Type colorTableType, string containerQualifier)
	{
		return colorTableType.Name + "+" + containerQualifier;
	}

	public static string GetContextualKey(Type colorTableType)
	{
		return colorTableType.Name;
	}
}
