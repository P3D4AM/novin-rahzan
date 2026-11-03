using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar;
using EntityCache.Attendance.Business;
using FormCustom.CurrencyTextBox;
using FormCustom.Properties;
using FormCustom.UC;
using FormCustom.UC.Calendar;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData.Calendar;

namespace FormCustom.Calender;

public class frmCalendar : AccountingBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	private DateBoxFa txtDate;

	internal Label labelX3;

	internal Label label1;

	internal TextBox txtMonasebat;

	private CheckBox chbHoliday;

	internal Label Label4;

	internal TextBox txtDescription;

	private PictureBox picAddPatternSms;

	private Panel panel1;

	private Panel panel3;

	private PictureBox pictureBox8;

	private PictureBox pictureBoxDropDone;

	private PictureBox pictureBox4;

	private PictureBox pictureBox3;

	private Label lblPublicInfo;

	private Label lblSmsSetting;

	private PictureBox pictureBox1;

	private UcButtons btnSabt;

	private Panel panel2;

	private FlowLayoutPanel flowLayoutPanel2;

	private Panel pnlPublicInfo;

	private UcGroupBox ucGroupBox1;

	private Panel pnlSmsSetting;

	private FlowLayoutPanel pnlSmsConfigs;

	private UcButtons btnCancel;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public CalenderBusiness CurrentObject { get; private set; }

	public ICalendarAggregator Aggregator { get; private set; }

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 34);
		}
	}

	private async Task SetDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					txtDate.MiladiDate = CurrentObject?.DateM ?? DateTime.Now;
					chbHoliday.Checked = CurrentObject?.STRasmi ?? false;
					((Control)txtMonasebat).Text = CurrentObject?.Monasebat ?? "";
					((Control)txtDescription).Text = CurrentObject?.Description ?? "";
					if (!(CurrentObject?.CalendarSmsMetaData?.CalendarSmsList?.Any()).GetValueOrDefault())
					{
						UcTimeSendSms ucTimeSendSms2 = new UcTimeSendSms();
						((Control)ucTimeSendSms2).Width = ((Control)picAddPatternSms).Width;
						UcTimeSendSms c = ucTimeSendSms2;
						Task.Run(() => c.InitAsync(base.ContextGuid, null, Aggregator));
						((Control)pnlSmsConfigs).Controls.Add((Control)(object)c);
						return;
					}
					foreach (CalendarSms item in CurrentObject.CalendarSmsMetaData.CalendarSmsList)
					{
						UcTimeSendSms ucTimeSendSms3 = new UcTimeSendSms();
						((Control)ucTimeSendSms3).Width = ((Control)picAddPatternSms).Width;
						UcTimeSendSms ucTimeSendSms = ucTimeSendSms3;
						Task.Run(() => ucTimeSendSms.InitAsync(base.ContextGuid, item, Aggregator));
						UcTimeSendSms ucTimeSendSms4 = ucTimeSendSms;
						ucTimeSendSms4.OnDelete = (Func<Guid, Task>)Delegate.Combine(ucTimeSendSms4.OnDelete, new Func<Guid, Task>(UcTimeSendSms_OnDelete));
						((Control)pnlSmsConfigs).Controls.Add((Control)(object)ucTimeSendSms);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 72);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 76);
		}
	}

	public frmCalendar(string contextGuid, CalenderBusiness cls, ICalendarAggregator agg)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			CurrentObject = cls;
			Aggregator = agg;
			Task.Run(() => SetDataAsync());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 88);
		}
	}

	private Task UcTimeSendSms_OnDelete(Guid guid)
	{
		try
		{
			if (guid == Guid.Empty)
			{
				return Task.CompletedTask;
			}
			foreach (UcTimeSendSms c in (ArrangedElementCollection)((Control)pnlSmsConfigs).Controls)
			{
				if (!(c.Guid != guid))
				{
					((Control)pnlSmsConfigs).Controls.Remove((Control)(object)c);
					CalendarSms calendarSms = (CurrentObject?.CalendarSmsMetaData?.CalendarSmsList).Where((CalendarSms q) => q.Guid == c.Guid).SingleOrDefault();
					if (calendarSms != null)
					{
						CurrentObject.CalendarSmsMetaData.CalendarSmsList.Remove(calendarSms);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UcTimeSendSms_OnDelete", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 106);
		}
		return Task.CompletedTask;
	}

	private void frmCalendar_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void btn_cancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btn_cancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 117);
		}
	}

	private async void btnSabt_OnClick(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				if (CurrentObject.Guid == Guid.Empty)
				{
					CurrentObject.Guid = Guid.NewGuid();
					CurrentObject.Status = true;
				}
				if (txtDate.MiladiDate.Date.DayOfWeek == DayOfWeek.Friday)
				{
					chbHoliday.Checked = true;
				}
				CurrentObject.Modified = DateTime.Now;
				CurrentObject.DateM = txtDate.MiladiDate;
				CurrentObject.STRasmi = chbHoliday.Checked;
				CurrentObject.Monasebat = ((Control)txtMonasebat).Text;
				CurrentObject.Description = ((Control)txtDescription).Text;
				CalenderBusiness currentObject = CurrentObject;
				if (currentObject.CalendarSmsMetaData == null)
				{
					currentObject.CalendarSmsMetaData = new CalendarSmsMetaData();
				}
				foreach (object item in (ArrangedElementCollection)((Control)pnlSmsConfigs).Controls)
				{
					if (!(item is UcTimeSendSms ucTimeSendSms))
					{
						continue;
					}
					if (string.IsNullOrEmpty(ucTimeSendSms.TextPattern))
					{
						res.AddError("متن پیامک وارد نشده است", "", "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 145);
						return;
					}
					CurrentObject.CalendarSmsMetaData.AddCalendarSms(new CalendarSms
					{
						TimeTo = CurrentObject.DateM.AddHours(ucTimeSendSms.HourTo).AddMinutes(ucTimeSendSms.MinutesTo),
						TimeFrom = CurrentObject.DateM.AddHours(ucTimeSendSms.HourFrom).AddMinutes(ucTimeSendSms.MinutesFrom),
						SmsPattern = ucTimeSendSms.TextPattern,
						Guid = ucTimeSendSms.Guid,
						CustomerGuidList = ucTimeSendSms.CalendarSms.CustomerGuidList
					});
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CurrentObject.SaveAsync(), ReturnedState.None, "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 157);
				goto end_IL_0032;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 161);
				res.AddReturnedValue(ex, "", "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 162);
				goto end_IL_0032;
			}
			end_IL_0032:;
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void frmCalendar_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					btnSabt.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmCalendar_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 184);
		}
	}

	private void txtMonasebat_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				SendKeys.Send("{Tab}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtMonasebat_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 193);
		}
	}

	private void txtMonasebat_Enter(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.FromArgb(210, 221, 251);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtMonasebat_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 202);
		}
	}

	private void txtMonasebat_Leave(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.White;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtMonasebat_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 211);
		}
	}

	private void picAddPatternSms_Click(object sender, EventArgs e)
	{
		try
		{
			UcTimeSendSms ucTimeSendSms2 = new UcTimeSendSms();
			((Control)ucTimeSendSms2).Width = ((Control)picAddPatternSms).Width;
			UcTimeSendSms ucTimeSendSms = ucTimeSendSms2;
			Task.Run(() => ucTimeSendSms.InitAsync(base.ContextGuid, null, Aggregator));
			UcTimeSendSms ucTimeSendSms3 = ucTimeSendSms;
			ucTimeSendSms3.OnDelete = (Func<Guid, Task>)Delegate.Combine(ucTimeSendSms3.OnDelete, new Func<Guid, Task>(UcTimeSendSms_OnDelete));
			((Control)pnlSmsConfigs).Controls.Add((Control)(object)ucTimeSendSms);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picAddPatternSms_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 222);
		}
	}

	private void lblPublicInfo_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)pnlPublicInfo).Visible = true;
			((Control)pnlSmsSetting).Visible = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblPublicInfo_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 231);
		}
	}

	private void lblSmsSetting_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)pnlPublicInfo).Visible = false;
			((Control)pnlSmsSetting).Visible = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblSmsSetting_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 240);
		}
	}

	private void lblPublicInfo_Enter(object sender, EventArgs e)
	{
		try
		{
			Label val = (Label)((sender is Label) ? sender : null);
			if (val != null)
			{
				((Control)val).ForeColor = Color.FromArgb(65, 81, 220);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblPublicInfo_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 249);
		}
	}

	private void lblPublicInfo_Leave(object sender, EventArgs e)
	{
		try
		{
			Label val = (Label)((sender is Label) ? sender : null);
			if (val != null)
			{
				((Control)val).ForeColor = Color.FromArgb(98, 116, 125);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblPublicInfo_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendar.cs", 258);
		}
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Expected O, but got Unknown
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Expected O, but got Unknown
		//IL_0f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5b: Expected O, but got Unknown
		//IL_0fa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1000: Unknown result type (might be due to invalid IL or missing references)
		//IL_100a: Expected O, but got Unknown
		//IL_127b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1285: Expected O, but got Unknown
		//IL_12a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b2: Expected O, but got Unknown
		//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b0: Expected O, but got Unknown
		//IL_14f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1556: Unknown result type (might be due to invalid IL or missing references)
		//IL_1560: Expected O, but got Unknown
		//IL_15e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f2: Expected O, but got Unknown
		//IL_1605: Unknown result type (might be due to invalid IL or missing references)
		//IL_160f: Expected O, but got Unknown
		//IL_1614: Unknown result type (might be due to invalid IL or missing references)
		//IL_1651: Unknown result type (might be due to invalid IL or missing references)
		//IL_165b: Expected O, but got Unknown
		//IL_1663: Unknown result type (might be due to invalid IL or missing references)
		//IL_166d: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmCalendar));
		txtDate = new DateBoxFa();
		labelX3 = new Label();
		label1 = new Label();
		txtMonasebat = new TextBox();
		chbHoliday = new CheckBox();
		Label4 = new Label();
		txtDescription = new TextBox();
		picAddPatternSms = new PictureBox();
		panel1 = new Panel();
		panel3 = new Panel();
		pictureBox8 = new PictureBox();
		pictureBoxDropDone = new PictureBox();
		pictureBox4 = new PictureBox();
		pictureBox3 = new PictureBox();
		lblPublicInfo = new Label();
		lblSmsSetting = new Label();
		pictureBox1 = new PictureBox();
		btnSabt = new UcButtons();
		panel2 = new Panel();
		flowLayoutPanel2 = new FlowLayoutPanel();
		pnlPublicInfo = new Panel();
		ucGroupBox1 = new UcGroupBox();
		pnlSmsSetting = new Panel();
		pnlSmsConfigs = new FlowLayoutPanel();
		btnCancel = new UcButtons();
		((ISupportInitialize)picAddPatternSms).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)panel3).SuspendLayout();
		((ISupportInitialize)pictureBox8).BeginInit();
		((ISupportInitialize)pictureBoxDropDone).BeginInit();
		((ISupportInitialize)pictureBox4).BeginInit();
		((ISupportInitialize)pictureBox3).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)panel2).SuspendLayout();
		((Control)flowLayoutPanel2).SuspendLayout();
		((Control)pnlPublicInfo).SuspendLayout();
		((Control)pnlSmsSetting).SuspendLayout();
		((Control)pnlSmsConfigs).SuspendLayout();
		((Control)this).SuspendLayout();
		txtDate.AllowNull = false;
		txtDate.BackcolorTooltip = Color.Red;
		txtDate.Border.Class = "TextBoxBorder";
		txtDate.Border.CornerType = eCornerType.Square;
		txtDate.EnterToTab = true;
		txtDate.EscToClose = true;
		txtDate.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		txtDate.FocusBackColor = Color.FromArgb(210, 221, 251);
		txtDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		txtDate.FocusForeColor = Color.Blue;
		txtDate.FocusTextSelect = true;
		((Control)txtDate).Font = new Font("Times New Roman", 12f);
		((Control)txtDate).Location = new Point(297, 37);
		txtDate.MessageEmptyShowDialog = false;
		txtDate.MessegeEmpty = "";
		txtDate.MessegeEmptyInFormRight = true;
		txtDate.MessegeEmptyShowInForm = false;
		txtDate.MiladiDate = new DateTime(2024, 5, 5, 16, 11, 24, 394);
		((Control)txtDate).Name = "txtDate";
		((Control)txtDate).RightToLeft = (RightToLeft)0;
		txtDate.ShowNameOfDayOnToolTip = false;
		txtDate.ShowToolTipText = false;
		((Control)txtDate).Size = new Size(113, 26);
		((Control)txtDate).TabIndex = 1;
		((Control)txtDate).Text = "5/5/2024 1";
		((TextBox)txtDate).TextAlign = (HorizontalAlignment)2;
		txtDate.TootTipDelay = 500;
		((Control)txtDate).KeyDown += new KeyEventHandler(txtMonasebat_KeyDown);
		((Control)labelX3).AutoSize = true;
		((Control)labelX3).BackColor = Color.FromArgb(249, 250, 255);
		((Control)labelX3).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)labelX3).Location = new Point(440, 41);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(47, 20);
		((Control)labelX3).TabIndex = 184;
		((Control)labelX3).Text = "[تاریخ]:";
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label1).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)label1).Location = new Point(428, 71);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(59, 20);
		((Control)label1).TabIndex = 184;
		((Control)label1).Text = "[مناسبت]:";
		((Control)txtMonasebat).Font = new Font("B Yekan", 9.75f);
		((Control)txtMonasebat).Location = new Point(20, 68);
		((Control)txtMonasebat).Name = "txtMonasebat";
		((Control)txtMonasebat).Size = new Size(390, 27);
		((Control)txtMonasebat).TabIndex = 3;
		((Control)txtMonasebat).Enter += txtMonasebat_Enter;
		((Control)txtMonasebat).KeyDown += new KeyEventHandler(txtMonasebat_KeyDown);
		((Control)txtMonasebat).Leave += txtMonasebat_Leave;
		((Control)chbHoliday).AutoSize = true;
		((Control)chbHoliday).BackColor = Color.FromArgb(249, 250, 255);
		((Control)chbHoliday).Cursor = Cursors.Hand;
		((Control)chbHoliday).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)chbHoliday).Location = new Point(200, 40);
		((Control)chbHoliday).Name = "chbHoliday";
		((Control)chbHoliday).Size = new Size(67, 24);
		((Control)chbHoliday).TabIndex = 2;
		((Control)chbHoliday).Text = "[تعطیل]";
		((ButtonBase)chbHoliday).UseVisualStyleBackColor = false;
		((Control)Label4).AutoSize = true;
		((Control)Label4).BackColor = Color.FromArgb(249, 250, 255);
		((Control)Label4).Font = new Font("B Yekan", 9.75f);
		((Control)Label4).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)Label4).Location = new Point(416, 105);
		((Control)Label4).Name = "Label4";
		((Control)Label4).Size = new Size(71, 20);
		((Control)Label4).TabIndex = 188;
		((Control)Label4).Text = "[توضیحات]:";
		Label4.TextAlign = (ContentAlignment)16;
		((Control)txtDescription).Font = new Font("B Yekan", 9.75f);
		((Control)txtDescription).Location = new Point(20, 102);
		((TextBoxBase)txtDescription).Multiline = true;
		((Control)txtDescription).Name = "txtDescription";
		txtDescription.ScrollBars = (ScrollBars)2;
		((Control)txtDescription).Size = new Size(390, 182);
		((Control)txtDescription).TabIndex = 4;
		((Control)txtDescription).Enter += txtMonasebat_Enter;
		((Control)txtDescription).KeyDown += new KeyEventHandler(txtMonasebat_KeyDown);
		((Control)txtDescription).Leave += txtMonasebat_Leave;
		((Control)picAddPatternSms).BackColor = Color.Transparent;
		((Control)picAddPatternSms).Cursor = Cursors.Hand;
		picAddPatternSms.Image = (Image)(object)FormCustom.Properties.Resources.Border2;
		((Control)picAddPatternSms).Location = new Point(29, 3);
		((Control)picAddPatternSms).Name = "picAddPatternSms";
		((Control)picAddPatternSms).Size = new Size(480, 45);
		picAddPatternSms.SizeMode = (PictureBoxSizeMode)1;
		picAddPatternSms.TabIndex = 190;
		picAddPatternSms.TabStop = false;
		((Control)picAddPatternSms).Click += picAddPatternSms_Click;
		((Control)panel1).BackgroundImage = (Image)(object)FormCustom.Properties.Resources._41;
		((Control)panel1).Controls.Add((Control)(object)panel3);
		((Control)panel1).Controls.Add((Control)(object)pictureBox1);
		((Control)panel1).Location = new Point(550, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(236, 565);
		((Control)panel1).TabIndex = 192;
		((Control)panel3).Anchor = (AnchorStyles)15;
		((Control)panel3).BackColor = Color.Transparent;
		((Control)panel3).Controls.Add((Control)(object)pictureBox8);
		((Control)panel3).Controls.Add((Control)(object)pictureBoxDropDone);
		((Control)panel3).Controls.Add((Control)(object)pictureBox4);
		((Control)panel3).Controls.Add((Control)(object)pictureBox3);
		((Control)panel3).Controls.Add((Control)(object)lblPublicInfo);
		((Control)panel3).Controls.Add((Control)(object)lblSmsSetting);
		((Control)panel3).Location = new Point(4, 184);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(227, 369);
		((Control)panel3).TabIndex = 1;
		((Control)pictureBox8).BackgroundImageLayout = (ImageLayout)0;
		((Control)pictureBox8).Cursor = Cursors.Hand;
		pictureBox8.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_11;
		((Control)pictureBox8).Location = new Point(7, 73);
		((Control)pictureBox8).Margin = new Padding(15);
		((Control)pictureBox8).Name = "pictureBox8";
		((Control)pictureBox8).Size = new Size(17, 17);
		pictureBox8.SizeMode = (PictureBoxSizeMode)1;
		pictureBox8.TabIndex = 4;
		pictureBox8.TabStop = false;
		((Control)pictureBox8).Click += lblSmsSetting_Click;
		((Control)pictureBoxDropDone).BackgroundImageLayout = (ImageLayout)0;
		((Control)pictureBoxDropDone).Cursor = Cursors.Hand;
		pictureBoxDropDone.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_11;
		((Control)pictureBoxDropDone).Location = new Point(7, 32);
		((Control)pictureBoxDropDone).Margin = new Padding(15);
		((Control)pictureBoxDropDone).Name = "pictureBoxDropDone";
		((Control)pictureBoxDropDone).Size = new Size(17, 17);
		pictureBoxDropDone.SizeMode = (PictureBoxSizeMode)1;
		pictureBoxDropDone.TabIndex = 3;
		pictureBoxDropDone.TabStop = false;
		((Control)pictureBoxDropDone).Click += lblPublicInfo_Click;
		((Control)pictureBox4).Anchor = (AnchorStyles)9;
		((Control)pictureBox4).Cursor = Cursors.Hand;
		pictureBox4.Image = (Image)(object)FormCustom.Properties.Resources.Tell;
		((Control)pictureBox4).Location = new Point(182, 60);
		((Control)pictureBox4).Name = "pictureBox4";
		((Control)pictureBox4).Size = new Size(31, 40);
		pictureBox4.SizeMode = (PictureBoxSizeMode)4;
		pictureBox4.TabIndex = 1;
		pictureBox4.TabStop = false;
		((Control)pictureBox4).Click += lblSmsSetting_Click;
		((Control)pictureBox3).Anchor = (AnchorStyles)9;
		((Control)pictureBox3).Cursor = Cursors.Hand;
		pictureBox3.Image = (Image)(object)FormCustom.Properties.Resources.PublicInfoAccess;
		((Control)pictureBox3).Location = new Point(182, 20);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(31, 40);
		pictureBox3.SizeMode = (PictureBoxSizeMode)4;
		pictureBox3.TabIndex = 1;
		pictureBox3.TabStop = false;
		((Control)pictureBox3).Click += lblPublicInfo_Click;
		((Control)lblPublicInfo).Anchor = (AnchorStyles)13;
		((Control)lblPublicInfo).Cursor = Cursors.Hand;
		((Control)lblPublicInfo).Font = new Font("B Yekan", 12.25f, (FontStyle)1);
		((Control)lblPublicInfo).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblPublicInfo).Location = new Point(22, 29);
		((Control)lblPublicInfo).Name = "lblPublicInfo";
		((Control)lblPublicInfo).Size = new Size(154, 26);
		((Control)lblPublicInfo).TabIndex = 0;
		((Control)lblPublicInfo).Text = "[مشخصـات عمومی]";
		lblPublicInfo.TextAlign = (ContentAlignment)16;
		((Control)lblPublicInfo).Click += lblPublicInfo_Click;
		((Control)lblPublicInfo).MouseEnter += lblPublicInfo_Enter;
		((Control)lblPublicInfo).MouseLeave += lblPublicInfo_Leave;
		((Control)lblSmsSetting).Anchor = (AnchorStyles)13;
		((Control)lblSmsSetting).Cursor = Cursors.Hand;
		((Control)lblSmsSetting).Font = new Font("B Yekan", 12.25f, (FontStyle)1);
		((Control)lblSmsSetting).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblSmsSetting).Location = new Point(22, 70);
		((Control)lblSmsSetting).Name = "lblSmsSetting";
		((Control)lblSmsSetting).Size = new Size(154, 26);
		((Control)lblSmsSetting).TabIndex = 0;
		((Control)lblSmsSetting).Text = "[تنظیمات پیامک]";
		lblSmsSetting.TextAlign = (ContentAlignment)16;
		((Control)lblSmsSetting).Click += lblSmsSetting_Click;
		((Control)lblSmsSetting).MouseEnter += lblPublicInfo_Enter;
		((Control)lblSmsSetting).MouseLeave += lblPublicInfo_Leave;
		((Control)pictureBox1).Anchor = (AnchorStyles)13;
		((Control)pictureBox1).BackColor = Color.Transparent;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.Logo55;
		((Control)pictureBox1).Location = new Point(4, 44);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(226, 88);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)btnSabt).Anchor = (AnchorStyles)6;
		((Control)btnSabt).BackColor = Color.Transparent;
		btnSabt.CornerType = EnButtonCornerType.Round;
		((Control)btnSabt).Cursor = Cursors.Hand;
		btnSabt.DisplayText = "[تایید](F5)";
		((Control)btnSabt).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSabt.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnSabt.IsShowMoreDetails = false;
		((Control)btnSabt).Location = new Point(185, 526);
		((Control)btnSabt).Margin = new Padding(4, 5, 4, 5);
		((Control)btnSabt).Name = "btnSabt";
		((Control)btnSabt).RightToLeft = (RightToLeft)1;
		((Control)btnSabt).Size = new Size(284, 30);
		((Control)btnSabt).TabIndex = 2;
		btnSabt.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSabt.Type = EnButtonType.Bold;
		btnSabt.OnClick += btnSabt_OnClick;
		((Control)panel2).BackColor = Color.White;
		((Control)panel2).Controls.Add((Control)(object)flowLayoutPanel2);
		((Control)panel2).Controls.Add((Control)(object)btnSabt);
		((Control)panel2).Controls.Add((Control)(object)btnCancel);
		((Control)panel2).Location = new Point(2, 1);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(548, 564);
		((Control)panel2).TabIndex = 193;
		((ScrollableControl)flowLayoutPanel2).AutoScroll = true;
		((Control)flowLayoutPanel2).Controls.Add((Control)(object)pnlPublicInfo);
		((Control)flowLayoutPanel2).Controls.Add((Control)(object)pnlSmsSetting);
		flowLayoutPanel2.FlowDirection = (FlowDirection)1;
		((Control)flowLayoutPanel2).Location = new Point(3, 4);
		((Control)flowLayoutPanel2).Name = "flowLayoutPanel2";
		((Control)flowLayoutPanel2).Size = new Size(538, 511);
		((Control)flowLayoutPanel2).TabIndex = 0;
		flowLayoutPanel2.WrapContents = false;
		((Control)pnlPublicInfo).Controls.Add((Control)(object)txtDescription);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)Label4);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)txtMonasebat);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)label1);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)chbHoliday);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)txtDate);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)labelX3);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)ucGroupBox1);
		((Control)pnlPublicInfo).Location = new Point(6, 3);
		((Control)pnlPublicInfo).Name = "pnlPublicInfo";
		((Control)pnlPublicInfo).Size = new Size(512, 498);
		((Control)pnlPublicInfo).TabIndex = 0;
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("B Yekan", 12.25f);
		((Control)ucGroupBox1).Dock = (DockStyle)5;
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 30;
		ucGroupBox1.IsShowHeader = true;
		((Control)ucGroupBox1).Location = new Point(0, 0);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(512, 498);
		((Control)ucGroupBox1).TabIndex = 0;
		ucGroupBox1.Title = "[مشخصات عمومی]";
		((Control)pnlSmsSetting).Controls.Add((Control)(object)pnlSmsConfigs);
		((Control)pnlSmsSetting).Location = new Point(6, 507);
		((Control)pnlSmsSetting).Name = "pnlSmsSetting";
		((Control)pnlSmsSetting).Size = new Size(512, 498);
		((Control)pnlSmsSetting).TabIndex = 221;
		((Control)pnlSmsSetting).Visible = false;
		((ScrollableControl)pnlSmsConfigs).AutoScroll = true;
		((Control)pnlSmsConfigs).Controls.Add((Control)(object)picAddPatternSms);
		((Control)pnlSmsConfigs).Dock = (DockStyle)5;
		pnlSmsConfigs.FlowDirection = (FlowDirection)1;
		((Control)pnlSmsConfigs).Location = new Point(0, 0);
		((Control)pnlSmsConfigs).Name = "pnlSmsConfigs";
		((Control)pnlSmsConfigs).Size = new Size(512, 498);
		((Control)pnlSmsConfigs).TabIndex = 228;
		pnlSmsConfigs.WrapContents = false;
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = EnButtonCornerType.Round;
		((Control)btnCancel).Cursor = Cursors.Hand;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(26, 526);
		((Control)btnCancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(156, 30);
		((Control)btnCancel).TabIndex = 154;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btn_cancel_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(786, 565);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmCalendar";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[مدیریت مناسبت ها]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmCalendar_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmCalendar_KeyDown);
		((ISupportInitialize)picAddPatternSms).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)panel3).ResumeLayout(false);
		((ISupportInitialize)pictureBox8).EndInit();
		((ISupportInitialize)pictureBoxDropDone).EndInit();
		((ISupportInitialize)pictureBox4).EndInit();
		((ISupportInitialize)pictureBox3).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)panel2).ResumeLayout(false);
		((Control)flowLayoutPanel2).ResumeLayout(false);
		((Control)pnlPublicInfo).ResumeLayout(false);
		((Control)pnlPublicInfo).PerformLayout();
		((Control)pnlSmsSetting).ResumeLayout(false);
		((Control)pnlSmsConfigs).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
