using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class OneNote2007SuperTabItem : SuperTabItemBaseDisplay
{
	private const int Radius = 10;

	public OneNote2007SuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	internal override Rectangle ContentRectangle()
	{
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		int num = base.TabStripItem.TabDisplay.TabOverlap + base.TabStripItem.TabDisplay.TabSpacing;
		if (base.TabStripItem.IsVertical)
		{
			displayRectangle.Height -= num;
		}
		else
		{
			displayRectangle.Width -= num;
		}
		return displayRectangle;
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
		{
			displayRectangle.Height += 2;
		}
		GraphicsPath val = new GraphicsPath();
		int num = base.TabStripItem.TabDisplay.TabOverlap + base.TabStripItem.TabDisplay.TabSpacing * 2;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, 10, 10);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Top + 10);
		val.AddArc(rectangle, 180f, 90f);
		val.AddLine(displayRectangle.Right - num, displayRectangle.Top, displayRectangle.Right, displayRectangle.Bottom);
		return val;
	}

	private GraphicsPath BottomTabPath()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
		{
			displayRectangle.Y -= 2;
			displayRectangle.Height += 2;
		}
		GraphicsPath val = new GraphicsPath();
		int num = base.TabStripItem.TabDisplay.TabOverlap + base.TabStripItem.TabDisplay.TabSpacing * 2;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 10, 10, 10);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right - num, displayRectangle.Bottom);
		val.AddArc(rectangle, 90f, 90f);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - 10, displayRectangle.X, displayRectangle.Top);
		return val;
	}

	private GraphicsPath LeftTabPath()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
		{
			displayRectangle.Width += 2;
		}
		GraphicsPath val = new GraphicsPath();
		int num = base.TabStripItem.TabDisplay.TabOverlap + base.TabStripItem.TabDisplay.TabSpacing;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Top, 10, 10);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.X + 10, displayRectangle.Top);
		val.AddArc(rectangle, -90f, -90f);
		val.AddLine(displayRectangle.X, displayRectangle.Top + 10, displayRectangle.X, displayRectangle.Bottom - num);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - num, displayRectangle.Right, displayRectangle.Bottom);
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
		int num = base.TabStripItem.TabDisplay.TabOverlap + base.TabStripItem.TabDisplay.TabSpacing;
		if (!base.TabStripItem.HorizontalText)
		{
			num += base.TabStripItem.TabDisplay.TabSpacing;
		}
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 10, displayRectangle.Top, 10, 10);
		val.AddLine(displayRectangle.X, displayRectangle.Top, displayRectangle.Right - 10, displayRectangle.Top);
		val.AddArc(rectangle, 270f, 90f);
		val.AddLine(displayRectangle.Right, displayRectangle.Top + 10, displayRectangle.Right, displayRectangle.Bottom - num);
		val.AddLine(displayRectangle.Right, displayRectangle.Bottom - num, displayRectangle.X, displayRectangle.Bottom);
		return val;
	}
}
