using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design;

public class StiServiceSelectForm : Form
{
	private IContainer components;

	public StiTreeView tvServices;

	private Button btCancel;

	private Button btOk;

	private StiGroupLine stiGroupLine1;

	public StiServiceSelectForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("FormTitles", "ServiceSelectForm");
	}

	private void NodeSetCheck(TreeNode node, bool check)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val = node2;
			val.Checked = check;
			NodeSetCheck(val, check);
		}
	}

	private void StiServiceSelectForm_Load(object sender, EventArgs e)
	{
		((TreeView)tvServices).Nodes[0].ExpandAll();
	}

	private void tvServices_BeforeCheck(object sender, TreeViewCancelEventArgs e)
	{
		NodeSetCheck(e.Node, !e.Node.Checked);
	}

	private void StiServiceSelectForm_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		tvServices = new StiTreeView();
		btCancel = new Button();
		btOk = new Button();
		stiGroupLine1 = new StiGroupLine();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)tvServices).Anchor = (AnchorStyles)15;
		((Control)tvServices).BackColor = Color.White;
		((TreeView)tvServices).BorderStyle = (BorderStyle)0;
		((TreeView)tvServices).CheckBoxes = true;
		((TreeView)tvServices).HideSelection = false;
		((Control)tvServices).Location = new Point(12, 12);
		((Control)tvServices).Name = "tvServices";
		((Control)tvServices).Size = new Size(298, 370);
		((Control)tvServices).TabIndex = 0;
		((TreeView)tvServices).BeforeCheck += new TreeViewCancelEventHandler(tvServices_BeforeCheck);
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(235, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(154, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 399);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(322, 48);
		((Control)stiGroupLine1).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(322, 447);
		((Control)this).Controls.Add((Control)(object)tvServices);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiServiceSelectForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiServiceSelectForm";
		((Form)this).Closed += StiServiceSelectForm_Closed;
		((Form)this).Load += StiServiceSelectForm_Load;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
