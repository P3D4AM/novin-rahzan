using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.UC;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftProgrammable : FormExtention
{
	private GharardadNew ClsGharardad;

	private List<UCMonth> UCMonthlst = new List<UCMonth>();

	private bool isEmptyBefore;

	private bool isHelpDisplayed;

	private DateTime StartDate;

	private DateTime EndDate;

	private CancellationTokenSource InitializeBtncancellationToken = new CancellationTokenSource();

	private CancellationTokenSource TokenFrm = new CancellationTokenSource();

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private Font _font;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX lblGharardadNo;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private FlowLayoutPanel flowLayoutPanel1;

	private LabelX labelX39;

	private FlowLayoutPanel flowLayoutPanel2;

	private LabelX labelX5;

	private LabelX labelX3;

	private LabelX labelX1;

	private LabelX lblEndDate;

	private LabelX lblStartDate;

	private LabelX lblFullName;

	internal ButtonX BtnDelete;

	internal ButtonX BtnSelectGroup;

	internal ButtonX BtnExit;

	public static List<ShiftCodeing> ShiftCodingLst { get; set; }

	private bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
			}
			return true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 63);
		}
	}

	public FrmShiftProgrammable(string contextGuid, Font font, int gharardadId, DateTime startDate, DateTime endDate)
		: base(contextGuid, true, true)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			StartDate = startDate;
			EndDate = endDate;
			((Control)BtnAccept).Enabled = false;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(gharardadId, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 84);
		}
	}

	private async Task LoadDataAsync(int gharardadId, CancellationToken token)
	{
		try
		{
			ClsGharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, gharardadId, token);
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						((Control)lblGharardadNo).Text = ClsGharardad.No;
						((Control)lblFullName).Text = ClsGharardad.FullName;
						((Control)lblStartDate).Text = Calendar.MiladiToCalture(StartDate);
						((Control)lblEndDate).Text = Calendar.MiladiToCalture(EndDate);
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
			((Control)this).DoubleBuffered = true;
			if (!token.IsCancellationRequested && !IsDisposed)
			{
				await InitializeBtnsAsync(token);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 128);
		}
	}

	private async Task InitializeBtnsAsync(CancellationToken token)
	{
		_ = 8;
		try
		{
			List<ShiftProgrammableDet> list = await ShiftProgrammableDet.SelectRowByGharardadIDAsync(((CacheBaseForm)this).ContextGuid, ClsGharardad.ID, token);
			List<ShiftDetView> shiftDetViews = new List<ShiftDetView>();
			foreach (ShiftProgrammableDet item in list)
			{
				List<ShiftDetView> list2 = shiftDetViews;
				list2.Add(await ShiftDetView.ConvertToShiftDetViewAsync(((CacheBaseForm)this).ContextGuid, item, token));
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			bool isFirst = false;
			if (shiftDetViews.Count <= 0)
			{
				isFirst = true;
				isEmptyBefore = true;
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[برای قرارداد شیفت برنامه ریزی شده ای تعریف نشده است. ابتدا بایستی یکی از شیفت های موجود را انتخاب نموده سپس روی آن برنامه ریزی را انجام دهید. آیا مایل به ادامه هستید؟]"), "", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Question, FMessageBox.FMessageBoxDefaultButton.Button1) != 6)
				{
					while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
					{
						await Task.Delay(100);
					}
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					((Control)this).Invoke((Delegate)(Action)delegate
					{
						try
						{
							((Control)BtnAccept).Enabled = false;
						}
						catch (Exception ex6)
						{
							WebErrorLog.ErrorLogInstance.StartLog(ex6, "", "InitializeBtnsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 226);
						}
					});
					((Form)this).DialogResult = (DialogResult)2;
					return;
				}
				FrmShiftMain frmShiftMain = new FrmShiftMain(((CacheBaseForm)this).ContextGuid, _font, Select: true);
				DialogResult dialogResult = ((Form)frmShiftMain).ShowDialog();
				Shifts selectedShift = await frmShiftMain.GetSelectedShiftAsync(token);
				if ((int)dialogResult == 1)
				{
					if (selectedShift == null)
					{
						Utility.MessageBoxShowWarning("[قبل از تعیین شیفت برنامه ریزی بایستی از منوی شیفت، حداقل یک شیفت کاری تعریف شده باشد]", ((CacheBaseForm)this).ContextGuid);
						while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
						{
							await Task.Delay(100);
						}
						if (IsDisposed || token.IsCancellationRequested)
						{
							return;
						}
						((Control)this).Invoke((Delegate)(Action)delegate
						{
							try
							{
								((Control)BtnAccept).Enabled = false;
							}
							catch (Exception ex5)
							{
								WebErrorLog.ErrorLogInstance.StartLog(ex5, "", "InitializeBtnsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 171);
							}
						});
						((Form)this).DialogResult = (DialogResult)2;
						return;
					}
					if (selectedShift?.LstShiftDet != null)
					{
						foreach (ShiftDet item2 in selectedShift.LstShiftDet)
						{
							ShiftDetView clsDet = new ShiftDetView(((CacheBaseForm)this).ContextGuid)
							{
								DateStartM = item2.DateStartM,
								DateFinishM = item2.DateFinishM,
								ShiftCodingId = item2.ShiftCodingId,
								Description = item2.Description,
								DayOfWeeks = new bool[7] { item2.Sat, item2.Sun, item2.Mon, item2.Tue, item2.Wed, item2.Tur, item2.Fri },
								WorkOnHolidaysWithOutFriDay = Convert.ToBoolean(selectedShift.StTatili),
								Fritype = item2.FriType
							};
							await ShiftDetView.SetClsShiftCodingAsync(((CacheBaseForm)this).ContextGuid, clsDet, item2.ShiftCodeing.CurrDate, token);
							shiftDetViews.Add(clsDet);
						}
					}
				}
				else
				{
					while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
					{
						await Task.Delay(100);
					}
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					((Control)this).Invoke((Delegate)(Action)delegate
					{
						try
						{
							((Control)BtnAccept).Enabled = false;
						}
						catch (Exception ex4)
						{
							WebErrorLog.ErrorLogInstance.StartLog(ex4, "", "InitializeBtnsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 208);
						}
					});
				}
			}
			if (shiftDetViews.Count <= 0)
			{
				return;
			}
			string stDateTime = Calendar.MiladiToCalture(StartDate);
			string enDateTime = Calendar.MiladiToCalture(EndDate);
			List<Holiday> pHolidays = await Holiday.GetAllAsync(((CacheBaseForm)this).ContextGuid, StartDate, EndDate, isHolidayOnly: true, token);
			for (int i = 0; i < shiftDetViews.Count; i++)
			{
				while (enDateTime != null && stDateTime != null && string.Compare(stDateTime, enDateTime, StringComparison.Ordinal) < 0)
				{
					UCMonth curMonth = new UCMonth(((CacheBaseForm)this).ContextGuid, _font, stDateTime, shiftDetViews, pHolidays, isFirst);
					UCMonthlst.Add(curMonth);
					((Control)curMonth).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
					((Control)curMonth).Location = new Point(801, 3);
					((Control)curMonth).Name = "ucMonth" + UCMonthlst.Count;
					((Control)curMonth).RightToLeft = (RightToLeft)1;
					((Control)curMonth).Size = new Size(252, 186);
					((Control)curMonth).TabIndex = 0;
					((Control)this).Invoke((Delegate)(Action)delegate
					{
						try
						{
							((Control)flowLayoutPanel1).Controls.Add((Control)(object)curMonth);
						}
						catch (Exception ex3)
						{
							WebErrorLog.ErrorLogInstance.StartLog(ex3, "", "InitializeBtnsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 258);
						}
					});
					stDateTime = stDateTime.Substring(0, 5) + (int.Parse(stDateTime.Substring(5, 2)) + 1).ToString("00") + "/01";
					if (stDateTime.Substring(5, 2) == "13")
					{
						stDateTime = int.Parse(stDateTime.Substring(0, 4)) + 1 + "/01/01";
					}
				}
			}
			while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
			{
				await Task.Delay(100);
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)BtnAccept).Enabled = true;
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "InitializeBtnsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 277);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitializeBtnsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 281);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			CancellationToken token = default(CancellationToken);
			if (!isEmptyBefore)
			{
				DialogResult val = FMessageBox.Show(Utilities.GetTranslate("[قبلا\u064b برای این قرارداد شیفت برنامه ریزی شده ای وجود داشته است .آیا میخواهید عملیات ذخیره ادامه پیدا کند؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNoCancel, FMessageBox.FMessageBoxIcon.Information, FMessageBox.FMessageBoxDefaultButton.Button1);
				if ((int)val == 2)
				{
					return;
				}
				if ((int)val == 7)
				{
					((FormExtention)this).ResetControlChanged();
					((Form)this).Close();
					return;
				}
			}
			List<ShiftProgrammableDet> xShiftProgDets = new List<ShiftProgrammableDet>();
			ShiftProgrammableDet shiftProgrammableDet = null;
			bool flag = true;
			int num = 0;
			while (flag)
			{
				flag = false;
				foreach (UCMonth item in UCMonthlst)
				{
					foreach (UCLblDay ucLblDay in item.UcLblDays)
					{
						if (num < ucLblDay.tShiftCodeings.Count)
						{
							flag = true;
							if (shiftProgrammableDet == null || shiftProgrammableDet.ShiftCodingID != ucLblDay.tShiftCodeings[num].Id)
							{
								shiftProgrammableDet = new ShiftProgrammableDet(((CacheBaseForm)this).ContextGuid)
								{
									GharardadID = ClsGharardad.ID,
									ShiftCodingID = ucLblDay.tShiftCodeings[num].Id,
									DateStartM = ucLblDay.DateM,
									Desc = ucLblDay.tShiftCodeings[num].Description,
									WorkOnHolidays = Convert.ToBoolean(ucLblDay.workOnHolidayWithOutFriday),
									FriDayType = ucLblDay.FriDayType
								};
								xShiftProgDets.Add(shiftProgrammableDet);
							}
							shiftProgrammableDet.DateFinishM = ucLblDay.DateM.AddDays(1.0).AddSeconds(-1.0);
						}
						else
						{
							shiftProgrammableDet = null;
						}
					}
				}
				num++;
				shiftProgrammableDet = null;
			}
			await ShiftProgrammableDet.DeleteByGharardadAsync(((CacheBaseForm)this).ContextGuid, ClsGharardad.ID, token);
			await ShiftProgrammableDet.SaveListAsync(((CacheBaseForm)this).ContextGuid, xShiftProgDets, ClsGharardad.PersonId, token);
			((FormExtention)this).ResetControlChanged();
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[اطلاعات با موفقیت ثبت شد]", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 352);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if ((int)FMessageBox.Show(Utilities.GetTranslate("[آیا مایل به حذف این شیفت برنامه ریزی شده میباشید؟]"), "", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Information, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				TokenFrm?.Cancel();
				TokenFrm = new CancellationTokenSource();
				CancellationToken token = TokenFrm.Token;
				await ShiftProgrammableDet.DeleteByGharardadAsync(((CacheBaseForm)this).ContextGuid, ClsGharardad.ID, token);
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 369);
		}
	}

	private async void BtnSelectGroup_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnSelectGroup).Text == Utilities.GetTranslate("[انتخاب گروهی]"))
			{
				if (!isHelpDisplayed)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)6, "[ابتدا شیفت یا شیفتهای مورد نظر را انتخاب کنید]", "", 3);
				}
				FrmShiftCodeingList frmShiftCodeingList = new FrmShiftCodeingList(((CacheBaseForm)this).ContextGuid, _font, isSelect: true, (TypeShiftEnumType)0, new List<ShiftCodeing>());
				if ((int)((Form)frmShiftCodeingList).ShowDialog() == 1)
				{
					if (!isHelpDisplayed)
					{
						Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "[حال روزهای مورد نظر را انتخاب نمائید. در نهایت روی ثبت موقت کلیک کنید]", "", 10);
						isHelpDisplayed = true;
					}
					ShiftCodingLst = frmShiftCodeingList.ListShiftCodeingsSelected;
					((Control)BtnSelectGroup).Text = Utilities.GetTranslate("[ثبت موقت]");
					((Control)BtnAccept).Enabled = false;
				}
				return;
			}
			foreach (UCMonth item in UCMonthlst)
			{
				foreach (UCLblDay ucLblDay in item.UcLblDays)
				{
					if (ucLblDay.isSelected)
					{
						if (ShiftCodingLst != null)
						{
							ucLblDay.tShiftCodeings = ShiftCodingLst;
						}
						ucLblDay.ResetLblDay();
					}
				}
			}
			((Control)BtnSelectGroup).Text = Utilities.GetTranslate("[انتخاب گروهی]");
			((Control)BtnAccept).Enabled = true;
			ShiftCodingLst = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelectGroup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftProgrammable.cs", 413);
		}
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Expected O, but got Unknown
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Expected O, but got Unknown
		//IL_0d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Expected O, but got Unknown
		//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1c: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		BtnSelectGroup = new ButtonX();
		BtnDelete = new ButtonX();
		flowLayoutPanel2 = new FlowLayoutPanel();
		flowLayoutPanel1 = new FlowLayoutPanel();
		labelX5 = new LabelX();
		labelX3 = new LabelX();
		labelX1 = new LabelX();
		labelX39 = new LabelX();
		BtnAccept = new ButtonX();
		BtnCancel = new ButtonX();
		lblEndDate = new LabelX();
		lblStartDate = new LabelX();
		lblFullName = new LabelX();
		lblGharardadNo = new LabelX();
		BtnExit = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnExit);
		((Control)panelEx1).Controls.Add((Control)(object)BtnSelectGroup);
		((Control)panelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)panelEx1).Controls.Add((Control)(object)flowLayoutPanel2);
		((Control)panelEx1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)labelX39);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)lblEndDate);
		((Control)panelEx1).Controls.Add((Control)(object)lblStartDate);
		((Control)panelEx1).Controls.Add((Control)(object)lblFullName);
		((Control)panelEx1).Controls.Add((Control)(object)lblGharardadNo);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(789, 508);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 4;
		((Control)BtnSelectGroup).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSelectGroup).Anchor = (AnchorStyles)6;
		BtnSelectGroup.ColorTable = (eButtonColor)6;
		((Control)BtnSelectGroup).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnSelectGroup.Image = (Image)(object)Resources.report_check;
		BtnSelectGroup.ImagePosition = (eImagePosition)1;
		((Control)BtnSelectGroup).Location = new Point(214, 437);
		((Control)BtnSelectGroup).Name = "BtnSelectGroup";
		((Control)BtnSelectGroup).Padding = new Padding(2);
		((Control)BtnSelectGroup).Size = new Size(95, 33);
		((PopupItemControl)BtnSelectGroup).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectGroup).TabIndex = 36;
		((Control)BtnSelectGroup).Text = "[انتخاب گروهی]";
		((Control)BtnSelectGroup).Click += BtnSelectGroup_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDelete).Anchor = (AnchorStyles)6;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(421, 438);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(95, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 35;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)flowLayoutPanel2).Anchor = (AnchorStyles)14;
		((ScrollableControl)flowLayoutPanel2).AutoScroll = true;
		((Control)flowLayoutPanel2).Location = new Point(534, 437);
		((Control)flowLayoutPanel2).Name = "flowLayoutPanel2";
		((Control)flowLayoutPanel2).Size = new Size(243, 35);
		((Control)flowLayoutPanel2).TabIndex = 0;
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)15;
		((ScrollableControl)flowLayoutPanel1).AutoScroll = true;
		((Control)flowLayoutPanel1).Location = new Point(12, 42);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(765, 362);
		((Control)flowLayoutPanel1).TabIndex = 34;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX5).Location = new Point(146, 12);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(19, 23);
		((Control)labelX5).TabIndex = 33;
		((Control)labelX5).Text = "[تا]:";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX3).Location = new Point(294, 12);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(99, 23);
		((Control)labelX3).TabIndex = 33;
		((Control)labelX3).Text = "[تاریخ قرارداد از]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX1).Location = new Point(493, 12);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(122, 23);
		((Control)labelX1).TabIndex = 33;
		((Control)labelX1).Text = "[نام و نام خانوادگی]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX39).Anchor = (AnchorStyles)9;
		((Control)labelX39).BackColor = Color.Transparent;
		((BaseItemControl)labelX39).BackgroundStyle.Class = "";
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX39).Location = new Point(669, 12);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(96, 23);
		((Control)labelX39).TabIndex = 33;
		((Control)labelX39).Text = "[شماره قرارداد]:";
		labelX39.TextAlignment = (StringAlignment)2;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(113, 436);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 4;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(315, 437);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 3;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((BaseItemControl)lblEndDate).BackgroundStyle.Class = "";
		((BaseItemControl)lblEndDate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblEndDate).ForeColor = Color.DarkRed;
		((Control)lblEndDate).Location = new Point(24, 12);
		((Control)lblEndDate).Name = "lblEndDate";
		((Control)lblEndDate).Size = new Size(116, 23);
		((Control)lblEndDate).TabIndex = 10;
		((Control)lblEndDate).Text = "-----";
		((BaseItemControl)lblStartDate).BackgroundStyle.Class = "";
		((BaseItemControl)lblStartDate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblStartDate).ForeColor = Color.DarkRed;
		((Control)lblStartDate).Location = new Point(171, 12);
		((Control)lblStartDate).Name = "lblStartDate";
		((Control)lblStartDate).Size = new Size(117, 23);
		((Control)lblStartDate).TabIndex = 10;
		((Control)lblStartDate).Text = "-----";
		((Control)lblFullName).Anchor = (AnchorStyles)13;
		((BaseItemControl)lblFullName).BackgroundStyle.Class = "";
		((BaseItemControl)lblFullName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblFullName).ForeColor = Color.DarkRed;
		((Control)lblFullName).Location = new Point(399, 12);
		((Control)lblFullName).Name = "lblFullName";
		((Control)lblFullName).Size = new Size(88, 23);
		((Control)lblFullName).TabIndex = 10;
		((Control)lblFullName).Text = "-----";
		((Control)lblGharardadNo).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblGharardadNo).BackgroundStyle.Class = "";
		((BaseItemControl)lblGharardadNo).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblGharardadNo).ForeColor = Color.DarkRed;
		((Control)lblGharardadNo).Location = new Point(621, 12);
		((Control)lblGharardadNo).Name = "lblGharardadNo";
		((Control)lblGharardadNo).Size = new Size(42, 23);
		((Control)lblGharardadNo).TabIndex = 10;
		((Control)lblGharardadNo).Text = "-----";
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExit).Anchor = (AnchorStyles)6;
		BtnExit.ColorTable = (eButtonColor)6;
		((Control)BtnExit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnExit.Image = (Image)(object)Resources.gnome_application_exit;
		BtnExit.ImagePosition = (eImagePosition)1;
		((Control)BtnExit).Location = new Point(12, 437);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Padding = new Padding(2);
		((Control)BtnExit).Size = new Size(95, 33);
		((PopupItemControl)BtnExit).Style = (eDotNetBarStyle)9;
		((Control)BtnExit).TabIndex = 37;
		((Control)BtnExit).Text = "[خروج]";
		((Control)BtnExit).Click += BtnExit_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(789, 530);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmShiftProgrammable";
		((Form)this).ShowInTaskbar = true;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "[شیفت خاص یک کارمند]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
