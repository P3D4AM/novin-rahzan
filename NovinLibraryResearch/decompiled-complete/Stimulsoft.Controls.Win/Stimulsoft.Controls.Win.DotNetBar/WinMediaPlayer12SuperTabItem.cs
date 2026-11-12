using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class WinMediaPlayer12SuperTabItem : SuperTabItemBaseDisplay
{
	private const int Radius = 4;

	public WinMediaPlayer12SuperTabItem(SuperTabItem tabItem)
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
		int tabSpacing = base.TabStripItem.TabDisplay.TabSpacing;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, 4, 4);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Top + 4);
		val.AddArc(rectangle, 180f, 90f);
		val.AddLine(displayRectangle.X + 4, displayRectangle.Top, displayRectangle.Right - tabSpacing - 4, displayRectangle.Top);
		rectangle.X = displayRectangle.Right - tabSpacing - 4;
		val.AddArc(rectangle, 270f, 90f);
		val.AddLine(displayRectangle.Right - tabSpacing, displayRectangle.Top + 4, displayRectangle.Right - tabSpacing, displayRectangle.Bottom);
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
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 4, displayRectangle.Bottom - 4, 4, 4);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right, displayRectangle.Bottom - 4);
		val.AddArc(rectangle, 0f, 90f);
		val.AddLine(displayRectangle.Right - 4, displayRectangle.Bottom, displayRectangle.X + 4, displayRectangle.Bottom);
		rectangle.X = displayRectangle.X;
		val.AddArc(rectangle, 90f, 90f);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - 4, displayRectangle.X, displayRectangle.Top);
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
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 4, 4, 4);
		val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X + 4, displayRectangle.Bottom);
		val.AddArc(rectangle, 90f, 90f);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - 4, displayRectangle.X, displayRectangle.Top + 4);
		rectangle.Y = displayRectangle.Y;
		val.AddArc(rectangle, 180f, 90f);
		val.AddLine(displayRectangle.X + 4, displayRectangle.Top, displayRectangle.Right, displayRectangle.Top);
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
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 4, displayRectangle.Top, 4, 4);
		val.AddLine(displayRectangle.X, displayRectangle.Top, displayRectangle.Right - 4, displayRectangle.Top);
		val.AddArc(rectangle, 270f, 90f);
		val.AddLine(displayRectangle.Right, displayRectangle.Top + 4, displayRectangle.Right, displayRectangle.Bottom - 4);
		rectangle.Y = displayRectangle.Bottom - 4;
		val.AddArc(rectangle, 0f, 90f);
		val.AddLine(displayRectangle.Right - 4, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
		return val;
	}
}
