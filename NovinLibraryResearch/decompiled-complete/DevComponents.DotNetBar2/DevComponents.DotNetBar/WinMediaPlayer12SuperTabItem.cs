using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

public class WinMediaPlayer12SuperTabItem : SuperTabItemBaseDisplay
{
	private const int _136D = 4;

	public WinMediaPlayer12SuperTabItem(SuperTabItem tabItem)
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
		int num = base._1FD4.έ._1FFF;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, 4, 4);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Top + 4);
		val.AddArc(rectangle, 180f, 90f);
		val.AddLine(displayRectangle.X + 4, displayRectangle.Top, displayRectangle.Right - num - 4, displayRectangle.Top);
		rectangle.X = displayRectangle.Right - num - 4;
		val.AddArc(rectangle, 270f, 90f);
		val.AddLine(displayRectangle.Right - num, displayRectangle.Top + 4, displayRectangle.Right - num, displayRectangle.Bottom);
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
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 4, displayRectangle.Bottom - 4, 4, 4);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right, displayRectangle.Bottom - 4);
		val.AddArc(rectangle, 0f, 90f);
		val.AddLine(displayRectangle.Right - 4, displayRectangle.Bottom, displayRectangle.X + 4, displayRectangle.Bottom);
		rectangle.X = displayRectangle.X;
		val.AddArc(rectangle, 90f, 90f);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - 4, displayRectangle.X, displayRectangle.Top);
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
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 4, 4, 4);
		val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X + 4, displayRectangle.Bottom);
		val.AddArc(rectangle, 90f, 90f);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - 4, displayRectangle.X, displayRectangle.Top + 4);
		rectangle.Y = displayRectangle.Y;
		val.AddArc(rectangle, 180f, 90f);
		val.AddLine(displayRectangle.X + 4, displayRectangle.Top, displayRectangle.Right, displayRectangle.Top);
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
