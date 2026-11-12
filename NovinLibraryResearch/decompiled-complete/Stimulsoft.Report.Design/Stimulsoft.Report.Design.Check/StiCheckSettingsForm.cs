using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Check;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Check;

public class StiCheckSettingsForm : StiFormUX
{
	private IContainer components;

	private StiButtonUX btOk;

	private ImageList imageList1;

	public StiTreeViewUX tvChecks;

	private TextBox tbCheckInfo;

	private StiButtonUX btCancel;

	private StiButtonPanelUX groupLineBottom;

	private StiGroupPanelUX groupLineInformation;

	public StiCheckSettingsForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			LoadChecks();
			Localize();
			DoTheming();
			DoScaling();
		}
	}

	private void DoTheming()
	{
		((Control)tbCheckInfo).ForeColor = StiUX.InputForeground;
		((Control)tbCheckInfo).BackColor = StiUX.Background;
	}

	private void DoScaling()
	{
		imageList1.ImageSize = new Size(StiScale.I16, StiScale.I16);
		imageList1.Images.Clear();
		imageList1.Images.Add(StiReportDesignImages.Actions.Error());
		imageList1.Images.Add(StiReportDesignImages.Actions.Warning());
		imageList1.Images.Add(StiReportDesignImages.Actions.Information());
		imageList1.Images.Add(StiReportDesignImages.Actions.Check());
		imageList1.Images.Add(StiReportDesignImages.Actions.Properties());
	}

	private void LoadChecks()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		int num = 0;
		int num2 = 1;
		int num3 = 2;
		int num4 = 3;
		((TreeView)tvChecks).ImageList = imageList1;
		List<StiCheck> checks = StiCheckEngine.Checks;
		TreeNode val = new TreeNode
		{
			Checked = true,
			Text = "Checks",
			ImageIndex = num4,
			SelectedImageIndex = num4
		};
		((TreeView)tvChecks).Nodes.Add(val);
		TreeNode val2 = new TreeNode
		{
			Checked = true,
			Text = "Error Messages",
			Tag = "Error Messages",
			ImageIndex = num,
			SelectedImageIndex = num
		};
		val.Nodes.Add(val2);
		TreeNode val3 = new TreeNode
		{
			Checked = true,
			Text = "Warning Messages",
			Tag = "Warning Messages",
			ImageIndex = num2,
			SelectedImageIndex = num2
		};
		val.Nodes.Add(val3);
		TreeNode val4 = new TreeNode
		{
			Checked = true,
			Text = "Information Messages",
			Tag = "Information Messages",
			ImageIndex = num3,
			SelectedImageIndex = num3
		};
		val.Nodes.Add(val4);
		foreach (StiCheck item in checks)
		{
			if (item is StiLicenseTrialCheck)
			{
				item.Enabled = true;
				continue;
			}
			TreeNode val5 = new TreeNode
			{
				Text = item.GetType().Name,
				ImageIndex = num4,
				SelectedImageIndex = num4,
				Tag = item,
				Checked = item.Enabled
			};
			if (item.Status == StiCheckStatus.Error)
			{
				val2.Nodes.Add(val5);
			}
			else if (item.Status == StiCheckStatus.Warning)
			{
				val3.Nodes.Add(val5);
			}
			else if (item.Status == StiCheckStatus.Information)
			{
				val4.Nodes.Add(val5);
			}
		}
		((TreeView)tvChecks).ExpandAll();
	}

	public void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Export", "Settings");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void TreeViewChecks_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		if (e.Node.Tag is StiCheck)
		{
			((StiCheck)e.Node.Tag).Enabled = e.Node.Checked;
			return;
		}
		foreach (TreeNode node in e.Node.Nodes)
		{
			TreeNode val = node;
			val.Checked = e.Node.Checked;
		}
	}

	private void TreeViewChecks_AfterSelect(object sender, TreeViewEventArgs e)
	{
		((Control)tbCheckInfo).Text = ((e.Node.Tag is StiCheck) ? ((StiCheck)e.Node.Tag).LongMessage : string.Empty);
		if (string.IsNullOrWhiteSpace(((Control)tbCheckInfo).Text))
		{
			((Control)groupLineInformation).Visible = false;
			((Control)tvChecks).Height = ((Control)groupLineBottom).Top - ((Control)tvChecks).Top - StiScale.I12;
		}
		else
		{
			((Control)groupLineInformation).Visible = true;
			((Control)tvChecks).Height = ((Control)groupLineInformation).Top - ((Control)tvChecks).Top - StiScale.I12;
		}
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCheckSettingsForm));
		btOk = new StiButtonUX();
		imageList1 = new ImageList(components);
		tvChecks = new StiTreeViewUX();
		tbCheckInfo = new TextBox();
		btCancel = new StiButtonUX();
		groupLineBottom = new StiButtonPanelUX();
		groupLineInformation = new StiGroupPanelUX();
		((Control)groupLineBottom).SuspendLayout();
		((Control)groupLineInformation).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Button)btOk).DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(358, 23);
		((Control)btOk).Margin = new Padding(6, 6, 6, 6);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(150, 44);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Transparent;
		imageList1.Images.SetKeyName(0, "ChecherSettings.png");
		imageList1.Images.SetKeyName(1, "Error.png");
		imageList1.Images.SetKeyName(2, "Message.png");
		imageList1.Images.SetKeyName(3, "Warning.png");
		imageList1.Images.SetKeyName(4, "Check.png");
		((Control)tvChecks).Anchor = (AnchorStyles)15;
		((TreeView)tvChecks).BorderStyle = (BorderStyle)0;
		((TreeView)tvChecks).CheckBoxes = true;
		((TreeView)tvChecks).HideSelection = false;
		((Control)tvChecks).Location = new Point(24, 23);
		((Control)tvChecks).Margin = new Padding(8, 8, 8, 8);
		((Control)tvChecks).Name = "tvChecks";
		((Control)tvChecks).Size = new Size(650, 635);
		((Control)tvChecks).TabIndex = 0;
		((TreeView)tvChecks).AfterCheck += new TreeViewEventHandler(TreeViewChecks_AfterCheck);
		((TreeView)tvChecks).AfterSelect += new TreeViewEventHandler(TreeViewChecks_AfterSelect);
		((Control)tvChecks).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)tbCheckInfo).BackColor = Color.White;
		((TextBoxBase)tbCheckInfo).BorderStyle = (BorderStyle)0;
		((Control)tbCheckInfo).ForeColor = SystemColors.ControlText;
		((Control)tbCheckInfo).Location = new Point(24, 23);
		((Control)tbCheckInfo).Margin = new Padding(6, 6, 6, 6);
		((TextBoxBase)tbCheckInfo).Multiline = true;
		((Control)tbCheckInfo).Name = "tbCheckInfo";
		((TextBoxBase)tbCheckInfo).ReadOnly = true;
		((Control)tbCheckInfo).Size = new Size(646, 119);
		((Control)tbCheckInfo).TabIndex = 1;
		((Control)tbCheckInfo).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(520, 23);
		((Control)btCancel).Margin = new Padding(6, 6, 6, 6);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(150, 44);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)groupLineBottom).BackColor = Color.White;
		((Control)groupLineBottom).Controls.Add((Control)(object)btCancel);
		((Control)groupLineBottom).Controls.Add((Control)(object)btOk);
		((Control)groupLineBottom).Dock = (DockStyle)2;
		groupLineBottom.LinePosition = StiLinePosition.Top;
		groupLineBottom.LineStyle = StiLineStyle.Dot;
		((Control)groupLineBottom).Location = new Point(4, 839);
		((Control)groupLineBottom).Margin = new Padding(6, 6, 6, 6);
		((Control)groupLineBottom).Name = "groupLineBottom";
		((Control)groupLineBottom).Size = new Size(690, 88);
		((Control)groupLineBottom).TabIndex = 2;
		((Control)groupLineInformation).Controls.Add((Control)(object)tbCheckInfo);
		((Control)groupLineInformation).Dock = (DockStyle)2;
		groupLineInformation.LinePosition = StiLinePosition.None;
		groupLineInformation.LineStyle = StiLineStyle.Dot;
		((Control)groupLineInformation).Location = new Point(4, 672);
		((Control)groupLineInformation).Margin = new Padding(6, 6, 6, 6);
		((Control)groupLineInformation).Name = "groupLineInformation";
		((Control)groupLineInformation).Size = new Size(690, 167);
		((Control)groupLineInformation).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(698, 931);
		((Control)this).Controls.Add((Control)(object)groupLineInformation);
		((Control)this).Controls.Add((Control)(object)groupLineBottom);
		((Control)this).Controls.Add((Control)(object)tvChecks);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiCheckSettingsForm";
		((Control)this).Padding = new Padding(4, 4, 4, 4);
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Settings";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)groupLineBottom).ResumeLayout(false);
		((Control)groupLineInformation).ResumeLayout(false);
		((Control)groupLineInformation).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
