using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace FKAttendDllCSSample;

public class FrmCheckDevices : Form
{
	private IContainer components = null;

	private ButtonX BtnHysoon;

	private ButtonX BtnEbio;

	public FrmCheckDevices()
	{
		InitializeComponent();
	}

	private void BtnHysoon_Click(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			FrmCheckDeviceHysoon frmCheckDeviceHysoon = new FrmCheckDeviceHysoon();
			((Form)frmCheckDeviceHysoon).ShowDialog();
		}
		catch (Exception)
		{
		}
	}

	private void BtnEbio_Click(object sender, EventArgs e)
	{
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
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		BtnHysoon = new ButtonX();
		BtnEbio = new ButtonX();
		((Control)this).SuspendLayout();
		((Control)BtnHysoon).AccessibleRole = (AccessibleRole)43;
		((Control)BtnHysoon).Anchor = (AnchorStyles)15;
		BtnHysoon.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnHysoon).Location = new Point(12, 29);
		((Control)BtnHysoon).Name = "BtnHysoon";
		((Control)BtnHysoon).Size = new Size(251, 34);
		BtnHysoon.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnHysoon).TabIndex = 0;
		((Control)BtnHysoon).Text = "بررسي دستگاههاي سري 567 ، 587 ،566 ، 586";
		((Control)BtnHysoon).Click += BtnHysoon_Click;
		((Control)BtnEbio).AccessibleRole = (AccessibleRole)43;
		((Control)BtnEbio).Anchor = (AnchorStyles)15;
		BtnEbio.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnEbio).Location = new Point(12, 74);
		((Control)BtnEbio).Name = "BtnEbio";
		((Control)BtnEbio).Size = new Size(251, 34);
		BtnEbio.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnEbio).TabIndex = 1;
		((Control)BtnEbio).Text = "بررسي دستگاههاي سري 261 ، 2815";
		((Control)BtnEbio).Click += BtnEbio_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(274, 145);
		((Control)this).Controls.Add((Control)(object)BtnEbio);
		((Control)this).Controls.Add((Control)(object)BtnHysoon);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(290, 184);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(290, 184);
		((Control)this).Name = "FrmCheckDevices";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "فرم بررسي دستگاهها";
		((Control)this).ResumeLayout(false);
	}
}
