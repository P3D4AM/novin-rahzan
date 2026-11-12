using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

[TypeConverter(typeof(StiTableDashboardInteractionConverter))]
public class StiTableDashboardInteraction : StiDashboardInteraction, IStiTableDashboardInteraction, IStiDashboardInteraction, ICloneable, IStiDefault, IStiAllowUserSortingDashboardInteraction, IStiAllowUserColumnSelectionDashboardInteraction, IStiAllowUserFilteringDashboardInteraction, IStiInteractionLayout
{
	[StiSerializable]
	[DefaultValue(true)]
	public bool AllowUserSorting { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool AllowUserColumnSelection { get; set; } = true;


	[StiSerializable]
	public bool AllowUserFiltering { get; set; } = true;


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

	public override StiInteractionIdent Ident { get; } = StiInteractionIdent.Table;


	[Browsable(false)]
	internal override StiAvailableInteractionOnClick AvailableOnClick => StiAvailableInteractionOnClick.None;

	[Browsable(false)]
	internal override StiAvailableInteractionOnHover AvailableOnHover => StiAvailableInteractionOnHover.None;

	[Browsable(false)]
	internal override StiAvailableInteractionOnDataManipulation AvailableOnDataManipulation => StiAvailableInteractionOnDataManipulation.AllowSorting | StiAvailableInteractionOnDataManipulation.AllowFiltering | StiAvailableInteractionOnDataManipulation.AllowColumnSelection;

	[DefaultValue(true)]
	[StiSerializable]
	public bool DrillDownFiltered { get; set; } = true;


	[DefaultValue(false)]
	[StiSerializable]
	public bool FullRowSelect { get; set; }

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (IsDefaultLayout && AllowUserFiltering && AllowUserSorting && AllowUserColumnSelection && DrillDownFiltered && !FullRowSelect)
			{
				return base.IsDefault;
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyBool("AllowUserSorting", AllowUserSorting, defaultValue: true);
		jObject.AddPropertyBool("AllowUserColumnSelection", AllowUserColumnSelection, defaultValue: true);
		jObject.AddPropertyBool("AllowUserFiltering", AllowUserFiltering, defaultValue: true);
		jObject.AddPropertyBool("ShowFullScreenButton", ShowFullScreenButton, defaultValue: true);
		jObject.AddPropertyBool("ShowSaveButton", ShowSaveButton, defaultValue: true);
		jObject.AddPropertyBool("ShowViewDataButton", ShowViewDataButton, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("FileName", FileName);
		jObject.AddPropertyStringNullOrEmpty("HeaderText", HeaderText);
		jObject.AddPropertyStringNullOrEmpty("FooterText", FooterText);
		jObject.AddPropertyBool("DrillDownSelected", DrillDownFiltered, defaultValue: true);
		jObject.AddPropertyBool("FullRowSelect", FullRowSelect);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowUserSorting":
				AllowUserSorting = item.DeserializeBool();
				break;
			case "AllowUserColumnSelection":
				AllowUserColumnSelection = item.DeserializeBool();
				break;
			case "AllowUserFiltering":
				AllowUserFiltering = item.DeserializeBool();
				break;
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
			case "DrillDownSelected":
				DrillDownFiltered = item.DeserializeBool();
				break;
			case "FullRowSelect":
				FullRowSelect = item.DeserializeBool();
				break;
			}
		}
	}

	public StiTableDashboardInteraction()
	{
	}

	public StiTableDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters, bool allowFiltering, bool allowSorting)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
		AllowUserFiltering = allowFiltering;
		AllowUserSorting = allowSorting;
	}

	public StiTableDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters, bool allowFiltering, bool allowSorting, string fileName, string headerText, string footerText, bool fullScreenButton, bool saveButton, bool viewDataButton)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
		AllowUserFiltering = allowFiltering;
		AllowUserSorting = allowSorting;
		FileName = fileName;
		HeaderText = headerText;
		FooterText = footerText;
		ShowFullScreenButton = fullScreenButton;
		ShowSaveButton = saveButton;
		ShowViewDataButton = viewDataButton;
	}

	public StiTableDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters, bool allowFiltering, bool allowSorting, string fileName, string headerText, string footerText, bool fullScreenButton, bool saveButton, bool viewDataButton, bool drillDownSelected, bool fullRowSelected)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
		AllowUserFiltering = allowFiltering;
		AllowUserSorting = allowSorting;
		FileName = fileName;
		HeaderText = headerText;
		FooterText = footerText;
		ShowFullScreenButton = fullScreenButton;
		ShowSaveButton = saveButton;
		ShowViewDataButton = viewDataButton;
		DrillDownFiltered = drillDownSelected;
		FullRowSelect = fullRowSelected;
	}

	public StiTableDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters, bool allowFiltering, bool allowSorting, bool allowColumnSelection, string fileName, string headerText, string footerText, bool fullScreenButton, bool saveButton, bool viewDataButton, bool drillDownSelected, bool fullRowSelected)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
		AllowUserFiltering = allowFiltering;
		AllowUserSorting = allowSorting;
		AllowUserColumnSelection = allowColumnSelection;
		FileName = fileName;
		HeaderText = headerText;
		FooterText = footerText;
		ShowFullScreenButton = fullScreenButton;
		ShowSaveButton = saveButton;
		ShowViewDataButton = viewDataButton;
		DrillDownFiltered = drillDownSelected;
		FullRowSelect = fullRowSelected;
	}
}
