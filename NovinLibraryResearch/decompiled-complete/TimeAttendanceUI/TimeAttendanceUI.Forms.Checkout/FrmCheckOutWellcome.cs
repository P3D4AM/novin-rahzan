using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmCheckOutWellcome : FormExtention, ICheckout
{
	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private ICheckout _nextPage;

	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX lblDescShiftSelect;

	internal LabelX labelX1;

	internal LabelX labelX4;

	internal LabelX labelX3;

	internal LabelX labelX2;

	public FrmCheckOutWellcome(string contextGuid, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout, Font font)
		: base(contextGuid, true, true)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_checkout = checkout;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((FormExtention)this).OnLoad(e);
			((Control)base.statusStrip1).Visible = false;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public void FillCheckout()
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		try
		{
			return _nextPage ?? (_nextPage = new FrmCheckoutBackup(((CacheBaseForm)this).ContextGuid, _checkout, _font));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
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
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Expected O, but got Unknown
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		labelX1 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		lblDescShiftSelect = new LabelX();
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
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel4).Controls.Add((Control)(object)lblDescShiftSelect);
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
		((Control)groupPanel4).Text = "به عملیات تسویه حساب کارکنان خوش آمدید";
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = Color.Firebrick;
		((Control)labelX1).Location = new Point(10, 60);
		((Control)labelX1).Name = "labelX1";
		labelX1.PaddingBottom = 5;
		labelX1.PaddingLeft = 5;
		labelX1.PaddingRight = 5;
		labelX1.PaddingTop = 5;
		((Control)labelX1).Size = new Size(557, 34);
		((Control)labelX1).TabIndex = 7;
		((Control)labelX1).Text = "لطفا قبل از شروع عملیات به نکات زیر توجه نمایید :";
		labelX1.WordWrap = true;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(9, 235);
		((Control)labelX4).Name = "labelX4";
		labelX4.PaddingBottom = 5;
		labelX4.PaddingLeft = 5;
		labelX4.PaddingRight = 5;
		labelX4.PaddingTop = 5;
		((Control)labelX4).Size = new Size(557, 35);
		((Control)labelX4).TabIndex = 7;
		((Control)labelX4).Text = " مانند پایان شرکت و انتقال قرادادها با شرایط حقوق و مزایای جدید به شرکت جدید.";
		labelX4.WordWrap = true;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(9, 202);
		((Control)labelX3).Name = "labelX3";
		labelX3.PaddingBottom = 5;
		labelX3.PaddingLeft = 5;
		labelX3.PaddingRight = 5;
		labelX3.PaddingTop = 5;
		((Control)labelX3).Size = new Size(557, 37);
		((Control)labelX3).TabIndex = 7;
		((Control)labelX3).Text = " یا قصد داریم قراردادهای جاری شخص یا اشخاص را  تعیین وضعیت و به قرارداد جدید منتقل کنیم.";
		labelX3.WordWrap = true;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(9, 153);
		((Control)labelX2).Name = "labelX2";
		labelX2.PaddingBottom = 5;
		labelX2.PaddingLeft = 5;
		labelX2.PaddingRight = 5;
		labelX2.PaddingTop = 5;
		((Control)labelX2).Size = new Size(557, 37);
		((Control)labelX2).TabIndex = 7;
		((Control)labelX2).Text = " خود با شرکت یا مجموعه را داشته و در نظر داریم حقوق و مزایا را تعیین وضعیت و تسویه نمائیم.";
		labelX2.WordWrap = true;
		((Control)lblDescShiftSelect).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblDescShiftSelect).BackgroundStyle.Class = "";
		((BaseItemControl)lblDescShiftSelect).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblDescShiftSelect).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblDescShiftSelect).Location = new Point(17, 122);
		((Control)lblDescShiftSelect).Name = "lblDescShiftSelect";
		lblDescShiftSelect.PaddingBottom = 5;
		lblDescShiftSelect.PaddingLeft = 5;
		lblDescShiftSelect.PaddingRight = 5;
		lblDescShiftSelect.PaddingTop = 5;
		((Control)lblDescShiftSelect).Size = new Size(540, 34);
		((Control)lblDescShiftSelect).TabIndex = 7;
		((Control)lblDescShiftSelect).Text = "عملیات تسویه قرارداد اشخاص زمانی انجام می گیرد که شخص یا اشخاص قصد اتمام همکاری";
		lblDescShiftSelect.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCheckOutWellcome";
		((Control)this).Text = "FrmCheckOutWellcome";
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
