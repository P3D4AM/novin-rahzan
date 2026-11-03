using System;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class Office2010ColorTable : Office2007ColorTable
{
	private eOffice2010ColorScheme __21AD;

	private static Office2007ButtonItemColorTable _21AE;

	public eOffice2010ColorScheme ColorScheme => __21AD;

	public Office2010ColorTable()
	{
		_21B1._21B2(this, ColorFactory.Empty);
		_21B0(this, ColorFactory.Empty);
	}

	public Office2010ColorTable(eOffice2010ColorScheme color)
	{
		switch (color)
		{
		case eOffice2010ColorScheme.Blue:
			_21B1._21B2(this, ColorFactory.Empty);
			break;
		case eOffice2010ColorScheme.Silver:
			_21BB._21B2(this, ColorFactory.Empty);
			break;
		case eOffice2010ColorScheme.Black:
			_21BC._21B2(this, ColorFactory.Empty);
			break;
		case eOffice2010ColorScheme.VS2010:
			_21C3._21B2(this, ColorFactory.Empty);
			break;
		}
		_21B0(this, ColorFactory.Empty);
		__21AD = color;
	}

	public Office2010ColorTable(eOffice2010ColorScheme color, Color blendColor)
	{
		ColorFactory ቖ = (blendColor.IsEmpty ? ColorFactory.Empty : new ColorBlendFactory(blendColor));
		switch (color)
		{
		case eOffice2010ColorScheme.Blue:
			_21B1._21B2(this, ቖ);
			break;
		case eOffice2010ColorScheme.Silver:
			_21BB._21B2(this, ቖ);
			break;
		case eOffice2010ColorScheme.Black:
			_21BC._21B2(this, ቖ);
			break;
		case eOffice2010ColorScheme.VS2010:
			_21C3._21B2(this, ቖ);
			break;
		}
		_21B0(this, ቖ);
		__21AD = color;
	}

	internal static Office2007ButtonItemColorTable _21AF()
	{
		if (_21AE != null)
		{
			return _21AE;
		}
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(4095451, 2514866);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(DevComponents.DotNetBar.ColorScheme.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(1984672, 2379686);
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(32, 16777215);
		office2007ButtonItemStateColorTable.Text = DevComponents.DotNetBar.ColorScheme.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(4687589, 2975164);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(DevComponents.DotNetBar.ColorScheme.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(2050209, 2708649);
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(DevComponents.DotNetBar.ColorScheme.GetColor(72, 16777215), DevComponents.DotNetBar.ColorScheme.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = DevComponents.DotNetBar.ColorScheme.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(2646454, 1923762);
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(DevComponents.DotNetBar.ColorScheme.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(2050209, 2248358);
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(DevComponents.DotNetBar.ColorScheme.GetColor(72, 16777215), DevComponents.DotNetBar.ColorScheme.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = DevComponents.DotNetBar.ColorScheme.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		_21AE = office2007ButtonItemColorTable;
		return office2007ButtonItemColorTable;
	}

	internal static void _21B0(Office2007ColorTable _139D, ColorFactory ቖ)
	{
		Office2007ButtonItemColorTableCollection applicationButtonColors = _139D.ApplicationButtonColors;
		applicationButtonColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4095451), ቖ.GetColor(2514866));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(64, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(1984672), ቖ.GetColor(2379686));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(32, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Default = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4687589), ቖ.GetColor(2975164));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2050209), ቖ.GetColor(2708649));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = office2007ButtonItemStateColorTable;
		office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(2646454), ቖ.GetColor(1923762));
		office2007ButtonItemStateColorTable.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(82, 16777215), Color.Transparent);
		office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2050209), ቖ.GetColor(2248358));
		office2007ButtonItemStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(72, 16777215), ቖ.GetColor(48, 16777215));
		office2007ButtonItemStateColorTable.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed = office2007ButtonItemStateColorTable;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
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
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
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
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
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
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
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
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		applicationButtonColors.Add(office2007ButtonItemColorTable);
	}
}
