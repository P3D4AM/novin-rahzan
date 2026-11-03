using System;
using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Viewer;

public class StiViewerConfigService : StiService
{
	public override string ServiceCategory => StiLocalization.Get("Services", "categoryRender");

	public override Type ServiceType => typeof(StiViewerConfigService);

	[StiServiceParam]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[RefreshProperties(RefreshProperties.All)]
	public virtual bool ToolSelectActive
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetBool("Viewer", "ToolSelectActive", nullValue: true);
		}
		set
		{
			StiSettings.Load();
			StiSettings.Set("Viewer", "ToolSelectActive", value);
			StiSettings.Save();
		}
	}

	[StiServiceParam]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[RefreshProperties(RefreshProperties.All)]
	public virtual bool ToolEditorActive
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetBool("Viewer", "ToolEditorActive", nullValue: false);
		}
		set
		{
			StiSettings.Load();
			StiSettings.Set("Viewer", "ToolEditorActive", value);
			StiSettings.Save();
		}
	}

	[StiServiceParam]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[RefreshProperties(RefreshProperties.All)]
	public virtual bool ToolHandActive
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetBool("Viewer", "ToolHandActive", nullValue: false);
		}
		set
		{
			StiSettings.Load();
			StiSettings.Set("Viewer", "ToolHandActive", value);
			StiSettings.Save();
		}
	}

	[DefaultValue(1.0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual double Zoom
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetDouble("Viewer", "Zoom", 1.0);
		}
		set
		{
			StiSettings.Load();
			StiSettings.Set("Viewer", "Zoom", value);
			StiSettings.Save();
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? ThumbsPanelEnabled { get; set; }

	[Browsable(false)]
	[Obsolete("Please use 'ThumbsPanelEnabled' property instead 'ShowThumbsPanel' property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool? ShowThumbsPanel
	{
		get
		{
			return ThumbsPanelEnabled;
		}
		set
		{
			ThumbsPanelEnabled = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? ZoomEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowZoom;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowZoom = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? FullScreenEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowFullScreen;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowFullScreen = value;
		}
	}

	[Browsable(false)]
	[Obsolete("Report Viewer does not use more SelectTool.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool? ToolSelectEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowSelectTool;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowSelectTool = value;
		}
	}

	[Browsable(false)]
	[Obsolete("Report Viewer does not use more SelectTool.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool? ToolHandEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowHandTool;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowHandTool = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? ToolFindEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowFindTool;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowFindTool = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? ToolEditorEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowEditorTool;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowEditorTool = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageNewEnabled
	{
		get
		{
			if (StiOptions.Viewer.Windows.ShowEditorTool != false)
			{
				return StiOptions.Viewer.Windows.ShowPageNewButton;
			}
			return false;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageNewButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageDeleteEnabled
	{
		get
		{
			if (StiOptions.Viewer.Windows.ShowEditorTool != false)
			{
				return StiOptions.Viewer.Windows.ShowPageDeleteButton;
			}
			return false;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageDeleteButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageDesignEnabled
	{
		get
		{
			if (StiOptions.Viewer.Windows.ShowEditorTool != false)
			{
				return StiOptions.Viewer.Windows.ShowPageDesignButton;
			}
			return false;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageDesignButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageSizeEnabled
	{
		get
		{
			if (StiOptions.Viewer.Windows.ShowEditorTool != false)
			{
				return StiOptions.Viewer.Windows.ShowPageSizeButton;
			}
			return false;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageSizeButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageViewModeEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPageViewMode;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageViewMode = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageViewModeSingleEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPageViewSingleMode;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageViewSingleMode = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageViewModeContinuousEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPageViewContinuousMode;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageViewContinuousMode = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageViewModeMultipleEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPageViewMultipleMode;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageViewMultipleMode = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PrintEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPrintButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPrintButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? CloseEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowCloseButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowCloseButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PageControlEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPageControl;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPageControl = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? FirstPageEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowFirstPage;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowFirstPage = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PreviousPageEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPreviousPage;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPreviousPage = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? NextPageEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowNextPage;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowNextPage = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? LastPageEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowLastPage;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowLastPage = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? SaveEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowSaveButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowSaveButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? OpenEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowOpenButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowOpenButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Obsolete("Please use SaveEnabled property instead ExportEnabled property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool? ExportEnabled
	{
		get
		{
			return SaveEnabled;
		}
		set
		{
			SaveEnabled = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? SendEMailEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowSendEMailButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowSendEMailButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? SaveDocumentFileEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowSaveDocumentFileButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowSaveDocumentFileButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? SendEMailDocumentFileEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowSendEMailDocumentFileButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowSendEMailDocumentFileButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? ToolbarEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowToolbar;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowToolbar = value;
		}
	}
}
