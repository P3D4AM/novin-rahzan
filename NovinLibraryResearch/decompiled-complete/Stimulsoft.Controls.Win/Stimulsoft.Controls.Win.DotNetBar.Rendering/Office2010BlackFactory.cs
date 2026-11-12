using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Schedule;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal static class Office2010BlackFactory
{
	public static void InitializeColorTable(Office2007ColorTable table, ColorFactory factory)
	{
		table.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010NormalSilver.png");
		table.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010HotSilver.png");
		table.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010PressedSilver.png");
		table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(4079423), factory.GetColor(5131854));
		table.RibbonControl.InnerBorder = LinearGradientColorTable.Empty;
		table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(7434609));
		table.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, factory.GetColor(4802889));
		table.RibbonControl.TabDividerBorder = Color.Empty;
		table.RibbonControl.TabDividerBorderLight = Color.Empty;
		table.RibbonControl.CornerSize = 1;
		table.RibbonControl.PanelTopBackgroundHeight = 0f;
		table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(13224393), factory.GetColor(10724259));
		table.RibbonControl.PanelBottomBackground = null;
		table.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		table.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		table.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		table.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		table.ItemGroup.ItemGroupDividerDark = Color.Empty;
		table.ItemGroup.ItemGroupDividerLight = Color.Empty;
		table.RibbonBar.Default = GetRibbonBar(factory);
		table.RibbonBar.MouseOver = GetRibbonBarMouseOver(factory);
		table.RibbonBar.Expanded = GetRibbonBarExpanded(factory);
		table.RibbonButtonItemColors.Clear();
		table.ButtonItemColors.Clear();
		table.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable buttonItemBlueOrange = GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = GetButtonItemBlueOrangeWithBackground(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueBlue(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueBlueWithBackground(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueMagenta(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2007ColorTableFactory.GetButtonItemBlueMagentaWithBackground(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = Office2010BlueFactory.GetButtonItemOffice2007WithBackground(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
		table.BackstageButtonItemColors.Clear();
		table.BackstageButtonItemColors.Add(GetButtonItemBackstageDefault(factory));
		table.NavigationPaneButtonItemColors.Clear();
		table.NavigationPaneButtonItemColors.Add(GetNavigationPaneButtonItemTable(factory));
		table.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable ribbonTabItemDefault = GetRibbonTabItemDefault(factory);
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		ribbonTabItemDefault = GetRibbonTabItemMagenta(factory);
		ribbonTabItemDefault.CornerSize = 2;
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		ribbonTabItemDefault = GetRibbonTabItemBlueGreen(factory);
		ribbonTabItemDefault.CornerSize = 2;
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		ribbonTabItemDefault = GetRibbonTabItemBlueOrange(factory);
		ribbonTabItemDefault.CornerSize = 2;
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		table.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable ribbonTabGroupDefault = Office2010BlueFactory.GetRibbonTabGroupDefault(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = Office2010BlueFactory.GetRibbonTabGroupMagenta(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = Office2010BlueFactory.GetRibbonTabGroupGreen(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = Office2010BlueFactory.GetRibbonTabGroupOrange(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(4934475), factory.GetColor(4144959));
		table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(4144959), factory.GetColor(3355443));
		table.Bar.ToolbarBottomBorder = factory.GetColor(5395026);
		table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.Bar.PopupToolbarBorder = factory.GetColor(10988464);
		table.Bar.StatusBarTopBorder = factory.GetColor(2894892);
		table.Bar.StatusBarTopBorderLight = factory.GetColor(Color.FromArgb(32, Color.White));
		table.Bar.StatusBarAltBackground.Clear();
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(4934475), 0f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(3355443), 1f));
		table.Menu.Background = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.Menu.Border = new LinearGradientColorTable(factory.GetColor(10988464), Color.Empty);
		table.Menu.Side = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(14869735), Color.Empty);
		table.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(15066597), Color.Empty);
		table.Menu.FileBackgroundBlend.Clear();
		table.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(16777215), 0f),
			new BackgroundColorBlend(factory.GetColor(16777215), 1f)
		});
		table.Menu.FileContainerBorder = factory.GetColor(10988464);
		table.Menu.FileContainerBorderLight = Color.Transparent;
		table.Menu.FileColumnOneBackground = factory.GetColor(16777215);
		table.Menu.FileColumnOneBorder = factory.GetColor(10988464);
		table.Menu.FileColumnTwoBackground = factory.GetColor(16777215);
		table.Menu.FileBottomContainerBackgroundBlend.Clear();
		table.ComboBox.Default.Background = factory.GetColor(13027014);
		table.ComboBox.Default.Border = factory.GetColor(9539985);
		table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandText = factory.GetColor(5263699);
		table.ComboBox.DefaultStandalone.Background = factory.GetColor(16777215);
		int color = 3223857;
		table.ComboBox.DefaultStandalone.Border = factory.GetColor(color);
		table.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(14869218), factory.GetColor(12895428), 90);
		table.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		table.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(9539985), Color.Empty, 90);
		table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(5592664);
		table.ComboBox.MouseOver.Background = factory.GetColor(16777215);
		table.ComboBox.MouseOver.Border = factory.GetColor(9539985);
		table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(16114849), factory.GetColor(16182985), 90);
		table.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(16512739));
		table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(15253839), Color.Empty, 90);
		table.ComboBox.MouseOver.ExpandText = factory.GetColor(6051393);
		table.ComboBox.DroppedDown.Background = factory.GetColor(16777215);
		table.ComboBox.DroppedDown.Border = factory.GetColor(9539985);
		table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(14993831), factory.GetColor(15711329), 90);
		table.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(13942953), factory.GetColor(15724985), 90);
		table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(12751419), Color.Empty, 90);
		table.ComboBox.DroppedDown.ExpandText = factory.GetColor(4542298);
		table.DialogLauncher.Default.DialogLauncher = factory.GetColor(7039851);
		table.DialogLauncher.Default.DialogLauncherShade = Color.Transparent;
		table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(9866340);
		table.DialogLauncher.MouseOver.DialogLauncherShade = Color.Transparent;
		table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(15851417), factory.GetColor(15849339));
		table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(15849339), factory.GetColor(15852463));
		table.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16446946), factory.GetColor(16447470));
		table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15056715), Color.Empty);
		table.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(7041917);
		table.DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(128, factory.GetColor(16777215));
		table.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(14993573), factory.GetColor(15583377));
		table.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(15645794), factory.GetColor(15656075));
		table.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(13942696), factory.GetColor(15724994));
		table.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(12751419), factory.GetColor(12760162));
		table.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(14474460));
		table.SystemButton.Default.LightShade = Color.Empty;
		table.SystemButton.Default.DarkShade = factory.GetColor(5395813);
		table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(14474460));
		table.SystemButton.MouseOver.LightShade = Color.Empty;
		table.SystemButton.MouseOver.DarkShade = factory.GetColor(5395813);
		table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(9737364), factory.GetColor(9145227));
		table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(9145227), factory.GetColor(7829367));
		table.SystemButton.MouseOver.TopHighlight = null;
		table.SystemButton.MouseOver.BottomHighlight = null;
		table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(5329233));
		table.SystemButton.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(14474460));
		table.SystemButton.Pressed.LightShade = Color.Empty;
		table.SystemButton.Pressed.DarkShade = factory.GetColor(5395813);
		table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(6842472));
		table.SystemButton.Pressed.TopHighlight = null;
		table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(7434609));
		table.SystemButton.Pressed.BottomHighlight = null;
		table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(5329233));
		table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(6513507));
		table.SystemButtonClose = new Office2007SystemButtonColorTable();
		table.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(factory.GetColor(14474460));
		table.SystemButtonClose.Default.LightShade = Color.Empty;
		table.SystemButtonClose.Default.DarkShade = factory.GetColor(5395813);
		table.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(14474460));
		table.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		table.SystemButtonClose.MouseOver.DarkShade = factory.GetColor(10173757);
		table.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16745602), factory.GetColor(16482174));
		table.SystemButtonClose.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16152953), factory.GetColor(14901602));
		table.SystemButtonClose.MouseOver.TopHighlight = null;
		table.SystemButtonClose.MouseOver.BottomHighlight = null;
		table.SystemButtonClose.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10173757));
		table.SystemButtonClose.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		table.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(14474460));
		table.SystemButtonClose.Pressed.LightShade = Color.Empty;
		table.SystemButtonClose.Pressed.DarkShade = factory.GetColor(10173757);
		table.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(15890294));
		table.SystemButtonClose.Pressed.TopHighlight = null;
		table.SystemButtonClose.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(15890294));
		table.SystemButtonClose.Pressed.BottomHighlight = null;
		table.SystemButtonClose.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(10173757));
		table.SystemButtonClose.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(14311779));
		table.Form.Active.BorderColors = new Color[5]
		{
			factory.GetColor(6513507),
			factory.GetColor(8618883),
			factory.GetColor(7829367),
			factory.GetColor(7829367),
			factory.GetColor(7829367)
		};
		table.Form.Inactive.BorderColors = new Color[5]
		{
			factory.GetColor(7829367),
			factory.GetColor(10395294),
			factory.GetColor(10395294),
			factory.GetColor(10395294),
			factory.GetColor(10395294)
		};
		table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(7960953), factory.GetColor(7960953));
		table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(7895160), factory.GetColor(7434609));
		table.Form.Active.CaptionBottomBorder = new Color[2]
		{
			factory.GetColor(6184542),
			factory.GetColor(8618883)
		};
		table.Form.Active.CaptionText = factory.GetColor(16777215);
		table.Form.Active.CaptionTextExtra = factory.GetColor(14869218);
		table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(10395294));
		table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(10395294));
		table.Form.Inactive.CaptionText = factory.GetColor(13948116);
		table.Form.Inactive.CaptionTextExtra = factory.GetColor(13948116);
		table.Form.BackColor = factory.GetColor(13882323);
		table.Form.TextColor = factory.GetColor(0);
		table.Form.MdiClientBackgroundImage = (Image)(object)BarFunctions.LoadBitmap("SystemImages.Office2010BlackClientBackground.png");
		table.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		table.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		table.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(7960953));
		table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(8684676);
		table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(15790837);
		table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(3881787);
		table.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		table.TabControl.Default = new Office2007TabItemStateColorTable();
		table.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(9539985), factory.GetColor(8289918));
		table.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(8289918), factory.GetColor(7105644));
		table.TabControl.Default.InnerBorder = factory.GetColor(9539985);
		table.TabControl.Default.OuterBorder = factory.GetColor(color);
		table.TabControl.Default.Text = factory.GetColor(16777215);
		table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(12698049), factory.GetColor(11447982));
		table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(11447982), factory.GetColor(9539985));
		table.TabControl.MouseOver.InnerBorder = factory.GetColor(9539985);
		table.TabControl.MouseOver.OuterBorder = factory.GetColor(3421236);
		table.TabControl.MouseOver.Text = factory.GetColor(0);
		table.TabControl.Selected = new Office2007TabItemStateColorTable();
		table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(9539985), factory.GetColor(16777215));
		table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(16777215));
		table.TabControl.Selected.InnerBorder = factory.GetColor(9539985);
		table.TabControl.Selected.OuterBorder = factory.GetColor(3421236);
		table.TabControl.Selected.Text = factory.GetColor(0);
		table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(6974058), Color.Empty);
		table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.TabControl.TabPanelBorder = factory.GetColor(color);
		Office2007CheckBoxColorTable checkBoxItem = table.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(15922165), Color.Empty);
		checkBoxItem.Default.CheckBorder = factory.GetColor(9014156);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(15264235), factory.GetColor(15987700));
		checkBoxItem.Default.CheckInnerBorder = factory.GetColor(11974842);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(7435124), Color.Empty);
		checkBoxItem.Default.Text = factory.GetColor(0);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(16576962), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = factory.GetColor(13602871);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(16444616), factory.GetColor(16577501));
		checkBoxItem.MouseOver.CheckInnerBorder = factory.GetColor(16559453);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(4933956), Color.Empty);
		checkBoxItem.MouseOver.Text = factory.GetColor(0);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(16574397));
		checkBoxItem.Pressed.CheckBorder = factory.GetColor(12741917);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(16441506), factory.GetColor(16642775));
		checkBoxItem.Pressed.CheckInnerBorder = factory.GetColor(16557400);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(4933956), Color.Empty);
		checkBoxItem.Pressed.Text = factory.GetColor(3881787);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = factory.GetColor(11974842);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(16316665));
		checkBoxItem.Disabled.CheckInnerBorder = factory.GetColor(14672096);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(11645876), Color.Empty);
		checkBoxItem.Disabled.Text = factory.GetColor(9276813);
		InitializeScrollBarColorTable(table, factory);
		InitializeAppScrollBarColorTable(table, factory);
		Office2007ProgressBarColorTable progressBarItem = table.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = factory.GetColor(3618617);
		progressBarItem.InnerBorder = factory.GetColor(10921638);
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
		progressBarItem.ChunkShadow = new GradientColorTable(8224383, 9474448, 0);
		progressBarItem = table.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = factory.GetColor(3618617);
		progressBarItem.InnerBorder = factory.GetColor(10921638);
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
		progressBarItem.ChunkShadow = new GradientColorTable(8224383, 9474448, 0);
		progressBarItem = table.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = factory.GetColor(3618617);
		progressBarItem.InnerBorder = factory.GetColor(10921638);
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
		progressBarItem.ChunkShadow = new GradientColorTable(8224383, 9474448, 0);
		Office2007GalleryColorTable gallery = table.Gallery;
		gallery.GroupLabelBackground = factory.GetColor(15790837);
		gallery.GroupLabelText = factory.GetColor(3881787);
		gallery.GroupLabelBorder = factory.GetColor(15790837);
		table.LegacyColors.BarBackground = factory.GetColor(13224393);
		table.LegacyColors.BarBackground2 = factory.GetColor(10724259);
		table.LegacyColors.BarStripeColor = factory.GetColor(5395026);
		table.LegacyColors.BarCaptionBackground = factory.GetColor(6842472);
		table.LegacyColors.BarCaptionBackground2 = Color.Empty;
		table.LegacyColors.BarCaptionInactiveBackground = factory.GetColor(8158332);
		table.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		table.LegacyColors.BarCaptionInactiveText = factory.GetColor(16777215);
		table.LegacyColors.BarCaptionText = factory.GetColor(16777215);
		table.LegacyColors.BarFloatingBorder = factory.GetColor(4473924);
		table.LegacyColors.BarPopupBackground = factory.GetColor(16777215);
		table.LegacyColors.BarPopupBorder = factory.GetColor(10988464);
		table.LegacyColors.ItemBackground = Color.Empty;
		table.LegacyColors.ItemBackground2 = Color.Empty;
		table.LegacyColors.ItemCheckedBackground = factory.GetColor(16569720);
		table.LegacyColors.ItemCheckedBackground2 = factory.GetColor(16500815);
		table.LegacyColors.ItemCheckedBorder = factory.GetColor(12276995);
		table.LegacyColors.ItemCheckedText = factory.GetColor(0);
		table.LegacyColors.ItemDisabledBackground = Color.Empty;
		table.LegacyColors.ItemDisabledText = factory.GetColor(9276813);
		table.LegacyColors.ItemExpandedShadow = Color.Empty;
		table.LegacyColors.ItemExpandedBackground = factory.GetColor(14938110);
		table.LegacyColors.ItemExpandedBackground2 = factory.GetColor(10076144);
		table.LegacyColors.ItemExpandedText = factory.GetColor(0);
		table.LegacyColors.ItemHotBackground = factory.GetColor(16774604);
		table.LegacyColors.ItemHotBackground2 = factory.GetColor(16767861);
		table.LegacyColors.ItemHotBorder = factory.GetColor(16760169);
		table.LegacyColors.ItemHotText = factory.GetColor(3881787);
		table.LegacyColors.ItemPressedBackground = factory.GetColor(16553789);
		table.LegacyColors.ItemPressedBackground2 = factory.GetColor(16758878);
		table.LegacyColors.ItemPressedBorder = factory.GetColor(16485436);
		table.LegacyColors.ItemPressedText = factory.GetColor(3881787);
		table.LegacyColors.ItemSeparator = factory.GetColor(8817553);
		table.LegacyColors.ItemSeparatorShade = Color.FromArgb(192, factory.GetColor(16645630));
		table.LegacyColors.ItemText = factory.GetColor(0);
		table.LegacyColors.MenuBackground = factory.GetColor(16777215);
		table.LegacyColors.MenuBackground2 = Color.Empty;
		table.LegacyColors.MenuBarBackground = factory.GetColor(13224393);
		table.LegacyColors.MenuBorder = factory.GetColor(10988464);
		table.LegacyColors.ItemExpandedBorder = table.LegacyColors.MenuBorder;
		table.LegacyColors.MenuSide = factory.GetColor(16777215);
		table.LegacyColors.MenuSide2 = Color.Empty;
		table.LegacyColors.MenuUnusedBackground = table.LegacyColors.MenuBackground;
		table.LegacyColors.MenuUnusedSide = factory.GetColor(14342874);
		table.LegacyColors.MenuUnusedSide2 = Color.Empty;
		table.LegacyColors.ItemDesignTimeBorder = Color.Black;
		table.LegacyColors.BarDockedBorder = factory.GetColor(6184542);
		table.LegacyColors.DockSiteBackColor = factory.GetColor(13224393);
		table.LegacyColors.DockSiteBackColor2 = Color.Empty;
		table.LegacyColors.CustomizeBackground = factory.GetColor(8684676);
		table.LegacyColors.CustomizeBackground2 = factory.GetColor(7960953);
		table.LegacyColors.CustomizeText = factory.GetColor(0);
		table.LegacyColors.PanelBackground = factory.GetColor(13224393);
		table.LegacyColors.PanelBackground2 = factory.GetColor(10724259);
		table.LegacyColors.PanelText = factory.GetColor(0);
		table.LegacyColors.PanelBorder = factory.GetColor(3158064);
		table.LegacyColors.ExplorerBarBackground = factory.GetColor(13619151);
		table.LegacyColors.ExplorerBarBackground2 = factory.GetColor(13224393);
		table.LegacyColors.SplitterBackground = factory.GetColor(6710886);
		table.LegacyColors.SplitterBackground2 = factory.GetColor(5329233);
		table.LegacyColors.SplitterBorder = factory.GetColor(3158064);
		table.LegacyColors.SplitterText = table.LegacyColors.SplitterBorder;
		table.NavigationPane.ButtonBackground = new GradientColorTable();
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(5987163), 0f));
		table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(15000816));
		table.SuperTooltip.TextColor = factory.GetColor(5000268);
		Office2007SliderColorTable slider = table.Slider;
		slider.Default.LabelColor = factory.GetColor(0);
		slider.Default.SliderLabelColor = factory.GetColor(0);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12632256), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(8158332), 1f));
		slider.Default.PartBorderColor = factory.GetColor(2500134);
		slider.Default.PartBorderLightColor = Color.Transparent;
		slider.Default.PartForeColor = factory.GetColor(3355443);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = factory.GetColor(2500134);
		slider.Default.TrackLineLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.MouseOver.LabelColor = factory.GetColor(0);
		slider.MouseOver.SliderLabelColor = factory.GetColor(0);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16776693), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16768899), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14526221), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16774350), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16774350), 1f));
		slider.MouseOver.PartBorderColor = factory.GetColor(8358292);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.MouseOver.PartForeColor = factory.GetColor(5132370);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = factory.GetColor(2500134);
		slider.MouseOver.TrackLineLightColor = factory.GetColor(13421772);
		slider.Pressed.LabelColor = factory.GetColor(0);
		slider.Pressed.SliderLabelColor = factory.GetColor(0);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14382360), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16224303), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15561728), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16761224), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15913653), 1f));
		slider.Pressed.PartBorderColor = factory.GetColor(8358292);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.Pressed.PartForeColor = factory.GetColor(5132370);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = factory.GetColor(2500134);
		slider.Pressed.TrackLineLightColor = factory.GetColor(13421772);
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
		table.ListViewEx.Border = factory.GetColor(color);
		table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(15395562), factory.GetColor(14671839));
		table.ListViewEx.ColumnSeparator = factory.GetColor(3881787);
		table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(10997232), Color.Empty);
		table.ListViewEx.SelectionBorder = factory.GetColor(5779983);
		table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(8817553);
		table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(15461872), factory.GetColor(14014684), 90);
		table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(16374175), factory.GetColor(15843679), 90);
		table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(16766349), factory.GetColor(15897146), 90);
		table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(14672612), factory.GetColor(12371410), 90);
		table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(8888247);
		table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(12371410), factory.GetColor(14672612), 90);
		table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(16777215);
		table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(15461872));
		table.DataGridView.RowNormalBorder = factory.GetColor(8817553);
		table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(16766349));
		table.DataGridView.RowSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(14672612));
		table.DataGridView.RowMouseOverBorder = factory.GetColor(8888247);
		table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(12305617));
		table.DataGridView.RowPressedBorder = factory.GetColor(16777215);
		table.DataGridView.GridColor = factory.GetColor(11184810);
		table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(13027014));
		table.DataGridView.SelectorBorder = factory.GetColor(9474450);
		table.DataGridView.SelectorBorderDark = factory.GetColor(12829635);
		table.DataGridView.SelectorBorderLight = factory.GetColor(16382457);
		table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(15921906));
		table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(9150645));
		table.DataGridView.SelectorMouseOverBorder = factory.GetColor(10401486);
		table.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(11587575);
		table.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(14017778);
		table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(16382715), factory.GetColor(14146274));
		table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
		table.SideBar.Border = factory.GetColor(color);
		table.SideBar.SideBarPanelItemText = factory.GetColor(3881787);
		table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14410215), 0f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(12305357), 1f));
		table.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16505781), 0f));
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16697208), 0.4f));
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16692310), 0.4f));
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16640927), 1f));
		table.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16776409), 0f));
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770957), 0.4f));
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16766792), 0.4f));
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770963), 1f));
		table.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16300137), 0f));
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16622433), 0.4f));
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16484924), 0.4f));
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16694112), 1f));
		table.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(3881787);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(3881787);
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
		table.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(3881787);
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
		table.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(3881787);
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
		table.WarningBox.BackColor = factory.GetColor(factory.GetColor(13882323));
		table.WarningBox.WarningBorderColor = factory.GetColor(color);
		table.WarningBox.WarningBackColor1 = factory.GetColor(16711422);
		table.WarningBox.WarningBackColor2 = factory.GetColor(15067630);
		table.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(factory.GetColor(11581118)),
			new ColorDef(factory.GetColor(3552822)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(11581118)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(15264492)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(15264492)),
			new ColorDef(factory.GetColor(5002076)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(13619928),
				factory.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(11581118)),
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
		table.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(3552822)),
			new ColorDef(factory.GetColor(12105912)),
			new ColorDef(factory.GetColor(12105912)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(factory.GetColor(15436032))
		};
		table.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14475234),
				factory.GetColor(13883098),
				factory.GetColor(11844289),
				factory.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(factory.GetColor(6384246)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14475234),
				factory.GetColor(13817306),
				factory.GetColor(12041411),
				factory.GetColor(13291220)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }, 0f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(factory.GetColor(15264492)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(13619928),
				factory.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(11581118)),
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
		table.CalendarView.AppointmentColors = new ColorDef[17]
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
		table.SuperTab.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15067629), Color.Empty);
		table.SuperTab.InnerBorder = factory.GetColor(14672872);
		table.SuperTab.OuterBorder = factory.GetColor(8356744);
		table.SuperTab.ControlBoxDefault.Image = factory.GetColor(255, 0);
		table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(16770978);
		table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(16760169);
		table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		table.SuperTab.ControlBoxPressed.Background = factory.GetColor(16485436);
		table.SuperTab.ControlBoxPressed.Border = factory.GetColor(16760169);
		table.SuperTab.ControlBoxPressed.Image = factory.GetColor(255, 0);
		table.SuperTab.InsertMarker = factory.GetColor(255, 128);
		table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(15725043),
			factory.GetColor(14804201),
			factory.GetColor(15593458),
			factory.GetColor(15725043)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		table.SuperTabItem.Default.Normal.InnerBorder = factory.GetColor(14672872);
		table.SuperTabItem.Default.Normal.OuterBorder = factory.GetColor(8356744);
		table.SuperTabItem.Default.Normal.Text = factory.GetColor(5067609);
		table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(4222879);
		table.SuperTabItem.Default.Disabled.Text = factory.GetColor(9276813);
		table.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		table.SuperTabItem.Default.Disabled.CloseMarker = factory.GetColor(9276813);
		table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(14870251),
			factory.GetColor(16382715),
			factory.GetColor(16382715),
			factory.GetColor(16777215)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(14672872);
		table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(8356744);
		table.SuperTabItem.Default.Selected.Text = factory.GetColor(5067609);
		table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(4222879);
		table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(14672872),
			factory.GetColor(16251129),
			factory.GetColor(16251129),
			factory.GetColor(16711423)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(15987958);
		table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(8817553);
		table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(5067609);
		table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(4222879);
		table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(14672872),
			factory.GetColor(16251129),
			factory.GetColor(16251129),
			factory.GetColor(16711423)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(15987958);
		table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(8817553);
		table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(5067609);
		table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(4222879);
		table.SuperTabItem.Left = table.SuperTabItem.Default;
		table.SuperTabItem.Bottom = table.SuperTabItem.Default;
		table.SuperTabItem.Right = table.SuperTabItem.Default;
		table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.SuperTabPanel.Default.InnerBorder = factory.GetColor(14672872);
		table.SuperTabPanel.Default.OuterBorder = factory.GetColor(5067609);
		table.SuperTabPanel.Left = table.SuperTabPanel.Default;
		table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
		table.SuperTabPanel.Right = table.SuperTabPanel.Default;
		SuperTabStyleColorFactory.GetOffice2010BackstageBlackColorTable(table.Backstage, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageBlackItemColorTable(table.BackstageItem, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageBlackPanelColorTable(table.BackstagePanel, factory);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = factory.GetColor(9539985);
		switchButtonColorTable.OffBackColor = factory.GetColor(13224393);
		switchButtonColorTable.OffTextColor = factory.GetColor(0);
		switchButtonColorTable.OnBackColor = factory.GetColor(9621584);
		switchButtonColorTable.OnTextColor = factory.GetColor(0);
		switchButtonColorTable.SwitchBackColor = factory.GetColor(10790052);
		switchButtonColorTable.SwitchBorderColor = factory.GetColor(5592405);
		switchButtonColorTable.TextColor = factory.GetColor(0);
		table.SwitchButton = new SwitchButtonColors();
		table.SwitchButton.Default = switchButtonColorTable;
		table.SwitchButton.Disabled.BorderColor = table.CheckBoxItem.Disabled.CheckBorder;
		table.SwitchButton.Disabled.SwitchBorderColor = table.SwitchButton.Disabled.BorderColor;
		table.SwitchButton.Disabled.OffTextColor = table.CheckBoxItem.Disabled.Text;
		table.SwitchButton.Disabled.OnTextColor = table.SwitchButton.Disabled.OffTextColor;
		table.SwitchButton.Disabled.TextColor = table.SwitchButton.Disabled.OffTextColor;
		table.SwitchButton.Disabled.SwitchBackColor = table.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		table.SwitchButton.Disabled.OffBackColor = table.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		table.SwitchButton.Disabled.OnBackColor = table.SwitchButton.Disabled.OffBackColor;
		table.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = factory.GetColor(8684676);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = factory.GetColor(12303291);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetFileMenuContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetTwoColumnMenuContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMenuColumnOneContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMenuColumnTwoContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMenuBottomContainer(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetRichTextBoxStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(color), SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2010BlueFactory.GetRibbonClientPanelStyle(factory, eOffice2010ColorScheme.Black);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetListViewBorderStyle(table.ListViewEx);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetStatusBarAltStyle(table.Bar);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewDateTimeStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewNumericStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewIpAddressStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetSlideOutButtonStyle(table.CheckBoxItem.Default.CheckBorder);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMetroTilePanelStyle(factory.GetColor(Color.White));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMetroTileGroupStyle(factory.GetColor(Color.DarkGray));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMonthCalendarStyle(SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		table.StepIndicator.BackgroundColor = factory.GetColor(ColorFunctions.GetShade(table.Form.BackColor, 10));
		table.StepIndicator.IndicatorColor = factory.GetColor(10800168);
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBar(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(12105912), factory.GetColor(5395026));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(13355979), factory.GetColor(11513775));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(13224393), factory.GetColor(10724259));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(2368548);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarMouseOver(ColorFactory factory)
	{
		return GetRibbonBar(factory);
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarExpanded(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(12105912), factory.GetColor(5395026));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(13355979), factory.GetColor(11513775));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(13224393), factory.GetColor(10724259));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(2368548);
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007ButtonItemColorTable GetButtonItemBackstageDefault(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBackstageDefault = Office2010BlueFactory.GetButtonItemBackstageDefault(factory);
		buttonItemBackstageDefault.Default.Text = factory.GetColor(16777215);
		buttonItemBackstageDefault.Checked.Text = factory.GetColor(16777215);
		buttonItemBackstageDefault.Expanded.Text = factory.GetColor(16777215);
		buttonItemBackstageDefault.MouseOver.Text = factory.GetColor(16777215);
		buttonItemBackstageDefault.Pressed.Text = factory.GetColor(16777215);
		return buttonItemBackstageDefault;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrange(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(16238656), factory.GetColor(16307239));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16775910), factory.GetColor(16777200));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16772776), factory.GetColor(16769916));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16769916), factory.GetColor(16776416));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(15253582), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16775912), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(4605510);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(16238400));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(12744235), factory.GetColor(12754503));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(16173159));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(16504440), factory.GetColor(16770186));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16770186), factory.GetColor(16704135));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(15253582), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16775912), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(4605510);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(12749360), factory.GetColor(12756045));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(16770421), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(16770421), factory.GetColor(16767083));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767083), factory.GetColor(16772217));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16774272), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(4605510);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(12749360), factory.GetColor(12756045));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(16770421), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(16770421), factory.GetColor(16767083));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767083), factory.GetColor(16772474));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16774272), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(4605510);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrangeWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueOrange = GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14869218), factory.GetColor(14277081));
		buttonItemBlueOrange.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(14277081), factory.GetColor(12895428));
		buttonItemBlueOrange.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9539985));
		buttonItemBlueOrange.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(16119285), factory.GetColor(15527148));
		buttonItemBlueOrange.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(15066597), factory.GetColor(15198183));
		buttonItemBlueOrange.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(12369084), Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.Text = factory.GetColor(9276813);
		return buttonItemBlueOrange;
	}

	public static void SetBlueExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
	{
		Color color = factory.GetColor(6182461);
		Color empty = Color.Empty;
		ct.Default.ExpandBackground = color;
		ct.Default.ExpandLight = empty;
		ct.Checked.ExpandBackground = color;
		ct.Checked.ExpandLight = empty;
		ct.Expanded.ExpandBackground = color;
		ct.Expanded.ExpandLight = empty;
		ct.MouseOver.ExpandBackground = color;
		ct.MouseOver.ExpandLight = empty;
		ct.Pressed.ExpandBackground = color;
		ct.Pressed.ExpandLight = empty;
	}

	public static Office2007ButtonItemColorTable GetNavigationPaneButtonItemTable(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(2500134));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(10987431));
		office2007ButtonItemColorTable.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(9013641), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(8289918), factory.GetColor(5329233));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(2500134));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(2631720));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(4079166));
		office2007ButtonItemColorTable.Pressed.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(4737096), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(factory.GetColor(4737096));
		office2007ButtonItemColorTable.Pressed.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(3026478));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(7105642));
		office2007ButtonItemColorTable.Checked.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(8289918), Color.Transparent);
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(factory.GetColor(5987163), factory.GetColor(5592405));
		office2007ButtonItemColorTable.Checked.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Checked;
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemDefault(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.GlassText = factory.GetColor(0);
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13619151), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(6184542));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13619151), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(6184542));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(9145486), factory.GetColor(5723991));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(11711154), factory.GetColor(10790052));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(6184542));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = factory.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemMagenta(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.Default.GlassText = factory.GetColor(0);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(12165809), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(11559317), factory.GetColor(10235767));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(12165809), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(11559317), factory.GetColor(10235767));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(10911896), factory.GetColor(5854295));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(11898021), factory.GetColor(9207944));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10235767));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = factory.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.Default.GlassText = factory.GetColor(0);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(11583660), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(5807443), factory.GetColor(1143569));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(11583660), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(5807443), factory.GetColor(1143569));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(8692860), factory.GetColor(5528147));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(9286786), factory.GetColor(8295541));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(32256), factory.GetColor(32256));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = factory.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.Default.GlassText = factory.GetColor(0);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(12233636), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(10511915), factory.GetColor(11032064));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(12233636), factory.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10511915), factory.GetColor(11032064));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(11442027), factory.GetColor(6117710));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(12692322), factory.GetColor(10392922));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(11032064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = factory.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle GetFileMenuContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 22;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend.CopyTo(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle GetTwoColumnMenuContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.BorderBottom = eStyleBorderType.Double;
		elementStyle.BorderBottomWidth = 0;
		elementStyle.BorderColor = menu.FileContainerBorder;
		elementStyle.BorderLeft = eStyleBorderType.None;
		elementStyle.BorderLeftWidth = 0;
		elementStyle.BorderRight = eStyleBorderType.None;
		elementStyle.BorderRightWidth = 0;
		elementStyle.BorderTop = eStyleBorderType.Solid;
		elementStyle.BorderTopWidth = 1;
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.PaddingBottom = 0;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 0;
		return elementStyle;
	}

	public static ElementStyle GetMenuColumnOneContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle GetMenuColumnTwoContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle GetMenuBottomContainer(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void InitializeScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(15724527), factory.GetColor(16579836), 0);
		@default.Border = new LinearGradientColorTable(factory.GetColor(15461871), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(15922163), 0f),
			new BackgroundColorBlend(factory.GetColor(15790320), 0.5f),
			new BackgroundColorBlend(factory.GetColor(15461356), 0.5f),
			new BackgroundColorBlend(factory.GetColor(15132648), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(11975359), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(9408399)), Color.FromArgb(64, Color.White));
		@default = t.ScrollBar.MouseOver;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16777215), 0f),
			new BackgroundColorBlend(factory.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(11975359));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16777215), 0f),
			new BackgroundColorBlend(factory.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(9408399));
		@default = t.ScrollBar.MouseOverControl;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16777215), 0f),
			new BackgroundColorBlend(factory.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(11975359));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16777215), 0f),
			new BackgroundColorBlend(factory.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15922164), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(9408399));
		@default = t.ScrollBar.Pressed;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(14803683), 0f),
			new BackgroundColorBlend(factory.GetColor(14803683), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(11449272));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8947849));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(14803683), 0f),
			new BackgroundColorBlend(factory.GetColor(14803683), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(9408399));
		@default = t.ScrollBar.Disabled;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(12570615), factory.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void InitializeAppScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(6513507), factory.GetColor(6842472), 0);
		@default.Border = new LinearGradientColorTable(factory.GetColor(5131854));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(3421236), factory.GetColor(2039583), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(factory.GetColor(7631988), 0f),
			new BackgroundColorBlend(factory.GetColor(6316128), 0.8f),
			new BackgroundColorBlend(factory.GetColor(5855577), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(2368548), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(10000536));
		@default = t.AppScrollBar.MouseOver;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(8355711), 0f),
			new BackgroundColorBlend(factory.GetColor(6447714), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(8355711), factory.GetColor(7237230));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(2368548), Color.Empty);
		@default.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(10000536));
		@default = t.AppScrollBar.MouseOverControl;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = t.AppScrollBar.Default.TrackSignBackground;
		@default = t.AppScrollBar.Pressed;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(7039851), 0f),
			new BackgroundColorBlend(factory.GetColor(5131854), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(2368548), Color.Empty);
		@default.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(2368548), Color.Empty);
		@default.TrackSignBackground = t.AppScrollBar.Default.TrackSignBackground;
		@default = t.AppScrollBar.Disabled;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(12570615), factory.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}
}
