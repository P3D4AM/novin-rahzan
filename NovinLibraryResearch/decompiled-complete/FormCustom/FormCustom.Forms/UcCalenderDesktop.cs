using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using BehComponents;
using EntityCache;
using EntityCache.Attendance.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Mapping.Attandance;
using FormCustom.Assistance;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace FormCustom.Forms;

public class UcCalenderDesktop : UserControl, IHasContext, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private System.Timers.Timer _timer;

	private string contextGuid;

	private CancellationTokenSource Token = new CancellationTokenSource();

	private CancellationTokenSource TokenHover = new CancellationTokenSource();

	private List<BedehiTalabReportBusiness> CashListBedahi = new List<BedehiTalabReportBusiness>();

	private IContainer components;

	private MonthCalendarX monthCalender;

	private Panel panel2;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem MenuAddNote;

	private ToolStripMenuItem menuReadNote;

	private ToolStripSeparator toolStripSeparator1;

	private Timer timer1;

	private IEnumerable<CalenderBusiness> LstCalender { get; set; }

	private CalenderBusiness Current
	{
		get
		{
			if (monthCalender == null)
			{
				return new CalenderBusiness(ContextGuid);
			}
			return LstCalender.FirstOrDefault((CalenderBusiness p) => p.DateM.Date == monthCalender.GetSelectedDateInDateTime().Date);
		}
	}

	public string ContextGuid
	{
		get
		{
			return contextGuid;
		}
		set
		{
			contextGuid = value;
			if (!string.IsNullOrEmpty(contextGuid) && ClsCache.GetCache(contextGuid) != null)
			{
				Token?.Cancel();
				Token = new CancellationTokenSource(60000);
				CancellationToken tkn = Token.Token;
				Task.Run(() => BindingSourceAsync(tkn));
			}
		}
	}

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	private void InitializeTimer()
	{
		_timer = new System.Timers.Timer(1000.0);
		_timer.Elapsed += OnTimerElapsed;
		_timer.AutoReset = true;
		_timer.Enabled = true;
	}

	private void OnTimerElapsed(object sender, ElapsedEventArgs e)
	{
		if (IsDisposed)
		{
			return;
		}
		DateTime currentDate = DateTime.Now.Date;
		DateTime date = monthCalender.GetSelectedDateInDateTime().Date;
		if (!(currentDate != date))
		{
			return;
		}
		((Control)this).BeginInvoke((Delegate)(Action)delegate
		{
			if (IsDisposed)
			{
				return;
			}
			try
			{
				monthCalender.SetSelectedDate(currentDate);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "OnTimerElapsed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 60);
			}
		});
	}

	public UcCalenderDesktop()
	{
		try
		{
			InitializeComponent();
			((Control)(object)this).Transelate();
			InitializeTimer();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 76);
		}
	}

	public CalenderBusiness GetCurrentOnHover(int day, CancellationToken token)
	{
		try
		{
			if (LstCalender == null)
			{
				return null;
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return null;
			}
			DateTime DtNnow = $"{monthCalender.GetSelectedDateInPersianDateTime().Year}/{monthCalender.GetSelectedDateInPersianDateTime().Month}/{day}".ShamsiToMiladi();
			if (token.IsCancellationRequested)
			{
				return null;
			}
			return LstCalender.Where((CalenderBusiness p) => p.DateM.Date == DtNnow).SingleOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCurrentOnHover", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 126);
			return null;
		}
	}

	private async Task<ReturnedSaveFuncInfo> GetAndAveFromApi(string contextGuid, CancellationToken token)
	{
		if (IsDisposed)
		{
			return null;
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			new WebCalender();
			ReturnedSaveFuncInfoWithValue<List<WebCalender>> returnedSaveFuncInfoWithValue = await WebCalender.GetAllAsync(token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				LstCalender = null;
			}
			else
			{
				LstCalender = await CalenderMapper.Instance.MapListAsync(ContextGuid, returnedSaveFuncInfoWithValue.value);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CalenderBusiness.SaveRangeAsync(contextGuid, LstCalender, token), ReturnedState.None, "GetAndAveFromApi", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 143);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAndAveFromApi", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 146);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> BindingSource(CancellationToken PTokenm, IEnumerable<CalenderBusiness> pLstCalender = null)
	{
		if (IsDisposed)
		{
			return null;
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (PTokenm.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("", "", "BindingSource", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 156);
			}
			LstCalender = pLstCalender;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BindingSource", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 161);
			ret.AddReturnedValue(ex, "", "BindingSource", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 162);
		}
		if (!ret.HasError)
		{
			await FillFormAsync(PTokenm);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> BindingSourceAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 1;
			try
			{
				if (IsDisposed)
				{
					result = null;
					goto IL_032e;
				}
				if (string.IsNullOrEmpty(ContextGuid))
				{
					ret.AddError(EnSystem.None, "Context Not Set", "", "BindingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 174);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_032e;
				}
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "BindingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 176);
				}
				DateTime value = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, 0);
				IEnumerable<CalenderBusiness> enumerable = await CalenderBusiness.GetAllAsync(d2: new DateTime(DateTime.Now.Year, 12, 29, 0, 0, 0, 0), contextGuid: ContextGuid, d1: value, status: true, token: token);
				if (enumerable != null && enumerable.Any())
				{
					LstCalender = enumerable;
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await GetAndAveFromApi(ContextGuid, token), ReturnedState.None, "BindingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 183);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BindingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 185);
			}
		}
		finally
		{
			if (!ret.HasError)
			{
				await FillFormAsync(token);
			}
		}
		return ret;
		IL_032e:
		return result;
	}

	private void monthCalender_SelectedDateChanged(DateTime selectedDateTime, PersianDateTime selectedPersianDateTime)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (!IsDisposed && LstCalender == null)
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "Calender Not Initialize", "", "monthCalender_SelectedDateChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 201);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "monthCalender_SelectedDateChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 207);
		}
	}

	private async Task SetToolTipsAsync(CalenderBusiness pCurrent, CancellationToken token)
	{
		int line = 0;
		try
		{
			new List<ToolStripMenuItem>();
			line = 1;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			line = 2;
			int i = 0;
			line = 3;
			if (!string.IsNullOrEmpty(pCurrent?.Monasebat ?? ""))
			{
				line = 4;
				ToolStripMenuItem val = new ToolStripMenuItem
				{
					Name = "MonsebatItem",
					AutoSize = true,
					BackColor = Color.LightBlue,
					Text = ((pCurrent?.Monasebat ?? "") ?? "")
				};
				line = 5;
				((ToolStrip)contextMenuStrip1).Items.Add((ToolStripItem)(object)val);
				line = 6;
			}
			line = 7;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			line = 8;
			foreach (string item in await DescriptionSpliterReturnAsync(pCurrent?.Description ?? "", token))
			{
				line = 9;
				ToolStripMenuItem val2 = new ToolStripMenuItem
				{
					Name = "Note" + i,
					AutoSize = true,
					BackColor = Color.LightSeaGreen,
					Text = item
				};
				line = 10;
				((ToolStrip)contextMenuStrip1).Items.Add((ToolStripItem)(object)val2);
				line = 11;
				i++;
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				line = 12;
			}
			line = 13;
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				line = 14;
				ToolStripItemCollection items = ((ToolStrip)contextMenuStrip1).Items;
				items.AddRange(CreateBedahiToolStrip(await GetListBedehi(token)).ToArray());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SetToolTipsAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 261);
		}
	}

	private async Task<ReturnedSaveFuncInfo> FillFormAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			line = 1;
			while (!((Control)this).IsHandleCreated)
			{
				line = 2;
				await Task.Delay(100);
				line = 3;
				if (IsDisposed || token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 276);
				}
				line = 4;
			}
			line = 5;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 280);
			}
			line = 6;
			IEnumerable<CalenderBusiness> lstTempTatili = null;
			line = 7;
			IEnumerable<CalenderBusiness> lstTempDescription = null;
			line = 8;
			if (LstCalender != null)
			{
				line = 9;
				lstTempTatili = LstCalender?.Where((CalenderBusiness p) => p.STRasmi);
				line = 10;
				lstTempDescription = LstCalender?.Where((CalenderBusiness p) => !string.IsNullOrEmpty(p.Description.Trim()));
				line = 11;
			}
			line = 12;
			line = 13;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 297);
			}
			line = 14;
			PersianDateTime BehDayConverter;
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						ret.AddErrorTokenIsCancelled("", "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 305);
						return;
					}
					line = 16;
					try
					{
						line = 17;
						monthCalender.PersianAnnuallyBoldedDates_RemoveAll();
						monthCalender.EnglishHolidayDates_RemoveAll();
						line = 20;
						if (IsDisposed || token.IsCancellationRequested)
						{
							line = 21;
							ret.AddErrorTokenIsCancelled("", "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 318);
						}
						else
						{
							line = 30;
							IEnumerable<CalenderBusiness> enumerable = lstTempTatili;
							if (enumerable != null && enumerable.Any())
							{
								foreach (CalenderBusiness item in lstTempTatili)
								{
									line = 40;
									BehDayConverter = new PersianDateTime(item.DateM);
									monthCalender.PersianHolidayDates_Add(BehDayConverter);
								}
							}
							line = 50;
							IEnumerable<CalenderBusiness> enumerable2 = lstTempDescription;
							if (enumerable2 != null && enumerable2.Any())
							{
								foreach (CalenderBusiness item2 in lstTempDescription)
								{
									line = 60;
									BehDayConverter = new PersianDateTime(item2.DateM);
									if (!string.IsNullOrEmpty(item2.Description))
									{
										monthCalender.PersianAnnuallyBoldedDates_Add(BehDayConverter);
									}
								}
								return;
							}
						}
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, $"11//Line:{line}", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 340);
					}
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 342);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"22//Line:{line}", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 347);
			ret.AddError(EnSystem.None, ex.Message, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 348);
		}
		return ret;
	}

	private async Task<string> DescriptionSpliterAsync(string Description, CancellationToken token)
	{
		string ReturnString = "";
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
			MatchCollection matchCollection = new Regex("\\[(?<CashierGuids>)[a-fA-F0-9-]+\\]\\[(?<Text>)[\\u0600-\\u06ff\\sa-zA-Z0-9#$+-@$()%~]+\\]\\#*", RegexOptions.IgnoreCase | RegexOptions.Compiled).Matches(Description);
			if (matchCollection.Count > 0)
			{
				ReturnString = "----------------یادداشتهای روزانه--------------------- ";
				foreach (Match item in matchCollection)
				{
					string[] array = item.Value.Split(new char[1] { '[' });
					ReturnString = ((!(Guid.Parse(array[1].Replace("]", "").Trim()) == cashierBusiness.Guid)) ? "" : (ReturnString + " \r\n " + array[2].Replace("]", "").Trim() + " "));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DescriptionSpliterAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 375);
		}
		return ReturnString;
	}

	private async Task<List<string>> DescriptionSpliterReturnAsync(string Description, CancellationToken token)
	{
		List<string> ReturnString = new List<string>();
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
			MatchCollection matchCollection = new Regex("\\[(?<CashierGuids>)[a-fA-F0-9-]+\\]\\[(?<Text>)[\\u0600-\\u06ff\\sa-zA-Z0-9#$+-@$()%~]+\\]\\#*", RegexOptions.IgnoreCase | RegexOptions.Compiled).Matches(Description);
			if (matchCollection.Count > 0)
			{
				foreach (Match item in matchCollection)
				{
					string[] array = item.Value.Split(new char[1] { '[' });
					if (Guid.Parse(array[1].Replace("]", "").Trim()) == cashierBusiness.Guid)
					{
						ReturnString.Add(array[2].Replace("]", "").Trim() + " ");
					}
					else
					{
						ReturnString.Add("");
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DescriptionSpliterReturnAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 400);
		}
		return ReturnString;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				if (_timer != null)
				{
					_timer.Stop();
					_timer.Dispose();
					_timer = null;
				}
				Token?.Cancel();
				Token = null;
				LstCalender = null;
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 430);
		}
	}

	private async void MenuAddNote_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
			string input = "";
			if ((int)InputBoxCSharp.ShowInputDialog(ref input, "لطفا یادداشت خود را وراد کنید") != 1)
			{
				return;
			}
			if (!string.IsNullOrEmpty(input))
			{
				string text = "[" + cashierBusiness.Guid.ToString() + "][" + input + "]";
				DateTime dtSelected = monthCalender.GetSelectedDateInDateTime();
				CalenderBusiness? calenderBusiness = LstCalender.SingleOrDefault((CalenderBusiness p) => p.DateM.Date == dtSelected.Date);
				calenderBusiness.Description += text;
				ret = await calenderBusiness.SaveAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MenuAddNote_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 450);
		}
		finally
		{
			if (!IsDisposed)
			{
				if (ret.HasError)
				{
					this.ShowReturnedSavedFuncInfoForUserControl(ret, "پیغام سیستم", "MenuAddNote_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs");
				}
				else
				{
					Token?.Cancel();
					Token = new CancellationTokenSource(60000);
					CancellationToken tkn = Token.Token;
					Task.Run(() => BindingSourceAsync(tkn));
				}
			}
		}
	}

	private async void menuReadNote_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)MessageBox.Show((IWin32Window)(object)this, "ایا از حذف متن یادآوری مطمئن هستید".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)48) == 6)
			{
				Current.Description = "";
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Current.SaveAsync(), ReturnedState.None, "menuReadNote_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 476);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "menuReadNote_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 481);
		}
		if (IsDisposed)
		{
			return;
		}
		if (ret.HasError)
		{
			this.ShowReturnedSavedFuncInfoForUserControl(ret, "پیغام سیستم", "menuReadNote_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs");
			return;
		}
		Token?.Cancel();
		Token = new CancellationTokenSource(60000);
		CancellationToken tkn = Token.Token;
		Task.Run(() => BindingSourceAsync(tkn));
	}

	private async Task<List<BedehiTalabReportBusiness>> GetListBedehi(CancellationToken token)
	{
		try
		{
			BedehiTalabFilter filter = new BedehiTalabFilter
			{
				Date1 = monthCalender.GetSelectedDateInDateTime().Date,
				Date2 = monthCalender.GetSelectedDateInDateTime().Date.AddHours(23.0).AddSeconds(59.0),
				Search = "",
				TypeList = new List<EnBedehiTalabType>
				{
					EnBedehiTalabType.CheckSh,
					EnBedehiTalabType.CheckM,
					EnBedehiTalabType.AghsatSh,
					EnBedehiTalabType.AghsatM
				}
			};
			if (IsDisposed || token.IsCancellationRequested)
			{
				return new List<BedehiTalabReportBusiness>();
			}
			return await BedehiTalabReportBusiness.GetAllAsync(ContextGuid, filter, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetListBedehi", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 511);
			return new List<BedehiTalabReportBusiness>();
		}
	}

	private List<ToolStripItem> CreateBedahiToolStrip(List<BedehiTalabReportBusiness> BedList)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		List<ToolStripItem> list = new List<ToolStripItem>();
		try
		{
			int num = 1;
			int num2 = 1;
			int num3 = 1;
			int num4 = 1;
			ToolStripMenuItem item = new ToolStripMenuItem();
			foreach (BedehiTalabReportBusiness Bed in BedList)
			{
				switch (Bed.Type)
				{
				case EnBedehiTalabType.CheckSh:
					if (num < 3)
					{
						ToolStripMenuItem val = new ToolStripMenuItem();
						((ToolStripItem)val).Name = "CheckSh" + num;
						((ToolStripItem)val).AutoSize = true;
						((ToolStripItem)val).BackColor = Color.LightGoldenrodYellow;
						((ToolStripItem)val).Text = "چک پرداختی به  مبلغ " + Bed.Price.ToString("0") + " مربوط به " + Bed.TafsilName + " ...";
						item = val;
					}
					if (num == 3)
					{
						item = new ToolStripMenuItem
						{
							Name = "CheckSh" + num,
							AutoSize = true,
							BackColor = Color.LightGoldenrodYellow,
							Text = "نمایش موارد بیشتر  ..."
						};
					}
					list.Add((ToolStripItem)(object)item);
					num++;
					break;
				case EnBedehiTalabType.CheckM:
					if (num < 3)
					{
						ToolStripMenuItem val = new ToolStripMenuItem();
						((ToolStripItem)val).Name = "CheckM" + num2;
						((ToolStripItem)val).AutoSize = true;
						((ToolStripItem)val).BackColor = Color.LightCoral;
						((ToolStripItem)val).Text = "چک دریافتی به  مبلغ " + Bed.Price.ToString("0") + " مربوط به " + Bed.TafsilName + " ...";
						item = val;
					}
					if (num == 3)
					{
						item = new ToolStripMenuItem
						{
							Name = "CheckM" + num2,
							AutoSize = true,
							BackColor = Color.LightCoral,
							Text = "نمایش موارد بیشتر  ..."
						};
					}
					list.Add((ToolStripItem)(object)item);
					num2++;
					break;
				case EnBedehiTalabType.AghsatM:
					if (num < 3)
					{
						ToolStripMenuItem val = new ToolStripMenuItem();
						((ToolStripItem)val).Name = "AghsatM" + num3;
						((ToolStripItem)val).AutoSize = true;
						((ToolStripItem)val).BackColor = Color.LightCoral;
						((ToolStripItem)val).Text = "قسط مشتری به مبلغ " + Bed.Price.ToString("0") + " مربوط به " + Bed.TafsilName + " ...";
						item = val;
					}
					if (num == 3)
					{
						item = new ToolStripMenuItem
						{
							Name = "AghsatM" + num3,
							AutoSize = true,
							BackColor = Color.LightCoral,
							Text = "نمایش موارد بیشتر  ..."
						};
					}
					num3++;
					list.Add((ToolStripItem)(object)item);
					break;
				case EnBedehiTalabType.AghsatSh:
					if (num < 3)
					{
						item = new ToolStripMenuItem
						{
							Name = "AghsatSh" + num4,
							AutoSize = true,
							BackColor = Color.LightCoral,
							Text = $"قسط شخصی به مبلغ  {Bed.Price} مربوط به {Bed.TafsilName} ..."
						};
					}
					if (num == 3)
					{
						item = new ToolStripMenuItem
						{
							Name = "AghsatSh" + num4,
							AutoSize = true,
							BackColor = Color.LightSalmon,
							Text = "نمایش موارد بیشتر  ..."
						};
					}
					list.Add((ToolStripItem)(object)item);
					num4++;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateBedahiToolStrip", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 635);
		}
		return list;
	}

	private async void monthCalender_DayMouseClick(MonthCalendarX.DayMouseEventArgs e)
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			TokenHover?.Cancel();
			TokenHover = new CancellationTokenSource();
			CancellationToken token = TokenHover.Token;
			if (e.ValueOfDay != null)
			{
				await SetToolTipsAsync(GetCurrentOnHover(e.ValueOfDay.ToString().ParseToInt(), token), token);
				if (!TokenHover.IsCancellationRequested)
				{
					((ToolStripDropDown)contextMenuStrip1).Show((Control)(object)monthCalender, new Point(e.X_InCalendar + 10, e.Y_InCalendar));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "monthCalender_DayMouseClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 652);
		}
	}

	private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		try
		{
			if (((ArrangedElementCollection)((ToolStrip)contextMenuStrip1).Items).Count != 3)
			{
				((ToolStrip)contextMenuStrip1).Items.Clear();
				((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
				{
					(ToolStripItem)MenuAddNote,
					(ToolStripItem)menuReadNote,
					(ToolStripItem)toolStripSeparator1
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "contextMenuStrip1_Closed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcCalenderDesktop.cs", 668);
		}
	}

	private void InitializeComponent()
	{
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Expected O, but got Unknown
		components = new Container();
		monthCalender = new MonthCalendarX();
		contextMenuStrip1 = new ContextMenuStrip(components);
		MenuAddNote = new ToolStripMenuItem();
		menuReadNote = new ToolStripMenuItem();
		toolStripSeparator1 = new ToolStripSeparator();
		panel2 = new Panel();
		timer1 = new Timer(components);
		((Control)contextMenuStrip1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)monthCalender).AutoSize = true;
		((Control)monthCalender).BackColor = Color.Transparent;
		monthCalender.BoldedDayForeColor = Color.LightSeaGreen;
		monthCalender.BorderColor = Color.Transparent;
		((UserControl)monthCalender).BorderStyle = (BorderStyle)2;
		monthCalender.CalendarType = CalendarTypes.Persian;
		((Control)monthCalender).ContextMenuStrip = contextMenuStrip1;
		monthCalender.DayRectTickness = 2f;
		monthCalender.DaysBackColor = Color.Transparent;
		monthCalender.DaysFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		monthCalender.DaysForeColor = Color.Blue;
		((Control)monthCalender).Dock = (DockStyle)5;
		monthCalender.EnglishAnnuallyBoldedDates = new DateTime[0];
		monthCalender.EnglishBoldedDates = new DateTime[0];
		monthCalender.EnglishHolidayDates = new DateTime[0];
		monthCalender.EnglishMonthlyBoldedDates = new DateTime[0];
		monthCalender.HolidayForeColor = Color.Red;
		monthCalender.HolidayWeekly = MonthCalendarX.DayOfWeekForHoliday.Friday;
		monthCalender.LineWeekColor = Color.Black;
		((Control)monthCalender).Location = new Point(0, 0);
		((Control)monthCalender).Name = "monthCalender";
		monthCalender.PersianAnnuallyBoldedDates = new PersianDateTime[0];
		monthCalender.PersianBoldedDates = new PersianDateTime[0];
		monthCalender.PersianHolidayDates = new PersianDateTime[0];
		monthCalender.PersianMonthlyBoldedDates = new PersianDateTime[0];
		((Control)monthCalender).RightToLeft = (RightToLeft)1;
		monthCalender.ShowToday = true;
		monthCalender.ShowTodayRect = true;
		monthCalender.ShowToolTips = true;
		monthCalender.ShowTrailing = true;
		((Control)monthCalender).Size = new Size(311, 325);
		monthCalender.Style_DaysButton = ButtonX.ButtonStyles.Simple;
		monthCalender.Style_GotoTodayButton = ButtonX.ButtonStyles.Blue;
		monthCalender.Style_MonthButton = ButtonX.ButtonStyles.Simple;
		monthCalender.Style_NextMonthButton = ButtonX.ButtonStyles.Simple;
		monthCalender.Style_PreviousMonthButton = ButtonX.ButtonStyles.Simple;
		monthCalender.Style_YearButton = ButtonX.ButtonStyles.Simple;
		((Control)monthCalender).TabIndex = 1;
		monthCalender.TitleBackColor = Color.Transparent;
		monthCalender.TitleFont = new Font("Tahoma", 8.25f);
		monthCalender.TitleForeColor = Color.Black;
		monthCalender.TodayBackColor = Color.Transparent;
		monthCalender.TodayFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		monthCalender.TodayForeColor = Color.Black;
		monthCalender.TodayRectColor = Color.Black;
		monthCalender.TodayRectTickness = 2f;
		monthCalender.TrailingForeColor = Color.Peru;
		monthCalender.WeekDaysBackColor = Color.Transparent;
		monthCalender.WeekDaysFont = new Font("Tahoma", 8.25f);
		monthCalender.WeekDaysForeColor = Color.Black;
		monthCalender.WeekStartsOn = MonthCalendarX.WeekDays.Saturday;
		monthCalender.SelectedDateChanged += monthCalender_SelectedDateChanged;
		monthCalender.DayMouseClick += monthCalender_DayMouseClick;
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)MenuAddNote,
			(ToolStripItem)menuReadNote,
			(ToolStripItem)toolStripSeparator1
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((ToolStrip)contextMenuStrip1).RenderMode = (ToolStripRenderMode)2;
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((ToolStripDropDownMenu)contextMenuStrip1).ShowImageMargin = false;
		((ToolStrip)contextMenuStrip1).ShowItemToolTips = false;
		((Control)contextMenuStrip1).Size = new Size(179, 54);
		((ToolStripDropDown)contextMenuStrip1).Closed += new ToolStripDropDownClosedEventHandler(contextMenuStrip1_Closed);
		((ToolStripItem)MenuAddNote).Name = "MenuAddNote";
		((ToolStripItem)MenuAddNote).Size = new Size(178, 22);
		((ToolStripItem)MenuAddNote).Text = "اضافه کردن یادداشت جدید";
		((ToolStripItem)MenuAddNote).Click += MenuAddNote_Click;
		((ToolStripItem)menuReadNote).Name = "menuReadNote";
		((ToolStripItem)menuReadNote).Size = new Size(178, 22);
		((ToolStripItem)menuReadNote).Text = "حذف یادداشت";
		((ToolStripItem)menuReadNote).Click += menuReadNote_Click;
		((ToolStripItem)toolStripSeparator1).Name = "toolStripSeparator1";
		((ToolStripItem)toolStripSeparator1).Size = new Size(175, 6);
		((Control)panel2).Controls.Add((Control)(object)monthCalender);
		((Control)panel2).Dock = (DockStyle)5;
		((Control)panel2).Location = new Point(0, 0);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(311, 325);
		((Control)panel2).TabIndex = 4;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((UserControl)this).BorderStyle = (BorderStyle)2;
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Name = "UcCalenderDesktop";
		((Control)this).Size = new Size(311, 325);
		((Control)contextMenuStrip1).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)panel2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
