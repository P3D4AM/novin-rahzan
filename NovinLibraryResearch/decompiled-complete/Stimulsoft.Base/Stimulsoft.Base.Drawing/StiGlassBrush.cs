using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[RefreshProperties(RefreshProperties.All)]
[JsonObject]
public class StiGlassBrush : StiBrush
{
	private float blend;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a color of this StiGlassBrush object.")]
	[RefreshProperties(RefreshProperties.All)]
	public Color Color { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates draw hatch at background or not.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(190)]
	public virtual bool DrawHatch { get; set; } = true;


	[StiSerializable]
	[DefaultValue(0.2f)]
	[Description("Gets or sets a blend factor.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(190)]
	public virtual float Blend
	{
		get
		{
			return blend;
		}
		set
		{
			if (blend != value)
			{
				if (!(value >= 0f) || !(value <= 1f))
				{
					throw new ArgumentOutOfRangeException("Value must be in range between 0 and 1.");
				}
				blend = value;
			}
		}
	}

	public override StiBrushIdent Ident => StiBrushIdent.Glass;

	private bool ShouldSerializeColor()
	{
		return Color != Color.Silver;
	}

	protected bool Equals(StiGlassBrush other)
	{
		if (Color.Equals((object?)other.Color) && DrawHatch.Equals(other.DrawHatch))
		{
			return blend.Equals(other.blend);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj.GetType() == GetType())
				{
					return Equals((StiGlassBrush)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = "StiGlassBrush".GetHashCode();
		hashCode = (hashCode * 397) ^ Color.GetHashCode();
		hashCode = (hashCode * 397) ^ DrawHatch.GetHashCode();
		return (hashCode * 397) ^ blend.GetHashCode();
	}

	public Color GetTopColor()
	{
		return StiColorUtils.Light(Color, (byte)(64f * Blend));
	}

	public Color GetTopColorLight()
	{
		return StiColorUtils.Light(StiColorUtils.Light(Color, (byte)(64f * Blend)), 5);
	}

	public Color GetBottomColor()
	{
		return Color;
	}

	public Color GetBottomColorLight()
	{
		return StiColorUtils.Light(GetBottomColor(), 2);
	}

	public Brush GetTopBrush()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (!DrawHatch)
		{
			return (Brush)new SolidBrush(GetTopColor());
		}
		return (Brush)new HatchBrush((HatchStyle)20, GetTopColor(), GetTopColorLight());
	}

	public Brush GetBottomBrush()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (!DrawHatch)
		{
			return (Brush)new SolidBrush(GetBottomColor());
		}
		return (Brush)new HatchBrush((HatchStyle)20, GetBottomColor(), GetBottomColorLight());
	}

	public RectangleF GetTopRectangle(RectangleF rect)
	{
		RectangleF result = rect;
		result.Height /= 2f;
		if (result.Height * 2f < rect.Height)
		{
			result.Height++;
		}
		return result;
	}

	public RectangleF GetBottomRectangle(RectangleF rect)
	{
		RectangleF topRectangle = GetTopRectangle(rect);
		RectangleF result = rect;
		result.Height = rect.Height - topRectangle.Height;
		result.Y = topRectangle.Bottom;
		return result;
	}

	public void Draw(Graphics g, RectangleF rect)
	{
		RectangleF topRectangle = GetTopRectangle(rect);
		RectangleF bottomRectangle = GetBottomRectangle(rect);
		Brush topBrush = GetTopBrush();
		try
		{
			Brush bottomBrush = GetBottomBrush();
			try
			{
				g.FillRectangle(topBrush, topRectangle);
				g.FillRectangle(bottomBrush, bottomRectangle);
			}
			finally
			{
				((IDisposable)bottomBrush)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)topBrush)?.Dispose();
		}
	}

	public void LoadValuesFromJson(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "DrawHatch":
				DrawHatch = item.DeserializeBool();
				break;
			case "Blend":
				blend = item.DeserializeFloat();
				break;
			}
		}
	}

	public StiGlassBrush()
	{
		Color = Color.Silver;
		DrawHatch = true;
		blend = 0.2f;
	}

	public StiGlassBrush(Color color, bool drawHatch, float blend)
	{
		Color = color;
		DrawHatch = drawHatch;
		this.blend = blend;
	}
}
