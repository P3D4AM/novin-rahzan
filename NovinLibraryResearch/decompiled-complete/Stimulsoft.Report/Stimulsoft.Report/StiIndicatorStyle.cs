using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Images;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiIndicatorStyle : StiBaseStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiIndicatorStyle;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BackColor { get; set; } = Color.White;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color GlyphColor { get; set; } = ColorTranslator.FromHtml("#3498db");


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color ForeColor { get; set; } = ColorTranslator.FromHtml("#8c8c8c");


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color HotBackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color PositiveColor { get; set; } = Color.Green;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color NegativeColor { get; set; } = Color.Red;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "GlyphColor", GlyphColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, "#8c8c8c");
		JsonExtensions.AddPropertyColor(val, "HotBackColor", HotBackColor, Color.Transparent);
		JsonExtensions.AddPropertyColor(val, "PositiveColor", PositiveColor, Color.Green);
		JsonExtensions.AddPropertyColor(val, "NegativeColor", NegativeColor, Color.Red);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "GlyphColor":
				GlyphColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotBackColor":
				HotBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "PositiveColor":
				PositiveColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "NegativeColor":
				NegativeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.StyleName(),
			propertiesHelper.Description(),
			propertiesHelper.StyleCollectionName(),
			propertiesHelper.StyleConditions()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.BackColor(),
			propertiesHelper.ForeColor(),
			propertiesHelper.GlyphColor(),
			propertiesHelper.HotBackColor(),
			propertiesHelper.NegativeColor(),
			propertiesHelper.PositiveColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		return stiPropertyCollection;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	private bool ShouldSerializeGlyphColor()
	{
		return GlyphColor != ColorTranslator.FromHtml("#3498db");
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != ColorTranslator.FromHtml("#8c8c8c");
	}

	private bool ShouldSerializeHotBackColor()
	{
		return HotBackColor != Color.Transparent;
	}

	private bool ShouldSerializePositiveColor()
	{
		return PositiveColor != Color.Green;
	}

	private bool ShouldSerializeNegativeColor()
	{
		return NegativeColor != Color.Red;
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		if (rect.Height < StiScaleUI.I(32))
		{
			Bitmap indicatorStyle = StylesResource.IndicatorStyle16;
			g.DrawImage((Image)(object)indicatorStyle, new Point(rect.X, rect.Y + (rect.Height - 16) / 2));
		}
		else
		{
			Point point = new Point(rect.X + StiScaleUI.I17, rect.Y + (rect.Height - StiScaleUI.I(32)) / 2 + StiScaleUI.I2);
			GraphicsPath val = new GraphicsPath();
			try
			{
				val.AddLine(point.X + StiScaleUI.I8, point.Y, point.X + 11, point.Y + StiScaleUI.I5);
				val.AddLine(point.X + StiScaleUI.I11, point.Y + StiScaleUI.I5, point.X + StiScaleUI.I16, point.Y + StiScaleUI.I6);
				val.AddLine(point.X + StiScaleUI.I16, point.Y + StiScaleUI.I6, point.X + StiScaleUI.I12, point.Y + StiScaleUI.I10);
				val.AddLine(point.X + StiScaleUI.I12, point.Y + StiScaleUI.I10, point.X + StiScaleUI.I13, point.Y + StiScaleUI.I16);
				val.AddLine(point.X + StiScaleUI.I13, point.Y + StiScaleUI.I16, point.X + StiScaleUI.I8, point.Y + StiScaleUI.I13);
				val.AddLine(point.X + StiScaleUI.I8, point.Y + StiScaleUI.I13, point.X + StiScaleUI.I3, point.Y + StiScaleUI.I16);
				val.AddLine(point.X + StiScaleUI.I3, point.Y + StiScaleUI.I16, point.X + StiScaleUI.I4, point.Y + StiScaleUI.I10);
				val.AddLine(point.X + StiScaleUI.I4, point.Y + StiScaleUI.I10, point.X, point.Y + StiScaleUI.I6);
				val.AddLine(point.X, point.Y + StiScaleUI.I6, point.X + StiScaleUI.I5, point.Y + StiScaleUI.I5);
				val.AddLine(point.X + StiScaleUI.I5, point.Y + StiScaleUI.I5, point.X + StiScaleUI.I8, point.Y);
				SolidBrush val2 = new SolidBrush(GlyphColor);
				try
				{
					g.FillPath((Brush)(object)val2, val);
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
			SolidBrush val3 = new SolidBrush(ForeColor);
			try
			{
				Font val4 = new Font("Arial", 8f);
				try
				{
					StringFormat val5 = new StringFormat();
					try
					{
						val5.Alignment = (StringAlignment)1;
						val5.LineAlignment = (StringAlignment)1;
						g.DrawString("100", val4, (Brush)(object)val3, (RectangleF)new Rectangle(rect.X + StiScaleUI.I9, rect.Y + (rect.Height - StiScaleUI.I(32)) / 2 + StiScaleUI.I20, StiScaleUI.I(32), StiScaleUI.I12), val5);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (rect.Height < StiScaleUI.I(32))
		{
			rect.Width -= StiScaleUI.I(36);
			rect.X += StiScaleUI.I(36);
		}
		else
		{
			rect.Width -= StiScaleUI.I(56);
			rect.X += StiScaleUI.I(56);
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		SolidBrush val6 = new SolidBrush(Color.Black);
		try
		{
			Font val7 = new Font("Arial", 8f);
			try
			{
				StringFormat val8 = new StringFormat();
				try
				{
					val8.Alignment = (StringAlignment)0;
					val8.LineAlignment = (StringAlignment)1;
					g.DrawString(Name, val7, (Brush)(object)val6, (RectangleF)rect, val8);
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
	}

	public override void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		rect.X++;
		rect.Y++;
		rect.Width -= 2;
		rect.Height -= 3;
		DrawStyle(g, rect, paintValue, paintImage);
		Pen val = new Pen(Color.FromArgb(150, Color.Gray));
		try
		{
			val.DashStyle = (DashStyle)1;
			g.DrawRectangle(val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements)
	{
		if (styleElements != StiStyleElements.All)
		{
			throw new Exception("StiIndicatorStyle support only StiStyleElements.All.");
		}
	}

	public override void SetStyleToComponent(StiComponent component)
	{
	}

	public override void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiIndicatorStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiIndicatorStyle");
	}

	internal StiIndicatorStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
	}

	public StiIndicatorStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiIndicatorStyle(string name)
		: this(name, "")
	{
	}

	public StiIndicatorStyle()
		: this("")
	{
	}
}
