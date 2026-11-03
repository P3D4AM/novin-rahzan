using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public abstract class StiYAxis : StiAxis, IStiYAxis, IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault, IStiPropertyGridObject
{
	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiYAxis;

	[Browsable(false)]
	public string PropName => string.Empty;

	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault)
			{
				return !ShouldSerializeShowYAxis();
			}
			return false;
		}
	}

	[StiSerializable]
	[DefaultValue(StiShowYAxis.Both)]
	[Description("Gets or sets type of drawing Y axis.")]
	[TypeConverter(typeof(StiEnumConverter))]
	public virtual StiShowYAxis ShowYAxis { get; set; } = StiShowYAxis.Both;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "ShowYAxis", (Enum)ShowYAxis, (Enum)StiShowYAxis.Both);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "ShowYAxis")
			{
				ShowYAxis = StiJsonReportObjectHelper.DeserializeEnum<StiShowYAxis>(item);
			}
		}
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.StiYAxis() });
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	protected virtual bool ShouldSerializeShowYAxis()
	{
		return ShowYAxis == StiShowYAxis.Both;
	}

	public StiYAxis()
	{
	}

	public StiYAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowYAxis showYAxis, bool allowApplyStyle)
		: base(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, allowApplyStyle)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		ShowYAxis = showYAxis;
	}

	public StiYAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowYAxis showYAxis, bool allowApplyStyle)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, allowApplyStyle)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShowYAxis = showYAxis;
	}

	public StiYAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowYAxis showYAxis, bool allowApplyStyle, bool logarithmicScale)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, allowApplyStyle, logarithmicScale)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShowYAxis = showYAxis;
	}
}
