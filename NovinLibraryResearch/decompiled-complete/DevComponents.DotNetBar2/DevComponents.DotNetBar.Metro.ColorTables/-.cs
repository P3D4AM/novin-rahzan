using System;
using System.Drawing;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro.Helpers;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Schedule;
using DevComponents.WinForms.Drawing;

namespace DevComponents.DotNetBar.Metro.ColorTables;

internal static class _2215
{
	private static Office2007ButtonItemColorTable _21AE;

	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Color color = ቖ.GetColor(ጴ.ፚ ? ጴ.CanvasColor : ጴ.CanvasColorLight);
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.RibbonControl.InnerBorder = null;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(ጴ.CanvasColor));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(color);
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.KeyTips.KeyTipBackground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.KeyTips.KeyTipText = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.KeyTips.KeyTipBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ, ጴ, color);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ, ጴ, color);
		ቯ.RibbonBar.Expanded = _21B4(ቖ, ጴ, color);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _221A(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CB(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CD(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21B1.ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(_221B(ቖ, ጴ));
		ቯ.BackstageButtonItemColors.Clear();
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ, ጴ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "StatusBar"), _21BD(ቖ, ጴ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B6(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B8(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B9(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Bar.ToolbarBottomBackground = null;
		ቯ.Bar.ToolbarBottomBorder = Color.Empty;
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorderLight = LinearGradientColorTable.Empty;
		ቯ.Menu.SideUnused = LinearGradientColorTable.Empty;
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseButtonGradientStart), ቖ.GetColor(ጴ.BaseButtonGradientEnd), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDarker));
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty, 90);
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight), ቖ.GetColor(ጴ.BaseColorDark), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDarker));
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty, 90);
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight), Color.Empty);
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextInactiveColor));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.SystemButton.MouseOver.BottomBackground = null;
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = null;
		ቯ.SystemButton.MouseOver.InnerBorder = null;
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = null;
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = null;
		ቯ.SystemButton.Pressed.InnerBorder = null;
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextInactiveColor));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = null;
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = null;
		ቯ.SystemButtonClose.MouseOver.InnerBorder = null;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = null;
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = null;
		ቯ.SystemButtonClose.Pressed.InnerBorder = null;
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Active.CaptionBottomBorder = null;
		ቯ.Form.Active.CaptionText = ቖ.GetColor(ጴ.TextLightColor);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.Form.BackColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Form.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.Form.MdiClientBackgroundImage = null;
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.InnerBorder = Color.Empty;
		ቯ.TabControl.Default.OuterBorder = Color.Empty;
		ቯ.TabControl.Default.Text = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.MouseOver.InnerBorder = Color.Empty;
		ቯ.TabControl.MouseOver.OuterBorder = Color.Empty;
		ቯ.TabControl.MouseOver.Text = ጴ.BaseColor;
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Selected.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Selected.InnerBorder = Color.Empty;
		ቯ.TabControl.Selected.OuterBorder = ጴ.CanvasColorLightShade;
		ቯ.TabControl.Selected.Text = ጴ.BaseColor;
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ጴ.CanvasColorLightShade;
		ቯ.CheckBoxItem = _2218(ቖ, ጴ);
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = _2218(ቖ, ጴ);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(Bar)) + "+StatusBar", office2007CheckBoxColorTable);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(MetroStatusBar)), office2007CheckBoxColorTable);
		ዱ(ቯ, ቖ, ጴ);
		ዳ(ቯ, ቖ, ጴ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(ጴ.CanvasColor);
		progressBarItem.OuterBorder = ቖ.GetColor(ጴ.BaseColorDarker);
		progressBarItem.InnerBorder = Color.Empty;
		progressBarItem.Chunk = new GradientColorTable(ጴ.BaseColor);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkShadow = new GradientColorTable();
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		gallery.GroupLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		gallery.GroupLabelBorder = Color.Empty;
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarBackground2 = Color.Empty;
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarCaptionBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.ItemExpandedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemExpandedBorder = Color.Empty;
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.ItemHotBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemPressedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemSeparatorShade = Color.Empty;
		ቯ.LegacyColors.ItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBackground2 = Color.Empty;
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = Color.Empty;
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.Black;
		ቯ.LegacyColors.BarDockedBorder = Color.Empty;
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground2 = Color.Empty;
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.LegacyColors.PanelBackground2 = Color.Empty;
		ቯ.LegacyColors.PanelText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.ExplorerBarBackground2 = Color.Empty;
		ቯ.LegacyColors.SplitterBackground = ቯ.LegacyColors.PanelBackground;
		ቯ.LegacyColors.SplitterBackground2 = ቯ.LegacyColors.PanelBackground2;
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.PanelText;
		ቯ.LegacyColors.SplitterBorder = ቯ.LegacyColors.PanelBorder;
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(ጴ.TextColor);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.PartBackground = new GradientColorTable();
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextDisabledColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextColor));
		slider.TrackPart.MouseOver.PartBorderColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBorderLightColor = Color.Empty;
		slider.TrackPart.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.Pressed = slider.TrackPart.MouseOver;
		slider = new Office2007SliderColorTable();
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.Default.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		slider.Pressed.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
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
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.BaseTextColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.BaseColorDarker);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver = slider.TrackPart.Default;
		slider.TrackPart.Pressed = slider.TrackPart.Default;
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(Bar)) + "+StatusBar", slider);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(MetroStatusBar)), slider);
		ቮ(ቯ, ቖ, ጴ);
		ቯ.ListViewEx.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = Color.Empty;
		ቯ.ListViewEx.SelectionForeColor = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.DataGridView.BackgroundColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.ColumnHeaderNormalText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.ColumnHeaderSelectedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.GridColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.SelectorMouseOverBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SideBar.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_221F(ቯ.AdvTree, ቖ, ጴ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.TextColor);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(ጴ.CanvasColor);
		Office2007CalendarViewColorTable calendarView = ቯ.CalendarView;
		ColorDef[] array = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		Color[] colors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		float[] cPositions = new float[1];
		array[2] = new ColorDef(colors, cPositions, 90f);
		array[3] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[4] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[5] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[6] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[7] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[8] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[9] = new ColorDef(ቖ.GetColor(ጴ.BaseColor));
		array[10] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[11] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[12] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[13] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[14] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[15] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[16] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[17] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[18] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[19] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[20] = new ColorDef(new Color[4]
		{
			ቖ.GetColor(16772473),
			ቖ.GetColor(16767083),
			ቖ.GetColor(16759552),
			ቖ.GetColor(16771703)
		}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f);
		array[21] = new ColorDef(ቖ.GetColor(15436032));
		calendarView.WeekDayViewColors = array;
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(ጴ.BaseButtonGradientStart),
				ቖ.GetColor(ጴ.BaseButtonGradientEnd)
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
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(ጴ.BaseColor);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor)));
		ቯ.SuperTabItem.Default.Normal.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Normal.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(ጴ.CanvasColorLighterShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColor)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColorLightShade)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጳ(ቯ.Backstage, ቖ, ጴ);
		ጦ.ጺ(ቯ.BackstageItem, ቖ, ጴ);
		ጦ.ፀ(ቯ.BackstagePanel, ቖ, ጴ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(ጴ.TextColor);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(ጴ.BaseColor);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(ጴ.BaseTextColor);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(ጴ.TextColor);
		switchButtonColorTable.SwitchBorderColor = Color.Empty;
		switchButtonColorTable.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.SwitchBorderColor = Color.Empty;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SwitchButton.Disabled.OffBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.OnBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 0;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BackColor = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _21B1.ጌ(ቖ, eOffice2010ColorScheme.Silver);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade), ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጙ(ጴ.ComplementColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ጴ.BaseColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(color);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(ጴ.BaseColorLightText), ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.BaseColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.LabelItemColors.Clear();
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(MetroStatusBar)), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar), "StatusBar"), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar)), new LabelColors(ቖ.GetColor(ጴ.TextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		_21B0(ቯ, ቖ, ጴ);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(Color.FromArgb(128, ጴ.ComplementColorLight));
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(ጴ.ComplementColor);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColorDarkShade) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.BaseColorLightest) }, 90, new float[0], ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorLightest) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) });
		ቯ.TokenEditor = new TokenEditorColorTable(ጴ.TextColor, ጴ.CanvasColorLighterShade, ጴ.TextColor, ጴ.CanvasColorLightShade, ጴ.BaseTextColor, ጴ.BaseColor);
		ቯ.Flyout.BorderColor = ጴ.BaseColor;
		ቯ.Flyout.BackColor = ጴ.CanvasColor;
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseTextColor), new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseTextColor), new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(ጴ.BaseColorDark),
			Color.Empty,
			ቖ.GetColor(ጴ.BaseColorDark)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(ጴ.BaseColorDark)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(ጴ.TextLightColor);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(ጴ.TextLightColor);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLighterShade) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.TextLightColor), new Color[1] { ቖ.GetColor(color) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.BaseColor)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	internal static Office2007ButtonItemColorTable _2216()
	{
		if (_21AE != null)
		{
			return _21AE;
		}
		MetroPartColors metroPartColors = _2224._222D(MetroColorGeneratorParameters.Default.CanvasColor, MetroColorGeneratorParameters.Default.BaseColor);
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
		_21AE = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	internal static void _21B0(Office2007ColorTable _139D, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTableCollection applicationButtonColors = _139D.ApplicationButtonColors;
		applicationButtonColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(13378923), ቖ.GetColor(11602001));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9701185));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(32, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14037618), ቖ.GetColor(11602770));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9767234));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(11865171), ቖ.GetColor(11537234));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371965), ቖ.GetColor(9766721));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(15889232), ቖ.GetColor(15029551));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16293442), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13380370), ቖ.GetColor(13579285));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16157012), ቖ.GetColor(16223038));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16285261), ቖ.GetColor(15095086));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16498014), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447703));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16493146), ቖ.GetColor(16562263));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14897949), ቖ.GetColor(14700057));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16625484), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447187));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15229481), ቖ.GetColor(16485700));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4560689), ቖ.GetColor(2850604));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(7064133), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598263), ቖ.GetColor(1993273));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(5022257), ቖ.GetColor(5485361));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4626228), ቖ.GetColor(2522155));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(9033320), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1664055), ቖ.GetColor(2190140));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(6666563), ቖ.GetColor(7392330));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(3113514), ቖ.GetColor(2784812));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(6867768), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598519), ቖ.GetColor(1927736));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3575595), ቖ.GetColor(5881133));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(1415061), ቖ.GetColor(98945));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2144180), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(484452));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(1811101), ቖ.GetColor(2077097));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(2071705), ቖ.GetColor(230788));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(3591626), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(616295));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3193011), ቖ.GetColor(3457726));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(164994), ቖ.GetColor(163969));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2933438), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(550245));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(167821), ቖ.GetColor(1946797));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(ጴ.TextColor);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		return ᱬ(ቖ, ጴ, _2217);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(180, Color.White)), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Transparent);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007CheckBoxColorTable _2218(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = new Office2007CheckBoxColorTable();
		office2007CheckBoxColorTable.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Default.CheckBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		office2007CheckBoxColorTable.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Default.CheckInnerBorder = ቖ.GetColor(ጴ.CanvasColor);
		office2007CheckBoxColorTable.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextLightColor), Color.Empty);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007CheckBoxColorTable.MouseOver.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007CheckBoxColorTable.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Disabled.CheckBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		office2007CheckBoxColorTable.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Disabled.CheckInnerBorder = Color.Empty;
		office2007CheckBoxColorTable.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		office2007CheckBoxColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		return office2007CheckBoxColorTable;
	}

	private static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(192, 16777215));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _2219(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLighter));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLighter));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLighter));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.TextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _221A(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ጴ.CanvasColor);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.BaseColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ጴ.BaseColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CB(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CC(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.ComplementColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.ComplementColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ጴ.ComplementColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.ComplementColorText);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(4542298);
		Color expandLight = Color.FromArgb(192, Color.White);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = expandLight;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = expandLight;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = expandLight;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = expandLight;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = expandLight;
	}

	public static Office2007ButtonItemColorTable _221B(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueOrb);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Default.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Default.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.MouseOver.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOverSplitInactive.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseColorLight);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Checked.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Expanded.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1), Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.BaseColorLight1);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21BD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.BaseColorLight1);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextLightColor);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ጴ.CanvasColorLightShade);
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ጴ.CanvasColorLightShade);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColor);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15578584), ቖ.GetColor(16579579));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16579579), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16373742), ቖ.GetColor(16641528));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14891409));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11788003), ቖ.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13623521), ቖ.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle ጙ(Color _221C)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
		elementStyle.BackColor = _221C;
		elementStyle.TextColor = Color.White;
		return elementStyle;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
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
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
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

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.ScrollBar, ቖ, _221D);
	}

	public static void ዱ(Office2007ScrollBarColorTable _221E, ColorFactory ቖ, MetroPartColors _221D)
	{
		Office2007ScrollBarStateColorTable @default = _221E.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColor));
		@default.Border = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColor));
		@default = _221E.MouseOver;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLighterShade));
		@default = _221E.MouseOverControl;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.ThumbSignBackground = _221E.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColor));
		@default = _221E.Pressed;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLighterShade));
		@default = _221E.Disabled;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextDisabledColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.AppScrollBar, ቖ, _221D);
	}

	public static void _221F(TreeColorTable ዅ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ጴ.CanvasColorLightShade);
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.TextColor));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0.4f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(ጴ.CanvasColorLightShade);
	}

	public static Office2007RibbonTabGroupColorTable _21B6(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16183804));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(9658067);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(9658067));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B9(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16775405));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(13589514);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(16751872));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B7(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16576759));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(13195676);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(13195676));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B8(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(15135718));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(4825929);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(4825929));
		return office2007RibbonTabGroupColorTable;
	}

	internal static void ቮ(Office2007ColorTable ቯ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2010RangeSliderColorTable rangeSlider = ቯ.RangeSlider;
		rangeSlider.TickLineColor = ቖ.GetColor(ጴ.TextInactiveColor);
		rangeSlider.RangeBackground = new GradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		rangeSlider.RangeLineColor = ቖ.GetColor(ጴ.TextInactiveColor);
		rangeSlider.RangeValueBackground = new GradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		rangeSlider.RangeValueLineColor = ቖ.GetColor(ጴ.BaseColor);
		rangeSlider.RangeCornerRadius = 0;
		rangeSlider.MinRangeSlider.Default.Background = new GradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		rangeSlider.MinRangeSlider.Default.BorderColor = ቖ.GetColor(ጴ.BaseColor);
		rangeSlider.MinRangeSlider.Default.BorderLightColor = Color.Empty;
		rangeSlider.MinRangeSlider.Disabled.Background = new GradientColorTable(ቖ.GetColor(ጴ.TextDisabledColor));
		rangeSlider.MinRangeSlider.Disabled.BorderColor = ጴ.TextDisabledColor;
		rangeSlider.MinRangeSlider.Disabled.BorderLightColor = Color.Empty;
		rangeSlider.MinRangeSlider.MouseOver.Background = new GradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		rangeSlider.MinRangeSlider.MouseOver.BorderColor = ጴ.BaseColorDark;
		rangeSlider.MinRangeSlider.MouseOver.BorderLightColor = Color.Empty;
		rangeSlider.MinRangeSlider.Pressed.Background = rangeSlider.MinRangeSlider.MouseOver.Background;
		rangeSlider.MinRangeSlider.Pressed.BorderColor = rangeSlider.MinRangeSlider.MouseOver.BorderColor;
		rangeSlider.MinRangeSlider.Pressed.BorderLightColor = rangeSlider.MinRangeSlider.MouseOver.BorderLightColor;
		rangeSlider.MaxRangeSlider = rangeSlider.MinRangeSlider;
	}
}
internal class _2224
{
	private const double _2225 = 1.0 / 360.0;

	public static MetroColorTable _2226(MetroColorGeneratorParameters _139F)
	{
		return _2226(_139F.CanvasColor, _139F.BaseColor);
	}

	public static MetroColorTable _2226(Color _2227, Color ॲ)
	{
		MetroColorTable metroColorTable = new MetroColorTable();
		Office2007ColorTable office2007ColorTable = new Office2007ColorTable();
		_1A76(metroColorTable, office2007ColorTable, _2227, ॲ);
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = office2007ColorTable;
		}
		return metroColorTable;
	}

	private static double _2228(double _2229, int _222A)
	{
		_2229 += (double)_222A * (1.0 / 360.0);
		if (_2229 > 1.0)
		{
			_2229 -= 1.0;
		}
		else if (_2229 < 0.0)
		{
			_2229 += 1.0;
		}
		return _2229;
	}

	private static double _222B(double _222C)
	{
		if (_222C == 1.0)
		{
			return 1.0;
		}
		if (_222C < 0.81)
		{
			return _222C + 0.1;
		}
		if (_222C < 0.91)
		{
			return _222C + 0.05;
		}
		return _222C + (0.99 - _222C);
	}

	public static MetroPartColors _222D(Color _2227, Color ॲ)
	{
		ColorFunctions.HLSColor hLSColor = ColorFunctions.RGBToHSL(_2227);
		ColorFunctions.HLSColor hLSColor2 = ColorFunctions.RGBToHSL(ॲ);
		_1A78 obj = _1A7B._1A7C(ॲ);
		_1A78 obj2 = _1A7B._1A7C(_2227);
		MetroPartColors metroPartColors = new MetroPartColors();
		metroPartColors.CanvasColor = _2227;
		metroPartColors.BaseColor = ॲ;
		metroPartColors.TextColor = ((hLSColor.Lightness < 0.4) ? Color.White : Color.Black);
		metroPartColors.TextInactiveColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.53) : (obj2._༡ - 0.47));
		metroPartColors.TextDisabledColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.67));
		metroPartColors.TextLightColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.6) : (obj2._༡ + 0.4));
		metroPartColors.CanvasColorDarkShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.67));
		metroPartColors.CanvasColorLightShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.17) : (obj2._༡ - 0.17));
		metroPartColors.CanvasColorLighterShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.05) : (obj2._༡ - 0.05));
		if (metroPartColors.ፚ)
		{
			metroPartColors.CanvasColorLight = ((metroPartColors.TextColor == Color.Black) ? _1A7B._1A7D(0.0, 0.0, 0.99) : _1A7B._1A7D(obj2._1A79, obj2._1A7A, obj2._༡ + 0.05));
		}
		else
		{
			metroPartColors.CanvasColorLight = ((metroPartColors.TextColor == Color.Black) ? _1A7B._1A7D(obj2._1A79, obj2._1A7A, obj2._༡ + ((obj2._༡ >= 0.96) ? 0.02 : 0.058)) : _1A7B._1A7D(obj2._1A79, obj2._1A7A, obj2._༡ - 0.05));
		}
		metroPartColors.BaseTextColor = _1933(ॲ);
		metroPartColors.BaseColorLight = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.41), obj._༡ + 0.3);
		metroPartColors.BaseColorLight1 = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.12), obj._༡ + 0.12);
		metroPartColors.BaseColorLightText = _1933(metroPartColors.BaseColorLight);
		metroPartColors.BaseColorLightest = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.6), obj._༡ + 0.35);
		metroPartColors.BaseColorLighter = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.4), obj._༡ + 0.39);
		metroPartColors.BaseColorDark = _1A7B._1A7D(obj._1A79, obj._1A7A + 0.1, obj._༡ - 0.06);
		metroPartColors.BaseColorDarker = ColorFunctions.HLSToRGB(hLSColor2.Hue, hLSColor2.Lightness - 0.2, hLSColor2.Saturation);
		metroPartColors.ComplementColor = _1A7B._1A7D(_2232(obj._1A79), obj._1A7A, obj._༡ + ((obj._༡ > 0.5) ? (-0.35) : 0.0));
		ColorFunctions.HLSColor hLSColor3 = ColorFunctions.RGBToHSL(metroPartColors.ComplementColor);
		_1A78 obj3 = _1A7B._1A7C(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLight = _1A7B._1A7D(obj3._1A79, obj3._1A7A, obj3._༡ + 0.2);
		metroPartColors.ComplementColorDark = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.1, hLSColor3.Saturation);
		metroPartColors.ComplementColorDarker = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.2, hLSColor3.Saturation);
		metroPartColors.ComplementColorText = _1933(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLightText = _1933(metroPartColors.ComplementColorLight);
		metroPartColors.BaseButtonGradientStart = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.01) : (obj2._༡ + 0.08));
		metroPartColors.BaseButtonGradientEnd = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.08) : (obj2._༡ + 0.2));
		if (metroPartColors.TextColor == Color.Black)
		{
			metroPartColors.EditControlBackColor = Color.White;
		}
		else
		{
			metroPartColors.EditControlBackColor = Color.Black;
		}
		return metroPartColors;
	}

	public static void _1A76(MetroColorTable _222E, Office2007ColorTable _222F, Color _2227, Color ॲ)
	{
		MetroPartColors metroPartColors = _222D(_2227, ॲ);
		_222E.CanvasColor = metroPartColors.CanvasColor;
		_222E.CanvasColorShadeLight = metroPartColors.CanvasColorLightShade;
		_222E.CanvasColorShadeLighter = metroPartColors.CanvasColorLighterShade;
		_222E.ForeColor = metroPartColors.TextColor;
		_222E.BaseColor = ॲ;
		_222E.EditControlBackColor = metroPartColors.EditControlBackColor;
		_222E.MetroPartColors = metroPartColors;
		_222E.MetroAppForm.BorderThickness = new Thickness(0.0, 0.0, 1.0, 1.0);
		_222E.MetroAppForm.BorderColors = new BorderColors(metroPartColors.BaseColor);
		_222E.MetroAppForm.BorderColorsInactive = new BorderColors(metroPartColors.BaseColorLight);
		_222E.MetroForm.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
		_222E.MetroForm.BorderColors = new BorderColors[3]
		{
			new BorderColors(metroPartColors.BaseColorDark),
			new BorderColors(metroPartColors.BaseColor),
			new BorderColors(metroPartColors.BaseColor)
		};
		_222E.MetroForm.BorderColorsInactive = new BorderColors[4]
		{
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight)
		};
		_222E.MetroTab.ActiveCaptionText = metroPartColors.TextInactiveColor;
		_222E.MetroTab.InactiveCaptionText = metroPartColors.TextDisabledColor;
		_222E.MetroTab.MetroTabItem.Default = _2233(metroPartColors.TextInactiveColor);
		_222E.MetroTab.MetroTabItem.Selected = _2233(metroPartColors.BaseColor);
		_222E.MetroTab.MetroTabItem.Disabled = _2233(metroPartColors.TextDisabledColor);
		_222E.MetroTab.MetroTabItem.MouseOver = _2233(metroPartColors.TextInactiveColor, metroPartColors.BaseColor);
		_222E.MetroTab.MetroTabItem.Pressed = null;
		_222E.MetroTab.TabPanelBackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColor);
		_222E.MetroToolbar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColor);
		_222E.MetroStatusBar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.BaseColor);
		_222E.MetroStatusBar.TopBorders = new Color[0];
		_222E.MetroStatusBar.BottomBorders = new Color[0];
		_222E.MetroStatusBar.ResizeMarkerLightColor = Color.FromArgb(196, Color.White);
		_222E.MetroStatusBar.ResizeMarkerColor = metroPartColors.BaseColorDarker;
		_2215._21B2(_222F, ColorFactory.Empty, metroPartColors);
	}

	private static double _2230(double _2231, double ԝ)
	{
		if (ԝ < 0.0)
		{
			return _2231;
		}
		return ԝ;
	}

	private static Color _1933(Color _065B)
	{
		if (!(ColorFunctions.RGBToHSL(_065B).Lightness < 0.65))
		{
			return Color.Black;
		}
		return Color.White;
	}

	private static double _2232(double _2229)
	{
		if (_2229 <= 0.37777)
		{
			return Math.Min(1.0, _2229 + (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.37777)));
		}
		return Math.Max(0.0, _2229 - (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.6222222)));
	}

	private static MetroTabItemStateColorTable _2233(Color ໆ)
	{
		return _2233(ໆ, Color.Empty);
	}

	private static MetroTabItemStateColorTable _2233(Color ໆ, Color _2234)
	{
		ElementStyle elementStyle = new ElementStyle(ໆ);
		elementStyle.TextAlignment = eStyleTextAlignment.Center;
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		elementStyle.HideMnemonic = true;
		if (!_2234.IsEmpty)
		{
			elementStyle.BorderBottom = eStyleBorderType.Solid;
			elementStyle.BorderBottomWidth = 2;
			elementStyle.BorderBottomColor = _2234;
		}
		return new MetroTabItemStateColorTable(elementStyle);
	}
}
internal class _228D
{
	private const double _2225 = 1.0 / 360.0;

	public static MetroColorTable _2226(MetroColorGeneratorParameters _139F)
	{
		return _2226(_139F.CanvasColor, _139F.BaseColor);
	}

	public static MetroColorTable _2226(Color _2227, Color ॲ)
	{
		MetroColorTable metroColorTable = new MetroColorTable();
		Office2007ColorTable office2007ColorTable = new Office2007ColorTable();
		_1A76(metroColorTable, office2007ColorTable, _2227, ॲ);
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = office2007ColorTable;
		}
		return metroColorTable;
	}

	public static MetroPartColors _222D(Color _2227, Color ॲ)
	{
		ColorFunctions.HLSColor hLSColor = ColorFunctions.RGBToHSL(_2227);
		ColorFunctions.HLSColor hLSColor2 = ColorFunctions.RGBToHSL(ॲ);
		_1A78 obj = _1A7B._1A7C(ॲ);
		_1A78 obj2 = _1A7B._1A7C(_2227);
		MetroPartColors metroPartColors = new MetroPartColors();
		metroPartColors.CanvasColor = _2227;
		metroPartColors.BaseColor = ॲ;
		metroPartColors.TextColor = ((hLSColor.Lightness < 0.4) ? Color.White : Color.Black);
		metroPartColors.TextInactiveColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.53) : (obj2._༡ - 0.47));
		metroPartColors.TextDisabledColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.67));
		metroPartColors.TextLightColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.6) : (obj2._༡ + 0.4));
		metroPartColors.CanvasColorDarkShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.33));
		metroPartColors.CanvasColorLightShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.17) : (obj2._༡ - 0.17));
		metroPartColors.CanvasColorLighterShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.05) : (obj2._༡ - 0.05));
		metroPartColors.CanvasColorLight = ((metroPartColors.TextColor == Color.Black) ? _1A7B._1A7D(0.0, 0.0, 0.999) : _1A7B._1A7D(obj2._1A79, obj2._1A7A, obj2._༡ + 0.06));
		metroPartColors.BaseTextColor = _1933(ॲ);
		metroPartColors.BaseColorLight = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.41), obj._༡ + 0.3);
		metroPartColors.BaseColorLight1 = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.12), obj._༡ + 0.12);
		metroPartColors.BaseColorLightText = _1933(metroPartColors.BaseColorLight);
		metroPartColors.BaseColorLightest = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.6), obj._༡ + 0.5);
		metroPartColors.BaseColorLighter = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.46), obj._༡ + 0.39);
		metroPartColors.BaseColorDark = _1A7B._1A7D(obj._1A79, obj._1A7A + 0.1, obj._༡ - 0.06);
		metroPartColors.BaseColorDarker = ColorFunctions.HLSToRGB(hLSColor2.Hue, hLSColor2.Lightness - 0.2, hLSColor2.Saturation);
		metroPartColors.ComplementColor = _1A7B._1A7D(_2232(obj._1A79), obj._1A7A, obj._༡ + ((obj._༡ > 0.5) ? (-0.35) : 0.0));
		ColorFunctions.HLSColor hLSColor3 = ColorFunctions.RGBToHSL(metroPartColors.ComplementColor);
		_1A78 obj3 = _1A7B._1A7C(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLight = _1A7B._1A7D(obj3._1A79, obj3._1A7A, obj3._༡ + 0.2);
		metroPartColors.ComplementColorDark = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.1, hLSColor3.Saturation);
		metroPartColors.ComplementColorDarker = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.2, hLSColor3.Saturation);
		metroPartColors.ComplementColorText = _1933(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLightText = _1933(metroPartColors.ComplementColorLight);
		metroPartColors.BaseButtonGradientStart = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.01) : (obj2._༡ + 0.08));
		metroPartColors.BaseButtonGradientEnd = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.08) : (obj2._༡ + 0.2));
		if (metroPartColors.TextColor == Color.Black)
		{
			metroPartColors.EditControlBackColor = Color.White;
		}
		else
		{
			metroPartColors.EditControlBackColor = Color.Black;
		}
		return metroPartColors;
	}

	public static void _1A76(MetroColorTable _222E, Office2007ColorTable _222F, Color _2227, Color ॲ)
	{
		MetroPartColors metroPartColors = _222D(_2227, ॲ);
		_222E.CanvasColor = metroPartColors.CanvasColor;
		_222E.CanvasColorShadeLight = metroPartColors.CanvasColorLightShade;
		_222E.CanvasColorShadeLighter = metroPartColors.CanvasColorLighterShade;
		_222E.ForeColor = metroPartColors.TextColor;
		_222E.BaseColor = ॲ;
		_222E.EditControlBackColor = metroPartColors.EditControlBackColor;
		_222E.MetroPartColors = metroPartColors;
		_222E.MetroAppForm.BorderThickness = new Thickness(0.0, 0.0, 1.0, 1.0);
		_222E.MetroAppForm.BorderColors = new BorderColors(metroPartColors.BaseColor);
		_222E.MetroAppForm.BorderColorsInactive = new BorderColors(metroPartColors.BaseColorLight);
		_222E.MetroForm.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		_222E.MetroForm.BorderColors = new BorderColors[1]
		{
			new BorderColors(metroPartColors.BaseColor)
		};
		_222E.MetroForm.BorderColorsInactive = new BorderColors[4]
		{
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight)
		};
		_222E.MetroTab.ActiveCaptionText = metroPartColors.TextInactiveColor;
		_222E.MetroTab.InactiveCaptionText = metroPartColors.TextDisabledColor;
		_222E.MetroTab.MetroTabItem.Default = _2233(metroPartColors.TextInactiveColor);
		_222E.MetroTab.MetroTabItem.Selected = _2233(metroPartColors.BaseColor);
		_222E.MetroTab.MetroTabItem.Disabled = _2233(metroPartColors.TextDisabledColor);
		_222E.MetroTab.MetroTabItem.MouseOver = _2233(metroPartColors.TextInactiveColor, metroPartColors.BaseColor);
		_222E.MetroTab.MetroTabItem.Pressed = null;
		_222E.MetroTab.TabPanelBackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColor);
		_222E.MetroToolbar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColor);
		_222E.MetroStatusBar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.BaseColor);
		_222E.MetroStatusBar.TopBorders = new Color[0];
		_222E.MetroStatusBar.BottomBorders = new Color[0];
		_222E.MetroStatusBar.ResizeMarkerLightColor = Color.FromArgb(196, Color.White);
		_222E.MetroStatusBar.ResizeMarkerColor = metroPartColors.BaseColorDarker;
		_228E._21B2(_222F, ColorFactory.Empty, metroPartColors);
	}

	private static double _2230(double _2231, double ԝ)
	{
		if (ԝ < 0.0)
		{
			return _2231;
		}
		return ԝ;
	}

	private static Color _1933(Color _065B)
	{
		if (!(ColorFunctions.RGBToHSL(_065B).Lightness < 0.65))
		{
			return Color.Black;
		}
		return Color.White;
	}

	private static double _2232(double _2229)
	{
		if (_2229 <= 0.37777)
		{
			return Math.Min(1.0, _2229 + (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.37777)));
		}
		return Math.Max(0.0, _2229 - (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.6222222)));
	}

	private static MetroTabItemStateColorTable _2233(Color ໆ)
	{
		return _2233(ໆ, Color.Empty);
	}

	private static MetroTabItemStateColorTable _2233(Color ໆ, Color _2234)
	{
		ElementStyle elementStyle = new ElementStyle(ໆ);
		elementStyle.TextAlignment = eStyleTextAlignment.Center;
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		elementStyle.HideMnemonic = true;
		if (!_2234.IsEmpty)
		{
			elementStyle.BorderBottom = eStyleBorderType.Solid;
			elementStyle.BorderBottomWidth = 2;
			elementStyle.BorderBottomColor = _2234;
		}
		return new MetroTabItemStateColorTable(elementStyle);
	}
}
internal class _228E
{
	private static Office2007ButtonItemColorTable _21AE;

	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Color color = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.RibbonControl.InnerBorder = null;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(ጴ.CanvasColor));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(color);
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.KeyTips.KeyTipBackground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.KeyTips.KeyTipText = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.KeyTips.KeyTipBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ, ጴ, color);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ, ጴ, color);
		ቯ.RibbonBar.Expanded = _21B4(ቖ, ጴ, color);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _221A(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CB(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CD(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21B1.ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(_221B(ቖ, ጴ));
		ቯ.BackstageButtonItemColors.Clear();
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ, ጴ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "StatusBar"), _21BD(ቖ, ጴ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B6(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B8(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B9(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Bar.ToolbarBottomBackground = null;
		ቯ.Bar.ToolbarBottomBorder = Color.Empty;
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorderLight = LinearGradientColorTable.Empty;
		ቯ.Menu.SideUnused = LinearGradientColorTable.Empty;
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight), Color.Empty);
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.SystemButton.MouseOver.BottomBackground = null;
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = null;
		ቯ.SystemButton.MouseOver.InnerBorder = null;
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDarker));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = null;
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = null;
		ቯ.SystemButton.Pressed.InnerBorder = null;
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15208739));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = null;
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = null;
		ቯ.SystemButtonClose.MouseOver.InnerBorder = null;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15822970));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = null;
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = null;
		ቯ.SystemButtonClose.Pressed.InnerBorder = null;
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Active.CaptionBottomBorder = null;
		ቯ.Form.Active.CaptionText = ቖ.GetColor(ጴ.TextLightColor);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.Form.BackColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Form.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.Form.MdiClientBackgroundImage = null;
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.InnerBorder = Color.Empty;
		ቯ.TabControl.Default.OuterBorder = Color.Empty;
		ቯ.TabControl.Default.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1), Color.Empty);
		ቯ.TabControl.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.MouseOver.InnerBorder = Color.Empty;
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(ጴ.BaseColorLight1);
		ቯ.TabControl.MouseOver.Text = ጴ.TextColor;
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		ቯ.TabControl.Selected.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Selected.InnerBorder = Color.Empty;
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.TabControl.Selected.Text = ጴ.BaseTextColor;
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ጴ.CanvasColorLightShade;
		ቯ.DockTabStrip = new Office2007TabColorTable();
		ቯ.DockTabStrip.Default = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Default.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.Default.OuterBorder = Color.Empty;
		ቯ.DockTabStrip.Default.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.DockTabStrip.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.MouseOver.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.MouseOver.OuterBorder = Color.Empty;
		ቯ.DockTabStrip.MouseOver.Text = ጴ.BaseColor;
		ቯ.DockTabStrip.Selected = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.Selected.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Selected.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.Selected.OuterBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.DockTabStrip.Selected.Text = ጴ.BaseColor;
		ቯ.DockTabStrip.TabBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.DockTabStrip.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.TabPanelBorder = ጴ.CanvasColorLightShade;
		ቯ.CheckBoxItem = _2218(ቖ, ጴ);
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = _2218(ቖ, ጴ);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(Bar)) + "+StatusBar", office2007CheckBoxColorTable);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(MetroStatusBar)), office2007CheckBoxColorTable);
		ዱ(ቯ, ቖ, ጴ);
		ዳ(ቯ, ቖ, ጴ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(ጴ.CanvasColor);
		progressBarItem.OuterBorder = ቖ.GetColor(ጴ.BaseColorDarker);
		progressBarItem.InnerBorder = Color.Empty;
		progressBarItem.Chunk = new GradientColorTable(ጴ.BaseColor);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkShadow = new GradientColorTable();
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		gallery.GroupLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		gallery.GroupLabelBorder = Color.Empty;
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarBackground2 = Color.Empty;
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.BarCaptionBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.ItemExpandedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemExpandedBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.ItemHotBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemPressedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemSeparatorShade = Color.Empty;
		ቯ.LegacyColors.ItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBackground2 = Color.Empty;
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBorder = ቯ.LegacyColors.ItemExpandedBorder;
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = Color.Empty;
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.Black;
		ቯ.LegacyColors.BarDockedBorder = Color.Empty;
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground2 = Color.Empty;
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.PanelBackground2 = Color.Empty;
		ቯ.LegacyColors.PanelText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ExplorerBarBackground2 = Color.Empty;
		ቯ.LegacyColors.SplitterBackground = ቯ.LegacyColors.PanelBackground;
		ቯ.LegacyColors.SplitterBackground2 = ቯ.LegacyColors.PanelBackground2;
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.PanelText;
		ቯ.LegacyColors.SplitterBorder = ቯ.LegacyColors.PanelBorder;
		ቯ.LegacyColors.AutoHideTabBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.AutoHideTabText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.AutoHideTabBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.AutoHideSelectedTabBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.AutoHideSelectedTabBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.AutoHideSelectedTabText = ቖ.GetColor(ጴ.BaseColor);
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(ጴ.TextColor);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.PartBackground = new GradientColorTable();
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextDisabledColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextColor));
		slider.TrackPart.MouseOver.PartBorderColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBorderLightColor = Color.Empty;
		slider.TrackPart.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.Pressed = slider.TrackPart.MouseOver;
		slider = new Office2007SliderColorTable();
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.Default.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		slider.Pressed.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
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
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.BaseTextColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.BaseColorDarker);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver = slider.TrackPart.Default;
		slider.TrackPart.Pressed = slider.TrackPart.Default;
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(Bar)) + "+StatusBar", slider);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(MetroStatusBar)), slider);
		_2215.ቮ(ቯ, ቖ, ጴ);
		ቯ.ListViewEx.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = Color.Empty;
		ቯ.ListViewEx.SelectionForeColor = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.DataGridView.BackgroundColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.ColumnHeaderNormalText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.ColumnHeaderSelectedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.GridColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.SelectorMouseOverBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SideBar.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_221F(ቯ.AdvTree, ቖ, ጴ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.TextColor);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(ጴ.CanvasColor);
		Office2007CalendarViewColorTable calendarView = ቯ.CalendarView;
		ColorDef[] array = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		Color[] colors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		float[] cPositions = new float[1];
		array[2] = new ColorDef(colors, cPositions, 90f);
		array[3] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[4] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[5] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[6] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[7] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[8] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[9] = new ColorDef(ቖ.GetColor(ጴ.BaseColor));
		array[10] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[11] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[12] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[13] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[14] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[15] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[16] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[17] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[18] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[19] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[20] = new ColorDef(new Color[4]
		{
			ቖ.GetColor(16772473),
			ቖ.GetColor(16767083),
			ቖ.GetColor(16759552),
			ቖ.GetColor(16771703)
		}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f);
		array[21] = new ColorDef(ቖ.GetColor(15436032));
		calendarView.WeekDayViewColors = array;
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(ጴ.BaseButtonGradientStart),
				ቖ.GetColor(ጴ.BaseButtonGradientEnd)
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
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(ጴ.BaseColor);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor)));
		ቯ.SuperTabItem.Default.Normal.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Normal.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(ጴ.CanvasColorLighterShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColor)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColorLightShade)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጳ(ቯ.Backstage, ቖ, ጴ);
		ጦ.ጺ(ቯ.BackstageItem, ቖ, ጴ);
		ጦ.ፀ(ቯ.BackstagePanel, ቖ, ጴ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(ጴ.TextColor);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(ጴ.BaseColor);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(ጴ.BaseTextColor);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(ጴ.BaseColor);
		switchButtonColorTable.SwitchOnBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		switchButtonColorTable.SwitchBorderColor = Color.Empty;
		switchButtonColorTable.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.SwitchBorderColor = Color.Empty;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SwitchButton.Disabled.OffBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.OnBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 0;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BackColor = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _21B1.ጌ(ቖ, eOffice2010ColorScheme.Silver);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade), ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጙ(ጴ.ComplementColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ጴ.BaseColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(color);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(ጴ.BaseColorLightText), ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.BaseColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.LabelItemColors.Clear();
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(MetroStatusBar)), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar), "StatusBar"), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar)), new LabelColors(ቖ.GetColor(ጴ.TextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		_21B0(ቯ, ቖ, ጴ);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(Color.FromArgb(128, ጴ.ComplementColorLight));
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(ጴ.ComplementColor);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColorDarkShade) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.CanvasColorLight) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColorLight) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.BaseColor) }, 90, new float[0], ቖ.GetColor(ጴ.BaseTextColor), new Color[1] { ቖ.GetColor(ጴ.BaseColor) });
		ቯ.TokenEditor = new TokenEditorColorTable(ጴ.TextColor, ጴ.CanvasColorLighterShade, ጴ.TextColor, ጴ.CanvasColorLightShade, ጴ.BaseTextColor, ጴ.BaseColor);
		ቯ.Flyout.BorderColor = ጴ.BaseColor;
		ቯ.Flyout.BackColor = ጴ.CanvasColor;
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(ጴ.CanvasColor),
			Color.Empty,
			ቖ.GetColor(ጴ.CanvasColor)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(ጴ.BaseColor) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(ጴ.BaseColor)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(ጴ.TextColor);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(ጴ.TextLightColor);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.TextLightColor), new Color[1] { ቖ.GetColor(color) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.BaseColor)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	internal static Office2007ButtonItemColorTable _2216()
	{
		if (_21AE != null)
		{
			return _21AE;
		}
		MetroPartColors metroPartColors = _228D._222D(MetroColorGeneratorParameters.Default.CanvasColor, MetroColorGeneratorParameters.Default.BaseColor);
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
		_21AE = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	internal static void _21B0(Office2007ColorTable _139D, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTableCollection applicationButtonColors = _139D.ApplicationButtonColors;
		applicationButtonColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(13378923), ቖ.GetColor(11602001));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9701185));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(32, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14037618), ቖ.GetColor(11602770));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9767234));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(11865171), ቖ.GetColor(11537234));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371965), ቖ.GetColor(9766721));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(15889232), ቖ.GetColor(15029551));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16293442), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13380370), ቖ.GetColor(13579285));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16157012), ቖ.GetColor(16223038));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16285261), ቖ.GetColor(15095086));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16498014), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447703));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16493146), ቖ.GetColor(16562263));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14897949), ቖ.GetColor(14700057));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16625484), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447187));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15229481), ቖ.GetColor(16485700));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4560689), ቖ.GetColor(2850604));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(7064133), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598263), ቖ.GetColor(1993273));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(5022257), ቖ.GetColor(5485361));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4626228), ቖ.GetColor(2522155));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(9033320), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1664055), ቖ.GetColor(2190140));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(6666563), ቖ.GetColor(7392330));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(3113514), ቖ.GetColor(2784812));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(6867768), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598519), ቖ.GetColor(1927736));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3575595), ቖ.GetColor(5881133));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(1415061), ቖ.GetColor(98945));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2144180), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(484452));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(1811101), ቖ.GetColor(2077097));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(2071705), ቖ.GetColor(230788));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(3591626), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(616295));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3193011), ቖ.GetColor(3457726));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(164994), ቖ.GetColor(163969));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2933438), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(550245));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(167821), ቖ.GetColor(1946797));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(ጴ.TextColor);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		return ᱬ(ቖ, ጴ, _2217);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(180, Color.White)), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Transparent);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007CheckBoxColorTable _2218(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = new Office2007CheckBoxColorTable();
		office2007CheckBoxColorTable.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Default.CheckBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		office2007CheckBoxColorTable.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Default.CheckInnerBorder = ቖ.GetColor(ጴ.CanvasColor);
		office2007CheckBoxColorTable.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextLightColor), Color.Empty);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007CheckBoxColorTable.MouseOver.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007CheckBoxColorTable.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Disabled.CheckBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		office2007CheckBoxColorTable.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Disabled.CheckInnerBorder = Color.Empty;
		office2007CheckBoxColorTable.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		office2007CheckBoxColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		return office2007CheckBoxColorTable;
	}

	private static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _2219(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.CanvasColorLight);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _221A(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ጴ.CanvasColor);
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.BaseColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CB(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CC(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.ComplementColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.ComplementColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.ComplementColorText);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(4542298);
		Color expandLight = Color.FromArgb(192, Color.White);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = expandLight;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = expandLight;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = expandLight;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = expandLight;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = expandLight;
	}

	public static Office2007ButtonItemColorTable _221B(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueOrb);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Default.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Default.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseColorLight);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Checked.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Expanded.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1), Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.BaseColorLight1);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21BD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.BaseColorLight1);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextLightColor);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ጴ.CanvasColorLightShade);
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ጴ.CanvasColorLightShade);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColor);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15578584), ቖ.GetColor(16579579));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16579579), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16373742), ቖ.GetColor(16641528));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14891409));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11788003), ቖ.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13623521), ቖ.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle ጙ(Color _221C)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
		elementStyle.BackColor = _221C;
		elementStyle.TextColor = Color.White;
		return elementStyle;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 25;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
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

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.ScrollBar, ቖ, _221D);
	}

	public static void ዱ(Office2007ScrollBarColorTable _221E, ColorFactory ቖ, MetroPartColors _221D)
	{
		Office2007ScrollBarStateColorTable @default = _221E.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLight));
		@default.Border = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default = _221E.MouseOver;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default = _221E.MouseOverControl;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default.ThumbSignBackground = _221E.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default = _221E.Pressed;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default = _221E.Disabled;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextDisabledColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.AppScrollBar, ቖ, _221D);
	}

	public static void _221F(TreeColorTable ዅ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ጴ.CanvasColorLightShade);
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.TextColor));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0.4f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(ጴ.CanvasColorLightShade);
	}

	public static Office2007RibbonTabGroupColorTable _21B6(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16183804));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(9658067);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(9658067));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B9(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16775405));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(13589514);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(16751872));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B7(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16576759));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(13195676);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(13195676));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B8(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(15135718));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(4825929);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(4825929));
		return office2007RibbonTabGroupColorTable;
	}
}
internal class _228F
{
	private const double _2225 = 1.0 / 360.0;

	public static MetroColorTable _2226(MetroColorGeneratorParameters _139F)
	{
		return _2226(_139F.CanvasColor, _139F.BaseColor);
	}

	public static MetroColorTable _2226(Color _2227, Color ॲ)
	{
		MetroColorTable metroColorTable = new MetroColorTable();
		Office2007ColorTable office2007ColorTable = new Office2007ColorTable();
		_1A76(metroColorTable, office2007ColorTable, _2227, ॲ);
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = office2007ColorTable;
		}
		return metroColorTable;
	}

	public static MetroPartColors _222D(Color _2227, Color ॲ)
	{
		ColorFunctions.HLSColor hLSColor = ColorFunctions.RGBToHSL(_2227);
		ColorFunctions.HLSColor hLSColor2 = ColorFunctions.RGBToHSL(ॲ);
		_1A78 obj = _1A7B._1A7C(ॲ);
		_1A78 obj2 = _1A7B._1A7C(_2227);
		MetroPartColors metroPartColors = new MetroPartColors();
		metroPartColors.CanvasColor = _2227;
		metroPartColors.BaseColor = ॲ;
		metroPartColors.TextColor = ((hLSColor.Lightness < 0.4) ? Color.White : Color.Black);
		metroPartColors.TextInactiveColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.53) : (obj2._༡ - 0.47));
		metroPartColors.TextDisabledColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.67));
		metroPartColors.TextLightColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.6) : (obj2._༡ + 0.4));
		metroPartColors.CanvasColorDarkShade = ((metroPartColors.TextColor == Color.Black) ? ColorScheme.GetColor(13158600) : _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.33)));
		metroPartColors.CanvasColorDarkerShade = ColorScheme.GetColor(11842740);
		metroPartColors.CanvasColorLightShade = ColorScheme.GetColor(13421772);
		metroPartColors.CanvasColorLighterShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.05) : (obj2._༡ - 0.05));
		metroPartColors.CanvasColorLight = ((metroPartColors.TextColor == Color.Black) ? ColorScheme.GetColor(16448250) : _1A7B._1A7D(obj2._1A79, obj2._1A7A, obj2._༡ + 0.06));
		metroPartColors.BaseTextColor = _1933(ॲ);
		metroPartColors.BaseColorLight = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.41), obj._༡ + 0.3);
		metroPartColors.BaseColorLight1 = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.12), obj._༡ + 0.12);
		metroPartColors.BaseColorLightText = _1933(metroPartColors.BaseColorLight);
		metroPartColors.BaseColorLightest = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.6), obj._༡ + 0.5);
		metroPartColors.BaseColorLighter = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.46), obj._༡ + 0.39);
		metroPartColors.BaseColorDark = _1A7B._1A7D(obj._1A79, obj._1A7A + 0.1, obj._༡ - 0.06);
		metroPartColors.BaseColorDarker = ColorFunctions.HLSToRGB(hLSColor2.Hue, hLSColor2.Lightness - 0.2, hLSColor2.Saturation);
		metroPartColors.ComplementColor = _1A7B._1A7D(_2232(obj._1A79), obj._1A7A, obj._༡ + ((obj._༡ > 0.5) ? (-0.35) : 0.0));
		ColorFunctions.HLSColor hLSColor3 = ColorFunctions.RGBToHSL(metroPartColors.ComplementColor);
		_1A78 obj3 = _1A7B._1A7C(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLight = _1A7B._1A7D(obj3._1A79, obj3._1A7A, obj3._༡ + 0.2);
		metroPartColors.ComplementColorDark = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.1, hLSColor3.Saturation);
		metroPartColors.ComplementColorDarker = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.2, hLSColor3.Saturation);
		metroPartColors.ComplementColorText = _1933(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLightText = _1933(metroPartColors.ComplementColorLight);
		metroPartColors.BaseButtonGradientStart = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.01) : (obj2._༡ + 0.08));
		metroPartColors.BaseButtonGradientEnd = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.08) : (obj2._༡ + 0.2));
		if (metroPartColors.TextColor == Color.Black)
		{
			metroPartColors.EditControlBackColor = Color.White;
		}
		else
		{
			metroPartColors.EditControlBackColor = Color.Black;
		}
		return metroPartColors;
	}

	public static void _1A76(MetroColorTable _222E, Office2007ColorTable _222F, Color _2227, Color ॲ)
	{
		MetroPartColors metroPartColors = _222D(_2227, ॲ);
		_222E.CanvasColor = metroPartColors.CanvasColor;
		_222E.CanvasColorShadeLight = metroPartColors.CanvasColorLightShade;
		_222E.CanvasColorShadeLighter = metroPartColors.CanvasColorLighterShade;
		_222E.ForeColor = metroPartColors.TextColor;
		_222E.BaseColor = ॲ;
		_222E.EditControlBackColor = metroPartColors.EditControlBackColor;
		_222E.MetroPartColors = metroPartColors;
		_222E.MetroAppForm.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		_222E.MetroAppForm.BorderColors = new BorderColors(metroPartColors.BaseColor);
		_222E.MetroAppForm.BorderColorsInactive = new BorderColors(metroPartColors.BaseColorLight);
		_222E.MetroForm.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		_222E.MetroForm.BorderColors = new BorderColors[1]
		{
			new BorderColors(metroPartColors.BaseColor)
		};
		_222E.MetroForm.BorderColorsInactive = new BorderColors[4]
		{
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight)
		};
		_222E.MetroTab.ActiveCaptionText = metroPartColors.BaseTextColor;
		_222E.MetroTab.InactiveCaptionText = metroPartColors.BaseTextColor;
		_222E.MetroTab.MetroTabItem.Default = _2233(metroPartColors.BaseTextColor, Color.Empty);
		_222E.MetroTab.MetroTabItem.Selected = _2233(metroPartColors.BaseColor, metroPartColors.CanvasColorLight);
		_222E.MetroTab.MetroTabItem.Disabled = _2233(metroPartColors.TextDisabledColor, Color.Empty);
		_222E.MetroTab.MetroTabItem.MouseOver = null;
		_222E.MetroTab.MetroTabItem.Pressed = null;
		_222E.MetroTab.TabStrip.BackgroundStyle = new ElementStyle(metroPartColors.BaseTextColor, metroPartColors.BaseColor);
		_222E.MetroTab.CaptionTextFormat = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoPrefix | eTextFormat.VerticalCenter;
		_222E.MetroTab.TabPanelBackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColorLight);
		_222E.MetroToolbar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColorLight);
		_222E.MetroStatusBar.BackgroundStyle = new ElementStyle(metroPartColors.BaseTextColor, metroPartColors.BaseColor);
		_222E.MetroStatusBar.TopBorders = new Color[0];
		_222E.MetroStatusBar.BottomBorders = new Color[0];
		_222E.MetroStatusBar.ResizeMarkerLightColor = Color.FromArgb(196, Color.White);
		_222E.MetroStatusBar.ResizeMarkerColor = metroPartColors.BaseColorDarker;
		_2290._21B2(_222F, ColorFactory.Empty, metroPartColors);
	}

	private static double _2230(double _2231, double ԝ)
	{
		if (ԝ < 0.0)
		{
			return _2231;
		}
		return ԝ;
	}

	private static Color _1933(Color _065B)
	{
		if (!(ColorFunctions.RGBToHSL(_065B).Lightness < 0.65))
		{
			return Color.Black;
		}
		return Color.White;
	}

	private static double _2232(double _2229)
	{
		if (_2229 <= 0.37777)
		{
			return Math.Min(1.0, _2229 + (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.37777)));
		}
		return Math.Max(0.0, _2229 - (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.6222222)));
	}

	private static MetroTabItemStateColorTable _2233(Color ໆ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ໆ);
		elementStyle.TextAlignment = eStyleTextAlignment.Center;
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		elementStyle.HideMnemonic = true;
		if (!ጁ.IsEmpty)
		{
			elementStyle.CornerTypeTopRight = eCornerType.Rounded;
			elementStyle.CornerTypeTopLeft = eCornerType.Rounded;
			elementStyle.CornerDiameter = 2;
			elementStyle.BorderLeft = eStyleBorderType.Solid;
			elementStyle.BorderTop = eStyleBorderType.Solid;
			elementStyle.BorderRight = eStyleBorderType.Solid;
			elementStyle.BorderLeftWidth = 1;
			elementStyle.BorderTopWidth = 1;
			elementStyle.BorderRightWidth = 1;
			elementStyle.BorderColor = ጁ;
			elementStyle.BackColor = ጁ;
		}
		elementStyle.TextColor = ໆ;
		return new MetroTabItemStateColorTable(elementStyle);
	}
}
internal class _2290
{
	private static Office2007ButtonItemColorTable _21AE;

	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Color color = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.RibbonControl.InnerBorder = null;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(ጴ.CanvasColor));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(color);
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.KeyTips.KeyTipBackground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.KeyTips.KeyTipText = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.KeyTips.KeyTipBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ, ጴ, color);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ, ጴ, color);
		ቯ.RibbonBar.Expanded = _21B4(ቖ, ጴ, color);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _221A(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CB(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CD(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21B1.ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(_221B(ቖ, ጴ));
		ቯ.BackstageButtonItemColors.Clear();
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ, ጴ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "StatusBar"), _21BD(ቖ, ጴ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "MetroTabStrip"), _21BD(ቖ, ጴ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B6(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B7(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B8(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B9(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Bar.ToolbarBottomBackground = null;
		ቯ.Bar.ToolbarBottomBorder = Color.Empty;
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorderLight = LinearGradientColorTable.Empty;
		ቯ.Menu.SideUnused = LinearGradientColorTable.Empty;
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight), Color.Empty);
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.SystemButton.MouseOver.BottomBackground = null;
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = null;
		ቯ.SystemButton.MouseOver.InnerBorder = null;
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDarker));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = null;
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = null;
		ቯ.SystemButton.Pressed.InnerBorder = null;
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(Color.White);
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15208739));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = null;
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = null;
		ቯ.SystemButtonClose.MouseOver.InnerBorder = null;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15822970));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = null;
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = null;
		ቯ.SystemButtonClose.Pressed.InnerBorder = null;
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Active.CaptionBottomBorder = null;
		ቯ.Form.Active.CaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(ጴ.BaseColorLightText);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(ጴ.BaseColorLightText);
		ቯ.Form.BackColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Form.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.Form.MdiClientBackgroundImage = null;
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.InnerBorder = Color.Empty;
		ቯ.TabControl.Default.OuterBorder = Color.Empty;
		ቯ.TabControl.Default.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1), Color.Empty);
		ቯ.TabControl.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.MouseOver.InnerBorder = Color.Empty;
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(ጴ.BaseColorLight1);
		ቯ.TabControl.MouseOver.Text = ጴ.TextColor;
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		ቯ.TabControl.Selected.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Selected.InnerBorder = Color.Empty;
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.TabControl.Selected.Text = ጴ.BaseColor;
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ጴ.CanvasColorLightShade;
		ቯ.DockTabStrip = new Office2007TabColorTable();
		ቯ.DockTabStrip.Default = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Default.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.Default.OuterBorder = Color.Empty;
		ቯ.DockTabStrip.Default.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.DockTabStrip.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.MouseOver.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.MouseOver.OuterBorder = Color.Empty;
		ቯ.DockTabStrip.MouseOver.Text = ጴ.BaseColor;
		ቯ.DockTabStrip.Selected = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.Selected.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Selected.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.Selected.OuterBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.DockTabStrip.Selected.Text = ጴ.BaseColor;
		ቯ.DockTabStrip.TabBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.DockTabStrip.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.TabPanelBorder = ጴ.CanvasColorLightShade;
		ቯ.CheckBoxItem = _2218(ቖ, ጴ);
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = _2218(ቖ, ጴ);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(Bar)) + "+StatusBar", office2007CheckBoxColorTable);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(MetroStatusBar)), office2007CheckBoxColorTable);
		ዱ(ቯ, ቖ, ጴ);
		ዳ(ቯ, ቖ, ጴ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(ጴ.CanvasColor);
		progressBarItem.OuterBorder = ቖ.GetColor(ጴ.BaseColorDarker);
		progressBarItem.InnerBorder = Color.Empty;
		progressBarItem.Chunk = new GradientColorTable(ጴ.BaseColor);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkShadow = new GradientColorTable();
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		gallery.GroupLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		gallery.GroupLabelBorder = Color.Empty;
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.BarBackground2 = Color.Empty;
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.BarCaptionBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.ItemExpandedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemExpandedBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ItemHotBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemPressedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemSeparatorShade = Color.Empty;
		ቯ.LegacyColors.ItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBackground2 = Color.Empty;
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBorder = ቯ.LegacyColors.ItemExpandedBorder;
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = Color.Empty;
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.Black;
		ቯ.LegacyColors.BarDockedBorder = Color.Empty;
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground2 = Color.Empty;
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.PanelBackground2 = Color.Empty;
		ቯ.LegacyColors.PanelText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ExplorerBarBackground2 = Color.Empty;
		ቯ.LegacyColors.SplitterBackground = ቯ.LegacyColors.PanelBackground;
		ቯ.LegacyColors.SplitterBackground2 = ቯ.LegacyColors.PanelBackground2;
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.PanelText;
		ቯ.LegacyColors.SplitterBorder = ቯ.LegacyColors.PanelBorder;
		ቯ.LegacyColors.AutoHideTabBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.AutoHideTabText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.AutoHideTabBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.AutoHideSelectedTabBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.AutoHideSelectedTabBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.AutoHideSelectedTabText = ቖ.GetColor(ጴ.BaseColor);
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(ጴ.TextColor);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.PartBackground = new GradientColorTable();
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextColor));
		slider.TrackPart.MouseOver.PartBorderColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBorderLightColor = Color.Empty;
		slider.TrackPart.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.Pressed = slider.TrackPart.MouseOver;
		slider = new Office2007SliderColorTable();
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.Default.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		slider.Pressed.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
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
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.BaseTextColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.BaseColorDarker);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver = slider.TrackPart.Default;
		slider.TrackPart.Pressed = slider.TrackPart.Default;
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(Bar)) + "+StatusBar", slider);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(MetroStatusBar)), slider);
		_2215.ቮ(ቯ, ቖ, ጴ);
		ቯ.ListViewEx.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = Color.Empty;
		ቯ.ListViewEx.SelectionForeColor = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.DataGridView.BackgroundColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.ColumnHeaderNormalText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.ColumnHeaderSelectedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.GridColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.SelectorMouseOverBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SideBar.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_221F(ቯ.AdvTree, ቖ, ጴ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.TextColor);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(ጴ.CanvasColor);
		Office2007CalendarViewColorTable calendarView = ቯ.CalendarView;
		ColorDef[] array = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		Color[] colors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		float[] cPositions = new float[1];
		array[2] = new ColorDef(colors, cPositions, 90f);
		array[3] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[4] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[5] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[6] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[7] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[8] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[9] = new ColorDef(ቖ.GetColor(ጴ.BaseColor));
		array[10] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[11] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[12] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[13] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[14] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[15] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[16] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[17] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[18] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[19] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[20] = new ColorDef(new Color[4]
		{
			ቖ.GetColor(16772473),
			ቖ.GetColor(16767083),
			ቖ.GetColor(16759552),
			ቖ.GetColor(16771703)
		}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f);
		array[21] = new ColorDef(ቖ.GetColor(15436032));
		calendarView.WeekDayViewColors = array;
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(ጴ.BaseButtonGradientStart),
				ቖ.GetColor(ጴ.BaseButtonGradientEnd)
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
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(ጴ.BaseColor);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor)));
		ቯ.SuperTabItem.Default.Normal.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Normal.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(ጴ.CanvasColorLighterShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColor)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColorLightShade)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጳ(ቯ.Backstage, ቖ, ጴ);
		ጦ.ጺ(ቯ.BackstageItem, ቖ, ጴ);
		ጦ.ፀ(ቯ.BackstagePanel, ቖ, ጴ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(ጴ.TextColor);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(ጴ.BaseTextColor);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		switchButtonColorTable.SwitchOnBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		switchButtonColorTable.SwitchBorderColor = Color.Empty;
		switchButtonColorTable.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.SwitchBorderColor = Color.Empty;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SwitchButton.Disabled.OffBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.OnBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 0;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BackColor = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _21B1.ጌ(ቖ, eOffice2010ColorScheme.Silver);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade), ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጙ(ጴ.ComplementColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ጴ.BaseColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(color);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(ጴ.BaseColorLightText), ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.BaseColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.LabelItemColors.Clear();
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(MetroStatusBar)), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar)), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		_21B0(ቯ, ቖ, ጴ);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(Color.FromArgb(128, ጴ.ComplementColorLight));
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(ጴ.ComplementColor);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColorDarkShade) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.CanvasColorLight) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColorLight) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.BaseColor) }, 90, new float[0], ቖ.GetColor(ጴ.BaseTextColor), new Color[1] { ቖ.GetColor(ጴ.BaseColor) });
		ቯ.TokenEditor = new TokenEditorColorTable(ጴ.TextColor, ጴ.CanvasColorLighterShade, ጴ.TextColor, ጴ.CanvasColorLightShade, ጴ.BaseTextColor, ጴ.BaseColor);
		ቯ.Flyout.BorderColor = ጴ.BaseColor;
		ቯ.Flyout.BackColor = ጴ.CanvasColor;
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(ጴ.CanvasColor),
			Color.Empty,
			ቖ.GetColor(ጴ.CanvasColor)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(ጴ.BaseColor) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(ጴ.BaseColor)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.CaptionTextFormat |= eTextFormat.HorizontalCenter;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.BaseColor) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.BaseColor) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLighterShade) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.TextLightColor), new Color[1] { ቖ.GetColor(color) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.BaseColor)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	internal static Office2007ButtonItemColorTable _2216()
	{
		if (_21AE != null)
		{
			return _21AE;
		}
		MetroPartColors metroPartColors = _228D._222D(MetroColorGeneratorParameters.Default.CanvasColor, MetroColorGeneratorParameters.Default.BaseColor);
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
		_21AE = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	internal static void _21B0(Office2007ColorTable _139D, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTableCollection applicationButtonColors = _139D.ApplicationButtonColors;
		applicationButtonColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(13378923), ቖ.GetColor(11602001));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9701185));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(32, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14037618), ቖ.GetColor(11602770));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9767234));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(11865171), ቖ.GetColor(11537234));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371965), ቖ.GetColor(9766721));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(15889232), ቖ.GetColor(15029551));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16293442), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13380370), ቖ.GetColor(13579285));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16157012), ቖ.GetColor(16223038));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16285261), ቖ.GetColor(15095086));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16498014), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447703));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16493146), ቖ.GetColor(16562263));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14897949), ቖ.GetColor(14700057));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16625484), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447187));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15229481), ቖ.GetColor(16485700));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4560689), ቖ.GetColor(2850604));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(7064133), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598263), ቖ.GetColor(1993273));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(5022257), ቖ.GetColor(5485361));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4626228), ቖ.GetColor(2522155));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(9033320), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1664055), ቖ.GetColor(2190140));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(6666563), ቖ.GetColor(7392330));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(3113514), ቖ.GetColor(2784812));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(6867768), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598519), ቖ.GetColor(1927736));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3575595), ቖ.GetColor(5881133));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(1415061), ቖ.GetColor(98945));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2144180), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(484452));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(1811101), ቖ.GetColor(2077097));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(2071705), ቖ.GetColor(230788));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(3591626), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(616295));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3193011), ቖ.GetColor(3457726));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(164994), ቖ.GetColor(163969));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2933438), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(550245));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(167821), ቖ.GetColor(1946797));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(ጴ.TextColor);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		return ᱬ(ቖ, ጴ, _2217);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(180, Color.White)), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Transparent);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007CheckBoxColorTable _2218(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = new Office2007CheckBoxColorTable();
		office2007CheckBoxColorTable.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Default.CheckBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		office2007CheckBoxColorTable.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Default.CheckInnerBorder = ቖ.GetColor(ጴ.CanvasColor);
		office2007CheckBoxColorTable.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextLightColor), Color.Empty);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007CheckBoxColorTable.MouseOver.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007CheckBoxColorTable.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Disabled.CheckBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		office2007CheckBoxColorTable.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Disabled.CheckInnerBorder = Color.Empty;
		office2007CheckBoxColorTable.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		office2007CheckBoxColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		return office2007CheckBoxColorTable;
	}

	private static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(192, 16777215));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _2219(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.CanvasColorLightShade);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _221A(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ጴ.CanvasColor);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.BaseColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ጴ.BaseColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CB(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CC(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.ComplementColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.ComplementColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ጴ.ComplementColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.ComplementColorText);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(4542298);
		Color expandLight = Color.FromArgb(192, Color.White);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = expandLight;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = expandLight;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = expandLight;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = expandLight;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = expandLight;
	}

	public static Office2007ButtonItemColorTable _221B(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueOrb);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Default.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Default.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseColorLight);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Checked.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Expanded.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1), Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.BaseColorLight1);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21BD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.BaseColorLight1);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(ጴ.CanvasColor);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.CanvasColor);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15578584), ቖ.GetColor(16579579));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16579579), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16373742), ቖ.GetColor(16641528));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14891409));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11788003), ቖ.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13623521), ቖ.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle ጙ(Color _221C)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
		elementStyle.BackColor = _221C;
		elementStyle.TextColor = Color.White;
		return elementStyle;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 25;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
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

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.ScrollBar, ቖ, _221D);
	}

	public static void ዱ(Office2007ScrollBarColorTable _221E, ColorFactory ቖ, MetroPartColors _221D)
	{
		Office2007ScrollBarStateColorTable @default = _221E.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLight));
		@default.Border = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default = _221E.MouseOver;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default = _221E.MouseOverControl;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default.ThumbSignBackground = _221E.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default = _221E.Pressed;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = LinearGradientColorTable.Empty;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextColor));
		@default = _221E.Disabled;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextDisabledColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.AppScrollBar, ቖ, _221D);
	}

	public static void _221F(TreeColorTable ዅ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ጴ.CanvasColorLightShade);
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.TextColor));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0.4f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(ጴ.CanvasColorLightShade);
	}

	public static Office2007RibbonTabGroupColorTable _21B6(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(9658067);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(9658067));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B9(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(16751872);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(16751872));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B7(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(13195676);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(13195676));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B8(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(4825929);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(4825929));
		return office2007RibbonTabGroupColorTable;
	}
}
internal class _2291
{
	private const double _2225 = 1.0 / 360.0;

	public static MetroColorTable _2226(MetroColorGeneratorParameters _139F)
	{
		return _2226(_139F.CanvasColor, _139F.BaseColor);
	}

	public static MetroColorTable _2226(Color _2227, Color ॲ)
	{
		MetroColorTable metroColorTable = new MetroColorTable();
		Office2007ColorTable office2007ColorTable = new Office2007ColorTable();
		_1A76(metroColorTable, office2007ColorTable, _2227, ॲ);
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = office2007ColorTable;
		}
		return metroColorTable;
	}

	public static MetroPartColors _222D(Color _2227, Color ॲ)
	{
		ColorFunctions.HLSColor hLSColor = ColorFunctions.RGBToHSL(_2227);
		ColorFunctions.HLSColor hLSColor2 = ColorFunctions.RGBToHSL(ॲ);
		_1A78 obj = _1A7B._1A7C(ॲ);
		_1A78 obj2 = _1A7B._1A7C(_2227);
		MetroPartColors metroPartColors = new MetroPartColors();
		metroPartColors.CanvasColor = _2227;
		metroPartColors.BaseColor = ॲ;
		metroPartColors.TextColor = ((hLSColor.Lightness < 0.4) ? Color.White : Color.Black);
		metroPartColors.TextInactiveColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.53) : (obj2._༡ - 0.47));
		metroPartColors.TextDisabledColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.67));
		metroPartColors.TextLightColor = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.6) : (obj2._༡ + 0.4));
		metroPartColors.CanvasColorDarkShade = ((metroPartColors.TextColor == Color.Black) ? ColorScheme.GetColor(13158600) : _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.Black) ? (obj2._༡ - 0.33) : (obj2._༡ + 0.33)));
		metroPartColors.CanvasColorDarkerShade = ColorScheme.GetColor(11842740);
		metroPartColors.CanvasColorLightShade = ColorScheme.GetColor(13421772);
		metroPartColors.CanvasColorLighterShade = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (metroPartColors.TextColor == Color.White) ? (obj2._༡ + 0.08) : (obj2._༡ - 0.08));
		metroPartColors.CanvasColorLight = ((metroPartColors.TextColor == Color.Black) ? ColorScheme.GetColor(15856113) : _1A7B._1A7D(obj2._1A79, obj2._1A7A, obj2._༡ + 0.06));
		metroPartColors.BaseTextColor = _1933(ॲ);
		metroPartColors.BaseColorLight = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.41), obj._༡ + 0.3);
		metroPartColors.BaseColorLight1 = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.12), obj._༡ + 0.12);
		metroPartColors.BaseColorLightText = _1933(metroPartColors.BaseColorLight);
		metroPartColors.BaseColorLightest = _1A7B._1A7D(obj._1A79, _2230(0.05, obj._1A7A - 0.6), obj._༡ + 0.5);
		metroPartColors.BaseColorLighter = _1A7B._1A7D(obj._1A79, _2230(0.08, obj._1A7A - 0.46), obj._༡ + 0.39);
		metroPartColors.BaseColorDark = _1A7B._1A7D(obj._1A79, obj._1A7A + 0.1, obj._༡ - 0.06);
		metroPartColors.BaseColorDarker = ColorFunctions.HLSToRGB(hLSColor2.Hue, hLSColor2.Lightness - 0.2, hLSColor2.Saturation);
		metroPartColors.ComplementColor = _1A7B._1A7D(_2232(obj._1A79), obj._1A7A, obj._༡ + ((obj._༡ > 0.5) ? (-0.35) : 0.0));
		ColorFunctions.HLSColor hLSColor3 = ColorFunctions.RGBToHSL(metroPartColors.ComplementColor);
		_1A78 obj3 = _1A7B._1A7C(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLight = _1A7B._1A7D(obj3._1A79, obj3._1A7A, obj3._༡ + 0.2);
		metroPartColors.ComplementColorDark = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.1, hLSColor3.Saturation);
		metroPartColors.ComplementColorDarker = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.2, hLSColor3.Saturation);
		metroPartColors.ComplementColorText = _1933(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLightText = _1933(metroPartColors.ComplementColorLight);
		metroPartColors.BaseButtonGradientStart = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.01) : (obj2._༡ + 0.08));
		metroPartColors.BaseButtonGradientEnd = _1A7B._1A7D(obj2._1A79, obj2._1A7A, (obj2._༡ > 0.0) ? (obj2._༡ - 0.08) : (obj2._༡ + 0.2));
		if (metroPartColors.TextColor == Color.Black)
		{
			metroPartColors.EditControlBackColor = Color.White;
		}
		else
		{
			metroPartColors.EditControlBackColor = Color.Black;
		}
		return metroPartColors;
	}

	public static void _1A76(MetroColorTable _222E, Office2007ColorTable _222F, Color _2227, Color ॲ)
	{
		MetroPartColors metroPartColors = _222D(_2227, ॲ);
		_222E.CanvasColor = metroPartColors.CanvasColor;
		_222E.CanvasColorShadeLight = metroPartColors.CanvasColorLightShade;
		_222E.CanvasColorShadeLighter = metroPartColors.CanvasColorLighterShade;
		_222E.ForeColor = metroPartColors.TextColor;
		_222E.BaseColor = ॲ;
		_222E.EditControlBackColor = metroPartColors.EditControlBackColor;
		_222E.MetroPartColors = metroPartColors;
		_222E.MetroAppForm.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		_222E.MetroAppForm.BorderColors = new BorderColors(metroPartColors.BaseColor);
		_222E.MetroAppForm.BorderColorsInactive = new BorderColors(metroPartColors.BaseColorLight);
		_222E.MetroForm.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		_222E.MetroForm.BorderColors = new BorderColors[1]
		{
			new BorderColors(metroPartColors.BaseColor)
		};
		_222E.MetroForm.BorderColorsInactive = new BorderColors[4]
		{
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight)
		};
		_222E.MetroTab.ActiveCaptionText = metroPartColors.BaseTextColor;
		_222E.MetroTab.InactiveCaptionText = metroPartColors.BaseTextColor;
		_222E.MetroTab.MetroTabItem.Default = _2233(metroPartColors.BaseTextColor, Color.Empty);
		_222E.MetroTab.MetroTabItem.Selected = _2233(metroPartColors.BaseColor, metroPartColors.CanvasColorLight);
		_222E.MetroTab.MetroTabItem.Disabled = _2233(metroPartColors.TextDisabledColor, Color.Empty);
		_222E.MetroTab.MetroTabItem.MouseOver = null;
		_222E.MetroTab.MetroTabItem.Pressed = null;
		_222E.MetroTab.TabStrip.BackgroundStyle = new ElementStyle(metroPartColors.BaseTextColor, metroPartColors.BaseColor);
		_222E.MetroTab.CaptionTextFormat = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoPrefix | eTextFormat.VerticalCenter;
		_222E.MetroTab.TabPanelBackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColorLight);
		_222E.MetroToolbar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColorLight);
		_222E.MetroStatusBar.BackgroundStyle = new ElementStyle(metroPartColors.BaseTextColor, metroPartColors.BaseColor);
		_222E.MetroStatusBar.TopBorders = new Color[0];
		_222E.MetroStatusBar.BottomBorders = new Color[0];
		_222E.MetroStatusBar.ResizeMarkerLightColor = Color.FromArgb(196, Color.White);
		_222E.MetroStatusBar.ResizeMarkerColor = metroPartColors.BaseColorDarker;
		_2292._21B2(_222F, ColorFactory.Empty, metroPartColors);
	}

	private static double _2230(double _2231, double ԝ)
	{
		if (ԝ < 0.0)
		{
			return _2231;
		}
		return ԝ;
	}

	private static Color _1933(Color _065B)
	{
		if (!(ColorFunctions.RGBToHSL(_065B).Lightness < 0.65))
		{
			return Color.Black;
		}
		return Color.White;
	}

	private static double _2232(double _2229)
	{
		if (_2229 <= 0.37777)
		{
			return Math.Min(1.0, _2229 + (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.37777)));
		}
		return Math.Max(0.0, _2229 - (0.3805555555555556 + 43.0 / 180.0 * (_2229 / 0.6222222)));
	}

	private static MetroTabItemStateColorTable _2233(Color ໆ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ໆ);
		elementStyle.TextAlignment = eStyleTextAlignment.Center;
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		elementStyle.HideMnemonic = true;
		if (!ጁ.IsEmpty)
		{
			elementStyle.CornerTypeTopRight = eCornerType.Rounded;
			elementStyle.CornerTypeTopLeft = eCornerType.Rounded;
			elementStyle.CornerDiameter = 2;
			elementStyle.BorderLeft = eStyleBorderType.Solid;
			elementStyle.BorderTop = eStyleBorderType.Solid;
			elementStyle.BorderRight = eStyleBorderType.Solid;
			elementStyle.BorderLeftWidth = 1;
			elementStyle.BorderTopWidth = 1;
			elementStyle.BorderRightWidth = 1;
			elementStyle.BorderColor = ጁ;
			elementStyle.BackColor = ጁ;
		}
		elementStyle.TextColor = ໆ;
		return new MetroTabItemStateColorTable(elementStyle);
	}
}
internal class _2292
{
	private static Office2007ButtonItemColorTable _21AE;

	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Color color = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.RibbonControl.InnerBorder = null;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(ጴ.CanvasColor));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(color);
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.KeyTips.KeyTipBackground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.KeyTips.KeyTipText = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.KeyTips.KeyTipBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ, ጴ, color);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ, ጴ, color);
		ቯ.RibbonBar.Expanded = _21B4(ቖ, ጴ, color);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _221A(ቖ, ጴ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CB(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CD(ቖ, ጴ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21B1.ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(_221B(ቖ, ጴ));
		ቯ.BackstageButtonItemColors.Clear();
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ, ጴ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "StatusBar"), _21BD(ቖ, ጴ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "MetroTabStrip"), _21BD(ቖ, ጴ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "NavigationBar"), _2293(ቖ, ጴ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዑ(ቖ, ጴ, color);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B6(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B7(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B8(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B9(ቖ, ጴ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Bar.ToolbarBottomBackground = null;
		ቯ.Bar.ToolbarBottomBorder = Color.Empty;
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(ጴ.BaseColor);
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.Menu.SideBorderLight = LinearGradientColorTable.Empty;
		ቯ.Menu.SideUnused = LinearGradientColorTable.Empty;
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.DialogLauncher.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(ጴ.BaseColor);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		ቯ.DialogLauncher.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight), Color.Empty);
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		ቯ.SystemButton.MouseOver.BottomBackground = null;
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = null;
		ቯ.SystemButton.MouseOver.InnerBorder = null;
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = null;
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = null;
		ቯ.SystemButton.Pressed.InnerBorder = null;
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15208739));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = null;
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = null;
		ቯ.SystemButtonClose.MouseOver.InnerBorder = null;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(Color.Black));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15822970));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = null;
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = null;
		ቯ.SystemButtonClose.Pressed.InnerBorder = null;
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(ጴ.BaseColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColor)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Active.CaptionBottomBorder = null;
		ቯ.Form.Active.CaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(ጴ.BaseColorLightText);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(ጴ.BaseColorLightText);
		ቯ.Form.BackColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.Form.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.Form.MdiClientBackgroundImage = null;
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.InnerBorder = Color.Empty;
		ቯ.TabControl.Default.OuterBorder = Color.Empty;
		ቯ.TabControl.Default.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1), Color.Empty);
		ቯ.TabControl.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.MouseOver.InnerBorder = Color.Empty;
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(ጴ.BaseColorLight1);
		ቯ.TabControl.MouseOver.Text = ጴ.TextColor;
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		ቯ.TabControl.Selected.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Selected.InnerBorder = Color.Empty;
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.TabControl.Selected.Text = ጴ.BaseColor;
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ጴ.CanvasColorLightShade;
		ቯ.DockTabStrip = new Office2007TabColorTable();
		ቯ.DockTabStrip.Default = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Default.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.Default.OuterBorder = Color.Empty;
		ቯ.DockTabStrip.Default.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.DockTabStrip.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.MouseOver.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.MouseOver.OuterBorder = Color.Empty;
		ቯ.DockTabStrip.MouseOver.Text = ጴ.BaseColor;
		ቯ.DockTabStrip.Selected = new Office2007TabItemStateColorTable();
		ቯ.DockTabStrip.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.Selected.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.DockTabStrip.Selected.InnerBorder = Color.Empty;
		ቯ.DockTabStrip.Selected.OuterBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.DockTabStrip.Selected.Text = ጴ.BaseColor;
		ቯ.DockTabStrip.TabBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.DockTabStrip.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLight), Color.Empty);
		ቯ.DockTabStrip.TabPanelBorder = ጴ.CanvasColorLightShade;
		ቯ.CheckBoxItem = _2218(ቖ, ጴ);
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = _2218(ቖ, ጴ);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(Bar)) + "+StatusBar", office2007CheckBoxColorTable);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), typeof(MetroStatusBar)), office2007CheckBoxColorTable);
		ዱ(ቯ, ቖ, ጴ);
		ዳ(ቯ, ቖ, ጴ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(ጴ.CanvasColor);
		progressBarItem.OuterBorder = ቖ.GetColor(ጴ.BaseColorDarker);
		progressBarItem.InnerBorder = Color.Empty;
		progressBarItem.Chunk = new GradientColorTable(ጴ.BaseColor);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkShadow = new GradientColorTable();
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
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
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		gallery.GroupLabelText = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		gallery.GroupLabelBorder = Color.Empty;
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.BarBackground2 = Color.Empty;
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.BarCaptionBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.ItemExpandedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemExpandedBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ItemHotBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemPressedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.LegacyColors.ItemSeparatorShade = Color.Empty;
		ቯ.LegacyColors.ItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBackground2 = Color.Empty;
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuBorder = ቯ.LegacyColors.ItemExpandedBorder;
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = Color.Empty;
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.Black;
		ቯ.LegacyColors.BarDockedBorder = Color.Empty;
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground2 = Color.Empty;
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.PanelBackground2 = Color.Empty;
		ቯ.LegacyColors.PanelText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(ጴ.CanvasColorLight);
		ቯ.LegacyColors.ExplorerBarBackground2 = Color.Empty;
		ቯ.LegacyColors.SplitterBackground = ቯ.LegacyColors.PanelBackground;
		ቯ.LegacyColors.SplitterBackground2 = ቯ.LegacyColors.PanelBackground2;
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.PanelText;
		ቯ.LegacyColors.SplitterBorder = ቯ.LegacyColors.PanelBorder;
		ቯ.LegacyColors.AutoHideTabBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.AutoHideTabText = ቖ.GetColor(ጴ.TextColor);
		ቯ.LegacyColors.AutoHideTabBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.LegacyColors.AutoHideSelectedTabBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.LegacyColors.AutoHideSelectedTabBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.LegacyColors.AutoHideSelectedTabText = ቖ.GetColor(ጴ.BaseColor);
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(ጴ.TextColor);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.PartBackground = new GradientColorTable();
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextDisabledColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.TextColor));
		slider.TrackPart.MouseOver.PartBorderColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartBorderLightColor = Color.Empty;
		slider.TrackPart.MouseOver.PartForeColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.TrackPart.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.Pressed = slider.TrackPart.MouseOver;
		slider = new Office2007SliderColorTable();
		slider.Default.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBorderColor = Color.Empty;
		slider.Default.PartBorderLightColor = Color.Empty;
		slider.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.Default.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.MouseOver.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBorderColor = Color.Empty;
		slider.MouseOver.PartBorderLightColor = Color.Empty;
		slider.MouseOver.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.MouseOver.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.Pressed.LabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBorderColor = Color.Empty;
		slider.Pressed.PartBorderLightColor = Color.Empty;
		slider.Pressed.PartForeColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		slider.Pressed.PartForeLightColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.Pressed.TrackLineColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(ጴ.TextDisabledColor);
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
		slider.TrackPart = new Office2007SliderPartColorTable();
		slider.TrackPart.Default.LabelColor = ቖ.GetColor(ጴ.BaseTextColor);
		slider.TrackPart.Default.SliderLabelColor = ቖ.GetColor(ጴ.TextColor);
		slider.TrackPart.Default.PartBackground = new GradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		slider.TrackPart.Default.PartBorderColor = ቖ.GetColor(ጴ.BaseColorDarker);
		slider.TrackPart.Default.PartBorderLightColor = Color.Empty;
		slider.TrackPart.Default.PartForeColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.PartForeLightColor = ቖ.GetColor(ጴ.CanvasColor);
		slider.TrackPart.Default.TrackLineColor = ቖ.GetColor(ጴ.BaseColorDark);
		slider.TrackPart.Default.TrackLineLightColor = ቖ.GetColor(Color.FromArgb(48, Color.White));
		slider.TrackPart.MouseOver = slider.TrackPart.Default;
		slider.TrackPart.Pressed = slider.TrackPart.Default;
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(Bar)) + "+StatusBar", slider);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(MetroStatusBar)), slider);
		_2215.ቮ(ቯ, ቖ, ጴ);
		ቯ.ListViewEx.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = Color.Empty;
		ቯ.ListViewEx.SelectionForeColor = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.DataGridView.BackgroundColor = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.DataGridView.DefaultCellText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.ColumnHeaderNormalText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.ColumnHeaderSelectedText = ቖ.GetColor(ጴ.TextColor);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.GridColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.DataGridView.SelectorBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.DataGridView.SelectorMouseOverBorderDark = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverBorderLight = Color.Empty;
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.SideBar.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(ጴ.TextColor);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.CanvasColor), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_221F(ቯ.AdvTree, ቖ, ጴ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.TextColor);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColor), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(ጴ.BaseTextColor);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 0f),
			new BackgroundColorBlend(ቖ.GetColor(ጴ.BaseColorDark), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor(ጴ.BaseColorDark);
		crumbBarItemViewStateColorTable.BorderLight = Color.Empty;
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(ጴ.CanvasColor);
		Office2007CalendarViewColorTable calendarView = ቯ.CalendarView;
		ColorDef[] array = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		Color[] colors = new Color[1] { ቖ.GetColor(ጴ.CanvasColor) };
		float[] cPositions = new float[1];
		array[2] = new ColorDef(colors, cPositions, 90f);
		array[3] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[4] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[5] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[6] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[7] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[8] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[9] = new ColorDef(ቖ.GetColor(ጴ.BaseColor));
		array[10] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[11] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[12] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[13] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[14] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[15] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[16] = new ColorDef(ቖ.GetColor(ጴ.CanvasColor));
		array[17] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade));
		array[18] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		array[19] = new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade));
		array[20] = new ColorDef(new Color[4]
		{
			ቖ.GetColor(16772473),
			ቖ.GetColor(16767083),
			ቖ.GetColor(16759552),
			ቖ.GetColor(16771703)
		}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f);
		array[21] = new ColorDef(ቖ.GetColor(15436032));
		calendarView.WeekDayViewColors = array;
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColor)),
			new ColorDef(ቖ.GetColor(ጴ.BaseColor)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLighterShade)),
			new ColorDef(ቖ.GetColor(ጴ.TextColor)),
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorLightShade))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(ጴ.CanvasColorDarkShade)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(ጴ.BaseButtonGradientStart),
				ቖ.GetColor(ጴ.BaseButtonGradientEnd)
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
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(ጴ.BaseColor);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(ጴ.CanvasColor)));
		ቯ.SuperTabItem.Default.Normal.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Normal.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(ጴ.TextInactiveColor);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(ጴ.CanvasColorLighterShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColorLightShade),
			ቖ.GetColor(ጴ.CanvasColor)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(ጴ.CanvasColor),
			ቖ.GetColor(ጴ.CanvasColorLightShade)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ጴ.CanvasColorLightShade);
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጳ(ቯ.Backstage, ቖ, ጴ);
		ጦ.ጺ(ቯ.BackstageItem, ቖ, ጴ);
		ጦ.ፀ(ቯ.BackstagePanel, ቖ, ጴ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(ጴ.TextColor);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(ጴ.BaseTextColor);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		switchButtonColorTable.SwitchOnBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		switchButtonColorTable.SwitchBorderColor = Color.Empty;
		switchButtonColorTable.TextColor = ቖ.GetColor(ጴ.TextColor);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.SwitchBorderColor = Color.Empty;
		ቯ.SwitchButton.Disabled.OffTextColor = ቖ.GetColor(160, ጴ.TextColor);
		ቯ.SwitchButton.Disabled.OnTextColor = ቖ.GetColor(160, ጴ.TextColor);
		ቯ.SwitchButton.Disabled.TextColor = ቖ.GetColor(160, ጴ.TextColor);
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		ቯ.SwitchButton.Disabled.OffBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.SwitchButton.Disabled.OnBackColor = ቖ.GetColor(ጴ.CanvasColorLightShade);
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 0;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BackColor = ቖ.GetColor(ጴ.EditControlBackColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _21B1.ጌ(ቖ, eOffice2010ColorScheme.Silver);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(ጴ.CanvasColorDarkShade), ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Empty, ቖ.GetColor(ጴ.CanvasColorLighterShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade), ቖ.GetColor(ጴ.CanvasColorDarkShade));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጙ(ጴ.ComplementColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(ቖ.GetColor(ጴ.CanvasColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ጴ.BaseColor);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(ቖ.GetColor(16119543));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(ጴ.TextColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(ጴ.BaseColorLightText), ቖ.GetColor(ጴ.BaseColorLightest));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.BaseColor));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.LabelItemColors.Clear();
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(MetroStatusBar)), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar)), new LabelColors(ቖ.GetColor(ጴ.TextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar), "StatusBar"), new LabelColors(ቖ.GetColor(ጴ.BaseTextColor), ቖ.GetColor(ጴ.TextDisabledColor)));
		_21B0(ቯ, ቖ, ጴ);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ጴ.CanvasColorLighterShade);
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(Color.FromArgb(128, ጴ.ComplementColorLight));
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(ጴ.ComplementColor);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(ጴ.CanvasColor);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(ጴ.TextDisabledColor);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(ጴ.BaseColor);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColorDarkShade) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.CanvasColorLight) }, 90, new float[0], ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColorLight) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[1] { ቖ.GetColor(ጴ.BaseColor) }, 90, new float[0], ቖ.GetColor(ጴ.BaseTextColor), new Color[1] { ቖ.GetColor(ጴ.BaseColor) });
		ቯ.TokenEditor = new TokenEditorColorTable(ጴ.TextColor, ጴ.CanvasColorLighterShade, ጴ.TextColor, ጴ.CanvasColorLightShade, ጴ.BaseTextColor, ጴ.BaseColor);
		ቯ.Flyout.BorderColor = ጴ.BaseColor;
		ቯ.Flyout.BackColor = ጴ.CanvasColor;
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorLight) }, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.BaseColorLightText), new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, new Color[1] { ቖ.GetColor(ጴ.BaseColorDark) }, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(ጴ.TextColor), new Color[1] { ቖ.GetColor(ጴ.CanvasColor) }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(ጴ.CanvasColor),
			Color.Empty,
			ቖ.GetColor(ጴ.CanvasColor)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(ጴ.BaseColor) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(ጴ.BaseColorDark);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(ጴ.BaseColor)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.CaptionTextFormat = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoPrefix | eTextFormat.VerticalCenter;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(ጴ.BaseTextColor);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.BaseColor) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(ጴ.BaseColor) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLighterShade) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.TextLightColor), new Color[1] { ቖ.GetColor(color) }, new Color[1] { ቖ.GetColor(ጴ.CanvasColorLightShade) }, ቖ.GetColor(ጴ.BaseColor)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	internal static Office2007ButtonItemColorTable _2216()
	{
		if (_21AE != null)
		{
			return _21AE;
		}
		MetroPartColors metroPartColors = _228D._222D(MetroColorGeneratorParameters.Default.CanvasColor, MetroColorGeneratorParameters.Default.BaseColor);
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
		_21AE = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	internal static void _21B0(Office2007ColorTable _139D, ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTableCollection applicationButtonColors = _139D.ApplicationButtonColors;
		applicationButtonColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.OuterBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(13378923), ቖ.GetColor(11602001));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9701185));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(32, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14037618), ቖ.GetColor(11602770));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371964), ቖ.GetColor(9767234));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(11865171), ቖ.GetColor(11537234));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9371965), ቖ.GetColor(9766721));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(15889232), ቖ.GetColor(15029551));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16293442), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13380370), ቖ.GetColor(13579285));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16157012), ቖ.GetColor(16223038));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(16285261), ቖ.GetColor(15095086));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16498014), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447703));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16493146), ቖ.GetColor(16562263));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(14897949), ቖ.GetColor(14700057));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16625484), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13248528), ቖ.GetColor(13447187));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15229481), ቖ.GetColor(16485700));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4560689), ቖ.GetColor(2850604));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(7064133), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598263), ቖ.GetColor(1993273));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(5022257), ቖ.GetColor(5485361));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4626228), ቖ.GetColor(2522155));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(9033320), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1664055), ቖ.GetColor(2190140));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(6666563), ቖ.GetColor(7392330));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(3113514), ቖ.GetColor(2784812));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(6867768), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1598519), ቖ.GetColor(1927736));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3575595), ቖ.GetColor(5881133));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(1415061), ቖ.GetColor(98945));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2144180), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(484452));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(1811101), ቖ.GetColor(2077097));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(2071705), ቖ.GetColor(230788));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(3591626), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(616295));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(3193011), ቖ.GetColor(3457726));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(164994), ቖ.GetColor(163969));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(2933438), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(351838), ቖ.GetColor(550245));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(167821), ቖ.GetColor(1946797));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(ጴ.TextColor);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		return ᱬ(ቖ, ጴ, _2217);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ቖ.GetColor(180, Color.White)), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(_2217);
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLighterShade), Color.Transparent);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007CheckBoxColorTable _2218(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = new Office2007CheckBoxColorTable();
		office2007CheckBoxColorTable.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Default.CheckBorder = ቖ.GetColor(ጴ.TextDisabledColor);
		office2007CheckBoxColorTable.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Default.CheckInnerBorder = ቖ.GetColor(ጴ.CanvasColor);
		office2007CheckBoxColorTable.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextLightColor), Color.Empty);
		office2007CheckBoxColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007CheckBoxColorTable.MouseOver.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007CheckBoxColorTable.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007CheckBoxColorTable.Pressed.CheckInnerBorder = ቖ.GetColor(ጴ.BaseColorLight);
		office2007CheckBoxColorTable.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.TextColor), Color.Empty);
		office2007CheckBoxColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007CheckBoxColorTable.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), Color.Empty);
		office2007CheckBoxColorTable.Disabled.CheckBorder = ቖ.GetColor(ጴ.CanvasColorLightShade);
		office2007CheckBoxColorTable.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007CheckBoxColorTable.Disabled.CheckInnerBorder = Color.Empty;
		office2007CheckBoxColorTable.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade), Color.Empty);
		office2007CheckBoxColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorDarkShade);
		return office2007CheckBoxColorTable;
	}

	private static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(192, 16777215));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _2219(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.CanvasColorLightShade);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkerShade));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _221A(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _2219(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ጴ.CanvasColor);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CA(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.BaseColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ጴ.BaseColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CB(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CA(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CC(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorLight));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.ComplementColorLightText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ጴ.ComplementColorDarker);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ጴ.ComplementColorDark);
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.ComplementColorText);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CC(ቖ, ጴ);
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.ComplementColor));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.ComplementColorText);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColorLightShade), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.CanvasColorLightShade);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(4542298);
		Color expandLight = Color.FromArgb(192, Color.White);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = expandLight;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = expandLight;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = expandLight;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = expandLight;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = expandLight;
	}

	public static Office2007ButtonItemColorTable _221B(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueOrb);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.TopBackground = null;
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.BottomBackground = null;
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Default.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Default.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Default.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Default.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.MouseOver.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseColorLight);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseTextColor));
		office2007ButtonItemColorTable.Checked.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLightest));
		office2007ButtonItemColorTable.Expanded.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseColorLightest);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = null;
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.BottomBackground = null;
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Disabled.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1), Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorderWidth = 2;
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(ጴ.BaseColorLight1);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21BD(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.BaseColorLight1);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _2293(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(ጴ.TextColor);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = null;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = null;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ጴ.BaseColorLight1);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.TopBackground = null;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.BottomBackground = null;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		office2007ButtonItemColorTable.Checked.TopBackground = null;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.BottomBackground = null;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(ጴ.BaseTextColor);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.TopBackground = null;
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.BottomBackground = null;
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = null;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ, MetroPartColors ጴ, Color _2217)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(ጴ.CanvasColor);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable();
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(_2217);
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(ጴ.BaseColor);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(ጴ.CanvasColor);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15578584), ቖ.GetColor(16579579));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16579579), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16373742), ቖ.GetColor(16641528));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14891409));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11788003), ቖ.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.CornerSize = 0;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3881787);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13623521), ቖ.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3881787);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle ጙ(Color _221C)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
		elementStyle.BackColor = _221C;
		elementStyle.TextColor = Color.White;
		return elementStyle;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 25;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
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

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.ScrollBar, ቖ, _221D);
	}

	public static void ዱ(Office2007ScrollBarColorTable _221E, ColorFactory ቖ, MetroPartColors _221D)
	{
		Office2007ScrollBarStateColorTable @default = _221E.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColor));
		@default.Border = LinearGradientColorTable.Empty;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColor));
		@default = _221E.MouseOver;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLighterShade));
		@default = _221E.MouseOverControl;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.ThumbSignBackground = _221E.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLightShade));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColor));
		@default = _221E.Pressed;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(_221D.TextInactiveColor));
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.CanvasColorLighterShade));
		@default = _221E.Disabled;
		@default.Background = _221E.Default.Background;
		@default.Border = _221E.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(_221D.TextDisabledColor));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ, MetroPartColors _221D)
	{
		ዱ(ዲ.AppScrollBar, ቖ, _221D);
	}

	public static void _221F(TreeColorTable ዅ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ጴ.CanvasColorLightShade);
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.CanvasColorLightShade));
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(ጴ.BaseColor));
		selectionColorTable.TextColor = ቖ.GetColor(ጴ.BaseTextColor);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.TextColor));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(ጴ.CanvasColorDarkShade), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(ጴ.CanvasColor));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(ጴ.CanvasColorDarkShade));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(ጴ.TextColor), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColor), 0.4f),
			new ColorStop(ቖ.GetColor(ጴ.CanvasColorLighterShade), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(ጴ.CanvasColorLightShade);
	}

	public static Office2007RibbonTabGroupColorTable _21B6(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(ጴ.BaseColorLighter);
		office2007RibbonTabGroupColorTable.Border = LinearGradientColorTable.Empty;
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B9(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(ጴ.BaseColorLighter);
		office2007RibbonTabGroupColorTable.Border = LinearGradientColorTable.Empty;
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B7(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(ጴ.BaseColorLighter);
		office2007RibbonTabGroupColorTable.Border = LinearGradientColorTable.Empty;
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B8(ColorFactory ቖ, MetroPartColors ጴ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(ጴ.BaseColorLighter);
		office2007RibbonTabGroupColorTable.Border = LinearGradientColorTable.Empty;
		return office2007RibbonTabGroupColorTable;
	}
}
