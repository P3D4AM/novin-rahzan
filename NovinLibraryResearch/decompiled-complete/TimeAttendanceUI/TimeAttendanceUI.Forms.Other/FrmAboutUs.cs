using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Forms;
using NPResourceManagement;

namespace TimeAttendanceUI.Forms.Other;

public class FrmAboutUs : BaseForm
{
	private IContainer components;

	public FrmAboutUs(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
	}

	private void FRMAboutUs_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)ImageResourceManagement.Hozor_AboutUS;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(600, 408);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(616, 447);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(616, 447);
		((Control)this).Name = "FrmAboutUs";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[درباره ما]";
		((Control)this).KeyDown += new KeyEventHandler(FRMAboutUs_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
