using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(StiBrushConverter))]
[RefreshProperties(RefreshProperties.All)]
[Editor("Stimulsoft.Base.Drawing.Design.StiBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
[JsonConverter(typeof(StiBrushJsonConverter))]
[StiReferenceIgnore]
[JsonObject]
public abstract class StiBrush : ICloneable
{
	[Browsable(false)]
	[StiBrowsable(false)]
	public abstract StiBrushIdent Ident { get; }

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			return this == obj;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool IsEmpty(StiBrush brush)
	{
		if (brush != null)
		{
			return brush is StiEmptyBrush;
		}
		return true;
	}

	public static bool IsTransparent(StiBrush brush)
	{
		if (!IsEmpty(brush))
		{
			if (brush is StiSolidBrush)
			{
				return ((StiSolidBrush)brush).Color == Color.Transparent;
			}
			return false;
		}
		return true;
	}

	public Image GetImage(int width, int height)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(width, height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			if (this is StiEmptyBrush)
			{
				val2.Clear(Color.White);
				val2.DrawRectangle(Pens.DarkGray, 0, 0, width - 1, height - 1);
			}
			else
			{
				Rectangle rectangle = new Rectangle(0, 0, width, height);
				Brush brush = GetBrush(this, rectangle);
				try
				{
					val2.FillRectangle(brush, rectangle);
				}
				finally
				{
					((IDisposable)brush)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		return (Image)(object)val;
	}

	public static StiBrush Light(StiBrush baseBrush, byte value)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (baseBrush is StiSolidBrush)
		{
			return new StiSolidBrush(StiColorUtils.Light(((StiSolidBrush)baseBrush).Color, value));
		}
		if (baseBrush is StiGradientBrush)
		{
			StiGradientBrush stiGradientBrush = baseBrush as StiGradientBrush;
			return new StiGradientBrush(StiColorUtils.Light(stiGradientBrush.StartColor, value), StiColorUtils.Light(stiGradientBrush.EndColor, value), stiGradientBrush.Angle);
		}
		if (baseBrush is StiHatchBrush)
		{
			StiHatchBrush stiHatchBrush = baseBrush as StiHatchBrush;
			return new StiHatchBrush(stiHatchBrush.Style, StiColorUtils.Light(stiHatchBrush.ForeColor, value), StiColorUtils.Light(stiHatchBrush.BackColor, value));
		}
		if (baseBrush is StiGlareBrush)
		{
			StiGlareBrush stiGlareBrush = baseBrush as StiGlareBrush;
			return new StiGlareBrush(StiColorUtils.Light(stiGlareBrush.StartColor, value), StiColorUtils.Light(stiGlareBrush.EndColor, value), stiGlareBrush.Angle);
		}
		if (baseBrush is StiGlassBrush)
		{
			StiGlassBrush stiGlassBrush = baseBrush as StiGlassBrush;
			return new StiGlassBrush(StiColorUtils.Light(stiGlassBrush.Color, value), stiGlassBrush.DrawHatch, stiGlassBrush.Blend);
		}
		return baseBrush;
	}

	public static StiBrush Dark(StiBrush baseBrush, byte value)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (baseBrush is StiSolidBrush)
		{
			return new StiSolidBrush(StiColorUtils.Dark(((StiSolidBrush)baseBrush).Color, value));
		}
		if (baseBrush is StiGradientBrush)
		{
			StiGradientBrush stiGradientBrush = baseBrush as StiGradientBrush;
			return new StiGradientBrush(StiColorUtils.Dark(stiGradientBrush.StartColor, value), StiColorUtils.Dark(stiGradientBrush.EndColor, value), stiGradientBrush.Angle);
		}
		if (baseBrush is StiHatchBrush)
		{
			StiHatchBrush stiHatchBrush = baseBrush as StiHatchBrush;
			return new StiHatchBrush(stiHatchBrush.Style, StiColorUtils.Dark(stiHatchBrush.ForeColor, value), StiColorUtils.Dark(stiHatchBrush.BackColor, value));
		}
		if (baseBrush is StiGlareBrush)
		{
			StiGlareBrush stiGlareBrush = baseBrush as StiGlareBrush;
			return new StiGlareBrush(StiColorUtils.Dark(stiGlareBrush.StartColor, value), StiColorUtils.Dark(stiGlareBrush.EndColor, value), stiGlareBrush.Angle);
		}
		if (baseBrush is StiGlassBrush)
		{
			StiGlassBrush stiGlassBrush = baseBrush as StiGlassBrush;
			return new StiGlassBrush(StiColorUtils.Dark(stiGlassBrush.Color, value), stiGlassBrush.DrawHatch, stiGlassBrush.Blend);
		}
		return baseBrush;
	}

	public static Brush GetBrush(StiBrush brush, Rectangle rect)
	{
		return GetBrush(brush, RectangleD.CreateFromRectangle(rect));
	}

	public static Brush GetBrush(StiBrush brush, RectangleF rect)
	{
		return GetBrush(brush, RectangleD.CreateFromRectangle(rect));
	}

	public static Brush GetBrush(StiBrush brush, RectangleD rect)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		if (brush is StiEmptyBrush || brush is StiStyleBrush || brush is StiDefaultBrush)
		{
			return (Brush)new SolidBrush(Color.Transparent);
		}
		if (brush is StiSolidBrush)
		{
			return (Brush)new SolidBrush(((StiSolidBrush)brush).Color);
		}
		if (brush is StiGradientBrush)
		{
			RectangleF rectangleF = rect.ToRectangleF();
			if (rectangleF.Width < 1f)
			{
				rectangleF.Width = 1f;
			}
			if (rectangleF.Height < 1f)
			{
				rectangleF.Height = 1f;
			}
			StiGradientBrush stiGradientBrush = brush as StiGradientBrush;
			if (stiGradientBrush.Angle != 0.0)
			{
				if (stiGradientBrush.Angle != 180.0)
				{
					return (Brush)new LinearGradientBrush(rectangleF, stiGradientBrush.StartColor, stiGradientBrush.EndColor, (float)stiGradientBrush.Angle);
				}
				return (Brush)new LinearGradientBrush(new PointF(rectangleF.Left - 1f, rectangleF.Top - 1f), new PointF(rectangleF.Right + 1f, rectangleF.Bottom + 1f), stiGradientBrush.EndColor, stiGradientBrush.StartColor);
			}
			return (Brush)new LinearGradientBrush(new PointF(rectangleF.Left - 1f, rectangleF.Top - 1f), new PointF(rectangleF.Right + 1f, rectangleF.Bottom + 1f), stiGradientBrush.StartColor, stiGradientBrush.EndColor);
		}
		if (brush is StiHatchBrush)
		{
			StiHatchBrush stiHatchBrush = brush as StiHatchBrush;
			return (Brush)new HatchBrush(stiHatchBrush.Style, stiHatchBrush.ForeColor, stiHatchBrush.BackColor);
		}
		if (brush is StiGlareBrush)
		{
			RectangleF rectangleF2 = rect.ToRectangleF();
			if (rectangleF2.Width < 1f)
			{
				rectangleF2.Width = 1f;
			}
			if (rectangleF2.Height < 1f)
			{
				rectangleF2.Height = 1f;
			}
			StiGlareBrush stiGlareBrush = brush as StiGlareBrush;
			LinearGradientBrush val = new LinearGradientBrush(rectangleF2, stiGlareBrush.StartColor, stiGlareBrush.EndColor, (float)stiGlareBrush.Angle);
			val.SetSigmaBellShape(stiGlareBrush.Focus, stiGlareBrush.Scale);
			return (Brush)(object)val;
		}
		if (brush is StiGlassBrush)
		{
			Bitmap val2 = new Bitmap((int)rect.Width + 1, (int)rect.Height + 1);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				((StiGlassBrush)brush).Draw(val3, new RectangleF(0f, 0f, (float)rect.Width + 1f, (float)rect.Height + 1f));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			TextureBrush val4 = new TextureBrush((Image)(object)val2);
			val4.TranslateTransform((float)rect.X, (float)rect.Y);
			return (Brush)(object)val4;
		}
		return null;
	}

	public static StiBrush LoadFromJson(JObject jObject)
	{
		JProperty jProperty = jObject.Properties().FirstOrDefault((JProperty x) => x.Name == "Ident");
		switch (jProperty.Value.ToObject<string>())
		{
		case "StiStyleBrush":
			return new StiStyleBrush();
		case "StiDefaultBrush":
			return new StiDefaultBrush();
		case "StiEmptyBrush":
			return new StiEmptyBrush();
		case "StiSolidBrush":
		{
			StiSolidBrush stiSolidBrush = new StiSolidBrush();
			stiSolidBrush.LoadValuesFromJson(jObject);
			return stiSolidBrush;
		}
		case "StiGradientBrush":
		{
			StiGradientBrush stiGradientBrush = new StiGradientBrush();
			stiGradientBrush.LoadValuesFromJson(jObject);
			return stiGradientBrush;
		}
		case "StiGlareBrush":
		{
			StiGlareBrush stiGlareBrush = new StiGlareBrush();
			stiGlareBrush.LoadValuesFromJson(jObject);
			return stiGlareBrush;
		}
		case "StiGlassBrush":
		{
			StiGlassBrush stiGlassBrush = new StiGlassBrush();
			stiGlassBrush.LoadValuesFromJson(jObject);
			return stiGlassBrush;
		}
		case "StiHatchBrush":
		{
			StiHatchBrush stiHatchBrush = new StiHatchBrush();
			stiHatchBrush.LoadValuesFromJson(jObject);
			return stiHatchBrush;
		}
		default:
			throw new Exception("Type is not supported!");
		}
	}

	public static Color ToColor(StiBrush brush)
	{
		if (brush is StiStyleBrush)
		{
			return Color.White;
		}
		if (brush is StiDefaultBrush)
		{
			return Color.White;
		}
		if (brush is StiEmptyBrush)
		{
			return Color.Transparent;
		}
		if (brush is StiSolidBrush)
		{
			return ((StiSolidBrush)brush).Color;
		}
		if (brush is StiGradientBrush)
		{
			return ((StiGradientBrush)brush).StartColor;
		}
		if (brush is StiGlareBrush)
		{
			return ((StiGlareBrush)brush).StartColor;
		}
		if (brush is StiGlassBrush)
		{
			return ((StiGlassBrush)brush).Color;
		}
		if (brush is StiHatchBrush)
		{
			return ((StiHatchBrush)brush).ForeColor;
		}
		return Color.Empty;
	}
}
