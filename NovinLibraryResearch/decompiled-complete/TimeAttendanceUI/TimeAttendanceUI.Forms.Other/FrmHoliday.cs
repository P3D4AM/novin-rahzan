using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Other;

public class FrmHoliday : CacheBaseForm
{
	private readonly Holiday _cls;

	private string frm_Text = "[افزودن تاریخ]";

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	internal PanelEx PanelEx2;

	private TextBox TxtDesc;

	private TextBox TxtMonasebat;

	internal DateBoxFa FarsiDateDate;

	internal LabelX LabelX2;

	internal LabelX LabelX3;

	internal LabelX LabelX4;

	internal PictureBox PictureBox1;

	private CheckBoxX ChbTatili;

	public FrmHoliday(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
		try
		{
			((Control)FarsiDateDate).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now);
			frm_Text = "[افزودن تاریخ]";
			_cls = new Holiday(((CacheBaseForm)this).ContextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 30);
		}
	}

	public FrmHoliday(string contextGuid, Holiday holiday, DateTime pDateM)
		: base(contextGuid)
	{
		InitializeComponent();
		try
		{
			((Control)this).Text = "ویرایش تاریخ";
			frm_Text = "[ویرایش تاریخ]";
			_cls = holiday;
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 43);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationHolidays)
		{
			((Form)this).Close();
		}
	}

	private void FillForm()
	{
		try
		{
			((Control)FarsiDateDate).Text = Calendar.MiladiToCalture(_cls.DateM);
			((Control)TxtDesc).Text = _cls.Description;
			((Control)TxtMonasebat).Text = _cls.Monasebat;
			ChbTatili.Checked = _cls.StRasmi;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 63);
		}
	}

	private void GetData()
	{
		try
		{
			_cls.DateM = FarsiDateDate.MiladiDate;
			_cls.Description = Text.FixString(((Control)TxtDesc).Text.Trim());
			_cls.Monasebat = Text.FixString(((Control)TxtMonasebat).Text.Trim());
			_cls.StRasmi = ChbTatili.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 77);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Visible || !((Control)BtnAccept).Enabled)
		{
			return;
		}
		try
		{
			GetData();
			if (_cls.StRasmi && ((Control)TxtMonasebat).Text == "")
			{
				Utility.MessageBoxShow("[لطفا متن مناسبت را وارد کنید ]", ((CacheBaseForm)this).ContextGuid);
				((Control)TxtMonasebat).Focus();
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await _cls.SaveAsync(token);
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 106);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 119);
		}
	}

	private void FRMHoliday_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 116)
					{
						e.Handled = true;
						((PopupItemControl)BtnAccept).PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMHoliday_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 146);
		}
	}

	private void FrmHoliday_Load(object sender, EventArgs e)
	{
		try
		{
			frm_Text = Utilities.GetTranslate(frm_Text);
			((Control)this).Text = frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmHoliday_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHoliday.cs", 159);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((CacheBaseForm)this).Dispose(disposing);
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Expected O, but got Unknown
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Expected O, but got Unknown
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Expected O, but got Unknown
		//IL_0a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0e: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		PanelEx2 = new PanelEx();
		ChbTatili = new CheckBoxX();
		PictureBox1 = new PictureBox();
		TxtDesc = new TextBox();
		TxtMonasebat = new TextBox();
		FarsiDateDate = new DateBoxFa();
		LabelX2 = new LabelX();
		LabelX3 = new LabelX();
		LabelX4 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((ISupportInitialize)PictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)PanelEx2);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(351, 261);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)ChbTatili);
		((Control)PanelEx2).Controls.Add((Control)(object)PictureBox1);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtDesc);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtMonasebat);
		((Control)PanelEx2).Controls.Add((Control)(object)FarsiDateDate);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX3);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX4);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(7, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(337, 199);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.BorderWidth = 3;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((BaseItemControl)ChbTatili).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbTatili).Location = new Point(147, 71);
		((Control)ChbTatili).Name = "ChbTatili";
		((Control)ChbTatili).Size = new Size(86, 23);
		((BaseItemControl)ChbTatili).Style = (eDotNetBarStyle)9;
		((Control)ChbTatili).TabIndex = 11;
		((Control)ChbTatili).Text = "[تعطیلی]";
		((Control)PictureBox1).Anchor = (AnchorStyles)9;
		PictureBox1.BorderStyle = (BorderStyle)1;
		PictureBox1.Image = (Image)(object)Resources.calendar_today;
		((Control)PictureBox1).Location = new Point(287, 10);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(47, 52);
		PictureBox1.SizeMode = (PictureBoxSizeMode)3;
		PictureBox1.TabIndex = 10;
		PictureBox1.TabStop = false;
		((Control)TxtDesc).Location = new Point(13, 100);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(220, 88);
		((Control)TxtDesc).TabIndex = 7;
		((Control)TxtMonasebat).Location = new Point(13, 45);
		((Control)TxtMonasebat).Name = "TxtMonasebat";
		((Control)TxtMonasebat).Size = new Size(220, 20);
		((Control)TxtMonasebat).TabIndex = 5;
		FarsiDateDate.AllowNull = false;
		((Control)FarsiDateDate).Anchor = (AnchorStyles)9;
		FarsiDateDate.BackcolorTooltip = Color.Red;
		((TextBoxX)FarsiDateDate).Border.CornerType = (eCornerType)1;
		FarsiDateDate.EnterToTab = true;
		FarsiDateDate.EscToClose = true;
		FarsiDateDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		FarsiDateDate.FocusBackColor = Color.Yellow;
		FarsiDateDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		FarsiDateDate.FocusForeColor = Color.Blue;
		FarsiDateDate.FocusTextSelect = true;
		((Control)FarsiDateDate).Font = new Font("Arial", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)FarsiDateDate).Location = new Point(143, 10);
		FarsiDateDate.MessageEmptyShowDialog = false;
		FarsiDateDate.MessegeEmpty = "";
		FarsiDateDate.MessegeEmptyInFormRight = true;
		FarsiDateDate.MessegeEmptyShowInForm = false;
		FarsiDateDate.MiladiDate = new DateTime(2021, 12, 6, 11, 2, 44, 27);
		((Control)FarsiDateDate).Name = "FarsiDateDate";
		((Control)FarsiDateDate).RightToLeft = (RightToLeft)0;
		((Control)FarsiDateDate).Size = new Size(89, 23);
		((Control)FarsiDateDate).TabIndex = 1;
		((Control)FarsiDateDate).Text = "06/12/2021";
		((TextBox)FarsiDateDate).TextAlign = (HorizontalAlignment)2;
		FarsiDateDate.TootTipDelay = 500;
		((Control)LabelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX2).Location = new Point(237, 10);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).Size = new Size(31, 23);
		((Control)LabelX2).TabIndex = 0;
		((Control)LabelX2).Text = "[تاریخ]";
		LabelX2.TextAlignment = (StringAlignment)2;
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX3).Location = new Point(237, 45);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).Size = new Size(45, 23);
		((Control)LabelX3).TabIndex = 4;
		((Control)LabelX3).Text = "[مناسبت] :";
		LabelX3.TextAlignment = (StringAlignment)2;
		((Control)LabelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX4).Location = new Point(237, 100);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).Size = new Size(72, 23);
		((Control)LabelX4).TabIndex = 6;
		((Control)LabelX4).Text = "[شرح]:";
		LabelX4.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(7, 216);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(84, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو] (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(101, 216);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(84, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ذخیره] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(351, 261);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(367, 300);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(367, 300);
		((Control)this).Name = "FrmHoliday";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).Load += FrmHoliday_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMHoliday_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((Control)PanelEx2).PerformLayout();
		((ISupportInitialize)PictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
