using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Forms;

namespace TimeAttendanceUI.Forms.Other;

public class FrmCallUs : BaseForm
{
	private IContainer components;

	private PictureBox pictureBox1;

	public FrmCallUs(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
	}

	private void FRMCallUs_KeyDown(object sender, KeyEventArgs e)
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmCallUs));
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)pictureBox1).Dock = (DockStyle)5;
		pictureBox1.Image = (Image)componentResourceManager.GetObject("pictureBox1.Image");
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(1012, 716);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 11;
		pictureBox1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(1012, 716);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FrmCallUs";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تماس با ما]";
		((Control)this).KeyDown += new KeyEventHandler(FRMCallUs_KeyDown);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
