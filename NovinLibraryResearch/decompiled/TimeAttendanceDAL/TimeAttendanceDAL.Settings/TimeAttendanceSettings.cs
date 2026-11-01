using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendanceDAL.Settings;

[Serializable]
[DoNotPruneType]
public sealed class TimeAttendanceSettings
{
	private static ConcurrentDictionary<string, TimeAttendanceSettings> _dic = new ConcurrentDictionary<string, TimeAttendanceSettings>();

	private Font _fontProgram;

	public static SemaphoreSlim _getSettingSemaphore = new SemaphoreSlim(1);

	public Payamak Payamak { get; set; }

	public WebServiceKey WebServiceKey { get; set; }

	public SaleKari SaleKari { get; set; }

	public CodeingAccounting CodeingAccounting { get; set; }

	public LaborLaw LaborLaw { get; set; }

	public UpdateDetailsClass UpdateDetails { get; set; }

	public Font FontProgram
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0027: Expected O, but got Unknown
			Font obj = _fontProgram;
			if (obj == null)
			{
				Font val = new Font(new FontFamily("Tahoma"), 8f, (FontStyle)1);
				Font val2 = val;
				_fontProgram = val;
				obj = val2;
			}
			return obj;
		}
		set
		{
			_fontProgram = value;
		}
	}

	public int GetTaradodFormCountDaysAgo { get; set; } = 365;


	public bool AutoEzafe { get; set; }

	public bool CalcEzafeOutOfShift { get; set; } = true;


	public int RoundBelowPrice { get; set; } = 1;


	public bool NotKarkardWhenNoGharardad { get; set; }

	public bool ViewEndedGharardad { get; set; }

	public bool FromDateEndedGharardad { get; set; }

	public int GharardadEndedAgo { get; set; }

	public int GharardadEndedNext { get; set; }

	public List<GharardadStatus> GharardadStatuses { get; set; }

	public int ZaribEzafeDay { get; set; }

	public int ZaribFriday { get; set; }

	public int ZaribMinesBimeOfTax { get; set; }

	public int ZaribTatil { get; set; }

	public int MaxTedadOlaadInSalary { get; set; }

	public string AppRunAfterLogOnPerson { get; set; }

	public bool IsRunAppAfterLogOnPerson { get; set; }

	public int Rasmi { get; set; }

	public int GhRasmi { get; set; }

	public string Version { get; set; }

	public bool RunInStart
	{
		get
		{
			return Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "Hozor", "").ToString().Trim() != "";
		}
		set
		{
			if (value)
			{
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "Hozor", Application.ExecutablePath);
				Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "Hozor", Application.ExecutablePath);
			}
			else
			{
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "Hozor", "");
				Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "Hozor", "");
			}
		}
	}

	public bool IsGharardadDetConvertedToJson { get; set; }

	public bool IsGharardadDetUpdateTypeAccount { get; set; }

	public bool GharardadDetUpdateTypeAccountFromFishHesab { get; set; }

	public bool IsFishDetConvertedToJson { get; set; }

	public short IsDisplayMessageGheybat { get; set; } = -1;


	public short IsEnabled { get; set; } = -1;


	public short LoginSt { get; set; }

	public bool AutomaticCalcMovetoNextDay { get; set; } = true;


	public bool CalcEkWhenHasVacationOrMission { get; set; }

	public bool NotCalkEK_KK { get; set; }

	public bool CalcEkInSourceDate { get; set; }

	public static Font appFont { get; set; } = new Font("arial", 12f);


	public static async Task<TimeAttendanceSettings> GeTimeAttendanceSettingsAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		TimeAttendanceSettings ret = null;
		try
		{
			_dic.TryGetValue(contextGuid, out ret);
			if (ret != null)
			{
				return ret;
			}
			semaphoreHasBeenTaken = await _getSettingSemaphore.WaitAsync(60000, token);
			string text = await SettingAttendance.GetValueAsync(contextGuid, "TimeAttendanceApplicationSettings", 0, token);
			TimeAttendanceSettings timeAttendanceSettings;
			if (text.Trim() != "")
			{
				text = ChangeFieldValueFromStrToLong(text);
				ret = JsonConvert.DeserializeObject<TimeAttendanceSettings>(text);
				if (ret != null)
				{
					timeAttendanceSettings = ret;
					if (timeAttendanceSettings.CodeingAccounting == null)
					{
						timeAttendanceSettings.CodeingAccounting = new CodeingAccounting();
					}
					timeAttendanceSettings = ret;
					if (timeAttendanceSettings.UpdateDetails == null)
					{
						timeAttendanceSettings.UpdateDetails = new UpdateDetailsClass();
					}
					if (ret.CodeingAccounting.RewardId == 0)
					{
						await ret.CodeingAccounting.InitSalaryIdsAsync(contextGuid, token);
					}
					timeAttendanceSettings = ret;
					if (timeAttendanceSettings.LaborLaw == null)
					{
						timeAttendanceSettings.LaborLaw = new LaborLaw();
					}
					timeAttendanceSettings = ret;
					if (timeAttendanceSettings.WebServiceKey == null)
					{
						timeAttendanceSettings.WebServiceKey = new WebServiceKey();
					}
				}
			}
			else
			{
				ret = new TimeAttendanceSettings
				{
					Payamak = new Payamak(),
					WebServiceKey = new WebServiceKey(),
					SaleKari = new SaleKari(),
					CodeingAccounting = new CodeingAccounting(),
					LaborLaw = new LaborLaw(),
					UpdateDetails = new UpdateDetailsClass()
				};
			}
			if (ret.IsDisplayMessageGheybat == -1)
			{
				TimeAttendanceSettings timeAttendanceSettings2 = ret;
				timeAttendanceSettings2.IsDisplayMessageGheybat = ((await SettingAttendance.GetBoolValueAsync(contextGuid, "DisplayMessageGheybatInGharardad", def: true, 0, token)) ? ((short)1) : ((short)0));
				await ret.SaveAsync(contextGuid, token);
			}
			SaleKari saleKari = ret.SaleKari;
			DateTime dateM = (ret.SaleKari.DateM2 = DateTime.Now);
			saleKari.DateM1 = dateM;
			ret.LoginSt = 0;
			appFont = ret.FontProgram;
			_dic.TryRemove(contextGuid, out timeAttendanceSettings);
			_dic.TryAdd(contextGuid, ret);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_getSettingSemaphore.Release();
			}
		}
		return ret;
	}

	private static string ChangeFieldValueFromStrToLong(string ret)
	{
		try
		{
			ret = ret.Replace("\"MoeinBaseSalary\": \"\",", "\"MoeinBaseSalary\": 0,");
			ret = ret.Replace("\"TafsilBaseSalary\": \"\",", "\"TafsilBaseSalary\": 0,");
			ret = ret.Replace("\"MoeinOverTime\": \"\",", "\"MoeinOverTime\": 0,");
			ret = ret.Replace("\"TafsilOverTime\": \"\",", "\"TafsilOverTime\": 0,");
			ret = ret.Replace("\"MoeinShiftTime\": \"\",", "\"MoeinShiftTime\": 0,");
			ret = ret.Replace("\"TafsilShiftTime\": \"\",", "\"TafsilShiftTime\": 0,");
			ret = ret.Replace("\"MoeinHolidayTime\": \"\",", "\"MoeinHolidayTime\": 0,");
			ret = ret.Replace("\"TafsilHolidayTime\": \"\",", "\"TafsilHolidayTime\": 0,");
			ret = ret.Replace("\"MoeinNightTime\": \"\",", "\"MoeinNightTime\": 0,");
			ret = ret.Replace("\"TafsilNightTime\": \"\",", "\"TafsilNightTime\": 0,");
			ret = ret.Replace("\"MoeinVacation\": \"\",", "\"MoeinVacation\": 0,");
			ret = ret.Replace("\"TafsilVacation\": \"\",", "\"TafsilVacation\": 0,");
			ret = ret.Replace("\"MoeinLeaveTime\": \"\",", "\"MoeinLeaveTime\": 0,");
			ret = ret.Replace("\"TafsilLeaveTime\": \"\",", "\"TafsilLeaveTime\": 0,");
			ret = ret.Replace("\"MoeinAbsentTime\": \"\",", "\"MoeinAbsentTime\": 0,");
			ret = ret.Replace("\"TafsilAbsentTime\": \"\",", "\"TafsilAbsentTime\": 0,");
			ret = ret.Replace("\"MoeinEmployer\": \"\",", "\"MoeinEmployer\": 0,");
			ret = ret.Replace("\"TafsilEmployer\": \"\",", "\"TafsilEmployer\": 0,");
			ret = ret.Replace("\"MoeinUnEmployMent\": \"\",", "\"MoeinUnEmployMent\": 0,");
			ret = ret.Replace("\"TafsilUnEmployMent\": \"\",", "\"TafsilUnEmployMent\": 0,");
			ret = ret.Replace("\"TafsilTaminGov\": \"\",", "\"TafsilTaminGov\": 0,");
			ret = ret.Replace("\"MoeinTaminGov\": \"\",", "\"MoeinTaminGov\": 0,");
			ret = ret.Replace("\"MoeinTaxGov\": \"\",", "\"MoeinTaxGov\": 0,");
			ret = ret.Replace("\"TafsilTaxGov\": \"\",", "\"TafsilTaxGov\": 0,");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChangeFieldValueFromStrToLong", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\TimeAttendanceSettings.cs", 216);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			string value = JsonConvert.SerializeObject((object)this, (Formatting)1);
			await SettingAttendance.SetValueAsync(contextGuid, "TimeAttendanceApplicationSettings", value, 0, token);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue((ReturnedState)2, ex.Message, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\TimeAttendanceSettings.cs", 293);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\TimeAttendanceSettings.cs", 294);
		}
		return ret;
	}
}
