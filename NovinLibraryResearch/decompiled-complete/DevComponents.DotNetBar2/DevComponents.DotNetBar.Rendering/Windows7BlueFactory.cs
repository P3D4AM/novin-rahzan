using System;
using System.Drawing;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Rendering;

public class Windows7BlueFactory
{
	public static void InitializeBlueColorTable(Office2007ColorTable table, ColorFactory factory)
	{
		table.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankWin7NormalBlue.png");
		table.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankWin7HotBlue.png");
		table.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankWin7PressedBlue.png");
		table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(12241371), factory.GetColor(10464962));
		table.RibbonControl.InnerBorder = null;
		table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(14674421), Color.Empty);
		table.RibbonControl.TabDividerBorder = factory.GetColor(10992337);
		table.RibbonControl.TabDividerBorderLight = factory.GetColor(16054525);
		table.RibbonControl.CornerSize = 1;
		table.RibbonControl.PanelTopBackgroundHeight = 0f;
		table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(16251903), factory.GetColor(14477301));
		table.RibbonControl.PanelBottomBackground = null;
		table.ItemGroup.OuterBorder = new LinearGradientColorTable(factory.GetColor(11451856), factory.GetColor(11583701));
		table.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		table.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		table.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		table.ItemGroup.ItemGroupDividerDark = Color.Empty;
		table.ItemGroup.ItemGroupDividerLight = Color.Empty;
		table.RibbonBar.Default = GetRibbonBar(factory);
		table.RibbonBar.MouseOver = GetRibbonBar(factory);
		table.RibbonBar.Expanded = ዂ.ዐ(factory);
		table.RibbonButtonItemColors.Clear();
		table.ButtonItemColors.Clear();
		table.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable buttonItemBlueOrange = GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = GetButtonItemBlueOrangeWithBackground(factory);
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = ዂ.ዉ(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11386322));
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = ዂ.ዊ(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11386322));
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = ዂ.ዋ(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11386322));
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = ዂ.ዌ(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11386322));
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		buttonItemBlueOrange = ዂ.ው(factory);
		buttonItemBlueOrange.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11386322));
		buttonItemBlueOrange.MouseOverSplitInactive = buttonItemBlueOrange.Default;
		buttonItemBlueOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		table.ButtonItemColors.Add(buttonItemBlueOrange);
		table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
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
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = ዂ.ዕ(factory);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		table.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዂ.ዖ(factory);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		table.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዂ._12D7(factory);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		table.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዂ.ዘ(factory);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		table.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(16580351), factory.GetColor(15266299));
		table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(14477044), factory.GetColor(14543351));
		table.Bar.ToolbarBottomBorder = factory.GetColor(10530755);
		table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(16579836), Color.Empty);
		int color = 8688294;
		table.Bar.PopupToolbarBorder = factory.GetColor(color);
		table.Bar.StatusBarTopBorder = factory.GetColor(10530755);
		table.Bar.StatusBarTopBorderLight = factory.GetColor(Color.FromArgb(148, Color.White));
		table.Bar.StatusBarAltBackground.Clear();
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(16251903), 0f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(14477301), 1f));
		table.Menu.Background = new LinearGradientColorTable(factory.GetColor(16579836), Color.Empty);
		table.Menu.Border = new LinearGradientColorTable(factory.GetColor(color), Color.Empty);
		table.Menu.Side = new LinearGradientColorTable(factory.GetColor(15988731), Color.Empty);
		table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(13622251), Color.Empty);
		table.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(15066597), Color.Empty);
		table.Menu.FileBackgroundBlend.Clear();
		table.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16580351), 0f),
			new BackgroundColorBlend(factory.GetColor(15266299), 10f),
			new BackgroundColorBlend(factory.GetColor(14477044), 10f),
			new BackgroundColorBlend(factory.GetColor(14543351), 1f)
		});
		table.Menu.FileContainerBorder = factory.GetColor(11253961);
		table.Menu.FileContainerBorderLight = Color.Transparent;
		table.Menu.FileColumnOneBackground = factory.GetColor(16579836);
		table.Menu.FileColumnOneBorder = factory.GetColor(13622251);
		table.Menu.FileColumnTwoBackground = factory.GetColor(15726074);
		table.Menu.FileBottomContainerBackgroundBlend.Clear();
		table.Menu.FileBottomContainerBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(16449023), 0f),
			new BackgroundColorBlend(factory.GetColor(14871289), 1f)
		});
		table.ComboBox.Default.Background = factory.GetColor(15988732);
		table.ComboBox.Default.Border = factory.GetColor(11189202);
		table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandText = factory.GetColor(5005434);
		table.ComboBox.DefaultStandalone.Background = factory.GetColor(16777215);
		table.ComboBox.DefaultStandalone.Border = factory.GetColor(10464962);
		table.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(15331575), factory.GetColor(14148079), 90);
		table.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(16054010));
		table.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(11977943), Color.Empty, 90);
		table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(5005434);
		table.ComboBox.MouseOver.Background = factory.GetColor(16777215);
		table.ComboBox.MouseOver.Border = factory.GetColor(10464962);
		table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(16640209), factory.GetColor(16765288), 90);
		table.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(16776435), 90);
		table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(16760595), Color.Empty, 90);
		table.ComboBox.MouseOver.ExpandText = factory.GetColor(5005434);
		table.ComboBox.DroppedDown.Background = factory.GetColor(16777215);
		table.ComboBox.DroppedDown.Border = factory.GetColor(10464962);
		table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(14993831), factory.GetColor(15711329), 90);
		table.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(13942953), factory.GetColor(15724985), 90);
		table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(12751419), Color.Empty, 90);
		table.ComboBox.DroppedDown.ExpandText = factory.GetColor(5005434);
		table.DialogLauncher.Default.DialogLauncher = factory.GetColor(7041917);
		table.DialogLauncher.Default.DialogLauncherShade = factory.GetColor(172, 16777215);
		table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(7041917);
		table.DialogLauncher.MouseOver.DialogLauncherShade = Color.FromArgb(192, factory.GetColor(16777215));
		table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16641758), factory.GetColor(16638141));
		table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16765551), factory.GetColor(16774553));
		table.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16710131), factory.GetColor(16777203));
		table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(16758528), Color.Empty);
		table.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(7041917);
		table.DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(128, factory.GetColor(16777215));
		table.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(14993573), factory.GetColor(15583377));
		table.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(15645794), factory.GetColor(15656075));
		table.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(13942696), factory.GetColor(15724994));
		table.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(12751419), factory.GetColor(12760162));
		table.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(7966119), factory.GetColor(9152460));
		table.SystemButton.Default.LightShade = factory.GetColor(16316922);
		table.SystemButton.Default.DarkShade = factory.GetColor(6645093);
		table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(7966119), factory.GetColor(9152460));
		table.SystemButton.MouseOver.LightShade = factory.GetColor(16316922);
		table.SystemButton.MouseOver.DarkShade = factory.GetColor(6645093);
		table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16120062), factory.GetColor(15397631));
		table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(13821182), factory.GetColor(13755131));
		table.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(factory.GetColor(16514303), Color.Transparent);
		table.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(factory.GetColor(16514303), Color.Transparent);
		table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12702958), factory.GetColor(11191529));
		table.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16711423), factory.GetColor(15135998));
		table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(7966119), factory.GetColor(9152460));
		table.SystemButton.Pressed.LightShade = factory.GetColor(16316922);
		table.SystemButton.Pressed.DarkShade = factory.GetColor(6645093);
		table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(12244472), factory.GetColor(10404589));
		table.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(factory.GetColor(12111593), Color.Transparent);
		table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(8696553), factory.GetColor(11523831));
		table.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(factory.GetColor(13036285), Color.Transparent);
		table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(10600421), factory.GetColor(11717351));
		table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(13887231), factory.GetColor(14477819));
		table.Form.Active.BorderColors = new Color[5]
		{
			factory.GetColor(5920857),
			factory.GetColor(13952757),
			factory.GetColor(14674421),
			factory.GetColor(14674421),
			factory.GetColor(14674421)
		};
		table.Form.Inactive.BorderColors = new Color[5]
		{
			factory.GetColor(12633806),
			factory.GetColor(13424354),
			factory.GetColor(14674421),
			factory.GetColor(14674421),
			factory.GetColor(14674421)
		};
		table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(16251903), factory.GetColor(14674421));
		table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(14674421), factory.GetColor(14674421));
		table.Form.Active.CaptionBottomBorder = new Color[2]
		{
			factory.GetColor(10464962),
			factory.GetColor(13556715)
		};
		table.Form.Active.CaptionText = factory.GetColor(1909278);
		table.Form.Active.CaptionTextExtra = factory.GetColor(2441828);
		table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(14936044), factory.GetColor(14607853));
		table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(14213612), factory.GetColor(14936303));
		table.Form.Inactive.CaptionText = factory.GetColor(10526880);
		table.Form.Inactive.CaptionTextExtra = factory.GetColor(10526880);
		table.Form.BackColor = factory.GetColor(16579836);
		table.Form.TextColor = factory.GetColor(0);
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
		table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(14674421));
		table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(11714514);
		table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(15463159);
		table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(5005434);
		table.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		table.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		table.TabControl.Default = new Office2007TabItemStateColorTable();
		table.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		table.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		table.TabControl.Default.InnerBorder = factory.GetColor(15988733);
		table.TabControl.Default.OuterBorder = factory.GetColor(9610695);
		table.TabControl.Default.Text = factory.GetColor(5005434);
		table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776683), factory.GetColor(16772264));
		table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767577), factory.GetColor(16770701));
		table.TabControl.MouseOver.InnerBorder = factory.GetColor(16777211);
		table.TabControl.MouseOver.OuterBorder = factory.GetColor(11967859);
		table.TabControl.MouseOver.Text = factory.GetColor(5005434);
		table.TabControl.Selected = new Office2007TabItemStateColorTable();
		table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.White), factory.GetColor(16645630));
		table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(16645630), factory.GetColor(16645630));
		table.TabControl.Selected.InnerBorder = factory.GetColor(Color.White);
		table.TabControl.Selected.OuterBorder = factory.GetColor(9610695);
		table.TabControl.Selected.Text = factory.GetColor(5005434);
		table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(14674421), Color.Empty);
		table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(16251903), Color.Empty);
		table.TabControl.TabPanelBorder = factory.GetColor(9610695);
		Office2007CheckBoxColorTable checkBoxItem = table.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = factory.GetColor(11256286);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(10661049)), Color.FromArgb(164, factory.GetColor(16185078)));
		checkBoxItem.Default.CheckInnerBorder = factory.GetColor(10661049);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.Default.Text = factory.GetColor(2441828);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(14609146), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = factory.GetColor(5601187);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(16438650)), Color.FromArgb(128, factory.GetColor(16709863)));
		checkBoxItem.MouseOver.CheckInnerBorder = factory.GetColor(16438650);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.MouseOver.Text = factory.GetColor(2441828);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(12703989), Color.Empty);
		checkBoxItem.Pressed.CheckBorder = factory.GetColor(5601187);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(15894822)), Color.FromArgb(164, factory.GetColor(16774357)));
		checkBoxItem.Pressed.CheckInnerBorder = factory.GetColor(15894822);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(4877206), Color.Empty);
		checkBoxItem.Pressed.Text = factory.GetColor(2441828);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = factory.GetColor(11448757);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(14738149)), Color.FromArgb(164, factory.GetColor(16514043)));
		checkBoxItem.Disabled.CheckInnerBorder = factory.GetColor(14738149);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(9276813), Color.Empty);
		checkBoxItem.Disabled.Text = factory.GetColor(9276813);
		ዂ.ዱ(table, factory);
		ዂ.ዳ(table, factory);
		Office2007ProgressBarColorTable progressBarItem = table.ProgressBarItem;
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
		progressBarItem = table.ProgressBarItemPaused;
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
		progressBarItem = table.ProgressBarItemError;
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
		Office2007GalleryColorTable gallery = table.Gallery;
		gallery.GroupLabelBackground = factory.GetColor(14542830);
		gallery.GroupLabelText = factory.GetColor(5486);
		gallery.GroupLabelBorder = factory.GetColor(12961221);
		table.LegacyColors.BarBackground = factory.GetColor(16186367);
		table.LegacyColors.BarBackground2 = factory.GetColor(14477044);
		table.LegacyColors.BarStripeColor = factory.GetColor(10464962);
		table.LegacyColors.BarCaptionBackground = factory.GetColor(9213863);
		table.LegacyColors.BarCaptionBackground2 = Color.Empty;
		table.LegacyColors.BarCaptionInactiveBackground = factory.GetColor(15726333);
		table.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		table.LegacyColors.BarCaptionInactiveText = factory.GetColor(538482);
		table.LegacyColors.BarCaptionText = factory.GetColor(16777215);
		table.LegacyColors.BarFloatingBorder = factory.GetColor(3630240);
		table.LegacyColors.BarPopupBackground = factory.GetColor(16185078);
		table.LegacyColors.BarPopupBorder = factory.GetColor(color);
		table.LegacyColors.ItemBackground = Color.Empty;
		table.LegacyColors.ItemBackground2 = Color.Empty;
		table.LegacyColors.ItemCheckedBackground = factory.GetColor(16569720);
		table.LegacyColors.ItemCheckedBackground2 = factory.GetColor(16500815);
		table.LegacyColors.ItemCheckedBorder = factory.GetColor(12276995);
		table.LegacyColors.ItemCheckedText = factory.GetColor(2441828);
		table.LegacyColors.ItemDisabledBackground = Color.Empty;
		table.LegacyColors.ItemDisabledText = factory.GetColor(9276813);
		table.LegacyColors.ItemExpandedShadow = Color.Empty;
		table.LegacyColors.ItemExpandedBackground = factory.GetColor(14938110);
		table.LegacyColors.ItemExpandedBackground2 = factory.GetColor(10076144);
		table.LegacyColors.ItemExpandedText = factory.GetColor(2441828);
		table.LegacyColors.ItemHotBackground = factory.GetColor(16774604);
		table.LegacyColors.ItemHotBackground2 = factory.GetColor(16767861);
		table.LegacyColors.ItemHotBorder = factory.GetColor(16760169);
		table.LegacyColors.ItemHotText = factory.GetColor(2441828);
		table.LegacyColors.ItemPressedBackground = factory.GetColor(16553789);
		table.LegacyColors.ItemPressedBackground2 = factory.GetColor(16758878);
		table.LegacyColors.ItemPressedBorder = factory.GetColor(16485436);
		table.LegacyColors.ItemPressedText = factory.GetColor(2441828);
		table.LegacyColors.ItemSeparator = factory.GetColor(160, 5263440);
		table.LegacyColors.ItemSeparatorShade = Color.FromArgb(96, factory.GetColor(16777215));
		table.LegacyColors.ItemText = factory.GetColor(2441828);
		table.LegacyColors.MenuBackground = factory.GetColor(16185078);
		table.LegacyColors.MenuBackground2 = Color.Empty;
		table.LegacyColors.MenuBarBackground = factory.GetColor(12573695);
		table.LegacyColors.MenuBorder = factory.GetColor(color);
		table.LegacyColors.ItemExpandedBorder = table.LegacyColors.MenuBorder;
		table.LegacyColors.MenuSide = factory.GetColor(15331054);
		table.LegacyColors.MenuSide2 = Color.Empty;
		table.LegacyColors.MenuUnusedBackground = table.LegacyColors.MenuBackground;
		table.LegacyColors.MenuUnusedSide = factory.GetColor(14342874);
		table.LegacyColors.MenuUnusedSide2 = Color.Empty;
		table.LegacyColors.ItemDesignTimeBorder = Color.Black;
		table.LegacyColors.BarDockedBorder = factory.GetColor(5005434);
		table.LegacyColors.DockSiteBackColor = factory.GetColor(13095392);
		table.LegacyColors.DockSiteBackColor2 = Color.Empty;
		table.LegacyColors.CustomizeBackground = factory.GetColor(13292771);
		table.LegacyColors.CustomizeBackground2 = factory.GetColor(9543604);
		table.LegacyColors.CustomizeText = factory.GetColor(5005434);
		table.LegacyColors.PanelBackground = factory.GetColor(16251903);
		table.LegacyColors.PanelBackground2 = factory.GetColor(14477301);
		table.LegacyColors.PanelText = factory.GetColor(2441828);
		table.LegacyColors.PanelBorder = factory.GetColor(color);
		table.LegacyColors.ExplorerBarBackground = factory.GetColor(12896468);
		table.LegacyColors.ExplorerBarBackground2 = factory.GetColor(11645896);
		table.LegacyColors.SplitterBackground = table.LegacyColors.PanelBackground;
		table.LegacyColors.SplitterBackground2 = table.LegacyColors.PanelBackground2;
		table.LegacyColors.SplitterText = table.LegacyColors.PanelText;
		table.LegacyColors.SplitterBorder = table.LegacyColors.PanelBorder;
		table.NavigationPane.ButtonBackground = new GradientColorTable();
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15726333), 0f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15266299), 0.4f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14477044), 0.4f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14543351), 1f));
		table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(13228527));
		table.SuperTooltip.TextColor = factory.GetColor(5000268);
		Office2007SliderColorTable slider = table.Slider;
		slider.Default.LabelColor = factory.GetColor(663677);
		slider.Default.SliderLabelColor = factory.GetColor(2441828);
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
		slider.MouseOver.SliderLabelColor = factory.GetColor(2441828);
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
		slider.Pressed.SliderLabelColor = factory.GetColor(2441828);
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
		Office2007ColorTable.ቮ(table, factory);
		table.ListViewEx.Border = factory.GetColor(color);
		table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(12901887));
		table.ListViewEx.ColumnSeparator = factory.GetColor(10143487);
		table.ListViewEx.ColumnForeColor = factory.GetColor(2441828);
		table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(10997232), Color.Empty);
		table.ListViewEx.SelectionBorder = factory.GetColor(14938111);
		table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(10401486);
		table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(16383229), factory.GetColor(13884393), 90);
		table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(16374175), factory.GetColor(15843679), 90);
		table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(16766349), factory.GetColor(15897146), 90);
		table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(14672612), factory.GetColor(12371410), 90);
		table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(8888247);
		table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(12371410), factory.GetColor(14672612), 90);
		table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(16777215);
		table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(15002871));
		table.DataGridView.RowNormalBorder = factory.GetColor(10401486);
		table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(16766349));
		table.DataGridView.RowSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(12305617));
		table.DataGridView.RowPressedBorder = factory.GetColor(16777215);
		table.DataGridView.GridColor = factory.GetColor(13686757);
		table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(11125993));
		table.DataGridView.SelectorBorder = factory.GetColor(10401486);
		table.DataGridView.SelectorBorderDark = factory.GetColor(11587575);
		table.DataGridView.SelectorBorderLight = factory.GetColor(14017778);
		table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(16382715), factory.GetColor(14146274));
		table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(9150645));
		table.DataGridView.SelectorMouseOverBorder = factory.GetColor(10401486);
		table.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(11587575);
		table.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(14017778);
		table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(16382715), factory.GetColor(14146274));
		table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
		table.SideBar.Border = factory.GetColor(color);
		table.SideBar.SideBarPanelItemText = factory.GetColor(2441828);
		table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(16383229), 0f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14806521), 0.4f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14017010), 0.4f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14083574), 1f));
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
		table.AdvTree = new TreeColorTable();
		_135B._135C(table.AdvTree, factory);
		table.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(2441828);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(2441828);
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
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(2441828);
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
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(2441828);
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
		table.WarningBox.BackColor = factory.GetColor(16579836);
		table.WarningBox.WarningBorderColor = factory.GetColor(color);
		table.WarningBox.WarningBackColor1 = factory.GetColor(16186367);
		table.WarningBox.WarningBackColor2 = factory.GetColor(14477301);
		table.SuperTab.Background = new SuperTabLinearGradientColorTable(factory.GetColor(14674421));
		table.SuperTab.InnerBorder = factory.GetColor(16645887);
		table.SuperTab.OuterBorder = factory.GetColor(12241371);
		table.SuperTab.ControlBoxDefault.Image = factory.GetColor(2441828);
		table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		table.SuperTab.ControlBoxPressed.Background = factory.GetColor(12045024);
		table.SuperTab.ControlBoxPressed.Border = factory.GetColor(3381759);
		table.SuperTab.ControlBoxPressed.Image = factory.GetColor(255, 0);
		table.SuperTab.InsertMarker = factory.GetColor(255, 0);
		table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		table.SuperTabItem.Default.Normal.Text = factory.GetColor(2441828);
		table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(2441828);
		table.SuperTabItem.Default.Disabled.Text = factory.GetColor(9276813);
		table.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		table.SuperTabItem.Default.Disabled.CloseMarker = factory.GetColor(9276813);
		table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(16645887),
			factory.GetColor(16251903)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(16645887);
		table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(9610695);
		table.SuperTabItem.Default.Selected.Text = factory.GetColor(2441828);
		table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(2441828);
		table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(16776958),
			factory.GetColor(16775932)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16776958);
		table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(16758528);
		table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(2441828);
		table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(2441828);
		table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15331575),
			factory.GetColor(15331320)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(16776956);
		table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(14598247);
		table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(2441828);
		table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(2441828);
		table.SuperTabItem.Left = table.SuperTabItem.Default;
		table.SuperTabItem.Bottom = table.SuperTabItem.Default;
		table.SuperTabItem.Right = table.SuperTabItem.Right;
		table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16251903), Color.Empty);
		table.SuperTabPanel.Default.InnerBorder = factory.GetColor(Color.Empty);
		table.SuperTabPanel.Default.OuterBorder = factory.GetColor(9610695);
		table.SuperTabPanel.Left = table.SuperTabPanel.Default;
		table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
		table.SuperTabPanel.Right = table.SuperTabPanel.Default;
		table.Backstage.Background = new SuperTabLinearGradientColorTable(factory.GetColor(14674421));
		table.Backstage.InnerBorder = factory.GetColor(16645887);
		table.Backstage.OuterBorder = factory.GetColor(12241371);
		table.Backstage.ControlBoxDefault.Image = factory.GetColor(2441828);
		table.Backstage.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		table.Backstage.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		table.Backstage.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		table.Backstage.ControlBoxPressed.Background = factory.GetColor(12045024);
		table.Backstage.ControlBoxPressed.Border = factory.GetColor(3381759);
		table.Backstage.ControlBoxPressed.Image = factory.GetColor(255, 0);
		table.Backstage.InsertMarker = factory.GetColor(255, 0);
		table.BackstageItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		table.BackstageItem.Default.Normal.Text = factory.GetColor(2441828);
		table.BackstageItem.Default.Normal.CloseMarker = factory.GetColor(2441828);
		table.BackstageItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(16645887),
			factory.GetColor(16251903)
		}, new float[2] { 0f, 1f });
		table.BackstageItem.Default.Selected.InnerBorder = factory.GetColor(16645887);
		table.BackstageItem.Default.Selected.OuterBorder = factory.GetColor(9610695);
		table.BackstageItem.Default.Selected.Text = factory.GetColor(2441828);
		table.BackstageItem.Default.Selected.CloseMarker = factory.GetColor(2441828);
		table.BackstageItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(16776958),
			factory.GetColor(16775932)
		}, new float[2] { 0f, 1f });
		table.BackstageItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16776958);
		table.BackstageItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(16758528);
		table.BackstageItem.Default.SelectedMouseOver.Text = factory.GetColor(2441828);
		table.BackstageItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(2441828);
		table.BackstageItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15331575),
			factory.GetColor(15331320)
		}, new float[2] { 0f, 1f });
		table.BackstageItem.Default.MouseOver.InnerBorder = factory.GetColor(16776956);
		table.BackstageItem.Default.MouseOver.OuterBorder = factory.GetColor(14598247);
		table.BackstageItem.Default.MouseOver.Text = factory.GetColor(2441828);
		table.BackstageItem.Default.MouseOver.CloseMarker = factory.GetColor(2441828);
		table.BackstageItem.Left = table.BackstageItem.Default;
		table.BackstageItem.Bottom = table.BackstageItem.Default;
		table.BackstageItem.Right = table.BackstageItem.Right;
		table.BackstagePanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16251903), Color.Empty);
		table.BackstagePanel.Default.InnerBorder = factory.GetColor(Color.Empty);
		table.BackstagePanel.Default.OuterBorder = factory.GetColor(9610695);
		table.BackstagePanel.Left = table.BackstagePanel.Default;
		table.BackstagePanel.Bottom = table.BackstagePanel.Default;
		table.BackstagePanel.Right = table.BackstagePanel.Default;
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = factory.GetColor(11189202);
		switchButtonColorTable.OffBackColor = factory.GetColor(16186367);
		switchButtonColorTable.OffTextColor = factory.GetColor(2441828);
		switchButtonColorTable.OnBackColor = factory.GetColor(9621584);
		switchButtonColorTable.OnTextColor = factory.GetColor(2441828);
		switchButtonColorTable.SwitchBackColor = factory.GetColor(14477301);
		switchButtonColorTable.SwitchBorderColor = factory.GetColor(10400457);
		switchButtonColorTable.TextColor = factory.GetColor(2441828);
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
		elementStyle.BorderColor = factory.GetColor(11189202);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = factory.GetColor(15988732);
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
		elementStyle = ዂ.ዽ(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(factory.GetColor(color), SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጌ(factory, eOffice2007ColorScheme.Blue);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(table.ListViewEx);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(table.Bar);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(factory.GetColor(color), Color.Empty);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(factory.GetColor(16383229), factory.GetColor(13884393), factory.GetColor(10401486));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(factory.GetColor(16383229), factory.GetColor(13884393), factory.GetColor(10401486));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(factory.GetColor(0));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(factory.GetColor(Color.White), factory.GetColor("FF567DB0"), factory.GetColor("FF2F578D"));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጙ(table.CheckBoxItem.Default.CheckBorder);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(factory.GetColor(Color.White));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(factory.GetColor(Color.DarkGray));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(factory.GetColor(12371928));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(factory.GetColor(16251903));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(factory.GetColor(table.LegacyColors.PanelBorder), Color.Empty);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(factory.GetColor(2441828));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(factory.GetColor(2441828), factory.GetColor(16772277));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(factory.GetColor(2441828), factory.GetColor(14477301));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		table.StepIndicator.BackgroundColor = factory.GetColor(ColorFunctions.GetShade(table.Form.BackColor, 10));
		table.StepIndicator.IndicatorColor = factory.GetColor(10800168);
		table.TabForm.Active.CaptionText = factory.GetColor(2441828);
		table.TabForm.Inactive.CaptionText = factory.GetColor(2441828);
		table.TabForm.Active.CaptionBackColors = new Color[1] { factory.GetColor(14674421) };
		table.TabForm.Inactive.CaptionBackColors = new Color[1] { factory.GetColor(14674421) };
		table.TabFormItemColorTables.Clear();
		table.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(factory, new Color[1] { factory.GetColor(14674421) }, new Color[1] { factory.GetColor(12241371) }, factory.GetColor(2441828), new Color[2]
		{
			factory.GetColor(16645887),
			factory.GetColor(16251903)
		}, new Color[1] { factory.GetColor(12241371) }, factory.GetColor(2441828)));
		table.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(factory));
		table.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(factory));
		table.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(factory));
		table.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(factory));
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBar(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(10334921), factory.GetColor(10860752));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(160, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(16251903), factory.GetColor(14477301));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(7570329);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarMouseOver(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(10269127), factory.GetColor(10794959));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(16185854), factory.GetColor(15462649));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(16119547), factory.GetColor(14477301));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(15202812), Color.Transparent);
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(7570329);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrange(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(2441828);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(16758528), factory.GetColor(16758528));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16710132), factory.GetColor(16776936));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16641243), factory.GetColor(16637624));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16764520), factory.GetColor(16768653));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16776604), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(16758528), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16705978), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(2441828);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(11386322));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(12751419), factory.GetColor(12759897));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(13942956), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(14994094), factory.GetColor(15583117));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(15711329), factory.GetColor(15650948));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(15724989), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(12755277), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14070639), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(2441828);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(12751419), factory.GetColor(12759375));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(14862766), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(16045233), factory.GetColor(16633997));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(16761950), factory.GetColor(16640148));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16777157), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(2441828);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(12751419), factory.GetColor(12759375));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(14862766), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(16045233), factory.GetColor(16633997));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(16761950), factory.GetColor(16640148));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16777157), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(2441828);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrangeWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueOrange = GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(16383229), factory.GetColor(14806521));
		buttonItemBlueOrange.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(14017010), factory.GetColor(14083574));
		buttonItemBlueOrange.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9281463), Color.Empty);
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
		Color color = factory.GetColor(5005434);
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

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueDefault(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(16645887), factory.GetColor(16251903));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16645887), factory.GetColor(16580351));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12241371), factory.GetColor(12241371));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16776958), factory.GetColor(16775932));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776958), factory.GetColor(16776701));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(16758528), factory.GetColor(16754766));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(15331575), factory.GetColor(15331320));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776956), factory.GetColor(15922681));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(16758528), factory.GetColor(14598247));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(2441828);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueMagenta(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13945058), factory.GetColor(15525874));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(13484767));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12499649), factory.GetColor(12632001));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13945058), factory.GetColor(15525874));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(13484767));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12499649), factory.GetColor(12632001));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(12899832), factory.GetColor(12762344));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14741247), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(2441828);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(14019283), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12173751), factory.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14019283), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12173751), factory.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(11788003), factory.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14872575), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(2441828);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289388), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289388), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(2441828);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(13623521), factory.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14872575), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(2441828);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle GetFileMenuContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.PaddingBottom = 0;
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
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBottomContainerBackgroundBlend.Count];
		menu.FileBottomContainerBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		elementStyle.MarginTop = 0;
		elementStyle.MarginRight = 2;
		elementStyle.PaddingTop = 1;
		return elementStyle;
	}
}
