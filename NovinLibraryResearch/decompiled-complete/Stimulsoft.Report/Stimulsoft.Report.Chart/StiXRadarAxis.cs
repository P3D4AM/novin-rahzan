using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiXRadarAxis : StiRadarAxis, IStiXRadarAxis, IStiRadarAxis, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiXRadarAxis;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public StiXRadarAxisCoreXF XCore => base.Core as StiXRadarAxisCoreXF;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public IStiRadarAxisLabels Labels { get; set; } = new StiRadarAxisLabels();


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "Labels", ((IStiJsonReportObject)Labels).SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Labels")
			{
				((IStiJsonReportObject)Labels).LoadFromJsonObject((JObject)item.Value);
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.XRadarAxis() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		IStiXRadarAxis stiXRadarAxis = base.Clone() as IStiXRadarAxis;
		stiXRadarAxis.Labels = Labels.Clone() as IStiRadarAxisLabels;
		return stiXRadarAxis;
	}

	public StiXRadarAxis()
	{
		base.Core = new StiXRadarAxisCoreXF(this);
	}

	[StiUniversalConstructor("Axis")]
	public StiXRadarAxis(IStiRadarAxisLabels labels, bool visible, bool allowApplyStyle)
		: base(visible, allowApplyStyle)
	{
		base.Core = new StiXRadarAxisCoreXF(this);
		Labels = labels;
	}
}
