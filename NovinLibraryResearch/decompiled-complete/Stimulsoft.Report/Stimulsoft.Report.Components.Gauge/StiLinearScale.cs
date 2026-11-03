using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiLinearScale : StiScaleBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearScale;

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets linearScale orientation.")]
	[StiOrder(220)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Orientation Orientation { get; set; } = (Orientation)1;


	[StiSerializable]
	[DefaultValue(0.9f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(230)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the height factor relative to the height of the basic component.")]
	public float RelativeHeight { get; set; } = 0.9f;


	public override StiGaugeElemenType ScaleType => StiGaugeElemenType.LinearElement;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyEnum(val, "Orientation", (Enum)(object)Orientation);
		JsonExtensions.AddPropertyFloat(val, "RelativeHeight", RelativeHeight, 0.9f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Orientation"))
			{
				if (name == "RelativeHeight")
				{
					RelativeHeight = StiJsonReportObjectHelper.DeserializeFloat(item);
				}
			}
			else
			{
				Orientation = StiJsonReportObjectHelper.DeserializeEnum<Orientation>(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[9]
		{
			propertiesHelper.Minimum(),
			propertiesHelper.Maximum(),
			propertiesHelper.MajorInterval(),
			propertiesHelper.MinorInterval(),
			propertiesHelper.StartWidth(),
			propertiesHelper.EndWidth(),
			propertiesHelper.IsReversed(),
			propertiesHelper.Orientation(),
			propertiesHelper.RelativeHeight()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Scale, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.LeftF(),
			propertiesHelper.TopF()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Position, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override void ApplyStyle(IStiGaugeStyle style)
	{
		if (AllowApplyStyle)
		{
			base.Brush = style.Core.LinearScaleBrush;
		}
		foreach (StiGaugeElement item in base.Items)
		{
			item.ApplyStyle(style);
		}
	}

	protected override void InteractiveClick(MouseEventArgs e)
	{
	}

	public override StiScaleBase CreateNew()
	{
		return new StiLinearScale();
	}

	public StiLinearScale()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		barGeometry = new StiLinearBarGeometry(this);
	}
}
