using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Entities;
using TimeAttendanceDAL;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Forms.Bimeh;
using TimeAttendanceUI.Forms.Device;
using TimeAttendanceUI.Forms.Gharardads;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Forms.Taradod;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms;
using TimeAttendanceUI.Salary.Shift;

namespace TimeAttendanceUI.Forms.User;

public class FrmLogUser : FormExtention
{
	private Font _font;

	private readonly LogUsers _clsUsers;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TabItem tabItem2;

	private LabelX labelX5;

	private LabelX labelX4;

	private LabelX labelX3;

	private LabelX labelX2;

	private LabelX labelX1;

	private TextBoxX TxtUserName;

	private TextBoxX txtTime;

	private TextBoxX txtDate;

	private TextBoxX txtDesc;

	private TextBoxX txtTitle;

	private ButtonX btnDisplayDetail;

	public FrmLogUser(string contextGuid, LogUsers logUsers, Font font)
		: base(contextGuid, true, true)
	{
		try
		{
			_font = font;
			InitializeComponent();
			MainFormOwner.owner = (ILkUiFactory)(object)this;
			_clsUsers = logUsers;
			((Control)TxtUserName).Text = _clsUsers.UserName;
			((Control)txtDate).Text = Calendar.MiladiToCalture(_clsUsers.DateM);
			((Control)txtTime).Text = _clsUsers.TheTime;
			((Control)txtTitle).Text = _clsUsers.DescActionTypeName;
			((Control)txtDesc).Text = _clsUsers.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUser.cs", 45);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		try
		{
			((FormExtention)this).OnLoad(e);
			((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUser.cs", 72);
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUser.cs", 82);
		}
	}

	private async void btnDisplayDetail_Click(object sender, EventArgs e)
	{
		_ = 6;
		try
		{
			switch (_clsUsers.ActionTypeName)
			{
			case "Personnel.PersonnelInsert":
			case "Personnel.PersonnelDelete":
			case "Personnel.PersonnelUpdate":
				if (_clsUsers.LogData != "")
				{
					TimeAttendanceDAL.Dal.FLRPerson.Person person = _clsUsers.Deserialize<TimeAttendanceDAL.Dal.FLRPerson.Person>();
					await ((CacheBaseForm)new FrmPersonel(((CacheBaseForm)this).ContextGuid, person, _font, isReadOnly: true)).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case "Gharardad.GharardadInsert":
			case "Gharardad.GharardadDelete":
			case "Gharardad.GharardadUpdate":
				if (_clsUsers.LogData != "")
				{
					GharardadNew gharardad = _clsUsers.Deserialize<GharardadNew>();
					await ((CacheBaseForm)new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, gharardad)).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case "BillPrice.BillPriceInsert":
			case "BillPrice.BillPriceDelete":
			case "BillPrice.BillPriceUpdate":
				if (_clsUsers.LogData != "")
				{
					Fish pFish = _clsUsers.Deserialize<Fish>();
					await ((CacheBaseForm)new FrmFishHoghogh(((CacheBaseForm)this).ContextGuid, pFish, _font, isReadOnly: true)).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case "Vacation.VacationDelete":
			case "Vacation.VacationInsert":
			case "Vacation.VacationUpdate":
				if (_clsUsers.LogData != "")
				{
					Morakhasidaily pMorakhasidaily = _clsUsers.Deserialize<Morakhasidaily>();
					await ((CacheBaseForm)new FrmMorakhasiDaily(((CacheBaseForm)this).ContextGuid, pMorakhasidaily, readOnly: true)).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case "Company.CompanyDelete":
			case "Company.CompanyInsert":
			case "Company.CompanyUpdate":
				if (_clsUsers.LogData != "")
				{
					Company pCompany = _clsUsers.Deserialize<Company>();
					await ((CacheBaseForm)new FrmCompany(((CacheBaseForm)this).ContextGuid, pCompany)).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case "Shift.ShiftDelete":
			case "Shift.ShiftInsert":
			case "Shift.ShiftUpdate":
				if (_clsUsers.LogData != "")
				{
					Shifts shifts = _clsUsers.Deserialize<Shifts>();
					shifts.ContextGuid = _clsUsers.ContextGuid;
					await ((CacheBaseForm)new FrmShift(((CacheBaseForm)this).ContextGuid, shifts, _font, isReadOnly: true)).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case "Device.DeviceDelete":
			case "Device.DeviceInsert":
			case "Device.DeviceUpdate":
				if (_clsUsers.LogData != "")
				{
					DeviceInfo pDeviceInfo = _clsUsers.Deserialize<DeviceInfo>();
					await ((CacheBaseForm)new FrmDevice(((CacheBaseForm)this).ContextGuid, pDeviceInfo)).ShowAsync((IWin32Window)(object)this);
				}
				break;
			default:
				Utility.MessageBoxShow("[جهت این نوع لاگ ها ، ریز اطلاعات وجود ندارد]", ((CacheBaseForm)this).ContextGuid);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnDisplayDetail_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmLogUser.cs", 170);
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_0bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be8: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		txtTime = new TextBoxX();
		txtDate = new TextBoxX();
		txtDesc = new TextBoxX();
		txtTitle = new TextBoxX();
		TxtUserName = new TextBoxX();
		BtnCancel = new ButtonX();
		btnDisplayDetail = new ButtonX();
		BtnAccept = new ButtonX();
		labelX5 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		tabItem2 = new TabItem(components);
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)txtTime);
		((Control)panelEx1).Controls.Add((Control)(object)txtDate);
		((Control)panelEx1).Controls.Add((Control)(object)txtDesc);
		((Control)panelEx1).Controls.Add((Control)(object)txtTitle);
		((Control)panelEx1).Controls.Add((Control)(object)TxtUserName);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)btnDisplayDetail);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(750, 379);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		txtTime.Border.Class = "TextBoxBorder";
		txtTime.Border.CornerType = (eCornerType)1;
		((Control)txtTime).Location = new Point(15, 41);
		((TextBoxBase)txtTime).MaxLength = 100;
		((Control)txtTime).Name = "txtTime";
		((Control)txtTime).Size = new Size(297, 21);
		((Control)txtTime).TabIndex = 5;
		txtDate.Border.Class = "TextBoxBorder";
		txtDate.Border.CornerType = (eCornerType)1;
		((Control)txtDate).Location = new Point(367, 41);
		((TextBoxBase)txtDate).MaxLength = 100;
		((Control)txtDate).Name = "txtDate";
		((Control)txtDate).Size = new Size(297, 21);
		((Control)txtDate).TabIndex = 3;
		txtDesc.Border.Class = "TextBoxBorder";
		txtDesc.Border.CornerType = (eCornerType)1;
		((Control)txtDesc).Location = new Point(15, 98);
		((TextBoxBase)txtDesc).MaxLength = 100;
		((TextBoxBase)txtDesc).Multiline = true;
		((Control)txtDesc).Name = "txtDesc";
		((Control)txtDesc).Size = new Size(649, 230);
		((Control)txtDesc).TabIndex = 9;
		txtTitle.Border.Class = "TextBoxBorder";
		txtTitle.Border.CornerType = (eCornerType)1;
		((Control)txtTitle).Location = new Point(15, 69);
		((TextBoxBase)txtTitle).MaxLength = 100;
		((Control)txtTitle).Name = "txtTitle";
		((Control)txtTitle).Size = new Size(649, 21);
		((Control)txtTitle).TabIndex = 7;
		TxtUserName.Border.Class = "TextBoxBorder";
		TxtUserName.Border.CornerType = (eCornerType)1;
		((Control)TxtUserName).Location = new Point(15, 14);
		((TextBoxBase)TxtUserName).MaxLength = 100;
		((Control)TxtUserName).Name = "TxtUserName";
		((Control)TxtUserName).Size = new Size(650, 21);
		((Control)TxtUserName).TabIndex = 1;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 334);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 11;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "لغو   (Esc)";
		((Control)btnDisplayDetail).AccessibleRole = (AccessibleRole)43;
		((Control)btnDisplayDetail).Anchor = (AnchorStyles)6;
		btnDisplayDetail.ColorTable = (eButtonColor)6;
		((Control)btnDisplayDetail).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnDisplayDetail).ForeColor = Color.Black;
		btnDisplayDetail.Image = (Image)(object)Resources.search2;
		btnDisplayDetail.ImagePosition = (eImagePosition)1;
		((Control)btnDisplayDetail).Location = new Point(255, 334);
		((Control)btnDisplayDetail).Name = "btnDisplayDetail";
		((Control)btnDisplayDetail).RightToLeft = (RightToLeft)1;
		btnDisplayDetail.Shortcuts.Add((eShortcut)116);
		((Control)btnDisplayDetail).Size = new Size(191, 34);
		((PopupItemControl)btnDisplayDetail).Style = (eDotNetBarStyle)9;
		((Control)btnDisplayDetail).TabIndex = 10;
		((Control)btnDisplayDetail).Text = "مشاهده ریز اطلاعات";
		((Control)btnDisplayDetail).Click += btnDisplayDetail_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(138, 334);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 10;
		((Control)BtnAccept).Text = "ثبت  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(672, 98);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(75, 20);
		((Control)labelX5).TabIndex = 8;
		((Control)labelX5).Text = "شرح :";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(672, 69);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(75, 20);
		((Control)labelX4).TabIndex = 6;
		((Control)labelX4).Text = "عنوان :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(320, 41);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(75, 20);
		((Control)labelX3).TabIndex = 4;
		((Control)labelX3).Text = "[زمان] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(672, 41);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(75, 20);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "تاریخ :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(672, 14);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(75, 20);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "نام کاربر :";
		labelX1.TextAlignment = (StringAlignment)2;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "مزایا و کسورات";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(750, 401);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(766, 439);
		((Control)this).Name = "FrmLogUser";
		((Form)this).ShowInTaskbar = true;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "لاگ عملکرد کاربر";
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
