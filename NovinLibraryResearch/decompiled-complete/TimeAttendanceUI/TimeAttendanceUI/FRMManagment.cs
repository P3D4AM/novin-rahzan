using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FKAttendDllCSSample;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using SBXPCSampleCSharp;
using Serilog;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI;

public class FRMManagment : BaseForm
{
	private IContainer components;

	private ButtonX BtnCalcEnteghalToNextDate;

	private PanelEx panelEx1;

	private ButtonX BtnDeleteRepeatedDatas;

	private ButtonX btnEbioDevices;

	private ButtonX btnHysoonDevices;

	private ButtonX BtnResetAccessLeverUserId1;

	public FRMManagment(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
	}

	private async void BtnCalcEnteghalToNextDate_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			CancellationToken token = default(CancellationToken);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[SP_LogoNPersonel_Update_MoveToNextDate]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				((DbCommand)val).CommandTimeout = 10000;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[ساعتهای انتقال به روز بعد بروزرسانی شد.]", "", 3);
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCalcEnteghalToNextDate_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMManagment.cs", 40);
		}
	}

	private async void BtnDeleteRepeatedDatas_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			CancellationToken token = default(CancellationToken);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[SP_LogONPersonel_delete_repeatedTaradods]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				((DbCommand)val).CommandTimeout = 30000;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[تردد های تکراری که یک شخص در طول یک دقیقه انجام داده حذف شد.]", "", 3);
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDeleteRepeatedDatas_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMManagment.cs", 60);
		}
	}

	private void BtnEbioDevices_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string path = Path.Combine(Application.StartupPath, "datEnrollDat.mdb");
			if (!File.Exists(path))
			{
				File.WriteAllBytes(path, Resources.datEnrollDat);
			}
			((Form)new FrmConnectDevice()).Show((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			Utility.MessageBoxShowError(ex.Message, ((CacheBaseForm)this).ContextGuid);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEbioDevices_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMManagment.cs", 76);
		}
	}

	private void BtnHysoonDevices_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Form)new FrmConnectDeviceHysoon()).Show((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnHysoonDevices_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMManagment.cs", 87);
		}
	}

	private async void BtnResetAccessLeverUserId1_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			CancellationToken token = default(CancellationToken);
			if ((int)FMessageBox.Show("آیا مطمئن هستید که میخواهید دسترسی کاربران را ریست کنید؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation) == 7)
			{
				return;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Reset_UserPermission]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				((DbCommand)val).CommandTimeout = 30000;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "دسترسی تمام کاربران ریست شد .\r\n رمز پیش فرض یک عدد کاراکتر فضای خالی می باشد", "", 3);
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Expected O, but got Unknown
		BtnCalcEnteghalToNextDate = new ButtonX();
		panelEx1 = new PanelEx();
		BtnResetAccessLeverUserId1 = new ButtonX();
		btnHysoonDevices = new ButtonX();
		btnEbioDevices = new ButtonX();
		BtnDeleteRepeatedDatas = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)BtnCalcEnteghalToNextDate).AccessibleRole = (AccessibleRole)43;
		BtnCalcEnteghalToNextDate.ColorTable = (eButtonColor)3;
		((Control)BtnCalcEnteghalToNextDate).Location = new Point(14, 12);
		((Control)BtnCalcEnteghalToNextDate).Name = "BtnCalcEnteghalToNextDate";
		((Control)BtnCalcEnteghalToNextDate).Size = new Size(313, 23);
		((PopupItemControl)BtnCalcEnteghalToNextDate).Style = (eDotNetBarStyle)9;
		((Control)BtnCalcEnteghalToNextDate).TabIndex = 0;
		((Control)BtnCalcEnteghalToNextDate).Text = "[محاسبه مجدد انتقال به روز بعد]";
		((Control)BtnCalcEnteghalToNextDate).Click += BtnCalcEnteghalToNextDate_Click;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnResetAccessLeverUserId1);
		((Control)panelEx1).Controls.Add((Control)(object)btnHysoonDevices);
		((Control)panelEx1).Controls.Add((Control)(object)btnEbioDevices);
		((Control)panelEx1).Controls.Add((Control)(object)BtnDeleteRepeatedDatas);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCalcEnteghalToNextDate);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(341, 273);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 2;
		((Control)BtnResetAccessLeverUserId1).AccessibleRole = (AccessibleRole)43;
		BtnResetAccessLeverUserId1.ColorTable = (eButtonColor)3;
		((Control)BtnResetAccessLeverUserId1).Location = new Point(12, 128);
		((Control)BtnResetAccessLeverUserId1).Name = "BtnResetAccessLeverUserId1";
		((Control)BtnResetAccessLeverUserId1).RightToLeft = (RightToLeft)1;
		((Control)BtnResetAccessLeverUserId1).Size = new Size(313, 23);
		((PopupItemControl)BtnResetAccessLeverUserId1).Style = (eDotNetBarStyle)9;
		((Control)BtnResetAccessLeverUserId1).TabIndex = 5;
		((Control)BtnResetAccessLeverUserId1).Text = "بازنشانی سطح دسترسی کاربران";
		((Control)BtnResetAccessLeverUserId1).Click += BtnResetAccessLeverUserId1_Click;
		((Control)btnHysoonDevices).AccessibleRole = (AccessibleRole)43;
		btnHysoonDevices.ColorTable = (eButtonColor)3;
		((Control)btnHysoonDevices).Location = new Point(12, 99);
		((Control)btnHysoonDevices).Name = "btnHysoonDevices";
		((Control)btnHysoonDevices).RightToLeft = (RightToLeft)1;
		((Control)btnHysoonDevices).Size = new Size(313, 23);
		((PopupItemControl)btnHysoonDevices).Style = (eDotNetBarStyle)9;
		((Control)btnHysoonDevices).TabIndex = 3;
		((Control)btnHysoonDevices).Text = "[ارتباط با دستگاه های] NP395,NP586";
		((Control)btnHysoonDevices).Click += BtnHysoonDevices_Click;
		((Control)btnEbioDevices).AccessibleRole = (AccessibleRole)43;
		btnEbioDevices.ColorTable = (eButtonColor)3;
		((Control)btnEbioDevices).Location = new Point(14, 70);
		((Control)btnEbioDevices).Name = "btnEbioDevices";
		((Control)btnEbioDevices).RightToLeft = (RightToLeft)1;
		((Control)btnEbioDevices).Size = new Size(313, 23);
		((PopupItemControl)btnEbioDevices).Style = (eDotNetBarStyle)9;
		((Control)btnEbioDevices).TabIndex = 2;
		((Control)btnEbioDevices).Text = "[ارتباط با دستگاه های]  NP2815,NP261,NP302 ";
		((Control)btnEbioDevices).Click += BtnEbioDevices_Click;
		((Control)BtnDeleteRepeatedDatas).AccessibleRole = (AccessibleRole)43;
		BtnDeleteRepeatedDatas.ColorTable = (eButtonColor)3;
		((Control)BtnDeleteRepeatedDatas).Location = new Point(14, 41);
		((Control)BtnDeleteRepeatedDatas).Name = "BtnDeleteRepeatedDatas";
		((Control)BtnDeleteRepeatedDatas).Size = new Size(313, 23);
		((PopupItemControl)BtnDeleteRepeatedDatas).Style = (eDotNetBarStyle)9;
		((Control)BtnDeleteRepeatedDatas).TabIndex = 1;
		((Control)BtnDeleteRepeatedDatas).Text = "[حذف تردد های تکرار که در یک دقیقه انجام شده است]";
		((Control)BtnDeleteRepeatedDatas).Click += BtnDeleteRepeatedDatas_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(341, 273);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Name = "FRMManagment";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[مدیریت ویژه]";
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
