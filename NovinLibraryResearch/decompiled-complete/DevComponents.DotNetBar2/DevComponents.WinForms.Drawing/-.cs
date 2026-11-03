using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar;

namespace DevComponents.WinForms.Drawing;

internal enum _1366
{
	ቹ,
	ቺ,
	ቻ
}
internal class _2783 : ᙝ
{
	private Border __128F;

	private Fill __2785;

	private CornerRadius __2786;

	[Description("Indicates shape border.")]
	[DefaultValue(null)]
	public Border ᙴ
	{
		get
		{
			return __128F;
		}
		set
		{
			__128F = value;
		}
	}

	[Description("Indicates shape fill")]
	[DefaultValue(null)]
	public Fill ᙵ
	{
		get
		{
			return __2785;
		}
		set
		{
			__2785 = value;
		}
	}

	public CornerRadius _2784
	{
		get
		{
			return __2786;
		}
		set
		{
			__2786 = value;
		}
	}

	public override void Paint(Graphics g, Rectangle bounds)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		if (bounds.Width < 2 || bounds.Height < 2 || g == null || (__2785 == null && __128F == null))
		{
			return;
		}
		GraphicsPath val = null;
		if (!__2786.ኺ)
		{
			val = DisplayHelp.GetRoundedRectanglePath(bounds, __2786.TopLeft, __2786.TopRight, __2786.BottomRight, __2786.BottomLeft);
		}
		if (__2785 != null)
		{
			Brush val2 = __2785.CreateBrush(bounds);
			if (val2 != null)
			{
				SmoothingMode smoothingMode = g.SmoothingMode;
				if (val2 is SolidBrush && val == null)
				{
					g.SmoothingMode = (SmoothingMode)3;
				}
				if (val == null)
				{
					g.FillRectangle(val2, bounds);
				}
				else
				{
					g.FillPath(val2, val);
				}
				g.SmoothingMode = smoothingMode;
				val2.Dispose();
			}
		}
		if (__128F != null)
		{
			Pen val3 = __128F.CreatePen();
			if (val3 != null)
			{
				if (val == null)
				{
					g.DrawRectangle(val3, bounds);
				}
				else
				{
					g.DrawPath(val3, val);
				}
				val3.Dispose();
			}
		}
		ᙝ ᙝ2 = base._277F;
		if (ᙝ2 != null)
		{
			Rectangle bounds2 = Border.Deflate(bounds, __128F);
			Region val4 = null;
			if (val != null && base._2780)
			{
				val4 = g.Clip;
				g.SetClip(val, (CombineMode)1);
			}
			ᙝ2.Paint(g, bounds2);
			if (val4 != null)
			{
				g.Clip = val4;
			}
		}
		if (val != null)
		{
			val.Dispose();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCornerRadius()
	{
		return !__2786.ኺ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCornerRadius()
	{
		_2784 = default(CornerRadius);
	}
}
