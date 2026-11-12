using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Viewer.Design;

[Browsable(false)]
[ToolboxItem(false)]
public class StiWinViewerWrapper : Control
{
	private bool isRibbonControl;

	public bool IsReportViewerActive { get; set; } = true;


	public object Viewer { get; private set; }

	public StiReport Report
	{
		get
		{
			if (!(Viewer is IStiViewerControl stiViewerControl))
			{
				return null;
			}
			return stiViewerControl.Report;
		}
		set
		{
			if (Viewer is IStiViewerControl)
			{
				IStiViewerControl stiViewerControl = Viewer as IStiViewerControl;
				stiViewerControl.Report = value;
			}
		}
	}

	public bool ShowZoom
	{
		get
		{
			return GetBoolProperty("ShowZoom");
		}
		set
		{
			SetProperty("ShowZoom", value);
		}
	}

	public bool ShowBookmarksPanel
	{
		get
		{
			return GetBoolProperty("ShowBookmarksPanel");
		}
		set
		{
			SetProperty("ShowBookmarksPanel", value);
		}
	}

	internal bool BookmarksButtonChecked
	{
		get
		{
			return GetBoolProperty("GetBookmarksButtonChecked");
		}
		set
		{
			InvokeMethod("SetBookmarksButtonChecked", value);
		}
	}

	public bool ShowThumbsPanel
	{
		get
		{
			return GetBoolProperty("ShowThumbsPanel");
		}
		set
		{
			SetProperty("ShowThumbsPanel", value);
		}
	}

	public bool ShowParametersButton
	{
		get
		{
			return GetBoolProperty("ShowParametersButton");
		}
		set
		{
			SetProperty("ShowParametersButton", value);
		}
	}

	public bool ShowResourcesButton
	{
		get
		{
			return GetBoolProperty("ShowResourcesButton");
		}
		set
		{
			SetProperty("ShowResourcesButton", value);
		}
	}

	public bool ThumbsPanelEnabled
	{
		get
		{
			return GetBoolProperty("ThumbsPanelEnabled");
		}
		set
		{
			SetProperty("ThumbsPanelEnabled", value);
		}
	}

	public bool ShowSelectTool
	{
		get
		{
			return GetBoolProperty("ShowSelectTool");
		}
		set
		{
			SetProperty("ShowSelectTool", value);
		}
	}

	public bool ShowEditorTool
	{
		get
		{
			return GetBoolProperty("ShowEditorTool");
		}
		set
		{
			SetProperty("ShowEditorTool", value);
		}
	}

	public bool ShowEditor
	{
		get
		{
			return GetBoolProperty("ShowEditor");
		}
		set
		{
			SetProperty("ShowEditor", value);
		}
	}

	public bool ShowFind
	{
		get
		{
			return GetBoolProperty("ShowFind");
		}
		set
		{
			SetProperty("ShowFind", value);
		}
	}

	public bool ShowZoomOnePage
	{
		get
		{
			return GetBoolProperty("ShowZoom");
		}
		set
		{
			SetProperty("ShowZoom", value);
		}
	}

	public bool ShowZoomTwoPages
	{
		get
		{
			return GetBoolProperty("ShowZoom");
		}
		set
		{
			SetProperty("ShowZoom", value);
		}
	}

	public bool ShowZoomMultiplePages
	{
		get
		{
			return GetBoolProperty("ShowZoom");
		}
		set
		{
			SetProperty("ShowZoom", value);
		}
	}

	public bool ShowZoomPageWidth
	{
		get
		{
			return GetBoolProperty("ShowZoom");
		}
		set
		{
			SetProperty("ShowZoom", value);
		}
	}

	public bool ShowComboBoxZoom
	{
		get
		{
			return GetBoolProperty("ShowComboBoxZoom");
		}
		set
		{
			SetProperty("ShowComboBoxZoom", value);
		}
	}

	public bool ShowSliderZoom
	{
		get
		{
			return GetBoolProperty("ShowSliderZoom");
		}
		set
		{
			SetProperty("ShowSliderZoom", value);
		}
	}

	public bool ShowViewModeSinglePage
	{
		get
		{
			return GetBoolProperty("ShowViewModeSinglePage");
		}
		set
		{
			SetProperty("ShowViewModeSinglePage", value);
		}
	}

	public bool ShowViewModeContinuous
	{
		get
		{
			return GetBoolProperty("ShowViewModeContinuous");
		}
		set
		{
			SetProperty("ShowViewModeContinuous", value);
		}
	}

	public bool ShowViewModeMultiplePages
	{
		get
		{
			return GetBoolProperty("ShowViewModeMultiplePages");
		}
		set
		{
			SetProperty("ShowViewModeMultiplePages", value);
		}
	}

	public bool ShowPageViewMode
	{
		get
		{
			return GetBoolProperty("ShowPageViewMode");
		}
		set
		{
			SetProperty("ShowPageViewMode", value);
		}
	}

	public bool ShowDotMatrixModeButton
	{
		get
		{
			return GetBoolProperty("ShowDotMatrixModeButton");
		}
		set
		{
			SetProperty("ShowDotMatrixModeButton", value);
		}
	}

	public bool ShowPageControl
	{
		get
		{
			return GetBoolProperty("ShowPageControl");
		}
		set
		{
			SetProperty("ShowPageControl", value);
		}
	}

	public bool ShowFirstPage
	{
		get
		{
			return GetBoolProperty("ShowFirstPage");
		}
		set
		{
			SetProperty("ShowFirstPage", value);
		}
	}

	public bool ShowPreviousPage
	{
		get
		{
			return GetBoolProperty("ShowPreviousPage");
		}
		set
		{
			SetProperty("ShowPreviousPage", value);
		}
	}

	public bool ShowNextPage
	{
		get
		{
			return GetBoolProperty("ShowNextPage");
		}
		set
		{
			SetProperty("ShowNextPage", value);
		}
	}

	public bool ShowLastPage
	{
		get
		{
			return GetBoolProperty("ShowLastPage");
		}
		set
		{
			SetProperty("ShowLastPage", value);
		}
	}

	public bool ShowPageNew
	{
		get
		{
			return GetBoolProperty("ShowPageNew");
		}
		set
		{
			SetProperty("ShowPageNew", value);
		}
	}

	public bool ShowPageDelete
	{
		get
		{
			return GetBoolProperty("ShowPageDelete");
		}
		set
		{
			SetProperty("ShowPageDelete", value);
		}
	}

	public bool ShowPageDesign
	{
		get
		{
			return GetBoolProperty("ShowPageDesign");
		}
		set
		{
			SetProperty("ShowPageDesign", value);
		}
	}

	public bool ShowPageSize
	{
		get
		{
			return GetBoolProperty("ShowPageSize");
		}
		set
		{
			SetProperty("ShowPageSize", value);
		}
	}

	public bool ShowToolbar
	{
		get
		{
			return GetBoolProperty("ShowToolbar");
		}
		set
		{
			SetProperty("ShowToolbar", value);
		}
	}

	public bool ShowHorScrollBar
	{
		get
		{
			return GetBoolProperty("ShowHorScrollBar");
		}
		set
		{
			SetProperty("ShowHorScrollBar", value);
		}
	}

	public bool ShowVertScrollBar
	{
		get
		{
			return GetBoolProperty("ShowVertScrollBar");
		}
		set
		{
			SetProperty("ShowVertScrollBar", value);
		}
	}

	public bool ShowStatusBar
	{
		get
		{
			return GetBoolProperty("ShowStatusBar");
		}
		set
		{
			SetProperty("ShowStatusBar", value);
		}
	}

	public bool ShowContextMenu
	{
		get
		{
			return GetBoolProperty("ShowContextMenu");
		}
		set
		{
			SetProperty("ShowContextMenu", value);
		}
	}

	public bool ShowCloseButton
	{
		get
		{
			return GetBoolProperty("ShowCloseButton");
		}
		set
		{
			SetProperty("ShowCloseButton", value);
		}
	}

	public bool ShowPrint
	{
		get
		{
			return GetBoolProperty("ShowPrint");
		}
		set
		{
			SetProperty("ShowPrint", value);
		}
	}

	public bool ShowOpen
	{
		get
		{
			return GetBoolProperty("ShowOpen");
		}
		set
		{
			SetProperty("ShowOpen", value);
		}
	}

	public bool ShowSave
	{
		get
		{
			return GetBoolProperty("ShowSave");
		}
		set
		{
			SetProperty("ShowSave", value);
		}
	}

	public bool ShowExport
	{
		get
		{
			return GetBoolProperty("ShowExport");
		}
		set
		{
			SetProperty("ShowExport", value);
		}
	}

	public bool ShowSendEMail
	{
		get
		{
			return GetBoolProperty("ShowSendEMail");
		}
		set
		{
			SetProperty("ShowSendEMail", value);
		}
	}

	public bool ShowSaveDocumentFile
	{
		get
		{
			return GetBoolProperty("ShowSaveDocumentFile");
		}
		set
		{
			SetProperty("ShowSaveDocumentFile", value);
		}
	}

	public bool ShowSendEMailDocumentFile
	{
		get
		{
			return GetBoolProperty("ShowSendEMailDocumentFile");
		}
		set
		{
			SetProperty("ShowSendEMailDocumentFile", value);
		}
	}

	public StiPreviewSettings PreviewSettings
	{
		get
		{
			return (StiPreviewSettings)GetProperty("PreviewSettings");
		}
		set
		{
			SetProperty("PreviewSettings", value);
		}
	}

	public bool IgnoreEscapeKey
	{
		get
		{
			return GetBoolProperty("IgnoreEscapeKey");
		}
		set
		{
			SetProperty("IgnoreEscapeKey", value);
		}
	}

	public bool BlockFormClose
	{
		get
		{
			return GetBoolProperty("BlockFormClose");
		}
		set
		{
			SetProperty("BlockFormClose", value);
		}
	}

	public bool GetFirstPageEnabled => GetBoolProperty("GetFirstPageEnabled");

	public bool GetPreviousPageEnabled => GetBoolProperty("GetPreviousPageEnabled");

	public bool GetPageControlEnabled => GetBoolProperty("GetPageControlEnabled");

	public bool GetNextPageEnabled => GetBoolProperty("GetNextPageEnabled");

	public bool GetLastPageEnabled => GetBoolProperty("GetLastPageEnabled");

	public bool GetFirstPageVisibility => GetBoolProperty("GetFirstPageVisibility");

	public bool GetPreviousPageVisibility => GetBoolProperty("GetPreviousPageVisibility");

	public bool GetPageControlVisibility => GetBoolProperty("GetPageControlVisibility");

	public bool GetNextPageVisibility => GetBoolProperty("GetNextPageVisibility");

	public bool GetLastPageVisibility => GetBoolProperty("GetLastPageVisibility");

	public bool GetViewModeSinglePageEnabled => GetBoolProperty("GetViewModeSinglePageEnabled");

	public bool GetViewModeContinuousEnabled => GetBoolProperty("GetViewModeContinuousEnabled");

	public bool GetViewModeMultiplePagesEnabled => GetBoolProperty("GetViewModeMultiplePagesEnabled");

	public bool GetViewModeSinglePageVisibility => GetBoolProperty("GetViewModeSinglePageVisibility");

	public bool GetViewModeContinuousVisibility => GetBoolProperty("GetViewModeContinuousVisibility");

	public bool GetViewModeMultiplePagesVisibility => GetBoolProperty("GetViewModeMultiplePagesVisibility");

	public StiPageViewMode GetPageViewMode
	{
		get
		{
			object property = GetProperty("PageViewMode");
			if (!(property is StiPageViewMode))
			{
				return StiPageViewMode.SinglePage;
			}
			return (StiPageViewMode)property;
		}
	}

	public double Zoom => GetDoubleProperty("Zoom", 1.0);

	private bool GetBoolProperty(string name)
	{
		object property = GetProperty(name);
		if (property is bool)
		{
			return (bool)property;
		}
		return false;
	}

	private double GetDoubleProperty(string name, double defaultValue)
	{
		object property = GetProperty(name);
		if (!(property is double))
		{
			return defaultValue;
		}
		return (double)property;
	}

	private object GetProperty(string name)
	{
		return (Viewer?.GetType().GetProperty(name))?.GetValue(Viewer, new object[0]);
	}

	private void SetProperty(string name, object value)
	{
		(Viewer?.GetType().GetProperty(name))?.SetValue(Viewer, value, new object[0]);
	}

	private void InvokeMethod(string methodName, object value)
	{
		(Viewer?.GetType().GetMethod(methodName))?.Invoke(Viewer, new object[1] { value });
	}

	public void ApplyStyle()
	{
		(Viewer?.GetType().GetMethod("ApplyStyle"))?.Invoke(Viewer, new object[0]);
	}

	public void SaveConfig()
	{
		(Viewer?.GetType().GetMethod("SaveConfig"))?.Invoke(Viewer, new object[0]);
	}

	public void LoadConfig()
	{
		(Viewer?.GetType().GetMethod("LoadConfig"))?.Invoke(Viewer, new object[0]);
	}

	public void Localize()
	{
		(Viewer?.GetType().GetMethod("Localize"))?.Invoke(Viewer, new object[0]);
	}

	public void SetZoom(double zoom)
	{
		(Viewer?.GetType().GetMethod("SetZoom", new Type[1] { typeof(double) }))?.Invoke(Viewer, new object[1] { zoom });
	}

	public void RefreshPageNofM()
	{
		(Viewer?.GetType().GetMethod("RefreshPageNofM", BindingFlags.Instance | BindingFlags.NonPublic))?.Invoke(Viewer, new object[0]);
	}

	public string GetPageNofM()
	{
		return (string)(Viewer?.GetType().GetMethod("GetPageNofM"))?.Invoke(Viewer, new object[0]);
	}

	public void JumpToPage()
	{
		(Viewer?.GetType().GetMethod("JumpToPage"))?.Invoke(Viewer, new object[0]);
	}

	public void FirstPage()
	{
		(Viewer?.GetType().GetMethod("FirstPage"))?.Invoke(Viewer, new object[0]);
	}

	public void PrevPage()
	{
		(Viewer?.GetType().GetMethod("PrevPage"))?.Invoke(Viewer, new object[0]);
	}

	public void NextPage()
	{
		Viewer.GetType().GetMethod("NextPage")?.Invoke(Viewer, new object[0]);
	}

	public void LastPage()
	{
		(Viewer?.GetType().GetMethod("LastPage"))?.Invoke(Viewer, new object[0]);
	}

	public void InvokeViewModeSinglePage()
	{
		(Viewer?.GetType().GetMethod("InvokeViewModeSinglePage"))?.Invoke(Viewer, new object[0]);
	}

	public void InvokeViewModeContinuous()
	{
		(Viewer?.GetType().GetMethod("InvokeViewModeContinuous"))?.Invoke(Viewer, new object[0]);
	}

	public void InvokeViewModeMultiplePages()
	{
		(Viewer?.GetType().GetMethod("InvokeViewModeMultiplePages"))?.Invoke(Viewer, new object[0]);
	}

	public void InvokeZoomOnePage()
	{
		(Viewer?.GetType().GetMethod("InvokeZoomOnePage"))?.Invoke(Viewer, new object[0]);
	}

	internal void ResetShortcuts()
	{
		if (isRibbonControl && Viewer != null)
		{
			(Viewer?.GetType().GetMethod("ResetShortcuts"))?.Invoke(Viewer, null);
		}
	}

	internal void InitShortcuts()
	{
		if (isRibbonControl && Viewer != null)
		{
			(Viewer?.GetType().GetMethod("InitShortcuts"))?.Invoke(Viewer, null);
		}
	}

	internal void ClearZoomFields()
	{
		(Viewer?.GetType().GetMethod("ClearZoomFields", BindingFlags.Instance | BindingFlags.NonPublic))?.Invoke(Viewer, new object[0]);
	}

	internal void LockButtonOpen(bool value)
	{
		(Viewer?.GetType().GetProperty("LockButtonOpen", BindingFlags.Instance | BindingFlags.NonPublic))?.SetValue(Viewer, value, null);
	}

	internal void UpdateControlsState()
	{
		(Viewer?.GetType().GetMethod("UpdateControlsState", BindingFlags.Instance | BindingFlags.NonPublic))?.Invoke(Viewer, new object[0]);
	}

	private static Control ToControl(object value)
	{
		return (Control)((value is Control) ? value : null);
	}

	private void InitializeViewer(StiReport report, bool removeShortcuts, bool allowRibbon)
	{
		if (allowRibbon)
		{
			Type type = Type.GetType("Stimulsoft.Report.Viewer.StiRibbonViewerControl, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
			}
			Viewer = StiActivator.CreateObject(type, new object[3] { report, removeShortcuts, false });
			ToControl(Viewer).Dock = (DockStyle)5;
			((Control)this).Controls.Add(ToControl(Viewer));
			if (StiOptions.Viewer.HideStatusBarInDesigner)
			{
				ShowStatusBar = false;
			}
			isRibbonControl = true;
		}
		else
		{
			Type type2 = Type.GetType("Stimulsoft.Report.Viewer.StiViewerControl, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type2 == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
			}
			Viewer = StiActivator.CreateObject(type2, new object[2] { report, removeShortcuts });
			ToControl(Viewer).Dock = (DockStyle)5;
			((Control)this).Controls.Add(ToControl(Viewer));
		}
	}

	public StiWinViewerWrapper()
		: this(null)
	{
	}

	public StiWinViewerWrapper(StiReport report)
		: this(report, removeShortcuts: false)
	{
	}

	public StiWinViewerWrapper(StiReport report, bool removeShortcuts)
		: this(report, removeShortcuts, allowRibbon: true)
	{
	}

	public StiWinViewerWrapper(StiReport report, bool removeShortcuts, bool allowRibbon)
	{
		if (!((Component)this).DesignMode)
		{
			InitializeViewer(report, removeShortcuts, allowRibbon);
		}
	}
}
