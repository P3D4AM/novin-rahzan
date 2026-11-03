using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PayamakUi.Forms.FRMOthers;

public class FRMAbout : Form
{
	private IContainer components;

	public FRMAbout()
	{
		InitializeComponent();
	}

	private void FRMAbout_KeyDown(object sender, KeyEventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void FRMAbout_MouseClick(object sender, MouseEventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FRMAbout));
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)componentResourceManager.GetObject("$this.BackgroundImage");
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(484, 400);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMAbout";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMAbout";
		((Control)this).KeyDown += new KeyEventHandler(FRMAbout_KeyDown);
		((Control)this).MouseClick += new MouseEventHandler(FRMAbout_MouseClick);
		((Control)this).ResumeLayout(false);
	}
}
