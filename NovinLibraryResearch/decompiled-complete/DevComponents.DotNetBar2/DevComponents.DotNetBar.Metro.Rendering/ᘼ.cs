using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Presentation;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar.Metro.Rendering;

internal class ᘼ : ᘷ, ᙓ
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

	public ᘼ()
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
			ᙀ.ܐ = Dpi.Width1;
			ᙀ.ᙘ(office2007ScrollBarStateColorTable.ThumbOuterBorder);
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
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ጧ(state);
		if (office2007ScrollBarStateColorTable != null)
		{
			ᙊ.ᙘ(office2007ScrollBarStateColorTable.TrackSignBackground);
			ᙈ.ܐ = Dpi.Width1;
			ᙈ.ᙘ(office2007ScrollBarStateColorTable.TrackOuterBorder);
			ᙇ.Paint(new ᙰ(g, bounds));
		}
	}

	public override void PaintTrackVertical(Graphics g, Rectangle bounds, ᘹ state)
	{
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ጧ(state);
		if (office2007ScrollBarStateColorTable != null)
		{
			ᙊ.ᙘ(office2007ScrollBarStateColorTable.TrackSignBackground);
			ᙈ.ᙘ(office2007ScrollBarStateColorTable.TrackOuterBorder);
			ᙇ.Paint(new ᙰ(g, bounds));
		}
	}

	public override void PaintBackground(Graphics g, Rectangle bounds, ᘹ state, bool horizontal, bool sideBorderOnly, bool rtl)
	{
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ጧ(state);
		if (office2007ScrollBarStateColorTable == null)
		{
			return;
		}
		ᙌ.ܐ = Dpi.Width1;
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
				DisplayHelp.DrawLine(g, bounds.X, bounds.Y, bounds.Right, bounds.Y, office2007ScrollBarStateColorTable.Border.Start, Dpi.Width1);
			}
			else if (rtl)
			{
				DisplayHelp.DrawLine(g, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom, office2007ScrollBarStateColorTable.Border.Start, Dpi.Width1);
			}
			else
			{
				DisplayHelp.DrawLine(g, 0, bounds.Y, 0, bounds.Bottom, office2007ScrollBarStateColorTable.Border.Start, Dpi.Width1);
			}
		}
	}

	private ᙝ ᙎ()
	{
		ᙸ ᙸ = new ᙸ();
		ᙸ.ᙴ = ᙌ;
		ᙸ.ᙵ = ᙍ;
		return ᙸ;
	}

	private ᙝ ᙏ()
	{
		ᙸ ᙸ = new ᙸ();
		ᙸ.ᙴ = ᙀ;
		ᙸ.ᙟ = new ᙯ(1, 1, 1, 1);
		ᙅ = new ᙲ();
		ᙅ.ᙵ = ᙃ;
		ᙸ.ᙠ._0CCB(ᙅ);
		return ᙸ;
	}

	private ᙝ ᙐ()
	{
		ᙸ ᙸ = new ᙸ();
		ᙸ.ᙟ = new ᙯ(1, 1, 1, 1);
		ᙸ.ᙴ = ᙈ;
		ᙸ ᙸ2 = new ᙸ();
		ᙸ2.ᙟ = new ᙯ(2, 2, 2, 2);
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
				new PointF(-Dpi.Width1, size.Height),
				new PointF(size.Width / 2, -Dpi.Width1),
				new PointF(size.Width / 2, -Dpi.Width1),
				new PointF(size.Width, size.Height),
				new PointF(size.Width, size.Height),
				new PointF(-1f, size.Height)
			});
			val.CloseAllFigures();
			break;
		case ᘸ.ራ:
			val.AddLine(size.Width / 2, size.Height + Dpi.Width1, size.Width, Dpi.Width1);
			val.AddLine(size.Width, Dpi.Width1, 0, Dpi.Width1);
			val.AddLine(0, 1, size.Width / 2, size.Height + Dpi.Width1);
			val.CloseAllFigures();
			break;
		case ᘸ._0749:
		{
			size.Height += Dpi.Height1;
			int num = size.Height / 2;
			val.AddLine(0, num, size.Width, 0);
			val.AddLine(size.Width, 0, size.Width, size.Height);
			val.AddLine(size.Width, size.Height, 0, num);
			val.CloseAllFigures();
			break;
		}
		case ᘸ.ሪ:
			size.Height += Dpi.Height1;
			val.AddLine(size.Width, size.Height / 2, 0, 0);
			val.AddLine(0, 0, 0, size.Height);
			val.AddLine(0, size.Height, size.Width, size.Height / 2);
			val.CloseAllFigures();
			break;
		}
		return val;
	}
}
