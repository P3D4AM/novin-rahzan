using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Schedule;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class Office2007ColorTable : IElementStyleClassProvider, IDisposable
{
	private Office2007ButtonItemColorTableCollection m_ButtonItemColors = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection m_RibbonButtonItemColors = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection m_MenuButtonItemColors = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection _ApplicationButtonColors = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection _BackstageButtonColors = new Office2007ButtonItemColorTableCollection();

	private Office2007ButtonItemColorTableCollection _NavigationPaneButtonItemColors = new Office2007ButtonItemColorTableCollection();

	private Office2007RibbonTabItemColorTableCollection m_RibbonTabItemColors = new Office2007RibbonTabItemColorTableCollection();

	private Office2007RibbonTabGroupColorTableCollection m_RibbonTabGroupColors = new Office2007RibbonTabGroupColorTableCollection();

	private eOffice2007ColorScheme m_ColorScheme;

	private Hashtable m_StyleClasses = new Hashtable();

	private Dictionary<string, object> m_ContextualTables = new Dictionary<string, object>();

	private ColorFactory m_ColorFactory = new ColorFactory();

	public Office2007RibbonBarColorTable RibbonBar = new Office2007RibbonBarColorTable();

	private static Office2007ButtonItemColorTable FallBackButtonItemColorTable;

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

	public Office2007SuperTooltipColorTable SuperTooltip = new Office2007SuperTooltipColorTable();

	public Office2007ListViewColorTable ListViewEx = new Office2007ListViewColorTable();

	public Office2007DataGridViewColorTable DataGridView = new Office2007DataGridViewColorTable();

	public Office2007SideBarColorTable SideBar = new Office2007SideBarColorTable();

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

	public Dictionary<Type, LabelColors> LabelItemColors = new Dictionary<Type, LabelColors>();

	public StepIndicatorColorTable StepIndicator = new StepIndicatorColorTable();

	private static bool _CloneImagesOnAccess;

	public Office2007ButtonItemColorTableCollection ButtonItemColors => m_ButtonItemColors;

	public Office2007ButtonItemColorTableCollection RibbonButtonItemColors => m_RibbonButtonItemColors;

	public Office2007ButtonItemColorTableCollection MenuButtonItemColors => m_MenuButtonItemColors;

	public Office2007ButtonItemColorTableCollection ApplicationButtonColors => _ApplicationButtonColors;

	public Office2007ButtonItemColorTableCollection BackstageButtonItemColors => _BackstageButtonColors;

	public Office2007ButtonItemColorTableCollection NavigationPaneButtonItemColors => _NavigationPaneButtonItemColors;

	public Office2007RibbonTabItemColorTableCollection RibbonTabItemColors => m_RibbonTabItemColors;

	public Office2007RibbonTabGroupColorTableCollection RibbonTabGroupColors => m_RibbonTabGroupColors;

	public eOffice2007ColorScheme InitialColorScheme => m_ColorScheme;

	public Hashtable StyleClasses => m_StyleClasses;

	public static bool CloneImagesOnAccess
	{
		get
		{
			return _CloneImagesOnAccess;
		}
		set
		{
			_CloneImagesOnAccess = value;
		}
	}

	public Dictionary<string, object> ContextualTables => m_ContextualTables;

	public Office2007ColorTable()
	{
		InitializeBlueColorTable(m_ColorFactory);
	}

	public Office2007ColorTable(ColorFactory colorFactory)
	{
		m_ColorFactory = colorFactory;
		InitializeBlueColorTable(m_ColorFactory);
	}

	private void InitializeBlueColorTable(ColorFactory factory)
	{
		RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonNormal.png");
		RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonHot.png");
		RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonPressed.png");
		RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(9286371), factory.GetColor(8954306));
		RibbonControl.InnerBorder = new LinearGradientColorTable(factory.GetColor(15200248), factory.GetColor(12646911));
		RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(12573695), Color.Empty);
		RibbonControl.TabDividerBorder = factory.GetColor(11455216);
		RibbonControl.TabDividerBorderLight = factory.GetColor(13951989);
		RibbonControl.CornerSize = 3;
		RibbonControl.PanelTopBackgroundHeight = 15f;
		RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(14411508), factory.GetColor(13622767));
		RibbonControl.PanelBottomBackground = new LinearGradientColorTable(factory.GetColor(13228525), factory.GetColor(15201023));
		ItemGroup.OuterBorder = new LinearGradientColorTable(factory.GetColor(10073824), factory.GetColor(7574717));
		ItemGroup.InnerBorder = new LinearGradientColorTable(factory.GetColor(14017521), factory.GetColor(14937595));
		ItemGroup.TopBackground = new LinearGradientColorTable(factory.GetColor(13163502), factory.GetColor(13229558));
		ItemGroup.BottomBackground = new LinearGradientColorTable(factory.GetColor(12374249), factory.GetColor(13689335));
		ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, factory.GetColor(12110044));
		ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, factory.GetColor(16777215));
		RibbonBar.Default = Office2007ColorTableFactory.GetRibbonBarBlue(factory);
		RibbonBar.MouseOver = Office2007ColorTableFactory.GetRibbonBarBlueMouseOver(factory);
		RibbonBar.Expanded = Office2007ColorTableFactory.GetRibbonBarBlueExpanded(factory);
		Office2007ButtonItemColorTable buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		m_ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueOrangeWithBackground(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		m_ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueBlue(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		m_ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueBlueWithBackground(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		m_ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueMagenta(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		m_ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueMagentaWithBackground(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		m_ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemOffice2007WithBackground(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		m_ButtonItemColors.Add(buttonItemBlueOrange);
		m_ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
		Office2007RibbonTabItemColorTable ribbonTabItemBlueDefault = Office2007ColorTableFactory.GetRibbonTabItemBlueDefault(factory);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		m_RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = Office2007ColorTableFactory.GetRibbonTabItemBlueMagenta(factory);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		m_RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = Office2007ColorTableFactory.GetRibbonTabItemBlueGreen(factory);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		m_RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = Office2007ColorTableFactory.GetRibbonTabItemBlueOrange(factory);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		m_RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		Office2007RibbonTabGroupColorTable ribbonTabGroupBlueDefault = Office2007ColorTableFactory.GetRibbonTabGroupBlueDefault(factory);
		ribbonTabGroupBlueDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		m_RibbonTabGroupColors.Add(ribbonTabGroupBlueDefault);
		ribbonTabGroupBlueDefault = Office2007ColorTableFactory.GetRibbonTabGroupBlueMagenta(factory);
		ribbonTabGroupBlueDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		m_RibbonTabGroupColors.Add(ribbonTabGroupBlueDefault);
		ribbonTabGroupBlueDefault = Office2007ColorTableFactory.GetRibbonTabGroupBlueGreen(factory);
		ribbonTabGroupBlueDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		m_RibbonTabGroupColors.Add(ribbonTabGroupBlueDefault);
		ribbonTabGroupBlueDefault = Office2007ColorTableFactory.GetRibbonTabGroupBlueOrange(factory);
		ribbonTabGroupBlueDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		m_RibbonTabGroupColors.Add(ribbonTabGroupBlueDefault);
		Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		Bar.ToolbarBottomBorder = factory.GetColor(12244215);
		Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(16448250), Color.Empty);
		Bar.PopupToolbarBorder = factory.GetColor(8816262);
		Bar.StatusBarTopBorder = factory.GetColor(5668272);
		Bar.StatusBarTopBorderLight = factory.GetColor(Color.FromArgb(148, Color.White));
		Bar.StatusBarAltBackground.Clear();
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(12967160), 0f));
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(11127543), 0.4f));
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(9484010), 0.4f));
		Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(7640514), 1f));
		Menu.Background = new LinearGradientColorTable(factory.GetColor(16448250), Color.Empty);
		Menu.Border = new LinearGradientColorTable(factory.GetColor(8816262), Color.Empty);
		Menu.Side = new LinearGradientColorTable(factory.GetColor(15331054), Color.Empty);
		Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(12961221), Color.Empty);
		Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(16119285), Color.Empty);
		Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(15066597), Color.Empty);
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
		Menu.FileColumnOneBackground = factory.GetColor(16448250);
		Menu.FileColumnOneBorder = factory.GetColor(12961221);
		Menu.FileColumnTwoBackground = factory.GetColor(15330030);
		Menu.FileBottomContainerBackgroundBlend.Clear();
		Menu.FileBottomContainerBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(189, 211, 239), 0f),
			new BackgroundColorBlend(Color.FromArgb(190, 212, 240), 0.4f),
			new BackgroundColorBlend(Color.FromArgb(176, 201, 234), 0.4f),
			new BackgroundColorBlend(Color.FromArgb(207, 224, 245), 1f)
		});
		ComboBox.Default.Background = factory.GetColor(15397627);
		ComboBox.Default.Border = factory.GetColor(11256286);
		ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ComboBox.Default.ExpandText = factory.GetColor(1393291);
		ComboBox.DefaultStandalone.Background = factory.GetColor(16777215);
		int color = 11782113;
		ComboBox.DefaultStandalone.Border = factory.GetColor(color);
		ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(15003902), factory.GetColor(13623283), 90);
		ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable();
		ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(11519199), Color.Empty, 90);
		ComboBox.DefaultStandalone.ExpandText = factory.GetColor(1393291);
		ComboBox.MouseOver.Background = factory.GetColor(16777215);
		ComboBox.MouseOver.Border = factory.GetColor(color);
		ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(16776418), factory.GetColor(16770981), 90);
		ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(16776435), 90);
		ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(14405273), Color.Empty, 90);
		ComboBox.MouseOver.ExpandText = factory.GetColor(1393291);
		ComboBox.DroppedDown.Background = factory.GetColor(16777215);
		ComboBox.DroppedDown.Border = factory.GetColor(color);
		ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(15392959), factory.GetColor(16766038), 90);
		ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(15854549), factory.GetColor(16770708), 90);
		ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(10129251), Color.Empty, 90);
		ComboBox.DroppedDown.ExpandText = factory.GetColor(1393291);
		DialogLauncher.Default.DialogLauncher = factory.GetColor(6721199);
		DialogLauncher.Default.DialogLauncherShade = factory.GetColor(16777215);
		DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(6721199);
		DialogLauncher.MouseOver.DialogLauncherShade = Color.FromArgb(192, factory.GetColor(16777215));
		DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776415), factory.GetColor(16773031));
		DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767349), factory.GetColor(16769944));
		DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(16776178));
		DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14405273), Color.Empty);
		DialogLauncher.Pressed.DialogLauncher = factory.GetColor(6721199);
		DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(192, factory.GetColor(16777215));
		DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(15261373), factory.GetColor(15386253));
		DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16754488), factory.GetColor(16763982));
		DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(15788756), factory.GetColor(16769937));
		DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(10129251), factory.GetColor(11576434));
		SystemButton.Default = new Office2007SystemButtonStateColorTable();
		SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(7966119), factory.GetColor(9152460));
		SystemButton.Default.LightShade = factory.GetColor(16316922);
		SystemButton.Default.DarkShade = factory.GetColor(6645093);
		SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(7966119), factory.GetColor(9152460));
		SystemButton.MouseOver.LightShade = factory.GetColor(16316922);
		SystemButton.MouseOver.DarkShade = factory.GetColor(6645093);
		SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16120062), factory.GetColor(15397631));
		SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(13821182), factory.GetColor(13755131));
		SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(factory.GetColor(16514303), Color.Transparent);
		SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(factory.GetColor(16514303), Color.Transparent);
		SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12702958), factory.GetColor(11191529));
		SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16711423), factory.GetColor(15135998));
		SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(7966119), factory.GetColor(9152460));
		SystemButton.Pressed.LightShade = factory.GetColor(16316922);
		SystemButton.Pressed.DarkShade = factory.GetColor(6645093);
		SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(12244472), factory.GetColor(10404589));
		SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(factory.GetColor(12111593), Color.Transparent);
		SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(8696553), factory.GetColor(11523831));
		SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(factory.GetColor(13036285), Color.Transparent);
		SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(10600421), factory.GetColor(11717351));
		SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(13887231), factory.GetColor(14477819));
		Form.Active.BorderColors = new Color[5]
		{
			factory.GetColor(3889794),
			factory.GetColor(11650785),
			factory.GetColor(12769783),
			factory.GetColor(12573695),
			factory.GetColor(12573695)
		};
		Form.Inactive.BorderColors = new Color[5]
		{
			factory.GetColor(12633806),
			factory.GetColor(13424354),
			factory.GetColor(13950700),
			factory.GetColor(12573695),
			factory.GetColor(12573695)
		};
		Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(15002614), factory.GetColor(14346749));
		Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(13295351), factory.GetColor(15003645));
		Form.Active.CaptionBottomBorder = new Color[2]
		{
			factory.GetColor(14480638),
			factory.GetColor(11587575)
		};
		Form.Active.CaptionText = factory.GetColor(4090538);
		Form.Active.CaptionTextExtra = factory.GetColor(6910073);
		Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(14936044), factory.GetColor(14607853));
		Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(14213612), factory.GetColor(14936303));
		Form.Inactive.CaptionText = factory.GetColor(10526880);
		Form.Inactive.CaptionTextExtra = factory.GetColor(10526880);
		Form.BackColor = factory.GetColor(12769783);
		Form.TextColor = factory.GetColor(1393291);
		QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(factory.GetColor(14608372), factory.GetColor(15134457));
		QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(factory.GetColor(14411767), factory.GetColor(13228526));
		QuickAccessToolbar.Active.OutterBorderColor = factory.GetColor(16185852);
		QuickAccessToolbar.Active.MiddleBorderColor = factory.GetColor(10138581);
		QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(factory.GetColor(15133939), factory.GetColor(13555942));
		QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(factory.GetColor(13555942), factory.GetColor(13161443));
		QuickAccessToolbar.Inactive.OutterBorderColor = factory.GetColor(16185852);
		QuickAccessToolbar.Inactive.MiddleBorderColor = factory.GetColor(10138581);
		QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(11718125), factory.GetColor(11191786));
		QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(8298957);
		QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		QuickAccessToolbar.Standalone.InnerBorderColor = factory.GetColor(14477559);
		QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(14542830);
		QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(5486);
		QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		TabControl.Default = new Office2007TabItemStateColorTable();
		TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		TabControl.Default.InnerBorder = factory.GetColor(15988733);
		TabControl.Default.OuterBorder = factory.GetColor(9610695);
		TabControl.Default.Text = factory.GetColor(1395347);
		TabControl.MouseOver = new Office2007TabItemStateColorTable();
		TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776683), factory.GetColor(16772264));
		TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767577), factory.GetColor(16770701));
		TabControl.MouseOver.InnerBorder = factory.GetColor(16777211);
		TabControl.MouseOver.OuterBorder = factory.GetColor(11967859);
		TabControl.MouseOver.Text = factory.GetColor(1395347);
		TabControl.Selected = new Office2007TabItemStateColorTable();
		TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.White), factory.GetColor(16645630));
		TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(16645630), factory.GetColor(16645630));
		TabControl.Selected.InnerBorder = factory.GetColor(Color.White);
		TabControl.Selected.OuterBorder = factory.GetColor(9610695);
		TabControl.Selected.Text = factory.GetColor(1395347);
		TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(14938111), factory.GetColor(11588351));
		TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(16645630), factory.GetColor(10337507));
		TabControl.TabPanelBorder = factory.GetColor(9610695);
		Office2007CheckBoxColorTable checkBoxItem = CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = factory.GetColor(11256286);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(10661049)), Color.FromArgb(164, factory.GetColor(16185078)));
		checkBoxItem.Default.CheckInnerBorder = factory.GetColor(10661049);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.Default.Text = factory.GetColor(1393291);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(14609146), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = factory.GetColor(5601187);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(16438650)), Color.FromArgb(128, factory.GetColor(16709863)));
		checkBoxItem.MouseOver.CheckInnerBorder = factory.GetColor(16438650);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.MouseOver.Text = factory.GetColor(1393291);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(12703989), Color.Empty);
		checkBoxItem.Pressed.CheckBorder = factory.GetColor(5601187);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(15894822)), Color.FromArgb(164, factory.GetColor(16774357)));
		checkBoxItem.Pressed.CheckInnerBorder = factory.GetColor(15894822);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.Pressed.Text = factory.GetColor(1393291);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = factory.GetColor(11448757);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(14738149)), Color.FromArgb(164, factory.GetColor(16514043)));
		checkBoxItem.Disabled.CheckInnerBorder = factory.GetColor(14738149);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(9276813), Color.Empty);
		checkBoxItem.Disabled.Text = factory.GetColor(9276813);
		Office2007ColorTableFactory.InitializeScrollBarColorTable(this, factory);
		Office2007ColorTableFactory.InitializeAppBlueScrollBarColorTable(this, factory);
		Office2007ProgressBarColorTable progressBarItem = ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = factory.GetColor(14607084);
		progressBarItem.InnerBorder = factory.GetColor(7640770);
		progressBarItem.Chunk = new GradientColorTable(6918698, 15200980, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = factory.GetColor(14607084);
		progressBarItem.InnerBorder = factory.GetColor(7640770);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = factory.GetColor(14607084);
		progressBarItem.InnerBorder = factory.GetColor(7640770);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		Office2007GalleryColorTable gallery = Gallery;
		gallery.GroupLabelBackground = factory.GetColor(14542830);
		gallery.GroupLabelText = factory.GetColor(5486);
		gallery.GroupLabelBorder = factory.GetColor(12961221);
		LegacyColors.BarBackground = factory.GetColor(14938111);
		LegacyColors.BarBackground2 = factory.GetColor(11128574);
		LegacyColors.BarStripeColor = factory.GetColor(7314905);
		LegacyColors.BarCaptionBackground = factory.GetColor(6656975);
		LegacyColors.BarCaptionBackground2 = factory.GetColor(3630240);
		LegacyColors.BarCaptionInactiveBackground = factory.GetColor(14938111);
		LegacyColors.BarCaptionInactiveBackground2 = factory.GetColor(11522815);
		LegacyColors.BarCaptionInactiveText = factory.GetColor(538482);
		LegacyColors.BarCaptionText = factory.GetColor(16777215);
		LegacyColors.BarFloatingBorder = factory.GetColor(3630240);
		LegacyColors.BarPopupBackground = factory.GetColor(16185078);
		LegacyColors.BarPopupBorder = factory.GetColor(6656975);
		LegacyColors.ItemBackground = Color.Empty;
		LegacyColors.ItemBackground2 = Color.Empty;
		LegacyColors.ItemCheckedBackground = factory.GetColor(16569720);
		LegacyColors.ItemCheckedBackground2 = factory.GetColor(16500815);
		LegacyColors.ItemCheckedBorder = factory.GetColor(12276995);
		LegacyColors.ItemCheckedText = factory.GetColor(0);
		LegacyColors.ItemDisabledBackground = Color.Empty;
		LegacyColors.ItemDisabledText = factory.GetColor(9276813);
		LegacyColors.ItemExpandedShadow = Color.Empty;
		LegacyColors.ItemExpandedBackground = factory.GetColor(14938110);
		LegacyColors.ItemExpandedBackground2 = factory.GetColor(10076144);
		LegacyColors.ItemExpandedText = factory.GetColor(0);
		LegacyColors.ItemHotBackground = factory.GetColor(16774604);
		LegacyColors.ItemHotBackground2 = factory.GetColor(16767861);
		LegacyColors.ItemHotBorder = factory.GetColor(16760169);
		LegacyColors.ItemHotText = factory.GetColor(0);
		LegacyColors.ItemPressedBackground = factory.GetColor(16553789);
		LegacyColors.ItemPressedBackground2 = factory.GetColor(16758878);
		LegacyColors.ItemPressedBorder = factory.GetColor(16485436);
		LegacyColors.ItemPressedText = factory.GetColor(0);
		LegacyColors.ItemSeparator = Color.FromArgb(80, factory.GetColor(10143487));
		LegacyColors.ItemSeparatorShade = Color.FromArgb(250, factory.GetColor(16777215));
		LegacyColors.ItemText = factory.GetColor(0);
		LegacyColors.MenuBackground = factory.GetColor(16185078);
		LegacyColors.MenuBackground2 = Color.Empty;
		LegacyColors.MenuBarBackground = factory.GetColor(12573695);
		LegacyColors.MenuBorder = factory.GetColor(6656975);
		LegacyColors.ItemExpandedBorder = LegacyColors.MenuBorder;
		LegacyColors.MenuSide = factory.GetColor(15331054);
		LegacyColors.MenuSide2 = Color.Empty;
		LegacyColors.MenuUnusedBackground = LegacyColors.MenuBackground;
		LegacyColors.MenuUnusedSide = factory.GetColor(14342874);
		LegacyColors.MenuUnusedSide2 = Color.Empty;
		LegacyColors.ItemDesignTimeBorder = Color.Black;
		LegacyColors.BarDockedBorder = factory.GetColor(7314905);
		LegacyColors.DockSiteBackColor = factory.GetColor(12573695);
		LegacyColors.DockSiteBackColor2 = Color.Empty;
		LegacyColors.CustomizeBackground = factory.GetColor(14149887);
		LegacyColors.CustomizeBackground2 = factory.GetColor(7314905);
		LegacyColors.CustomizeText = factory.GetColor(0);
		LegacyColors.PanelBackground = factory.GetColor(14938111);
		LegacyColors.PanelBackground2 = factory.GetColor(11522815);
		LegacyColors.PanelText = factory.GetColor(538482);
		LegacyColors.PanelBorder = factory.GetColor(6656975);
		LegacyColors.ExplorerBarBackground = factory.GetColor(12896468);
		LegacyColors.ExplorerBarBackground2 = factory.GetColor(11645896);
		NavigationPane.ButtonBackground = new GradientColorTable();
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14938111), 0f));
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12901887), 0.4f));
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(11391487), 0.4f));
		NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12639231), 1f));
		SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(13228527));
		SuperTooltip.TextColor = factory.GetColor(5000268);
		Office2007SliderColorTable slider = Slider;
		slider.Default.LabelColor = factory.GetColor(663677);
		slider.Default.SliderLabelColor = factory.GetColor(1980763);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15857404), 0.15f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(11454958), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(6985936), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0.85f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 1f));
		slider.Default.PartBorderColor = factory.GetColor(3102605);
		slider.Default.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.Default.PartForeColor = factory.GetColor(5661557);
		slider.Default.PartForeLightColor = Color.FromArgb(168, factory.GetColor(15200505));
		slider.Default.TrackLineColor = factory.GetColor(7640770);
		slider.Default.TrackLineLightColor = factory.GetColor(14607084);
		slider.MouseOver.LabelColor = factory.GetColor(598113);
		slider.MouseOver.SliderLabelColor = factory.GetColor(1980763);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16776693), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16768899), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14526221), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16774350), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16774350), 1f));
		slider.MouseOver.PartBorderColor = factory.GetColor(3102605);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.MouseOver.PartForeColor = factory.GetColor(6775370);
		slider.MouseOver.PartForeLightColor = Color.FromArgb(168, factory.GetColor(15200505));
		slider.MouseOver.TrackLineColor = factory.GetColor(7640770);
		slider.MouseOver.TrackLineLightColor = factory.GetColor(14607084);
		slider.Pressed.LabelColor = factory.GetColor(598113);
		slider.Pressed.SliderLabelColor = factory.GetColor(1980763);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14382360), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16224303), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15561728), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16761224), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15913653), 1f));
		slider.Pressed.PartBorderColor = factory.GetColor(3102605);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.Pressed.PartForeColor = factory.GetColor(6771006);
		slider.Pressed.PartForeLightColor = Color.FromArgb(168, factory.GetColor(15200505));
		slider.Pressed.TrackLineColor = factory.GetColor(7640770);
		slider.Pressed.TrackLineLightColor = factory.GetColor(14607084);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = factory.GetColor(9276813);
		slider.Disabled.SliderLabelColor = factory.GetColor(9276813);
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
		ListViewEx.Border = factory.GetColor(color);
		ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(12901887));
		ListViewEx.ColumnSeparator = factory.GetColor(10143487);
		ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(10997232), Color.Empty);
		ListViewEx.SelectionBorder = factory.GetColor(14938111);
		DataGridView.ColumnHeaderNormalBorder = factory.GetColor(10401486);
		DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(16383229), factory.GetColor(13884393), 90);
		DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(16374175), factory.GetColor(15843679), 90);
		DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(15897910);
		DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(16766349), factory.GetColor(15897146), 90);
		DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(15897910);
		DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(14672612), factory.GetColor(12371410), 90);
		DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(8888247);
		DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(12371410), factory.GetColor(14672612), 90);
		DataGridView.ColumnHeaderPressedBorder = factory.GetColor(16777215);
		DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(15002871));
		DataGridView.RowNormalBorder = factory.GetColor(10401486);
		DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(16766349));
		DataGridView.RowSelectedBorder = factory.GetColor(15897910);
		DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		DataGridView.RowSelectedMouseOverBorder = factory.GetColor(15897910);
		DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		DataGridView.RowMouseOverBorder = factory.GetColor(15897910);
		DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(12305617));
		DataGridView.RowPressedBorder = factory.GetColor(16777215);
		DataGridView.GridColor = factory.GetColor(13686757);
		DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(11125993));
		DataGridView.SelectorBorder = factory.GetColor(10401486);
		DataGridView.SelectorBorderDark = factory.GetColor(11587575);
		DataGridView.SelectorBorderLight = factory.GetColor(14017778);
		DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(16382715), factory.GetColor(14146274));
		DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(9150645));
		DataGridView.SelectorMouseOverBorder = factory.GetColor(10401486);
		DataGridView.SelectorMouseOverBorderDark = factory.GetColor(11587575);
		DataGridView.SelectorMouseOverBorderLight = factory.GetColor(14017778);
		DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(16382715), factory.GetColor(14146274));
		SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
		SideBar.Border = factory.GetColor(6656975);
		SideBar.SideBarPanelItemText = factory.GetColor(1393291);
		SideBar.SideBarPanelItemDefault = new GradientColorTable();
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14938111), 0f));
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(12901887), 0.4f));
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(11391487), 0.4f));
		SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(12639231), 1f));
		SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16505781), 0f));
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16697208), 0.4f));
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16692310), 0.4f));
		SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16640927), 1f));
		SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16776409), 0f));
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770957), 0.4f));
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16766792), 0.4f));
		SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770963), 1f));
		SideBar.SideBarPanelItemPressed = new GradientColorTable();
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16300137), 0f));
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16622433), 0.4f));
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16484924), 0.4f));
		SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16694112), 1f));
		CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1393291);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1393291);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor("FFFCD9"), 0f),
			new BackgroundColorBlend(factory.GetColor("FFE78D"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFD748"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFE793"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor("FFB8A98E");
		crumbBarItemViewStateColorTable.BorderLight = factory.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1393291);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(factory.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = factory.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1393291);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor("FFC59B61"), 0f),
			new BackgroundColorBlend(factory.GetColor("FFEEB469"), 0.1f),
			new BackgroundColorBlend(factory.GetColor("FFFCA060"), 0.6f),
			new BackgroundColorBlend(factory.GetColor("FFFB8E3D"), 0.6f),
			new BackgroundColorBlend(factory.GetColor("FFFEBB60"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = factory.GetColor("408B7654");
		WarningBox.BackColor = factory.GetColor(Color.FromArgb(255, 196, 219, 249));
		WarningBox.WarningBorderColor = factory.GetColor(Color.FromArgb(255, 162, 188, 213));
		WarningBox.WarningBackColor1 = factory.GetColor(Color.FromArgb(255, 255, 255, 255));
		WarningBox.WarningBackColor2 = factory.GetColor(Color.FromArgb(255, 229, 244, 254));
		CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(factory.GetColor(6130889)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(15002870),
				factory.GetColor(14082801),
				factory.GetColor(12768491),
				factory.GetColor(13688559)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(15134199)),
			new ColorDef(factory.GetColor(10862561)),
			new ColorDef(factory.GetColor(14017009)),
			new ColorDef(factory.GetColor(2706554)),
			new ColorDef(factory.GetColor(6130889)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(12308457),
				factory.GetColor(9285337)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(16119285)),
			new ColorDef(factory.GetColor(15436032)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(factory.GetColor(15436032))
		};
		CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(factory.GetColor(14938111)),
			new ColorDef(factory.GetColor(6656975)),
			new ColorDef(factory.GetColor(6656975)),
			new ColorDef(factory.GetColor(6656975)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(factory.GetColor(15436032))
		};
		CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14805492),
				factory.GetColor(14082801),
				factory.GetColor(12768491),
				factory.GetColor(13688559)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(6130889)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14805492),
				factory.GetColor(14082801),
				factory.GetColor(12768491),
				factory.GetColor(13688559)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }, 0f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(6130889)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14805492),
				factory.GetColor(14082801),
				factory.GetColor(12768491),
				factory.GetColor(13688559)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(15134199)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(10862561)),
			new ColorDef(factory.GetColor(6130889)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(12308457),
				factory.GetColor(9285337)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(15436032)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(16777215))
		};
		CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(factory.GetColor(4944290)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(16645887),
				factory.GetColor(12702698)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(11650540),
				factory.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(12773564),
				factory.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(16369568),
				factory.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(12957158),
				factory.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(15837868),
				factory.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(16776362),
				factory.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(-1)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(8388736))
		};
		SuperTab = Office2007ColorTableFactory.GetSuperTabBlueDefault(factory);
		SuperTabItem = Office2007ColorTableFactory.GetSuperTabItemBlueDefault(factory);
		SuperTabPanel = Office2007ColorTableFactory.GetSuperTabPanelBlueDefault(factory);
		Backstage = Office2007ColorTableFactory.GetBackstageBlueDefault(factory);
		BackstageItem = Office2007ColorTableFactory.GetBackstageItemBlueDefault(factory);
		BackstagePanel = Office2007ColorTableFactory.GetBackstagePanelBlueDefault(factory);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = factory.GetColor(11256286);
		switchButtonColorTable.OffBackColor = factory.GetColor(14345715);
		switchButtonColorTable.OffTextColor = factory.GetColor(1393291);
		switchButtonColorTable.OnBackColor = factory.GetColor(9621584);
		switchButtonColorTable.OnTextColor = factory.GetColor(1393291);
		switchButtonColorTable.SwitchBackColor = factory.GetColor(12703984);
		switchButtonColorTable.SwitchBorderColor = factory.GetColor(8824271);
		switchButtonColorTable.TextColor = factory.GetColor(1393291);
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
		elementStyle.BorderColor = factory.GetColor(12177645);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = factory.GetColor(13952760);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetFileMenuContainerStyle(this);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetTwoColumnMenuContainerStyle(this);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMenuColumnOneContainerStyle(this);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMenuColumnTwoContainerStyle(this);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMenuBottomContainer(this);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(color));
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetRichTextBoxStyle(factory.GetColor(color));
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(color));
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(color), SystemColors.Window);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetRibbonClientPanelStyle(factory, eOffice2007ColorScheme.Blue);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetListViewBorderStyle(ListViewEx);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetStatusBarAltStyle(Bar);
		m_StyleClasses.Add(elementStyle.Class, elementStyle);
		Office2010ColorTable.InitAppButtonColors(this, ColorFactory.Empty);
		StepIndicator.BackgroundColor = factory.GetColor(ColorFunctions.GetShade(Form.BackColor, 10));
		StepIndicator.IndicatorColor = factory.GetColor(10800168);
	}

	public Office2007ColorTable(eOffice2007ColorScheme scheme)
	{
		switch (scheme)
		{
		case eOffice2007ColorScheme.Black:
			Office2007ColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
			break;
		case eOffice2007ColorScheme.Silver:
			Office2007SilverColorTableFactory.InitializeColorTable(this, m_ColorFactory);
			break;
		case eOffice2007ColorScheme.VistaGlass:
			Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
			break;
		default:
			InitializeBlueColorTable(m_ColorFactory);
			break;
		}
		m_ColorScheme = scheme;
	}

	public Office2007ColorTable(eOffice2007ColorScheme scheme, ColorFactory colorFactory)
	{
		m_ColorFactory = colorFactory;
		switch (scheme)
		{
		case eOffice2007ColorScheme.Black:
			Office2007ColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
			break;
		case eOffice2007ColorScheme.Silver:
			Office2007SilverColorTableFactory.InitializeColorTable(this, m_ColorFactory);
			break;
		case eOffice2007ColorScheme.VistaGlass:
			Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
			break;
		default:
			InitializeBlueColorTable(m_ColorFactory);
			break;
		}
		m_ColorScheme = scheme;
	}

	public Office2007ColorTable(eOffice2007ColorScheme scheme, Color baseSchemeColor)
	{
		if (baseSchemeColor.IsEmpty)
		{
			m_ColorFactory = ColorFactory.Empty;
		}
		else
		{
			m_ColorFactory = new ColorBlendFactory(baseSchemeColor);
		}
		switch (scheme)
		{
		case eOffice2007ColorScheme.Black:
			Office2007ColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
			break;
		case eOffice2007ColorScheme.Silver:
			Office2007SilverColorTableFactory.InitializeColorTable(this, m_ColorFactory);
			break;
		case eOffice2007ColorScheme.VistaGlass:
			Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
			break;
		default:
			InitializeBlueColorTable(m_ColorFactory);
			break;
		}
		m_ColorScheme = scheme;
	}

	internal static Office2007ButtonItemColorTable GetFallBackButtonItemColorTable()
	{
		if (FallBackButtonItemColorTable != null)
		{
			return FallBackButtonItemColorTable;
		}
		Office2007ButtonItemColorTable buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueOrange(ColorFactory.Empty);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		FallBackButtonItemColorTable = buttonItemBlueOrange;
		return buttonItemBlueOrange;
	}

	public ElementStyle GetClass(string className)
	{
		if (m_StyleClasses.ContainsKey(className))
		{
			return (ElementStyle)m_StyleClasses[className];
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
}
