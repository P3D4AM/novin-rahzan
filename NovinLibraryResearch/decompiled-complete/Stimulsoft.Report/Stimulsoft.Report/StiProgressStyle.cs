using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Images;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiProgressStyle : StiBaseStyle
{
	private Color[] DefaultSeriesColors = new Color[5]
	{
		ColorTranslator.FromHtml("#3498db"),
		ColorTranslator.FromHtml("#ef717a"),
		ColorTranslator.FromHtml("#6dcbb3"),
		ColorTranslator.FromHtml("#f28161"),
		ColorTranslator.FromHtml("#fccd1b")
	};

	public override StiComponentId ComponentId => StiComponentId.StiIndicatorStyle;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color TrackColor { get; set; } = ColorTranslator.FromHtml("#e7ebec");


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BandColor { get; set; } = ColorTranslator.FromHtml("#3498db");


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorsConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color[] SeriesColors { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color ForeColor { get; set; } = ColorTranslator.FromHtml("#8c8c8c");


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BackColor { get; set; } = Color.White;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "TrackColor", TrackColor, "#e7ebec");
		JsonExtensions.AddPropertyColor(val, "BandColor", BandColor, "#3498db");
		JsonExtensions.AddPropertyArrayColor(val, "SeriesColors", SeriesColors);
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, "#8c8c8c");
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.White);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "TrackColor":
				TrackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BandColor":
				BandColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SeriesColors":
				SeriesColors = StiJsonReportObjectHelper.DeserializeArrayColor(item);
				break;
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
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
		value = new StiPropertyObject[5]
		{
			propertiesHelper.BackColor(),
			propertiesHelper.GlyphColor(),
			propertiesHelper.ForeColor(),
			propertiesHelper.HotBackColor(),
			propertiesHelper.HotForeColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiProgressStyle stiProgressStyle = base.Clone() as StiProgressStyle;
		stiProgressStyle.SeriesColors = SeriesColors?.ToArray();
		return stiProgressStyle;
	}

	private bool ShouldSerializeTrackColor()
	{
		return TrackColor != ColorTranslator.FromHtml("#e7ebec");
	}

	private bool ShouldSerializeBandColor()
	{
		return BandColor != ColorTranslator.FromHtml("#3498db");
	}

	private bool ShouldSerializeSeriesColors()
	{
		if (SeriesColors != null && SeriesColors.Length == DefaultSeriesColors.Length)
		{
			return !SeriesColors.ToList().SequenceEqual(DefaultSeriesColors);
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != ColorTranslator.FromHtml("#8c8c8c");
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		Image val = StiReportImages.Styles.Progress((StiImageSize)2);
		Rectangle rectangle = new Rectangle(rect.X + StiScaleUI.I10 + (StiScaleUI.I(32) - val.Width) / 2, rect.Y + (rect.Height - val.Width) / 2, val.Width, val.Height);
		g.DrawImage(val, rectangle);
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
		SolidBrush val2 = new SolidBrush(Color.Black);
		try
		{
			Font val3 = new Font("Arial", 8f);
			try
			{
				StringFormat val4 = new StringFormat();
				try
				{
					val4.Alignment = (StringAlignment)0;
					val4.LineAlignment = (StringAlignment)1;
					g.DrawString(Name, val3, (Brush)(object)val2, (RectangleF)rect, val4);
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
		finally
		{
			((IDisposable)val2)?.Dispose();
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
		val.Serialize((object)this, stream, "StiProgressStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiProgressStyle");
	}

	internal StiProgressStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
		SeriesColors = DefaultSeriesColors.Clone() as Color[];
	}

	public StiProgressStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiProgressStyle(string name)
		: this(name, "")
	{
	}

	public StiProgressStyle()
		: this("")
	{
	}
}
