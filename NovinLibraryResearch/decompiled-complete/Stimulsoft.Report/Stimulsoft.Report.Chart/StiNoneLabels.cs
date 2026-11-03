using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiNoneLabels : StiSeriesLabels, IStiNoneLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiNoneLabels;

	[StiNonSerialized]
	[Browsable(false)]
	public override bool Antialiasing
	{
		get
		{
			return base.Antialiasing;
		}
		set
		{
			base.Antialiasing = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiNonSerialized]
	[Browsable(false)]
	public override Color LabelColor
	{
		get
		{
			return base.LabelColor;
		}
		set
		{
			base.LabelColor = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override Color BorderColor
	{
		get
		{
			return base.BorderColor;
		}
		set
		{
			base.BorderColor = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiBrush Brush
	{
		get
		{
			return base.Brush;
		}
		set
		{
			base.Brush = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override Font Font
	{
		get
		{
			return base.Font;
		}
		set
		{
			base.Font = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "Antialiasing");
		JsonExtensions.RemoveProperty(val, "Visible");
		JsonExtensions.RemoveProperty(val, "LabelColor");
		JsonExtensions.RemoveProperty(val, "BorderColor");
		JsonExtensions.RemoveProperty(val, "Brush");
		JsonExtensions.RemoveProperty(val, "Font");
		return val;
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesNoneLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiNoneLabels();
	}

	public StiNoneLabels()
	{
		base.Core = new StiNoneLabelsCoreXF(this);
	}
}
