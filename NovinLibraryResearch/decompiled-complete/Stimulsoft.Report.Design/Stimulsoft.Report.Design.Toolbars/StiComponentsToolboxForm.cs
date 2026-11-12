using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Toolbox;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiComponentsToolboxForm : StiFormUX
{
	private delegate bool IsComponentVisibleMethod(StiComponent comp);

	private StiToolboxMode mode;

	private bool lockState;

	private IContainer components;

	private StiTreeView tvComponent;

	private StiCheckBoxUX checkBoxShowInsertTab;

	private StiCheckBoxUX checkBoxShowToolbox;

	private StiButtonPanelUX groupButtonsPanel;

	private StiGroupPanelUX groupAlignmentPanel;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	public StiComponentsToolboxForm(StiToolboxMode mode)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.mode = mode;
			Localize();
			Build();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			StiSettings.Load();
			((CheckBox)checkBoxShowToolbox).Checked = StiSettings.GetBool("CloudDesigner", "ShowStandardToolbox", nullValue: true);
			((CheckBox)checkBoxShowInsertTab).Checked = StiSettings.GetBool("CloudDesigner", "ShowInsertTab", nullValue: true);
		}
	}

	private void Localize()
	{
		((Control)checkBoxShowToolbox).Text = StiLocalization.Get("MainMenu", "menuViewShowToolbox");
		((Control)checkBoxShowInsertTab).Text = StiLocalization.Get("MainMenu", "menuViewShowInsertTab");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("FormDesigner", "SetupToolbox");
	}

	private void Build()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		((TreeView)tvComponent).Nodes.Clear();
		ImageList val = new ImageList
		{
			ColorDepth = (ColorDepth)32,
			ImageSize = new Size(StiScale.XXI(16.0), StiScale.YYI(16.0))
		};
		IsComponentVisibleMethod isComponentVisibleMethod = null;
		switch (mode)
		{
		case StiToolboxMode.Reports:
			isComponentVisibleMethod = StiToolboxSetup.Reports.IsComponentVisible;
			break;
		case StiToolboxMode.Dashboards:
			isComponentVisibleMethod = StiToolboxSetup.Dashboards.IsComponentVisible;
			break;
		case StiToolboxMode.Dialogs:
			isComponentVisibleMethod = StiToolboxSetup.Dialogs.IsComponentVisible;
			break;
		case StiToolboxMode.ComponentUIs:
			isComponentVisibleMethod = StiToolboxSetup.ComponentUIs.IsComponentVisible;
			break;
		}
		List<StiComponent> list = StiDashboardAssembly.LoadDashboardElements();
		List<StiComponent> list2 = StiAppAssembly.LoadAppComponents();
		IEnumerable<StiComponent> mainList = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled);
		mainList = StiDesignerSpecificationHelper.CheckShowComponents(mainList);
		foreach (StiComponent item in mainList)
		{
			StiToolboxAttribute[] array = (StiToolboxAttribute[])item.GetType().GetCustomAttributes(typeof(StiToolboxAttribute), inherit: true);
			if (array.Length == 0 || !array[0].Enable || (list != null && list.Contains(item) && mode != StiToolboxMode.Dashboards) || ((mode != 0 || item is StiPage || item is IStiReportControl) && (mode != StiToolboxMode.Dashboards || item is IStiDashboard || !list.Contains(item)) && (mode != StiToolboxMode.ComponentUIs || item is IStiScreen || !list2.Contains(item)) && (mode != StiToolboxMode.Dialogs || item is IStiForm || !(item is IStiReportControl))))
			{
				continue;
			}
			string localizedCategory = item.LocalizedCategory;
			TreeNode val2 = null;
			foreach (TreeNode node in ((TreeView)tvComponent).Nodes)
			{
				TreeNode val3 = node;
				if (val3.Text == localizedCategory)
				{
					val2 = val3;
					break;
				}
			}
			string stepName = StiScale.StepName;
			if (val2 == null)
			{
				Image val4 = null;
				bool @checked = false;
				if (localizedCategory == StiLocalization.Get("Report", "Components"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catComponents" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowComponentsCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Report", "Bands"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catBands" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowBandsCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Report", "CrossBands"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catCrossBands" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowCrossBandsCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Report", "Shapes"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiShape" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowShapesCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Components", "StiChart"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiChart" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowChartCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Components", "StiBarCode"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiBarCode" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowBarCodesCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Components", "StiGauge"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiGauge" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowGaugeCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Components", "StiMap"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiMap" + stepName + ".png");
					@checked = StiToolboxSetup.Reports.ShowMapCategory;
				}
				else if (localizedCategory == StiLocalization.Get("Permissions", "ItemDashboards"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiDashboard" + stepName + ".png");
				}
				else if (localizedCategory == StiLocalization.Get("Report", "Dialogs"))
				{
					val4 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiScreen" + stepName + ".png");
				}
				if (val4 == null)
				{
					val4 = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(typeof(StiComponent)));
				}
				val.Images.Add(val4);
				int num = val.Images.Count - 1;
				val2 = new TreeNode(localizedCategory, num, num)
				{
					Tag = localizedCategory,
					Checked = @checked
				};
				((TreeView)tvComponent).Nodes.Add(val2);
			}
			if (!(localizedCategory == StiLocalization.Get("Report", "Shapes")) && !(localizedCategory == StiLocalization.Get("Components", "StiChart")) && !(localizedCategory == StiLocalization.Get("Components", "StiGauge")) && !(localizedCategory == StiLocalization.Get("Components", "StiMap")) && !(localizedCategory == StiLocalization.Get("Components", "StiBarCode")))
			{
				Image val5 = (Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + item.GetType().Name + stepName + ".png");
				if (val5 == null)
				{
					val5 = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(item.GetType()));
				}
				if (val5 == null)
				{
					val5 = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(typeof(StiComponent)));
				}
				val.Images.Add(val5);
				int num2 = val.Images.Count - 1;
				bool checked2 = isComponentVisibleMethod(item);
				TreeNode val6 = new TreeNode(item.ServiceName, num2, num2)
				{
					Checked = checked2,
					Tag = item
				};
				val2.Nodes.Add(val6);
				if (((TreeView)tvComponent).Nodes.Count > 0)
				{
					((TreeView)tvComponent).SelectedNode = ((TreeView)tvComponent).Nodes[0];
				}
			}
		}
		((TreeView)tvComponent).ImageList = val;
		((TreeView)tvComponent).ExpandAll();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		StiSettings.Load();
		StiSettings.Set("CloudDesigner", "ShowStandardToolbox", ((CheckBox)checkBoxShowToolbox).Checked);
		StiSettings.Set("CloudDesigner", "ShowInsertTab", ((CheckBox)checkBoxShowInsertTab).Checked);
		StiSettings.Save();
		switch (mode)
		{
		case StiToolboxMode.Reports:
			StiToolboxSetup.Reports.VisibleComponents.Clear();
			foreach (TreeNode node in ((TreeView)tvComponent).Nodes)
			{
				TreeNode val7 = node;
				string text = (string)val7.Tag;
				if (text == StiLocalization.Get("Report", "Components"))
				{
					StiToolboxSetup.Reports.ShowComponentsCategory = val7.Checked;
				}
				else if (text == StiLocalization.Get("Report", "Bands"))
				{
					StiToolboxSetup.Reports.ShowBandsCategory = val7.Checked;
				}
				else if (text == StiLocalization.Get("Report", "CrossBands"))
				{
					StiToolboxSetup.Reports.ShowCrossBandsCategory = val7.Checked;
				}
				else if (text == StiLocalization.Get("Report", "Shapes"))
				{
					StiToolboxSetup.Reports.ShowShapesCategory = val7.Checked;
				}
				else if (text == StiLocalization.Get("Components", "StiBarCode"))
				{
					StiToolboxSetup.Reports.ShowBarCodesCategory = val7.Checked;
				}
				else if (text == StiLocalization.Get("Components", "StiChart"))
				{
					StiToolboxSetup.Reports.ShowChartCategory = val7.Checked;
				}
				else if (text == StiLocalization.Get("Components", "StiGauge"))
				{
					StiToolboxSetup.Reports.ShowGaugeCategory = val7.Checked;
				}
				else if (text == StiLocalization.Get("Components", "StiMap"))
				{
					StiToolboxSetup.Reports.ShowMapCategory = val7.Checked;
				}
				foreach (TreeNode node2 in val7.Nodes)
				{
					TreeNode val8 = node2;
					if (val8.Checked)
					{
						string fullName4 = val8.Tag.GetType().FullName;
						StiToolboxSetup.Reports.VisibleComponents.Add(fullName4);
					}
				}
			}
			break;
		case StiToolboxMode.Dashboards:
			StiToolboxSetup.Dashboards.VisibleComponents.Clear();
			foreach (TreeNode node3 in ((TreeView)tvComponent).Nodes)
			{
				TreeNode val5 = node3;
				foreach (TreeNode node4 in val5.Nodes)
				{
					TreeNode val6 = node4;
					if (val6.Checked)
					{
						string fullName3 = val6.Tag.GetType().FullName;
						StiToolboxSetup.Dashboards.VisibleComponents.Add(fullName3);
					}
				}
			}
			break;
		case StiToolboxMode.ComponentUIs:
			StiToolboxSetup.ComponentUIs.VisibleComponents.Clear();
			foreach (TreeNode node5 in ((TreeView)tvComponent).Nodes)
			{
				TreeNode val3 = node5;
				foreach (TreeNode node6 in val3.Nodes)
				{
					TreeNode val4 = node6;
					if (val4.Checked)
					{
						string fullName2 = val4.Tag.GetType().FullName;
						StiToolboxSetup.ComponentUIs.VisibleComponents.Add(fullName2);
					}
				}
			}
			break;
		case StiToolboxMode.Dialogs:
			StiToolboxSetup.Dialogs.VisibleComponents.Clear();
			foreach (TreeNode node7 in ((TreeView)tvComponent).Nodes)
			{
				TreeNode val = node7;
				foreach (TreeNode node8 in val.Nodes)
				{
					TreeNode val2 = node8;
					if (val2.Checked)
					{
						string fullName = val2.Tag.GetType().FullName;
						StiToolboxSetup.Dialogs.VisibleComponents.Add(fullName);
					}
				}
			}
			break;
		}
		StiToolboxSetup.Save();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/reports_designer_toolbox_shortcut_panel.htm");
	}

	private void TreeViewComponent_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		if (mode == StiToolboxMode.Reports || lockState)
		{
			return;
		}
		lockState = true;
		if (((TreeView)tvComponent).Nodes[0] == e.Node)
		{
			bool @checked = e.Node.Checked;
			foreach (TreeNode node in e.Node.Nodes)
			{
				TreeNode val = node;
				val.Checked = @checked;
			}
		}
		else
		{
			int num = 0;
			foreach (TreeNode node2 in ((TreeView)tvComponent).Nodes[0].Nodes)
			{
				TreeNode val2 = node2;
				if (val2.Checked)
				{
					num++;
				}
			}
			((TreeView)tvComponent).Nodes[0].Checked = num == ((TreeView)tvComponent).Nodes[0].Nodes.Count;
		}
		lockState = false;
	}

	private void CheckBoxShowToolbox_CheckedChanged(object sender, EventArgs e)
	{
		bool @checked = ((CheckBox)checkBoxShowToolbox).Checked;
		bool checked2 = ((CheckBox)checkBoxShowInsertTab).Checked;
		if (!@checked && !checked2)
		{
			((CheckBox)checkBoxShowInsertTab).Checked = true;
		}
	}

	private void CheckBoxShowInsertTab_CheckedChanged(object sender, EventArgs e)
	{
		bool @checked = ((CheckBox)checkBoxShowToolbox).Checked;
		bool checked2 = ((CheckBox)checkBoxShowInsertTab).Checked;
		if (!@checked && !checked2)
		{
			((CheckBox)checkBoxShowToolbox).Checked = true;
		}
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
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		tvComponent = new StiTreeView();
		btCancel = new StiButtonUX();
		btOk = new StiButtonUX();
		checkBoxShowInsertTab = new StiCheckBoxUX();
		checkBoxShowToolbox = new StiCheckBoxUX();
		groupButtonsPanel = new StiButtonPanelUX();
		groupAlignmentPanel = new StiGroupPanelUX();
		((Control)groupButtonsPanel).SuspendLayout();
		((Control)groupAlignmentPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)tvComponent).Anchor = (AnchorStyles)15;
		((TreeView)tvComponent).BorderStyle = (BorderStyle)0;
		((TreeView)tvComponent).CheckBoxes = true;
		((TreeView)tvComponent).HideSelection = false;
		((Control)tvComponent).Location = new Point(24, 23);
		((Control)tvComponent).Margin = new Padding(6);
		((Control)tvComponent).Name = "tvComponent";
		((Control)tvComponent).Size = new Size(540, 599);
		((Control)tvComponent).TabIndex = 0;
		((TreeView)tvComponent).AfterCheck += new TreeViewEventHandler(TreeViewComponent_AfterCheck);
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		btCancel.FlatStyle = (FlatStyle)0;
		((Control)btCancel).Location = new Point(414, 23);
		((Control)btCancel).Margin = new Padding(6);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(150, 48);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.FlatStyle = (FlatStyle)0;
		((Control)btOk).Location = new Point(252, 23);
		((Control)btOk).Margin = new Padding(6);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(150, 48);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)checkBoxShowInsertTab).Anchor = (AnchorStyles)13;
		((Control)checkBoxShowInsertTab).Location = new Point(24, 81);
		((Control)checkBoxShowInsertTab).Margin = new Padding(6);
		((Control)checkBoxShowInsertTab).Name = "checkBoxShowInsertTab";
		((Control)checkBoxShowInsertTab).Size = new Size(540, 46);
		((Control)checkBoxShowInsertTab).TabIndex = 1;
		((Control)checkBoxShowInsertTab).Text = "Show 'Insert Tab'";
		((ButtonBase)checkBoxShowInsertTab).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxShowInsertTab).CheckedChanged += CheckBoxShowInsertTab_CheckedChanged;
		((Control)checkBoxShowToolbox).Anchor = (AnchorStyles)13;
		((Control)checkBoxShowToolbox).Location = new Point(24, 23);
		((Control)checkBoxShowToolbox).Margin = new Padding(6);
		((Control)checkBoxShowToolbox).Name = "checkBoxShowToolbox";
		((Control)checkBoxShowToolbox).Size = new Size(540, 46);
		((Control)checkBoxShowToolbox).TabIndex = 0;
		((Control)checkBoxShowToolbox).Text = "Show Toolbox";
		((ButtonBase)checkBoxShowToolbox).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxShowToolbox).CheckedChanged += CheckBoxShowToolbox_CheckedChanged;
		((Control)groupButtonsPanel).Controls.Add((Control)(object)btCancel);
		((Control)groupButtonsPanel).Controls.Add((Control)(object)btOk);
		((Control)groupButtonsPanel).Dock = (DockStyle)2;
		groupButtonsPanel.LinePosition = StiLinePosition.Top;
		groupButtonsPanel.LineStyle = StiLineStyle.Dot;
		((Control)groupButtonsPanel).Location = new Point(0, 798);
		((Control)groupButtonsPanel).Margin = new Padding(6);
		((Control)groupButtonsPanel).Name = "groupButtonsPanel";
		((Control)groupButtonsPanel).Size = new Size(588, 92);
		((Control)groupButtonsPanel).TabIndex = 2;
		((Control)groupAlignmentPanel).Controls.Add((Control)(object)checkBoxShowInsertTab);
		((Control)groupAlignmentPanel).Controls.Add((Control)(object)checkBoxShowToolbox);
		((Control)groupAlignmentPanel).Dock = (DockStyle)2;
		groupAlignmentPanel.LinePosition = StiLinePosition.Top;
		groupAlignmentPanel.LineStyle = StiLineStyle.Dot;
		((Control)groupAlignmentPanel).Location = new Point(0, 650);
		((Control)groupAlignmentPanel).Margin = new Padding(6);
		((Control)groupAlignmentPanel).Name = "groupAlignmentPanel";
		((Control)groupAlignmentPanel).Size = new Size(588, 148);
		((Control)groupAlignmentPanel).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(588, 890);
		((Control)this).Controls.Add((Control)(object)groupAlignmentPanel);
		((Control)this).Controls.Add((Control)(object)groupButtonsPanel);
		((Control)this).Controls.Add((Control)(object)tvComponent);
		((Form)this).Margin = new Padding(6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiComponentsToolboxForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiComponentsToolboxForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)groupButtonsPanel).ResumeLayout(false);
		((Control)groupAlignmentPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
