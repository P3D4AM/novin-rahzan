using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class VS2008DockSuperTabItem : SuperTabItemBaseDisplay
{
	public VS2008DockSuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	internal override GraphicsPath _1FDE()
	{
		GraphicsPath val = base._1FDE();
		if (val != null)
		{
			return val;
		}
		return (GraphicsPath)(base.ᑸ.TabAlignment switch
		{
			eTabStripAlignment.Top => _2083(), 
			eTabStripAlignment.Bottom => _2084(), 
			eTabStripAlignment.Left => _2085(), 
			_ => _2086(), 
		});
	}

	private GraphicsPath _2083()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Height += 2;
		}
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.X, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Bottom)
		});
		return val;
	}

	private GraphicsPath _2084()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Y -= 2;
			displayRectangle.Height += 2;
		}
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.Right, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Top)
		});
		return val;
	}

	private GraphicsPath _2085()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Width += 2;
		}
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.Right, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Top)
		});
		return val;
	}

	private GraphicsPath _2086()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.X -= 2;
			displayRectangle.Width += 2;
		}
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.X, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Bottom)
		});
		return val;
	}

	internal override void ῢ(eTabState ὡ, SuperTabItemStateColorTable ና)
	{
		if (base.ᑸ.PredefinedColor != 0)
		{
			SuperTabItemColorTable superTabItemColorTable = ጦ.ፐ(base.ᑸ.PredefinedColor, ColorFactory.Empty);
			SuperTabItemStateColorTable superTabItemStateColorTable = superTabItemColorTable.Default.Normal;
			switch (ὡ)
			{
			case eTabState.SelectedMouseOver:
				superTabItemStateColorTable = superTabItemColorTable.Default.SelectedMouseOver;
				break;
			case eTabState.MouseOver:
				superTabItemStateColorTable = superTabItemColorTable.Default.MouseOver;
				break;
			case eTabState.Selected:
				superTabItemStateColorTable = superTabItemColorTable.Default.Selected;
				superTabItemStateColorTable.InnerBorder = Color.Empty;
				break;
			}
			ና.InnerBorder = superTabItemStateColorTable.InnerBorder;
			ና.OuterBorder = superTabItemStateColorTable.OuterBorder;
			ና.Background = superTabItemStateColorTable.Background;
		}
	}

	internal override void ῢ(SuperTabPanelItemColorTable ῥ)
	{
		if (base.ᑸ.PredefinedColor != 0)
		{
			SuperTabPanelItemColorTable superTabPanelItemColorTable = ጦ.ፕ(base.ᑸ.PredefinedColor, ColorFactory.Empty);
			ῥ.Background = superTabPanelItemColorTable.Background;
			ῥ.InnerBorder = ῥ.Background.Colors[ῥ.Background.Colors.Length - 1];
			ῥ.OuterBorder = superTabPanelItemColorTable.OuterBorder;
		}
	}
}
