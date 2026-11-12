using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

[TypeConverter(typeof(StiRegionMapDashboardInteractionConverter))]
public class StiRegionMapDashboardInteraction : StiDashboardInteraction, IStiInteractionLayout
{
	public override StiInteractionIdent Ident { get; } = StiInteractionIdent.RegionMap;


	internal override StiAvailableInteractionOnClick AvailableOnClick => StiAvailableInteractionOnClick.ShowDashboard | StiAvailableInteractionOnClick.OpenHyperlink | StiAvailableInteractionOnClick.ApplyFilter;

	[DefaultValue(true)]
	[StiSerializable]
	public bool ShowFullScreenButton { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	public bool ShowViewDataButton { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	public bool ShowSaveButton { get; set; } = true;


	[DefaultValue(null)]
	[StiSerializable]
	public string FileName { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	public string HeaderText { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	public string FooterText { get; set; }

	public bool IsDefaultLayout
	{
		get
		{
			if (ShowFullScreenButton && ShowSaveButton && ShowViewDataButton && FileName == null && HeaderText == null)
			{
				return FooterText == null;
			}
			return false;
		}
	}

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (IsDefaultLayout && !ShouldSerializeOnHover() && !ShouldSerializeOnClick() && HyperlinkDestination == StiInteractionOpenHyperlinkDestination.NewTab && string.IsNullOrEmpty(Hyperlink) && string.IsNullOrEmpty(DrillDownPageKey) && DrillDownParameters != null && DrillDownParameters.Count == 0)
			{
				return string.IsNullOrEmpty(ToolTip);
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyBool("ShowFullScreenButton", ShowFullScreenButton, defaultValue: true);
		jObject.AddPropertyBool("ShowSaveButton", ShowSaveButton, defaultValue: true);
		jObject.AddPropertyBool("ShowViewDataButton", ShowViewDataButton, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("FileName", FileName);
		jObject.AddPropertyStringNullOrEmpty("HeaderText", HeaderText);
		jObject.AddPropertyStringNullOrEmpty("FooterText", FooterText);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowFullScreenButton":
				ShowFullScreenButton = item.DeserializeBool();
				break;
			case "ShowSaveButton":
				ShowSaveButton = item.DeserializeBool();
				break;
			case "ShowViewDataButton":
				ShowViewDataButton = item.DeserializeBool();
				break;
			case "FileName":
				FileName = item.DeserializeString();
				break;
			case "HeaderText":
				HeaderText = item.DeserializeString();
				break;
			case "FooterText":
				FooterText = item.DeserializeString();
				break;
			}
		}
	}

	public StiRegionMapDashboardInteraction()
	{
	}

	public StiRegionMapDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
	}

	public StiRegionMapDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters, string fileName, string headerText, string footerText, bool fullScreenButton, bool saveButton, bool viewDataButton)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
		FileName = fileName;
		HeaderText = headerText;
		FooterText = footerText;
		ShowFullScreenButton = fullScreenButton;
		ShowSaveButton = saveButton;
		ShowViewDataButton = viewDataButton;
	}
}
