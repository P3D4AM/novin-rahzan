using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI;

public class FrmManagment_Login : BaseForm
{
	private IContainer components;

	private BindingSource userAttendanceBindingSource;

	internal PanelEx PanelEx1;

	internal GroupPanel GroupPanel1;

	internal ButtonX BtnExit;

	internal ButtonX BtnEnter;

	internal TextBoxX TxtPASS;

	private PictureBox pictureBox1;

	public FrmManagment_Login(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		((Control)TxtPASS).Focus();
	}

	private async void BtnEnter_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		((Control)this).Hide();
		((Form)this).Close();
		FRMManagment fRMManagment = new FRMManagment(((CacheBaseForm)this).ContextGuid);
		((Form)fRMManagment).ShowDialog();
		((Component)(object)fRMManagment).Dispose();
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void FRMManagment_Login_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((PopupItemControl)BtnExit).PerformClick();
		}
		else if ((int)e.KeyCode == 13)
		{
			((PopupItemControl)BtnEnter).PerformClick();
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Expected O, but got Unknown
		components = new Container();
		userAttendanceBindingSource = new BindingSource(components);
		PanelEx1 = new PanelEx();
		pictureBox1 = new PictureBox();
		GroupPanel1 = new GroupPanel();
		BtnExit = new ButtonX();
		BtnEnter = new ButtonX();
		TxtPASS = new TextBoxX();
		((ISupportInitialize)userAttendanceBindingSource).BeginInit();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnExit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEnter);
		((Control)PanelEx1).Controls.Add((Control)(object)TxtPASS);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(237, 113);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.BorderWidth = 5;
		PanelEx1.Style.CornerDiameter = 10;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		pictureBox1.Image = (Image)(object)Resources.user;
		((Control)pictureBox1).Location = new Point(146, 3);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(91, 110);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 5;
		pictureBox1.TabStop = false;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)GroupPanel1).Location = new Point(12, 7);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(140, 28);
		((PanelControl)GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BorderBottom = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GroupPanel1).Style.BorderLeft = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.BorderRight = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.BorderTop = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextColor = Color.Black;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 1;
		((Control)GroupPanel1).Text = "         [دسترسی ویژه]";
		GroupPanel1.TitleImage = (Image)(object)Resources.passwordAnbar1;
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExit).Anchor = (AnchorStyles)6;
		BtnExit.ColorTable = (eButtonColor)3;
		((Control)BtnExit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnExit.Image = (Image)(object)Resources.stop1;
		BtnExit.ImagePosition = (eImagePosition)1;
		((Control)BtnExit).Location = new Point(12, 67);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Size = new Size(62, 34);
		((PopupItemControl)BtnExit).Style = (eDotNetBarStyle)9;
		((Control)BtnExit).TabIndex = 4;
		((Control)BtnExit).TabStop = false;
		((Control)BtnExit).Text = "[خروج]";
		BtnExit.Tooltip = "[خروج]";
		((Control)BtnExit).Click += BtnExit_Click;
		((Control)BtnEnter).AccessibleRole = (AccessibleRole)43;
		((Control)BtnEnter).Anchor = (AnchorStyles)6;
		BtnEnter.ColorTable = (eButtonColor)3;
		((Control)BtnEnter).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEnter.Image = (Image)(object)Resources.accept;
		BtnEnter.ImagePosition = (eImagePosition)1;
		((Control)BtnEnter).Location = new Point(78, 67);
		((Control)BtnEnter).Name = "BtnEnter";
		((Control)BtnEnter).Size = new Size(62, 34);
		((PopupItemControl)BtnEnter).Style = (eDotNetBarStyle)9;
		((Control)BtnEnter).TabIndex = 3;
		((Control)BtnEnter).TabStop = false;
		((Control)BtnEnter).Text = "[ورود]";
		((Control)BtnEnter).Click += BtnEnter_Click;
		TxtPASS.Border.Class = "TextBoxBorder";
		TxtPASS.Border.CornerType = (eCornerType)1;
		((Control)TxtPASS).Location = new Point(12, 41);
		((Control)TxtPASS).Name = "TxtPASS";
		((TextBox)TxtPASS).PasswordChar = '●';
		((Control)TxtPASS).Size = new Size(128, 21);
		((Control)TxtPASS).TabIndex = 2;
		((TextBox)TxtPASS).UseSystemPasswordChar = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(237, 113);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmManagment_Login";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).TopMost = true;
		((Control)this).KeyDown += new KeyEventHandler(FRMManagment_Login_KeyDown);
		((ISupportInitialize)userAttendanceBindingSource).EndInit();
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
