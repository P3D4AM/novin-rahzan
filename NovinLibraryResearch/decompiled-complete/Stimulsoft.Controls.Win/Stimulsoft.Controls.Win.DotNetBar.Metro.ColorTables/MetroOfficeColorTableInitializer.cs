using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Schedule;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

internal static class MetroOfficeColorTableInitializer
{
	private static Office2007ButtonItemColorTable FallBackAppButtonColorTable;

	public static void InitializeColorTable(Office2007ColorTable table, ColorFactory factory, MetroPartColors metroColors)
	{
		table.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010NormalSilver.png");
		table.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010HotSilver.png");
		table.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankOffice2010PressedSilver.png");
		table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade));
		table.RibbonControl.InnerBorder = null;
		table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, factory.GetColor(metroColors.CanvasColor));
		table.RibbonControl.TabDividerBorder = Color.Empty;
		table.RibbonControl.TabDividerBorderLight = Color.Empty;
		table.RibbonControl.CornerSize = 1;
		table.RibbonControl.PanelTopBackgroundHeight = 0f;
		table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.RibbonControl.PanelBottomBackground = null;
		table.KeyTips.KeyTipBackground = factory.GetColor(metroColors.TextDisabledColor);
		table.KeyTips.KeyTipText = factory.GetColor(metroColors.CanvasColor);
		table.KeyTips.KeyTipBorder = factory.GetColor(metroColors.TextDisabledColor);
		table.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		table.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		table.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		table.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		table.ItemGroup.ItemGroupDividerDark = Color.Empty;
		table.ItemGroup.ItemGroupDividerLight = Color.Empty;
		table.RibbonBar.Default = GetRibbonBar(factory, metroColors);
		table.RibbonBar.MouseOver = GetRibbonBarMouseOver(factory, metroColors);
		table.RibbonBar.Expanded = GetRibbonBarExpanded(factory, metroColors);
		table.RibbonButtonItemColors.Clear();
		table.ButtonItemColors.Clear();
		table.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable buttonItemDefault = GetButtonItemDefault(factory, metroColors);
		buttonItemDefault.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		table.ButtonItemColors.Add(buttonItemDefault);
		buttonItemDefault = GetButtonItemDefaultWithBackground(factory, metroColors);
		buttonItemDefault.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		table.ButtonItemColors.Add(buttonItemDefault);
		buttonItemDefault = GetButtonItemBlue(factory, metroColors);
		buttonItemDefault.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemDefault.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemDefault.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		table.ButtonItemColors.Add(buttonItemDefault);
		buttonItemDefault = GetButtonItemBlueWithBackground(factory, metroColors);
		buttonItemDefault.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemDefault.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemDefault.MouseOverSplitInactive = buttonItemDefault.Default;
		buttonItemDefault.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		table.ButtonItemColors.Add(buttonItemDefault);
		buttonItemDefault = GetButtonItemMagenta(factory, metroColors);
		buttonItemDefault.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemDefault.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemDefault.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		table.ButtonItemColors.Add(buttonItemDefault);
		buttonItemDefault = GetButtonItemMagentaWithBackground(factory, metroColors);
		buttonItemDefault.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemDefault.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemDefault.MouseOverSplitInactive = buttonItemDefault.Default;
		buttonItemDefault.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.ButtonItemColors.Add(buttonItemDefault);
		buttonItemDefault = Office2010BlueFactory.GetButtonItemOffice2007WithBackground(factory);
		buttonItemDefault.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemDefault.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11647684));
		buttonItemDefault.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		table.ButtonItemColors.Add(buttonItemDefault);
		table.ButtonItemColors.Add(CreateBlueOrbColorTable(factory, metroColors));
		table.BackstageButtonItemColors.Clear();
		table.BackstageButtonItemColors.Add(GetButtonItemBackstageDefault(factory, metroColors));
		table.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "StatusBar"), GetButtonItemStatusBar(factory, metroColors));
		table.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable ribbonTabItemBlueDefault = GetRibbonTabItemBlueDefault(factory, metroColors);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		table.RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = GetRibbonTabItemBlueDefault(factory, metroColors);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		table.RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = GetRibbonTabItemBlueDefault(factory, metroColors);
		ribbonTabItemBlueDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		table.RibbonTabItemColors.Add(ribbonTabItemBlueDefault);
		ribbonTabItemBlueDefault = GetRibbonTabItemBlueDefault(factory, metroColors);
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
		table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.Bar.ToolbarBottomBackground = null;
		table.Bar.ToolbarBottomBorder = Color.Empty;
		table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		table.Bar.PopupToolbarBorder = factory.GetColor(metroColors.BaseColor);
		table.Bar.StatusBarTopBorder = factory.GetColor(metroColors.BaseColor);
		table.Bar.StatusBarTopBorderLight = factory.GetColor(metroColors.BaseColor);
		table.Bar.StatusBarAltBackground.Clear();
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 0f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 1f));
		table.Menu.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		table.Menu.Border = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorDarkShade), Color.Empty);
		table.Menu.Side = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		table.Menu.SideBorderLight = LinearGradientColorTable.Empty;
		table.Menu.SideUnused = LinearGradientColorTable.Empty;
		table.Menu.FileBackgroundBlend.Clear();
		table.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(metroColors.CanvasColorLighterShade), 0f),
			new BackgroundColorBlend(factory.GetColor(metroColors.CanvasColorLighterShade), 1f)
		});
		table.Menu.FileContainerBorder = factory.GetColor(metroColors.CanvasColorLightShade);
		table.Menu.FileContainerBorderLight = Color.Transparent;
		table.Menu.FileColumnOneBackground = factory.GetColor(metroColors.CanvasColor);
		table.Menu.FileColumnOneBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.Menu.FileColumnTwoBackground = factory.GetColor(metroColors.CanvasColor);
		table.Menu.FileBottomContainerBackgroundBlend.Clear();
		table.ComboBox.Default.Background = factory.GetColor(metroColors.CanvasColor);
		table.ComboBox.Default.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandText = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.ComboBox.DefaultStandalone.Background = factory.GetColor(metroColors.CanvasColor);
		table.ComboBox.DefaultStandalone.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.ComboBox.DefaultStandalone.ExpandBackground = LinearGradientColorTable.Empty;
		table.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		table.ComboBox.DefaultStandalone.ExpandBorderOuter = LinearGradientColorTable.Empty;
		table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(metroColors.TextColor);
		table.ComboBox.MouseOver.Background = factory.GetColor(metroColors.CanvasColor);
		table.ComboBox.MouseOver.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseButtonGradientStart), factory.GetColor(metroColors.BaseButtonGradientEnd), 90);
		table.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDarker));
		table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor), Color.Empty, 90);
		table.ComboBox.MouseOver.ExpandText = factory.GetColor(metroColors.TextColor);
		table.ComboBox.DroppedDown.Background = factory.GetColor(metroColors.CanvasColor);
		table.ComboBox.DroppedDown.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight), factory.GetColor(metroColors.BaseColorDark), 90);
		table.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDarker));
		table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor), Color.Empty, 90);
		table.ComboBox.DroppedDown.ExpandText = factory.GetColor(metroColors.TextColor);
		table.DialogLauncher.Default.DialogLauncher = factory.GetColor(metroColors.TextInactiveColor);
		table.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(metroColors.BaseColor);
		table.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		table.DialogLauncher.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest), Color.Empty);
		table.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(metroColors.BaseColor);
		table.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		table.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		table.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		table.DialogLauncher.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		table.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight), Color.Empty);
		table.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(metroColors.TextInactiveColor));
		table.SystemButton.Default.LightShade = Color.Empty;
		table.SystemButton.Default.DarkShade = Color.Empty;
		table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.SystemButton.MouseOver.LightShade = Color.Empty;
		table.SystemButton.MouseOver.DarkShade = Color.Empty;
		table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		table.SystemButton.MouseOver.BottomBackground = null;
		table.SystemButton.MouseOver.TopHighlight = null;
		table.SystemButton.MouseOver.BottomHighlight = null;
		table.SystemButton.MouseOver.OuterBorder = null;
		table.SystemButton.MouseOver.InnerBorder = null;
		table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(metroColors.TextColor));
		table.SystemButton.Pressed.LightShade = Color.Empty;
		table.SystemButton.Pressed.DarkShade = Color.Empty;
		table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		table.SystemButton.Pressed.TopHighlight = null;
		table.SystemButton.Pressed.BottomBackground = null;
		table.SystemButton.Pressed.BottomHighlight = null;
		table.SystemButton.Pressed.OuterBorder = null;
		table.SystemButton.Pressed.InnerBorder = null;
		table.SystemButtonClose = new Office2007SystemButtonColorTable();
		table.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(factory.GetColor(metroColors.TextInactiveColor));
		table.SystemButtonClose.Default.LightShade = Color.Empty;
		table.SystemButtonClose.Default.DarkShade = Color.Empty;
		table.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		table.SystemButtonClose.MouseOver.DarkShade = Color.Empty;
		table.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		table.SystemButtonClose.MouseOver.BottomBackground = null;
		table.SystemButtonClose.MouseOver.TopHighlight = null;
		table.SystemButtonClose.MouseOver.BottomHighlight = null;
		table.SystemButtonClose.MouseOver.OuterBorder = null;
		table.SystemButtonClose.MouseOver.InnerBorder = null;
		table.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(metroColors.TextColor));
		table.SystemButtonClose.Pressed.LightShade = Color.Empty;
		table.SystemButtonClose.Pressed.DarkShade = Color.Empty;
		table.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		table.SystemButtonClose.Pressed.TopHighlight = null;
		table.SystemButtonClose.Pressed.BottomBackground = null;
		table.SystemButtonClose.Pressed.BottomHighlight = null;
		table.SystemButtonClose.Pressed.OuterBorder = null;
		table.SystemButtonClose.Pressed.InnerBorder = null;
		table.Form.Active.BorderColors = new Color[5]
		{
			factory.GetColor(metroColors.BaseColor),
			factory.GetColor(metroColors.CanvasColor),
			factory.GetColor(metroColors.CanvasColor),
			factory.GetColor(metroColors.CanvasColor),
			factory.GetColor(metroColors.CanvasColor)
		};
		table.Form.Inactive.BorderColors = new Color[5]
		{
			factory.GetColor(metroColors.BaseColor),
			factory.GetColor(metroColors.CanvasColor),
			factory.GetColor(metroColors.CanvasColor),
			factory.GetColor(metroColors.CanvasColor),
			factory.GetColor(metroColors.CanvasColor)
		};
		table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.Form.Active.CaptionBottomBorder = null;
		table.Form.Active.CaptionText = factory.GetColor(metroColors.TextLightColor);
		table.Form.Active.CaptionTextExtra = factory.GetColor(metroColors.TextInactiveColor);
		table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.Form.Inactive.CaptionText = factory.GetColor(metroColors.CanvasColorLightShade);
		table.Form.Inactive.CaptionTextExtra = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.Form.BackColor = factory.GetColor(metroColors.CanvasColor);
		table.Form.TextColor = factory.GetColor(metroColors.TextColor);
		table.Form.MdiClientBackgroundImage = null;
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
		table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(metroColors.CanvasColorLightShade);
		table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		table.TabControl.Default = new Office2007TabItemStateColorTable();
		table.TabControl.Default.TopBackground = LinearGradientColorTable.Empty;
		table.TabControl.Default.BottomBackground = LinearGradientColorTable.Empty;
		table.TabControl.Default.InnerBorder = Color.Empty;
		table.TabControl.Default.OuterBorder = Color.Empty;
		table.TabControl.Default.Text = factory.GetColor(metroColors.TextInactiveColor);
		table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		table.TabControl.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		table.TabControl.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		table.TabControl.MouseOver.InnerBorder = Color.Empty;
		table.TabControl.MouseOver.OuterBorder = Color.Empty;
		table.TabControl.MouseOver.Text = metroColors.BaseColor;
		table.TabControl.Selected = new Office2007TabItemStateColorTable();
		table.TabControl.Selected.TopBackground = LinearGradientColorTable.Empty;
		table.TabControl.Selected.BottomBackground = LinearGradientColorTable.Empty;
		table.TabControl.Selected.InnerBorder = Color.Empty;
		table.TabControl.Selected.OuterBorder = metroColors.CanvasColorLightShade;
		table.TabControl.Selected.Text = metroColors.BaseColor;
		table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		table.TabControl.TabPanelBorder = metroColors.CanvasColorLightShade;
		table.CheckBoxItem = GetCheckBoxItem(factory, metroColors);
		Office2007CheckBoxColorTable checkBoxItem = GetCheckBoxItem(factory, metroColors);
		checkBoxItem.Default.Text = factory.GetColor(metroColors.BaseTextColor);
		checkBoxItem.MouseOver.Text = factory.GetColor(metroColors.BaseTextColor);
		checkBoxItem.Pressed.Text = factory.GetColor(metroColors.BaseTextColor);
		table.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(Bar)) + "+StatusBar", checkBoxItem);
		table.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(MetroStatusBar)), checkBoxItem);
		InitializeScrollBarColorTable(table, factory, metroColors);
		InitializeAppBlueScrollBarColorTable(table, factory, metroColors);
		Office2007ProgressBarColorTable progressBarItem = table.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(metroColors.CanvasColor);
		progressBarItem.OuterBorder = factory.GetColor(metroColors.BaseColorDarker);
		progressBarItem.InnerBorder = Color.Empty;
		progressBarItem.Chunk = new GradientColorTable(metroColors.BaseColor);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkShadow = new GradientColorTable();
		progressBarItem = table.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = factory.GetColor(8817553);
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
		progressBarItem.OuterBorder = factory.GetColor(8817553);
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
		gallery.GroupLabelBackground = factory.GetColor(metroColors.CanvasColorLighterShade);
		gallery.GroupLabelText = factory.GetColor(metroColors.CanvasColorDarkShade);
		gallery.GroupLabelBorder = Color.Empty;
		table.LegacyColors.BarBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.BarBackground2 = Color.Empty;
		table.LegacyColors.BarStripeColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.LegacyColors.BarCaptionBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.BarCaptionBackground2 = Color.Empty;
		table.LegacyColors.BarCaptionInactiveBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		table.LegacyColors.BarCaptionInactiveText = factory.GetColor(metroColors.TextDisabledColor);
		table.LegacyColors.BarCaptionText = factory.GetColor(metroColors.TextInactiveColor);
		table.LegacyColors.BarFloatingBorder = factory.GetColor(metroColors.BaseColor);
		table.LegacyColors.BarPopupBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.BarPopupBorder = factory.GetColor(metroColors.BaseColor);
		table.LegacyColors.ItemBackground = Color.Empty;
		table.LegacyColors.ItemBackground2 = Color.Empty;
		table.LegacyColors.ItemCheckedBackground = Color.Empty;
		table.LegacyColors.ItemCheckedBackground2 = Color.Empty;
		table.LegacyColors.ItemCheckedBorder = factory.GetColor(metroColors.BaseColor);
		table.LegacyColors.ItemCheckedText = factory.GetColor(metroColors.TextColor);
		table.LegacyColors.ItemDisabledBackground = Color.Empty;
		table.LegacyColors.ItemDisabledText = factory.GetColor(metroColors.TextDisabledColor);
		table.LegacyColors.ItemExpandedShadow = Color.Empty;
		table.LegacyColors.ItemExpandedBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.ItemExpandedBackground2 = Color.Empty;
		table.LegacyColors.ItemExpandedText = factory.GetColor(metroColors.TextColor);
		table.LegacyColors.ItemExpandedBorder = Color.Empty;
		table.LegacyColors.ItemHotBackground = factory.GetColor(metroColors.CanvasColorLightShade);
		table.LegacyColors.ItemHotBackground2 = Color.Empty;
		table.LegacyColors.ItemHotBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.LegacyColors.ItemHotText = factory.GetColor(metroColors.TextColor);
		table.LegacyColors.ItemPressedBackground = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.LegacyColors.ItemPressedBackground2 = Color.Empty;
		table.LegacyColors.ItemPressedBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.LegacyColors.ItemPressedText = factory.GetColor(metroColors.TextColor);
		table.LegacyColors.ItemSeparator = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.LegacyColors.ItemSeparatorShade = Color.Empty;
		table.LegacyColors.ItemText = factory.GetColor(metroColors.TextInactiveColor);
		table.LegacyColors.MenuBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.MenuBackground2 = Color.Empty;
		table.LegacyColors.MenuBarBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.MenuBorder = factory.GetColor(metroColors.BaseColor);
		table.LegacyColors.MenuSide = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.MenuSide2 = Color.Empty;
		table.LegacyColors.MenuUnusedBackground = table.LegacyColors.MenuBackground;
		table.LegacyColors.MenuUnusedSide = Color.Empty;
		table.LegacyColors.MenuUnusedSide2 = Color.Empty;
		table.LegacyColors.ItemDesignTimeBorder = Color.Black;
		table.LegacyColors.BarDockedBorder = Color.Empty;
		table.LegacyColors.DockSiteBackColor = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.DockSiteBackColor2 = Color.Empty;
		table.LegacyColors.CustomizeBackground = Color.Empty;
		table.LegacyColors.CustomizeBackground2 = Color.Empty;
		table.LegacyColors.CustomizeText = factory.GetColor(metroColors.TextInactiveColor);
		table.LegacyColors.PanelBackground = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.LegacyColors.PanelBackground2 = Color.Empty;
		table.LegacyColors.PanelText = factory.GetColor(metroColors.TextColor);
		table.LegacyColors.PanelBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.LegacyColors.ExplorerBarBackground = factory.GetColor(metroColors.CanvasColor);
		table.LegacyColors.ExplorerBarBackground2 = Color.Empty;
		table.LegacyColors.SplitterBackground = table.LegacyColors.PanelBackground;
		table.LegacyColors.SplitterBackground2 = table.LegacyColors.PanelBackground2;
		table.LegacyColors.SplitterText = table.LegacyColors.PanelText;
		table.LegacyColors.SplitterBorder = table.LegacyColors.PanelBorder;
		table.NavigationPane.ButtonBackground = new GradientColorTable();
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.CanvasColor), 0f));
		table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade), factory.GetColor(metroColors.CanvasColor));
		table.SuperTooltip.TextColor = factory.GetColor(metroColors.TextColor);
		Office2007SliderColorTable slider = table.Slider;
		slider.Default.LabelColor = factory.GetColor(metroColors.TextColor);
		slider.Default.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.Default.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = factory.GetColor(metroColors.TextColor);
		slider.MouseOver.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = factory.GetColor(metroColors.TextColor);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = factory.GetColor(metroColors.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = factory.GetColor(metroColors.TextColor);
		slider.Pressed.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = factory.GetColor(metroColors.TextColor);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = factory.GetColor(metroColors.TextColor);
		slider.Pressed.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.Disabled.SliderLabelColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.Disabled.PartBackground = new GradientColorTable();
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.TrackPart.Default.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(factory.GetColor(metroColors.TextDisabledColor));
		slider.TrackPart.Default.PartBorderColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.TrackPart.Default.PartForeLightColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.TrackPart.Default.TrackLineColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.TrackPart.Default.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver.LabelColor = factory.GetColor(metroColors.TextColor);
		slider.TrackPart.MouseOver.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.TrackPart.MouseOver.PartBackground = new GradientColorTable(factory.GetColor(metroColors.TextColor));
		slider.TrackPart.MouseOver.PartBorderColor = factory.GetColor(metroColors.TextColor);
		slider.TrackPart.MouseOver.PartBorderLightColor = Color.Empty;
		slider.TrackPart.MouseOver.PartForeColor = factory.GetColor(metroColors.TextColor);
		slider.TrackPart.MouseOver.PartForeLightColor = factory.GetColor(metroColors.TextColor);
		slider.TrackPart.MouseOver.TrackLineColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.TrackPart.MouseOver.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.Pressed = slider.TrackPart.MouseOver;
		slider = new Office2007SliderColorTable();
		slider.Default.LabelColor = factory.GetColor(metroColors.TextColor);
		slider.Default.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = factory.GetColor(metroColors.CanvasColor);
		slider.Default.PartForeLightColor = factory.GetColor(metroColors.BaseColorDark);
		slider.Default.TrackLineColor = factory.GetColor(metroColors.BaseColorDark);
		slider.Default.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = factory.GetColor(metroColors.TextColor);
		slider.MouseOver.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = factory.GetColor(metroColors.CanvasColor);
		slider.MouseOver.PartForeLightColor = factory.GetColor(metroColors.BaseColorDark);
		slider.MouseOver.TrackLineColor = factory.GetColor(metroColors.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = factory.GetColor(metroColors.TextColor);
		slider.Pressed.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = factory.GetColor(metroColors.CanvasColorLighterShade);
		slider.Pressed.PartForeLightColor = factory.GetColor(metroColors.BaseColorDark);
		slider.Pressed.TrackLineColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		slider.Pressed.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.Disabled.SliderLabelColor = factory.GetColor(metroColors.TextDisabledColor);
		slider.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color in slider.Default.PartBackground.Colors)
		{
			slider.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color.Color), color.Position));
		}
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = factory.GetColor(metroColors.BaseTextColor);
		slider.TrackPart.Default.SliderLabelColor = factory.GetColor(metroColors.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(factory.GetColor(metroColors.CanvasColor));
		slider.TrackPart.Default.PartBorderColor = factory.GetColor(metroColors.BaseColorDarker);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = factory.GetColor(metroColors.CanvasColor);
		slider.TrackPart.Default.PartForeLightColor = factory.GetColor(metroColors.CanvasColor);
		slider.TrackPart.Default.TrackLineColor = factory.GetColor(metroColors.BaseColorDark);
		slider.TrackPart.Default.TrackLineLightColor = factory.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver = slider.TrackPart.Default;
		slider.TrackPart.Pressed = slider.TrackPart.Default;
		table.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(Bar)) + "+StatusBar", slider);
		table.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(MetroStatusBar)), slider);
		table.ListViewEx.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade));
		table.ListViewEx.ColumnSeparator = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor), Color.Empty);
		table.ListViewEx.SelectionBorder = Color.Empty;
		table.ListViewEx.SelectionForeColor = factory.GetColor(metroColors.BaseTextColor);
		table.DataGridView.BackgroundColor = factory.GetColor(metroColors.CanvasColor);
		table.DataGridView.DefaultCellBackground = factory.GetColor(metroColors.CanvasColor);
		table.DataGridView.DefaultCellText = factory.GetColor(metroColors.TextColor);
		table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade));
		table.DataGridView.ColumnHeaderNormalText = factory.GetColor(metroColors.TextColor);
		table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLighterShade));
		table.DataGridView.ColumnHeaderSelectedText = factory.GetColor(metroColors.TextColor);
		table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(metroColors.BaseColorDark);
		table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(metroColors.BaseColorDark);
		table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(metroColors.BaseColorDark);
		table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade));
		table.DataGridView.RowNormalBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLighterShade));
		table.DataGridView.RowSelectedBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(metroColors.BaseColorDark);
		table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.DataGridView.RowMouseOverBorder = factory.GetColor(metroColors.BaseColorDark);
		table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		table.DataGridView.RowPressedBorder = factory.GetColor(metroColors.BaseColorDark);
		table.DataGridView.GridColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade));
		table.DataGridView.SelectorBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.DataGridView.SelectorBorderDark = Color.Empty;
		table.DataGridView.SelectorBorderLight = Color.Empty;
		table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(metroColors.TextColor));
		table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		table.DataGridView.SelectorMouseOverBorder = factory.GetColor(metroColors.BaseColorDark);
		table.DataGridView.SelectorMouseOverBorderDark = Color.Empty;
		table.DataGridView.SelectorMouseOverBorderLight = Color.Empty;
		table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(metroColors.TextColor));
		table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		table.SideBar.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SideBar.SideBarPanelItemText = factory.GetColor(metroColors.TextColor);
		table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.CanvasColor), 0f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.CanvasColor), 1f));
		table.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 0f));
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 1f));
		table.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 0f));
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColorDark), 1f));
		table.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColorDark), 0f));
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(metroColors.BaseColorDark), 1f));
		table.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(metroColors.TextColor);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(metroColors.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 0f),
			new BackgroundColorBlend(factory.GetColor(metroColors.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor(metroColors.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(metroColors.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 0f),
			new BackgroundColorBlend(factory.GetColor(metroColors.BaseColor), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor(metroColors.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(metroColors.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(metroColors.BaseColorDark), 0f),
			new BackgroundColorBlend(factory.GetColor(metroColors.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor(metroColors.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		table.WarningBox.BackColor = factory.GetColor(factory.GetColor(metroColors.CanvasColor));
		table.WarningBox.WarningBorderColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.WarningBox.WarningBackColor1 = factory.GetColor(metroColors.CanvasColor);
		table.WarningBox.WarningBackColor2 = factory.GetColor(metroColors.CanvasColor);
		table.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(new Color[1] { factory.GetColor(metroColors.CanvasColor) }, new float[1], 90f),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLighterShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.BaseColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLighterShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLighterShade)),
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
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
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
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.BaseColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(factory.GetColor(metroColors.CanvasColor)),
			new ColorDef(factory.GetColor(metroColors.BaseColor)),
			new ColorDef(factory.GetColor(metroColors.TextColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLighterShade)),
			new ColorDef(factory.GetColor(metroColors.TextColor)),
			new ColorDef(factory.GetColor(metroColors.CanvasColorLightShade))
		};
		table.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(factory.GetColor(metroColors.CanvasColorDarkShade)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(metroColors.BaseButtonGradientStart),
				factory.GetColor(metroColors.BaseButtonGradientEnd)
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
		table.SuperTab.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		table.SuperTab.InnerBorder = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.SuperTab.OuterBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTab.ControlBoxDefault.Image = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(metroColors.CanvasColorLightShade);
		table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(metroColors.TextColor);
		table.SuperTab.ControlBoxPressed.Background = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTab.ControlBoxPressed.Border = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTab.ControlBoxPressed.Image = factory.GetColor(metroColors.TextColor);
		table.SuperTab.InsertMarker = factory.GetColor(metroColors.BaseColor);
		table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(factory.GetColor(factory.GetColor(metroColors.CanvasColor)));
		table.SuperTabItem.Default.Normal.InnerBorder = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.SuperTabItem.Default.Normal.OuterBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabItem.Default.Normal.Text = factory.GetColor(metroColors.TextInactiveColor);
		table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(metroColors.CanvasColorLightShade);
		table.SuperTabItem.Default.Disabled.Text = factory.GetColor(metroColors.TextDisabledColor);
		table.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		table.SuperTabItem.Default.Disabled.CloseMarker = factory.GetColor(metroColors.TextDisabledColor);
		table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(metroColors.CanvasColorLighterShade),
			factory.GetColor(metroColors.CanvasColorLightShade),
			factory.GetColor(metroColors.CanvasColorLightShade),
			factory.GetColor(metroColors.CanvasColor)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabItem.Default.Selected.Text = factory.GetColor(metroColors.TextColor);
		table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(metroColors.CanvasColor),
			factory.GetColor(metroColors.CanvasColorLightShade)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(metroColors.TextColor);
		table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), metroColors.CanvasColorLightShade);
		table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(metroColors.TextColor);
		table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabItem.Left = table.SuperTabItem.Default;
		table.SuperTabItem.Bottom = table.SuperTabItem.Default;
		table.SuperTabItem.Right = table.SuperTabItem.Default;
		table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		table.SuperTabPanel.Default.InnerBorder = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.SuperTabPanel.Default.OuterBorder = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SuperTabPanel.Left = table.SuperTabPanel.Default;
		table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
		table.SuperTabPanel.Right = table.SuperTabPanel.Default;
		SuperTabStyleColorFactory.GetMetroBackstageColorTable(table.Backstage, factory, metroColors);
		SuperTabStyleColorFactory.GetMetroBackstageItemColorTable(table.BackstageItem, factory, metroColors);
		SuperTabStyleColorFactory.GetMetroBackstagePanelColorTable(table.BackstagePanel, factory, metroColors);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		switchButtonColorTable.OffBackColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		switchButtonColorTable.OffTextColor = factory.GetColor(metroColors.TextColor);
		switchButtonColorTable.OnBackColor = factory.GetColor(metroColors.BaseColor);
		switchButtonColorTable.OnTextColor = factory.GetColor(metroColors.BaseTextColor);
		switchButtonColorTable.SwitchBackColor = factory.GetColor(metroColors.TextColor);
		switchButtonColorTable.SwitchBorderColor = Color.Empty;
		switchButtonColorTable.TextColor = factory.GetColor(metroColors.TextColor);
		table.SwitchButton = new SwitchButtonColors();
		table.SwitchButton.Default = switchButtonColorTable;
		table.SwitchButton.Disabled.BorderColor = factory.GetColor(metroColors.CanvasColorLightShade);
		table.SwitchButton.Disabled.SwitchBorderColor = Color.Empty;
		table.SwitchButton.Disabled.OffTextColor = table.CheckBoxItem.Disabled.Text;
		table.SwitchButton.Disabled.OnTextColor = table.SwitchButton.Disabled.OffTextColor;
		table.SwitchButton.Disabled.TextColor = table.SwitchButton.Disabled.OffTextColor;
		table.SwitchButton.Disabled.SwitchBackColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		table.SwitchButton.Disabled.OffBackColor = factory.GetColor(metroColors.CanvasColorLightShade);
		table.SwitchButton.Disabled.OnBackColor = factory.GetColor(metroColors.CanvasColorLightShade);
		table.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = factory.GetColor(metroColors.CanvasColorDarkShade);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 0;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BackColor = factory.GetColor(metroColors.CanvasColor);
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
		elementStyle = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(metroColors.CanvasColorDarkShade));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetRichTextBoxStyle(factory.GetColor(metroColors.CanvasColorDarkShade));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(metroColors.CanvasColorDarkShade), factory.GetColor(metroColors.CanvasColor));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(metroColors.CanvasColorDarkShade), factory.GetColor(metroColors.CanvasColor));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2010BlueFactory.GetRibbonClientPanelStyle(factory, eOffice2010ColorScheme.Silver);
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
		elementStyle = GetSlideOutButtonStyle(metroColors.ComplementColor);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMetroTilePanelStyle(factory.GetColor(metroColors.CanvasColor));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMetroTileGroupStyle(factory.GetColor(metroColors.TextColor));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMonthCalendarStyle(factory.GetColor(metroColors.CanvasColor));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		table.LabelItemColors.Clear();
		table.LabelItemColors.Add(typeof(MetroStatusBar), new LabelColors(factory.GetColor(metroColors.BaseTextColor), factory.GetColor(metroColors.TextDisabledColor)));
		table.LabelItemColors.Add(typeof(Bar), new LabelColors(factory.GetColor(metroColors.BaseTextColor), factory.GetColor(metroColors.TextDisabledColor)));
		InitAppButtonColors(table, factory, metroColors);
		table.StepIndicator.BackgroundColor = factory.GetColor(metroColors.CanvasColorLighterShade);
		table.StepIndicator.IndicatorColor = factory.GetColor(Color.FromArgb(128, metroColors.ComplementColorLight));
	}

	internal static Office2007ButtonItemColorTable GetAppFallBackColorTable()
	{
		if (FallBackAppButtonColorTable != null)
		{
			return FallBackAppButtonColorTable;
		}
		MetroPartColors metroPartColors = MetroColorTableInitializer.CreateMetroPartColors(MetroColorGeneratorParameters.Default.CanvasColor, MetroColorGeneratorParameters.Default.BaseColor);
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(metroPartColors.BaseColor);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = metroPartColors.BaseTextColor;
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(metroPartColors.BaseColorLight1);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = metroPartColors.BaseTextColor;
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(metroPartColors.BaseColorDark);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = metroPartColors.BaseTextColor;
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		FallBackAppButtonColorTable = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	internal static void InitAppButtonColors(Office2007ColorTable colorTable, ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTableCollection applicationButtonColors = colorTable.ApplicationButtonColors;
		applicationButtonColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight1));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(13378923), factory.GetColor(11602001));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(9371964), factory.GetColor(9701185));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(32, 16777215));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(14037618), factory.GetColor(11602770));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(9371964), factory.GetColor(9767234));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 16777215), factory.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(11865171), factory.GetColor(11537234));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(9371965), factory.GetColor(9766721));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 16777215), factory.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(15889232), factory.GetColor(15029551));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16293442), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(13380370), factory.GetColor(13579285));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(16157012), factory.GetColor(16223038));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(16285261), factory.GetColor(15095086));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16498014), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(13248528), factory.GetColor(13447703));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(16493146), factory.GetColor(16562263));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(14897949), factory.GetColor(14700057));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16625484), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(13248528), factory.GetColor(13447187));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(15229481), factory.GetColor(16485700));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(4560689), factory.GetColor(2850604));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(7064133), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(1598263), factory.GetColor(1993273));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(5022257), factory.GetColor(5485361));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(4626228), factory.GetColor(2522155));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(9033320), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(1664055), factory.GetColor(2190140));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(6666563), factory.GetColor(7392330));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(3113514), factory.GetColor(2784812));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(6867768), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(1598519), factory.GetColor(1927736));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(3575595), factory.GetColor(5881133));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(1415061), factory.GetColor(98945));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(2144180), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(351838), factory.GetColor(484452));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(1811101), factory.GetColor(2077097));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(2071705), factory.GetColor(230788));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(3591626), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(351838), factory.GetColor(616295));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(3193011), factory.GetColor(3457726));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(164994), factory.GetColor(163969));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(2933438), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(351838), factory.GetColor(550245));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(167821), factory.GetColor(1946797));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBar(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(metroColors.TextColor);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarMouseOver(ColorFactory factory, MetroPartColors metroColors)
	{
		return GetRibbonBar(factory, metroColors);
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarExpanded(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), factory.GetColor(metroColors.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(factory.GetColor(180, Color.White)), factory.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLighterShade), Color.Transparent);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007CheckBoxColorTable GetCheckBoxItem(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = new Office2007CheckBoxColorTable();
		office2007CheckBoxColorTable.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Default.CheckBorder = factory.GetColor(metroColors.TextDisabledColor);
		office2007CheckBoxColorTable.Default.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007CheckBoxColorTable.Default.CheckInnerBorder = factory.GetColor(metroColors.CanvasColor);
		office2007CheckBoxColorTable.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(metroColors.TextLightColor), Color.Empty);
		office2007CheckBoxColorTable.Default.Text = factory.GetColor(metroColors.TextColor);
		office2007CheckBoxColorTable.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.CheckBorder = factory.GetColor(metroColors.BaseColorLight);
		office2007CheckBoxColorTable.MouseOver.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		office2007CheckBoxColorTable.MouseOver.CheckInnerBorder = factory.GetColor(metroColors.BaseColorLightest);
		office2007CheckBoxColorTable.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(metroColors.TextColor), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.Text = factory.GetColor(metroColors.TextColor);
		office2007CheckBoxColorTable.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckBorder = factory.GetColor(metroColors.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckInnerBorder = factory.GetColor(metroColors.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(metroColors.TextColor), Color.Empty);
		office2007CheckBoxColorTable.Pressed.Text = factory.GetColor(metroColors.TextColor);
		office2007CheckBoxColorTable.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Disabled.CheckBorder = factory.GetColor(metroColors.CanvasColorLightShade);
		office2007CheckBoxColorTable.Disabled.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007CheckBoxColorTable.Disabled.CheckInnerBorder = factory.GetColor(metroColors.CanvasColorLightShade);
		office2007CheckBoxColorTable.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorDarkShade), Color.Empty);
		office2007CheckBoxColorTable.Disabled.Text = factory.GetColor(metroColors.CanvasColorDarkShade);
		return office2007CheckBoxColorTable;
	}

	private static Office2007ButtonItemColorTable GetButtonItemBackstageDefault(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(192, 16777215));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(32, metroColors.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(32, metroColors.BaseColor));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(metroColors.TextColor);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemDefault(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(metroColors.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(metroColors.BaseColorLightest);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(metroColors.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(metroColors.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLighter));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLighter));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLighter));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(metroColors.BaseColorLightText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(metroColors.TextColor);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemDefaultWithBackground(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable buttonItemDefault = GetButtonItemDefault(factory, metroColors);
		buttonItemDefault.Default.TopBackground = null;
		buttonItemDefault.Default.TopBackgroundHighlight = null;
		buttonItemDefault.Default.BottomBackground = null;
		buttonItemDefault.Default.BottomBackgroundHighlight = null;
		buttonItemDefault.Default.Background = new LinearGradientColorTable(metroColors.CanvasColor);
		buttonItemDefault.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemDefault.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorDarkShade));
		buttonItemDefault.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemDefault.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemDefault.MouseOverSplitInactive = buttonItemDefault.Default;
		buttonItemDefault.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemDefault.Disabled.TopBackground = null;
		buttonItemDefault.Disabled.TopBackgroundHighlight = null;
		buttonItemDefault.Disabled.BottomBackground = null;
		buttonItemDefault.Disabled.BottomBackgroundHighlight = null;
		buttonItemDefault.Disabled.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		buttonItemDefault.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemDefault.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade), Color.Empty);
		buttonItemDefault.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemDefault.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemDefault.Disabled.Text = factory.GetColor(metroColors.CanvasColorLightShade);
		return buttonItemDefault;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlue(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(metroColors.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(metroColors.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(metroColors.BaseColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(metroColors.BaseColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(metroColors.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(metroColors.BaseTextColor);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueWithBackground(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable buttonItemBlue = GetButtonItemBlue(factory, metroColors);
		buttonItemBlue.Default.TopBackground = null;
		buttonItemBlue.Default.TopBackgroundHighlight = null;
		buttonItemBlue.Default.BottomBackground = null;
		buttonItemBlue.Default.BottomBackgroundHighlight = null;
		buttonItemBlue.Default.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		buttonItemBlue.Default.InnerBorder = LinearGradientColorTable.Empty;
		buttonItemBlue.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		buttonItemBlue.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Default.Text = factory.GetColor(metroColors.BaseTextColor);
		buttonItemBlue.MouseOverSplitInactive = buttonItemBlue.Default;
		buttonItemBlue.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlue.Disabled.TopBackground = null;
		buttonItemBlue.Disabled.TopBackgroundHighlight = null;
		buttonItemBlue.Disabled.BottomBackground = null;
		buttonItemBlue.Disabled.BottomBackgroundHighlight = null;
		buttonItemBlue.Disabled.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		buttonItemBlue.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade), Color.Empty);
		buttonItemBlue.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled.Text = factory.GetColor(metroColors.CanvasColorLightShade);
		return buttonItemBlue;
	}

	public static Office2007ButtonItemColorTable GetButtonItemMagenta(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(metroColors.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(metroColors.ComplementColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(metroColors.ComplementColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(metroColors.ComplementColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(metroColors.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(metroColors.ComplementColorText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(metroColors.ComplementColorText);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemMagentaWithBackground(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable buttonItemMagenta = GetButtonItemMagenta(factory, metroColors);
		buttonItemMagenta.Default.TopBackground = null;
		buttonItemMagenta.Default.TopBackgroundHighlight = null;
		buttonItemMagenta.Default.BottomBackground = null;
		buttonItemMagenta.Default.BottomBackgroundHighlight = null;
		buttonItemMagenta.Default.Background = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		buttonItemMagenta.Default.InnerBorder = LinearGradientColorTable.Empty;
		buttonItemMagenta.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.ComplementColor));
		buttonItemMagenta.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Default.Text = factory.GetColor(metroColors.ComplementColorText);
		buttonItemMagenta.MouseOverSplitInactive = buttonItemMagenta.Default;
		buttonItemMagenta.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemMagenta.Disabled.TopBackground = null;
		buttonItemMagenta.Disabled.TopBackgroundHighlight = null;
		buttonItemMagenta.Disabled.BottomBackground = null;
		buttonItemMagenta.Disabled.BottomBackgroundHighlight = null;
		buttonItemMagenta.Disabled.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		buttonItemMagenta.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColorLightShade), Color.Empty);
		buttonItemMagenta.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled.Text = factory.GetColor(metroColors.CanvasColorLightShade);
		return buttonItemMagenta;
	}

	public static void SetBlueExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
	{
		Color color = factory.GetColor(4542298);
		Color expandLight = Color.FromArgb(192, Color.White);
		ct.Default.ExpandBackground = color;
		ct.Default.ExpandLight = expandLight;
		ct.Checked.ExpandBackground = color;
		ct.Checked.ExpandLight = expandLight;
		ct.Expanded.ExpandBackground = color;
		ct.Expanded.ExpandLight = expandLight;
		ct.MouseOver.ExpandBackground = color;
		ct.MouseOver.ExpandLight = expandLight;
		ct.Pressed.ExpandBackground = color;
		ct.Pressed.ExpandLight = expandLight;
	}

	public static Office2007ButtonItemColorTable CreateBlueOrbColorTable(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueOrb);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseTextColor));
		office2007ButtonItemColorTable.Default.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Default.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(metroColors.BaseColorLightest);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(metroColors.BaseColorLight);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseTextColor));
		office2007ButtonItemColorTable.Checked.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLightest));
		office2007ButtonItemColorTable.Expanded.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(metroColors.BaseColorLightest);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight1), Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = factory.GetColor(metroColors.BaseColorLight1);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemStatusBar(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(metroColors.BaseColorLight1);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight1));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(metroColors.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(metroColors.BaseTextColor);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueDefault(ColorFactory factory, MetroPartColors metroColors)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(metroColors.TextLightColor);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(metroColors.CanvasColorLightShade);
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(metroColors.BaseColor);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(metroColors.CanvasColor));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(metroColors.CanvasColorLightShade);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(metroColors.BaseColor);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(metroColors.BaseColor);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueMagenta(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(16047854), factory.GetColor(16711422));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(14715072), factory.GetColor(14642602));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16047854), factory.GetColor(16711422));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14715072), factory.GetColor(14642602));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(15578584), factory.GetColor(16579579));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16579579), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16373742), factory.GetColor(16641528));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14891409));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(14019283), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12173751), factory.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14019283), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12173751), factory.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(11788003), factory.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14872575), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289388), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289388), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(13623521), factory.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14872575), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle GetSlideOutButtonStyle(Color complementColor)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
		elementStyle.BackColor = complementColor;
		return elementStyle;
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

	public static void InitializeScrollBarColorTable(Office2007ColorTable t, ColorFactory factory, MetroPartColors metroParts)
	{
		InitializeScrollBarColorTable(t.ScrollBar, factory, metroParts);
	}

	public static void InitializeScrollBarColorTable(Office2007ScrollBarColorTable scrollBarColorTable, ColorFactory factory, MetroPartColors metroParts)
	{
		Color start = Color.FromArgb(205, 205, 205);
		Color start2 = Color.FromArgb(166, 166, 166);
		Color start3 = Color.FromArgb(166, 166, 166);
		Color start4 = Color.FromArgb(180, 180, 180);
		Color start5 = Color.FromArgb(160, 160, 160);
		Office2007ScrollBarStateColorTable @default = scrollBarColorTable.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(metroParts.CanvasColor));
		@default.Border = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(metroParts.TextInactiveColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(start4);
		@default.TrackSignBackground = new LinearGradientColorTable(start);
		@default = scrollBarColorTable.MouseOver;
		@default.Background = scrollBarColorTable.Default.Background;
		@default.Border = scrollBarColorTable.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(metroParts.TextColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(start4);
		@default.TrackSignBackground = new LinearGradientColorTable(start2);
		@default = scrollBarColorTable.MouseOverControl;
		@default.Background = scrollBarColorTable.Default.Background;
		@default.Border = scrollBarColorTable.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(metroParts.TextColor));
		@default.ThumbSignBackground = scrollBarColorTable.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(start);
		@default = scrollBarColorTable.Pressed;
		@default.Background = scrollBarColorTable.Default.Background;
		@default.Border = scrollBarColorTable.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(metroParts.TextColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(start5);
		@default.TrackSignBackground = new LinearGradientColorTable(start3);
		@default = scrollBarColorTable.Disabled;
		@default.Background = scrollBarColorTable.Default.Background;
		@default.Border = scrollBarColorTable.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(metroParts.TextDisabledColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void InitializeAppBlueScrollBarColorTable(Office2007ColorTable t, ColorFactory factory, MetroPartColors metroParts)
	{
		InitializeScrollBarColorTable(t.AppScrollBar, factory, metroParts);
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupDefault(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(16183804));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(9658067);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(9658067));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupOrange(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(16775405));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(13589514);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(16751872));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupMagenta(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(16576759));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(13195676);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(13195676));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupGreen(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(factory.GetColor(15135718));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(4825929);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(factory.GetColor(4825929));
		return office2007RibbonTabGroupColorTable;
	}
}
