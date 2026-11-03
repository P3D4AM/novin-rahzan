using System;
using System.ComponentModel;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.SaveLoad;

public class StiDocument
{
	private StiResourcesCollection resources;

	[StiSerializable]
	public string ReportName
	{
		get
		{
			return Report.ReportName;
		}
		set
		{
			Report.ReportName = value;
		}
	}

	[StiSerializable]
	public string ReportAlias
	{
		get
		{
			return Report.ReportAlias;
		}
		set
		{
			Report.ReportAlias = value;
		}
	}

	[StiSerializable]
	public string ReportAuthor
	{
		get
		{
			return Report.ReportAuthor;
		}
		set
		{
			Report.ReportAuthor = value;
		}
	}

	[StiSerializable]
	public string ReportVersion
	{
		get
		{
			return Report.ReportVersion;
		}
		set
		{
			Report.ReportVersion = value;
		}
	}

	[StiSerializable]
	public string ReportDescription
	{
		get
		{
			return Report.ReportDescription;
		}
		set
		{
			Report.ReportDescription = value;
		}
	}

	[StiSerializable]
	public DateTime ReportCreated
	{
		get
		{
			return DateTime.Now;
		}
		set
		{
		}
	}

	[StiSerializable]
	[DefaultValue(StiRenderedWith.Unknown)]
	public StiRenderedWith RenderedWith
	{
		get
		{
			return Report.RenderedWith;
		}
		set
		{
			Report.RenderedWith = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiUnit Unit
	{
		get
		{
			return Report.Unit;
		}
		set
		{
			Report.Unit = value;
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	public string Culture
	{
		get
		{
			return Report.Culture;
		}
		set
		{
			Report.Culture = value;
		}
	}

	[StiSerializable]
	[DefaultValue(260039167)]
	public int PreviewSettings
	{
		get
		{
			return Report.PreviewSettings;
		}
		set
		{
			Report.PreviewSettings = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiPagesCollection Pages => Report.RenderedPages;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiVariablesCollection Variables => Report.Dictionary.Variables;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiResourcesCollection Resources
	{
		get
		{
			if (resources == null)
			{
				resources = Report.Dictionary.Resources;
			}
			return resources;
		}
		set
		{
			resources = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiStylesCollection Styles => Report.Styles;

	[Browsable(false)]
	[StiSerializable]
	public string Guid
	{
		get
		{
			return Report.ReportGuid;
		}
		set
		{
			Report.ReportGuid = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiBookmark Bookmark
	{
		get
		{
			return Report.Bookmark;
		}
		set
		{
			Report.Bookmark = value;
		}
	}

	public StiReport Report { get; set; }

	public StiDocument(StiReport report)
	{
		Report = report;
	}
}
