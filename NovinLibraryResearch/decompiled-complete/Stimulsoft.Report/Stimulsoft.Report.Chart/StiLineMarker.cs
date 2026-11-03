using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiLineMarkerConverter))]
public class StiLineMarker : StiMarker, IStiLineMarker, IStiMarker, ICloneable, IStiFontIconsSeries, IStiJsonReportObject, IStiDefault
{
	public override StiComponentId ComponentId => StiComponentId.StiLineMarker;

	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault)
			{
				return Step == 15.0;
			}
			return false;
		}
	}

	[DefaultValue(15.0)]
	[StiSerializable]
	[Description("Gets or sets step of the line marker.")]
	public virtual double Step { get; set; } = 15.0;


	[DefaultValue(5f)]
	[StiSerializable]
	[Description("Gets or sets size of marker.")]
	public override float Size
	{
		get
		{
			return base.Size;
		}
		set
		{
			base.Size = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets visibility of marker.")]
	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyDouble(val, "Step", Step, 15.0);
		JsonExtensions.AddPropertyFloat(val, "Size", Size, 5f);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, false, false);
		JsonExtensions.AddPropertyColor(val, "BorderColor", base.BorderColor, Color.Transparent);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Step")
			{
				Step = StiJsonReportObjectHelper.DeserializeDouble(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.LineMarker() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	protected override bool ShouldSerializeSize()
	{
		return Size != 5f;
	}

	protected override bool ShouldSerializeVisible()
	{
		return Visible;
	}

	public StiLineMarker()
	{
		Visible = false;
		base.BorderColor = Color.Transparent;
		Size = 5f;
	}
}
