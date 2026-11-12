using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Map.Design;

public class StiMapDesignerForm : Form
{
	private StiMapTabChoroplethControl choroplethControl;

	private StiMapTabOnlineControl onlineControl;

	private StiMap cloneMap;

	private StiMap map;

	private bool dialogResult;

	private IContainer components;

	private StiCloudTabulator tabControl;

	private StiCloudTabulatorPage pageChoropleth;

	private StiCloudTabulatorPage pageOnline;

	private StiGroupLine panelFooter;

	private Button btOk;

	private Button btCancel;

	internal StiReport Report => map.Report;

	internal StiDesignerControl DesignerControl { get; private set; }

	public StiMapDesignerForm(StiMap map)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (map.MapIdent == StiMapID.ChinaWithHongKongAndMacau.ToString() || map.MapIdent == StiMapID.ChinaWithHongKongMacauAndTaiwan.ToString())
			{
				map.MapIdent = StiMapID.China.ToString();
			}
			this.map = map;
			cloneMap = (StiMap)map.Clone();
			DesignerControl = map.Report.Designer.DesignerControl as StiDesignerControl;
			tabControl.SelectedIndex = ((map.MapMode != 0) ? 1 : 0);
			tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
			TabControl_SelectedIndexChanged(null, null);
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			Localize();
			DoScaling();
			((Form)this).ShowIcon = false;
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = tabControl.DockPadding;
		int right = (tabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageChoropleth.Image = StiReportDesignImages.Tasks.MapChoropleth();
		pageOnline.Image = StiReportDesignImages.Tasks.MapOnline();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Map", "MapEditorForm");
		((Control)pageChoropleth).Text = StiLocalization.Get("PropertyEnum", "StiMapModeChoropleth");
		((Control)pageOnline).Text = StiLocalization.Get("PropertyEnum", "StiMapModeOnline");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Closed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		if (dialogResult)
		{
			return;
		}
		StiContainer parent = map.Parent;
		if (parent != null)
		{
			int num = parent.Components.IndexOf(map);
			if (num != -1)
			{
				parent.Components.RemoveAt(num);
				parent.Components.Insert(num, cloneMap);
				((Control)DesignerControl).Refresh();
			}
		}
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (tabControl.SelectedIndex)
		{
		case 0:
			map.MapMode = StiMapMode.Choropleth;
			if (choroplethControl == null)
			{
				StiMapTabChoroplethControl stiMapTabChoroplethControl = new StiMapTabChoroplethControl(this, map);
				((Control)stiMapTabChoroplethControl).Dock = (DockStyle)5;
				choroplethControl = stiMapTabChoroplethControl;
				((Control)pageChoropleth).Controls.Add((Control)(object)choroplethControl);
			}
			((Control)choroplethControl.comboBoxDataFrom).Focus();
			break;
		case 1:
			map.MapMode = StiMapMode.Online;
			if (onlineControl == null)
			{
				StiMapTabOnlineControl stiMapTabOnlineControl = new StiMapTabOnlineControl(this, map);
				((Control)stiMapTabOnlineControl).Dock = (DockStyle)5;
				onlineControl = stiMapTabOnlineControl;
				((Control)pageOnline).Controls.Add((Control)(object)onlineControl);
			}
			break;
		}
		((Control)DesignerControl).Refresh();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		dialogResult = true;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Expected O, but got Unknown
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiMapDesignerForm));
		panelFooter = new StiGroupLine();
		btOk = new Button();
		btCancel = new Button();
		tabControl = new StiCloudTabulator();
		pageChoropleth = new StiCloudTabulatorPage();
		pageOnline = new StiCloudTabulatorPage();
		((Control)panelFooter).SuspendLayout();
		((Control)tabControl).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)btOk);
		((Control)panelFooter).Controls.Add((Control)(object)btCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 506);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(600, 46);
		((Control)panelFooter).TabIndex = 7;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(432, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(513, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)tabControl).Controls.Add((Control)(object)pageChoropleth);
		((Control)tabControl).Controls.Add((Control)(object)pageOnline);
		((Control)tabControl).Dock = (DockStyle)5;
		((Control)tabControl).Location = new Point(0, 0);
		((Control)tabControl).Margin = new Padding(0, 3, 3, 3);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(0, 0, 119, 0);
		tabControl.ScrollHeight = 0;
		tabControl.ScrollWidth = 100;
		((Control)tabControl).Size = new Size(600, 506);
		((Control)tabControl).TabIndex = 58;
		tabControl.TitleBackColor = SystemColors.Control;
		tabControl.TitleInLine = false;
		tabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		tabControl.TitleWidth = 110;
		pageChoropleth.Dock = (DockStyle)5;
		pageChoropleth.Image = (Image)componentResourceManager.GetObject("pageChoropleth.Image");
		pageChoropleth.InvisibleVisible = true;
		((Control)pageChoropleth).Location = new Point(0, 0);
		((Control)pageChoropleth).Name = "pageChoropleth";
		((Control)pageChoropleth).Padding = new Padding(0, 0, 1, 0);
		((Control)pageChoropleth).Size = new Size(481, 506);
		((Control)pageChoropleth).TabIndex = 0;
		((Control)pageChoropleth).Text = "Choropleth";
		((Control)pageChoropleth).Visible = false;
		pageOnline.Dock = (DockStyle)5;
		pageOnline.Image = (Image)componentResourceManager.GetObject("pageOnline.Image");
		pageOnline.InvisibleVisible = false;
		((Control)pageOnline).Location = new Point(0, 0);
		((Control)pageOnline).Name = "pageOnline";
		((Control)pageOnline).Padding = new Padding(0, 0, 1, 0);
		((Control)pageOnline).Size = new Size(481, 506);
		((Control)pageOnline).TabIndex = 1;
		((Control)pageOnline).Text = "Online";
		((Control)pageOnline).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(600, 552);
		((Control)this).Controls.Add((Control)(object)tabControl);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(1016, 1385);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(595, 535);
		((Control)this).Name = "StiMapDesignerForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiMapDesignerForm";
		((Form)this).FormClosed += new FormClosedEventHandler(This_Closed);
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panelFooter).ResumeLayout(false);
		((Control)tabControl).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
