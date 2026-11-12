using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

public class StiAdvancedBorder : StiBorder
{
	public override StiBorderIdent Ident => StiBorderIdent.AdvancedBorder;

	[StiSerializable]
	[StiOrder(10)]
	[TypeConverter(typeof(StiBorderSideConverter))]
	[Description("Gets or sets frame of left side.")]
	[RefreshProperties(RefreshProperties.All)]
	public virtual StiBorderSide LeftSide { get; set; }

	[StiSerializable]
	[StiOrder(20)]
	[TypeConverter(typeof(StiBorderSideConverter))]
	[Description("Gets or sets frame of right side.")]
	[RefreshProperties(RefreshProperties.All)]
	public virtual StiBorderSide RightSide { get; set; }

	[StiSerializable]
	[StiOrder(30)]
	[TypeConverter(typeof(StiBorderSideConverter))]
	[Description("Gets or sets frame of top side.")]
	[RefreshProperties(RefreshProperties.All)]
	public virtual StiBorderSide TopSide { get; set; }

	[StiSerializable]
	[StiOrder(40)]
	[TypeConverter(typeof(StiBorderSideConverter))]
	[Description("Gets or sets frame of bottom side.")]
	[RefreshProperties(RefreshProperties.All)]
	public virtual StiBorderSide BottomSide { get; set; }

	public override bool IsTopBorderSidePresent => TopSide.Style != StiPenStyle.None;

	public override bool IsBottomBorderSidePresent => BottomSide.Style != StiPenStyle.None;

	public override bool IsLeftBorderSidePresent => LeftSide.Style != StiPenStyle.None;

	public override bool IsRightBorderSidePresent => RightSide.Style != StiPenStyle.None;

	public override bool IsAllBorderSidesPresent
	{
		get
		{
			if (IsLeftBorderSidePresent && IsRightBorderSidePresent && IsTopBorderSidePresent)
			{
				return IsBottomBorderSidePresent;
			}
			return false;
		}
	}

	public override StiBorderSides Side
	{
		get
		{
			StiBorderSides stiBorderSides = StiBorderSides.None;
			if (IsLeftBorderSidePresent)
			{
				stiBorderSides |= StiBorderSides.Left;
			}
			if (IsRightBorderSidePresent)
			{
				stiBorderSides |= StiBorderSides.Right;
			}
			if (IsTopBorderSidePresent)
			{
				stiBorderSides |= StiBorderSides.Top;
			}
			if (IsBottomBorderSidePresent)
			{
				stiBorderSides |= StiBorderSides.Bottom;
			}
			return stiBorderSides;
		}
		set
		{
			if ((value & StiBorderSides.Left) > StiBorderSides.None)
			{
				if (!IsLeftBorderSidePresent)
				{
					LeftSide.Style = StiPenStyle.Solid;
				}
			}
			else
			{
				LeftSide.Style = StiPenStyle.None;
			}
			if ((value & StiBorderSides.Right) > StiBorderSides.None)
			{
				if (!IsRightBorderSidePresent)
				{
					RightSide.Style = StiPenStyle.Solid;
				}
			}
			else
			{
				RightSide.Style = StiPenStyle.None;
			}
			if ((value & StiBorderSides.Top) > StiBorderSides.None)
			{
				if (!IsTopBorderSidePresent)
				{
					TopSide.Style = StiPenStyle.Solid;
				}
			}
			else
			{
				TopSide.Style = StiPenStyle.None;
			}
			if ((value & StiBorderSides.Bottom) > StiBorderSides.None)
			{
				if (!IsBottomBorderSidePresent)
				{
					BottomSide.Style = StiPenStyle.Solid;
				}
			}
			else
			{
				BottomSide.Style = StiPenStyle.None;
			}
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override Color Color
	{
		get
		{
			return LeftSide.Color;
		}
		set
		{
			LeftSide.Color = value;
			RightSide.Color = value;
			TopSide.Color = value;
			BottomSide.Color = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double Size
	{
		get
		{
			return LeftSide.Size;
		}
		set
		{
			LeftSide.Size = value;
			RightSide.Size = value;
			TopSide.Size = value;
			BottomSide.Size = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiPenStyle Style
	{
		get
		{
			return LeftSide.Style;
		}
		set
		{
			LeftSide.Style = value;
			RightSide.Style = value;
			TopSide.Style = value;
			BottomSide.Style = value;
		}
	}

	public override bool IsDefault
	{
		get
		{
			if (!DropShadow && !Topmost && ShadowSize == 4.0 && ShadowBrush is StiSolidBrush && ((StiSolidBrush)ShadowBrush).Color == Color.Black && LeftSide.IsDefault && RightSide.IsDefault && TopSide.IsDefault)
			{
				return BottomSide.IsDefault;
			}
			return false;
		}
	}

	public override object Clone()
	{
		StiAdvancedBorder stiAdvancedBorder = base.Clone() as StiAdvancedBorder;
		stiAdvancedBorder.TopSide = TopSide.Clone() as StiBorderSide;
		stiAdvancedBorder.BottomSide = BottomSide.Clone() as StiBorderSide;
		stiAdvancedBorder.LeftSide = LeftSide.Clone() as StiBorderSide;
		stiAdvancedBorder.RightSide = RightSide.Clone() as StiBorderSide;
		return stiAdvancedBorder;
	}

	protected bool Equals(StiAdvancedBorder other)
	{
		if (Equals((StiBorder)other) && object.Equals(BottomSide, other.BottomSide) && object.Equals(TopSide, other.TopSide) && object.Equals(LeftSide, other.LeftSide))
		{
			return object.Equals(RightSide, other.RightSide);
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
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((StiAdvancedBorder)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = base.GetHashCode();
		hashCode = (hashCode * 397) ^ ((BottomSide != null) ? BottomSide.GetHashCode() : 0);
		hashCode = (hashCode * 397) ^ ((TopSide != null) ? TopSide.GetHashCode() : 0);
		hashCode = (hashCode * 397) ^ ((LeftSide != null) ? LeftSide.GetHashCode() : 0);
		return (hashCode * 397) ^ ((RightSide != null) ? RightSide.GetHashCode() : 0);
	}

	public static bool operator ==(StiAdvancedBorder left, StiAdvancedBorder right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(StiAdvancedBorder left, StiAdvancedBorder right)
	{
		return !object.Equals(left, right);
	}

	public override void Draw(Graphics g, RectangleF rect, float zoom, Color emptyColor, bool drawBorderFormatting, bool drawBorderSides)
	{
		if (drawBorderFormatting)
		{
			DrawBorderShadow(g, rect, zoom);
		}
		if (drawBorderSides)
		{
			if (IsLeftBorderSidePresent)
			{
				LeftSide.Draw(g, rect, zoom, emptyColor, this);
			}
			if (IsRightBorderSidePresent)
			{
				RightSide.Draw(g, rect, zoom, emptyColor, this);
			}
			if (IsTopBorderSidePresent)
			{
				TopSide.Draw(g, rect, zoom, emptyColor, this);
			}
			if (IsBottomBorderSidePresent)
			{
				BottomSide.Draw(g, rect, zoom, emptyColor, this);
			}
		}
	}

	public StiAdvancedBorder()
		: this(new StiBorderSide(), new StiBorderSide(), new StiBorderSide(), new StiBorderSide(), dropShadow: false, 4.0, new StiSolidBrush(Color.Black))
	{
	}

	public StiAdvancedBorder(Color topSideColor, double topSideSize, StiPenStyle topSideStyle, Color bottomSideColor, double bottomSideSize, StiPenStyle bottomSideStyle, Color leftSideColor, double leftSideSize, StiPenStyle leftSideStyle, Color rightSideColor, double rightSideSize, StiPenStyle rightSideStyle, bool dropShadow, double shadowSize, StiBrush shadowBrush)
		: this(new StiBorderSide(topSideColor, topSideSize, topSideStyle), new StiBorderSide(bottomSideColor, bottomSideSize, bottomSideStyle), new StiBorderSide(leftSideColor, leftSideSize, leftSideStyle), new StiBorderSide(rightSideColor, rightSideSize, rightSideStyle), dropShadow, shadowSize, shadowBrush)
	{
	}

	public StiAdvancedBorder(Color topSideColor, double topSideSize, StiPenStyle topSideStyle, Color bottomSideColor, double bottomSideSize, StiPenStyle bottomSideStyle, Color leftSideColor, double leftSideSize, StiPenStyle leftSideStyle, Color rightSideColor, double rightSideSize, StiPenStyle rightSideStyle, bool dropShadow, double shadowSize, StiBrush shadowBrush, bool topmost)
		: this(new StiBorderSide(topSideColor, topSideSize, topSideStyle), new StiBorderSide(bottomSideColor, bottomSideSize, bottomSideStyle), new StiBorderSide(leftSideColor, leftSideSize, leftSideStyle), new StiBorderSide(rightSideColor, rightSideSize, rightSideStyle), dropShadow, shadowSize, shadowBrush, topmost)
	{
	}

	public StiAdvancedBorder(StiBorderSide topSide, StiBorderSide bottomSide, StiBorderSide leftSide, StiBorderSide rightSide, bool dropShadow, double shadowSize, StiBrush shadowBrush)
		: this(topSide, bottomSide, leftSide, rightSide, dropShadow, shadowSize, shadowBrush, topmost: false)
	{
	}

	public StiAdvancedBorder(StiBorderSide topSide, StiBorderSide bottomSide, StiBorderSide leftSide, StiBorderSide rightSide, bool dropShadow, double shadowSize, StiBrush shadowBrush, bool topmost)
	{
		TopSide = topSide;
		BottomSide = bottomSide;
		LeftSide = leftSide;
		RightSide = rightSide;
		LeftSide.side = StiBorderSides.Left;
		RightSide.side = StiBorderSides.Right;
		TopSide.side = StiBorderSides.Top;
		BottomSide.side = StiBorderSides.Bottom;
		ShadowBrush = shadowBrush;
		ShadowSize = shadowSize;
		DropShadow = dropShadow;
		Topmost = topmost;
	}
}
