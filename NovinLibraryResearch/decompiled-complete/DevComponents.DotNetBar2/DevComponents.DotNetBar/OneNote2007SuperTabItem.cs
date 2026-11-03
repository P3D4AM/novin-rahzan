using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

public class OneNote2007SuperTabItem : SuperTabItemBaseDisplay
{
	private const int _136D = 10;

	public OneNote2007SuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	internal override Rectangle _1FDC()
	{
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		int num = base._1FD4.έ._136F + base._1FD4.έ._1FFF;
		if (base._1FD4.ᓙ)
		{
			displayRectangle.Height -= num;
		}
		else
		{
			displayRectangle.Width -= num;
		}
		return displayRectangle;
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Height += 2;
		}
		GraphicsPath val = new GraphicsPath();
		int num = base._1FD4.έ._136F + base._1FD4.έ._1FFF * 2;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, 10, 10);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Top + 10);
		val.AddArc(rectangle, 180f, 90f);
		val.AddLine(displayRectangle.Right - num, displayRectangle.Top, displayRectangle.Right, displayRectangle.Bottom);
		return val;
	}

	private GraphicsPath _2084()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Y -= 2;
			displayRectangle.Height += 2;
		}
		GraphicsPath val = new GraphicsPath();
		int num = base._1FD4.έ._136F + base._1FD4.έ._1FFF * 2;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 10, 10, 10);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right - num, displayRectangle.Bottom);
		val.AddArc(rectangle, 90f, 90f);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - 10, displayRectangle.X, displayRectangle.Top);
		return val;
	}

	private GraphicsPath _2085()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Width += 2;
		}
		GraphicsPath val = new GraphicsPath();
		int num = base._1FD4.έ._136F + base._1FD4.έ._1FFF;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Top, 10, 10);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.X + 10, displayRectangle.Top);
		val.AddArc(rectangle, -90f, -90f);
		val.AddLine(displayRectangle.X, displayRectangle.Top + 10, displayRectangle.X, displayRectangle.Bottom - num);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom - num, displayRectangle.Right, displayRectangle.Bottom);
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
		int num = base._1FD4.έ._136F + base._1FD4.έ._1FFF;
		if (!base._1FD4.HorizontalText)
		{
			num += base._1FD4.έ._1FFF;
		}
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 10, displayRectangle.Top, 10, 10);
		val.AddLine(displayRectangle.X, displayRectangle.Top, displayRectangle.Right - 10, displayRectangle.Top);
		val.AddArc(rectangle, 270f, 90f);
		val.AddLine(displayRectangle.Right, displayRectangle.Top + 10, displayRectangle.Right, displayRectangle.Bottom - num);
		val.AddLine(displayRectangle.Right, displayRectangle.Bottom - num, displayRectangle.X, displayRectangle.Bottom);
		return val;
	}
}
