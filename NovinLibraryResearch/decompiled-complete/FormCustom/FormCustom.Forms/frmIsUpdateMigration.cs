using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Forms;

public class frmIsUpdateMigration : AccountingBaseForm, IDbMigrator
{
	private IWin32Window _owner;

	private IContainer components;

	private Label label1;

	private Button btnYes;

	private Button btnNo;

	private PictureBox pictureBox1;

	public frmIsUpdateMigration(IWin32Window owner)
	{
		InitializeComponent();
		_owner = owner;
	}

	public bool IsMigrationAllowed()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		try
		{
			return (int)((Form)this).ShowDialog(_owner) == 6;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsMigrationAllowed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmIsUpdateMigration.cs", 27);
			return false;
		}
	}

	private void btnYes_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)6;
		((Form)this).Close();
	}

	private void btnNo_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)7;
		((Form)this).Close();
	}

	private void frmIsUpdateMigration_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)btnNo).Focus();
			SystemSounds.Hand.Play();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmIsUpdateMigration_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmIsUpdateMigration.cs", 53);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		label1 = new Label();
		btnYes = new Button();
		btnNo = new Button();
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).Location = new Point(11, 33);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(352, 16);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[یک بروزرسانی در جداول دیتابیس موجود است مایل به اجرای آن هستید؟]";
		label1.TextAlign = (ContentAlignment)64;
		((Control)btnYes).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((ButtonBase)btnYes).Image = (Image)(object)FormCustom.Properties.Resources.accept;
		((ButtonBase)btnYes).ImageAlign = (ContentAlignment)64;
		((Control)btnYes).Location = new Point(210, 83);
		((Control)btnYes).Name = "btnYes";
		((Control)btnYes).Size = new Size(113, 39);
		((Control)btnYes).TabIndex = 1;
		((Control)btnYes).Text = "[بله]";
		((ButtonBase)btnYes).TextAlign = (ContentAlignment)16;
		((ButtonBase)btnYes).TextImageRelation = (TextImageRelation)4;
		((ButtonBase)btnYes).UseVisualStyleBackColor = true;
		((Control)btnYes).Click += btnYes_Click;
		((ButtonBase)btnNo).Image = (Image)(object)FormCustom.Properties.Resources.edit_delete;
		((ButtonBase)btnNo).ImageAlign = (ContentAlignment)64;
		((Control)btnNo).Location = new Point(91, 83);
		((Control)btnNo).Name = "btnNo";
		((Control)btnNo).Size = new Size(113, 39);
		((Control)btnNo).TabIndex = 1;
		((Control)btnNo).Text = "[خیر]";
		((ButtonBase)btnNo).TextAlign = (ContentAlignment)16;
		((ButtonBase)btnNo).TextImageRelation = (TextImageRelation)4;
		((ButtonBase)btnNo).UseVisualStyleBackColor = true;
		((Control)btnNo).Click += btnNo_Click;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.icons8_warning_50;
		((Control)pictureBox1).Location = new Point(369, 23);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(36, 37);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 2;
		pictureBox1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.LightSkyBlue;
		((Form)this).ClientSize = new Size(409, 125);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)btnNo);
		((Control)this).Controls.Add((Control)(object)btnYes);
		((Control)this).Controls.Add((Control)(object)label1);
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmIsUpdateMigration";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[اخطار]";
		((Form)this).Load += frmIsUpdateMigration_Load;
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
