using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Table.Design;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Table;

[TypeConverter(typeof(StiTableColumnConverter))]
public abstract class StiTableColumn : StiMeter, IStiHorAlignment, IStiTextFormat, IStiDataFormat, IStiTableColumn, IStiForeColor, IStiElementInteraction
{
	[DefaultValue(StiHorAlignment.Left)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public virtual StiHorAlignment HorAlignment { get; set; } = StiHorAlignment.Left;


	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class, StiSerializeTypes.SerializeToAllExceptCode)]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The foreground color of this component, which is used to display text.")]
	public Color ForeColor { get; set; } = Color.Transparent;


	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
	[Browsable(false)]
	public virtual IStiDashboardInteraction DashboardInteraction { get; set; } = new StiTableColumnDashboardInteraction();


	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && Visibility == StiTableColumnVisibility.True && VisibilityExpression == null && !ShowTotalSummary && SummaryType == StiSummaryColumnType.Sum && HorAlignment == StiHorAlignment.Left && SummaryAlignment == StiHorAlignment.Right && !ShouldSerializeTextFormat() && !ShouldSerializeForeColor())
			{
				return !ShouldSerializeDashboardInteraction();
			}
			return false;
		}
	}

	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Obsolete("Please use 'Visibility' property instead 'Visible' property!")]
	[Browsable(false)]
	public virtual bool Visible
	{
		get
		{
			return Visibility == StiTableColumnVisibility.True;
		}
		set
		{
			Visibility = ((!value) ? StiTableColumnVisibility.False : StiTableColumnVisibility.True);
		}
	}

	[DefaultValue(StiTableColumnVisibility.True)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public virtual StiTableColumnVisibility Visibility { get; set; }

	[DefaultValue(null)]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public virtual string VisibilityExpression { get; set; }

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public virtual bool ShowTotalSummary { get; set; }

	[DefaultValue(StiSummaryColumnType.Sum)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public virtual StiSummaryColumnType SummaryType { get; set; }

	[DefaultValue(StiHorAlignment.Right)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public virtual StiHorAlignment SummaryAlignment { get; set; } = StiHorAlignment.Right;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Left);
		jObject.AddPropertyEnum("SummaryAlignment", SummaryAlignment, StiHorAlignment.Right);
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
		}
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyBool("ShowTotalSummary", ShowTotalSummary);
		jObject.AddPropertyEnum("SummaryType", SummaryType, StiSummaryColumnType.Sum);
		jObject.AddPropertyEnum("Visibility", Visibility, StiTableColumnVisibility.True);
		if (VisibilityExpression != null)
		{
			jObject.AddPropertyStringNullOrEmpty("VisibilityExpression", VisibilityExpression);
		}
		if (DashboardInteraction is StiTableColumnDashboardInteraction stiTableColumnDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiTableColumnDashboardInteraction.SaveToJsonObject(mode));
		}
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "DashboardInteraction":
				if (StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) is StiTableColumnDashboardInteraction dashboardInteraction)
				{
					DashboardInteraction = dashboardInteraction;
				}
				break;
			case "HorAlignment":
				HorAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "SummaryAlignment":
				SummaryAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "SummaryType":
				SummaryType = item.DeserializeEnum<StiSummaryColumnType>();
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Visible":
				Visibility = ((!item.DeserializeBool()) ? StiTableColumnVisibility.False : StiTableColumnVisibility.True);
				break;
			case "ShowTotalSummary":
				ShowTotalSummary = item.DeserializeBool();
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Visibility":
				Visibility = item.DeserializeEnum<StiTableColumnVisibility>();
				break;
			case "VisibilityExpression":
				VisibilityExpression = item.DeserializeString();
				break;
			}
		}
	}

	public override object Clone()
	{
		StiTableColumn stiTableColumn = ((IStiTableColumn)base.Clone()) as StiTableColumn;
		stiTableColumn.TextFormat = TextFormat.Clone() as StiFormatService;
		stiTableColumn.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		return stiTableColumn;
	}

	private bool ShouldSerializeTextFormat()
	{
		return !(TextFormat is StiGeneralFormatService);
	}

	public StiDataFormatKind GetDataFormat()
	{
		if (TextFormat is StiBooleanFormatService)
		{
			return StiDataFormatKind.Boolean;
		}
		if (TextFormat is StiPercentageFormatService)
		{
			return StiDataFormatKind.Percentage;
		}
		if (TextFormat is StiCurrencyFormatService)
		{
			return StiDataFormatKind.Currency;
		}
		if (TextFormat is StiNumberFormatService)
		{
			return StiDataFormatKind.Number;
		}
		if (TextFormat is StiDateFormatService)
		{
			return StiDataFormatKind.Date;
		}
		if (TextFormat is StiTimeFormatService)
		{
			return StiDataFormatKind.Time;
		}
		return StiDataFormatKind.General;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	private bool ShouldSerializeDashboardInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		uniqueCode = (uniqueCode * 397) ^ (int)HorAlignment;
		uniqueCode = (uniqueCode * 397) ^ (int)SummaryAlignment;
		uniqueCode = (uniqueCode * 397) ^ (int)SummaryType;
		uniqueCode = (uniqueCode * 397) ^ ((TextFormat != null) ? TextFormat.GetHashCode() : 0);
		uniqueCode = (uniqueCode * 397) ^ Visibility.GetHashCode();
		if (VisibilityExpression != null)
		{
			uniqueCode = (uniqueCode * 397) ^ VisibilityExpression.GetHashCode();
		}
		return uniqueCode;
	}

	public StiTableColumn()
	{
	}

	public StiTableColumn(string key, string expression, string label)
		: base(key, expression, label)
	{
	}

	public StiTableColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Visibility = StiTableColumnVisibility.True;
	}

	public StiTableColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression)
		: base(expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Visibility = visibility;
		VisibilityExpression = visibilityExpression;
	}

	public StiTableColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Visibility = visibility;
		VisibilityExpression = visibilityExpression;
		ShowTotalSummary = showTotalSummary;
		SummaryType = summaryType;
		SummaryAlignment = summaryAlignment;
	}

	public StiTableColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Visibility = visibility;
		VisibilityExpression = visibilityExpression;
		ForeColor = foreColor;
		ShowTotalSummary = showTotalSummary;
		SummaryType = summaryType;
		SummaryAlignment = summaryAlignment;
	}

	public StiTableColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Visibility = visibility;
		VisibilityExpression = visibilityExpression;
		ForeColor = foreColor;
		ShowTotalSummary = showTotalSummary;
		SummaryType = summaryType;
		SummaryAlignment = summaryAlignment;
	}

	public StiTableColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment, StiTableColumnDashboardInteraction interaction)
		: base(key, expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Visibility = visibility;
		VisibilityExpression = visibilityExpression;
		ShowTotalSummary = showTotalSummary;
		SummaryType = summaryType;
		SummaryAlignment = summaryAlignment;
		ForeColor = foreColor;
		DashboardInteraction = interaction;
	}
}
