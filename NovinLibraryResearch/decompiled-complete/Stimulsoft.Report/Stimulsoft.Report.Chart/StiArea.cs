using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[StiServiceBitmap(typeof(StiArea), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiServiceCategoryBitmap(typeof(StiArea), "Stimulsoft.Report.Images.Components.StiChart.png")]
[TypeConverter(typeof(StiUniversalConverter))]
public abstract class StiArea : StiService, IStiArea, ICloneable, IStiJsonReportObject, IStiSerializeToCodeAsClass, IStiPropertyGridObject
{
	private bool allowApplyStyle = true;

	[Browsable(false)]
	public abstract StiComponentId ComponentId { get; }

	[Browsable(false)]
	public string PropName => string.Empty;

	public override string ServiceName => Core.LocalizedName;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Chart";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiArea);

	[Browsable(false)]
	public bool IsDefaultSeriesTypeFullStackedColumnSeries => GetDefaultSeriesType() == typeof(StiFullStackedColumnSeries);

	[Browsable(false)]
	public bool IsDefaultSeriesTypeFullStackedBarSeries => GetDefaultSeriesType() == typeof(StiFullStackedBarSeries);

	[Browsable(false)]
	public StiAreaCoreXF Core { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public virtual IStiChart Chart { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle
	{
		get
		{
			return allowApplyStyle;
		}
		set
		{
			if (allowApplyStyle != value)
			{
				allowApplyStyle = value;
				if (value && Chart != null)
				{
					Core.ApplyStyle(Chart.Style);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that each series is drawn by its own colour.")]
	public virtual bool ColorEach { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public virtual bool ColorEachAllowed => true;

	[DefaultValue(false)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates necessary draw shadod or no.")]
	public bool ShowShadow { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets border color of this area.")]
	public Color BorderColor { get; set; } = Color.Gray;


	[StiSerializable]
	[Description("Gets or sets a brush to fill a area.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", allowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "ColorEach", ColorEach, false, false);
		JsonExtensions.AddPropertyBool(val, "ShowShadow", ShowShadow, false, false);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Gray);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				allowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ColorEach":
				ColorEach = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowShadow":
				ShowShadow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			}
		}
	}

	internal static IStiArea CreateFromJsonObject(JObject jObject)
	{
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiArea stiArea = StiOptions.Services.ChartAreas.FirstOrDefault((StiArea x) => ((object)x).GetType().Name == ident);
		if (stiArea == null)
		{
			throw new Exception("Type " + ident + " is not found!");
		}
		StiArea stiArea2 = stiArea.CreateNew();
		stiArea2.LoadFromJsonObject(jObject);
		return stiArea2;
	}

	public abstract StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		IStiArea stiArea = ((StiService)this).Clone() as IStiArea;
		object obj = Brush.Clone();
		stiArea.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		if (Core != null)
		{
			stiArea.Core = Core.Clone() as StiAreaCoreXF;
			stiArea.Core.Area = stiArea;
		}
		return stiArea;
	}

	public abstract StiArea CreateNew();

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}

	public abstract Type GetDefaultSeriesType();

	public abstract Type[] GetSeriesTypes();

	public abstract Type GetDefaultSeriesLabelsType();

	public abstract Type[] GetSeriesLabelsTypes();
}
