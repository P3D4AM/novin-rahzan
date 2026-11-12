using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.CrossTab;

public class StiCrossTabDataSourceForm : Form
{
	public enum StartTab
	{
		DataSource,
		DataRelation,
		Sort,
		Filters
	}

	private StiCrossTab crossTab;

	private StiCrossTab selectedCrossTab;

	private IContainer components;

	private StiGroupLine panelFooter;

	private Button btOk;

	private Button btCancel;

	private ImageList ilDataImages;

	private StiCrossTabDataControl crossTabDataControl;

	public StiCrossTabDataSourceForm(StiCrossTab crossTab, StartTab? tab = null)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		crossTabDataControl.StartTab = tab;
		this.crossTab = crossTab;
		selectedCrossTab = (StiCrossTab)crossTab.Clone();
		Localize();
		((Form)this).ShowIcon = false;
		crossTabDataControl.SetCrossTab(selectedCrossTab);
		if (!tab.HasValue || !tab.HasValue)
		{
			return;
		}
		switch (tab.GetValueOrDefault())
		{
		case StartTab.DataSource:
			crossTabDataControl.TabControl.SelectedTab = crossTabDataControl.pageDataSource;
			break;
		case StartTab.DataRelation:
			if (!crossTabDataControl.pageDataSourceDataRelation.Invisible)
			{
				crossTabDataControl.TabControl.SelectedTab = crossTabDataControl.pageDataSourceDataRelation;
			}
			break;
		case StartTab.Filters:
			if (!crossTabDataControl.pageBusinessObjectFilters.Invisible)
			{
				crossTabDataControl.TabControl.SelectedTab = crossTabDataControl.pageBusinessObjectFilters;
			}
			else
			{
				crossTabDataControl.TabControl.SelectedTab = crossTabDataControl.pageDataSourceFilters;
			}
			break;
		case StartTab.Sort:
			if (!crossTabDataControl.pageBusinessObjectSort.Invisible)
			{
				crossTabDataControl.TabControl.SelectedTab = crossTabDataControl.pageBusinessObjectSort;
			}
			else
			{
				crossTabDataControl.TabControl.SelectedTab = crossTabDataControl.pageDataSourceSort;
			}
			break;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiCrossTab");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	internal void Save()
	{
		crossTabDataControl.SaveData();
		crossTab.DataSourceName = ((crossTabDataControl.GetSelectedDataSource() != null) ? crossTabDataControl.GetSelectedDataSource().Name : string.Empty);
		crossTab.BusinessObjectGuid = ((crossTabDataControl.GetSelectedBusinessObject() != null) ? crossTabDataControl.GetSelectedBusinessObject().Guid : string.Empty);
		crossTab.DataRelationName = selectedCrossTab.DataRelationName;
		crossTab.Sort = selectedCrossTab.Sort;
		crossTab.Filters.Clear();
		crossTab.Filters.AddRange(selectedCrossTab.Filters);
		crossTab.FilterMode = selectedCrossTab.FilterMode;
		crossTab.SummaryDirection = selectedCrossTab.SummaryDirection;
		crossTab.Components.Clear();
		crossTab.CrossTabStyleIndex = selectedCrossTab.CrossTabStyleIndex;
		crossTab.Components.AddRange(selectedCrossTab.Components);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_crosstable_cross_table_editor.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		Save();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCrossTabDataSourceForm));
		ilDataImages = new ImageList(components);
		btOk = new Button();
		btCancel = new Button();
		crossTabDataControl = new StiCrossTabDataControl();
		panelFooter = new StiGroupLine();
		((Control)panelFooter).SuspendLayout();
		((Control)this).SuspendLayout();
		ilDataImages.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilDataImages.ImageStream");
		ilDataImages.TransparentColor = Color.Magenta;
		ilDataImages.Images.SetKeyName(0, "");
		ilDataImages.Images.SetKeyName(1, "BusinessObjectNew.png");
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(815, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(896, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)crossTabDataControl).Dock = (DockStyle)5;
		((Control)crossTabDataControl).Location = new Point(0, 0);
		((Control)crossTabDataControl).Name = "crossTabDataControl";
		((Control)crossTabDataControl).Size = new Size(983, 653);
		((Control)crossTabDataControl).TabIndex = 0;
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)btOk);
		((Control)panelFooter).Controls.Add((Control)(object)btCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 653);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(983, 46);
		((Control)panelFooter).TabIndex = 5;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(983, 699);
		((Control)this).Controls.Add((Control)(object)crossTabDataControl);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiCrossTabDataSourceForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "CrossTab Editor";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelFooter).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
