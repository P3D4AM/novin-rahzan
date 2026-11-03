using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Images;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiMapStyle : StiBaseStyle
{
	private Color[] DefaultColors = new Color[6]
	{
		ColorTranslator.FromHtml("#70ad47"),
		ColorTranslator.FromHtml("#4472c4"),
		ColorTranslator.FromHtml("#ffc000"),
		ColorTranslator.FromHtml("#43682b"),
		ColorTranslator.FromHtml("#fd6a37"),
		ColorTranslator.FromHtml("#997300")
	};

	public override StiComponentId ComponentId => StiComponentId.StiMapStyle;

	[StiNonSerialized]
	[Obsolete("This property is obsoleted. Please use Heatmap/HeatmapWithGroup properties.")]
	public virtual Color[] HeatmapColors
	{
		get
		{
			return null;
		}
		set
		{
			if (value != null && value.Length != 0)
			{
				Heatmap.Color = value[0];
				HeatmapWithGroup.Colors = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiHeatmapStyleData Heatmap { get; set; } = new StiHeatmapStyleData();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiHeatmapWithGroupStyleData HeatmapWithGroup { get; set; } = new StiHeatmapWithGroupStyleData();


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color IndividualColor { get; set; } = ColorTranslator.FromHtml("#70ad47");


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorsConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color[] Colors { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color DefaultColor { get; set; } = ColorTranslator.FromHtml("#4472c4");


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color BackColor { get; set; } = Color.White;


	[StiNonSerialized]
	[Browsable(false)]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Obsolete("ForeColor property is obsolete. Please use LabelForeground property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color ForeColor { get; set; } = Color.White;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0.5)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public double BorderSize { get; set; } = 0.7;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color BorderColor { get; set; } = Color.White;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color LabelShadowForeground { get; set; } = Color.FromArgb(255, 251, 251, 251);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color LabelForeground { get; set; } = Color.FromArgb(255, 37, 37, 37);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color BubbleBackColor { get; set; } = Color.Red;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color BubbleBorderColor { get; set; } = Color.White;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		if (Heatmap != null)
		{
			JsonExtensions.AddPropertyJObject(val, "Heatmap", Heatmap.SaveToJsonObject(mode));
		}
		if (HeatmapWithGroup != null)
		{
			JsonExtensions.AddPropertyJObject(val, "HeatmapWithGroup", HeatmapWithGroup.SaveToJsonObject(mode));
		}
		JsonExtensions.AddPropertyColor(val, "IndividualColor", IndividualColor, "#70ad47");
		JsonExtensions.AddPropertyArrayColor(val, "Colors", Colors);
		JsonExtensions.AddPropertyColor(val, "DefaultColor", DefaultColor, "#4472c4");
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.White);
		JsonExtensions.AddPropertyDouble(val, "BorderSize", BorderSize, 0.7);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "LabelShadowForeground", LabelShadowForeground, Color.FromArgb(255, 251, 251, 251));
		JsonExtensions.AddPropertyColor(val, "LabelForeground", LabelForeground, Color.FromArgb(255, 37, 37, 37));
		JsonExtensions.AddPropertyColor(val, "BubbleBackColor", BubbleBackColor, Color.Red);
		JsonExtensions.AddPropertyColor(val, "BubbleBorderColor", BubbleBorderColor, Color.White);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Heatmap":
				Heatmap.LoadFromJsonObject((JObject)item.Value);
				break;
			case "HeatmapWithGroup":
				HeatmapWithGroup.LoadFromJsonObject((JObject)item.Value);
				break;
			case "IndividualColor":
				IndividualColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Colors":
				Colors = StiJsonReportObjectHelper.DeserializeArrayColor(item);
				break;
			case "DefaultColor":
				DefaultColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderSize":
				BorderSize = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LabelShadowForeground":
				LabelShadowForeground = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LabelForeground":
				LabelForeground = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BubbleBackColor":
				BubbleBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BubbleBorderColor":
				BubbleBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
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
			propertiesHelper.Colors(),
			propertiesHelper.DefaultColor(),
			propertiesHelper.Heatmap(),
			propertiesHelper.HeatmapWithGroup()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.BorderColor(),
			propertiesHelper.BorderSize()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Map, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiMapStyle stiMapStyle = base.Clone() as StiMapStyle;
		stiMapStyle.Colors = Colors?.ToArray();
		stiMapStyle.Heatmap = Heatmap.Clone() as StiHeatmapStyleData;
		stiMapStyle.HeatmapWithGroup = HeatmapWithGroup.Clone() as StiHeatmapWithGroupStyleData;
		return stiMapStyle;
	}

	private bool ShouldSerializeHeatmap()
	{
		StiHeatmapStyleData heatmap = Heatmap;
		if (heatmap == null)
		{
			return true;
		}
		return !heatmap.IsDefault();
	}

	private bool ShouldSerializeHeatmapWithGroup()
	{
		StiHeatmapWithGroupStyleData heatmapWithGroup = HeatmapWithGroup;
		if (heatmapWithGroup == null)
		{
			return true;
		}
		return !heatmapWithGroup.IsDefault();
	}

	private bool ShouldSerializeIndividualColor()
	{
		return IndividualColor != ColorTranslator.FromHtml("#70ad47");
	}

	private bool ShouldSerializeColors()
	{
		if (Colors != null && Colors.Length == DefaultColors.Length)
		{
			return !Colors.ToList().SequenceEqual(DefaultColors);
		}
		return true;
	}

	private bool ShouldSerializeDefaultColor()
	{
		return DefaultColor != ColorTranslator.FromHtml("#4472c4");
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	private bool ShouldSerializeBorderSize()
	{
		return BorderSize != 0.7;
	}

	private bool ShouldSerializeBorderColor()
	{
		return BackColor != Color.White;
	}

	private bool ShouldSerializeLabelShadowForeground()
	{
		return LabelShadowForeground != Color.FromArgb(255, 37, 37, 37);
	}

	private bool ShouldSerializeLabelForeground()
	{
		return LabelForeground != Color.FromArgb(180, 251, 251, 251);
	}

	private bool ShouldSerializeBubbleBackColor()
	{
		return BubbleBackColor != Color.Red;
	}

	private bool ShouldSerializeBubbleBorderColor()
	{
		return BubbleBorderColor != Color.White;
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		Image val = StiReportImages.Styles.Map((StiImageSize)2);
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
			throw new Exception("StiCrossTabStyle support only StiStyleElements.All.");
		}
		if (component is StiMap stiMap && stiMap.GetComponentStyle() is StiMapStyle stiMapStyle)
		{
			Colors = ((stiMapStyle.Colors == null) ? null : (stiMapStyle.Colors.Clone() as Color[]));
			Heatmap = ((stiMapStyle.Heatmap == null) ? null : (stiMapStyle.Heatmap.Clone() as StiHeatmapStyleData));
			HeatmapWithGroup = ((stiMapStyle.HeatmapWithGroup == null) ? null : (stiMapStyle.Heatmap.Clone() as StiHeatmapWithGroupStyleData));
			IndividualColor = stiMapStyle.IndividualColor;
			DefaultColor = stiMapStyle.DefaultColor;
			BackColor = stiMapStyle.BackColor;
			BorderSize = stiMapStyle.BorderSize;
			BorderColor = stiMapStyle.BorderColor;
			LabelShadowForeground = stiMapStyle.LabelShadowForeground;
			LabelForeground = stiMapStyle.LabelForeground;
			BubbleBackColor = stiMapStyle.BubbleBackColor;
			BubbleBorderColor = stiMapStyle.BubbleBorderColor;
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
		val.Serialize((object)this, stream, "StiMapStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiMapStyle");
	}

	internal StiMapStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
		Colors = DefaultColors.Clone() as Color[];
	}

	public StiMapStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiMapStyle(string name)
		: this(name, "")
	{
	}

	public StiMapStyle()
		: this("")
	{
	}
}
