using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiRangeBase : ICloneable, IStiPropertyGridObject
{
	internal StiScaleRangeList rangeList;

	public abstract StiComponentId ComponentId { get; }

	[Browsable(false)]
	public string PropName => null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush to fill a component.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border of the component.")]
	public StiBrush BorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[DefaultValue(1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border thickness of the component.")]
	public float BorderWidth { get; set; } = 1f;


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start value of the range.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float StartValue { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets end value of the range.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float EndValue { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start width of the range bar.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float StartWidth { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets end width of the range bar.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float EndWidth { get; set; }

	[DefaultValue(StiPlacement.Overlay)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the placement of the component.")]
	public StiPlacement Placement { get; set; } = StiPlacement.Overlay;


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the offset ratio of an item.")]
	public float Offset { get; set; }

	[Browsable(false)]
	public StiScaleRangeList RangeList => rangeList;

	[Browsable(false)]
	public abstract string LocalizeName { get; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "BorderBrush", BorderBrush);
		JsonExtensions.AddPropertyFloat(val, "BorderWidth", BorderWidth, 1f);
		JsonExtensions.AddPropertyFloat(val, "StartValue", StartValue, 0f);
		JsonExtensions.AddPropertyFloat(val, "EndValue", EndValue, 0f);
		JsonExtensions.AddPropertyFloat(val, "StartWidth", StartWidth, 0f);
		JsonExtensions.AddPropertyFloat(val, "EndWidth", EndWidth, 0f);
		JsonExtensions.AddPropertyEnum(val, "Placement", (Enum)Placement);
		JsonExtensions.AddPropertyFloat(val, "Offset", Offset, 0f);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderBrush":
				BorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderWidth":
				BorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "StartValue":
				StartValue = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "EndValue":
				EndValue = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "StartWidth":
				StartWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "EndWidth":
				EndWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Placement":
				Placement = StiJsonReportObjectHelper.DeserializeEnum<StiPlacement>(item);
				break;
			case "Offset":
				Offset = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public abstract StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		StiRangeBase stiRangeBase = (StiRangeBase)MemberwiseClone();
		stiRangeBase.Brush = (StiBrush)Brush.Clone();
		stiRangeBase.BorderBrush = (StiBrush)BorderBrush.Clone();
		return stiRangeBase;
	}

	protected internal abstract void DrawRange(StiGaugeContextPainter context, StiScaleBase scale);

	public virtual StiRangeBase CreateNew()
	{
		throw new NotImplementedException();
	}
}
