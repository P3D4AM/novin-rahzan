using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Wizards;

public class StiSortZoneControl : StiZoneControl
{
	private IContainer components;

	public StiHitTestVisibleLabel lbSortBy;

	public StiTreeViewMultiBox cbColumn;

	public StiComboBox cbOrder;

	public StiSortZoneControl()
	{
		InitializeComponent();
		DoScaling();
	}

	private void DoScaling()
	{
		cbColumn.FixedHeight = false;
		((Control)cbColumn).Height = ((Control)cbOrder).Height;
		cbColumn.ButtonBitmap2 = StiReportDesignImages.Actions.EditSmall();
	}

	private void lbSortBy_Click(object sender, EventArgs e)
	{
		((Control)this).Focus();
	}

	private void lbSortBy_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSortZoneControl));
		cbColumn = new StiTreeViewMultiBox();
		cbOrder = new StiComboBox();
		lbSortBy = new StiHitTestVisibleLabel();
		((Control)this).SuspendLayout();
		((Control)cbColumn).BackColor = SystemColors.Window;
		cbColumn.ButtonBitmap2 = (Image)componentResourceManager.GetObject("cbColumn.ButtonBitmap2");
		cbColumn.DropDownWidth = 400;
		cbColumn.Flat = false;
		((Control)cbColumn).ForeColor = SystemColors.ControlText;
		cbColumn.Hint = null;
		((Control)cbColumn).Location = new Point(128, 12);
		cbColumn.MaxDropDownHeight = 300;
		((Control)cbColumn).Name = "cbColumn";
		cbColumn.ShowButton2 = true;
		((Control)cbColumn).Size = new Size(200, 21);
		((Control)cbColumn).TabIndex = 0;
		((Control)cbColumn).Text = "StiTreeViewBox1";
		((Control)cbOrder).Anchor = (AnchorStyles)13;
		((Control)cbOrder).BackColor = SystemColors.Window;
		cbOrder.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbOrder).Location = new Point(342, 12);
		((Control)cbOrder).Name = "cbOrder";
		((Control)cbOrder).Size = new Size(125, 21);
		((Control)cbOrder).TabIndex = 1;
		((Control)lbSortBy).BackColor = Color.Transparent;
		((Control)lbSortBy).ForeColor = Color.DimGray;
		((Control)lbSortBy).Location = new Point(12, 12);
		((Control)lbSortBy).Name = "lbSortBy";
		((Control)lbSortBy).Size = new Size(110, 20);
		((Control)lbSortBy).TabIndex = 2;
		((Control)lbSortBy).Text = "Sort by";
		((Label)lbSortBy).TextAlign = (ContentAlignment)16;
		((Control)lbSortBy).Click += lbSortBy_Click;
		((Control)lbSortBy).MouseDown += new MouseEventHandler(lbSortBy_MouseDown);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)lbSortBy);
		((Control)this).Controls.Add((Control)(object)cbOrder);
		((Control)this).Controls.Add((Control)(object)cbColumn);
		((Control)this).Name = "StiSortZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(496, 46);
		((Control)this).ResumeLayout(false);
	}
}
