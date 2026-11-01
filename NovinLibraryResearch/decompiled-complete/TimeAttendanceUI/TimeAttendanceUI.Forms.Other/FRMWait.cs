using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Forms;

namespace TimeAttendanceUI.Forms.Other;

public class FRMWait : BaseForm
{
	private IContainer components;

	internal PanelEx PanelEx1;

	private Label label1;

	private Label label2;

	public FRMWait(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		label1 = new Label();
		label2 = new Label();
		((Control)PanelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)label1);
		((Control)PanelEx1).Controls.Add((Control)(object)label2);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(327, 151);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 5;
		((Control)label1).Anchor = (AnchorStyles)13;
		((Control)label1).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.Crimson;
		((Control)label1).Location = new Point(26, 75);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(217, 47);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = "[لطفا کمی صبر کنید . . .]";
		((Control)label2).Anchor = (AnchorStyles)13;
		((Control)label2).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label2).ForeColor = Color.Crimson;
		((Control)label2).Location = new Point(-13, 28);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(337, 47);
		((Control)label2).TabIndex = 3;
		((Control)label2).Text = "[سیستم در حال بارگذاری اطلاعات می باشد]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(327, 151);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FRMWait";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMWait";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
