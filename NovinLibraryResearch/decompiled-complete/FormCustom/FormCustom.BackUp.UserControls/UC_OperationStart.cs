using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PacketParser.Entities;

namespace FormCustom.BackUp.UserControls;

public class UC_OperationStart : UserControl
{
	private enOperationStartType _type;

	private enOperationStartStatus _status;

	private IContainer components;

	private PanelEx PanelBazsazi;

	private Label lblBazsazi;

	private PictureBox ImgBazsazi;

	private PictureBox picBackUp;

	private Label label2;

	private PanelEx PanelRecovery;

	public enOperationStartType Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			SetUi(_type);
		}
	}

	public enOperationStartStatus Status
	{
		get
		{
			return _status;
		}
		set
		{
			_status = value;
			if (_status == enOperationStartStatus.End)
			{
				PanelEx panelRecovery = PanelRecovery;
				PanelEx panelBazsazi = PanelBazsazi;
				bool flag2 = (((Control)picBackUp).Visible = false);
				bool visible = (((Control)panelBazsazi).Visible = flag2);
				((Control)panelRecovery).Visible = visible;
			}
			else
			{
				SetUi(_type);
			}
		}
	}

	private void SetUi(enOperationStartType type)
	{
		try
		{
			switch (type)
			{
			case enOperationStartType.BackUp:
			{
				PanelEx panelBazsazi3 = PanelBazsazi;
				bool visible = (((Control)PanelRecovery).Visible = false);
				((Control)panelBazsazi3).Visible = visible;
				((Control)picBackUp).Visible = true;
				break;
			}
			case enOperationStartType.Bazsazi:
			{
				PanelEx panelRecovery = PanelRecovery;
				bool visible = (((Control)picBackUp).Visible = false);
				((Control)panelRecovery).Visible = visible;
				((Control)PanelBazsazi).Visible = true;
				break;
			}
			case enOperationStartType.Restore:
			{
				PanelEx panelBazsazi2 = PanelBazsazi;
				bool visible = (((Control)picBackUp).Visible = false);
				((Control)panelBazsazi2).Visible = visible;
				((Control)PanelRecovery).Visible = true;
				break;
			}
			case enOperationStartType.None:
			{
				PanelEx panelBazsazi = PanelBazsazi;
				PictureBox obj = picBackUp;
				bool flag2 = (((Control)PanelRecovery).Visible = false);
				bool visible = (((Control)obj).Visible = flag2);
				((Control)panelBazsazi).Visible = visible;
				break;
			}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetUi", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\UserControls\\UC_OperationStart.cs", 70);
		}
	}

	public UC_OperationStart()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UC_OperationStart));
		PanelBazsazi = new PanelEx();
		lblBazsazi = new Label();
		ImgBazsazi = new PictureBox();
		picBackUp = new PictureBox();
		label2 = new Label();
		PanelRecovery = new PanelEx();
		((Control)PanelBazsazi).SuspendLayout();
		((ISupportInitialize)ImgBazsazi).BeginInit();
		((ISupportInitialize)picBackUp).BeginInit();
		((Control)PanelRecovery).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelBazsazi.CanvasColor = SystemColors.Control;
		PanelBazsazi.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelBazsazi).Controls.Add((Control)(object)lblBazsazi);
		((Control)PanelBazsazi).Controls.Add((Control)(object)ImgBazsazi);
		PanelBazsazi.DisabledBackColor = Color.Empty;
		((Control)PanelBazsazi).Location = new Point(0, 0);
		((Control)PanelBazsazi).Name = "PanelBazsazi";
		((Control)PanelBazsazi).Size = new Size(91, 107);
		PanelBazsazi.Style.Alignment = (StringAlignment)1;
		PanelBazsazi.Style.BackColor1.Color = Color.White;
		PanelBazsazi.Style.BackColor2.Color = Color.White;
		PanelBazsazi.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelBazsazi.Style.CornerDiameter = 20;
		PanelBazsazi.Style.CornerType = eCornerType.Rounded;
		PanelBazsazi.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelBazsazi.Style.GradientAngle = 90;
		((Control)PanelBazsazi).TabIndex = 19;
		((Control)PanelBazsazi).Visible = false;
		((Control)lblBazsazi).Anchor = (AnchorStyles)13;
		((Control)lblBazsazi).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)lblBazsazi).ForeColor = Color.Maroon;
		((Control)lblBazsazi).Location = new Point(-2, 84);
		((Control)lblBazsazi).Name = "lblBazsazi";
		((Control)lblBazsazi).RightToLeft = (RightToLeft)1;
		((Control)lblBazsazi).Size = new Size(96, 17);
		((Control)lblBazsazi).TabIndex = 15;
		((Control)lblBazsazi).Text = "[بازسازی اطلاعات]";
		lblBazsazi.TextAlign = (ContentAlignment)32;
		((Control)ImgBazsazi).Anchor = (AnchorStyles)15;
		((Control)ImgBazsazi).BackColor = Color.Transparent;
		ImgBazsazi.Image = (Image)componentResourceManager.GetObject("ImgBazsazi.Image");
		((Control)ImgBazsazi).Location = new Point(5, 5);
		((Control)ImgBazsazi).Name = "ImgBazsazi";
		((Control)ImgBazsazi).Size = new Size(81, 67);
		ImgBazsazi.SizeMode = (PictureBoxSizeMode)1;
		ImgBazsazi.TabIndex = 14;
		ImgBazsazi.TabStop = false;
		((Control)picBackUp).BackColor = Color.Transparent;
		((Control)picBackUp).Dock = (DockStyle)5;
		picBackUp.Image = (Image)componentResourceManager.GetObject("picBackUp.Image");
		((Control)picBackUp).Location = new Point(0, 0);
		((Control)picBackUp).Name = "picBackUp";
		((Control)picBackUp).Size = new Size(93, 107);
		picBackUp.SizeMode = (PictureBoxSizeMode)1;
		picBackUp.TabIndex = 16;
		picBackUp.TabStop = false;
		((Control)picBackUp).UseWaitCursor = true;
		((Control)label2).Anchor = (AnchorStyles)13;
		((Control)label2).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)label2).ForeColor = Color.Maroon;
		((Control)label2).Location = new Point(0, 76);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)1;
		((Control)label2).Size = new Size(91, 31);
		((Control)label2).TabIndex = 16;
		((Control)label2).Text = "[در حال بازگردانی]";
		label2.TextAlign = (ContentAlignment)32;
		((Control)label2).UseWaitCursor = true;
		PanelRecovery.CanvasColor = SystemColors.Control;
		PanelRecovery.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelRecovery).Controls.Add((Control)(object)label2);
		PanelRecovery.DisabledBackColor = Color.Empty;
		((Control)PanelRecovery).Location = new Point(1, 0);
		((Control)PanelRecovery).Name = "PanelRecovery";
		((Control)PanelRecovery).Size = new Size(91, 107);
		PanelRecovery.Style.Alignment = (StringAlignment)1;
		PanelRecovery.Style.BackColor1.Color = Color.White;
		PanelRecovery.Style.BackColor2.Color = Color.White;
		PanelRecovery.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelRecovery.Style.CornerDiameter = 20;
		PanelRecovery.Style.CornerType = eCornerType.Rounded;
		PanelRecovery.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelRecovery.Style.GradientAngle = 90;
		((Control)PanelRecovery).TabIndex = 23;
		((Control)PanelRecovery).UseWaitCursor = true;
		((Control)PanelRecovery).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)PanelBazsazi);
		((Control)this).Controls.Add((Control)(object)picBackUp);
		((Control)this).Controls.Add((Control)(object)PanelRecovery);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_OperationStart";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(93, 107);
		((Control)PanelBazsazi).ResumeLayout(false);
		((ISupportInitialize)ImgBazsazi).EndInit();
		((ISupportInitialize)picBackUp).EndInit();
		((Control)PanelRecovery).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
