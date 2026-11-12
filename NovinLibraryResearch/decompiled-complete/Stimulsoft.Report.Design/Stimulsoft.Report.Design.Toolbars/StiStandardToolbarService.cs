using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dialogs;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiStandardToolbarService : StiToolbarService
{
	private ButtonItem tbReportNew;

	private ButtonItem tbReportOpen;

	private ButtonItem tbReportSave;

	private ButtonItem tbReportPreview;

	private ButtonItem tbPageNew;

	private ButtonItem tbPageDashdoardNew;

	private ButtonItem tbFormNew;

	private ButtonItem tbScreenNew;

	private ButtonItem tbPageDelete;

	private ButtonItem tbPageSetup;

	private ButtonItem tbUndo;

	private ButtonItem tbRedo;

	private ButtonItem tbCut;

	private ButtonItem tbCopy;

	private ButtonItem tbPaste;

	private ButtonItem tbDelete;

	private ButtonItem tbSelectAll;

	private ButtonItem tbShowGrid;

	private ButtonItem tbShowAlignToGrid;

	private ButtonItem tbShowHeaders;

	private ButtonItem tbShowRulers;

	private ButtonItem tbShowOrder;

	private ButtonItem tbQuickInfo;

	private ButtonItem itemNone;

	private ButtonItem itemShowComponentsNames;

	private ButtonItem itemShowAliases;

	private ButtonItem itemShowContent;

	private ButtonItem itemShowFields;

	private ButtonItem itemShowFieldsOnly;

	private ButtonItem itemShowEvents;

	private ButtonItem itemOverlay;

	private ComboBoxItem cbZoom;

	private bool locked;

	public override string ServiceName => StiLocalization.Get("Toolbars", "ToolbarStandard");

	public override int Position => 1;

	public bool CutButtonEnabled
	{
		get
		{
			return tbCut.Enabled;
		}
		set
		{
			tbCut.Enabled = value;
		}
	}

	public bool CopyButtonEnabled
	{
		get
		{
			return tbCopy.Enabled;
		}
		set
		{
			tbCopy.Enabled = value;
		}
	}

	public bool PasteButtonEnabled
	{
		get
		{
			return tbPaste.Enabled;
		}
		set
		{
			tbPaste.Enabled = value;
		}
	}

	public bool DeleteButtonEnabled
	{
		get
		{
			return tbDelete.Enabled;
		}
		set
		{
			tbDelete.Enabled = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowReportNew
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowReportNew;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowReportNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPageNew
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowPageNew;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowPageNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDashboardNew
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowDashboardNew;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowDashboardNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFormNew
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowFormNew;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowFormNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowScreenNew
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowScreenNew;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowScreenNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowReportOpen
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowReportOpen;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowReportOpen = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowReportSave
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowReportSave;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowReportSave = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPageDelete
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowPageDelete;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowPageDelete = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowReportPreview
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowReportPreview;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowReportPreview = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPageSetup
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowPageSetup;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowPageSetup = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowUndo
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowUndo;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowUndo = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowRedo
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowRedo;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowRedo = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCut
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowCut;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowCut = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCopy
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowCopy;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowCopy = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPaste
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowPaste;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowPaste = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDelete
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowDelete;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowDelete = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowSelectAll
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowSelectAll;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowSelectAll = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowShowGrid
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowShowGrid;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowShowGrid = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignToGrid
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowAlignToGrid;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowAlignToGrid = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowShowHeaders
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowShowHeaders;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowShowHeaders = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowShowRulers
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowShowRulers;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowShowRulers = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowShowOrder
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowShowOrder;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowShowOrder = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowQuickInfo
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowQuickInfo;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowQuickInfo = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowZoom
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowZoom;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowZoom = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("This property is obsolete!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowClose
	{
		get
		{
			return StiOptions.Designer.Toolbars.StandardToolbar.ShowClose;
		}
		set
		{
			StiOptions.Designer.Toolbars.StandardToolbar.ShowClose = value;
		}
	}

	private void Refresh(object sender, EventArgs e)
	{
		if (base.ToolBar.Visible)
		{
			locked = true;
			base.ToolBar.SuspendLayout();
			RefreshObject();
			RefreshReportOptions();
			if (tbPageDelete != null)
			{
				tbPageDelete.Enabled = Report != null && Report.Pages.Count > 1;
			}
			if (ShowPageSetup)
			{
				if (Report == null || Designer.Report.GetCurrentPage() is IStiForm)
				{
					tbPageSetup.Enabled = false;
				}
				else
				{
					tbPageSetup.Enabled = true;
				}
			}
			if (cbZoom != null)
			{
				if (Report != null)
				{
					cbZoom.Text = Math.Round(Report.Info.Zoom * 100.0, 0) + "%";
					cbZoom.Enabled = true;
				}
				else
				{
					cbZoom.Enabled = false;
				}
			}
			base.ToolBar.ResumeLayout();
			locked = false;
		}
		if (tbQuickInfo != null)
		{
			tbQuickInfo.Enabled = Report != null;
		}
		if (tbReportOpen != null)
		{
			tbReportOpen.Enabled = Report == null || !Report.IsPageDesigner;
		}
		if (tbReportSave != null)
		{
			tbReportSave.Enabled = Report != null && !Report.IsPageDesigner;
		}
		if (tbReportPreview != null)
		{
			tbReportPreview.Enabled = Report != null && !Report.IsPageDesigner;
		}
		if (tbPageNew != null)
		{
			tbPageNew.Enabled = Report != null && !Report.IsPageDesigner;
		}
		if (tbPageDashdoardNew != null)
		{
			tbPageDashdoardNew.Enabled = Report != null && !Report.IsPageDesigner;
		}
		if (tbFormNew != null)
		{
			tbFormNew.Enabled = Report != null && !Report.IsPageDesigner;
		}
		if (tbScreenNew != null)
		{
			tbScreenNew.Enabled = Report != null && !Report.IsPageDesigner;
		}
		if (tbReportNew != null)
		{
			tbReportNew.Enabled = Report == null || !Report.IsPageDesigner;
		}
	}

	public virtual void RefreshObject()
	{
		if (Designer.TabControl.SelectedTabIndex >= Designer.PageCodeIndex || Report == null)
		{
			if (tbCut != null && tbCut.Enabled)
			{
				tbCut.Enabled = false;
			}
			if (tbCopy != null && tbCopy.Enabled)
			{
				tbCopy.Enabled = false;
			}
			if (tbPaste != null && tbPaste.Enabled)
			{
				tbPaste.Enabled = false;
			}
			if (tbDelete != null && tbDelete.Enabled)
			{
				tbDelete.Enabled = false;
			}
			if (tbSelectAll != null && tbSelectAll.Enabled)
			{
				tbSelectAll.Enabled = false;
			}
		}
		else
		{
			if (StiClipboard.Get() != null)
			{
				if (tbPaste != null && !tbPaste.Enabled)
				{
					tbPaste.Enabled = true;
				}
			}
			else if (tbPaste != null && tbPaste.Enabled)
			{
				tbPaste.Enabled = false;
			}
			if (Designer.SelectedComponentsOnPage.Count > 0)
			{
				if (tbPaste != null && !tbCut.Enabled)
				{
					tbCut.Enabled = true;
				}
				if (tbCopy != null && !tbCopy.Enabled)
				{
					tbCopy.Enabled = true;
				}
				if (tbDelete != null && !tbDelete.Enabled)
				{
					tbDelete.Enabled = true;
				}
			}
			else
			{
				if (tbCut != null && tbCut.Enabled)
				{
					tbCut.Enabled = false;
				}
				if (tbCopy != null && tbCopy.Enabled)
				{
					tbCopy.Enabled = false;
				}
				if (tbDelete != null && tbDelete.Enabled)
				{
					tbDelete.Enabled = false;
				}
			}
			if (Designer.ComponentsOnPage.Count > 0)
			{
				if (tbSelectAll != null && !tbSelectAll.Enabled)
				{
					tbSelectAll.Enabled = true;
				}
			}
			else if (tbSelectAll != null && tbSelectAll.Enabled)
			{
				tbSelectAll.Enabled = false;
			}
		}
		if (Designer.TabControl.SelectedTabIndex >= Designer.PageCodeIndex)
		{
			if (tbUndo != null && tbUndo.Enabled)
			{
				tbUndo.Enabled = false;
			}
			if (tbRedo != null && tbRedo.Enabled)
			{
				tbRedo.Enabled = false;
			}
		}
		else if (Designer.UndoRedo != null)
		{
			if (tbUndo != null && tbUndo.Enabled != Designer.UndoRedo.CanUndo)
			{
				tbUndo.Enabled = Designer.UndoRedo.CanUndo;
			}
			if (tbRedo != null && tbRedo.Enabled != Designer.UndoRedo.CanRedo)
			{
				tbRedo.Enabled = Designer.UndoRedo.CanRedo;
			}
			if (!StiOptions.Designer.ShowSuperToolTip)
			{
				if (tbUndo != null && tbUndo.Enabled)
				{
					tbUndo.Tooltip = StiHint.CreateHint(string.Format(StiLocalization.Get("MainMenu", "menuEditUndoText"), Designer.UndoRedo.UndoText));
				}
				if (tbRedo != null && tbRedo.Enabled)
				{
					tbRedo.Tooltip = StiHint.CreateHint(string.Format(StiLocalization.Get("MainMenu", "menuEditRedoText"), Designer.UndoRedo.RedoText));
				}
			}
			else
			{
				Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
				if (tbUndo != null)
				{
					SetSuperToolTip(tbUndo, StiHint.CreateHint(string.Format(StiLocalization.Get("MainMenu", "menuEditUndoText"), Designer.UndoRedo.UndoText)), "Undo", globalMeasureGraphics);
				}
				if (tbRedo != null)
				{
					SetSuperToolTip(tbRedo, StiHint.CreateHint(string.Format(StiLocalization.Get("MainMenu", "menuEditRedoText"), Designer.UndoRedo.RedoText)), "Redo", globalMeasureGraphics);
				}
			}
		}
		((Control)Designer).Text = Designer.GetDesignerTitle();
	}

	public virtual void RefreshReportOptions()
	{
		if (Report != null)
		{
			if (tbShowGrid != null)
			{
				tbShowGrid.Checked = Report.Info.ShowGrid;
			}
			if (tbShowAlignToGrid != null)
			{
				tbShowAlignToGrid.Checked = Report.Info.AlignToGrid;
			}
			if (tbShowHeaders != null)
			{
				tbShowHeaders.Checked = Report.Info.ShowHeaders;
			}
			if (tbShowRulers != null)
			{
				tbShowRulers.Checked = Report.Info.ShowRulers;
			}
			if (tbShowOrder != null)
			{
				tbShowOrder.Checked = Report.Info.ShowOrder;
			}
			if (tbShowGrid != null)
			{
				tbShowGrid.Enabled = true;
			}
			if (tbShowAlignToGrid != null)
			{
				tbShowAlignToGrid.Enabled = true;
			}
			if (tbShowHeaders != null)
			{
				tbShowHeaders.Enabled = true;
			}
			if (tbShowRulers != null)
			{
				tbShowRulers.Enabled = true;
			}
			if (tbShowOrder != null)
			{
				tbShowOrder.Enabled = true;
			}
		}
		else
		{
			if (tbShowGrid != null)
			{
				tbShowGrid.Enabled = false;
			}
			if (tbShowAlignToGrid != null)
			{
				tbShowAlignToGrid.Enabled = false;
			}
			if (tbShowGrid != null)
			{
				tbShowHeaders.Enabled = false;
			}
			if (tbShowRulers != null)
			{
				tbShowRulers.Enabled = false;
			}
			if (tbShowOrder != null)
			{
				tbShowOrder.Enabled = false;
			}
		}
	}

	private void tbReportNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportNew(Designer);
	}

	private void tbPageNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageNew(Designer);
	}

	private void tbPageDashboardNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeDashboardNew(Designer);
	}

	private void tbFormNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeFormNew(Designer);
	}

	private void tbScreenNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeScreenNew(Designer);
	}

	private void tbReportOpen_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportOpen(Designer);
	}

	private void tbReportSave_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportSave(Designer);
	}

	private void tbReportSaveAs_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportSaveAs(Designer);
	}

	private void tbPageDelete_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageDelete(Designer);
	}

	private void tbReportPreview_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportStandardGuiPreview(Designer);
	}

	private void tbPageSetup_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageSetup(Designer);
	}

	private void tbReportSetup_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportSetup(Designer);
	}

	private void tbUndo_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeUndo(Designer);
	}

	private void tbRedo_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeRedo(Designer);
	}

	private void tbCut_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCut(Designer);
	}

	private void tbCopy_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCopy(Designer);
	}

	private void tbPaste_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokePaste(Designer);
	}

	private void tbDelete_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeDelete(Designer);
	}

	private void tbSelectAll_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeSelectAll(Designer);
	}

	private void tbShowGrid_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowGrid(Designer);
	}

	private void tbAlignToGrid_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeAlignToGrid(Designer);
	}

	private void tbShowHeaders_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowHeaders(Designer);
	}

	private void tbShowRulers_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowRulers(Designer);
	}

	private void tbShowOrder_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowOrder(Designer);
	}

	private void tbQuickInfo_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		itemNone.Checked = Report.Info.QuickInfoType == StiQuickInfoType.None;
		itemShowComponentsNames.Checked = Report.Info.QuickInfoType == StiQuickInfoType.ShowComponentsNames;
		itemShowAliases.Checked = Report.Info.QuickInfoType == StiQuickInfoType.ShowAliases;
		itemShowContent.Checked = Report.Info.QuickInfoType == StiQuickInfoType.ShowContent;
		itemShowFields.Checked = Report.Info.QuickInfoType == StiQuickInfoType.ShowFields;
		itemShowFieldsOnly.Checked = Report.Info.QuickInfoType == StiQuickInfoType.ShowFieldsOnly;
		itemShowEvents.Checked = Report.Info.QuickInfoType == StiQuickInfoType.ShowEvents;
		itemOverlay.Checked = Report.Info.QuickInfoOverlay;
	}

	private void ciQuickInfoNone_Click(object sender, EventArgs e)
	{
		itemNone.Checked = true;
		itemShowComponentsNames.Checked = false;
		itemShowAliases.Checked = false;
		itemShowContent.Checked = false;
		itemShowFields.Checked = false;
		itemShowFieldsOnly.Checked = false;
		itemShowEvents.Checked = false;
		Report.Info.QuickInfoType = StiQuickInfoType.None;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoShowComponentsNames_Click(object sender, EventArgs e)
	{
		itemNone.Checked = false;
		itemShowComponentsNames.Checked = true;
		itemShowAliases.Checked = false;
		itemShowContent.Checked = false;
		itemShowFields.Checked = false;
		itemShowFieldsOnly.Checked = false;
		itemShowEvents.Checked = false;
		Report.Info.QuickInfoType = StiQuickInfoType.ShowComponentsNames;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoShowAliases_Click(object sender, EventArgs e)
	{
		itemNone.Checked = false;
		itemShowComponentsNames.Checked = false;
		itemShowAliases.Checked = true;
		itemShowContent.Checked = false;
		itemShowFields.Checked = false;
		itemShowFieldsOnly.Checked = false;
		itemShowEvents.Checked = false;
		Report.Info.QuickInfoType = StiQuickInfoType.ShowAliases;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoShowContent_Click(object sender, EventArgs e)
	{
		itemNone.Checked = false;
		itemShowComponentsNames.Checked = false;
		itemShowAliases.Checked = false;
		itemShowContent.Checked = true;
		itemShowFields.Checked = false;
		itemShowFieldsOnly.Checked = false;
		itemShowEvents.Checked = false;
		Report.Info.QuickInfoType = StiQuickInfoType.ShowContent;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoShowFields_Click(object sender, EventArgs e)
	{
		itemNone.Checked = false;
		itemShowComponentsNames.Checked = false;
		itemShowAliases.Checked = false;
		itemShowContent.Checked = false;
		itemShowFields.Checked = true;
		itemShowFieldsOnly.Checked = false;
		itemShowEvents.Checked = false;
		Report.Info.QuickInfoType = StiQuickInfoType.ShowFields;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoShowFieldsOnly_Click(object sender, EventArgs e)
	{
		itemNone.Checked = false;
		itemShowComponentsNames.Checked = false;
		itemShowAliases.Checked = false;
		itemShowContent.Checked = false;
		itemShowFields.Checked = false;
		itemShowFieldsOnly.Checked = true;
		itemShowEvents.Checked = false;
		Report.Info.QuickInfoType = StiQuickInfoType.ShowFieldsOnly;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoOverlay_Click(object sender, EventArgs e)
	{
		Report.Info.QuickInfoOverlay = !Report.Info.QuickInfoOverlay;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoShowEvents_Click(object sender, EventArgs e)
	{
		itemNone.Checked = false;
		itemShowComponentsNames.Checked = false;
		itemShowAliases.Checked = false;
		itemShowContent.Checked = false;
		itemShowFields.Checked = false;
		itemShowFieldsOnly.Checked = false;
		itemShowEvents.Checked = true;
		Report.Info.QuickInfoType = StiQuickInfoType.ShowEvents;
		((Control)Designer).Refresh();
	}

	private void cbZoom_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			cbZoomSet(cbZoom.Text);
			e.Handled = true;
		}
	}

	private void cbZoom_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			cbZoomSet(cbZoom.Text);
		}
	}

	private void cbZoomSet(string textZoom)
	{
		if (textZoom == StiLocalization.Get("Zoom", "PageWidth"))
		{
			StiPageViewControl currentPageView = Designer.PageView.GetCurrentPageView();
			if (currentPageView != null)
			{
				double num = Report.GetCurrentPage().Unit.ConvertToHInches(Report.GetCurrentPage().DisplayRectangle.Width);
				int clientWidth = currentPageView.ClientWidth;
				Designer.SetZoom((float)Math.Round((double)(clientWidth - 20) / num, 2));
			}
		}
		else if (textZoom == StiLocalization.Get("Zoom", "PageHeight"))
		{
			StiPageViewControl currentPageView2 = Designer.PageView.GetCurrentPageView();
			if (currentPageView2 != null)
			{
				double num2 = Report.GetCurrentPage().Unit.ConvertToHInches(Report.GetCurrentPage().DisplayRectangle.Height);
				double num3 = currentPageView2.ClientHeight;
				Designer.SetZoom((float)Math.Round((num3 - 20.0) / num2, 2));
			}
		}
		else if (textZoom == StiLocalization.Get("Zoom", "OnePage"))
		{
			StiPageViewControl currentPageView3 = Designer.PageView.GetCurrentPageView();
			if (currentPageView3 != null)
			{
				double num4 = Report.GetCurrentPage().Unit.ConvertToHInches(Report.GetCurrentPage().DisplayRectangle.Width);
				double num5 = Report.GetCurrentPage().Unit.ConvertToHInches(Report.GetCurrentPage().DisplayRectangle.Height);
				int clientWidth2 = currentPageView3.ClientWidth;
				int clientHeight = currentPageView3.ClientHeight;
				double val = (double)(clientWidth2 - 20) / num4;
				double val2 = (double)(clientHeight - 20) / num5;
				Designer.SetZoom((float)Math.Round(Math.Min(val, val2), 2));
			}
		}
		else
		{
			try
			{
				if (textZoom[textZoom.Length - 1] == '%')
				{
					textZoom = textZoom.Substring(0, textZoom.Length - 1);
				}
				double num6 = double.Parse(textZoom.Substring(0, textZoom.Length)) / 100.0;
				if (num6 < 0.1)
				{
					num6 = 0.1;
				}
				if (num6 > 10.0)
				{
					num6 = 10.0;
				}
				Designer.SetZoom((float)num6);
			}
			catch
			{
			}
		}
		((Control)cbZoom.ComboBoxEx).Text = Report.Info.Zoom * 100.0 + "%";
		Designer.Builder.BuildReport();
	}

	private void LocalizeSuperToolTip()
	{
		if (Designer == null)
		{
			return;
		}
		if (StiOptions.Designer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			if (tbReportNew != null)
			{
				SetSuperToolTip(tbReportNew, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportNew")) + " (Ctrl + N)", "ReportNew", globalMeasureGraphics);
			}
			if (tbReportOpen != null)
			{
				SetSuperToolTip(tbReportOpen, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportOpen")) + " (Ctrl + O)", "ReportOpen", globalMeasureGraphics);
			}
			if (tbReportSave != null)
			{
				SetSuperToolTip(tbReportSave, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportSave")) + " (Ctrl + S)", "ReportSave", globalMeasureGraphics);
			}
			if (tbReportPreview != null)
			{
				SetSuperToolTip(tbReportPreview, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportPreview")) + " (F5)", "ReportPreview", globalMeasureGraphics);
			}
			if (tbPageNew != null)
			{
				SetSuperToolTip(tbPageNew, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageNew")) + " (Ctrl + Shift + N)", "PageNew", globalMeasureGraphics);
			}
			if (tbPageDashdoardNew != null)
			{
				SetSuperToolTip(tbPageDashdoardNew, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileDashboardNew")) + " (Ctrl + Shift + D)", "DashboardNew", globalMeasureGraphics);
			}
			if (tbFormNew != null)
			{
				SetSuperToolTip(tbFormNew, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileFormNew")) ?? "", "FormNew", globalMeasureGraphics);
			}
			if (tbScreenNew != null)
			{
				SetSuperToolTip(tbScreenNew, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileScreenNew")) + " (Ctrl + Shift + F)", "ScreenNew", globalMeasureGraphics);
			}
			if (tbPageDelete != null)
			{
				SetSuperToolTip(tbPageDelete, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageDelete")), "PageDelete", globalMeasureGraphics);
			}
			if (tbPageSetup != null)
			{
				SetSuperToolTip(tbPageSetup, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageSetup")), "PageSetup", globalMeasureGraphics);
			}
			if (tbUndo != null)
			{
				SetSuperToolTip(tbUndo, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditUndo")) + " (Ctrl + Z)", "Undo", globalMeasureGraphics);
			}
			if (tbRedo != null)
			{
				SetSuperToolTip(tbRedo, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditRedo")) + " (Ctrl + Y)", "Redo", globalMeasureGraphics);
			}
			if (tbCut != null)
			{
				SetSuperToolTip(tbCut, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCut")) + " (Ctrl + X)", "menuEditCut", globalMeasureGraphics);
			}
			if (tbCopy != null)
			{
				SetSuperToolTip(tbCopy, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCopy")) + " (Ctrl + C)", "menuEditCopy", globalMeasureGraphics);
			}
			if (tbPaste != null)
			{
				SetSuperToolTip(tbPaste, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditPaste")) + " (Ctrl + V)", "menuEditPaste", globalMeasureGraphics);
			}
			if (tbDelete != null)
			{
				SetSuperToolTip(tbDelete, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditDelete")) + " (Del)", "menuEditDelete", globalMeasureGraphics);
			}
			if (tbSelectAll != null)
			{
				SetSuperToolTip(tbSelectAll, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditSelectAll")) + " (Ctrl + A)", "SelectAll", globalMeasureGraphics);
			}
			if (tbShowGrid != null)
			{
				SetSuperToolTip(tbShowGrid, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowGrid")), "menuViewShowGrid", globalMeasureGraphics);
			}
			if (tbShowAlignToGrid != null)
			{
				SetSuperToolTip(tbShowAlignToGrid, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewAlignToGrid")), "menuViewAlignToGrid", globalMeasureGraphics);
			}
			if (tbShowHeaders != null)
			{
				SetSuperToolTip(tbShowHeaders, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowHeaders")), "menuViewShowHeaders", globalMeasureGraphics);
			}
			if (tbShowOrder != null)
			{
				SetSuperToolTip(tbShowOrder, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowOrder")), "menuViewShowOrder", globalMeasureGraphics);
			}
			if (tbQuickInfo != null)
			{
				SetSuperToolTip(tbQuickInfo, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewQuickInfo")), "menuViewQuickInfo", globalMeasureGraphics);
			}
			if (tbShowRulers != null)
			{
				SetSuperToolTip(tbShowRulers, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowRulers")), "menuViewShowRulers", globalMeasureGraphics);
			}
			if (cbZoom != null)
			{
				SetSuperToolTip(cbZoom, StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Zoom")), "Zoom", globalMeasureGraphics);
			}
		}
		else
		{
			if (tbReportNew != null)
			{
				tbReportNew.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportNew")) + " (Ctrl + N)";
			}
			if (tbReportOpen != null)
			{
				tbReportOpen.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportOpen")) + " (Ctrl + O)";
			}
			if (tbReportSave != null)
			{
				tbReportSave.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportSave")) + " (Ctrl + S)";
			}
			if (tbReportPreview != null)
			{
				tbReportPreview.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportPreview")) + " (F5)";
			}
			if (tbPageNew != null)
			{
				tbPageNew.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageNew")) + " (Ctrl + Shift + N)";
			}
			if (tbFormNew != null)
			{
				tbFormNew.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileFormNew")) + " (" + StiLocalization.Get("Report", "Obsoleted") + ")";
			}
			if (tbScreenNew != null)
			{
				tbScreenNew.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileScreenNew")) + " (Ctrl + Shift + F)";
			}
			if (tbPageDelete != null)
			{
				tbPageDelete.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageDelete"));
			}
			if (tbPageSetup != null)
			{
				tbPageSetup.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageSetup"));
			}
			if (tbUndo != null)
			{
				tbUndo.Tooltip = string.Format("{0} ({1})", StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditUndo")), " (Ctrl + Z)");
			}
			if (tbRedo != null)
			{
				tbRedo.Tooltip = string.Format("{0} ({1})", StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditRedo")), "Ctrl + Y");
			}
			if (tbCut != null)
			{
				tbCut.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCut")) + " (Ctrl + X)";
			}
			if (tbCopy != null)
			{
				tbCopy.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCopy")) + " (Ctrl + C)";
			}
			if (tbPaste != null)
			{
				tbPaste.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditPaste")) + " (Ctrl + V)";
			}
			if (tbDelete != null)
			{
				tbDelete.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditDelete")) + " (Del)";
			}
			if (tbSelectAll != null)
			{
				tbSelectAll.Tooltip = string.Format("{0}{1}", StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditSelectAll")), " (Ctrl + A)");
			}
			if (tbShowGrid != null)
			{
				tbShowGrid.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowGrid"));
			}
			if (tbShowAlignToGrid != null)
			{
				tbShowAlignToGrid.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewAlignToGrid"));
			}
			if (tbShowHeaders != null)
			{
				tbShowHeaders.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowHeaders"));
			}
			if (tbShowOrder != null)
			{
				tbShowOrder.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowOrder"));
			}
			if (tbQuickInfo != null)
			{
				tbQuickInfo.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewQuickInfo"));
			}
			if (tbShowRulers != null)
			{
				tbShowRulers.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowRulers"));
			}
			if (cbZoom != null)
			{
				cbZoom.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Zoom"));
			}
		}
	}

	public static StiStandardToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiStandardToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiStandardToolbarService)) as StiStandardToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiStandardToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarStandard");
		}
		if (tbReportNew != null)
		{
			tbReportNew.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportNew"));
		}
		if (tbReportOpen != null)
		{
			tbReportOpen.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportOpen"));
		}
		if (tbReportSave != null)
		{
			tbReportSave.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportSave"));
		}
		if (tbReportPreview != null)
		{
			tbReportPreview.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportPreview"));
		}
		if (tbPageNew != null)
		{
			tbPageNew.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageNew"));
		}
		if (tbPageDashdoardNew != null)
		{
			tbPageDashdoardNew.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileDashboardNew"));
		}
		if (tbFormNew != null)
		{
			tbFormNew.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileFormNew") + " (" + StiLocalization.Get("Report", "Obsoleted") + ")");
		}
		if (tbScreenNew != null)
		{
			tbScreenNew.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileScreenNew"));
		}
		if (tbPageDelete != null)
		{
			tbPageDelete.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageDelete"));
		}
		if (tbPageSetup != null)
		{
			tbPageSetup.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageSetup"));
		}
		if (tbUndo != null)
		{
			tbUndo.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditUndo"));
		}
		if (tbRedo != null)
		{
			tbRedo.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditRedo"));
		}
		if (tbCut != null)
		{
			tbCut.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCut"));
		}
		if (tbCopy != null)
		{
			tbCopy.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditCopy"));
		}
		if (tbPaste != null)
		{
			tbPaste.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditPaste"));
		}
		if (tbDelete != null)
		{
			tbDelete.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditDelete"));
		}
		if (tbSelectAll != null)
		{
			tbSelectAll.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditSelectAll"));
		}
		if (tbShowGrid != null)
		{
			tbShowGrid.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowGrid"));
		}
		if (tbShowAlignToGrid != null)
		{
			tbShowAlignToGrid.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewAlignToGrid"));
		}
		if (tbShowHeaders != null)
		{
			tbShowHeaders.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowHeaders"));
		}
		if (tbShowOrder != null)
		{
			tbShowOrder.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowOrder"));
		}
		if (tbQuickInfo != null)
		{
			tbQuickInfo.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewQuickInfo"));
		}
		if (tbShowRulers != null)
		{
			tbShowRulers.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuViewShowRulers"));
		}
		if (itemNone != null)
		{
			itemNone.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoNone");
		}
		if (itemShowComponentsNames != null)
		{
			itemShowComponentsNames.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowComponentsNames");
		}
		if (itemShowAliases != null)
		{
			itemShowAliases.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowAliases");
		}
		if (itemShowContent != null)
		{
			itemShowContent.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowContent");
		}
		if (itemShowFields != null)
		{
			itemShowFields.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFields");
		}
		if (itemShowFieldsOnly != null)
		{
			itemShowFieldsOnly.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFieldsOnly");
		}
		if (itemShowEvents != null)
		{
			itemShowEvents.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowEvents");
		}
		if (itemOverlay != null)
		{
			itemOverlay.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoOverlay");
		}
		if (cbZoom != null)
		{
			cbZoom.Items.Clear();
			cbZoom.Items.Add((object)"25%");
			cbZoom.Items.Add((object)"50%");
			cbZoom.Items.Add((object)"75%");
			cbZoom.Items.Add((object)"100%");
			cbZoom.Items.Add((object)"150%");
			cbZoom.Items.Add((object)"200%");
			cbZoom.Items.Add((object)StiLocalization.Get("Zoom", "OnePage"));
			cbZoom.Items.Add((object)StiLocalization.Get("Zoom", "PageWidth"));
			cbZoom.Items.Add((object)StiLocalization.Get("Zoom", "PageHeight"));
		}
		LocalizeSuperToolTip();
	}

	public override void Init(StiDesignerControl designer)
	{
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf3: Expected O, but got Unknown
		base.Init(designer);
		CreateToolbar();
		base.ToolBar.SuspendLayout();
		((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarStandard");
		if (ShowReportNew)
		{
			Add(ref tbReportNew, StiReportDesignImages.Actions.NewFile(), StiLocalization.Get("MainMenu", "menuFileReportNew"), tbReportNew_Click);
			tbReportNew.Enabled = Report == null || !Report.IsPageDesigner;
			tbReportNew.Name = "tbReportNew";
		}
		if (ShowReportOpen)
		{
			Add(ref tbReportOpen, StiReportDesignImages.Actions.OpenFile(), StiLocalization.Get("MainMenu", "menuFileReportOpen"), tbReportOpen_Click);
			tbReportOpen.Enabled = Report == null || !Report.IsPageDesigner;
			tbReportOpen.Name = "tbReportOpen";
		}
		if (ShowReportSave)
		{
			Add(ref tbReportSave, StiReportDesignImages.Actions.SaveFile(), StiLocalization.Get("MainMenu", "menuFileReportSave"), tbReportSave_Click);
			tbReportSave.Enabled = Report != null && !Report.IsPageDesigner;
			tbReportSave.Name = "tbReportSave";
		}
		if (ShowReportPreview && designer.IsViewerPresent)
		{
			Add(ref tbReportPreview, StiReportDesignImages.Actions.Preview(), StiLocalization.Get("MainMenu", "menuFileReportPreview"), tbReportPreview_Click);
			tbReportPreview.Enabled = Report != null && !Report.IsPageDesigner;
			tbReportPreview.Name = "tbReportPreview";
		}
		AddToolButtonSeparator();
		if (ShowPageNew)
		{
			Add(ref tbPageNew, StiReportDesignImages.Actions.NewPage(), StiLocalization.Get("MainMenu", "menuFilePageNew"), tbPageNew_Click);
			tbPageNew.Enabled = Report != null && !Report.IsPageDesigner;
			tbPageNew.Name = "tbPageNew";
		}
		if (ShowDashboardNew && StiDashboardDesignAssembly.IsAssemblyLoaded)
		{
			Add(ref tbPageDashdoardNew, StiReportDesignImages.Actions.NewDashboard(), Loc.Get("MainMenu", "menuFileDashboardNew"), tbPageDashboardNew_Click);
			tbPageDashdoardNew.Enabled = Report != null && !Report.IsPageDesigner;
			tbPageDashdoardNew.Name = "tbPageDashdoardNew";
		}
		if (ShowScreenNew && StiAppDesignAssembly.IsAssemblyLoaded)
		{
			Add(ref tbScreenNew, StiReportDesignImages.Actions.NewScreen(), StiLocalization.Get("MainMenu", "menuFileScreenNew"), tbScreenNew_Click);
			tbScreenNew.Enabled = Report != null && !Report.IsPageDesigner;
			tbScreenNew.Name = "tbScreenNew";
		}
		if (ShowFormNew)
		{
			Add(ref tbFormNew, StiReportDesignImages.Actions.NewForm(), StiLocalization.Get("MainMenu", "menuFileFormNew") + " (" + StiLocalization.Get("Report", "Obsoleted") + ")", tbFormNew_Click);
			tbFormNew.Enabled = Report != null && !Report.IsPageDesigner;
			tbFormNew.Name = "tbFormNew";
		}
		if (ShowPageDelete)
		{
			Add(ref tbPageDelete, StiReportDesignImages.Actions.Remove(), StiLocalization.Get("MainMenu", "menuFilePageDelete"), tbPageDelete_Click);
			tbPageDelete.Name = "tbPageDelete";
		}
		if (ShowPageSetup)
		{
			Add(ref tbPageSetup, StiReportDesignImages.Actions.SetupPage(), StiLocalization.Get("MainMenu", "menuFilePageSetup"), tbPageSetup_Click);
			tbPageSetup.Name = "tbPageSetup";
		}
		AddToolButtonSeparator();
		if (ShowUndo)
		{
			Add(ref tbUndo, StiReportDesignImages.Editor.Undo(), StiLocalization.Get("MainMenu", "menuEditUndo"), tbUndo_Click);
			tbUndo.Name = "tbUndo";
		}
		if (ShowRedo)
		{
			Add(ref tbRedo, StiReportDesignImages.Editor.Redo(), StiLocalization.Get("MainMenu", "menuEditRedo"), tbRedo_Click);
			tbRedo.Name = "tbRedo";
		}
		AddToolButtonSeparator();
		if (ShowCut)
		{
			Add(ref tbCut, StiReportDesignImages.Editor.ClipboardCut(), StiLocalization.Get("MainMenu", "menuEditCut"), tbCut_Click);
			tbCut.Name = "tbCut";
		}
		if (ShowCopy)
		{
			Add(ref tbCopy, StiReportDesignImages.Editor.ClipboardCopy(), StiLocalization.Get("MainMenu", "menuEditCopy"), tbCopy_Click);
			tbCopy.Name = "tbCopy";
		}
		if (ShowPaste)
		{
			Add(ref tbPaste, StiReportDesignImages.Editor.ClipboardPaste(), StiLocalization.Get("MainMenu", "menuEditPaste"), tbPaste_Click);
			tbPaste.Name = "tbPaste";
		}
		if (ShowDelete)
		{
			Add(ref tbDelete, StiReportDesignImages.Actions.Remove(), StiLocalization.Get("MainMenu", "menuEditDelete"), tbDelete_Click);
			tbDelete.Name = "tbDelete";
		}
		AddToolButtonSeparator();
		if (ShowSelectAll)
		{
			Add(ref tbSelectAll, StiReportDesignImages.Editor.SelectAll(), StiLocalization.Get("MainMenu", "menuEditSelectAll"), tbSelectAll_Click);
			tbSelectAll.Name = "tbSelectAll";
		}
		AddToolButtonSeparator();
		if (ShowShowGrid)
		{
			Add(ref tbShowGrid, StiReportDesignImages.Editor.ShowGrid(), StiLocalization.Get("MainMenu", "menuViewShowGrid"), tbShowGrid_Click);
			tbShowGrid.Name = "tbShowGrid";
		}
		if (ShowAlignToGrid)
		{
			Add(ref tbShowAlignToGrid, StiReportDesignImages.Layout.AlignToGrid(), StiLocalization.Get("MainMenu", "menuViewAlignToGrid"), tbAlignToGrid_Click);
			tbShowAlignToGrid.Name = "tbShowAlignToGrid";
		}
		if (ShowShowHeaders)
		{
			Add(ref tbShowHeaders, StiReportDesignImages.Editor.ShowHeaders(), StiLocalization.Get("MainMenu", "menuViewShowHeaders"), tbShowHeaders_Click);
			tbShowHeaders.Name = "tbShowHeaders";
		}
		if (ShowShowOrder)
		{
			Add(ref tbShowOrder, StiReportDesignImages.Editor.ShowOrder(), StiLocalization.Get("MainMenu", "menuViewShowOrder"), tbShowOrder_Click);
			tbShowOrder.Name = "tbShowOrder";
		}
		if (ShowQuickInfo)
		{
			Add(ref tbQuickInfo, StiReportDesignImages.Editor.ShowQuickInfo(), StiLocalization.Get("MainMenu", "menuViewQuickInfo"), null);
			tbQuickInfo.Name = "tbQuickInfo";
			tbQuickInfo.AutoExpandOnClick = true;
			tbQuickInfo.PopupOpen += tbQuickInfo_PopupOpen;
			itemNone = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoNone"),
				AutoCollapseOnClick = false
			};
			itemNone.Click += ciQuickInfoNone_Click;
			itemShowComponentsNames = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowComponentsNames"),
				AutoCollapseOnClick = false
			};
			itemShowComponentsNames.Click += ciQuickInfoShowComponentsNames_Click;
			itemShowAliases = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowAliases"),
				AutoCollapseOnClick = false
			};
			itemShowAliases.Click += ciQuickInfoShowAliases_Click;
			itemShowContent = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowContent"),
				AutoCollapseOnClick = false
			};
			itemShowContent.Click += ciQuickInfoShowContent_Click;
			itemShowFields = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFields"),
				AutoCollapseOnClick = false
			};
			itemShowFields.Click += ciQuickInfoShowFields_Click;
			itemShowFieldsOnly = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFieldsOnly"),
				AutoCollapseOnClick = false
			};
			itemShowFieldsOnly.Click += ciQuickInfoShowFieldsOnly_Click;
			itemShowEvents = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowEvents"),
				AutoCollapseOnClick = false
			};
			itemShowEvents.Click += ciQuickInfoShowEvents_Click;
			itemOverlay = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoOverlay"),
				BeginGroup = true,
				AutoCollapseOnClick = false,
				AutoCheckOnClick = true
			};
			itemOverlay.Click += ciQuickInfoOverlay_Click;
			SubItemsCollection subItems = tbQuickInfo.SubItems;
			BaseItem[] items = new ButtonItem[8] { itemNone, itemShowComponentsNames, itemShowAliases, itemShowContent, itemShowFields, itemShowFieldsOnly, itemShowEvents, itemOverlay };
			subItems.AddRange(items);
		}
		if (ShowShowRulers)
		{
			Add(ref tbShowRulers, StiReportDesignImages.Editor.ShowRulers(), StiLocalization.Get("MainMenu", "menuViewShowRulers"), tbShowRulers_Click);
			tbShowRulers.Name = "tbShowRulers";
		}
		if (ShowZoom)
		{
			cbZoom = new ComboBoxItem
			{
				Caption = StiLocalization.Get("PropertyMain", "Zoom"),
				Name = "cbZoom",
				BeginGroup = true
			};
			((ComboBox)cbZoom.ComboBoxEx).DropDownStyle = (ComboBoxStyle)1;
			cbZoom.ComboWidth = StiScale.I(70);
			((ComboBox)cbZoom.ComboBoxEx).MaxDropDownItems = 9;
			((ComboBox)cbZoom.ComboBoxEx).DropDownWidth = StiScale.I(100);
			cbZoom.Items.Add((object)"25%");
			cbZoom.Items.Add((object)"50%");
			cbZoom.Items.Add((object)"75%");
			cbZoom.Items.Add((object)"100%");
			cbZoom.Items.Add((object)"150%");
			cbZoom.Items.Add((object)"200%");
			cbZoom.Items.Add((object)StiLocalization.Get("Zoom", "OnePage"));
			cbZoom.Items.Add((object)StiLocalization.Get("Zoom", "PageWidth"));
			cbZoom.Items.Add((object)StiLocalization.Get("Zoom", "PageHeight"));
			((Control)cbZoom.ComboBoxEx).KeyDown += new KeyEventHandler(cbZoom_KeyDown);
			cbZoom.SelectedIndexChanged += cbZoom_SelectedIndexChanged;
			base.ToolBar.Items.Add(cbZoom);
		}
		AddToolbarToDockSite();
		base.ToolBar.ResumeLayout();
		LocalizeSuperToolTip();
		designer.RefreshDesigner += Refresh;
	}

	public override void Dispose()
	{
		base.Dispose();
		tbReportNew = null;
		tbPageDashdoardNew = null;
		tbReportOpen = null;
		tbReportSave = null;
		tbReportPreview = null;
		tbPageNew = null;
		tbFormNew = null;
		tbScreenNew = null;
		tbPageDelete = null;
		tbPageSetup = null;
		tbUndo = null;
		tbRedo = null;
		tbCut = null;
		tbCopy = null;
		tbPaste = null;
		tbDelete = null;
		tbSelectAll = null;
		tbShowGrid = null;
		tbShowAlignToGrid = null;
		tbShowHeaders = null;
		tbShowRulers = null;
		tbShowOrder = null;
		tbQuickInfo = null;
		cbZoom = null;
	}
}
