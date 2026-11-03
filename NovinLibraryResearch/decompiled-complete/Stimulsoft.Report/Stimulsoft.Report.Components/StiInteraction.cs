using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiInteractionConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiInteraction : ICloneable, IStiSerializeToCodeAsClass, IStiInteractionClass, IStiReportProperty, IStiDefault, IStiJsonReportObject
{
	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (SortingEnabled && string.IsNullOrEmpty(SortingColumn))
			{
				return IsDefaultDrillDown;
			}
			return false;
		}
	}

	[Browsable(false)]
	public virtual bool IsDefaultSorting
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(SortingColumn))
			{
				return !SortingEnabled;
			}
			return true;
		}
	}

	[Browsable(false)]
	public virtual bool IsDefaultDrillDown
	{
		get
		{
			if (!DrillDownEnabled && string.IsNullOrEmpty(DrillDownReport) && DrillDownPageGuid == null && DrillDownParameter1.IsDefault && DrillDownParameter2.IsDefault && DrillDownParameter3.IsDefault && DrillDownParameter4.IsDefault && DrillDownParameter5.IsDefault && DrillDownParameter6.IsDefault && DrillDownParameter7.IsDefault && DrillDownParameter8.IsDefault && DrillDownParameter9.IsDefault)
			{
				return DrillDownParameter10.IsDefault;
			}
			return false;
		}
	}

	[Browsable(false)]
	public virtual bool IsDefaultExt
	{
		get
		{
			if ((!SortingEnabled || string.IsNullOrEmpty(SortingColumn)) && IsDefaultDrillDown && string.IsNullOrEmpty(Bookmark?.Value) && string.IsNullOrEmpty(Hyperlink?.Value) && string.IsNullOrEmpty(Tag?.Value))
			{
				return string.IsNullOrEmpty(ToolTip?.Value);
			}
			return false;
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether it is allowed or not, using given component, data re-sorting in the report viewer.")]
	[StiOrder(180)]
	public virtual bool SortingEnabled { get; set; } = true;


	[StiSerializable]
	[DefaultValue("")]
	public virtual string SortingColumn { get; set; } = string.Empty;


	[Browsable(false)]
	public virtual int SortingIndex { get; set; }

	[Browsable(false)]
	public virtual StiInteractionSortDirection SortingDirection { get; set; } = StiInteractionSortDirection.None;


	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the Drill-Down operation can be executed.")]
	[StiOrder(140)]
	public virtual bool DrillDownEnabled { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	[Description("Gets or sets a path to a report for the Drill-Down operation.")]
	[StiOrder(155)]
	public virtual string DrillDownReport { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue(StiDrillDownMode.MultiPage)]
	[Description("Gets or sets a value which indicates opening of sub-reports in the one tab.")]
	[StiOrder(155)]
	[TypeConverter(typeof(StiEnumConverter))]
	public virtual StiDrillDownMode DrillDownMode { get; set; } = StiDrillDownMode.MultiPage;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets first Drill-Down parameter.")]
	[StiOrder(156)]
	public virtual StiDrillDownParameter DrillDownParameter1 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets second Drill-Down parameter.")]
	[StiOrder(157)]
	public virtual StiDrillDownParameter DrillDownParameter2 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets third Drill-Down parameter.")]
	[StiOrder(158)]
	public virtual StiDrillDownParameter DrillDownParameter3 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets fourth Drill-Down parameter.")]
	[StiOrder(159)]
	public virtual StiDrillDownParameter DrillDownParameter4 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets fifth Drill-Down parameter.")]
	[StiOrder(160)]
	public virtual StiDrillDownParameter DrillDownParameter5 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets sixth Drill-Down parameter.")]
	[StiOrder(161)]
	public virtual StiDrillDownParameter DrillDownParameter6 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets seventh Drill-Down parameter.")]
	[StiOrder(162)]
	public virtual StiDrillDownParameter DrillDownParameter7 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets eighth Drill-Down parameter.")]
	[StiOrder(163)]
	public virtual StiDrillDownParameter DrillDownParameter8 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets nineth Drill-Down parameter.")]
	[StiOrder(164)]
	public virtual StiDrillDownParameter DrillDownParameter9 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets tenth Drill-Down parameter.")]
	[StiOrder(165)]
	public virtual StiDrillDownParameter DrillDownParameter10 { get; set; }

	[StiNonSerialized]
	[Editor("Stimulsoft.Report.Components.Design.StiInteractionDrillDownPageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Description("Gets or sets a page for the Drill-Down operation.")]
	[StiOrder(150)]
	public virtual StiPage DrillDownPage
	{
		get
		{
			if (ParentComponent == null || ParentComponent.Report == null)
			{
				return null;
			}
			foreach (StiPage page in ParentComponent.Report.Pages)
			{
				if (page.Guid == DrillDownPageGuid)
				{
					return page;
				}
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				DrillDownPageGuid = null;
				return;
			}
			if (value.Guid == null)
			{
				value.Guid = StiGuidUtils.NewGuid();
			}
			DrillDownPageGuid = value.Guid;
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	public string DrillDownPageGuid { get; set; }

	[StiOrder(100)]
	[Description("Gets or sets the expression to fill a component bookmark.")]
	public virtual StiBookmarkExpression Bookmark
	{
		get
		{
			return ParentComponent.Bookmark;
		}
		set
		{
			ParentComponent.Bookmark = value;
		}
	}

	[StiOrder(170)]
	[Description("Gets or sets an expression to fill a component hyperlink.")]
	public virtual StiHyperlinkExpression Hyperlink
	{
		get
		{
			return ParentComponent.Hyperlink;
		}
		set
		{
			ParentComponent.Hyperlink = value;
		}
	}

	[Description("Gets or sets the expression to fill a component tag.")]
	[StiOrder(200)]
	public virtual StiTagExpression Tag
	{
		get
		{
			return ParentComponent.Tag;
		}
		set
		{
			ParentComponent.Tag = value;
		}
	}

	[StiOrder(210)]
	[Description("Gets or sets the expression to fill a component tooltip.")]
	public virtual StiToolTipExpression ToolTip
	{
		get
		{
			return ParentComponent.ToolTip;
		}
		set
		{
			ParentComponent.ToolTip = value;
		}
	}

	public StiComponent ParentComponent { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyBool(val, "SortingEnabled", SortingEnabled, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SortingColumn", SortingColumn);
		JsonExtensions.AddPropertyInt(val, "SortingIndex", SortingIndex, 0);
		JsonExtensions.AddPropertyEnum(val, "SortingDirection", (Enum)SortingDirection, (Enum)StiInteractionSortDirection.None);
		JsonExtensions.AddPropertyBool(val, "DrillDownEnabled", DrillDownEnabled, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DrillDownReport", DrillDownReport);
		JsonExtensions.AddPropertyEnum(val, "DrillDownMode", (Enum)DrillDownMode, (Enum)StiDrillDownMode.MultiPage);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DrillDownPageGuid", DrillDownPageGuid);
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter1", DrillDownParameter1.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter2", DrillDownParameter2.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter3", DrillDownParameter3.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter4", DrillDownParameter4.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter5", DrillDownParameter5.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter6", DrillDownParameter6.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter7", DrillDownParameter7.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter8", DrillDownParameter8.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter9", DrillDownParameter9.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DrillDownParameter10", DrillDownParameter10.SaveToJsonObject(mode));
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected O, but got Unknown
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "SortingEnabled":
				SortingEnabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SortingColumn":
				SortingColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SortingIndex":
				SortingIndex = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "SortingDirection":
				SortingDirection = StiJsonReportObjectHelper.DeserializeEnum<StiInteractionSortDirection>(item);
				break;
			case "DrillDownEnabled":
				DrillDownEnabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DrillDownReport":
				DrillDownReport = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DrillDownMode":
				DrillDownMode = StiJsonReportObjectHelper.DeserializeEnum<StiDrillDownMode>(item);
				break;
			case "DrillDownPageGuid":
				DrillDownPageGuid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DrillDownParameter1":
				DrillDownParameter1.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter2":
				DrillDownParameter2.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter3":
				DrillDownParameter3.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter4":
				DrillDownParameter4.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter5":
				DrillDownParameter5.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter6":
				DrillDownParameter6.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter7":
				DrillDownParameter7.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter8":
				DrillDownParameter8.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter9":
				DrillDownParameter9.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DrillDownParameter10":
				DrillDownParameter10.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	internal static StiInteraction LoadInteractionFromJsonObject(JObject jObject)
	{
		string text = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiInteraction stiInteraction = null;
		switch (text)
		{
		case "StiInteraction":
			stiInteraction = new StiInteraction();
			break;
		case "StiBandInteraction":
			stiInteraction = new StiBandInteraction();
			break;
		case "StiCrossHeaderInteraction":
			stiInteraction = new StiCrossHeaderInteraction();
			break;
		}
		stiInteraction.LoadFromJsonObject(jObject);
		return stiInteraction;
	}

	public object GetReport()
	{
		if (ParentComponent != null)
		{
			return ParentComponent.Report;
		}
		return null;
	}

	public object Clone()
	{
		return (StiInteraction)MemberwiseClone();
	}

	internal void ResetParametersToDefaultState()
	{
		DrillDownParameter1 = new StiDrillDownParameter();
		DrillDownParameter2 = new StiDrillDownParameter();
		DrillDownParameter3 = new StiDrillDownParameter();
		DrillDownParameter4 = new StiDrillDownParameter();
		DrillDownParameter5 = new StiDrillDownParameter();
		DrillDownParameter6 = new StiDrillDownParameter();
		DrillDownParameter7 = new StiDrillDownParameter();
		DrillDownParameter8 = new StiDrillDownParameter();
		DrillDownParameter9 = new StiDrillDownParameter();
		DrillDownParameter10 = new StiDrillDownParameter();
	}

	public string GetSortDataBandName()
	{
		if (string.IsNullOrEmpty(SortingColumn) || !SortingEnabled)
		{
			return string.Empty;
		}
		int num = StringExt.IndexOfInvariant(SortingColumn, ".");
		if (num == -1)
		{
			return string.Empty;
		}
		return SortingColumn.Substring(0, num);
	}

	public string[] GetSortColumns()
	{
		string sortColumnsString = GetSortColumnsString();
		if (sortColumnsString.Length != 0)
		{
			return sortColumnsString.Split(new char[1] { '.' });
		}
		return null;
	}

	public string GetSortColumnsString()
	{
		if (string.IsNullOrEmpty(SortingColumn) || !SortingEnabled)
		{
			return string.Empty;
		}
		int num = StringExt.IndexOfInvariant(SortingColumn, ".");
		if (num == -1)
		{
			return string.Empty;
		}
		return SortingColumn.Substring(num + 1);
	}

	public StiInteraction()
	{
		ResetParametersToDefaultState();
	}
}
