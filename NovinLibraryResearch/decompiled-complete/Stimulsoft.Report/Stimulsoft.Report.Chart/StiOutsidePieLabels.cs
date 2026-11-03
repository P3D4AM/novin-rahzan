using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiOutsidePieLabels : StiCenterPieLabels, IStiOutsidePieLabels, IStiCenterPieLabels, IStiPieSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiOutsidePieLabels;

	[StiSerializable]
	[StiOrder(260)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that values from series will be shown in series labels.")]
	public virtual bool ShowValue { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public override float Angle
	{
		get
		{
			return base.Angle;
		}
		set
		{
			base.Angle = value;
		}
	}

	[StiSerializable]
	[StiOrder(205)]
	[DefaultValue(20f)]
	[Description("Gets or sets line length between border of series labels and border of pie series.")]
	public virtual float LineLength { get; set; } = 20f;


	[StiSerializable]
	[StiOrder(200)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color of line.")]
	public virtual Color LineColor { get; set; } = Color.Black;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "Angle");
		JsonExtensions.AddPropertyBool(val, "ShowValue", ShowValue, false, false);
		JsonExtensions.AddPropertyFloat(val, "LineLength", LineLength, 20f);
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Black);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowValue":
				ShowValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LineLength":
				LineLength = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesOutsidePieLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiOutsidePieLabels();
	}

	public StiOutsidePieLabels()
	{
		base.Core = new StiOutsidePieLabelsCoreXF(this);
	}
}
