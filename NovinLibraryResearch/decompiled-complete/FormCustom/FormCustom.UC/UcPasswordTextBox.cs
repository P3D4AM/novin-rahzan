using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcPasswordTextBox : UserControl
{
	public Func<object, KeyEventArgs, Task> OnKeyDown;

	private IContainer components;

	private TextBox txtPass;

	private PictureBox picStatus;

	private Label lblStrength;

	public string Password
	{
		get
		{
			return ((Control)txtPass).Text;
		}
		set
		{
			((Control)txtPass).Text = value;
		}
	}

	public bool ShowStrength
	{
		get
		{
			return ((Control)lblStrength).Visible;
		}
		set
		{
			((Control)lblStrength).Visible = value;
		}
	}

	public Color ForeColor
	{
		get
		{
			return ((Control)txtPass).ForeColor;
		}
		set
		{
			((Control)txtPass).ForeColor = value;
		}
	}

	public UcPasswordTextBox()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void picStatus_Click(object sender, EventArgs e)
	{
		try
		{
			txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;
			picStatus.Image = (Image)(object)(txtPass.UseSystemPasswordChar ? FormCustom.Properties.Resources.showPass : FormCustom.Properties.Resources.hidePass);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picStatus_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPasswordTextBox.cs", 32);
		}
	}

	private async void txtPass_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (OnKeyDown != null)
			{
				await OnKeyDown(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtPass_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPasswordTextBox.cs", 41);
		}
	}

	private void txtPass_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)lblStrength).Visible)
			{
				EnPasswordScore enPasswordScore = ((Control)txtPass).Text.CheckPasswordStrength();
				((Control)lblStrength).Text = enPasswordScore.GetDisplay().GetTranslate();
				switch (enPasswordScore)
				{
				case EnPasswordScore.Blank:
				case EnPasswordScore.VeryWeak:
				case EnPasswordScore.Weak:
					((Control)lblStrength).ForeColor = Color.Red;
					break;
				case EnPasswordScore.Medium:
					((Control)lblStrength).ForeColor = Color.Orange;
					break;
				case EnPasswordScore.Strong:
				case EnPasswordScore.VeryStrong:
					((Control)lblStrength).ForeColor = Color.Green;
					break;
				default:
					throw new NotImplementedException();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtPass_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPasswordTextBox.cs", 65);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		txtPass = new TextBox();
		picStatus = new PictureBox();
		lblStrength = new Label();
		((ISupportInitialize)picStatus).BeginInit();
		((Control)this).SuspendLayout();
		((Control)txtPass).Anchor = (AnchorStyles)13;
		((Control)txtPass).Location = new Point(118, 2);
		((Control)txtPass).Name = "txtPass";
		((Control)txtPass).Size = new Size(100, 27);
		((Control)txtPass).TabIndex = 0;
		txtPass.TextAlign = (HorizontalAlignment)2;
		txtPass.UseSystemPasswordChar = true;
		((Control)txtPass).TextChanged += txtPass_TextChanged;
		((Control)txtPass).KeyDown += new KeyEventHandler(txtPass_KeyDown);
		((Control)picStatus).Cursor = Cursors.Hand;
		picStatus.Image = (Image)(object)FormCustom.Properties.Resources.showPass;
		((Control)picStatus).Location = new Point(88, 3);
		((Control)picStatus).Name = "picStatus";
		((Control)picStatus).Size = new Size(29, 26);
		picStatus.SizeMode = (PictureBoxSizeMode)1;
		picStatus.TabIndex = 1;
		picStatus.TabStop = false;
		((Control)picStatus).Click += picStatus_Click;
		((Control)lblStrength).Cursor = Cursors.Default;
		((Control)lblStrength).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblStrength).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblStrength).Location = new Point(2, 1);
		((Control)lblStrength).Margin = new Padding(4, 0, 4, 0);
		((Control)lblStrength).Name = "lblStrength";
		((Control)lblStrength).Padding = new Padding(0, 0, 5, 0);
		((Control)lblStrength).Size = new Size(83, 30);
		((Control)lblStrength).TabIndex = 2;
		((Control)lblStrength).Text = "---";
		lblStrength.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)lblStrength);
		((Control)this).Controls.Add((Control)(object)picStatus);
		((Control)this).Controls.Add((Control)(object)txtPass);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcPasswordTextBox";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(220, 31);
		((ISupportInitialize)picStatus).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
