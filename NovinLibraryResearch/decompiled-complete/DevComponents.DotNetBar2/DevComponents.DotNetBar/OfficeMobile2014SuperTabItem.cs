using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

public class OfficeMobile2014SuperTabItem : SuperTabItemBaseDisplay
{
	private const int _136D = 8;

	private const int _136E = 16;

	private const int ܭ = 2;

	private const int _208B = 7;

	private const int _208C = 5;

	private const int _208D = 3;

	public OfficeMobile2014SuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	internal override Rectangle _1FDC()
	{
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		int _136F = base._1FD4.έ._136F;
		int num = base._1FD4.έ._1FFF;
		if (base._1FD4.ᓙ)
		{
			displayRectangle.Y += _136F / 2 + num;
			displayRectangle.Height -= _136F + num * 2;
		}
		else
		{
			displayRectangle.X += _136F / 2 + num;
			displayRectangle.Width -= _136F + num * 2;
		}
		int num2 = Dpi.Width(7);
		int num3 = Dpi.Width(3);
		switch (base.ᑸ.TabAlignment)
		{
		case eTabStripAlignment.Top:
			displayRectangle.Height -= num2;
			break;
		case eTabStripAlignment.Bottom:
			displayRectangle.Height -= 5;
			displayRectangle.Y += 5;
			break;
		case eTabStripAlignment.Left:
			if (!base._1FD4.HorizontalText)
			{
				displayRectangle.X -= num3;
				displayRectangle.Y += num3;
			}
			displayRectangle.Width -= num2;
			break;
		default:
			if (base._1FD4.HorizontalText)
			{
				displayRectangle.X += num3;
				displayRectangle.Width -= 5;
			}
			else
			{
				displayRectangle.X += num2;
				displayRectangle.Width -= num2;
				displayRectangle.Height += num2;
			}
			break;
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width -= Dpi.Width1;
		displayRectangle.Height -= Dpi.Height(5);
		int num = Math.Min(20, displayRectangle.Height);
		float num2 = (float)num / 20f;
		int num3 = (int)(90f * num2) - 25;
		int num4 = (int)((float)(base._1FD4.έ._136F / 2) * num2);
		GraphicsPath val = new GraphicsPath();
		int num5 = Dpi.Width(16);
		Rectangle rectangle = new Rectangle(displayRectangle.X - Dpi.Width(8), displayRectangle.Bottom - num5, num5, num5);
		val.AddArc(rectangle, 90f, (float)(-num3));
		int num6 = Dpi.Width(2);
		rectangle = new Rectangle(displayRectangle.X + Dpi.Width(8) + Dpi.Width(num6), displayRectangle.Y, num5, num5);
		val.AddArc(rectangle, (float)(270 - num3), (float)num3);
		rectangle = new Rectangle(displayRectangle.Right - (num5 + num6 + num4), displayRectangle.Y, num5, num5);
		val.AddArc(rectangle, 270f, (float)num3);
		rectangle = new Rectangle(displayRectangle.Right - (num5 - num6 * 2), displayRectangle.Bottom - num5, num5, num5);
		val.AddArc(rectangle, (float)(90 + num3), (float)(-num3));
		return val;
	}

	private GraphicsPath _2084()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height -= 6;
		displayRectangle.Y += 5;
		GraphicsPath val = new GraphicsPath();
		int num = Math.Min(20, displayRectangle.Height);
		float num2 = (float)num / 20f;
		int num3 = (int)(90f * num2) - 25;
		int num4 = (int)((float)(base._1FD4.έ._136F / 2) * num2);
		Rectangle rectangle = new Rectangle(displayRectangle.X - 8, displayRectangle.Y, 16, 16);
		val.AddArc(rectangle, 270f, (float)num3);
		rectangle = new Rectangle(displayRectangle.X + 8 + 2, displayRectangle.Bottom - 16, 16, 16);
		val.AddArc(rectangle, (float)(90 + num3), (float)(-num3));
		rectangle = new Rectangle(displayRectangle.Right - (18 + num4), displayRectangle.Bottom - 16, 16, 16);
		val.AddArc(rectangle, 90f, (float)(-num3));
		rectangle = new Rectangle(displayRectangle.Right - 12, displayRectangle.Y, 16, 16);
		val.AddArc(rectangle, (float)(270 - num3), (float)num3);
		return val;
	}

	private GraphicsPath _2085()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width -= 5;
		displayRectangle.Height--;
		GraphicsPath val = new GraphicsPath();
		int num = Math.Min(20, displayRectangle.Width);
		float num2 = (float)num / 20f;
		int num3 = (int)(90f * num2) - 25;
		int num4 = (int)((float)(base._1FD4.έ._136F / 2) * num2);
		Rectangle rectangle = new Rectangle(displayRectangle.Right - 16, displayRectangle.Y - 8, 16, 16);
		val.AddArc(rectangle, 0f, (float)num3);
		rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y + 2 + num4, 16, 16);
		val.AddArc(rectangle, (float)(-180 + num3), (float)(-num3));
		rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 16 - 2, 16, 16);
		val.AddArc(rectangle, 180f, (float)(-num3));
		rectangle = new Rectangle(displayRectangle.Right - 16, displayRectangle.Bottom - 8 + num4, 16, 16);
		val.AddArc(rectangle, (float)(-num3), (float)num3);
		return val;
	}

	private GraphicsPath _2086()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.X += 5;
		displayRectangle.Width -= 6;
		displayRectangle.Height--;
		GraphicsPath val = new GraphicsPath();
		int num = Math.Min(20, displayRectangle.Width);
		float num2 = (float)num / 20f;
		int num3 = (int)(90f * num2) - 25;
		int num4 = (int)((float)(base._1FD4.έ._136F / 2) * num2);
		Rectangle rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y - 8, 16, 16);
		val.AddArc(rectangle, 180f, (float)(-num3));
		rectangle = new Rectangle(displayRectangle.Right - 16, displayRectangle.Y + 2 + num4, 16, 16);
		val.AddArc(rectangle, (float)(-num3), (float)num3);
		rectangle = new Rectangle(displayRectangle.Right - 16, displayRectangle.Bottom - 16 - 2, 16, 16);
		val.AddArc(rectangle, 0f, (float)num3);
		rectangle = new Rectangle(displayRectangle.X, displayRectangle.Bottom - 8 + num4, 16, 16);
		val.AddArc(rectangle, (float)(180 + num3), (float)(-num3));
		return val;
	}
}
