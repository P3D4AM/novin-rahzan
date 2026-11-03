using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Presentation;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.ScrollBar;

internal class ᚂ : ᘷ, ᙓ
{
	private Office2007ColorTable ຊ;

	private bool ᘿ;

	private ᙔ ᙀ = new ᙔ(1);

	private ᙔ ᙁ = new ᙔ(1);

	private ᙙ ᙂ = new ᙙ();

	private ᙙ ᙃ = new ᙙ();

	private ᙝ ᙄ;

	private ᙲ ᙅ;

	private Size ᙆ = new Size(9, 5);

	private ᙝ ᙇ;

	private ᙔ ᙈ = new ᙔ(1);

	private ᙔ ᙉ = new ᙔ(1);

	private ᙙ ᙊ = new ᙙ();

	private ᙝ ᙋ;

	private ᙔ ᙌ = new ᙔ(1);

	private ᙙ ᙍ = new ᙙ();

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public bool ᘾ
	{
		get
		{
			return ᘿ;
		}
		set
		{
			ᘿ = value;
		}
	}

	public ᚂ()
	{
		ᙄ = ᙏ();
		ᙇ = ᙐ();
		ᙋ = ᙎ();
	}

	public override void PaintThumb(Graphics g, Rectangle bounds, ᘸ position, ᘹ state)
	{
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ጧ(state);
		if (office2007ScrollBarStateColorTable != null)
		{
			ᙀ.ᙘ(office2007ScrollBarStateColorTable.ThumbOuterBorder);
			ᙁ.ᙘ(office2007ScrollBarStateColorTable.ThumbInnerBorder);
			ᙂ.ᙚ = office2007ScrollBarStateColorTable.ThumbBackground;
			ᙃ.ᙘ(office2007ScrollBarStateColorTable.ThumbSignBackground);
			ᙅ.ᙳ = ᙑ(position);
			ᙄ.Paint(new ᙰ(g, bounds));
			ᙅ.ᙳ.Dispose();
			ᙅ.ᙳ = null;
		}
	}

	private Office2007ScrollBarStateColorTable ጧ(ᘹ య)
	{
		Office2007ScrollBarColorTable office2007ScrollBarColorTable = (ᘿ ? ຊ.AppScrollBar : ຊ.ScrollBar);
		return య switch
		{
			ᘹ.ཊ => office2007ScrollBarColorTable.Default, 
			ᘹ.ड़ => office2007ScrollBarColorTable.Disabled, 
			ᘹ.ᘻ => office2007ScrollBarColorTable.MouseOverControl, 
			ᘹ.ᘺ => office2007ScrollBarColorTable.MouseOver, 
			ᘹ._0F78 => office2007ScrollBarColorTable.Pressed, 
			_ => null, 
		};
	}

	public override void PaintTrackHorizontal(Graphics g, Rectangle bounds, ᘹ state)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ጧ(state);
		if (office2007ScrollBarStateColorTable == null)
		{
			return;
		}
		ᙈ.ᙘ(office2007ScrollBarStateColorTable.TrackOuterBorder);
		ᙉ.ᙘ(office2007ScrollBarStateColorTable.TrackInnerBorder);
		ᙊ.ᙚ = office2007ScrollBarStateColorTable.TrackBackground;
		ᙊ.ᙗ = 90;
		ᙇ.Paint(new ᙰ(g, bounds));
		if (bounds.Width <= Dpi.Width10 || office2007ScrollBarStateColorTable.TrackSignBackground.Start.IsEmpty)
		{
			return;
		}
		Rectangle rectangle = new Rectangle(bounds.X + (bounds.Width - Dpi.Width7) / 2, bounds.Y + (bounds.Height - Dpi.Height8) / 2, Dpi.Width8, Dpi.Height8);
		Pen val = new Pen(office2007ScrollBarStateColorTable.TrackSignBackground.Start, (float)Dpi.Width1);
		Pen val2 = null;
		if (!office2007ScrollBarStateColorTable.TrackSignBackground.End.IsEmpty)
		{
			val2 = new Pen(office2007ScrollBarStateColorTable.TrackSignBackground.End, (float)Dpi.Width1);
		}
		for (int i = 0; i < 4; i++)
		{
			g.DrawLine(val, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
			rectangle.X += Dpi.Width1;
			if (val2 != null)
			{
				g.DrawLine(val2, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
			}
			rectangle.X += Dpi.Width1;
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	public override void PaintTrackVertical(Graphics g, Rectangle bounds, ᘹ state)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ጧ(state);
		if (office2007ScrollBarStateColorTable == null)
		{
			return;
		}
		ᙈ.ᙘ(office2007ScrollBarStateColorTable.TrackOuterBorder);
		ᙉ.ᙘ(office2007ScrollBarStateColorTable.TrackInnerBorder);
		ᙊ.ᙚ = office2007ScrollBarStateColorTable.TrackBackground;
		ᙊ.ᙗ = 0;
		ᙇ.Paint(new ᙰ(g, bounds));
		if (bounds.Height <= Dpi.Height10 || office2007ScrollBarStateColorTable.TrackSignBackground.Start.IsEmpty)
		{
			return;
		}
		Rectangle rectangle = new Rectangle(bounds.X + (bounds.Width - Dpi.Width8) / 2, bounds.Y + (bounds.Height - Dpi.Height6) / 2, Dpi.Width8, Dpi.Height8);
		Pen val = new Pen(office2007ScrollBarStateColorTable.TrackSignBackground.Start, (float)Dpi.Width1);
		Pen val2 = null;
		if (!office2007ScrollBarStateColorTable.TrackSignBackground.End.IsEmpty)
		{
			val2 = new Pen(office2007ScrollBarStateColorTable.TrackSignBackground.End, (float)Dpi.Width1);
		}
		for (int i = 0; i < 4; i++)
		{
			g.DrawLine(val, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y);
			rectangle.Y += Dpi.Height1;
			if (val2 != null)
			{
				g.DrawLine(val2, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y);
			}
			rectangle.Y += Dpi.Height1;
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	public override void PaintBackground(Graphics g, Rectangle bounds, ᘹ state, bool horizontal, bool sideBorderOnly, bool rtl)
	{
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ጧ(state);
		if (office2007ScrollBarStateColorTable == null)
		{
			return;
		}
		if (sideBorderOnly)
		{
			ᙌ.ᙘ(null);
		}
		else
		{
			ᙌ.ᙘ(office2007ScrollBarStateColorTable.Border);
		}
		ᙍ.ᙘ(office2007ScrollBarStateColorTable.Background);
		ᙍ.ᙗ = (horizontal ? 90 : 0);
		ᙋ.Paint(new ᙰ(g, bounds));
		if (sideBorderOnly && !office2007ScrollBarStateColorTable.Border.Start.IsEmpty)
		{
			if (horizontal)
			{
				DisplayHelp.DrawLine(g, bounds.X, bounds.Y, bounds.Right, bounds.Y, office2007ScrollBarStateColorTable.Border.Start, 1);
			}
			else if (rtl)
			{
				DisplayHelp.DrawLine(g, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom, office2007ScrollBarStateColorTable.Border.Start, 1);
			}
			else
			{
				DisplayHelp.DrawLine(g, 0, bounds.Y, 0, bounds.Bottom, office2007ScrollBarStateColorTable.Border.Start, 1);
			}
		}
	}

	private ᙝ ᙎ()
	{
		ᙸ ᙸ = new ᙸ();
		ᙸ.ᙴ = ᙌ;
		ᙸ.ᙵ = ᙍ;
		ᙸ.ᙟ = new ᙯ(1, 1, 1, 1);
		return ᙸ;
	}

	private ᙝ ᙏ()
	{
		ᙸ ᙸ = new ᙸ();
		ᙸ.ᙴ = ᙀ;
		ᙸ.ᙟ = new ᙯ(1, 1, 1, 1);
		ᙸ ᙸ2 = new ᙸ();
		ᙸ2.ᙟ = new ᙯ(1, 1, 1, 1);
		ᙸ2.ᙴ = ᙁ;
		ᙸ2.ᙵ = ᙂ;
		ᙸ.ᙠ._0CCB(ᙸ2);
		ᙅ = new ᙲ();
		ᙅ.ᙵ = ᙃ;
		ᙸ2.ᙠ._0CCB(ᙅ);
		return ᙸ;
	}

	private ᙝ ᙐ()
	{
		ᙸ ᙸ = new ᙸ();
		ᙸ.ᙹ = eCornerType.Rounded;
		ᙸ.ଖ = 1;
		ᙸ.ᙴ = ᙈ;
		ᙸ.ᙟ = new ᙯ(1, 1, 1, 1);
		ᙸ ᙸ2 = new ᙸ();
		ᙸ2.ᙟ = new ᙯ(1, 1, 1, 1);
		ᙸ2.ᙴ = ᙉ;
		ᙸ2.ᙵ = ᙊ;
		ᙸ.ᙠ._0CCB(ᙸ2);
		return ᙸ;
	}

	private GraphicsPath ᙑ(ᘸ ᙒ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Size size = Dpi.Size(ᙆ);
		if (ᙒ == ᘸ._0749 || ᙒ == ᘸ.ሪ)
		{
			int width = size.Width;
			size.Width = size.Height;
			size.Height = width;
		}
		switch (ᙒ)
		{
		case ᘸ._074A:
			val.AddPolygon(new PointF[6]
			{
				new PointF(-1f, size.Height),
				new PointF(size.Width / 2, -1f),
				new PointF(size.Width / 2, -1f),
				new PointF(size.Width, size.Height),
				new PointF(size.Width, size.Height),
				new PointF(-1f, size.Height)
			});
			val.CloseAllFigures();
			break;
		case ᘸ.ራ:
			val.AddLine(size.Width / 2, size.Height + 1, size.Width, 1);
			val.AddLine(size.Width, 1, 0, 1);
			val.AddLine(0, 1, size.Width / 2, size.Height + 1);
			val.CloseAllFigures();
			break;
		case ᘸ._0749:
		{
			size.Height++;
			int num = size.Height / 2;
			val.AddLine(0, num, size.Width, 0);
			val.AddLine(size.Width, 0, size.Width, size.Height);
			val.AddLine(size.Width, size.Height, 0, num);
			val.CloseAllFigures();
			break;
		}
		case ᘸ.ሪ:
			size.Height++;
			val.AddLine(size.Width, size.Height / 2, 0, 0);
			val.AddLine(0, 0, 0, size.Height);
			val.AddLine(0, size.Height, size.Width, size.Height / 2);
			val.CloseAllFigures();
			break;
		}
		return val;
	}
}
