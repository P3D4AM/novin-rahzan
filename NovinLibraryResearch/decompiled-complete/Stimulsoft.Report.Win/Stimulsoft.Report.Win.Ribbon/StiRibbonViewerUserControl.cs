using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Win.Ribbon;

[ToolboxItem(false)]
public class StiRibbonViewerUserControl : UserControl
{
	private StiRibbonViewerControl rvControl;

	private IContainer components;

	private ButtonItem biOpen;

	private ButtonItem biSave;

	private ButtonItem biSendEMail;

	private ButtonItem biPageNew;

	private ButtonItem biPageDelete;

	private ButtonItem biPageDesign;

	private ButtonItem biPageSize;

	private ButtonItem biPrint;

	private ItemContainer itemContainer1;

	private ItemContainer itemContainer2;

	private ItemContainer itemContainer4;

	private ButtonItem biZoomOnePage;

	private ButtonItem biZoomTwoPages;

	private ItemContainer itemContainer3;

	private ButtonItem biZoomMultiplePages;

	private ButtonItem biZoomPageWidth;

	private ButtonItem biSendEMailDocumentFile;

	private RibbonBar ribbonBarFile;

	private RibbonBar ribbonBarEdit;

	private RibbonBar ribbonBarViewing;

	private RibbonBar ribbonBarClose;

	private ButtonItem biSaveDocumentFile;

	private ControlContainerItem cMultiPagesUserControl;

	private RibbonBar ribbonBarTools;

	private ButtonItem biFind;

	private RibbonBar ribbonBarPanels;

	internal ButtonItem biThumbs;

	internal ButtonItem biParameters;

	internal ButtonItem biResources;

	internal ButtonItem biBookmarks;

	private ButtonItem biEditor;

	private ItemContainer itemContainer5;

	private ButtonItem biFullScreen;

	private ItemContainer itemContainer6;

	private ButtonItem biDotMatrix;

	private ButtonItem biClose;

	private void biPageSize_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickPageSizeButton();
	}

	private void biPageDesign_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickPageDesignButton();
	}

	private void biPageDelete_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickPageDeleteButton();
	}

	private void biPageNew_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickPageNewButton();
	}

	private void biSendEMailDocumentFile_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickSaveSendEMailDocumentButton();
	}

	private void biSendEMail_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		((BaseItem)biSendEMailDocumentFile).Visible = rvControl.GetSendEMailDocumentFileVisibility;
	}

	private void biSaveDocumentFile_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickSaveButton();
	}

	private void biSave_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		((BaseItem)biSaveDocumentFile).Visible = rvControl.GetSaveDocumentFileVisibility;
	}

	private void biOpen_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickOpenButton();
	}

	private void biPrint_Click(object sender, EventArgs e)
	{
		rvControl.InvokeClickPrintButton();
	}

	private void biDotMatrix_Click(object sender, EventArgs e)
	{
		rvControl.Report.ShowDotMatrixWithRibbonGUI();
	}

	private void DoScaling()
	{
		biSaveDocumentFile.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.SaveDocumentFile" + StiScale.StepName + ".png");
		biSendEMailDocumentFile.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.SaveDocumentFile" + StiScale.StepName + ".png");
		biOpen.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportOpen" + StiScale.StepName + ".png");
		biSave.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportSave" + StiScale.StepName + ".png");
		biSendEMail.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.SendEMail" + StiScale.StepName + ".png");
		biPrint.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportPrint" + StiScale.BigStepName + ".png");
		biBookmarks.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Bookmarks" + StiScale.BigStepName + ".png");
		biParameters.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Parameters" + StiScale.BigStepName + ".png");
		biResources.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Resources" + StiScale.BigStepName + ".png");
		biEditor.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Editor" + StiScale.BigStepName + ".png");
		biThumbs.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Thumbnails" + StiScale.BigStepName + ".png");
		biFind.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Find" + StiScale.BigStepName + ".png");
		biZoomOnePage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomOnePage" + StiScale.StepName + ".png");
		biZoomTwoPages.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomTwoPages" + StiScale.StepName + ".png");
		biZoomMultiplePages.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomMultiplePages" + StiScale.StepName + ".png");
		biZoomPageWidth.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomPageWidth" + StiScale.BigStepName + ".png");
		biFullScreen.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.FullScreen" + StiScale.BigStepName + ".png");
		biDotMatrix.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.DotMatrix" + StiScale.BigStepName + ".png");
		biPageSize.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageSize" + StiScale.BigStepName + ".png");
		biPageNew.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageNew" + StiScale.StepName + ".png");
		biPageDelete.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageDelete" + StiScale.StepName + ".png");
		biPageDesign.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageDesign" + StiScale.StepName + ".png");
		biClose.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Close" + StiScale.BigStepName + ".png");
	}

	private void InitEventsButton()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		((BaseItem)biPrint).Click += biPrint_Click;
		((BaseItem)biOpen).Click += biOpen_Click;
		((PopupItem)biSave).PopupOpen += new PopupOpenEventHandler(biSave_PopupOpen);
		((BaseItem)biSaveDocumentFile).Click += biSaveDocumentFile_Click;
		((PopupItem)biSendEMail).PopupOpen += new PopupOpenEventHandler(biSendEMail_PopupOpen);
		((BaseItem)biSendEMailDocumentFile).Click += biSendEMailDocumentFile_Click;
		((BaseItem)biPageNew).Click += biPageNew_Click;
		((BaseItem)biPageDelete).Click += biPageDelete_Click;
		((BaseItem)biPageDesign).Click += biPageDesign_Click;
		((BaseItem)biPageSize).Click += biPageSize_Click;
		biBookmarks.CheckedChanged += rvControl.ButtonBookmarks_CheckedChanged;
		biThumbs.CheckedChanged += rvControl.ButtonThumbs_Click;
		biParameters.CheckedChanged += rvControl.ButtonParameters_CheckedChanged;
		biResources.CheckedChanged += rvControl.ButtonResources_CheckedChanged;
		((BaseItem)biFind).Click += rvControl.ButtonToolFind_Click;
		((BaseItem)biEditor).Click += rvControl.ButtonToolEditor_Click;
		biEditor.CheckedChanged += rvControl.ButtonToolEditor_CheckedChanged;
		((BaseItem)biFullScreen).Click += rvControl.ButtonFullScreen_Click;
		((BaseItem)biZoomOnePage).Click += rvControl.ButtonZoomOnePage_Click;
		((BaseItem)biZoomTwoPages).Click += rvControl.ButtonZoomTwoPages_Click;
		((BaseItem)biZoomPageWidth).Click += rvControl.ButtonZoomPageWidth_Click;
		cMultiPagesUserControl.Control = (Control)(object)rvControl.multiPagesUserControl;
		((BaseItem)biDotMatrix).Click += biDotMatrix_Click;
		((BaseItem)biClose).Click += rvControl.ButtonClose_Click;
	}

	private void UpdateButton()
	{
		rvControl.tbPrint = biPrint;
		rvControl.tbOpen = biOpen;
		rvControl.tbSave = biSave;
		rvControl.tbSaveDocumentFile = biSaveDocumentFile;
		rvControl.tbSendEMail = biSendEMail;
		rvControl.tbSendEMailDocumentFile = biSendEMailDocumentFile;
		rvControl.tbPageNew = biPageNew;
		rvControl.tbPageDelete = biPageDelete;
		rvControl.tbPageDesign = biPageDesign;
		rvControl.tbPageSize = biPageSize;
		rvControl.tbBookmarks = biBookmarks;
		rvControl.tbThumbs = biThumbs;
		rvControl.tbParameters = biParameters;
		rvControl.tbResources = biResources;
		rvControl.tbFind = biFind;
		rvControl.tbEditor = biEditor;
		rvControl.tbFullScreen = biFullScreen;
		rvControl.tbZoomOnePage = biZoomOnePage;
		rvControl.tbZoomTwoPages = biZoomTwoPages;
		rvControl.tbZoomMultiplePages = biZoomMultiplePages;
		rvControl.tbZoomPageWidth = biZoomPageWidth;
		rvControl.tbClose = biClose;
		rvControl.tbDotMatrix = biDotMatrix;
	}

	private void BuildExportMenu()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		if (!StiOptions.Services.Exports.Any((StiExportService s) => ((StiService)s).ServiceEnabled))
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		StiStringCollection val = new StiStringCollection();
		foreach (StiExportService item in StiOptions.Services.Exports.Where((StiExportService s) => ((StiService)s).ServiceEnabled))
		{
			if (rvControl.AllowExportService(item) && hashtable[item.GroupCategory] == null)
			{
				hashtable.Add(item.GroupCategory, item.GroupCategory);
				val.Add(item.GroupCategory);
			}
		}
		foreach (string item2 in (CollectionBase)(object)val)
		{
			int num = 0;
			foreach (StiExportService item3 in StiOptions.Services.Exports.Where((StiExportService s) => ((StiService)s).ServiceEnabled))
			{
				if (!rvControl.AllowExportService(item3) || !(item2 == item3.GroupCategory))
				{
					continue;
				}
				string text2 = GetExportServiceText(item3).Replace("...", "");
				if ((!(item3.GroupCategory == "Image") || item3 is StiImageExportService) && !(item3.GroupCategory == "Meta") && (!(item3.GroupCategory == "Excel") || item3 is StiOdsExportService || item3 is StiExcelExportService) && (!(item3.GroupCategory == "Web") || item3 is StiHtmlExportService) && (!(item3.GroupCategory == "Data") || item3 is StiDataExportService))
				{
					ButtonItem val2 = new ButtonItem(item3.ExportNameInMenu, text2);
					val2.Image = (Image)(object)GetImageExport(item3);
					val2.ImageFixedSize = new Size(StiScale.XXI(16.0), StiScale.YYI(16.0));
					if (num == 0)
					{
						((BaseItem)val2).BeginGroup = true;
					}
					((BaseItem)val2).Tag = item3;
					((BaseItem)val2).Click += rvControl.ItemExport_Click;
					((BaseItem)biSave).SubItems.Add((BaseItem)(object)val2);
					ButtonItem val3 = new ButtonItem(item3.ExportNameInMenu, text2);
					val3.Image = (Image)(object)GetImageExport(item3);
					if (num == 0)
					{
						((BaseItem)val3).BeginGroup = true;
					}
					((BaseItem)val3).Tag = item3;
					val3.ImageFixedSize = new Size(StiScale.XXI(16.0), StiScale.YYI(16.0));
					((BaseItem)val3).Click += rvControl.ItemSendEMail_Click;
					((BaseItem)biSendEMail).SubItems.Add((BaseItem)(object)val3);
				}
				num++;
			}
		}
	}

	private Bitmap GetImageExport(StiExportService service)
	{
		Bitmap val = null;
		string pathImageOffice2013FromBitmapService = StiImageHelper.GetPathImageOffice2013FromBitmapService((StiService)(object)service, StiScale.Step);
		if (!string.IsNullOrEmpty(pathImageOffice2013FromBitmapService))
		{
			val = StiImageUtils.GetImage("Stimulsoft.Report", pathImageOffice2013FromBitmapService);
		}
		if (val == null)
		{
			val = StiServiceBitmapAttribute.GetImage(((object)service).GetType());
		}
		return val;
	}

	private void UpdateControlsState()
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Expected O, but got Unknown
		((BaseItem)biPrint).Visible = rvControl.GetPrintVisibility;
		((BaseItem)biOpen).Visible = rvControl.GetOpenVisibility;
		((BaseItem)biSave).Visible = rvControl.GetSaveVisibility;
		((BaseItem)biSendEMail).Visible = rvControl.GetSendEMailVisibility;
		if (!((BaseItem)biPrint).Visible && !((BaseItem)biOpen).Visible && !((BaseItem)biSave).Visible && !((BaseItem)biSendEMail).Visible)
		{
			((Control)ribbonBarFile).Visible = false;
		}
		else
		{
			((Control)ribbonBarFile).Visible = true;
			((Control)ribbonBarFile).Refresh();
		}
		((BaseItem)biBookmarks).Visible = rvControl.GetBookmarksPanelVisibility;
		((BaseItem)biThumbs).Visible = rvControl.GetThumbsPanelVisibility;
		((BaseItem)biParameters).Visible = rvControl.GetParametersButtonVisibility;
		((BaseItem)biResources).Visible = rvControl.GetResourcesButtonVisibility;
		((Control)ribbonBarPanels).Width = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)ribbonBarPanels).Controls)
		{
			Control val = item;
			RibbonBar obj = ribbonBarPanels;
			((Control)obj).Width = ((Control)obj).Width + val.Width;
		}
		((BaseItem)biFind).Visible = rvControl.GetFindVisibility;
		((BaseItem)biEditor).Visible = rvControl.GetEditorVisibilityInRibbonMode;
		if (((BaseItem)biFind).Visible && ((BaseItem)biEditor).Visible)
		{
			((Control)ribbonBarTools).Width = 150;
		}
		((BaseItem)biFullScreen).Visible = rvControl.GetFullScreenVisibility;
		bool getZoomOnePageVisibility = rvControl.GetZoomOnePageVisibility;
		bool getZoomTwoPagesVisibility = rvControl.GetZoomTwoPagesVisibility;
		bool getZoomMultiplePagesVisibility = rvControl.GetZoomMultiplePagesVisibility;
		((BaseItem)biZoomPageWidth).Visible = rvControl.GetZoomPageWidthVisibility;
		if (getZoomOnePageVisibility && !getZoomTwoPagesVisibility && !getZoomMultiplePagesVisibility)
		{
			biZoomOnePage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomOnePage" + StiScale.BigStepName + ".png");
			biZoomOnePage.ImagePosition = (eImagePosition)2;
		}
		else
		{
			biZoomOnePage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomOnePage" + StiScale.StepName + ".png");
			biZoomOnePage.ImagePosition = (eImagePosition)0;
		}
		((BaseItem)biZoomOnePage).Visible = getZoomOnePageVisibility;
		((BaseItem)biZoomTwoPages).Visible = getZoomTwoPagesVisibility;
		((BaseItem)biZoomMultiplePages).Visible = getZoomMultiplePagesVisibility;
		((BaseItem)biDotMatrix).Visible = rvControl.GetDotMatrixModeButtonVisibility;
		((BaseItem)biPageNew).Visible = rvControl.GetPageNewVisibility;
		((BaseItem)biPageDelete).Visible = rvControl.GetPageDeleteVisibility;
		((BaseItem)biPageDesign).Visible = rvControl.GetPageDesignVisibility;
		((BaseItem)biPageSize).Visible = rvControl.GetPageSizeVisibility;
		if (!rvControl.GetPageSizeVisibility && !rvControl.GetPageNewVisibility && !rvControl.GetPageDeleteVisibility && !rvControl.GetPageDesignVisibility)
		{
			((Control)ribbonBarEdit).Visible = false;
		}
		else
		{
			((Control)ribbonBarEdit).Visible = true;
			((Control)ribbonBarEdit).Refresh();
		}
		if (!rvControl.GetBookmarksPanelVisibility && !rvControl.GetParametersButtonVisibility && !rvControl.GetThumbsPanelVisibility)
		{
			((Control)ribbonBarPanels).Visible = false;
		}
		else
		{
			((Control)ribbonBarPanels).Visible = true;
			((Control)ribbonBarPanels).Refresh();
		}
		if (!rvControl.GetFindVisibility && !rvControl.GetEditorVisibility)
		{
			((Control)ribbonBarTools).Visible = false;
		}
		else
		{
			((Control)ribbonBarTools).Visible = true;
			((Control)ribbonBarTools).Refresh();
		}
		if (!rvControl.GetZoomOnePageVisibility && !rvControl.GetZoomTwoPagesVisibility && !rvControl.GetZoomMultiplePagesVisibility && !rvControl.GetZoomPageWidthVisibility && !rvControl.GetFullScreenVisibility && !rvControl.GetDotMatrixModeButtonVisibility && !rvControl.GetCloseVisibility)
		{
			((Control)ribbonBarViewing).Visible = false;
		}
		else
		{
			((Control)ribbonBarViewing).Visible = true;
			((Control)ribbonBarViewing).Width = 0;
			foreach (Control item2 in (ArrangedElementCollection)((Control)ribbonBarViewing).Controls)
			{
				Control val2 = item2;
				RibbonBar obj2 = ribbonBarViewing;
				((Control)obj2).Width = ((Control)obj2).Width + val2.Width;
			}
			((Control)ribbonBarViewing).Refresh();
		}
		((Control)ribbonBarClose).Visible = rvControl.GetCloseVisibility;
		((Control)ribbonBarClose).Refresh();
		((BaseItem)biPrint).Enabled = rvControl.GetPrintEnabled;
		((BaseItem)biOpen).Enabled = rvControl.GetOpenEnabled;
		((BaseItem)biSave).Enabled = rvControl.GetSaveEnabled;
		((BaseItem)biSendEMail).Enabled = rvControl.GetSendEMailEnabled;
		((BaseItem)biBookmarks).Enabled = rvControl.GetBookmarksPanelEnabled;
		((BaseItem)biThumbs).Enabled = rvControl.GetThumbsPanelEnabled;
		((BaseItem)biParameters).Enabled = rvControl.GetParametersPanelEnabled;
		((BaseItem)biResources).Enabled = rvControl.GetResourcesPanelEnabled;
		((BaseItem)biFind).Enabled = rvControl.GetFindEnabled;
		((BaseItem)biEditor).Enabled = rvControl.GetEditorEnabledInRibbon;
		((BaseItem)biFullScreen).Enabled = rvControl.GetFullScreenEnabled;
		((BaseItem)biZoomOnePage).Enabled = rvControl.GetZoomOnePageEnabled;
		((BaseItem)biZoomTwoPages).Enabled = rvControl.GetZoomTwoPagesEnabled;
		((BaseItem)biZoomMultiplePages).Enabled = rvControl.GetZoomMultiplePagesEnabled;
		((BaseItem)biZoomPageWidth).Enabled = rvControl.GetZoomPageWidthEnabled;
		((BaseItem)biDotMatrix).Enabled = rvControl.GetDotMatrixModeButtonEnabled;
		((BaseItem)biPageNew).Enabled = rvControl.GetPageNewEnabled;
		((BaseItem)biPageDelete).Enabled = rvControl.GetPageDeleteEnabled;
		((BaseItem)biPageDesign).Enabled = rvControl.GetPageDesignEnabled;
		((BaseItem)biPageSize).Enabled = rvControl.GetPageSizeEnabled;
		if (!rvControl.GetPageNewEnabled && !rvControl.GetPageDeleteEnabled && !rvControl.GetPageDesignEnabled && !rvControl.GetPageSizeEnabled)
		{
			((Control)ribbonBarEdit).Visible = false;
		}
		((BaseItem)biClose).Enabled = rvControl.GetCloseEnabled;
		((BaseItem)biZoomOnePage).Enabled = rvControl.GetZoomOnePageEnabled;
		((BaseItem)biZoomPageWidth).Enabled = rvControl.GetZoomPageWidthEnabled;
		((BaseItem)biZoomTwoPages).Enabled = rvControl.GetZoomTwoPagesEnabled;
		biFind.Checked = rvControl.GetFindChecked;
		biEditor.Checked = rvControl.GetEditorChecked;
		biBookmarks.Checked = rvControl.GetBookmarksButtonChecked;
		biThumbs.Checked = rvControl.GetThumbsButtonChecked;
		biResources.Checked = rvControl.GetResourcesButtonChecked;
		biZoomOnePage.Checked = rvControl.GetZoomOnePageChecked;
		biZoomTwoPages.Checked = rvControl.GetZoomTwoPagesChecked;
		biZoomMultiplePages.Checked = rvControl.GetZoomMultiplePagesChecked;
		biZoomPageWidth.Checked = rvControl.GetZoomPageWidthChecked;
		if (!((BaseItem)biPageNew).Visible)
		{
			if (((BaseItem)biPageDelete).Visible)
			{
				((BaseItem)biPageDelete).BeginGroup = true;
			}
			else if (((BaseItem)biPageDesign).Visible)
			{
				((BaseItem)biPageDesign).BeginGroup = true;
			}
			else if (((BaseItem)biPageSize).Visible)
			{
				((BaseItem)biPageSize).BeginGroup = true;
			}
		}
		else
		{
			if (((BaseItem)biPageDelete).BeginGroup)
			{
				((BaseItem)biPageDelete).BeginGroup = false;
			}
			if (((BaseItem)biPageDesign).BeginGroup)
			{
				((BaseItem)biPageDesign).BeginGroup = false;
			}
			if (((BaseItem)biPageSize).BeginGroup)
			{
				((BaseItem)biPageSize).BeginGroup = false;
			}
		}
		if (!((BaseItem)biFullScreen).Visible)
		{
			if (((BaseItem)biZoomOnePage).Visible)
			{
				((BaseItem)biZoomOnePage).BeginGroup = true;
			}
			else if (((BaseItem)biZoomTwoPages).Visible)
			{
				((BaseItem)biZoomTwoPages).BeginGroup = true;
			}
			else if (((BaseItem)biZoomMultiplePages).Visible)
			{
				((BaseItem)biZoomMultiplePages).BeginGroup = true;
			}
			else if (((BaseItem)biZoomPageWidth).Visible)
			{
				((BaseItem)biZoomPageWidth).BeginGroup = true;
			}
		}
		else
		{
			if (((BaseItem)biZoomOnePage).BeginGroup)
			{
				((BaseItem)biZoomOnePage).BeginGroup = false;
			}
			if (((BaseItem)biZoomTwoPages).BeginGroup)
			{
				((BaseItem)biZoomTwoPages).BeginGroup = false;
			}
			if (((BaseItem)biZoomMultiplePages).BeginGroup)
			{
				((BaseItem)biZoomMultiplePages).BeginGroup = false;
			}
			if (((BaseItem)biZoomPageWidth).BeginGroup)
			{
				((BaseItem)biZoomPageWidth).BeginGroup = false;
			}
		}
		if (!((BaseItem)biFind).Visible)
		{
			((BaseItem)biEditor).BeginGroup = true;
		}
		else if (((BaseItem)biEditor).BeginGroup)
		{
			((BaseItem)biEditor).BeginGroup = false;
		}
	}

	internal void ResetShortcuts()
	{
		((CollectionBase)(object)((BaseItem)biOpen).Shortcuts).Clear();
	}

	internal void InitShortcuts()
	{
		((CollectionBase)(object)((BaseItem)biOpen).Shortcuts).Clear();
		((BaseItem)biOpen).Shortcuts.Add((eShortcut)131151);
	}

	private void UpdateSize(RibbonBar bar)
	{
		((Control)bar).Width = bar.GetAutoSizeWidth();
		((ItemControl)bar).RecalcLayout();
		((Control)bar).Invalidate();
	}

	private string GetExportServiceText(StiExportService service)
	{
		if (service == null)
		{
			return string.Empty;
		}
		if (service is StiImageExportService)
		{
			return StiLocalization.Get("Export", "ExportTypeImageFile");
		}
		if (service is StiDataExportService)
		{
			return StiLocalization.Get("Export", "ExportTypeDataFile");
		}
		return service.ExportNameInMenu;
	}

	public void Localize()
	{
		rvControl.SuperToolTip.RemoveAllSuperTooltip();
		try
		{
			((BaseItem)biPrint).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Print"));
			((BaseItem)biOpen).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((BaseItem)biSave).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Save"));
			((BaseItem)biSaveDocumentFile).Text = StiLocalization.Get("FormViewer", "DocumentFile").Replace("...", "");
			((BaseItem)biSendEMail).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "SendEMail"));
			((BaseItem)biSendEMailDocumentFile).Text = StiLocalization.Get("FormViewer", "DocumentFile").Replace("...", "");
			((BaseItem)biPageSize).Text = StiLocalization.Get("FormViewer", "PageSize").Replace("...", "");
			((BaseItem)biPageNew).Text = StiLocalization.Get("FormViewer", "PageNew");
			((BaseItem)biPageDelete).Text = StiLocalization.Get("FormViewer", "PageDelete");
			((BaseItem)biPageDesign).Text = StiLocalization.Get("FormViewer", "PageDesign").Replace("...", "");
			((BaseItem)biFind).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Find"));
			((BaseItem)biEditor).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Editor"));
			((BaseItem)biBookmarks).Text = StiLocalization.Get("FormViewer", "Bookmarks");
			((BaseItem)biThumbs).Text = StiLocalization.Get("FormViewer", "Thumbnails");
			((BaseItem)biParameters).Text = StiLocalization.Get("FormViewer", "Parameters");
			((BaseItem)biResources).Text = StiLocalization.Get("PropertyMain", "Resources");
			((BaseItem)biClose).Text = StiLocalization.Get("FormViewer", "Close");
			((BaseItem)biFullScreen).Text = StiLocalization.Get("FormViewer", "FullScreen");
			((BaseItem)biDotMatrix).Text = StiLocalization.Get("Export", "DotMatrixMode");
			((BaseItem)biZoomPageWidth).Text = StiLocalization.Get("FormViewer", "ZoomPageWidth");
			((BaseItem)biZoomOnePage).Text = StiLocalization.Get("FormViewer", "ZoomOnePage");
			((BaseItem)biZoomTwoPages).Text = StiLocalization.Get("FormViewer", "ZoomTwoPages");
			((BaseItem)biZoomMultiplePages).Text = StiLocalization.Get("FormViewer", "PageViewModeMultiplePages");
			((Control)ribbonBarFile).Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFile"));
			((Control)ribbonBarEdit).Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEdit"));
			((Control)ribbonBarViewing).Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuView"));
			((Control)ribbonBarTools).Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuTools"));
			((Control)ribbonBarPanels).Text = StiHint.CreateHint(StiLocalization.Get("Services", "categoryPanels"));
			((BaseItem)rvControl.miCollapseAll).Text = StiLocalization.Get("FormViewer", "CollapseAll");
			((BaseItem)rvControl.miExpandAll).Text = StiLocalization.Get("FormViewer", "ExpandAll");
			LocalizeToolTip();
			UpdateSize(ribbonBarPanels);
			UpdateSize(ribbonBarTools);
			UpdateSize(ribbonBarFile);
			UpdateSize(ribbonBarEdit);
			UpdateSize(ribbonBarViewing);
			UpdateSize(ribbonBarClose);
			UpdateLocalizeExportButtons();
		}
		catch
		{
		}
	}

	private void UpdateLocalizeExportButtons()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		foreach (ButtonItem item in (CollectionBase)(object)((BaseItem)biSave).SubItems)
		{
			ButtonItem val = item;
			StiExportService service = ((BaseItem)val).Tag as StiExportService;
			string exportServiceText = GetExportServiceText(service);
			if (!string.IsNullOrEmpty(exportServiceText))
			{
				((BaseItem)val).Text = exportServiceText;
			}
		}
		foreach (ButtonItem item2 in (CollectionBase)(object)((BaseItem)biSendEMail).SubItems)
		{
			ButtonItem val2 = item2;
			StiExportService service2 = ((BaseItem)val2).Tag as StiExportService;
			string exportServiceText2 = GetExportServiceText(service2);
			if (!string.IsNullOrEmpty(exportServiceText2))
			{
				((BaseItem)val2).Text = exportServiceText2;
			}
		}
	}

	private void LocalizeToolTip()
	{
		if (StiOptions.Viewer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			string url = "user-manual/index.html?viewer_reports.htm#toolbar";
			SetToolTip((BaseItem)(object)biPrint, globalMeasureGraphics, ((BaseItem)biPrint).Text, "Print", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biOpen, globalMeasureGraphics, ((BaseItem)biOpen).Text, "Open", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biSave, globalMeasureGraphics, ((BaseItem)biSave).Text, "Save", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biClose, globalMeasureGraphics, ((BaseItem)biClose).Text, "Close", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biSendEMail, globalMeasureGraphics, ((BaseItem)biSendEMail).Text, "SendEMail", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biPageNew, globalMeasureGraphics, ((BaseItem)biPageNew).Text, "PageNew", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biPageDelete, globalMeasureGraphics, ((BaseItem)biPageDelete).Text, "PageDelete", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biPageDesign, globalMeasureGraphics, ((BaseItem)biPageDesign).Text, "PageDesign", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biPageSize, globalMeasureGraphics, ((BaseItem)biPageSize).Text, "PageSize", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biZoomPageWidth, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomPageWidth")), "ZoomPageWidth", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biZoomOnePage, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomOnePage")), "ZoomOnePage", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biZoomTwoPages, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomTwoPages")), "ZoomTwoPages", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biZoomMultiplePages, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomMultiplePages")), "ZoomMultiplePages", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biFullScreen, globalMeasureGraphics, ((BaseItem)biFullScreen).Text, "FullScreen", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biFind, globalMeasureGraphics, ((BaseItem)biFind).Text, "Find", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biBookmarks, globalMeasureGraphics, ((BaseItem)biBookmarks).Text, "Bookmarks", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biThumbs, globalMeasureGraphics, ((BaseItem)biThumbs).Text, "Thumbnails", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biParameters, globalMeasureGraphics, ((BaseItem)biParameters).Text, "Parameters", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biResources, globalMeasureGraphics, ((BaseItem)biResources).Text, "Resources", imageBody: false, headerVisible: true, url);
			SetToolTip((BaseItem)(object)biDotMatrix, globalMeasureGraphics, ((BaseItem)biDotMatrix).Text, "DotMatrixMode", imageBody: false, headerVisible: true, url);
		}
		else
		{
			((BaseItem)biPrint).Tooltip = ((BaseItem)biPrint).Text;
			((BaseItem)biOpen).Tooltip = ((BaseItem)biOpen).Text;
			((BaseItem)biSave).Tooltip = ((BaseItem)biSave).Text;
			((BaseItem)biSendEMail).Tooltip = ((BaseItem)biSendEMail).Text;
			((BaseItem)biPageNew).Tooltip = ((BaseItem)biPageNew).Text;
			((BaseItem)biPageDelete).Tooltip = ((BaseItem)biPageDelete).Text;
			((BaseItem)biPageDesign).Tooltip = ((BaseItem)biPageDesign).Text;
			((BaseItem)biPageSize).Tooltip = ((BaseItem)biPageSize).Text;
			((BaseItem)biBookmarks).Tooltip = ((BaseItem)biBookmarks).Text;
			((BaseItem)biThumbs).Tooltip = ((BaseItem)biThumbs).Text;
			((BaseItem)biParameters).Tooltip = ((BaseItem)biParameters).Text;
			((BaseItem)biResources).Tooltip = ((BaseItem)biResources).Text;
			((BaseItem)biFind).Tooltip = ((BaseItem)biFind).Text;
			((BaseItem)biEditor).Tooltip = ((BaseItem)biEditor).Text;
			((BaseItem)biFullScreen).Tooltip = ((BaseItem)biFullScreen).Text;
			((BaseItem)biZoomOnePage).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomOnePage"));
			((BaseItem)biZoomTwoPages).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomTwoPages"));
			((BaseItem)biZoomMultiplePages).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomMultiplePages"));
			((BaseItem)biZoomPageWidth).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomPageWidth"));
			((BaseItem)biDotMatrix).Tooltip = ((BaseItem)biDotMatrix).Text;
			((BaseItem)biClose).Tooltip = ((BaseItem)biClose).Text;
		}
	}

	private void SetToolTip(BaseItem item, Graphics g, string header, string localizeName, bool imageBody, bool headerVisible, string url)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		if (item != null)
		{
			header = header.Replace("...", string.Empty).Replace("&", string.Empty);
			string text = StiLocalization.Get("HelpDesigner", "PressF1");
			string text2 = StiLocalization.Get("HelpViewer", localizeName);
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Help" + StiScale.StepName + ".png");
			SuperTooltipInfo val = new SuperTooltipInfo(header, text, text2, (Image)null, (Image)(object)image, rvControl.CurrentTooltipColor);
			val.HeaderVisible = headerVisible;
			val.Hyperlink = url;
			SizeF sizeF = rvControl.SuperToolTip.MeasureHeaderText(g, header);
			SizeF sizeF2 = rvControl.SuperToolTip.MeasureBodyText(g, text2, StiScale.I(280));
			SizeF sizeF3 = rvControl.SuperToolTip.MeasureFooterText(g, text);
			sizeF3.Width += StiScale.I(40);
			int num = ((sizeF.Width > sizeF2.Width) ? ((int)Math.Round((double)sizeF.Width + 0.5) + StiScale.I(15)) : ((int)Math.Round((double)sizeF2.Width + 0.5) + StiScale.I(25)));
			if (sizeF3.Width > (float)num)
			{
				num = (int)sizeF3.Width;
			}
			val.CustomSize = new Size(num, (int)Math.Round(sizeF2.Height + sizeF.Height) + StiScale.I(42));
			rvControl.SuperToolTip.SetSuperTooltip((IComponent)item, val);
		}
	}

	public StiRibbonViewerUserControl(StiRibbonViewerControl ribbonViewerControl)
	{
		rvControl = ribbonViewerControl;
		ribbonViewerControl.userRibbon = this;
		InitializeComponent();
		DoScaling();
		Localize();
		UpdateButton();
		BuildExportMenu();
		InitEventsButton();
		UpdateControlsState();
		((Control)rvControl).Refresh();
		rvControl.RefreshControlState += delegate
		{
			UpdateControlsState();
		};
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Expected O, but got Unknown
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Expected O, but got Unknown
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5b: Expected O, but got Unknown
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac4: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Expected O, but got Unknown
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cde: Expected O, but got Unknown
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Expected O, but got Unknown
		//IL_0da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dad: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e70: Expected O, but got Unknown
		//IL_0f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9d: Expected O, but got Unknown
		//IL_1009: Unknown result type (might be due to invalid IL or missing references)
		//IL_1013: Expected O, but got Unknown
		//IL_1192: Unknown result type (might be due to invalid IL or missing references)
		//IL_119c: Expected O, but got Unknown
		//IL_122c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1236: Expected O, but got Unknown
		//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d0: Expected O, but got Unknown
		//IL_1354: Unknown result type (might be due to invalid IL or missing references)
		//IL_135e: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonViewerUserControl));
		ribbonBarViewing = new RibbonBar();
		itemContainer4 = new ItemContainer();
		biZoomOnePage = new ButtonItem();
		biZoomTwoPages = new ButtonItem();
		biZoomMultiplePages = new ButtonItem();
		cMultiPagesUserControl = new ControlContainerItem();
		itemContainer3 = new ItemContainer();
		biZoomPageWidth = new ButtonItem();
		itemContainer5 = new ItemContainer();
		biFullScreen = new ButtonItem();
		itemContainer6 = new ItemContainer();
		biDotMatrix = new ButtonItem();
		biClose = new ButtonItem();
		ribbonBarClose = new RibbonBar();
		ribbonBarEdit = new RibbonBar();
		biPageSize = new ButtonItem();
		itemContainer2 = new ItemContainer();
		biPageNew = new ButtonItem();
		biPageDelete = new ButtonItem();
		biPageDesign = new ButtonItem();
		ribbonBarFile = new RibbonBar();
		biPrint = new ButtonItem();
		itemContainer1 = new ItemContainer();
		biOpen = new ButtonItem();
		biSave = new ButtonItem();
		biSaveDocumentFile = new ButtonItem();
		biSendEMail = new ButtonItem();
		biSendEMailDocumentFile = new ButtonItem();
		ribbonBarTools = new RibbonBar();
		biFind = new ButtonItem();
		biEditor = new ButtonItem();
		ribbonBarPanels = new RibbonBar();
		biBookmarks = new ButtonItem();
		biParameters = new ButtonItem();
		biResources = new ButtonItem();
		biThumbs = new ButtonItem();
		((Control)this).SuspendLayout();
		ribbonBarViewing.AutoOverflowEnabled = true;
		ribbonBarViewing.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarViewing).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarViewing).ContainerControlProcessDialogKey = true;
		ribbonBarViewing.Items.AddRange((BaseItem[])(object)new BaseItem[4]
		{
			(BaseItem)itemContainer4,
			(BaseItem)itemContainer3,
			(BaseItem)itemContainer5,
			(BaseItem)itemContainer6
		});
		((Control)ribbonBarViewing).Location = new Point(655, 3);
		((Control)ribbonBarViewing).Name = "ribbonBarViewing";
		((Control)ribbonBarViewing).Size = new Size(295, 93);
		ribbonBarViewing.Style = (eDotNetBarStyle)8;
		((Control)ribbonBarViewing).TabIndex = 3;
		((Control)ribbonBarViewing).Text = "Preview";
		ribbonBarViewing.TitleStyle.CornerType = (eCornerType)1;
		ribbonBarViewing.TitleStyleMouseOver.CornerType = (eCornerType)1;
		itemContainer4.BackgroundStyle.CornerType = (eCornerType)1;
		itemContainer4.LayoutOrientation = (eOrientation)1;
		((BaseItem)itemContainer4).Name = "itemContainer4";
		((BaseItem)itemContainer4).SubItems.AddRange((BaseItem[])(object)new BaseItem[3]
		{
			(BaseItem)biZoomOnePage,
			(BaseItem)biZoomTwoPages,
			(BaseItem)biZoomMultiplePages
		});
		itemContainer4.TitleStyle.CornerType = (eCornerType)1;
		biZoomOnePage.ButtonStyle = (eButtonStyle)2;
		biZoomOnePage.Image = (Image)componentResourceManager.GetObject("biZoomOnePage.Image");
		((BaseItem)biZoomOnePage).Name = "biZoomOnePage";
		((BaseItem)biZoomOnePage).Shortcuts.Add((eShortcut)114);
		((BaseItem)biZoomOnePage).Text = "One Page";
		biZoomTwoPages.ButtonStyle = (eButtonStyle)2;
		biZoomTwoPages.Image = (Image)componentResourceManager.GetObject("biZoomTwoPages.Image");
		((BaseItem)biZoomTwoPages).Name = "biZoomTwoPages";
		((BaseItem)biZoomTwoPages).Shortcuts.Add((eShortcut)115);
		((BaseItem)biZoomTwoPages).Text = "Two Pages";
		biZoomMultiplePages.AutoExpandOnClick = true;
		biZoomMultiplePages.ButtonStyle = (eButtonStyle)2;
		biZoomMultiplePages.Image = (Image)componentResourceManager.GetObject("biZoomMultiplePages.Image");
		((BaseItem)biZoomMultiplePages).Name = "biZoomMultiplePages";
		((PopupItem)biZoomMultiplePages).PopupType = (ePopupType)1;
		biZoomMultiplePages.SplitButton = true;
		((BaseItem)biZoomMultiplePages).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)cMultiPagesUserControl });
		((BaseItem)biZoomMultiplePages).Text = "Multiple Pages";
		cMultiPagesUserControl.AllowItemResize = true;
		cMultiPagesUserControl.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)cMultiPagesUserControl).Name = "cMultiPagesUserControl";
		((BaseItem)cMultiPagesUserControl).Text = "controlContainerItem1";
		itemContainer3.BackgroundStyle.CornerType = (eCornerType)1;
		((BaseItem)itemContainer3).Name = "itemContainer3";
		((BaseItem)itemContainer3).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)biZoomPageWidth });
		itemContainer3.TitleStyle.CornerType = (eCornerType)1;
		biZoomPageWidth.ButtonStyle = (eButtonStyle)2;
		biZoomPageWidth.Image = (Image)componentResourceManager.GetObject("biZoomPageWidth.Image");
		biZoomPageWidth.ImagePosition = (eImagePosition)2;
		((BaseItem)biZoomPageWidth).Name = "biZoomPageWidth";
		((BaseItem)biZoomPageWidth).Shortcuts.Add((eShortcut)116);
		((BaseItem)biZoomPageWidth).Text = "Page Width";
		itemContainer5.BackgroundStyle.CornerType = (eCornerType)1;
		itemContainer5.LayoutOrientation = (eOrientation)1;
		((BaseItem)itemContainer5).Name = "itemContainer5";
		((BaseItem)itemContainer5).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)biFullScreen });
		itemContainer5.TitleStyle.CornerType = (eCornerType)1;
		biFullScreen.ButtonStyle = (eButtonStyle)2;
		biFullScreen.Image = (Image)componentResourceManager.GetObject("biFullScreen.Image");
		biFullScreen.ImagePosition = (eImagePosition)2;
		((BaseItem)biFullScreen).Name = "biFullScreen";
		((BaseItem)biFullScreen).Shortcuts.Add((eShortcut)113);
		((BaseItem)biFullScreen).Text = "Full Screen";
		itemContainer6.BackgroundStyle.CornerType = (eCornerType)1;
		((BaseItem)itemContainer6).Name = "itemContainer6";
		((BaseItem)itemContainer6).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)biDotMatrix });
		itemContainer6.TitleStyle.CornerType = (eCornerType)1;
		((BaseItem)biDotMatrix).BeginGroup = true;
		biDotMatrix.ButtonStyle = (eButtonStyle)2;
		biDotMatrix.Image = (Image)componentResourceManager.GetObject("biDotMatrix.Image");
		biDotMatrix.ImagePosition = (eImagePosition)2;
		((BaseItem)biDotMatrix).Name = "biDotMatrix";
		((BaseItem)biDotMatrix).Text = "DotMatrix Mode";
		((BaseItem)biClose).BeginGroup = true;
		biClose.ButtonStyle = (eButtonStyle)2;
		biClose.Image = (Image)componentResourceManager.GetObject("biClose.Image");
		biClose.ImagePaddingHorizontal = 60;
		biClose.ImagePosition = (eImagePosition)2;
		((BaseItem)biClose).ItemAlignment = (eItemAlignment)1;
		((BaseItem)biClose).Name = "biClose";
		biClose.RibbonWordWrap = false;
		((BaseItem)biClose).Stretch = true;
		biClose.SubItemsExpandWidth = 14;
		((BaseItem)biClose).Text = "Close Preview";
		ribbonBarClose.AutoOverflowEnabled = true;
		ribbonBarClose.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarClose).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarClose).ContainerControlProcessDialogKey = true;
		ribbonBarClose.Items.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)biClose });
		((Control)ribbonBarClose).Location = new Point(956, 3);
		((Control)ribbonBarClose).Name = "ribbonBarClose";
		((Control)ribbonBarClose).Size = new Size(110, 93);
		ribbonBarClose.Style = (eDotNetBarStyle)8;
		((Control)ribbonBarClose).TabIndex = 4;
		ribbonBarClose.TitleStyle.CornerType = (eCornerType)1;
		ribbonBarClose.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBarEdit.AutoOverflowEnabled = true;
		ribbonBarEdit.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarEdit).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarEdit).ContainerControlProcessDialogKey = true;
		ribbonBarEdit.Items.AddRange((BaseItem[])(object)new BaseItem[2]
		{
			(BaseItem)biPageSize,
			(BaseItem)itemContainer2
		});
		((Control)ribbonBarEdit).Location = new Point(164, 2);
		((Control)ribbonBarEdit).Name = "ribbonBarEdit";
		((Control)ribbonBarEdit).Size = new Size(148, 93);
		ribbonBarEdit.Style = (eDotNetBarStyle)8;
		((Control)ribbonBarEdit).TabIndex = 2;
		((Control)ribbonBarEdit).Text = "Edit";
		ribbonBarEdit.TitleStyle.CornerType = (eCornerType)1;
		ribbonBarEdit.TitleStyleMouseOver.CornerType = (eCornerType)1;
		biPageSize.ButtonStyle = (eButtonStyle)2;
		biPageSize.Image = (Image)componentResourceManager.GetObject("biPageSize.Image");
		biPageSize.ImagePosition = (eImagePosition)2;
		((BaseItem)biPageSize).Name = "biPageSize";
		((BaseItem)biPageSize).Shortcuts.Add((eShortcut)196691);
		((BaseItem)biPageSize).Text = "Page Size";
		itemContainer2.BackgroundStyle.CornerType = (eCornerType)1;
		itemContainer2.LayoutOrientation = (eOrientation)1;
		((BaseItem)itemContainer2).Name = "itemContainer2";
		((BaseItem)itemContainer2).SubItems.AddRange((BaseItem[])(object)new BaseItem[3]
		{
			(BaseItem)biPageNew,
			(BaseItem)biPageDelete,
			(BaseItem)biPageDesign
		});
		itemContainer2.TitleStyle.CornerType = (eCornerType)1;
		biPageNew.ButtonStyle = (eButtonStyle)2;
		biPageNew.Image = (Image)componentResourceManager.GetObject("biPageNew.Image");
		((BaseItem)biPageNew).Name = "biPageNew";
		((BaseItem)biPageNew).Shortcuts.Add((eShortcut)196686);
		((BaseItem)biPageNew).Text = "New Page";
		biPageDelete.ButtonStyle = (eButtonStyle)2;
		biPageDelete.Image = (Image)componentResourceManager.GetObject("biPageDelete.Image");
		((BaseItem)biPageDelete).Name = "biPageDelete";
		((BaseItem)biPageDelete).Shortcuts.Add((eShortcut)196676);
		((BaseItem)biPageDelete).Text = "Delete Page";
		biPageDesign.ButtonStyle = (eButtonStyle)2;
		((BaseItem)biPageDesign).CanCustomize = false;
		biPageDesign.Image = (Image)componentResourceManager.GetObject("biPageDesign.Image");
		((BaseItem)biPageDesign).Name = "biPageDesign";
		((BaseItem)biPageDesign).Shortcuts.Add((eShortcut)196677);
		((BaseItem)biPageDesign).Text = "Edit Page";
		ribbonBarFile.AutoOverflowEnabled = true;
		ribbonBarFile.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarFile).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarFile).ContainerControlProcessDialogKey = true;
		ribbonBarFile.Items.AddRange((BaseItem[])(object)new BaseItem[2]
		{
			(BaseItem)biPrint,
			(BaseItem)itemContainer1
		});
		((Control)ribbonBarFile).Location = new Point(2, 2);
		((Control)ribbonBarFile).Name = "ribbonBarFile";
		((Control)ribbonBarFile).Size = new Size(156, 93);
		ribbonBarFile.Style = (eDotNetBarStyle)8;
		((Control)ribbonBarFile).TabIndex = 1;
		((Control)ribbonBarFile).Text = "File";
		ribbonBarFile.TitleStyle.CornerType = (eCornerType)1;
		ribbonBarFile.TitleStyleMouseOver.CornerType = (eCornerType)1;
		biPrint.ButtonStyle = (eButtonStyle)2;
		biPrint.Image = (Image)componentResourceManager.GetObject("biPrint.Image");
		biPrint.ImagePosition = (eImagePosition)2;
		((BaseItem)biPrint).Name = "biPrint";
		((BaseItem)biPrint).Shortcuts.Add((eShortcut)131152);
		((BaseItem)biPrint).Text = "Print";
		itemContainer1.BackgroundStyle.CornerType = (eCornerType)1;
		itemContainer1.LayoutOrientation = (eOrientation)1;
		((BaseItem)itemContainer1).Name = "itemContainer1";
		((BaseItem)itemContainer1).SubItems.AddRange((BaseItem[])(object)new BaseItem[3]
		{
			(BaseItem)biOpen,
			(BaseItem)biSave,
			(BaseItem)biSendEMail
		});
		itemContainer1.TitleStyle.CornerType = (eCornerType)1;
		biOpen.ButtonStyle = (eButtonStyle)2;
		biOpen.Image = (Image)componentResourceManager.GetObject("biOpen.Image");
		((BaseItem)biOpen).Name = "biOpen";
		((BaseItem)biOpen).Shortcuts.Add((eShortcut)131151);
		((BaseItem)biOpen).Stretch = true;
		((BaseItem)biOpen).Text = "Open";
		biSave.AutoExpandOnClick = true;
		biSave.ButtonStyle = (eButtonStyle)2;
		biSave.Image = (Image)componentResourceManager.GetObject("biSave.Image");
		((BaseItem)biSave).Name = "biSave";
		((BaseItem)biSave).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)biSaveDocumentFile });
		((BaseItem)biSave).Text = "Save";
		biSaveDocumentFile.Image = (Image)componentResourceManager.GetObject("biSaveDocumentFile.Image");
		((BaseItem)biSaveDocumentFile).Name = "biSaveDocumentFile";
		((BaseItem)biSaveDocumentFile).Shortcuts.Add((eShortcut)131155);
		((BaseItem)biSaveDocumentFile).Text = "Document File...";
		biSendEMail.AutoExpandOnClick = true;
		biSendEMail.ButtonStyle = (eButtonStyle)2;
		biSendEMail.Image = (Image)componentResourceManager.GetObject("biSendEMail.Image");
		((BaseItem)biSendEMail).Name = "biSendEMail";
		((BaseItem)biSendEMail).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)biSendEMailDocumentFile });
		((BaseItem)biSendEMail).Text = "Send EMail";
		biSendEMailDocumentFile.Image = (Image)componentResourceManager.GetObject("biSendEMailDocumentFile.Image");
		((BaseItem)biSendEMailDocumentFile).Name = "biSendEMailDocumentFile";
		((BaseItem)biSendEMailDocumentFile).Text = "Document File...";
		ribbonBarTools.AutoOverflowEnabled = true;
		ribbonBarTools.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarTools).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarTools).ContainerControlProcessDialogKey = true;
		ribbonBarTools.Items.AddRange((BaseItem[])(object)new BaseItem[2]
		{
			(BaseItem)biFind,
			(BaseItem)biEditor
		});
		((Control)ribbonBarTools).Location = new Point(545, 3);
		((Control)ribbonBarTools).Name = "ribbonBarTools";
		((Control)ribbonBarTools).Size = new Size(104, 93);
		ribbonBarTools.Style = (eDotNetBarStyle)8;
		((Control)ribbonBarTools).TabIndex = 5;
		((Control)ribbonBarTools).Text = "Tools";
		ribbonBarTools.TitleStyle.CornerType = (eCornerType)1;
		ribbonBarTools.TitleStyleMouseOver.CornerType = (eCornerType)1;
		biFind.ButtonStyle = (eButtonStyle)2;
		biFind.Image = (Image)componentResourceManager.GetObject("biFind.Image");
		biFind.ImagePosition = (eImagePosition)2;
		((BaseItem)biFind).Name = "biFind";
		((BaseItem)biFind).Shortcuts.Add((eShortcut)131142);
		biFind.SubItemsExpandWidth = 14;
		((BaseItem)biFind).Text = "Find";
		biEditor.ButtonStyle = (eButtonStyle)2;
		biEditor.Image = (Image)componentResourceManager.GetObject("biEditor.Image");
		biEditor.ImagePosition = (eImagePosition)2;
		((BaseItem)biEditor).Name = "biEditor";
		((BaseItem)biEditor).Shortcuts.Add((eShortcut)131141);
		biEditor.SubItemsExpandWidth = 14;
		((BaseItem)biEditor).Text = "Editor";
		ribbonBarPanels.AutoOverflowEnabled = true;
		ribbonBarPanels.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarPanels).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBarPanels).ContainerControlProcessDialogKey = true;
		ribbonBarPanels.Items.AddRange((BaseItem[])(object)new BaseItem[4]
		{
			(BaseItem)biBookmarks,
			(BaseItem)biParameters,
			(BaseItem)biResources,
			(BaseItem)biThumbs
		});
		((Control)ribbonBarPanels).Location = new Point(318, 2);
		((Control)ribbonBarPanels).Name = "ribbonBarPanels";
		((Control)ribbonBarPanels).Size = new Size(221, 93);
		ribbonBarPanels.Style = (eDotNetBarStyle)8;
		((Control)ribbonBarPanels).TabIndex = 6;
		((Control)ribbonBarPanels).Text = "Panels";
		ribbonBarPanels.TitleStyle.CornerType = (eCornerType)1;
		ribbonBarPanels.TitleStyleMouseOver.CornerType = (eCornerType)1;
		biBookmarks.AutoCheckOnClick = true;
		((BaseItem)biBookmarks).AutoCollapseOnClick = false;
		biBookmarks.ButtonStyle = (eButtonStyle)2;
		biBookmarks.Image = (Image)componentResourceManager.GetObject("biBookmarks.Image");
		biBookmarks.ImagePosition = (eImagePosition)2;
		((BaseItem)biBookmarks).Name = "biBookmarks";
		((BaseItem)biBookmarks).Shortcuts.Add((eShortcut)131138);
		biBookmarks.SubItemsExpandWidth = 14;
		((BaseItem)biBookmarks).Text = "Bookmarks";
		biParameters.AutoCheckOnClick = true;
		((BaseItem)biParameters).AutoCollapseOnClick = false;
		biParameters.ButtonStyle = (eButtonStyle)2;
		((BaseItem)biParameters).Enabled = false;
		biParameters.Image = (Image)componentResourceManager.GetObject("biParameters.Image");
		biParameters.ImagePosition = (eImagePosition)2;
		((BaseItem)biParameters).Name = "biParameters";
		((BaseItem)biParameters).Shortcuts.Add((eShortcut)196688);
		biParameters.SubItemsExpandWidth = 14;
		((BaseItem)biParameters).Text = "Parameters";
		biResources.AutoCheckOnClick = true;
		((BaseItem)biResources).AutoCollapseOnClick = false;
		biResources.ButtonStyle = (eButtonStyle)2;
		((BaseItem)biResources).Enabled = false;
		biResources.Image = (Image)componentResourceManager.GetObject("biResources.Image");
		biResources.ImagePosition = (eImagePosition)2;
		((BaseItem)biResources).Name = "biResources";
		((BaseItem)biResources).Shortcuts.Add((eShortcut)131154);
		biResources.SubItemsExpandWidth = 14;
		((BaseItem)biResources).Text = "Resources";
		biThumbs.AutoCheckOnClick = true;
		((BaseItem)biThumbs).AutoCollapseOnClick = false;
		biThumbs.ButtonStyle = (eButtonStyle)2;
		biThumbs.Image = (Image)componentResourceManager.GetObject("biThumbs.Image");
		biThumbs.ImagePosition = (eImagePosition)2;
		((BaseItem)biThumbs).Name = "biThumbs";
		((BaseItem)biThumbs).Shortcuts.Add((eShortcut)131156);
		biThumbs.SubItemsExpandWidth = 14;
		((BaseItem)biThumbs).Text = "Thumbnails";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Gainsboro;
		((Control)this).Controls.Add((Control)(object)ribbonBarFile);
		((Control)this).Controls.Add((Control)(object)ribbonBarPanels);
		((Control)this).Controls.Add((Control)(object)ribbonBarTools);
		((Control)this).Controls.Add((Control)(object)ribbonBarEdit);
		((Control)this).Controls.Add((Control)(object)ribbonBarViewing);
		((Control)this).Controls.Add((Control)(object)ribbonBarClose);
		((Control)this).Name = "StiRibbonViewerUserControl";
		((Control)this).Size = new Size(1138, 331);
		((Control)this).ResumeLayout(false);
	}
}
