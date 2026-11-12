using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[Editor("Stimulsoft.Base.Drawing.Design.StiSimpleBorderEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
[TypeConverter(typeof(StiSimpleBorderConverter))]
[RefreshProperties(RefreshProperties.All)]
[JsonConverter(typeof(StiSimpleBorderJsonConverter))]
[StiReferenceIgnore]
[JsonObject]
public class StiSimpleBorder : ICloneable
{
	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsTopBorderSidePresent => (Side & StiBorderSides.Top) != 0;

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsBottomBorderSidePresent => (Side & StiBorderSides.Bottom) != 0;

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsLeftBorderSidePresent => (Side & StiBorderSides.Left) != 0;

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsRightBorderSidePresent => (Side & StiBorderSides.Right) != 0;

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsAllBorderSidesPresent => Side == StiBorderSides.All;

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsNoneBorderSidesPresent => Side == StiBorderSides.None;

	[Editor("Stimulsoft.Base.Drawing.Design.StiBorderSidesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue(StiBorderSides.None)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets frame borders.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(300)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiBorderSides Side { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a border color.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(100)]
	public virtual Color Color { get; set; } = Color.Gray;


	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets a border size.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(400)]
	public virtual double Size { get; set; } = 1.0;


	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue(StiPenStyle.Solid)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a border style.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(500)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiPenStyle Style { get; set; }

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsDefault
	{
		get
		{
			if (Side == StiBorderSides.None && Color == Color.Gray && Size == 1.0)
			{
				return Style == StiPenStyle.Solid;
			}
			return false;
		}
	}

	public virtual object Clone()
	{
		return (StiSimpleBorder)MemberwiseClone();
	}

	public override bool Equals(object obj)
	{
		StiSimpleBorder stiSimpleBorder = (StiSimpleBorder)obj;
		if (stiSimpleBorder != null && stiSimpleBorder.Color == Color && stiSimpleBorder.Side == Side && stiSimpleBorder.Size == Size)
		{
			return stiSimpleBorder.Style == Style;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public StiBorder GetBorder()
	{
		return new StiBorder(Side, Color, Size, Style);
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

	internal float GetSizeIncludingSide()
	{
		if (Style == StiPenStyle.None)
		{
			return 0f;
		}
		if (Style == StiPenStyle.Double)
		{
			return 3f;
		}
		if (Side == StiBorderSides.None)
		{
			return 0f;
		}
		return (float)Size;
	}

	public void Draw(Graphics g, RectangleD rect, double zoom)
	{
		Draw(g, rect, zoom, Color.White);
	}

	public void Draw(Graphics g, RectangleD rect, double zoom, Color emptyColor)
	{
		Draw(g, rect.ToRectangleF(), (float)zoom, emptyColor);
	}

	public void Draw(Graphics g, RectangleF rect, float zoom)
	{
		Draw(g, rect, zoom, Color.White);
	}

	public virtual void Draw(Graphics g, RectangleF rect, float zoom, Color emptyColor)
	{
		Draw(g, rect, zoom, emptyColor, drawBorderFormatting: true, drawBorderSides: true);
	}

	public virtual void Draw(Graphics g, RectangleF rect, float zoom, Color emptyColor, bool drawBorderFormatting, bool drawBorderSides)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		if (IsDefault || !drawBorderSides)
		{
			return;
		}
		Pen val = null;
		Pen val2 = new Pen(Color);
		try
		{
			if (Style == StiPenStyle.Double)
			{
				val = new Pen(emptyColor);
			}
			val2.DashStyle = StiPenUtils.GetPenStyle(Style);
			if (Style == StiPenStyle.None)
			{
				return;
			}
			val2.Width = (int)(Size * (double)zoom);
			val2.StartCap = (LineCap)1;
			val2.EndCap = (LineCap)1;
			RectangleF rectangleF = rect;
			RectangleF rectangleF2 = rect;
			if (Style == StiPenStyle.Double)
			{
				rectangleF.Inflate(-1f, -1f);
				rectangleF2.Inflate(1f, 1f);
				val2.Width = 1f;
			}
			if (IsAllBorderSidesPresent)
			{
				if (Style == StiPenStyle.Double)
				{
					g.DrawRectangle(val, rect.X, rect.Y, rect.Width, rect.Height);
					g.DrawRectangle(val2, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
					g.DrawRectangle(val2, rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
				}
				else
				{
					g.DrawRectangle(val2, rect.X, rect.Y, rect.Width, rect.Height);
				}
			}
			else
			{
				float num = 0f;
				float num2 = 0f;
				if (IsTopBorderSidePresent)
				{
					num = rectangleF.Left;
					num2 = rectangleF.Right;
					if (!IsLeftBorderSidePresent)
					{
						num = rectangleF2.Left;
					}
					if (!IsRightBorderSidePresent)
					{
						num2 = rectangleF2.Right;
					}
					if (Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Left, rect.Top, rect.Right, rect.Top);
						g.DrawLine(val2, num, rectangleF.Top, num2, rectangleF.Top);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Top, rectangleF2.Right, rectangleF2.Top);
					}
					else
					{
						g.DrawLine(val2, rect.Left, rect.Top, rect.Right, rect.Top);
					}
				}
				float num3 = 0f;
				float num4 = 0f;
				if (IsLeftBorderSidePresent)
				{
					num3 = rectangleF.Top;
					num4 = rectangleF.Bottom;
					if (!IsTopBorderSidePresent)
					{
						num3 = rectangleF2.Top;
					}
					if (!IsBottomBorderSidePresent)
					{
						num4 = rectangleF2.Bottom;
					}
					if (Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Left, rect.Top, rect.Left, rect.Bottom);
						g.DrawLine(val2, rectangleF.Left, num3, rectangleF.Left, num4);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Top, rectangleF2.Left, rectangleF2.Bottom);
					}
					else
					{
						g.DrawLine(val2, rect.Left, rect.Top, rect.Left, rect.Bottom);
					}
				}
				if (IsBottomBorderSidePresent)
				{
					num = rectangleF.Left;
					num2 = rectangleF.Right;
					if (!IsLeftBorderSidePresent)
					{
						num = rectangleF2.Left;
					}
					if (!IsRightBorderSidePresent)
					{
						num2 = rectangleF2.Right;
					}
					if (Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
						g.DrawLine(val2, num, rectangleF.Bottom, num2, rectangleF.Bottom);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Bottom, rectangleF2.Right, rectangleF2.Bottom);
					}
					else
					{
						g.DrawLine(val2, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
					}
				}
				if (IsRightBorderSidePresent)
				{
					num3 = rectangleF.Top;
					num4 = rectangleF.Bottom;
					if (!IsTopBorderSidePresent)
					{
						num3 = rectangleF2.Top;
					}
					if (!IsBottomBorderSidePresent)
					{
						num4 = rectangleF2.Bottom;
					}
					if (Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Right, rect.Top, rect.Right, rect.Bottom);
						g.DrawLine(val2, rectangleF.Right, num3, rectangleF.Right, num4);
						g.DrawLine(val2, rectangleF2.Right, rectangleF2.Top, rectangleF2.Right, rectangleF2.Bottom);
					}
					else
					{
						g.DrawLine(val2, rect.Right, rect.Top, rect.Right, rect.Bottom);
					}
				}
			}
			if (val != null)
			{
				val.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Gray;
	}

	public void LoadFromJson(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Side":
				Side = item.DeserializeEnum<StiBorderSides>();
				break;
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "Size":
				Size = item.DeserializeDouble();
				break;
			case "Style":
				Style = item.DeserializeEnum<StiPenStyle>();
				break;
			}
		}
	}

	public StiSimpleBorder()
		: this(StiBorderSides.None, Color.Gray, 1.0, StiPenStyle.Solid)
	{
	}

	public StiSimpleBorder(StiBorderSides side, Color color, double size, StiPenStyle style)
	{
		Side = side;
		Color = color;
		Size = size;
		Style = style;
	}
}
