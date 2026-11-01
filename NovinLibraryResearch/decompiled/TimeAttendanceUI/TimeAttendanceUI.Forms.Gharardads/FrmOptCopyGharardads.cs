using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Settings;
using TimeAttendanceUI.Forms.Reports;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmOptCopyGharardads : FormExtention
{
	private List<GharardadNew> _lstGharardads;

	private Font _font;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX labelX12;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TabItem tabItem2;

	private LabelX labelX2;

	private LabelX labelX4;

	private ColorDialog colorDialog1;

	private RadioButton rbCpoyGharardadForOtherPerson;

	private RadioButton rbCpoyGharardadForNextYear;

	public FrmOptCopyGharardads(string contextGuid, List<GharardadNew> plst, Font font)
		: base(contextGuid, true, true)
	{
		_lstGharardads = plst;
		InitializeComponent();
		_font = font;
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (rbCpoyGharardadForNextYear.Checked)
			{
				await ((CacheBaseForm)new FrmCopyGharardads(((CacheBaseForm)this).ContextGuid, _lstGharardads)).ShowAsync((IWin32Window)(object)this);
			}
			else
			{
				TimeAttendanceSettings settings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
				FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, settings, (ReportFilterEnumType)26);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
			((FormExtention)this).ResetControlChanged();
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmOptCopyGharardads.cs", 50);
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		rbCpoyGharardadForOtherPerson = new RadioButton();
		rbCpoyGharardadForNextYear = new RadioButton();
		labelX4 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		labelX12 = new LabelX();
		labelX2 = new LabelX();
		tabItem2 = new TabItem(components);
		colorDialog1 = new ColorDialog();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)rbCpoyGharardadForOtherPerson);
		((Control)panelEx1).Controls.Add((Control)(object)rbCpoyGharardadForNextYear);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX12);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(721, 309);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 3;
		((Control)rbCpoyGharardadForOtherPerson).BackColor = Color.Transparent;
		((Control)rbCpoyGharardadForOtherPerson).Font = new Font("Tahoma", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbCpoyGharardadForOtherPerson).ForeColor = Color.Maroon;
		((Control)rbCpoyGharardadForOtherPerson).Location = new Point(45, 167);
		((Control)rbCpoyGharardadForOtherPerson).Name = "rbCpoyGharardadForOtherPerson";
		((Control)rbCpoyGharardadForOtherPerson).Size = new Size(645, 30);
		((Control)rbCpoyGharardadForOtherPerson).TabIndex = 13;
		((Control)rbCpoyGharardadForOtherPerson).Text = "کپی برداری از یک قرارداد نمونه بعنوان قرارداد جدید شخص/اشخاص";
		((ButtonBase)rbCpoyGharardadForOtherPerson).UseVisualStyleBackColor = false;
		((Control)rbCpoyGharardadForNextYear).BackColor = Color.Transparent;
		rbCpoyGharardadForNextYear.Checked = true;
		((Control)rbCpoyGharardadForNextYear).Font = new Font("Tahoma", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbCpoyGharardadForNextYear).ForeColor = Color.Maroon;
		((Control)rbCpoyGharardadForNextYear).Location = new Point(64, 63);
		((Control)rbCpoyGharardadForNextYear).Name = "rbCpoyGharardadForNextYear";
		((Control)rbCpoyGharardadForNextYear).Size = new Size(626, 22);
		((Control)rbCpoyGharardadForNextYear).TabIndex = 13;
		rbCpoyGharardadForNextYear.TabStop = true;
		((Control)rbCpoyGharardadForNextYear).Text = "کپی برداری از قرارداد قبلی هر شخص بعنوان قرارداد جدید وی";
		((ButtonBase)rbCpoyGharardadForNextYear).UseVisualStyleBackColor = false;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).ForeColor = Color.Blue;
		((Control)labelX4).Location = new Point(21, 203);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(655, 40);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "جهت ایجاد قرارداد جدید، بایستی در فرم بعدی ابتدا اشخاص مد نظر را انتخاب و سپس قرارداد نمونه  را تعیین نمائید";
		labelX4.WordWrap = true;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(35, 267);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 3;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "لغو   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(141, 267);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 2;
		((Control)BtnAccept).Text = "ثبت  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((Control)labelX12).BackColor = Color.Transparent;
		((BaseItemControl)labelX12).BackgroundStyle.Class = "";
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Font = new Font("Tahoma", 10f, (FontStyle)1);
		((Control)labelX12).ForeColor = Color.Black;
		((Control)labelX12).Location = new Point(12, 9);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(697, 26);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "روش ایجاد قرارداد/قراردادهای جدید را مشخص نمائید";
		labelX12.TextAlignment = (StringAlignment)1;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).ForeColor = Color.Blue;
		((Control)labelX2).Location = new Point(12, 91);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(664, 40);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "ابتدا مطمئن شوید در فرم قبلی نام اشخاصی که برای آنها قصد ایجاد قرارداد جدید همانند قرارداد قبلیشان را دارید انتخاب نموده اید";
		labelX2.WordWrap = true;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "مزایا و کسورات";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(721, 331);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(737, 370);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(737, 370);
		((Control)this).Name = "FrmOptCopyGharardads";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "تمدید قراردادها";
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
