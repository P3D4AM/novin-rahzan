using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartLegend : IStiExpressionParentComponent, IStiJsonReportObject, ICloneable
{
	public enum Order
	{
		Columns = 1,
		Direction,
		HorAlignment,
		Labels,
		Title,
		VertAlignment,
		Visible
	}

	private StiChartLegendTitle title = new StiChartLegendTitle();

	[StiSerializable]
	[DefaultValue(StiLegendHorAlignment.Left)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets horizontal alignment of legend placement.")]
	[StiOrder(3)]
	public virtual StiLegendHorAlignment HorAlignment { get; set; } = StiLegendHorAlignment.Left;


	[StiSerializable]
	[DefaultValue(StiLegendVertAlignment.TopOutside)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets vertical alignment of legend placement.")]
	[StiOrder(6)]
	public virtual StiLegendVertAlignment VertAlignment { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(4)]
	public StiChartLegendLabels Labels { get; set; } = new StiChartLegendLabels();


	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(5)]
	public StiChartLegendTitle Title
	{
		get
		{
			return title;
		}
		set
		{
			title = value;
			if (title != null)
			{
				title.ParentComponent = ParentComponent;
			}
		}
	}

	[DefaultValue(true)]
	[TypeConverter(typeof(StiChartLegendVisibleConverter))]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiChartLegendVisibleEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets visibility of the legend.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(7)]
	public virtual bool Visible
	{
		get
		{
			return Visibility == StiLegendVisibility.Auto;
		}
		set
		{
			Visibility = (value ? StiLegendVisibility.Auto : StiLegendVisibility.False);
		}
	}

	[StiSerializable]
	[DefaultValue(StiLegendVisibility.Auto)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public virtual StiLegendVisibility Visibility { get; set; } = StiLegendVisibility.Auto;


	[StiSerializable]
	[DefaultValue(StiLegendDirection.TopToBottom)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets direction which used for series drawing in legend.")]
	[StiOrder(2)]
	public virtual StiLegendDirection Direction { get; set; } = StiLegendDirection.TopToBottom;


	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets amount of columns.")]
	[Editor("Stimulsoft.Report.Design.Components.StiLegendColumnsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(1)]
	public int Columns { get; set; }

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Title.IsDefault && Labels.IsDefault && HorAlignment == StiLegendHorAlignment.Left && VertAlignment == StiLegendVertAlignment.TopOutside && Visibility == StiLegendVisibility.Auto && Direction == StiLegendDirection.TopToBottom)
			{
				return Columns == 0;
			}
			return false;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public StiComponent ParentComponent { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyJObject("Title", Title.SaveToJsonObject(mode));
		jObject.AddPropertyJObject("Labels", Labels.SaveToJsonObject(mode));
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiLegendHorAlignment.Left);
		jObject.AddPropertyEnum("VertAlignment", VertAlignment, StiLegendVertAlignment.TopOutside);
		jObject.AddPropertyEnum("Visibility", Visibility, StiLegendVisibility.Auto);
		jObject.AddPropertyEnum("Direction", Direction, StiLegendDirection.TopToBottom);
		jObject.AddPropertyInt("Columns", Columns);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Title":
				Title.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Labels":
				Labels.LoadFromJsonObject((JObject)item.Value);
				break;
			case "HorAlignment":
				HorAlignment = item.DeserializeEnum<StiLegendHorAlignment>();
				break;
			case "VertAlignment":
				VertAlignment = item.DeserializeEnum<StiLegendVertAlignment>();
				break;
			case "Visible":
				Visible = item.DeserializeBool();
				break;
			case "Visibility":
				Visibility = item.DeserializeEnum<StiLegendVisibility>();
				break;
			case "Direction":
				Direction = item.DeserializeEnum<StiLegendDirection>();
				break;
			case "Columns":
				Columns = item.DeserializeInt();
				break;
			}
		}
	}

	public object Clone()
	{
		StiChartLegend stiChartLegend = MemberwiseClone() as StiChartLegend;
		stiChartLegend.Title = Title.Clone() as StiChartLegendTitle;
		stiChartLegend.Labels = Labels.Clone() as StiChartLegendLabels;
		stiChartLegend.HorAlignment = HorAlignment;
		stiChartLegend.VertAlignment = VertAlignment;
		stiChartLegend.Visibility = Visibility;
		return stiChartLegend;
	}

	private bool ShouldSerializeLabels()
	{
		if (Labels != null)
		{
			return !Labels.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeTitle()
	{
		if (Title != null)
		{
			return !Title.IsDefault;
		}
		return true;
	}

	public StiChartLegend()
	{
	}

	public StiChartLegend(StiChartLegendTitle title, StiChartLegendLabels labels, StiLegendHorAlignment horAlignment, StiLegendVertAlignment vertAlignment)
	{
		Title = title;
		Labels = labels;
		HorAlignment = horAlignment;
		VertAlignment = vertAlignment;
	}

	[StiUniversalConstructor("Legend")]
	public StiChartLegend(StiChartLegendTitle title, StiChartLegendLabels labels, StiLegendHorAlignment horAlignment, StiLegendVertAlignment vertAlignment, StiLegendVisibility visibility, StiLegendDirection direction, int columns)
	{
		Title = title;
		Labels = labels;
		HorAlignment = horAlignment;
		VertAlignment = vertAlignment;
		Visibility = visibility;
		Direction = direction;
		Columns = columns;
	}
}
