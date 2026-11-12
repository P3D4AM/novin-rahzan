using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[JsonObject]
public class StiBorderSide : ICloneable
{
	internal StiBorderSides side;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a border color.")]
	[RefreshProperties(RefreshProperties.All)]
	public virtual Color Color { get; set; } = Color.Black;


	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets a border size.")]
	[RefreshProperties(RefreshProperties.All)]
	public virtual double Size { get; set; } = 1.0;


	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue(StiPenStyle.None)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a border style.")]
	[RefreshProperties(RefreshProperties.All)]
	public virtual StiPenStyle Style { get; set; } = StiPenStyle.None;


	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Color == Color.Black && Size == 1.0)
			{
				return Style == StiPenStyle.None;
			}
			return false;
		}
	}

	public object Clone()
	{
		StiBorderSide stiBorderSide = MemberwiseClone() as StiBorderSide;
		stiBorderSide.Style = Style;
		return stiBorderSide;
	}

	protected bool Equals(StiBorderSide other)
	{
		if (Size.Equals(other.Size) && Color.Equals((object?)other.Color) && side == other.side)
		{
			return Style == other.Style;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		StiBorderSide stiBorderSide = obj as StiBorderSide;
		if (stiBorderSide != null)
		{
			return Equals(stiBorderSide);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = Size.GetHashCode();
		hashCode = (hashCode * 397) ^ Color.GetHashCode();
		hashCode = (hashCode * 397) ^ (int)side;
		return (hashCode * 397) ^ (int)Style;
	}

	public static bool operator ==(StiBorderSide left, StiBorderSide right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(StiBorderSide left, StiBorderSide right)
	{
		return !object.Equals(left, right);
	}

	public float GetSizeOffset()
	{
		if (Style == StiPenStyle.None)
		{
			return 0f;
		}
		if (Style == StiPenStyle.Double)
		{
			return 1f;
		}
		return (float)Size / 2f;
	}

	internal float GetSize()
	{
		if (Style == StiPenStyle.None)
		{
			return 0f;
		}
		if (Style == StiPenStyle.Double)
		{
			return 3f;
		}
		return (float)Size;
	}

	internal void Draw(Graphics g, RectangleF rect, float zoom, Color emptyColor, StiAdvancedBorder border)
	{
		if (Style != StiPenStyle.None)
		{
			if (Style == StiPenStyle.Double)
			{
				DrawDoubleInternal(g, rect, zoom, emptyColor, border);
			}
			else if (Style == StiPenStyle.Solid && Size >= 1.0)
			{
				DrawSolidInternal(g, rect, zoom, border);
			}
			else
			{
				DrawInternal(g, rect, zoom, border);
			}
		}
	}

	internal void DrawInternal(Graphics g, RectangleF rect, float zoom, StiAdvancedBorder border)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Pen val = new Pen(Color);
		try
		{
			val.DashStyle = StiPenUtils.GetPenStyle(Style);
			val.Width = (int)(Size * (double)zoom);
			val.StartCap = (LineCap)16;
			val.EndCap = (LineCap)16;
			switch (side)
			{
			case StiBorderSides.Top:
				if (border.IsTopBorderSidePresent)
				{
					g.DrawLine(val, rect.Left, rect.Top, rect.Right, rect.Top);
				}
				break;
			case StiBorderSides.Bottom:
				if (border.IsBottomBorderSidePresent)
				{
					g.DrawLine(val, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
				}
				break;
			case StiBorderSides.Left:
				if (border.IsLeftBorderSidePresent)
				{
					g.DrawLine(val, rect.Left, rect.Top, rect.Left, rect.Bottom);
				}
				break;
			case StiBorderSides.Right:
				if (border.IsRightBorderSidePresent)
				{
					g.DrawLine(val, rect.Right, rect.Top, rect.Right, rect.Bottom);
				}
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal void DrawSolidInternal(Graphics g, RectangleF rect, float zoom, StiAdvancedBorder border)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(Color);
		try
		{
			float num = (float)(Size * (double)zoom);
			rect = GetAdvancedBorderRectangle(g, rect, zoom, border);
			switch (side)
			{
			case StiBorderSides.Top:
				if (border.IsTopBorderSidePresent)
				{
					g.FillRectangle((Brush)(object)val, rect.Left, rect.Top, rect.Width, num);
				}
				break;
			case StiBorderSides.Bottom:
				if (border.IsBottomBorderSidePresent)
				{
					g.FillRectangle((Brush)(object)val, rect.Left, rect.Bottom - num, rect.Width, num);
				}
				break;
			case StiBorderSides.Left:
				if (border.IsLeftBorderSidePresent)
				{
					g.FillRectangle((Brush)(object)val, rect.Left, rect.Top, num, rect.Height);
				}
				break;
			case StiBorderSides.Right:
				if (border.IsRightBorderSidePresent)
				{
					g.FillRectangle((Brush)(object)val, rect.Right - num, rect.Top, num, rect.Height);
				}
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawDoubleInternal(Graphics g, RectangleF rect, float zoom, Color emptyColor, StiAdvancedBorder border)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Pen val = new Pen(emptyColor);
		try
		{
			Pen val2 = new Pen(Color, 1f);
			try
			{
				val2.DashStyle = StiPenUtils.GetPenStyle(Style);
				val2.StartCap = (LineCap)1;
				val2.EndCap = (LineCap)1;
				RectangleF rectangleF = rect;
				RectangleF rectangleF2 = rect;
				rectangleF.Inflate(-1f, -1f);
				rectangleF2.Inflate(1f, 1f);
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				switch (side)
				{
				case StiBorderSides.Top:
					if (border.IsTopBorderSidePresent)
					{
						num = rectangleF.Left;
						num2 = rectangleF.Right;
						if (!border.IsLeftBorderSidePresent)
						{
							num = rectangleF2.Left;
						}
						if (!border.IsRightBorderSidePresent)
						{
							num2 = rectangleF2.Right;
						}
						g.DrawLine(val, rect.Left, rect.Top, rect.Right, rect.Top);
						g.DrawLine(val2, num, rectangleF.Top, num2, rectangleF.Top);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Top, rectangleF2.Right, rectangleF2.Top);
					}
					break;
				case StiBorderSides.Bottom:
					if (border.IsBottomBorderSidePresent)
					{
						num = rectangleF.Left;
						num2 = rectangleF.Right;
						if (!border.IsLeftBorderSidePresent)
						{
							num = rectangleF2.Left;
						}
						if (!border.IsRightBorderSidePresent)
						{
							num2 = rectangleF2.Right;
						}
						g.DrawLine(val, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
						g.DrawLine(val2, num, rectangleF.Bottom, num2, rectangleF.Bottom);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Bottom, rectangleF2.Right, rectangleF2.Bottom);
					}
					break;
				case StiBorderSides.Left:
					if (border.IsLeftBorderSidePresent)
					{
						num3 = rectangleF.Top;
						num4 = rectangleF.Bottom;
						if (!border.IsTopBorderSidePresent)
						{
							num3 = rectangleF2.Top;
						}
						if (!border.IsBottomBorderSidePresent)
						{
							num4 = rectangleF2.Bottom;
						}
						g.DrawLine(val, rect.Left, rect.Top, rect.Left, rect.Bottom);
						g.DrawLine(val2, rectangleF.Left, num3, rectangleF.Left, num4);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Top, rectangleF2.Left, rectangleF2.Bottom);
					}
					break;
				case StiBorderSides.Right:
					if (border.IsRightBorderSidePresent)
					{
						num3 = rectangleF.Top;
						num4 = rectangleF.Bottom;
						if (!border.IsTopBorderSidePresent)
						{
							num3 = rectangleF2.Top;
						}
						if (!border.IsBottomBorderSidePresent)
						{
							num4 = rectangleF2.Bottom;
						}
						g.DrawLine(val, rect.Right, rect.Top, rect.Right, rect.Bottom);
						g.DrawLine(val2, rectangleF.Right, num3, rectangleF.Right, num4);
						g.DrawLine(val2, rectangleF2.Right, rectangleF2.Top, rectangleF2.Right, rectangleF2.Bottom);
					}
					break;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private RectangleF GetAdvancedBorderRectangle(Graphics g, RectangleF rect, float zoom, StiAdvancedBorder border)
	{
		if (border.IsTopBorderSidePresent)
		{
			float num = border.TopSide.GetSizeOffset() * zoom;
			rect.Y -= num;
			rect.Height += num;
		}
		if (border.IsBottomBorderSidePresent)
		{
			float num2 = border.BottomSide.GetSizeOffset() * zoom;
			rect.Height += num2;
		}
		if (border.IsLeftBorderSidePresent)
		{
			float num3 = border.LeftSide.GetSizeOffset() * zoom;
			rect.X -= num3;
			rect.Width += num3;
		}
		if (border.IsRightBorderSidePresent)
		{
			float num4 = border.RightSide.GetSizeOffset() * zoom;
			rect.Width += num4;
		}
		return rect;
	}

	public StiBorderSide()
		: this(Color.Black, 1.0, StiPenStyle.None)
	{
	}

	public StiBorderSide(Color color, double size, StiPenStyle style)
	{
		Color = color;
		Size = size;
		Style = style;
	}
}
