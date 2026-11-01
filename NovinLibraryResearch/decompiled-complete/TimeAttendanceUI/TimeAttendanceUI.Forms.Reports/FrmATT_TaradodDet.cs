using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;

namespace TimeAttendanceUI.Forms.Reports;

public class FrmATT_TaradodDet : BaseForm
{
	private EN_TypeAtt _type;

	private DateTime _DateM;

	private TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson;

	private GharardadNew ClsGharardad;

	private string strUserLog = "";

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private IContainer components;

	private PanelEx panelEx1;

	private GroupPanel groupPanel1;

	private CheckBox CBBound;

	private Label label1;

	private NumericUpDown StartM;

	private NumericUpDown StartH;

	private Label label5;

	private NumericUpDown DurationM;

	private NumericUpDown DurationH;

	private Label label3;

	private NumericUpDown EndM;

	private NumericUpDown EndH;

	private Label label2;

	private ButtonX BTNOK;

	private ButtonX BTNCancel;

	private TextBoxX TXTDescription;

	private CheckBox CBManfi;

	private Label label4;

	private Panel panel1;

	private Label label7;

	private Label label6;

	public string DateSH => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(_DateM);

	public string Description
	{
		get
		{
			return Text.FixString(((Control)TXTDescription).Text).Trim();
		}
		set
		{
			((Control)TXTDescription).Text = Text.FixString(value).Trim();
		}
	}

	public DateTime? StartTime
	{
		get
		{
			try
			{
				if (CBBound.Checked)
				{
					return new DateTime(_DateM.Year, _DateM.Month, _DateM.Day, int.Parse(StartH.Value.ToString()), int.Parse(StartM.Value.ToString()), 0);
				}
				return null;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "StartTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmATT_TaradodDet.cs", 56);
				return null;
			}
		}
		set
		{
			if (value.HasValue)
			{
				_DateM = value.Value;
				CBBound.Checked = true;
				StartH.Value = value.Value.Hour;
				StartM.Value = value.Value.Minute;
			}
			else
			{
				CBBound.Checked = false;
				StartH.Value = 0m;
				StartM.Value = 0m;
			}
		}
	}

	public DateTime? EndTime
	{
		get
		{
			try
			{
				if (CBBound.Checked)
				{
					return new DateTime(_DateM.Year, _DateM.Month, _DateM.Day, int.Parse(EndH.Value.ToString()), int.Parse(EndM.Value.ToString()), 0);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, $"endh={EndH.Value} , endM={EndM.Value}", "EndTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmATT_TaradodDet.cs", 88);
			}
			return null;
		}
		set
		{
			if (value.HasValue)
			{
				CBBound.Checked = true;
				EndH.Value = value.Value.Hour;
				EndM.Value = value.Value.Minute;
			}
			else
			{
				CBBound.Checked = false;
				EndH.Value = 0m;
				EndM.Value = 0m;
			}
		}
	}

	public int Duration
	{
		get
		{
			if (CBManfi.Checked)
			{
				return -(int)DurationH.Value * 60 - (int)DurationM.Value;
			}
			return (int)DurationH.Value * 60 + (int)DurationM.Value;
		}
		set
		{
			try
			{
				DurationM.ValueChanged -= DurationM_ValueChanged;
				DurationH.ValueChanged -= DurationM_ValueChanged;
				CBManfi.Checked = value < 0;
				DurationH.Value = 0m;
				int num;
				for (num = Math.Abs(value); num >= 60; num -= 60)
				{
					NumericUpDown durationH = DurationH;
					durationH.Value += 1m;
				}
				DurationM.Value = num;
				DurationM.ValueChanged += DurationM_ValueChanged;
				DurationH.ValueChanged += DurationM_ValueChanged;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Duration", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmATT_TaradodDet.cs", 135);
			}
		}
	}

	private async Task LoadDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			await SetTrDictionaryAsync(token);
			ClsGharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM, token);
			ATT_TaradodDet temp = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM, token);
			TaradodFullDetView temp2 = null;
			if (_type == EN_TypeAtt.Vacation)
			{
				List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
				temp2 = await TaradodFullDet.GetAsync_(((CacheBaseForm)this).ContextGuid, devices, _DateM, clsPerson, ClsGharardad, checkValidationVorodAndKHoroj: false, token);
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((BaseForm)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!((BaseForm)this).IsDisposed && !token.IsCancellationRequested)
					{
						string value;
						switch (_type)
						{
						case EN_TypeAtt.none:
						{
							trDictionary.TryGetValue("[نامشخص]", out value);
							GroupPanel obj9 = groupPanel1;
							string text2 = (((Control)TXTDescription).Text = value);
							((Control)obj9).Text = text2;
							Description = "";
							Duration = 0;
							StartTime = null;
							EndTime = null;
							break;
						}
						case EN_TypeAtt.EzaferKar:
						{
							trDictionary.TryGetValue("[اضافه کار]", out value);
							GroupPanel obj12 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj12).Text = text2;
							if (!string.IsNullOrEmpty(temp.EKDesc))
							{
								Description = temp.EKDesc;
							}
							if (temp.EKStart.HasValue)
							{
								StartTime = temp.EKStart;
							}
							if (temp.EKEnd.HasValue)
							{
								EndTime = temp.EKEnd;
							}
							if (temp.EKTime != 0)
							{
								Duration = temp.EKTime;
							}
							else if (EndTime.HasValue && StartTime.HasValue)
							{
								TimeSpan timeSpan2 = EndTime.Value.Subtract(StartTime.Value);
								Duration = (int)Math.Round(timeSpan2.TotalMinutes);
							}
							break;
						}
						case EN_TypeAtt.KasrKar:
						{
							trDictionary.TryGetValue("[کسرکار]", out value);
							GroupPanel obj10 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj10).Text = text2;
							Description = temp.KKDesc;
							Duration = temp.KKTime;
							StartTime = temp.KKStart;
							EndTime = temp.KKEnd;
							break;
						}
						case EN_TypeAtt.Mission:
						{
							trDictionary.TryGetValue("[ماموریت]", out value);
							GroupPanel obj11 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj11).Text = text2;
							if (!string.IsNullOrEmpty(temp.MissionDesc))
							{
								Description = temp.MissionDesc;
							}
							if (temp.MissionStart.HasValue)
							{
								StartTime = temp.MissionStart;
							}
							if (temp.MissionEnd.HasValue)
							{
								EndTime = temp.MissionEnd;
							}
							if (temp.MissionTime != 0)
							{
								Duration = temp.MissionTime;
							}
							else if (EndTime.HasValue && StartTime.HasValue)
							{
								TimeSpan timeSpan = EndTime.Value.Subtract(StartTime.Value);
								Duration = (int)Math.Round(timeSpan.TotalMinutes);
							}
							break;
						}
						case EN_TypeAtt.Vacation:
						{
							int num = temp2.WorkOnTypeTaradod((TypeTaradodEnumType)5);
							trDictionary.TryGetValue("[مرخصی]", out value);
							GroupPanel obj8 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj8).Text = text2;
							Description = temp.VacationDesc;
							Duration = temp.VacationTime;
							int num2 = num - Duration;
							if (num2 + Duration < 0)
							{
								Duration = -num2;
							}
							StartTime = temp.VacationStart;
							EndTime = temp.VacationEnd;
							break;
						}
						case EN_TypeAtt.Friday:
						{
							trDictionary.TryGetValue("[جمعه کاری]", out value);
							GroupPanel obj7 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj7).Text = text2;
							Description = temp.FridayDesc;
							Duration = temp.FridayTime;
							StartTime = temp.FridayStart;
							EndTime = temp.FridayEnd;
							break;
						}
						case EN_TypeAtt.NobatKari1:
						{
							trDictionary.TryGetValue("[نوبت کاری صبح]", out value);
							GroupPanel obj6 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj6).Text = text2;
							Description = temp.ShiftDesc1;
							Duration = temp.ShiftTime1;
							StartTime = temp.ShiftStart1;
							EndTime = temp.ShiftEnd1;
							break;
						}
						case EN_TypeAtt.NobatKari2:
						{
							trDictionary.TryGetValue("[نوبت کاری عصر]", out value);
							GroupPanel obj5 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj5).Text = text2;
							Description = temp.ShiftDesc2;
							Duration = temp.ShiftTime2;
							StartTime = temp.ShiftStart2;
							EndTime = temp.ShiftEnd2;
							break;
						}
						case EN_TypeAtt.NobatKari3:
						{
							trDictionary.TryGetValue("[نوبت کاری شب]", out value);
							GroupPanel obj4 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj4).Text = text2;
							Description = temp.ShiftDesc3;
							Duration = temp.ShiftTime3;
							StartTime = temp.ShiftStart3;
							EndTime = temp.ShiftEnd3;
							break;
						}
						case EN_TypeAtt.Absent:
						{
							trDictionary.TryGetValue("[غیبت]", out value);
							GroupPanel obj3 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj3).Text = text2;
							Description = temp.AbsentDesc;
							Duration = temp.AbsentTime;
							StartTime = temp.AbsendStart;
							EndTime = temp.AbsentEnd;
							break;
						}
						case EN_TypeAtt.Night:
						{
							trDictionary.TryGetValue("[شب کاری]", out value);
							GroupPanel obj2 = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj2).Text = text2;
							Description = temp.NightDesc;
							Duration = temp.NightTime;
							StartTime = temp.NightStart;
							EndTime = temp.NightEnd;
							break;
						}
						default:
						{
							trDictionary.TryGetValue("[نامشخص]", out value);
							GroupPanel obj = groupPanel1;
							string text2 = (((Control)this).Text = value);
							((Control)obj).Text = text2;
							Description = "";
							Duration = 0;
							StartTime = null;
							EndTime = null;
							break;
						}
						}
						if (Duration > 0)
						{
							trDictionary.TryGetValue("[ویرایش]", out var value2);
							trDictionary.TryGetValue("[مدیریتی]", out var value3);
							trDictionary.TryGetValue("[مدت]", out var value4);
							trDictionary.TryGetValue("[قبل از ویرایش]", out var value5);
							trDictionary.TryGetValue("[از ساعت]", out var value6);
							trDictionary.TryGetValue("[الی]", out var value7);
							strUserLog = $"{value2} {((Control)this).Text} {value3} {((PersonBase)clsPerson).FullName} {value4} {((Control)this).Text} {value5} {DurationM.Value}:{DurationH.Value} {value6} {StartM.Value}:{StartH.Value} {value7} {EndM.Value}:{EndH.Value}";
						}
						else
						{
							trDictionary.TryGetValue("[افزودن]", out var value8);
							trDictionary.TryGetValue("[مدیریتی]", out var value9);
							strUserLog = value8 + " " + ((Control)this).Text + " " + value9 + " " + ((PersonBase)clsPerson).FullName;
						}
						trDictionary.TryGetValue("[InDate]", out var value10);
						strUserLog = strUserLog + value10 + ":" + DateSH;
						StartM.ValueChanged += Items_Changed;
						EndM.ValueChanged += Items_Changed;
						StartH.ValueChanged += Items_Changed;
						EndH.ValueChanged += Items_Changed;
						CBBound.CheckedChanged += Items_Changed;
						DurationM.ValueChanged += DurationM_ValueChanged;
						DurationH.ValueChanged += DurationM_ValueChanged;
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private Task SetTrDictionaryAsync(CancellationToken token)
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("", out var value))
			{
				trDictionary.Add("[نامشخص]", Utilities.GetTranslate("[نامشخص]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[اضافه کار]", Utilities.GetTranslate("[اضافه کار]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[کسرکار]", Utilities.GetTranslate("[کسرکار]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[ماموریت]", Utilities.GetTranslate("[ماموریت]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[مرخصی]", Utilities.GetTranslate("[مرخصی]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[جمعه کاری]", Utilities.GetTranslate("[جمعه کاری]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[نوبت کاری صبح]", Utilities.GetTranslate("[نوبت کاری صبح]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[نوبت کاری عصر]", Utilities.GetTranslate("[نوبت کاری عصر]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[نوبت کاری شب]", Utilities.GetTranslate("[نوبت کاری شب]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[غیبت]", Utilities.GetTranslate("[غیبت]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[شب کاری]", Utilities.GetTranslate("[شب کاری]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[ویرایش]", Utilities.GetTranslate("[ویرایش]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[مدیریتی]", Utilities.GetTranslate("[مدیریتی]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[مدت]", Utilities.GetTranslate("[مدت]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[قبل از ویرایش]", Utilities.GetTranslate("[قبل از ویرایش]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[از ساعت]", Utilities.GetTranslate("[از ساعت]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[الی]", Utilities.GetTranslate("[الی]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[افزودن]", Utilities.GetTranslate("[افزودن]"));
			}
			if (!trDictionary.TryGetValue("", out value))
			{
				trDictionary.Add("[InDate]", Utilities.GetTranslate("[InDate]"));
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmATT_TaradodDet.cs", 360);
			throw;
		}
	}

	public FrmATT_TaradodDet(string contextGuid, EN_TypeAtt Type, TimeAttendanceDAL.Dal.FLRPerson.Person pPerson, DateTime pdate, DateTime? endDate = null, string description = "")
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			_type = Type;
			StartTime = pdate.Date;
			EndTime = endDate?.Date;
			Description = description;
			clsPerson = pPerson;
			((Control)panel1).Enabled = CBBound.Checked;
			Task.Run(() => LoadDataAsync());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmATT_TaradodDet.cs", 380);
		}
	}

	private async void BTNOK_Click(object sender, EventArgs e)
	{
		_ = 22;
		try
		{
			switch (_type)
			{
			case EN_TypeAtt.EzaferKar:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.EKDesc = Description;
				cls10.EKStart = StartTime;
				cls10.EKEnd = EndTime;
				cls10.EKTime = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.KasrKar:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.KKDesc = Description;
				cls10.KKStart = StartTime;
				cls10.KKEnd = EndTime;
				cls10.KKTime = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.Mission:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.MissionDesc = Description;
				cls10.MissionStart = StartTime;
				cls10.MissionEnd = EndTime;
				cls10.MissionTime = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.Vacation:
			{
				List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid);
				TaradodFullDetView temp2 = await TaradodFullDet.GetAsync_(((CacheBaseForm)this).ContextGuid, devices, _DateM, clsPerson, ClsGharardad, checkValidationVorodAndKHoroj: false);
				int x = temp2.WorkOnTypeTaradod((TypeTaradodEnumType)5);
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.VacationDesc = Description;
				cls10.VacationStart = StartTime;
				cls10.VacationEnd = EndTime;
				int mainMorakhasi = x - cls10.VacationTime;
				int num = await temp2.GetKasrKarAsync() + cls10.VacationTime;
				if (Duration > num)
				{
					cls10.VacationTime = num;
				}
				else if (mainMorakhasi + Duration < 0)
				{
					cls10.VacationTime = -mainMorakhasi;
				}
				else
				{
					cls10.VacationTime = Duration;
				}
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.Friday:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.FridayDesc = Description;
				cls10.FridayStart = StartTime;
				cls10.FridayEnd = EndTime;
				cls10.FridayTime = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.NobatKari1:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.ShiftDesc1 = Description;
				cls10.ShiftStart1 = StartTime;
				cls10.ShiftEnd1 = EndTime;
				cls10.ShiftTime1 = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.NobatKari2:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.ShiftDesc2 = Description;
				cls10.ShiftStart2 = StartTime;
				cls10.ShiftEnd2 = EndTime;
				cls10.ShiftTime2 = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.NobatKari3:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.ShiftDesc3 = Description;
				cls10.ShiftStart3 = StartTime;
				cls10.ShiftEnd3 = EndTime;
				cls10.ShiftTime3 = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.Absent:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.AbsentDesc = Description;
				cls10.AbsendStart = StartTime;
				cls10.AbsentEnd = EndTime;
				cls10.AbsentTime = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case EN_TypeAtt.Night:
			{
				ATT_TaradodDet cls10 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, _DateM);
				cls10.NightDesc = Description;
				cls10.NightStart = StartTime;
				cls10.NightEnd = EndTime;
				cls10.NightTime = Duration;
				await cls10.SaveAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			}
			if ((int)((Form)this).DialogResult == 1)
			{
				if (strUserLog.StartsWith(Utilities.GetTranslate("[ویرایش]")))
				{
					strUserLog += " [و مدت بعد از ویرایش : ]";
				}
				else
				{
					strUserLog += " [ به مدت :] ";
				}
				strUserLog = strUserLog + DurationM.Value + " : " + DurationH.Value + " [از ساعت] " + StartM.Value + " : " + StartH.Value + "[الی] " + EndM.Value + " : " + EndH.Value;
				LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, strUserLog, "Taradod.TaradodEditManagement");
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmATT_TaradodDet.cs", 526);
		}
	}

	private void BTNCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmATT_TaradodDet.cs", 535);
		}
	}

	private void FRMATT_TaradodDet_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		Keys keyCode = e.KeyCode;
		if ((int)keyCode != 13)
		{
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					((PopupItemControl)BTNOK).PerformClick();
				}
			}
			else
			{
				((PopupItemControl)BTNCancel).PerformClick();
			}
		}
		else
		{
			((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
		}
	}

	private void Items_Changed(object sender, EventArgs e)
	{
		((Control)panel1).Enabled = CBBound.Checked;
		if (CBBound.Checked && StartTime.HasValue && EndTime.HasValue && StartTime < EndTime)
		{
			Duration = (int)Math.Round(EndTime.Value.Subtract(StartTime.Value).TotalMinutes);
		}
	}

	private void DurationM_ValueChanged(object sender, EventArgs e)
	{
		if (((Control)DurationM).Focused && CBBound.Checked)
		{
			EndTime = StartTime.Value.AddMinutes(Duration);
		}
	}

	private void CBManfi_CheckedChanged(object sender, EventArgs e)
	{
		if (CBManfi.Checked)
		{
			NumericUpDown durationM = DurationM;
			Color backColor = (((Control)DurationH).BackColor = Color.Red);
			((Control)durationM).BackColor = backColor;
		}
		else
		{
			NumericUpDown durationM2 = DurationM;
			Color backColor = (((Control)DurationH).BackColor = Color.Green);
			((Control)durationM2).BackColor = backColor;
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
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0ede: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee8: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		BTNOK = new ButtonX();
		BTNCancel = new ButtonX();
		groupPanel1 = new GroupPanel();
		panel1 = new Panel();
		EndM = new NumericUpDown();
		label1 = new Label();
		StartH = new NumericUpDown();
		label7 = new Label();
		label6 = new Label();
		StartM = new NumericUpDown();
		EndH = new NumericUpDown();
		label2 = new Label();
		CBManfi = new CheckBox();
		CBBound = new CheckBox();
		TXTDescription = new TextBoxX();
		label5 = new Label();
		label4 = new Label();
		DurationM = new NumericUpDown();
		DurationH = new NumericUpDown();
		label3 = new Label();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)EndM).BeginInit();
		((ISupportInitialize)StartH).BeginInit();
		((ISupportInitialize)StartM).BeginInit();
		((ISupportInitialize)EndH).BeginInit();
		((ISupportInitialize)DurationM).BeginInit();
		((ISupportInitialize)DurationH).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BTNOK);
		((Control)panelEx1).Controls.Add((Control)(object)BTNCancel);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(296, 295);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BTNOK).AccessibleRole = (AccessibleRole)43;
		((Control)BTNOK).Anchor = (AnchorStyles)6;
		BTNOK.ColorTable = (eButtonColor)3;
		((Control)BTNOK).Location = new Point(138, 247);
		((Control)BTNOK).Name = "BTNOK";
		((Control)BTNOK).Size = new Size(120, 36);
		((PopupItemControl)BTNOK).Style = (eDotNetBarStyle)9;
		((Control)BTNOK).TabIndex = 9;
		((Control)BTNOK).Text = "[تائید]";
		((Control)BTNOK).Click += BTNOK_Click;
		((Control)BTNCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BTNCancel).Anchor = (AnchorStyles)6;
		BTNCancel.ColorTable = (eButtonColor)3;
		((Control)BTNCancel).Location = new Point(12, 247);
		((Control)BTNCancel).Name = "BTNCancel";
		((Control)BTNCancel).Size = new Size(120, 36);
		((PopupItemControl)BTNCancel).Style = (eDotNetBarStyle)9;
		((Control)BTNCancel).TabIndex = 10;
		((Control)BTNCancel).Text = "[لغو]";
		((Control)BTNCancel).Click += BTNCancel_Click;
		((Control)groupPanel1).Anchor = (AnchorStyles)15;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)panel1);
		((Control)groupPanel1).Controls.Add((Control)(object)CBManfi);
		((Control)groupPanel1).Controls.Add((Control)(object)CBBound);
		((Control)groupPanel1).Controls.Add((Control)(object)TXTDescription);
		((Control)groupPanel1).Controls.Add((Control)(object)label5);
		((Control)groupPanel1).Controls.Add((Control)(object)label4);
		((Control)groupPanel1).Controls.Add((Control)(object)DurationM);
		((Control)groupPanel1).Controls.Add((Control)(object)DurationH);
		((Control)groupPanel1).Controls.Add((Control)(object)label3);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(12, 12);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(272, 229);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 0;
		((Control)groupPanel1).Text = "[زمان]";
		((Control)panel1).Controls.Add((Control)(object)EndM);
		((Control)panel1).Controls.Add((Control)(object)label1);
		((Control)panel1).Controls.Add((Control)(object)StartH);
		((Control)panel1).Controls.Add((Control)(object)label7);
		((Control)panel1).Controls.Add((Control)(object)label6);
		((Control)panel1).Controls.Add((Control)(object)StartM);
		((Control)panel1).Controls.Add((Control)(object)EndH);
		((Control)panel1).Controls.Add((Control)(object)label2);
		((Control)panel1).Location = new Point(-3, -6);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(241, 66);
		((Control)panel1).TabIndex = 1;
		((Control)EndM).Location = new Point(99, 39);
		EndM.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)EndM).Name = "EndM";
		((Control)EndM).Size = new Size(64, 20);
		((Control)EndM).TabIndex = 4;
		((UpDownBase)EndM).TextAlign = (HorizontalAlignment)2;
		((Control)label1).Location = new Point(76, 7);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(17, 20);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = ":";
		((Control)StartH).Location = new Point(6, 7);
		StartH.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)StartH).Name = "StartH";
		((Control)StartH).Size = new Size(64, 20);
		((Control)StartH).TabIndex = 1;
		((UpDownBase)StartH).TextAlign = (HorizontalAlignment)2;
		((Control)label7).Anchor = (AnchorStyles)9;
		((Control)label7).Location = new Point(169, 37);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(58, 20);
		((Control)label7).TabIndex = 4;
		((Control)label7).Text = "[زمان پایان]:";
		label7.TextAlign = (ContentAlignment)64;
		((Control)label6).Anchor = (AnchorStyles)9;
		((Control)label6).Location = new Point(170, 7);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(73, 20);
		((Control)label6).TabIndex = 0;
		((Control)label6).Text = "[زمان شروع]:";
		label6.TextAlign = (ContentAlignment)64;
		((Control)StartM).Location = new Point(100, 7);
		StartM.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)StartM).Name = "StartM";
		((Control)StartM).Size = new Size(64, 20);
		((Control)StartM).TabIndex = 2;
		((UpDownBase)StartM).TextAlign = (HorizontalAlignment)2;
		((Control)EndH).Location = new Point(6, 39);
		EndH.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)EndH).Name = "EndH";
		((Control)EndH).Size = new Size(64, 20);
		((Control)EndH).TabIndex = 3;
		((UpDownBase)EndH).TextAlign = (HorizontalAlignment)2;
		((Control)label2).Location = new Point(76, 39);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(17, 20);
		((Control)label2).TabIndex = 6;
		((Control)label2).Text = ":";
		((Control)CBManfi).Anchor = (AnchorStyles)13;
		((Control)CBManfi).Location = new Point(-7, 96);
		((Control)CBManfi).Name = "CBManfi";
		((Control)CBManfi).Size = new Size(168, 20);
		((Control)CBManfi).TabIndex = 7;
		((Control)CBManfi).Text = "[زمان منفی (کاهنده)]";
		((ButtonBase)CBManfi).UseVisualStyleBackColor = true;
		CBManfi.CheckedChanged += CBManfi_CheckedChanged;
		((Control)CBBound).Anchor = (AnchorStyles)13;
		((Control)CBBound).Location = new Point(158, 3);
		((Control)CBBound).Name = "CBBound";
		((Control)CBBound).Size = new Size(102, 20);
		((Control)CBBound).TabIndex = 0;
		((ButtonBase)CBBound).TextAlign = (ContentAlignment)64;
		((ButtonBase)CBBound).UseVisualStyleBackColor = true;
		((TextBox)TXTDescription).AcceptsReturn = true;
		((TextBoxBase)TXTDescription).AcceptsTab = true;
		((Control)TXTDescription).AllowDrop = true;
		((Control)TXTDescription).Anchor = (AnchorStyles)15;
		TXTDescription.AutoSelectAll = true;
		TXTDescription.Border.Class = "TextBoxBorder";
		TXTDescription.Border.CornerType = (eCornerType)1;
		TXTDescription.IsEnterInputKey = true;
		((Control)TXTDescription).Location = new Point(3, 122);
		((TextBoxBase)TXTDescription).Multiline = true;
		((Control)TXTDescription).Name = "TXTDescription";
		((Control)TXTDescription).Size = new Size(260, 83);
		((Control)TXTDescription).TabIndex = 8;
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).Location = new Point(203, 99);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(60, 20);
		((Control)label5).TabIndex = 7;
		((Control)label5).Text = "[شرح] :";
		label5.TextAlign = (ContentAlignment)64;
		((Control)label4).Location = new Point(167, 68);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(70, 20);
		((Control)label4).TabIndex = 2;
		((Control)label4).Text = "[مدت] :";
		label4.TextAlign = (ContentAlignment)64;
		((Control)DurationM).Location = new Point(96, 68);
		DurationM.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)DurationM).Name = "DurationM";
		((Control)DurationM).Size = new Size(64, 20);
		((Control)DurationM).TabIndex = 6;
		((UpDownBase)DurationM).TextAlign = (HorizontalAlignment)2;
		((Control)DurationH).Location = new Point(3, 68);
		DurationH.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)DurationH).Name = "DurationH";
		((Control)DurationH).Size = new Size(64, 20);
		((Control)DurationH).TabIndex = 5;
		((UpDownBase)DurationH).TextAlign = (HorizontalAlignment)2;
		((Control)label3).Location = new Point(73, 68);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(17, 20);
		((Control)label3).TabIndex = 4;
		((Control)label3).Text = ":";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(296, 295);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmATT_TaradodDet";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[زمان]";
		((Control)this).KeyDown += new KeyEventHandler(FRMATT_TaradodDet_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)EndM).EndInit();
		((ISupportInitialize)StartH).EndInit();
		((ISupportInitialize)StartM).EndInit();
		((ISupportInitialize)EndH).EndInit();
		((ISupportInitialize)DurationM).EndInit();
		((ISupportInitialize)DurationH).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
