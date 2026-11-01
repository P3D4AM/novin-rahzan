using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using PacketParser.Entities.Attendance;
using TimeAttendanceDAL.Dal.FLRCheckout;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmCheckOutVacation : FormExtention, ICheckout
{
	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private ICheckout _nextPage;

	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX1;

	internal LabelX labelX3;

	internal LabelX labelX2;

	private RadioButton rbtVacation3;

	private RadioButton rbtVacation2;

	private RadioButton rbtVacation1;

	public FrmCheckOutVacation(string contextGuid, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout, Font font)
		: base(contextGuid, true, true)
	{
		_font = font;
		InitializeComponent();
		_checkout = checkout;
	}

	protected override void OnLoad(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
		((Control)base.statusStrip1).Visible = false;
		FillFrmFields();
		((Control)rbtVacation1).Focus();
	}

	public void FillCheckout()
	{
		if (rbtVacation1.Checked)
		{
			_checkout.VacationType = (VacationTypeEnum)0;
		}
		else if (rbtVacation2.Checked)
		{
			_checkout.VacationType = (VacationTypeEnum)2;
		}
		else
		{
			_checkout.VacationType = (VacationTypeEnum)1;
		}
	}

	public void FillFrmFields()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		VacationTypeEnum vacationType = _checkout.VacationType;
		switch ((int)vacationType)
		{
		case 0:
			rbtVacation1.Checked = true;
			break;
		case 2:
			rbtVacation2.Checked = true;
			break;
		case 1:
			rbtVacation3.Checked = true;
			break;
		}
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		if (fillCheckout)
		{
			FillCheckout();
		}
		return _nextPage ?? (_nextPage = new FrmCheckOutReward(((CacheBaseForm)this).ContextGuid, _font, _checkout));
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		rbtVacation3 = new RadioButton();
		rbtVacation2 = new RadioButton();
		rbtVacation1 = new RadioButton();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 478);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)rbtVacation3);
		((Control)groupPanel4).Controls.Add((Control)(object)rbtVacation2);
		((Control)groupPanel4).Controls.Add((Control)(object)rbtVacation1);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 478);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.Class = "";
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.Class = "";
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.Class = "";
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "محاسبه مانده مرخصی";
		((Control)rbtVacation3).BackColor = Color.Transparent;
		((Control)rbtVacation3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbtVacation3).ForeColor = Color.Navy;
		((Control)rbtVacation3).Location = new Point(269, 247);
		((Control)rbtVacation3).Name = "rbtVacation3";
		((Control)rbtVacation3).Size = new Size(296, 22);
		((Control)rbtVacation3).TabIndex = 2;
		((Control)rbtVacation3).Text = "ابطال کامل مانده مرخصی";
		((ButtonBase)rbtVacation3).UseVisualStyleBackColor = false;
		((Control)rbtVacation2).BackColor = Color.Transparent;
		((Control)rbtVacation2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbtVacation2).ForeColor = Color.Navy;
		((Control)rbtVacation2).Location = new Point(269, 219);
		((Control)rbtVacation2).Name = "rbtVacation2";
		((Control)rbtVacation2).Size = new Size(296, 22);
		((Control)rbtVacation2).TabIndex = 1;
		((Control)rbtVacation2).Text = "بازخرید کامل مانده مرخصی مجاز طبق قرارداد جاری";
		((ButtonBase)rbtVacation2).UseVisualStyleBackColor = false;
		((Control)rbtVacation1).BackColor = Color.Transparent;
		((Control)rbtVacation1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbtVacation1).ForeColor = Color.Navy;
		((Control)rbtVacation1).Location = new Point(269, 191);
		((Control)rbtVacation1).Name = "rbtVacation1";
		((Control)rbtVacation1).Size = new Size(296, 22);
		((Control)rbtVacation1).TabIndex = 0;
		((Control)rbtVacation1).Text = "انتقال کامل مانده مرخصی مجاز طبق قرارداد جاری به قرارداد جدید";
		((ButtonBase)rbtVacation1).UseVisualStyleBackColor = false;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).ForeColor = Color.Black;
		((Control)labelX3).Location = new Point(10, 108);
		((Control)labelX3).Name = "labelX3";
		labelX3.PaddingBottom = 5;
		labelX3.PaddingLeft = 5;
		labelX3.PaddingRight = 5;
		labelX3.PaddingTop = 5;
		((Control)labelX3).Size = new Size(557, 40);
		((Control)labelX3).TabIndex = 7;
		((Control)labelX3).Text = " اجتماعی باشد . شما می توانید نحوه محاسبه مرخصی خود را انتخاب نمایید.";
		labelX3.WordWrap = true;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).ForeColor = Color.Black;
		((Control)labelX2).Location = new Point(8, 74);
		((Control)labelX2).Name = "labelX2";
		labelX2.PaddingBottom = 5;
		labelX2.PaddingLeft = 5;
		labelX2.PaddingRight = 5;
		labelX2.PaddingTop = 5;
		((Control)labelX2).Size = new Size(557, 34);
		((Control)labelX2).TabIndex = 7;
		((Control)labelX2).Text = "و پرداخت نماید ) و یا به قرارداد جدید انتقال داده . این مرخصی ها می تواند طبق قانون اداره کار و تامین";
		labelX2.WordWrap = true;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = Color.Black;
		((Control)labelX1).Location = new Point(10, 38);
		((Control)labelX1).Name = "labelX1";
		labelX1.PaddingBottom = 5;
		labelX1.PaddingLeft = 5;
		labelX1.PaddingRight = 5;
		labelX1.PaddingTop = 5;
		((Control)labelX1).Size = new Size(557, 31);
		((Control)labelX1).TabIndex = 7;
		((Control)labelX1).Text = "کارفرما می تواند مرخصی های استفاده نشده اشخاص را بازخرید ( مبلغ ریالی مانده مرخصی را محاسبه";
		labelX1.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCheckOutVacation";
		((Control)this).Text = "FrmCheckOutVacation";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	bool ICheckout.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void ICheckout.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void ICheckout.Show(IWin32Window owner)
	{
		((Form)this).Show(owner);
	}
}
