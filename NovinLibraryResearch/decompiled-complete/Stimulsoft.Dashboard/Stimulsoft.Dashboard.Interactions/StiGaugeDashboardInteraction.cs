using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

[TypeConverter(typeof(StiGaugeDashboardInteractionConverter))]
public class StiGaugeDashboardInteraction : StiDashboardInteraction, IStiAllowUserSortingDashboardInteraction, IStiInteractionLayout
{
	public override StiInteractionIdent Ident { get; } = StiInteractionIdent.Gauge;


	internal override StiAvailableInteractionOnClick AvailableOnClick => StiAvailableInteractionOnClick.None;

	[Browsable(false)]
	internal override StiAvailableInteractionOnHover AvailableOnHover => StiAvailableInteractionOnHover.None;

	[Browsable(false)]
	internal override StiAvailableInteractionOnDataManipulation AvailableOnDataManipulation => StiAvailableInteractionOnDataManipulation.AllowSorting;

	[StiSerializable]
	[DefaultValue(true)]
	public bool AllowUserSorting { get; set; } = true;


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
			if (IsDefaultLayout)
			{
				return AllowUserSorting;
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyBool("AllowUserSorting", AllowUserSorting, defaultValue: true);
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
			case "AllowUserSorting":
				AllowUserSorting = item.DeserializeBool();
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

	public StiGaugeDashboardInteraction()
	{
	}

	public StiGaugeDashboardInteraction(string fileName, string headerText, string footerText, bool fullScreenButton, bool saveButton, bool viewDataButton)
	{
		FileName = fileName;
		HeaderText = headerText;
		FooterText = footerText;
		ShowFullScreenButton = fullScreenButton;
		ShowSaveButton = saveButton;
		ShowViewDataButton = viewDataButton;
	}

	public StiGaugeDashboardInteraction(string fileName, string headerText, string footerText, bool fullScreenButton, bool saveButton, bool viewDataButton, bool allowUserSorting)
	{
		FileName = fileName;
		HeaderText = headerText;
		FooterText = footerText;
		ShowFullScreenButton = fullScreenButton;
		ShowSaveButton = saveButton;
		ShowViewDataButton = viewDataButton;
		AllowUserSorting = allowUserSorting;
	}
}
