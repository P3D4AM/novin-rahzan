using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

public class VS2008DocumentSuperTabItem : SuperTabItemBaseDisplay
{
	private const int _208E = 8;

	private const int _208F = 20;

	public VS2008DocumentSuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	internal override Rectangle _1FDC()
	{
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		int _136F = base._1FD4.έ._136F;
		if (base._1FD4.ᓙ)
		{
			displayRectangle.Y += _136F;
			displayRectangle.Height -= _136F;
		}
		else
		{
			displayRectangle.X += _136F;
			displayRectangle.Width -= _136F;
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
		int num = base._1FD4.έ._136F + base._1FD4.έ._1FFF;
		Rectangle rectangle = new Rectangle(displayRectangle.X + num, displayRectangle.Y, 20, 20);
		val.AddLine(displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
		val.AddArc(rectangle, 218f, 52f);
		rectangle = new Rectangle(displayRectangle.Right - 8, displayRectangle.Y, 8, 8);
		val.AddArc(rectangle, 270f, 90f);
		val.AddLine(displayRectangle.Right, displayRectangle.Top + 8, displayRectangle.Right, displayRectangle.Bottom);
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
		int _136F = base._1FD4.έ._136F;
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 8, displayRectangle.Bottom - 8, 8, 8);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right, displayRectangle.Bottom - 8);
		val.AddArc(rectangle, 0f, 90f);
		rectangle = new Rectangle(displayRectangle.X + _136F, displayRectangle.Bottom - 20, 20, 20);
		val.AddArc(rectangle, 90f, 62f);
		val.AddLine(displayRectangle.X, displayRectangle.Top, displayRectangle.X, displayRectangle.Top);
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
		int _136F = base._1FD4.έ._136F;
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 8, 8, 8);
		val.AddLine(displayRectangle.Right, displayRectangle.Bottom, displayRectangle.X + 8, displayRectangle.Bottom);
		val.AddArc(rectangle, 90f, 90f);
		rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y + _136F, 20, 20);
		val.AddArc(rectangle, 180f, 62f);
		val.AddLine(displayRectangle.Right, displayRectangle.Top, displayRectangle.Right, displayRectangle.Top);
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
		int _136F = base._1FD4.έ._136F;
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 20, displayRectangle.Top + _136F, 20, 20);
		val.AddLine(displayRectangle.X, displayRectangle.Top, displayRectangle.X, displayRectangle.Top);
		val.AddArc(rectangle, 298f, 62f);
		rectangle = new Rectangle(displayRectangle.Right - 8, displayRectangle.Bottom - 8, 8, 8);
		val.AddArc(rectangle, 0f, 90f);
		val.AddLine(displayRectangle.Right - 8, displayRectangle.Bottom, displayRectangle.X, displayRectangle.Bottom);
		return val;
	}
}
