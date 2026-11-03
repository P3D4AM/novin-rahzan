using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiBandInteraction : StiInteraction
{
	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && !CollapsingEnabled)
			{
				return !SelectionEnabled;
			}
			return false;
		}
	}

	[Browsable(false)]
	public override bool IsDefaultExt
	{
		get
		{
			if (IsDefaultSorting && IsDefaultDrillDown && string.IsNullOrEmpty(Bookmark?.Value) && string.IsNullOrEmpty(Hyperlink?.Value) && string.IsNullOrEmpty(Tag?.Value))
			{
				return string.IsNullOrEmpty(ToolTip?.Value);
			}
			return false;
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether it is allowed or not data collapsing in the report viewer.")]
	[StiOrder(110)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual bool CollapsingEnabled { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether it is allowed to select one data row which is output by this DataBand.")]
	[StiOrder(175)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual bool SelectionEnabled { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether it is necessary GroupFooter collapsing.")]
	[StiOrder(130)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual bool CollapseGroupFooter { get; set; }

	[Description("Gets or sets the boolean expression which indicates whether a group, when rendering, should be collapsed or not.")]
	[StiOrder(120)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual StiCollapsedExpression Collapsed
	{
		get
		{
			if (!(base.ParentComponent is StiGroupHeaderBand))
			{
				if (!(base.ParentComponent is StiDataBand))
				{
					return new StiCollapsedExpression();
				}
				return ((StiDataBand)base.ParentComponent).Collapsed;
			}
			return ((StiGroupHeaderBand)base.ParentComponent).Collapsed;
		}
		set
		{
			if (base.ParentComponent is StiGroupHeaderBand)
			{
				((StiGroupHeaderBand)base.ParentComponent).Collapsed = value;
			}
			else
			{
				((StiDataBand)base.ParentComponent).Collapsed = value;
			}
		}
	}

	[StiBrowsable(false)]
	[Browsable(false)]
	[StiNonSerialized]
	[DefaultValue(null)]
	public int? SelectedLine { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CollapsingEnabled", CollapsingEnabled, false, false);
		JsonExtensions.AddPropertyBool(val, "SelectionEnabled", SelectionEnabled, false, false);
		JsonExtensions.AddPropertyBool(val, "CollapseGroupFooter", CollapseGroupFooter, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CollapsingEnabled":
				CollapsingEnabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SelectionEnabled":
				SelectionEnabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CollapseGroupFooter":
				CollapseGroupFooter = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}
}
