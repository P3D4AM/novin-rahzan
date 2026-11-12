using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Schedule;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal static class VisualStudio2010Factory
{
	public static void InitializeColorTable(Office2007ColorTable table, ColorFactory factory)
	{
		table.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010NormalSilver.png");
		table.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010HotSilver.png");
		table.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010PressedSilver.png");
		table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(8950952));
		table.RibbonControl.InnerBorder = null;
		table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(12371928));
		table.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, factory.GetColor(12371928));
		table.RibbonControl.TabDividerBorder = Color.Empty;
		table.RibbonControl.TabDividerBorderLight = Color.Empty;
		table.RibbonControl.CornerSize = 1;
		table.RibbonControl.PanelTopBackgroundHeight = 0f;
		table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(13292514), factory.GetColor(11450829));
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
		buttonItemBlueOrange = GetButtonItemBlueBlueWithBackground(factory);
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
		buttonItemBlueOrange = GetButtonItemBlueMagentaWithBackground(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = GetButtonItemOffice2007WithBackground(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
		table.BackstageButtonItemColors.Add(GetButtonItemBackstageDefault(factory));
		table.NavigationPaneButtonItemColors.Clear();
		table.NavigationPaneButtonItemColors.Add(GetNavigationPaneButtonItemTable(factory));
		table.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable ribbonTabItemBlueDefault = GetRibbonTabItemBlueDefault(factory);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		table.RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = GetRibbonTabItemBlueMagenta(factory);
		ribbonTabItemBlueDefault.CornerSize = 2;
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		table.RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = GetRibbonTabItemBlueGreen(factory);
		ribbonTabItemBlueDefault.CornerSize = 2;
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		table.RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = GetRibbonTabItemBlueOrange(factory);
		ribbonTabItemBlueDefault.CornerSize = 2;
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		table.RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		table.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable ribbonTabGroupDefault = GetRibbonTabGroupDefault(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = GetRibbonTabGroupMagenta(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = GetRibbonTabGroupGreen(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = GetRibbonTabGroupOrange(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(12371928));
		table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(12371928));
		table.Bar.ToolbarBottomBorder = factory.GetColor(10267329);
		table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(15198957), Color.Empty);
		table.Bar.PopupToolbarBorder = factory.GetColor(10201015);
		table.Bar.StatusBarTopBorder = factory.GetColor(10267329);
		table.Bar.StatusBarTopBorderLight = Color.Empty;
		table.Bar.StatusBarAltBackground.Clear();
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(10267329), 0f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(10267329), 1f));
		table.Menu.Background = new LinearGradientColorTable(factory.GetColor(15330542), factory.GetColor(13686754));
		table.Menu.Border = new LinearGradientColorTable(factory.GetColor(10201015), Color.Empty);
		table.Menu.Side = new LinearGradientColorTable(factory.GetColor(15330542), Color.Empty);
		table.Menu.SideBorder = LinearGradientColorTable.Empty;
		table.Menu.SideBorderLight = LinearGradientColorTable.Empty;
		table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(12371928), Color.Empty);
		table.Menu.FileBackgroundBlend.Clear();
		table.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(15330542), 0f),
			new BackgroundColorBlend(factory.GetColor(13686754), 1f)
		});
		table.Menu.FileContainerBorder = factory.GetColor(10201015);
		table.Menu.FileContainerBorderLight = Color.Transparent;
		table.Menu.FileColumnOneBackground = factory.GetColor(15330542);
		table.Menu.FileColumnOneBorder = factory.GetColor(12501963);
		table.Menu.FileColumnTwoBackground = Color.Empty;
		table.Menu.FileBottomContainerBackgroundBlend.Clear();
		table.ComboBox.Default.Background = factory.GetColor(15856632);
		int color = 8753570;
		table.ComboBox.Default.Border = factory.GetColor(color);
		table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandText = factory.GetColor(5922145);
		table.ComboBox.DefaultStandalone.Background = factory.GetColor(16777215);
		table.ComboBox.DefaultStandalone.Border = factory.GetColor(color);
		table.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(13292514), factory.GetColor(11450829), 90);
		table.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		table.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(11254480), Color.Empty, 90);
		table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(1780030);
		table.ComboBox.MouseOver.Background = factory.GetColor(16777215);
		table.ComboBox.MouseOver.Border = factory.GetColor(15057765);
		table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(16776434), factory.GetColor(16772277), 90);
		table.ComboBox.MouseOver.ExpandBorderInner = LinearGradientColorTable.Empty;
		table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(15057765), Color.Empty, 90);
		table.ComboBox.MouseOver.ExpandText = factory.GetColor(0);
		table.ComboBox.DroppedDown.Background = factory.GetColor(16777215);
		table.ComboBox.DroppedDown.Border = factory.GetColor(15057765);
		table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(16771238), factory.GetColor(16771238), 90);
		table.ComboBox.DroppedDown.ExpandBorderInner = LinearGradientColorTable.Empty;
		table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(15057765));
		table.ComboBox.DroppedDown.ExpandText = factory.GetColor(0);
		table.DialogLauncher.Default.DialogLauncher = factory.GetColor(6120816);
		table.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(6120816);
		table.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776177), factory.GetColor(16773057));
		table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16773057), factory.GetColor(16772277));
		table.DialogLauncher.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15057765), Color.Empty);
		table.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(6120816);
		table.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		table.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(14993573), factory.GetColor(15583377));
		table.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(15645794), factory.GetColor(15656075));
		table.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(13942696), factory.GetColor(15724994));
		table.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(12751419), factory.GetColor(12760162));
		table.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(15397110));
		table.SystemButton.Default.LightShade = Color.Empty;
		table.SystemButton.Default.DarkShade = factory.GetColor(5395813);
		table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(15594233));
		table.SystemButton.MouseOver.LightShade = Color.Empty;
		table.SystemButton.MouseOver.DarkShade = factory.GetColor(5395813);
		table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(13292514), factory.GetColor(12437463));
		table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(12437463), factory.GetColor(11450829));
		table.SystemButton.MouseOver.TopHighlight = null;
		table.SystemButton.MouseOver.BottomHighlight = null;
		table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10201015));
		table.SystemButton.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(15331318));
		table.SystemButton.Pressed.LightShade = Color.Empty;
		table.SystemButton.Pressed.DarkShade = factory.GetColor(5395813);
		table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(11450829));
		table.SystemButton.Pressed.TopHighlight = null;
		table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(11450829));
		table.SystemButton.Pressed.BottomHighlight = null;
		table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(9414079));
		table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(11387354));
		table.SystemButtonClose = new Office2007SystemButtonColorTable();
		table.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(factory.GetColor(15397110));
		table.SystemButtonClose.Default.LightShade = Color.Empty;
		table.SystemButtonClose.Default.DarkShade = factory.GetColor(5395813);
		table.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(16570327));
		table.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		table.SystemButtonClose.MouseOver.DarkShade = factory.GetColor(10173757);
		table.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16745602), factory.GetColor(16482174));
		table.SystemButtonClose.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16152953), factory.GetColor(14901602));
		table.SystemButtonClose.MouseOver.TopHighlight = null;
		table.SystemButtonClose.MouseOver.BottomHighlight = null;
		table.SystemButtonClose.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10173757));
		table.SystemButtonClose.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		table.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(16307413));
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
			factory.GetColor(9476774),
			factory.GetColor(13952757),
			factory.GetColor(12308198),
			factory.GetColor(12308198),
			factory.GetColor(12308198)
		};
		table.Form.Inactive.BorderColors = new Color[5]
		{
			factory.GetColor(10661305),
			factory.GetColor(12308198),
			factory.GetColor(12308198),
			factory.GetColor(12308198),
			factory.GetColor(12308198)
		};
		table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(12371928), factory.GetColor(12371928));
		table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(12371928), factory.GetColor(12371928));
		table.Form.Active.CaptionBottomBorder = new Color[2]
		{
			factory.GetColor(10267329),
			factory.GetColor(13226721)
		};
		table.Form.Active.CaptionText = factory.GetColor(1780030);
		table.Form.Active.CaptionTextExtra = factory.GetColor(1780030);
		table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(13226721));
		table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(13226721));
		table.Form.Inactive.CaptionText = factory.GetColor(5654106);
		table.Form.Inactive.CaptionTextExtra = factory.GetColor(5654106);
		table.Form.BackColor = factory.GetColor(13686754);
		table.Form.TextColor = factory.GetColor(1780030);
		table.Form.MdiClientBackgroundImage = (Image)(object)BarFunctions.LoadBitmap("SystemImages.VS2010ClientBackground.png");
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
		table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(12371928));
		table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(10267329);
		table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(13292514);
		table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(1780030);
		table.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		table.TabControl.Default = new Office2007TabItemStateColorTable();
		table.TabControl.Default.TopBackground = LinearGradientColorTable.Empty;
		table.TabControl.Default.BottomBackground = LinearGradientColorTable.Empty;
		table.TabControl.Default.InnerBorder = Color.Empty;
		table.TabControl.Default.OuterBorder = Color.Empty;
		table.TabControl.Default.Text = factory.GetColor(16777215);
		table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(7305078), factory.GetColor(6384757));
		table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(6121845), factory.GetColor(5201780));
		table.TabControl.MouseOver.InnerBorder = Color.Empty;
		table.TabControl.MouseOver.OuterBorder = factory.GetColor(10201015);
		table.TabControl.MouseOver.Text = factory.GetColor(16777215);
		table.TabControl.Selected = new Office2007TabItemStateColorTable();
		table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(16776434), factory.GetColor(16774095));
		table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(16771238), factory.GetColor(16771238));
		table.TabControl.Selected.InnerBorder = Color.Empty;
		table.TabControl.Selected.OuterBorder = Color.Empty;
		table.TabControl.Selected.Text = factory.GetColor(0);
		table.TabControl.TabBackground = LinearGradientColorTable.Empty;
		table.TabControl.TabBackgroundImage = (Image)(object)BarFunctions.LoadBitmap("SystemImages.VS2010ClientBackground.png");
		table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.TabControl.TabPanelBorder = factory.GetColor(color);
		Office2007CheckBoxColorTable checkBoxItem = table.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = factory.GetColor(11256286);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(15264235), factory.GetColor(15921907));
		checkBoxItem.Default.CheckInnerBorder = factory.GetColor(12502220);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.Default.Text = factory.GetColor(1780030);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(16576962), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = factory.GetColor(13602871);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(16444616), factory.GetColor(16643812));
		checkBoxItem.MouseOver.CheckInnerBorder = factory.GetColor(16559453);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.MouseOver.Text = factory.GetColor(1780030);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(16574397));
		checkBoxItem.Pressed.CheckBorder = factory.GetColor(12741917);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(16441506), factory.GetColor(16642775));
		checkBoxItem.Pressed.CheckInnerBorder = factory.GetColor(16557400);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.Pressed.Text = factory.GetColor(1780030);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = factory.GetColor(11974842);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(16316665));
		checkBoxItem.Disabled.CheckInnerBorder = factory.GetColor(14672096);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(11645876), Color.Empty);
		checkBoxItem.Disabled.Text = factory.GetColor(8884895);
		InitializeScrollBarColorTable(table, factory);
		InitializeAppBlueScrollBarColorTable(table, factory);
		Office2007ProgressBarColorTable progressBarItem = table.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = factory.GetColor(7247820);
		progressBarItem.InnerBorder = factory.GetColor(16777215);
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
		progressBarItem = table.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = factory.GetColor(7247820);
		progressBarItem.InnerBorder = factory.GetColor(16777215);
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
		progressBarItem = table.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = factory.GetColor(7247820);
		progressBarItem.InnerBorder = factory.GetColor(16777215);
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
		Office2007GalleryColorTable gallery = table.Gallery;
		gallery.GroupLabelBackground = factory.GetColor(15790837);
		gallery.GroupLabelText = factory.GetColor(1780030);
		gallery.GroupLabelBorder = factory.GetColor(15790837);
		table.LegacyColors.BarBackground = factory.GetColor(12371928);
		table.LegacyColors.BarBackground2 = Color.Empty;
		table.LegacyColors.BarStripeColor = factory.GetColor(6320780);
		table.LegacyColors.BarCaptionBackground = factory.GetColor(16776434);
		table.LegacyColors.BarCaptionBackground2 = factory.GetColor(16771238);
		table.LegacyColors.BarCaptionText = factory.GetColor(0);
		table.LegacyColors.BarCaptionInactiveBackground = factory.GetColor(5070978);
		table.LegacyColors.BarCaptionInactiveBackground2 = factory.GetColor(4018807);
		table.LegacyColors.BarCaptionInactiveText = factory.GetColor(16777215);
		table.LegacyColors.BarFloatingBorder = factory.GetColor(2833497);
		table.LegacyColors.BarPopupBackground = factory.GetColor(16777215);
		table.LegacyColors.BarPopupBorder = factory.GetColor(10201015);
		table.LegacyColors.ItemBackground = Color.Empty;
		table.LegacyColors.ItemBackground2 = Color.Empty;
		table.LegacyColors.ItemCheckedBackground = factory.GetColor(16773051);
		table.LegacyColors.ItemCheckedBackground2 = factory.GetColor(16773051);
		table.LegacyColors.ItemCheckedBorder = factory.GetColor(15057765);
		table.LegacyColors.ItemCheckedText = factory.GetColor(1780030);
		table.LegacyColors.ItemDisabledBackground = Color.Empty;
		table.LegacyColors.ItemDisabledText = factory.GetColor(9276813);
		table.LegacyColors.ItemExpandedShadow = Color.Empty;
		table.LegacyColors.ItemExpandedBackground = factory.GetColor(15330542);
		table.LegacyColors.ItemExpandedBackground2 = Color.Empty;
		table.LegacyColors.ItemExpandedText = factory.GetColor(1780030);
		table.LegacyColors.ItemExpandedBorder = factory.GetColor(1780030);
		table.LegacyColors.ItemHotBackground = factory.GetColor(16776434);
		table.LegacyColors.ItemHotBackground2 = factory.GetColor(16772277);
		table.LegacyColors.ItemHotBorder = factory.GetColor(10201015);
		table.LegacyColors.ItemHotText = factory.GetColor(1780030);
		table.LegacyColors.ItemPressedBackground = factory.GetColor(16776434);
		table.LegacyColors.ItemPressedBackground2 = factory.GetColor(16772277);
		table.LegacyColors.ItemPressedBorder = factory.GetColor(15057765);
		table.LegacyColors.ItemPressedText = factory.GetColor(1780030);
		table.LegacyColors.ItemSeparator = factory.GetColor(12501963);
		table.LegacyColors.ItemSeparatorShade = Color.Empty;
		table.LegacyColors.ItemText = factory.GetColor(1780030);
		table.LegacyColors.MenuBackground = factory.GetColor(15330542);
		table.LegacyColors.MenuBackground2 = factory.GetColor(13686754);
		table.LegacyColors.MenuBarBackground = factory.GetColor(13292514);
		table.LegacyColors.MenuBarBackground2 = factory.GetColor(11450829);
		table.LegacyColors.MenuBorder = factory.GetColor(10201015);
		table.LegacyColors.ItemExpandedBorder = table.LegacyColors.MenuBorder;
		table.LegacyColors.MenuSide = factory.GetColor(15330542);
		table.LegacyColors.MenuSide2 = Color.Empty;
		table.LegacyColors.MenuUnusedBackground = table.LegacyColors.MenuBackground;
		table.LegacyColors.MenuUnusedSide = factory.GetColor(12371928);
		table.LegacyColors.MenuUnusedSide2 = Color.Empty;
		table.LegacyColors.ItemDesignTimeBorder = Color.DarkGray;
		table.LegacyColors.BarDockedBorder = factory.GetColor(10267329);
		table.LegacyColors.DockSiteBackColor = factory.GetColor(10267329);
		table.LegacyColors.DockSiteBackColor2 = Color.Empty;
		table.LegacyColors.CustomizeBackground = factory.GetColor(14015720);
		table.LegacyColors.CustomizeBackground2 = Color.Empty;
		table.LegacyColors.CustomizeText = factory.GetColor(1780030);
		table.LegacyColors.PanelBackground = factory.GetColor(13292514);
		table.LegacyColors.PanelBackground2 = factory.GetColor(11450829);
		table.LegacyColors.PanelText = factory.GetColor(1780030);
		table.LegacyColors.PanelBorder = factory.GetColor(color);
		table.LegacyColors.ExplorerBarBackground = factory.GetColor(5070978);
		table.LegacyColors.ExplorerBarBackground2 = factory.GetColor(4018807);
		table.LegacyColors.AutoHidePanelBackgroundImage = table.Form.MdiClientBackgroundImage;
		table.LegacyColors.AutoHidePanelBackground = Color.Transparent;
		table.LegacyColors.AutoHideTabBackground = factory.GetColor(4939393);
		table.LegacyColors.AutoHideTabText = factory.GetColor(16777215);
		table.LegacyColors.AutoHideTabBorder = Color.Transparent;
		table.LegacyColors.AutoHideSelectedTabBackground = factory.GetColor(5004404);
		table.LegacyColors.AutoHideSelectedTabBackground2 = factory.GetColor(7305078);
		table.LegacyColors.AutoHideSelectedTabText = factory.GetColor(16777215);
		table.LegacyColors.AutoHideSelectedTabBorder = factory.GetColor(10201015);
		table.LegacyColors.SplitterBackground = factory.GetColor(14412799);
		table.LegacyColors.SplitterBackground2 = factory.GetColor(11846870);
		table.LegacyColors.SplitterBorder = factory.GetColor(8756927);
		table.LegacyColors.SplitterText = table.LegacyColors.SplitterBorder;
		table.NavigationPane.ButtonBackground = new GradientColorTable();
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12371928), 0f));
		table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(16645630), factory.GetColor(15001072));
		table.SuperTooltip.TextColor = factory.GetColor(5000268);
		Office2007SliderColorTable slider = table.Slider;
		slider.Default.LabelColor = factory.GetColor(1780030);
		slider.Default.SliderLabelColor = factory.GetColor(1780030);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16251902), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14937336), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12372701), 1f));
		slider.Default.PartBorderColor = factory.GetColor(7767709);
		slider.Default.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.Default.PartForeColor = factory.GetColor(8290951);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = factory.GetColor(9084088);
		slider.Default.TrackLineLightColor = factory.GetColor(14609143);
		slider.MouseOver.LabelColor = factory.GetColor(1780030);
		slider.MouseOver.SliderLabelColor = factory.GetColor(1780030);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16180380), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16180634), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16446933), 1f));
		slider.MouseOver.PartBorderColor = factory.GetColor(15581758);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(16777215));
		slider.MouseOver.PartForeColor = factory.GetColor(8290951);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = factory.GetColor(9084088);
		slider.MouseOver.TrackLineLightColor = factory.GetColor(14609143);
		slider.Pressed.LabelColor = factory.GetColor(1780030);
		slider.Pressed.SliderLabelColor = factory.GetColor(1780030);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16504439), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770185), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770185), 1f));
		slider.Pressed.PartBorderColor = factory.GetColor(12746038);
		slider.Pressed.PartBorderLightColor = factory.GetColor(15843679);
		slider.Pressed.PartForeColor = factory.GetColor(8290951);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = factory.GetColor(9084088);
		slider.Pressed.TrackLineLightColor = factory.GetColor(14609143);
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
		table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(15726075), factory.GetColor(14806266));
		table.ListViewEx.ColumnSeparator = factory.GetColor(color);
		table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(10997232), Color.Empty);
		table.ListViewEx.SelectionBorder = factory.GetColor(14938111);
		table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(color);
		table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(15726075), factory.GetColor(14806266), 90);
		table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(16374175), factory.GetColor(15843679), 90);
		table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(16766349), factory.GetColor(15897146), 90);
		table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(13952247), factory.GetColor(12834540), 90);
		table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(color);
		table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(11914211), factory.GetColor(10861783), 90);
		table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(color);
		table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(15002871));
		table.DataGridView.RowNormalBorder = factory.GetColor(color);
		table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(16766349));
		table.DataGridView.RowSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(11914211), factory.GetColor(10861783), 90);
		table.DataGridView.RowPressedBorder = factory.GetColor(color);
		table.DataGridView.GridColor = factory.GetColor(11184810);
		table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(10861783));
		table.DataGridView.SelectorBorder = factory.GetColor(color);
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
		table.SideBar.SideBarPanelItemText = factory.GetColor(1780030);
		table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(15595005), 0f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14214386), 1f));
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
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1780030);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1780030);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor("F9E8A1"), 0f),
			new BackgroundColorBlend(factory.GetColor("FAF2CD"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor("EEC955");
		crumbBarItemViewStateColorTable.BorderLight = factory.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1780030);
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
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(1780030);
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
		table.WarningBox.BackColor = factory.GetColor(factory.GetColor(13492213));
		table.WarningBox.WarningBorderColor = factory.GetColor(color);
		table.WarningBox.WarningBackColor1 = factory.GetColor(13096681);
		table.WarningBox.WarningBackColor2 = factory.GetColor(11518940);
		table.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(factory.GetColor(color)),
			new ColorDef(factory.GetColor(1780030)),
			new ColorDef(factory.GetColor(10862561)),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(10862561)),
			new ColorDef(factory.GetColor(15134199)),
			new ColorDef(factory.GetColor(10862561)),
			new ColorDef(factory.GetColor(14017009)),
			new ColorDef(factory.GetColor(2706554)),
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
			new ColorDef(factory.GetColor(1780030)),
			new ColorDef(factory.GetColor(color)),
			new ColorDef(factory.GetColor(color)),
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
			new ColorDef(factory.GetColor(color)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(1780030)),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(15134199)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(color)),
			new ColorDef(factory.GetColor(10862561)),
			new ColorDef(factory.GetColor(2046329)),
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(factory.GetColor(15134199)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(10862561)),
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
			new ColorDef(factory.GetColor(2046329)),
			new ColorDef(factory.GetColor(16777215))
		};
		table.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(factory.GetColor(9285337)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(14739958),
				factory.GetColor(11782630)
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
		table.SuperTab.Background = new SuperTabLinearGradientColorTable(factory.GetColor(12308198));
		table.SuperTab.InnerBorder = factory.GetColor(Color.Transparent);
		table.SuperTab.OuterBorder = factory.GetColor(10465991);
		table.SuperTab.ControlBoxDefault.Image = factory.GetColor(1780030);
		table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(255, 1780030);
		table.SuperTab.ControlBoxPressed.Background = factory.GetColor(12045024);
		table.SuperTab.ControlBoxPressed.Border = factory.GetColor(3381759);
		table.SuperTab.ControlBoxPressed.Image = factory.GetColor(255, 1780030);
		table.SuperTab.InsertMarker = factory.GetColor(255, 0);
		table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		table.SuperTabItem.Default.Normal.Text = factory.GetColor(1780030);
		table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(1780030);
		table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(16120575),
			factory.GetColor(15726333)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.Selected.InnerBorder = Color.Transparent;
		table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(10465991);
		table.SuperTabItem.Default.Selected.Text = factory.GetColor(1780030);
		table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(1780030);
		table.SuperTabItem.Default.Disabled.Text = factory.GetColor(8884895);
		table.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		table.SuperTabItem.Default.Disabled.CloseMarker = factory.GetColor(8884895);
		table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(16120575),
			factory.GetColor(15726333)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = Color.Transparent;
		table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(10465991);
		table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(1780030);
		table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(1780030);
		table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(48, 16777215),
			factory.GetColor(180, 16777215)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(190, 16777215);
		table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(11646394);
		table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(1780030);
		table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(1780030);
		table.SuperTabItem.Left = table.SuperTabItem.Default;
		table.SuperTabItem.Bottom = table.SuperTabItem.Default;
		table.SuperTabItem.Right = table.SuperTabItem.Right;
		table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15726333));
		table.SuperTabPanel.Default.OuterBorder = factory.GetColor(10465991);
		table.SuperTabPanel.Default.InnerBorder = factory.GetColor(Color.Transparent);
		table.SuperTabPanel.Left = table.SuperTabPanel.Default;
		table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
		table.SuperTabPanel.Right = table.SuperTabPanel.Default;
		SuperTabStyleColorFactory.GetVS2010BackstageBlueColorTable(table.Backstage, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageBlueItemColorTable(table.BackstageItem, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageBluePanelColorTable(table.BackstagePanel, factory);
		table.SwitchButton = new SwitchButtonColors();
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
		elementStyle.BorderColor = factory.GetColor(8950952);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = factory.GetColor(15856632);
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
		elementStyle = GetRibbonClientPanelStyle(factory, eOffice2010ColorScheme.Blue);
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
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(10267329));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable();
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(13292514), factory.GetColor(11450829));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(3690099);
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
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(14871799), factory.GetColor(9676739));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(40, 16777215), factory.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(15726333), factory.GetColor(15200762));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(15200762), factory.GetColor(14214386));
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrange(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(1780030);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15057765));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776434), factory.GetColor(16774095));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16772277), factory.GetColor(16772277));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(15057765), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(1780030);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(15648599));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(15057765));
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(16771238));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16771238));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(15057765), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(1780030);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(15057765));
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(16773051));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(16773051));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(1780030);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(15057765));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(16771238));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(16771238));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(1780030);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrangeWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueOrange = GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(13292514));
		buttonItemBlueOrange.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(11450829));
		buttonItemBlueOrange.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(8950952));
		buttonItemBlueOrange.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(12371928));
		buttonItemBlueOrange.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(12371928));
		buttonItemBlueOrange.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(10267329), Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.Text = factory.GetColor(10987431);
		return buttonItemBlueOrange;
	}

	public static Office2007ButtonItemColorTable GetButtonItemOffice2007WithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueOrange = GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Default.Text = factory.GetColor(2500134);
		buttonItemBlueOrange.Checked.Text = buttonItemBlueOrange.Default.Text;
		buttonItemBlueOrange.Expanded.Text = buttonItemBlueOrange.Default.Text;
		buttonItemBlueOrange.MouseOver.Text = buttonItemBlueOrange.Default.Text;
		buttonItemBlueOrange.MouseOverSplitInactive.Text = buttonItemBlueOrange.Default.Text;
		buttonItemBlueOrange.Pressed.Text = buttonItemBlueOrange.Default.Text;
		buttonItemBlueOrange.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(16579837));
		buttonItemBlueOrange.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(16579837), factory.GetColor(15593457));
		buttonItemBlueOrange.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(14606304), factory.GetColor(10592932));
		buttonItemBlueOrange.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(15595005));
		buttonItemBlueOrange.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(15595005));
		buttonItemBlueOrange.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(13557229), Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.Text = factory.GetColor(10987431);
		return buttonItemBlueOrange;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueBlueWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueBlue = Office2007ColorTableFactory.GetButtonItemBlueBlue(factory);
		buttonItemBlueBlue.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(4095451), factory.GetColor(2843573));
		buttonItemBlueBlue.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(2711731), factory.GetColor(2977999));
		buttonItemBlueBlue.Default.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(4557022), Color.Transparent);
		buttonItemBlueBlue.Default.InnerBorder = new LinearGradientColorTable(factory.GetColor(4425700), Color.Empty);
		buttonItemBlueBlue.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(2050209), factory.GetColor(2379686));
		buttonItemBlueBlue.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Default.Text = factory.GetColor(16777215);
		buttonItemBlueBlue.MouseOverSplitInactive = buttonItemBlueBlue.Default;
		buttonItemBlueBlue.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueBlue.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(15595005));
		buttonItemBlueBlue.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(15595005));
		buttonItemBlueBlue.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(13557229), Color.Empty);
		buttonItemBlueBlue.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.Text = factory.GetColor(10987431);
		return buttonItemBlueBlue;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueMagentaWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueMagenta = Office2007ColorTableFactory.GetButtonItemBlueMagenta(factory);
		buttonItemBlueMagenta.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(11865171), factory.GetColor(11470669));
		buttonItemBlueMagenta.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(11470669), factory.GetColor(15221887));
		buttonItemBlueMagenta.Default.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14958972), Color.Transparent);
		buttonItemBlueMagenta.Default.InnerBorder = new LinearGradientColorTable(factory.GetColor(12523614), factory.GetColor(15222403));
		buttonItemBlueMagenta.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9306682), factory.GetColor(9569085));
		buttonItemBlueMagenta.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Default.Text = factory.GetColor(16777215);
		buttonItemBlueMagenta.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueMagenta.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(15595005));
		buttonItemBlueMagenta.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(15595005));
		buttonItemBlueMagenta.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(13557229), Color.Empty);
		buttonItemBlueMagenta.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.Text = factory.GetColor(10987431);
		return buttonItemBlueMagenta;
	}

	public static void SetBlueExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
	{
		Color color = factory.GetColor(1780030);
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
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(6456);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(8756927));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16252927));
		office2007ButtonItemColorTable.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(15069691), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(15069692), factory.GetColor(12770289));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(6456);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(7704235));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(9085890));
		office2007ButtonItemColorTable.Pressed.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(10467286), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(factory.GetColor(10467286));
		office2007ButtonItemColorTable.Pressed.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(6456);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(8756927));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(14543351));
		office2007ButtonItemColorTable.Checked.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14478326), Color.Transparent);
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(factory.GetColor(13163503), factory.GetColor(11848677));
		office2007ButtonItemColorTable.Checked.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(6456);
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Checked;
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueDefault(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(8950952));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(8950952));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(48, 16777215), factory.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(190, 16777215), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(8950952), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueMagenta(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(15043261), factory.GetColor(14575270));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15043261), factory.GetColor(14575270));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(48, 14891409), factory.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(190, 16439536), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14891409), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(6602591), factory.GetColor(5489230));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(6602591), factory.GetColor(5489230));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(48, 2535459), factory.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(190, 13170865), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(2535459), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(15902565), factory.GetColor(15900672));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15902565), factory.GetColor(15900672));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(16110224), factory.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(190, 16508527), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15760646), factory.GetColor(15695368));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupDefault(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(4491493), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(2050209), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupMagenta(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(12933008), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(13172855), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupGreen(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(192, 2535459), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(2200605), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupOrange(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(192, 16751872), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(14054681), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
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

	public static void InitializeAppBlueScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(11124437), factory.GetColor(12308196), 0);
		@default.Border = new LinearGradientColorTable(factory.GetColor(7507629), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(12177126), 0f),
			new BackgroundColorBlend(factory.GetColor(9546697), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(9408399)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.MouseOver;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(factory.GetColor(12703728), 0f),
			new BackgroundColorBlend(factory.GetColor(11849191), 0.5f),
			new BackgroundColorBlend(factory.GetColor(10731227), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(6059926));
		@default.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(factory.GetColor(12703728), 0f),
			new BackgroundColorBlend(factory.GetColor(11849191), 0.5f),
			new BackgroundColorBlend(factory.GetColor(10731227), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(9408399));
		@default = t.AppScrollBar.MouseOverControl;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(factory.GetColor(12177126), 0f),
			new BackgroundColorBlend(factory.GetColor(11125211), 0.5f),
			new BackgroundColorBlend(factory.GetColor(9546697), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(6059926));
		@default.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(factory.GetColor(12177126), 0f),
			new BackgroundColorBlend(factory.GetColor(11125211), 0.5f),
			new BackgroundColorBlend(factory.GetColor(9546697), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(9408399));
		@default = t.AppScrollBar.Pressed;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(11585502), 0f),
			new BackgroundColorBlend(factory.GetColor(9020609), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(6059926));
		@default.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(11585502), 0f),
			new BackgroundColorBlend(factory.GetColor(9020609), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(6059926), Color.Empty);
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

	public static void InitializeScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(16053492), factory.GetColor(16579836), 0);
		@default.Border = new LinearGradientColorTable(factory.GetColor(15133938));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7632246), factory.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(15791352), 0f),
			new BackgroundColorBlend(factory.GetColor(15396595), 0.8f),
			new BackgroundColorBlend(factory.GetColor(14738665), 0.8f),
			new BackgroundColorBlend(factory.GetColor(14146529), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(8224125));
		@default = t.ScrollBar.MouseOver;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16580094), 0f),
			new BackgroundColorBlend(factory.GetColor(15988473), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15396337), 0.8f),
			new BackgroundColorBlend(factory.GetColor(15001836), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(16580094), factory.GetColor(15856629));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7632246), factory.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(8224125));
		@default = t.ScrollBar.MouseOverControl;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(t.ScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = t.ScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = t.ScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7632246), factory.GetColor(4539975));
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(t.ScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = t.ScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = t.ScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = t.ScrollBar.Default.TrackSignBackground;
		@default = t.ScrollBar.Pressed;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(15462387), 0f),
			new BackgroundColorBlend(factory.GetColor(15067373), 0.8f),
			new BackgroundColorBlend(factory.GetColor(14343907), 0.8f),
			new BackgroundColorBlend(factory.GetColor(13685978), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = t.ScrollBar.Default.TrackSignBackground;
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

	public static ElementStyle GetRibbonClientPanelStyle(ColorFactory f, eOffice2010ColorScheme cs)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonClientPanelKey;
		elementStyle.BackColorGradientAngle = 90;
		switch (cs)
		{
		case eOffice2010ColorScheme.Blue:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(13097712), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(10138575), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(8757696), 1f));
			break;
		case eOffice2010ColorScheme.Silver:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(13488598), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(12633288), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(12238530), 1f));
			break;
		case eOffice2010ColorScheme.Black:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(7303023), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(5197647), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(4144959), 1f));
			break;
		}
		return elementStyle;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBackstageDefault(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(5406176));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(135, 16777215));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(32, 32767));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(32, 32767));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(5406176), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(192, 16777215));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(32, 32767));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(32, 32767));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(5406176), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}
}
