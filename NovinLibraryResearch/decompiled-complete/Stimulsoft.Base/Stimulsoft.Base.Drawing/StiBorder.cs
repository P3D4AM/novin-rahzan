using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[Editor("Stimulsoft.Base.Drawing.Design.StiBorderEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
[TypeConverter(typeof(StiBorderConverter))]
[RefreshProperties(RefreshProperties.All)]
[JsonConverter(typeof(StiBorderJsonConverter))]
[StiReferenceIgnore]
[JsonObject]
public class StiBorder : ICloneable
{
	protected class BitsBorder : ICloneable
	{
		public StiBorderSides Side { get; set; }

		public Color Color { get; set; }

		public double Size { get; set; }

		public StiPenStyle Style { get; set; }

		public double ShadowSize { get; set; }

		public bool DropShadow { get; set; }

		public bool Topmost { get; set; }

		public virtual object Clone()
		{
			return MemberwiseClone();
		}

		protected bool Equals(BitsBorder other)
		{
			if (Side == other.Side && Color.Equals((object?)other.Color) && Size.Equals(other.Size) && Style == other.Style && ShadowSize.Equals(other.ShadowSize) && Topmost == other.Topmost)
			{
				return DropShadow == other.DropShadow;
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
			if (obj is BitsBorder other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int side = (int)Side;
			side = (side * 397) ^ Color.GetHashCode();
			side = (side * 397) ^ Size.GetHashCode();
			side = (side * 397) ^ (int)Style;
			side = (side * 397) ^ ShadowSize.GetHashCode();
			side = (side * 397) ^ Topmost.GetHashCode();
			return (side * 397) ^ DropShadow.GetHashCode();
		}

		public static bool IsDefault(BitsBorder bits)
		{
			return IsDefault(bits.Side, bits.Color, bits.Size, bits.Style, bits.DropShadow, bits.ShadowSize, bits.Topmost);
		}

		public static bool IsDefault(StiBorderSides side, Color color, double size, StiPenStyle style, bool dropShadow, double shadowSize, bool topmost)
		{
			if (color == Color.Black && !dropShadow && !topmost && shadowSize == 4.0 && side == StiBorderSides.None && size == 1.0)
			{
				return style == StiPenStyle.Solid;
			}
			return false;
		}

		public BitsBorder(StiBorderSides side, Color color, double size, StiPenStyle style, double shadowSize, bool dropShadow, bool topmost)
		{
			Side = side;
			Color = color;
			Size = size;
			Style = style;
			ShadowSize = shadowSize;
			DropShadow = dropShadow;
			Topmost = topmost;
		}
	}

	protected BitsBorder bits;

	private static int defaultHashCode = "StiBorder".GetHashCode();

	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual StiBorderIdent Ident => StiBorderIdent.Border;

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsTopBorderSidePresent
	{
		get
		{
			if (bits == null)
			{
				return false;
			}
			return (bits.Side & StiBorderSides.Top) != 0;
		}
	}

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsBottomBorderSidePresent
	{
		get
		{
			if (bits == null)
			{
				return false;
			}
			return (bits.Side & StiBorderSides.Bottom) != 0;
		}
	}

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsLeftBorderSidePresent
	{
		get
		{
			if (bits == null)
			{
				return false;
			}
			return (bits.Side & StiBorderSides.Left) != 0;
		}
	}

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsRightBorderSidePresent
	{
		get
		{
			if (bits == null)
			{
				return false;
			}
			return (bits.Side & StiBorderSides.Right) != 0;
		}
	}

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsAllBorderSidesPresent
	{
		get
		{
			if (bits == null)
			{
				return false;
			}
			return bits.Side == StiBorderSides.All;
		}
	}

	[JsonIgnore]
	private bool IsDefaultShadowBrush
	{
		get
		{
			if (ShadowBrush is StiSolidBrush)
			{
				return ((StiSolidBrush)ShadowBrush).Color == Color.Black;
			}
			return false;
		}
	}

	[Editor("Stimulsoft.Base.Drawing.Design.StiBorderSidesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue(StiBorderSides.None)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets frame borders.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(300)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiBorderSides Side
	{
		get
		{
			if (bits != null)
			{
				return bits.Side;
			}
			return StiBorderSides.None;
		}
		set
		{
			if (value != 0 || bits != null)
			{
				if (bits != null)
				{
					bits.Side = value;
				}
				else
				{
					bits = new BitsBorder(value, Color, Size, Style, ShadowSize, DropShadow, Topmost);
				}
			}
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a border color.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(100)]
	public virtual Color Color
	{
		get
		{
			if (bits != null)
			{
				return bits.Color;
			}
			return Color.Black;
		}
		set
		{
			if (!(value == Color.Black) || bits != null)
			{
				if (bits != null)
				{
					bits.Color = value;
				}
				else
				{
					bits = new BitsBorder(Side, value, Size, Style, ShadowSize, DropShadow, Topmost);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets a border size.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(400)]
	public virtual double Size
	{
		get
		{
			if (bits != null)
			{
				return bits.Size;
			}
			return 1.0;
		}
		set
		{
			if (value == 1.0 && bits == null)
			{
				return;
			}
			if (bits != null)
			{
				if (value >= 0.0)
				{
					bits.Size = value;
				}
			}
			else
			{
				bits = new BitsBorder(Side, Color, value, Style, ShadowSize, DropShadow, Topmost);
			}
		}
	}

	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue(StiPenStyle.Solid)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a border style.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(500)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiPenStyle Style
	{
		get
		{
			if (bits != null)
			{
				return bits.Style;
			}
			return StiPenStyle.Solid;
		}
		set
		{
			if (value != 0 || bits != null)
			{
				if (bits != null)
				{
					bits.Style = value;
				}
				else
				{
					bits = new BitsBorder(Side, Color, Size, value, ShadowSize, DropShadow, Topmost);
				}
			}
		}
	}

	[StiSerializable]
	[Description("Gets or sets the border shadow brush.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(200)]
	public virtual StiBrush ShadowBrush { get; set; }

	[StiSerializable]
	[DefaultValue(4.0)]
	[Description("Gets or sets Shadow Size.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(250)]
	public virtual double ShadowSize
	{
		get
		{
			if (bits != null)
			{
				return bits.ShadowSize;
			}
			return 4.0;
		}
		set
		{
			if (value == 4.0 && bits == null)
			{
				return;
			}
			if (bits != null)
			{
				if (value >= 0.0)
				{
					bits.ShadowSize = value;
				}
			}
			else
			{
				bits = new BitsBorder(Side, Color, Size, Style, value, DropShadow, Topmost);
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates drop shadow or not.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(190)]
	public virtual bool DropShadow
	{
		get
		{
			if (bits != null)
			{
				return bits.DropShadow;
			}
			return false;
		}
		set
		{
			if (value || bits != null)
			{
				if (bits != null)
				{
					bits.DropShadow = value;
				}
				else
				{
					bits = new BitsBorder(Side, Color, Size, Style, ShadowSize, value, Topmost);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that border sides will be drawn on top of all components.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(600)]
	public virtual bool Topmost
	{
		get
		{
			if (bits != null)
			{
				return bits.Topmost;
			}
			return false;
		}
		set
		{
			if (value || bits != null)
			{
				if (bits != null)
				{
					bits.Topmost = value;
				}
				else
				{
					bits = new BitsBorder(Side, Color, Size, Style, ShadowSize, DropShadow, value);
				}
			}
		}
	}

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsDefault
	{
		get
		{
			if (IsDefaultShadowBrush)
			{
				return bits == null;
			}
			return false;
		}
	}

	public virtual object Clone()
	{
		StiBorder stiBorder = (StiBorder)MemberwiseClone();
		if (stiBorder.ShadowBrush != null)
		{
			stiBorder.ShadowBrush = (StiBrush)ShadowBrush.Clone();
		}
		else
		{
			stiBorder.ShadowBrush = null;
		}
		if (bits != null)
		{
			if (!BitsBorder.IsDefault(bits))
			{
				stiBorder.bits = bits.Clone() as BitsBorder;
			}
			else
			{
				stiBorder.bits = null;
			}
		}
		return stiBorder;
	}

	protected bool Equals(StiBorder other)
	{
		if (object.Equals(bits, other.bits))
		{
			return object.Equals(ShadowBrush, other.ShadowBrush);
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
		if (obj is StiBorder other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((defaultHashCode * 397) ^ ((bits != null) ? bits.GetHashCode() : 0)) * 397) ^ ((ShadowBrush != null) ? ShadowBrush.GetHashCode() : 0);
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		if (bits == null || IsDefault)
		{
			return;
		}
		if (drawBorderFormatting)
		{
			DrawBorderShadow(g, rect, zoom);
		}
		if (!drawBorderSides)
		{
			return;
		}
		Pen val = null;
		Pen val2 = new Pen(bits.Color);
		try
		{
			if (bits.Style == StiPenStyle.Double)
			{
				val = new Pen(emptyColor);
			}
			val2.DashStyle = StiPenUtils.GetPenStyle(bits.Style);
			if (bits.Style == StiPenStyle.None)
			{
				return;
			}
			val2.Width = (int)(bits.Size * (double)zoom);
			val2.StartCap = (LineCap)1;
			val2.EndCap = (LineCap)1;
			RectangleF rectangleF = rect;
			RectangleF rectangleF2 = rect;
			if (bits.Style == StiPenStyle.Double)
			{
				rectangleF.Inflate(-1f, -1f);
				rectangleF2.Inflate(1f, 1f);
				val2.Width = 1f;
			}
			if (IsAllBorderSidesPresent)
			{
				if (bits.Style == StiPenStyle.Double)
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
					if (bits.Style == StiPenStyle.Double)
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
					if (bits.Style == StiPenStyle.Double)
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
					if (bits.Style == StiPenStyle.Double)
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

	public virtual void DrawBorderShadow(Graphics g, RectangleF rect, float zoom)
	{
		if (IsDefault || bits == null || !bits.DropShadow)
		{
			return;
		}
		float num = (float)(bits.ShadowSize * (double)zoom);
		Brush val = StiBrush.GetBrush(rect: new RectangleF(rect.Left + num, rect.Top + num, rect.Width, rect.Height), brush: ShadowBrush);
		try
		{
			g.FillRectangle(val, rect.Right, rect.Top + num, num, rect.Height - num);
			g.FillRectangle(val, rect.Left + num, rect.Bottom, rect.Width, num);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
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
			case "ShadowBrush":
				ShadowBrush = item.DeserializeBrush();
				break;
			case "ShadowSize":
				ShadowSize = item.DeserializeDouble();
				break;
			case "DropShadow":
				DropShadow = item.DeserializeBool();
				break;
			case "Topmost":
				Topmost = item.DeserializeBool();
				break;
			}
		}
	}

	public StiBorder()
		: this(StiBorderSides.None, Color.Black, 1.0, StiPenStyle.Solid, dropShadow: false, 4.0, new StiSolidBrush(Color.Black))
	{
	}

	public StiBorder(StiBorderSides side, Color color, double size, StiPenStyle style)
		: this(side, color, size, style, dropShadow: false, 4.0, new StiSolidBrush(Color.Black))
	{
	}

	public StiBorder(StiBorderSides side, Color color, double size, StiPenStyle style, bool dropShadow, double shadowSize, StiBrush shadowBrush)
		: this(side, color, size, style, dropShadow, shadowSize, shadowBrush, topmost: false)
	{
	}

	public StiBorder(StiBorderSides side, Color color, double size, StiPenStyle style, bool dropShadow, double shadowSize, StiBrush shadowBrush, bool topmost)
	{
		bits = (BitsBorder.IsDefault(side, color, size, style, dropShadow, shadowSize, topmost) ? null : new BitsBorder(side, color, size, style, shadowSize, dropShadow, topmost));
		ShadowBrush = shadowBrush;
	}
}
