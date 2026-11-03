using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiSeriesInteractionConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiSeriesInteraction : IStiInteractionClass, IStiReportProperty, IStiDefault, IStiSeriesInteraction, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiSeriesInteraction;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (string.IsNullOrEmpty(HyperlinkDataColumn) && string.IsNullOrEmpty(TagDataColumn) && string.IsNullOrEmpty(ToolTipDataColumn) && string.IsNullOrEmpty(Hyperlink.Value) && string.IsNullOrEmpty(Tag.Value) && string.IsNullOrEmpty(ToolTip.Value) && string.IsNullOrEmpty(ListOfHyperlinks.Value) && string.IsNullOrEmpty(ListOfTags.Value))
			{
				return string.IsNullOrEmpty(ListOfToolTips.Value);
			}
			return false;
		}
	}

	[StiOrder(170)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an expression to fill a series hyperlink.")]
	public virtual StiHyperlinkExpression Hyperlink
	{
		get
		{
			return ParentSeries.Hyperlink;
		}
		set
		{
			ParentSeries.Hyperlink = value;
		}
	}

	[Description("Gets or sets the expression to fill a series tag.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	public virtual StiTagExpression Tag
	{
		get
		{
			return ParentSeries.Tag;
		}
		set
		{
			ParentSeries.Tag = value;
		}
	}

	[StiOrder(190)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a series tooltip.")]
	public virtual StiToolTipExpression ToolTip
	{
		get
		{
			return ParentSeries.ToolTip;
		}
		set
		{
			ParentSeries.ToolTip = value;
		}
	}

	[StiOrder(130)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the hyperlink value.")]
	public string HyperlinkDataColumn
	{
		get
		{
			return ParentSeries.HyperlinkDataColumn;
		}
		set
		{
			ParentSeries.HyperlinkDataColumn = value;
		}
	}

	[StiOrder(140)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the tag value.")]
	public string TagDataColumn
	{
		get
		{
			return ParentSeries.TagDataColumn;
		}
		set
		{
			ParentSeries.TagDataColumn = value;
		}
	}

	[StiOrder(150)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the tool tip value.")]
	public string ToolTipDataColumn
	{
		get
		{
			return ParentSeries.ToolTipDataColumn;
		}
		set
		{
			ParentSeries.ToolTipDataColumn = value;
		}
	}

	[StiOrder(210)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of hyperlinks. Example: 1;2;3")]
	public virtual StiListOfHyperlinksExpression ListOfHyperlinks
	{
		get
		{
			return ParentSeries.ListOfHyperlinks;
		}
		set
		{
			ParentSeries.ListOfHyperlinks = value;
		}
	}

	[StiOrder(220)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of tags. Example: 1;2;3")]
	public virtual StiListOfTagsExpression ListOfTags
	{
		get
		{
			return ParentSeries.ListOfTags;
		}
		set
		{
			ParentSeries.ListOfTags = value;
		}
	}

	[StiOrder(230)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of tool tips. Example: 1;2;3")]
	public virtual StiListOfToolTipsExpression ListOfToolTips
	{
		get
		{
			return ParentSeries.ListOfToolTips;
		}
		set
		{
			ParentSeries.ListOfToolTips = value;
		}
	}

	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value which indicates that the Drill-Down operation can be executed for Series.")]
	[StiOrder(90)]
	public virtual bool AllowSeries
	{
		get
		{
			return ParentSeries.AllowSeries;
		}
		set
		{
			ParentSeries.AllowSeries = value;
		}
	}

	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value which indicates that the Drill-Down operation can be executed for Series Elements.")]
	[StiOrder(95)]
	public virtual bool AllowSeriesElements
	{
		get
		{
			return ParentSeries.AllowSeriesElements;
		}
		set
		{
			ParentSeries.AllowSeriesElements = value;
		}
	}

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value which indicates whether the Drill-Down operation can be executed.")]
	[StiOrder(100)]
	public virtual bool DrillDownEnabled
	{
		get
		{
			return ParentSeries.DrillDownEnabled;
		}
		set
		{
			ParentSeries.DrillDownEnabled = value;
		}
	}

	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a path to a report for the Drill-Down operation.")]
	[StiOrder(120)]
	public virtual string DrillDownReport
	{
		get
		{
			return ParentSeries.DrillDownReport;
		}
		set
		{
			ParentSeries.DrillDownReport = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiInteractionDrillDownPageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a page for the Drill-Down operation.")]
	[StiOrder(110)]
	public virtual StiPage DrillDownPage
	{
		get
		{
			return ParentSeries.DrillDownPage;
		}
		set
		{
			ParentSeries.DrillDownPage = value;
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	public string DrillDownPageGuid
	{
		get
		{
			return ParentSeries.DrillDownPageGuid;
		}
		set
		{
			ParentSeries.DrillDownPageGuid = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public StiComponent ParentComponent => ParentSeries?.Chart as StiChart;

	[Browsable(false)]
	public StiSeries ParentSeries { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DrillDownPageGuid", DrillDownPageGuid);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "DrillDownPageGuid")
			{
				DrillDownPageGuid = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.SeriesInteraction() });
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object GetReport()
	{
		return (ParentSeries?.Chart as StiChart)?.Report;
	}

	public object Clone()
	{
		return (StiSeriesInteraction)MemberwiseClone();
	}
}
