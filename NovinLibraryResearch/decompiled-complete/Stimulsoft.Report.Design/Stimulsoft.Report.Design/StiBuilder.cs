using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dialogs;

namespace Stimulsoft.Report.Design;

public sealed class StiBuilder
{
	public enum Tabs
	{
		Page,
		Code,
		Preview
	}

	private StiDesignerPreviewControl previewControl;

	private List<StiRollerLine> rollerLines = new List<StiRollerLine>();

	private bool lockReportRefresh;

	private bool lockDesignerRefresh;

	private bool lockPageNamesRefresh;

	private bool lockBuildReport;

	internal int PagesCount { get; set; } = 1;


	public StiDesignerControl Designer { get; set; }

	public StiReport Report => Designer.Report;

	public void Localize()
	{
		if (previewControl != null)
		{
			previewControl.Localize();
		}
		if (Designer.RibbonDesignerUserControl != null)
		{
			LocalizePreview();
		}
	}

	private void LocalizePreview()
	{
		if (Designer.RibbonDesignerUserControl.RibbonTabPreview != null)
		{
			Designer.RibbonDesignerUserControl.RibbonTabPreview.Text = StiLocalization.Get("FormDesigner", "Preview");
			object tag = Designer.RibbonDesignerUserControl.RibbonTabPreview.Tag;
			MethodInfo method = tag.GetType().GetMethod("Localize");
			method.Invoke(tag, new object[0]);
		}
	}

	internal void CheckCodeTab(bool showCodeTab)
	{
		if (!StiOptions.Designer.IsCodeTabVisible)
		{
			return;
		}
		foreach (TabItem tab in Designer.TabControl.Tabs)
		{
			if (tab.Tag is Tabs && (Tabs)tab.Tag == Tabs.Code)
			{
				if (tab.Visible != showCodeTab)
				{
					tab.Visible = showCodeTab;
				}
				if (tab.Visible && ((ArrangedElementCollection)tab.AttachedControl.Controls).Count == 0)
				{
					CreateCodeTabContent(tab);
				}
				break;
			}
		}
	}

	private void CheckPreviewTab(bool showPreviewTab)
	{
		if (!StiOptions.Designer.PreviewReportVisible || !Designer.IsViewerPresent)
		{
			return;
		}
		foreach (TabItem tab in Designer.TabControl.Tabs)
		{
			if (tab.Tag is Tabs && (Tabs)tab.Tag == Tabs.Preview)
			{
				if (tab.Visible != showPreviewTab)
				{
					tab.Visible = showPreviewTab;
				}
				if (tab.Visible && ((ArrangedElementCollection)tab.AttachedControl.Controls).Count == 0)
				{
					CreatePreviewTabContent(tab);
				}
				break;
			}
		}
	}

	private void CreateCodeTabContent(TabItem page)
	{
		StiDesignerCodeControl stiDesignerCodeControl = new StiDesignerCodeControl(Designer);
		((Control)stiDesignerCodeControl).Dock = (DockStyle)5;
		StiDesignerCodeControl stiDesignerCodeControl2 = stiDesignerCodeControl;
		page.AttachedControl.Controls.Add((Control)(object)stiDesignerCodeControl2);
		page.AttachedControl.Controls.SetChildIndex((Control)(object)stiDesignerCodeControl2, 0);
		bool visible = StiOptions.Designer.IsCodeTabVisible;
		if (Report != null && Report.IsPageDesigner)
		{
			visible = false;
		}
		page.Visible = visible;
	}

	private void CreatePreviewTabContent(TabItem page)
	{
		StiDesignerPreviewControl stiDesignerPreviewControl = new StiDesignerPreviewControl(Designer);
		((Control)stiDesignerPreviewControl).Dock = (DockStyle)5;
		previewControl = stiDesignerPreviewControl;
		page.AttachedControl.Controls.Add((Control)(object)previewControl);
		page.AttachedControl.Controls.SetChildIndex((Control)(object)previewControl, 0);
		bool visible = StiOptions.Designer.PreviewReportVisible;
		if (Report != null && Report.IsPageDesigner)
		{
			visible = false;
		}
		page.Visible = visible;
	}

	private void AddCodeTab(bool showCodeTab)
	{
		TabItem tabItem = new TabItem
		{
			Tag = Tabs.Code,
			Text = StiLocalization.Get("FormDesigner", "Code"),
			Image = StiReportDesignImages.Actions.Code()
		};
		AddTabItemPanel(tabItem);
		if (showCodeTab)
		{
			CreateCodeTabContent(tabItem);
		}
		else
		{
			tabItem.Visible = false;
		}
		Designer.TabControl.Tabs.Add(tabItem);
	}

	private void AddPreviewTab(bool showPreview)
	{
		TabItem tabItem = new TabItem
		{
			Tag = Tabs.Preview,
			Image = StiReportDesignImages.Actions.Preview()
		};
		AddTabItemPanel(tabItem);
		tabItem.Text = StiLocalization.Get("FormDesigner", "Preview");
		if (Designer.IsViewerPresent && showPreview)
		{
			CreatePreviewTabContent(tabItem);
		}
		else
		{
			tabItem.Visible = false;
		}
		Designer.TabControl.Tabs.Add(tabItem);
	}

	private void CreateDesignerPreviewControl()
	{
		StiDesignerPreviewControl stiDesignerPreviewControl = new StiDesignerPreviewControl(Designer);
		((Control)stiDesignerPreviewControl).Dock = (DockStyle)5;
		((Control)stiDesignerPreviewControl).Location = new Point(((Control)Designer).Width / 2, ((Control)Designer).Height / 2);
		((Control)stiDesignerPreviewControl).Name = "previewControl";
		((Control)stiDesignerPreviewControl).Size = new Size(10, 10);
		((Control)stiDesignerPreviewControl).TabIndex = 0;
		((Control)stiDesignerPreviewControl).Visible = false;
		previewControl = stiDesignerPreviewControl;
		((Control)Designer).Parent.Controls.Add((Control)(object)previewControl);
		((Control)Designer).Parent.Controls.SetChildIndex((Control)(object)previewControl, 0);
	}

	private void AddDesignerPreviewControl(bool showPreview)
	{
		if (Designer.IsViewerPresent && showPreview)
		{
			CreateDesignerPreviewControl();
		}
		RibbonTabItem ribbonViewerTab = Designer.GetRibbonViewerTab();
		if (ribbonViewerTab == null)
		{
			return;
		}
		Designer.RibbonDesignerUserControl.RibbonTabPreview = ribbonViewerTab;
		int index = 0;
		for (int i = 0; i < Designer.RibbonControl.Items.Count; i++)
		{
			if (Designer.RibbonControl.Items[i] is RibbonTabItem)
			{
				index = i + 1;
			}
		}
		((Control)Designer.RibbonControl).Controls.Add((Control)(object)ribbonViewerTab.Panel);
		Designer.RibbonControl.Items.Insert(index, ribbonViewerTab);
		StiGuiHelper.ApplyStyle((Control)(object)Designer.RibbonControl);
	}

	public void Redraw(Rectangle clipRect)
	{
		StiPageViewControl currentPageView = Designer.PageView.GetCurrentPageView();
		if (currentPageView != null)
		{
			StiViewControl view = currentPageView.View;
			if (view != null)
			{
				((Control)view).Invalidate(clipRect);
			}
		}
	}

	public void Redraw()
	{
		Redraw(Rectangle.Empty);
	}

	public void RefreshPageNames()
	{
		if (lockPageNamesRefresh)
		{
			return;
		}
		for (int i = 0; i < Designer.TabControl.Tabs.Count; i++)
		{
			TabItem tabItem = Designer.TabControl.Tabs[i];
			if (!(tabItem.Tag is int))
			{
				continue;
			}
			int num = (int)tabItem.Tag;
			if (num != -1 || num < Designer.Report.Pages.Count)
			{
				StiPage stiPage = Designer.Report.Pages[num];
				string text = stiPage.ToString(StiOptions.Dictionary.ShowOnlyAliasForPages && Designer.UseAliases);
				if (tabItem.Text != text)
				{
					tabItem.Text = text;
				}
				SetPageEnabledColor(tabItem, stiPage);
			}
		}
	}

	public void RefreshDesigner()
	{
		if (!lockReportRefresh && !lockDesignerRefresh)
		{
			lockReportRefresh = true;
			Redraw();
			InvalidateRollerLines();
			lockReportRefresh = false;
			RefreshPageNames();
		}
	}

	private void AddTabItemPanel(TabItem tabItem)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		TabControlPanel tabControlPanel = new TabControlPanel();
		((Control)tabControlPanel).Dock = (DockStyle)5;
		((Control)tabControlPanel).Padding = new Padding(1);
		((Control)tabControlPanel).Size = new Size(SystemInformation.WorkingArea.Width, SystemInformation.WorkingArea.Height);
		tabControlPanel.TabItem = tabItem;
		TabControlPanel tabControlPanel2 = (TabControlPanel)(object)(tabItem.AttachedControl = (Control)(object)tabControlPanel);
		((Control)Designer.TabControl).Controls.Add((Control)(object)tabControlPanel2);
	}

	private static void SetPageEnabledColor(TabItem tabPage, StiPage page)
	{
		tabPage.TextColor = (page.Enabled ? Color.Black : Color.Silver);
	}

	private void DisposeRollerLines()
	{
		if (rollerLines != null)
		{
			foreach (StiRollerLine rollerLine in rollerLines)
			{
				rollerLine.DisposeRes();
			}
			rollerLines.Clear();
		}
		else
		{
			rollerLines = new List<StiRollerLine>();
		}
	}

	private void InvalidateRollerLines()
	{
		if (rollerLines == null)
		{
			return;
		}
		foreach (StiRollerLine rollerLine in rollerLines)
		{
			((Control)rollerLine).Invalidate();
		}
	}

	private void DisposePageViewControls()
	{
		if (Designer == null)
		{
			return;
		}
		for (int i = 0; i < Designer.TabControl.Tabs.Count - PagesCount; i++)
		{
			ControlCollection controls = Designer.TabControl.Tabs[i].AttachedControl.Controls;
			if (((ArrangedElementCollection)controls).Count >= 1 && controls[0] is StiPageViewControl stiPageViewControl)
			{
				stiPageViewControl.DisposeRes();
				Designer.ContextMenuBar.SetContextMenuEx((Control)(object)stiPageViewControl, null);
			}
		}
	}

	public void BuildReport(bool selectedObject = true)
	{
		if (lockBuildReport || Designer == null)
		{
			return;
		}
		StiSettings.Load();
		bool @bool = StiSettings.GetBool("StiDesignerOptions", "ShowCodeTab", nullValue: false);
		bool bool2 = StiSettings.GetBool("StiDesignerOptions", "ShowPreview", nullValue: true);
		bool flag = !(StiOptions.Designer.Ribbon.ShowTabPreview & Designer.IsRibbonGuiMode);
		Designer.PreviousSelectedIndex = -1;
		Designer.SortItems();
		lockDesignerRefresh = true;
		lockPageNamesRefresh = true;
		lockBuildReport = true;
		Designer.LockTabControlRefresh = true;
		((Control)Designer.TabControl).SuspendLayout();
		int num = 0;
		int selectedTabIndex = 0;
		if (Report != null)
		{
			while (Designer.TabControl.Tabs.Count > Report.Pages.Count + PagesCount)
			{
				if (Designer.TabControl.Tabs[0].AttachedControl.Controls[0] is StiPageViewControl stiPageViewControl)
				{
					Designer.ContextMenuBar.SetContextMenuEx((Control)(object)stiPageViewControl, null);
					stiPageViewControl.DisposeRes();
				}
				Designer.TabControl.Tabs.RemoveAt(0);
			}
			while (Designer.TabControl.Tabs.Count < Report.Pages.Count + PagesCount)
			{
				if (Designer.TabControl.Tabs.Count == 0)
				{
					AddCodeTab(@bool);
					if (flag)
					{
						AddPreviewTab(bool2);
						PagesCount++;
					}
				}
				TabItem tabItem = new TabItem();
				AddTabItemPanel(tabItem);
				tabItem.DoubleClick += Page_DoubleClick;
				Designer.TabControl.Tabs.Insert(Designer.TabControl.Tabs.Count - PagesCount, tabItem);
			}
			if (Designer.IsRibbonGuiMode && StiOptions.Designer.Ribbon.ShowTabPreview && Designer.RibbonDesignerUserControl.RibbonTabPreview == null)
			{
				AddDesignerPreviewControl(bool2);
			}
			CheckCodeTab(@bool);
			if (flag)
			{
				CheckPreviewTab(bool2);
			}
			int num2 = 0;
			foreach (StiPage page in Report.Pages)
			{
				TabItem tabItem2 = Designer.TabControl.Tabs[num2];
				tabItem2.Image = GetPageImage(page);
				tabItem2.Text = page.ToString(StiOptions.Dictionary.ShowOnlyAliasForPages && Designer.UseAliases);
				tabItem2.Tag = num2;
				SetPageEnabledColor(tabItem2, page);
				if (Designer.SelectedObject is StiPage)
				{
					if (Designer.SelectedObject == page)
					{
						selectedTabIndex = num;
					}
				}
				else if (Designer.SelectedObject is StiComponent && ((StiComponent)Designer.SelectedObject).Page == page)
				{
					selectedTabIndex = num;
				}
				StiPageViewControl stiPageViewControl2;
				if (((ArrangedElementCollection)tabItem2.AttachedControl.Controls).Count > 0)
				{
					stiPageViewControl2 = tabItem2.AttachedControl.Controls[0] as StiPageViewControl;
				}
				else
				{
					StiPageViewControl stiPageViewControl3 = new StiPageViewControl(Designer);
					((Control)stiPageViewControl3).Dock = (DockStyle)5;
					((Control)stiPageViewControl3).Width = SystemInformation.WorkingArea.Width;
					((Control)stiPageViewControl3).Height = SystemInformation.WorkingArea.Height;
					stiPageViewControl2 = stiPageViewControl3;
					if (StiOptions.Designer.AllowUseDragDrop)
					{
						((Control)stiPageViewControl2).AllowDrop = true;
					}
					tabItem2.AttachedControl.Controls.Add((Control)(object)stiPageViewControl2);
					Designer.ContextMenuBar.SetContextMenuEx((Control)(object)stiPageViewControl2, Designer.cmDesigner);
				}
				page.CheckLargeHeight(needFullCalculation: true);
				CheckRulers(tabItem2, stiPageViewControl2, page);
				stiPageViewControl2.PageIndex = num2++;
				num++;
			}
		}
		Designer.ApplyStyleTabControl();
		if (selectedObject)
		{
			Designer.TabControl.SelectedTabIndex = selectedTabIndex;
		}
		((Control)Designer.TabControl).ResumeLayout();
		Designer.InvokeRebuildDesigner();
		Designer.InvokeRefreshDictionary();
		((Control)Designer).Refresh();
		Designer.InvokeSelectedObjectsChanged();
		Redraw();
		StiPageViewControl currentPageView = Designer.PageView.GetCurrentPageView();
		if (currentPageView != null)
		{
			((Control)currentPageView.View).Focus();
		}
		Designer.LockTabControlRefresh = false;
		lockBuildReport = false;
		lockDesignerRefresh = false;
		lockPageNamesRefresh = false;
		StiElementActionHelper.CheckStatus(null, Designer);
	}

	private static Image GetPageImage(StiPage page)
	{
		if (page.Icon != null)
		{
			Image val = StiImageConverter.BytesToImage(page.Icon);
			int i = StiScale.I16;
			if (val.Width == i && val.Height == i)
			{
				return val;
			}
			return (Image)(object)StiImageUtils.ResizeImage(val, i, i);
		}
		if (page is StiForm)
		{
			return StiReportDesignImages.Controls.Form();
		}
		if (page is IStiScreen)
		{
			return StiReportDesignImages.ComponentUIs.Screen();
		}
		if (page is IStiDashboard)
		{
			return StiReportDesignImages.Elements.Dashboard();
		}
		return StiReportDesignImages.Components.Page();
	}

	private void CheckRulers(TabItem tabPage, StiPageViewControl pageView, StiPage page)
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		if (Report.Info.ShowRulers && !(page is IStiDashboard))
		{
			foreach (Control item in (ArrangedElementCollection)tabPage.AttachedControl.Controls)
			{
				Control val = item;
				if (val is StiHorRollerLine || val is StiVerRollerLine)
				{
					return;
				}
			}
			StiHorRollerLine stiHorRollerLine = new StiHorRollerLine(Designer, page, pageView);
			((Control)stiHorRollerLine).BackColor = Color.FromArgb(255, 155, 187, 227);
			((Control)stiHorRollerLine).Dock = (DockStyle)1;
			StiHorRollerLine stiHorRollerLine2 = stiHorRollerLine;
			tabPage.AttachedControl.Controls.Add((Control)(object)stiHorRollerLine2);
			rollerLines.Add(stiHorRollerLine2);
			StiVerRollerLine stiVerRollerLine = new StiVerRollerLine(Designer, page, pageView);
			((Control)stiVerRollerLine).BackColor = Color.FromArgb(255, 155, 187, 227);
			((Control)stiVerRollerLine).Dock = (DockStyle)3;
			StiVerRollerLine stiVerRollerLine2 = stiVerRollerLine;
			tabPage.AttachedControl.Controls.Add((Control)(object)stiVerRollerLine2);
			rollerLines.Add(stiVerRollerLine2);
			return;
		}
		List<StiRollerLine> list = new List<StiRollerLine>();
		foreach (Control item2 in (ArrangedElementCollection)tabPage.AttachedControl.Controls)
		{
			Control val2 = item2;
			if (val2 is StiHorRollerLine || val2 is StiVerRollerLine)
			{
				list.Add((StiRollerLine)(object)val2);
			}
		}
		foreach (StiRollerLine item3 in list)
		{
			tabPage.AttachedControl.Controls.Remove((Control)(object)item3);
			rollerLines.Remove(item3);
		}
	}

	public void InvalidateRulers()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		if (!Report.Info.ShowRulers)
		{
			return;
		}
		StiPageViewControl currentPageView = Designer.PageView.GetCurrentPageView();
		if (currentPageView == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)currentPageView).Parent.Controls)
		{
			Control val = item;
			if (val is StiHorRollerLine || val is StiVerRollerLine)
			{
				val.Invalidate();
			}
		}
	}

	public void RefreshPages(bool selectedObject = true)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		if (Report.Pages.Count == ((ArrangedElementCollection)((Control)Designer.TabControl).Controls).Count - PagesCount)
		{
			for (int i = 0; i < Report.Pages.Count; i++)
			{
				TabItem tabItem = Designer.TabControl.Tabs[i];
				tabItem.Tag = i;
				foreach (Control item in (ArrangedElementCollection)tabItem.AttachedControl.Controls)
				{
					Control val = item;
					if (val is StiPageViewControl stiPageViewControl)
					{
						stiPageViewControl.PageIndex = i;
					}
				}
			}
		}
		else
		{
			BuildReport(selectedObject);
		}
	}

	internal StiDesignerCodeControl GetCodeEditor()
	{
		if (!StiOptions.Designer.IsCodeTabVisible)
		{
			return null;
		}
		foreach (TabItem tab in Designer.TabControl.Tabs)
		{
			if (tab.Tag is Tabs && (Tabs)tab.Tag == Tabs.Code)
			{
				if (((ArrangedElementCollection)tab.AttachedControl.Controls).Count == 0)
				{
					return null;
				}
				return tab.AttachedControl.Controls[0] as StiDesignerCodeControl;
			}
		}
		return null;
	}

	internal StiDesignerPreviewControl GetPreviewControl()
	{
		if (Designer.IsRibbonGuiMode && StiOptions.Designer.Ribbon.ShowTabPreview)
		{
			return previewControl;
		}
		if (!StiOptions.Designer.PreviewReportVisible || !Designer.IsViewerPresent)
		{
			return null;
		}
		foreach (TabItem tab in Designer.TabControl.Tabs)
		{
			if (tab.Tag is Tabs && (Tabs)tab.Tag == Tabs.Preview)
			{
				if (((ArrangedElementCollection)tab.AttachedControl.Controls).Count == 0)
				{
					return null;
				}
				return tab.AttachedControl.Controls[0] as StiDesignerPreviewControl;
			}
		}
		return null;
	}

	private void Page_DoubleClick(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		StiPageRenameForm stiPageRenameForm = new StiPageRenameForm(Designer.Report.GetCurrentPage());
		try
		{
			Designer?.AddOwnedForm((Form)(object)stiPageRenameForm);
			if ((int)((Form)stiPageRenameForm).ShowDialog() == 1)
			{
				RefreshPageNames();
			}
		}
		finally
		{
			((IDisposable)stiPageRenameForm)?.Dispose();
		}
	}

	public void Dispose()
	{
		PagesCount = 1;
		DisposePageViewControls();
		DisposeRollerLines();
		Designer = null;
	}

	public StiBuilder(StiDesignerControl designer)
	{
		Designer = designer;
	}
}
