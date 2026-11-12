using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class VS2008DockSuperTabItem : SuperTabItemBaseDisplay
{
	public VS2008DockSuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	internal override GraphicsPath TabItemPath()
	{
		GraphicsPath val = base.TabItemPath();
		if (val != null)
		{
			return val;
		}
		return (GraphicsPath)(base.TabItem.TabAlignment switch
		{
			eTabStripAlignment.Top => TopTabPath(), 
			eTabStripAlignment.Bottom => BottomTabPath(), 
			eTabStripAlignment.Left => LeftTabPath(), 
			_ => RightTabPath(), 
		});
	}

	private GraphicsPath TopTabPath()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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

	private GraphicsPath BottomTabPath()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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

	private GraphicsPath LeftTabPath()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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

	private GraphicsPath RightTabPath()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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

	internal override void ApplyPredefinedColor(eTabState tabState, SuperTabItemStateColorTable sct)
	{
		if (base.TabItem.PredefinedColor != 0)
		{
			SuperTabItemColorTable predefinedTabColors = SuperTabStyleColorFactory.GetPredefinedTabColors(base.TabItem.PredefinedColor, ColorFactory.Empty);
			SuperTabItemStateColorTable superTabItemStateColorTable = predefinedTabColors.Default.Normal;
			switch (tabState)
			{
			case eTabState.SelectedMouseOver:
				superTabItemStateColorTable = predefinedTabColors.Default.SelectedMouseOver;
				break;
			case eTabState.MouseOver:
				superTabItemStateColorTable = predefinedTabColors.Default.MouseOver;
				break;
			case eTabState.Selected:
				superTabItemStateColorTable = predefinedTabColors.Default.Selected;
				superTabItemStateColorTable.InnerBorder = Color.Empty;
				break;
			}
			sct.InnerBorder = superTabItemStateColorTable.InnerBorder;
			sct.OuterBorder = superTabItemStateColorTable.OuterBorder;
			sct.Background = superTabItemStateColorTable.Background;
		}
	}

	internal override void ApplyPredefinedColor(SuperTabPanelItemColorTable pct)
	{
		if (base.TabItem.PredefinedColor != 0)
		{
			SuperTabPanelItemColorTable predefinedPanelColors = SuperTabStyleColorFactory.GetPredefinedPanelColors(base.TabItem.PredefinedColor, ColorFactory.Empty);
			pct.Background = predefinedPanelColors.Background;
			pct.InnerBorder = pct.Background.Colors[pct.Background.Colors.Length - 1];
			pct.OuterBorder = predefinedPanelColors.OuterBorder;
		}
	}
}
