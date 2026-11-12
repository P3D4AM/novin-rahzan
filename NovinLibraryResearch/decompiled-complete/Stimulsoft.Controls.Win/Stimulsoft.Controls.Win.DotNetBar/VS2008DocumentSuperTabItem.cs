using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class VS2008DocumentSuperTabItem : SuperTabItemBaseDisplay
{
	private const int SmRadius = 8;

	private const int LgRadius = 20;

	public VS2008DocumentSuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	internal override Rectangle ContentRectangle()
	{
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		int tabOverlap = base.TabStripItem.TabDisplay.TabOverlap;
		if (base.TabStripItem.IsVertical)
		{
			displayRectangle.Y += tabOverlap;
			displayRectangle.Height -= tabOverlap;
		}
		else
		{
			displayRectangle.X += tabOverlap;
			displayRectangle.Width -= tabOverlap;
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
		int num = base.TabStripItem.TabDisplay.TabOverlap + base.TabStripItem.TabDisplay.TabSpacing;
		Rectangle rectangle = new Rectangle(displayRectangle.X + num, displayRectangle.Y, 20, 20);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
		val.AddArc(rectangle, 218f, 52f);
		rectangle = new Rectangle(displayRectangle.Right - 8, displayRectangle.Y, 8, 8);
		val.AddArc(rectangle, 270f, 90f);
		val.AddLine(displayRectangle.Right, displayRectangle.Top + 8, displayRectangle.Right, displayRectangle.Bottom);
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
		int tabOverlap = base.TabStripItem.TabDisplay.TabOverlap;
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 8, displayRectangle.Bottom - 8, 8, 8);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right, displayRectangle.Bottom - 8);
		val.AddArc(rectangle, 0f, 90f);
		rectangle = new Rectangle(displayRectangle.X + tabOverlap, displayRectangle.Bottom - 20, 20, 20);
		val.AddArc(rectangle, 90f, 62f);
		val.AddLine(displayRectangle.X, displayRectangle.Top, displayRectangle.X, displayRectangle.Top);
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
		int tabOverlap = base.TabStripItem.TabDisplay.TabOverlap;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 8, 8, 8);
		val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X + 8, displayRectangle.Bottom);
		val.AddArc(rectangle, 90f, 90f);
		rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y + tabOverlap, 20, 20);
		val.AddArc(rectangle, 180f, 62f);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right, displayRectangle.Top);
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
		int tabOverlap = base.TabStripItem.TabDisplay.TabOverlap;
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 20, displayRectangle.Top + tabOverlap, 20, 20);
		val.AddLine(displayRectangle.X, displayRectangle.Top, displayRectangle.X, displayRectangle.Top);
		val.AddArc(rectangle, 298f, 62f);
		rectangle = new Rectangle(displayRectangle.Right - 8, displayRectangle.Bottom - 8, 8, 8);
		val.AddArc(rectangle, 0f, 90f);
		val.AddLine(displayRectangle.Right - 8, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
		return val;
	}
}
