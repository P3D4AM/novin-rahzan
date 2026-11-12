using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class Office2010ColorTable : Office2007ColorTable
{
	private eOffice2010ColorScheme _ColorScheme;

	private static Office2007ButtonItemColorTable FallBackAppButtonColorTable;

	public eOffice2010ColorScheme ColorScheme => _ColorScheme;

	public Office2010ColorTable()
	{
		Office2010BlueFactory.InitializeColorTable(this, ColorFactory.Empty);
		InitAppButtonColors(this, ColorFactory.Empty);
	}

	public Office2010ColorTable(eOffice2010ColorScheme color)
	{
		switch (color)
		{
		case eOffice2010ColorScheme.Blue:
			Office2010BlueFactory.InitializeColorTable(this, ColorFactory.Empty);
			break;
		case eOffice2010ColorScheme.Silver:
			Office2010SilverFactory.InitializeColorTable(this, ColorFactory.Empty);
			break;
		case eOffice2010ColorScheme.Black:
			Office2010BlackFactory.InitializeColorTable(this, ColorFactory.Empty);
			break;
		case eOffice2010ColorScheme.VS2010:
			VisualStudio2010Factory.InitializeColorTable(this, ColorFactory.Empty);
			break;
		}
		InitAppButtonColors(this, ColorFactory.Empty);
		_ColorScheme = color;
	}

	public Office2010ColorTable(eOffice2010ColorScheme color, Color blendColor)
	{
		ColorFactory factory = (blendColor.IsEmpty ? ColorFactory.Empty : new ColorBlendFactory(blendColor));
		switch (color)
		{
		case eOffice2010ColorScheme.Blue:
			Office2010BlueFactory.InitializeColorTable(this, factory);
			break;
		case eOffice2010ColorScheme.Silver:
			Office2010SilverFactory.InitializeColorTable(this, factory);
			break;
		case eOffice2010ColorScheme.Black:
			Office2010BlackFactory.InitializeColorTable(this, factory);
			break;
		case eOffice2010ColorScheme.VS2010:
			VisualStudio2010Factory.InitializeColorTable(this, factory);
			break;
		}
		InitAppButtonColors(this, factory);
		_ColorScheme = color;
	}

	internal static Office2007ButtonItemColorTable GetFallBackAppButtonColorTable()
	{
		if (FallBackAppButtonColorTable != null)
		{
			return FallBackAppButtonColorTable;
		}
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(4095451, 2514866);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(1984672, 2379686);
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(32, 16777215);
		office2007ButtonItemStateColorTable.Text = Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(4687589, 2975164);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(2050209, 2708649);
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(72, 16777215), Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(2646454, 1923762);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(2050209, 2248358);
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(72, 16777215), Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = Stimulsoft.Controls.Win.DotNetBar.ColorScheme.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		FallBackAppButtonColorTable = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	internal static void InitAppButtonColors(Office2007ColorTable colorTable, ColorFactory factory)
	{
		Office2007ButtonItemColorTableCollection applicationButtonColors = colorTable.ApplicationButtonColors;
		applicationButtonColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(4095451), factory.GetColor(2514866));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(1984672), factory.GetColor(2379686));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(32, 16777215));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(4687589), factory.GetColor(2975164));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(2050209), factory.GetColor(2708649));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 16777215), factory.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(factory.GetColor(2646454), factory.GetColor(1923762));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(2050209), factory.GetColor(2248358));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 16777215), factory.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = factory.GetColor(16777215);
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
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
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
}
