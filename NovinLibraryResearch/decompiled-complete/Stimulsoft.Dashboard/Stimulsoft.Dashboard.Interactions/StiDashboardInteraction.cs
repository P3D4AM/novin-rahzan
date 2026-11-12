using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

[JsonObject]
[JsonConverter(typeof(StiDashboardInteractionJsonConverter))]
[TypeConverter(typeof(StiDashboardInteractionConverter))]
public abstract class StiDashboardInteraction : IStiDashboardInteraction, ICloneable, IStiDefault, IStiJsonReportObject
{
	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (!ShouldSerializeOnHover() && !ShouldSerializeOnClick() && HyperlinkDestination == StiInteractionOpenHyperlinkDestination.NewTab && string.IsNullOrEmpty(Hyperlink) && string.IsNullOrEmpty(DrillDownPageKey) && DrillDownParameters != null && DrillDownParameters.Count == 0)
			{
				return string.IsNullOrEmpty(ToolTip);
			}
			return false;
		}
	}

	[Browsable(false)]
	public abstract StiInteractionIdent Ident { get; }

	[Browsable(false)]
	internal virtual StiAvailableInteractionOnClick AvailableOnClick => StiAvailableInteractionOnClick.All;

	[Browsable(false)]
	internal virtual StiAvailableInteractionOnHover AvailableOnHover => StiAvailableInteractionOnHover.All;

	[Browsable(false)]
	internal virtual StiAvailableInteractionOnDataManipulation AvailableOnDataManipulation => StiAvailableInteractionOnDataManipulation.None;

	[DefaultValue(StiInteractionOnHover.ShowToolTip)]
	[StiSerializable]
	public virtual StiInteractionOnHover OnHover { get; set; } = StiInteractionOnHover.ShowToolTip;


	[DefaultValue(StiInteractionOnClick.ApplyFilter)]
	[StiSerializable]
	public virtual StiInteractionOnClick OnClick { get; set; } = StiInteractionOnClick.ApplyFilter;


	[DefaultValue(StiInteractionOpenHyperlinkDestination.NewTab)]
	[StiSerializable]
	public virtual StiInteractionOpenHyperlinkDestination HyperlinkDestination { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public virtual string ToolTip { get; set; } = "";


	[DefaultValue("")]
	[StiSerializable]
	public virtual string Hyperlink { get; set; } = "";


	[DefaultValue("")]
	[StiSerializable]
	public virtual string DrillDownPageKey { get; set; } = "";


	[StiSerializable(StiSerializationVisibility.List)]
	public virtual List<StiDashboardDrillDownParameter> DrillDownParameters { get; set; } = new List<StiDashboardDrillDownParameter>();


	public object Clone()
	{
		return (StiDashboardInteraction)MemberwiseClone();
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyIdent("Ident", Ident.ToString());
		jObject.AddPropertyEnum("OnHover", OnHover);
		jObject.AddPropertyEnum("OnClick", OnClick);
		jObject.AddPropertyEnum("HyperlinkDestination", HyperlinkDestination);
		jObject.AddPropertyStringNullOrEmpty("ToolTip", ToolTip);
		jObject.AddPropertyStringNullOrEmpty("DrillDownPageKey", DrillDownPageKey);
		jObject.AddPropertyStringNullOrEmpty("Hyperlink", Hyperlink);
		jObject.AddPropertyJObject("DrillDownParameters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DrillDownParameters, mode));
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "OnHover":
				OnHover = item.DeserializeEnum<StiInteractionOnHover>();
				break;
			case "OnClick":
				OnClick = item.DeserializeEnum<StiInteractionOnClick>();
				break;
			case "HyperlinkDestination":
				HyperlinkDestination = item.DeserializeEnum<StiInteractionOpenHyperlinkDestination>();
				break;
			case "ToolTip":
				ToolTip = item.DeserializeString();
				break;
			case "DrillDownPageKey":
				DrillDownPageKey = item.DeserializeString();
				break;
			case "Hyperlink":
				Hyperlink = item.DeserializeString();
				break;
			case "DrillDownParameters":
				DrillDownParameters.Clear();
				DrillDownParameters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDashboardDrillDownParameter.LoadFromJson((JObject)p.Value));
				break;
			}
		}
	}

	public List<IStiDashboardDrillDownParameter> GetDrillDownParameters()
	{
		return DrillDownParameters.Cast<IStiDashboardDrillDownParameter>().ToList();
	}

	public void SetDrillDownParameters(ArrayList drillDownParameters)
	{
		DrillDownParameters = new List<StiDashboardDrillDownParameter>();
		drillDownParameters?.Cast<Hashtable>().ToList().ForEach(delegate(Hashtable p)
		{
			DrillDownParameters.Add(new StiDashboardDrillDownParameter
			{
				Name = (p["name"] as string),
				Expression = (p["expression"] as string)
			});
		});
	}

	protected virtual bool ShouldSerializeOnHover()
	{
		return OnHover != StiInteractionOnHover.ShowToolTip;
	}

	protected virtual bool ShouldSerializeOnClick()
	{
		return OnClick != StiInteractionOnClick.ApplyFilter;
	}

	public StiDashboardInteraction()
	{
	}

	public StiDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters)
	{
		OnHover = onHover;
		OnClick = onClick;
		HyperlinkDestination = hyperlinkDestination;
		ToolTip = toolTip;
		Hyperlink = hyperlink;
		DrillDownPageKey = drillDownPageKey;
		DrillDownParameters = drillDownParameters;
	}
}
