using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Attendance.InterFaces;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Mapper;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.View.Taradod;
using TimeAttendanceDevices;

namespace TimeAttendanceDAL.Dal.Taradod;

public class LogOnPersonel : ILogOnPersonel, IHasContext, IDalSaveAttLogOnPerson
{
	public delegate void OnSaveTaradodEventHandler(int current, int total, short st);

	public static SemaphoreSlim semafore_SaveAsync = new SemaphoreSlim(1, 1);

	public static SemaphoreSlim semafore_CalcPersonId = new SemaphoreSlim(1, 1);

	public static LogOnPersonel InstanceForEvents { get; } = new LogOnPersonel("");


	public string ContextGuid { get; set; }

	public int Id { get; set; }

	public DateTime DateTimeLogM { get; set; } = DateTime.MinValue;


	public string DatetimeCalture => Calendar.MiladiToCalture(DateTimeLogM) + " " + DateTimeLogM.TimeOfDay.ToString().Substring(0, 5);

	public long TrafficId { get; set; }

	public int PersonNo { get; set; }

	public int PersonId { get; set; }

	public int DeviceId_ { get; set; }

	public int DeviceNo { get; set; }

	public int Duration { get; set; }

	public bool IsDeleted { get; set; }

	public SavedTaradodEnumType IsVirtual { get; set; }

	public TypeTaradodEnumType TypeTaradod { get; set; } = (TypeTaradodEnumType)1;


	public TypeTaradodEnumType TypeTaradodVacationOrMission { get; set; } = (TypeTaradodEnumType)1;


	public string DeviceSerial_ { get; set; }

	public string SmsRefrenceIDs { get; private set; } = "";


	public Guid Guid { get; set; } = Guid.NewGuid();


	public event EventNotification OnStatusErorTaradod;

	public event OnSaveTaradodEventHandler OnSaveTaradod;

	public async Task SetIDtrafficFromTypeTaradodAsync(CancellationToken token)
	{
		try
		{
			LogonTypeTaradod logonTypeTaradod = await LogonTypeTaradod.GetAsync(ContextGuid, DeviceSerial_, DeviceNo, TypeTaradod, token);
			if (logonTypeTaradod == null)
			{
				Log.Error("آی دی ترافیکی با مشخصات داده شده زیر پیدا نشد " + $"\r\n typeTaradod= {TypeTaradod}" + $"\r\n deviceNo= {DeviceNo} " + "\r\n deviceSerial= " + DeviceSerial_);
			}
			TrafficId = logonTypeTaradod?.TraficId ?? 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetIDtrafficFromTypeTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 108);
		}
	}

	public LogOnPersonel(string contextGuid, DateTime dateM)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		DateTimeLogM = dateM;
	}

	public LogOnPersonel(string contextGuid)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
	}

	public LogOnPersonel Clone()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return new LogOnPersonel(ContextGuid)
			{
				DateTimeLogM = DateTimeLogM,
				TrafficId = TrafficId,
				PersonNo = PersonNo,
				DeviceId_ = DeviceId_,
				TypeTaradod = TypeTaradod,
				TypeTaradodVacationOrMission = TypeTaradodVacationOrMission,
				Duration = Duration,
				IsVirtual = IsVirtual,
				PersonId = PersonId,
				DeviceSerial_ = DeviceSerial_,
				DeviceNo = DeviceNo
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Clone", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 157);
			throw;
		}
	}

	public static async Task CalcPersonIdAsync(string contextGuid, CancellationToken token)
	{
		bool semaforeHasBeenTaken = false;
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_LogonPersonel_UpdatePersonIds]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await semafore_CalcPersonId.WaitAsync();
					semaforeHasBeenTaken = true;
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (token.IsCancellationRequested)
					{
						return;
					}
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(new CancellationTokenSource(60000).Token);
					((DbConnection)(object)cn).Close();
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		finally
		{
			if (semaforeHasBeenTaken)
			{
				semafore_CalcPersonId?.Release();
			}
		}
	}

	public static async Task<int> GetMaxId(string contextGuid, CancellationToken token)
	{
		_ = 2;
		try
		{
			int id = 0;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("select MAX (id) as id from LogonPersonel", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["id"] != DBNull.Value)
					{
						id = (int)((DbDataReader)(object)dr)["id"];
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					return id;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<LogOnPersonel> GetAsync(string contextGuid, int id, CancellationToken token)
	{
		_ = 2;
		try
		{
			LogOnPersonel ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_LogonPersonel_SelectRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@id", (object)id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					if (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						ret = LoadData(contextGuid, dr);
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					return ret;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task SaveSmsRefrencedIdsAsync(string smsRefrencedIds, CancellationToken token = default(CancellationToken), short tryCount = 20)
	{
		try
		{
			if (!string.IsNullOrEmpty(SmsRefrenceIDs))
			{
				throw new NotImplementedException(" روی شیء جاری قبلا شماره ارجاع گرفته!!");
			}
			if (!string.IsNullOrEmpty((await GetAsync(ContextGuid, Id, token)).SmsRefrenceIDs))
			{
				return;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonPersonel_SetSmsRefrencedId", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)Id);
					cmd.Parameters.AddWithValue("SmsRefrenceIds", (object)smsRefrencedIds);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (tryCount <= 0)
			{
				Log.Error(ex2, $"logonpersonel id = {Id},smsRefrencedIds = {smsRefrencedIds}");
				throw;
			}
			await Task.Delay(5000);
			LogOnPersonel logOnPersonel = this;
			short num = (short)(tryCount - 1);
			tryCount = num;
			await logOnPersonel.SaveSmsRefrencedIdsAsync(smsRefrencedIds, token, num);
		}
	}

	public async Task<int> SaveAsync(string caller, CancellationToken token = default(CancellationToken), [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		bool semaphoreHasBeenTaken = false;
		JaegerMonitor monitor = new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 60000);
		try
		{
			if (DeviceId_ == 0)
			{
				List<DeviceInfo> list = await DeviceInfo.GetAllAsync(ContextGuid, token);
				if (list != null && list.Any())
				{
					string deviceSerial_ = DeviceSerial_;
					if (deviceSerial_ != null && deviceSerial_.Length > 0)
					{
						DeviceInfo deviceInfo = list.FirstOrDefault((DeviceInfo p) => p.SerialNumberStr == DeviceSerial_.Trim());
						if (deviceInfo != null)
						{
							DeviceId_ = deviceInfo.Id;
							DeviceNo = deviceInfo.DeviceNo;
						}
					}
					if (DeviceId_ == 0)
					{
						DeviceId_ = list[0].Id;
						DeviceNo = list[0].DeviceNo;
						DeviceSerial_ = list[0].SerialNumberStr;
					}
				}
			}
			if (PersonId == 0 && PersonNo > 0 && DeviceId_ > 0)
			{
				Person person = await Person.GetbyDeviceIdPersonNoAsync(ContextGuid, DeviceId_, PersonNo, token);
				if (person != null)
				{
					PersonId = ((PersonBase)person).Id;
				}
			}
			ReturnedSaveFuncInfo val = Validatetion(caller);
			if (val.HasError)
			{
				string text = "";
				foreach (string error in val.ErrorList)
				{
					int num = error.IndexOf('\r');
					if (num > 0)
					{
						if (text == error.Substring(0, num))
						{
							continue;
						}
						text = error.Substring(0, num);
					}
					Log.Error(error);
					TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, text + "\r\n جهت اطلاعات دقیق تر، خطاهای سیستمی را بررسی نمائید", "", 3);
				}
				foreach (string warning in val.WarningList)
				{
					Log.Warning(warning);
				}
				return -1;
			}
			if (val.HasWarning)
			{
				foreach (string warning2 in val.WarningList)
				{
					Log.Warning(warning2);
				}
			}
			try
			{
				await semafore_SaveAsync.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
				try
				{
					SqlCommand val2 = new SqlCommand("", cn);
					((DbCommand)val2).CommandType = CommandType.StoredProcedure;
					SqlCommand cmd = val2;
					try
					{
						cmd.Parameters.AddWithValue("DateTimeLogM", (object)DateTimeLogM);
						cmd.Parameters.AddWithValue("IDTraffic", (object)TrafficId);
						cmd.Parameters.AddWithValue("IsVirtual", (object)(short)(int)IsVirtual);
						cmd.Parameters.AddWithValue("TrafficType", (object)(short)(int)TypeTaradod);
						cmd.Parameters.AddWithValue("TrafficTypeVacationOrMission", (object)(short)(int)TypeTaradodVacationOrMission);
						cmd.Parameters.AddWithValue("TrafficName", (object)(Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod) + " / " + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradodVacationOrMission)));
						await ((DbConnection)(object)cn).OpenAsync(token);
						if (Id <= 0)
						{
							cmd.Parameters.AddWithValue("PersonId", (object)PersonId);
							cmd.Parameters.AddWithValue("PersonNO", (object)PersonNo);
							cmd.Parameters.AddWithValue("DeviceId", (object)DeviceId_);
							cmd.Parameters.AddWithValue("DeviceNo", (object)DeviceNo);
							cmd.Parameters.AddWithValue("DeviceSerial", (object)DeviceSerial_);
							cmd.Parameters.AddWithValue("IsDeleted", (object)IsDeleted);
							((DbCommand)(object)cmd).CommandText = "sp_LogonPersonel_Insert";
							SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
							try
							{
								if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["id"] != DBNull.Value)
								{
									Id = (int)((DbDataReader)(object)dr)["id"];
								}
							}
							finally
							{
								((IDisposable)dr)?.Dispose();
							}
						}
						else
						{
							((DbCommand)(object)cmd).CommandText = "sp_LogonPersonel_Update";
							cmd.Parameters.AddWithValue("Id", (object)Id);
							cmd.Parameters.AddWithValue("Duration", (object)Duration);
							SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
							try
							{
								if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["id"] != DBNull.Value)
								{
									Id = (int)((DbDataReader)(object)dr)["id"];
								}
							}
							finally
							{
								((IDisposable)dr)?.Dispose();
							}
						}
						((DbConnection)(object)cn).Close();
						TimeAttendanceDAL.Utility.Utility.ReloadDgridLastTaradodsAsync(ContextGuid, token);
						return Id;
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
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				Log.Error(ex2, "caller:" + caller);
				throw;
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					semafore_SaveAsync.Release();
				}
			}
		}
		finally
		{
			((IDisposable)monitor)?.Dispose();
		}
	}

	public ReturnedSaveFuncInfo Validatetion(string caller)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Invalid comparison between Unknown and I4
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Invalid comparison between Unknown and I4
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (PersonId == 0)
			{
				val.AddWarning("آیدی شخص مشخص نیست\r\n" + $"PersonNo:{PersonNo}\r\n" + $"datetime:{DateTimeLogM}\r\n" + $"DeviceId:{DeviceId_} \r\n" + $"DeviceNo:{DeviceNo} \r\n" + "DeviceSerial:" + DeviceSerial_ + " \r\n" + $"IdTraffic:{TrafficId} \r\n" + $"IsVirtual:{IsVirtual} \r\n" + "TrafficType:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod) + "\r\nTrafficTypeVacationOrMission:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradodVacationOrMission) + "\r\ncaler:" + caller, "", "Validatetion", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 398);
			}
			if (DateTimeLogM > DateTime.Now.AddMinutes(120.0) && caller != "EditTaradod")
			{
				val.AddError("زمان تردد حداقل 120 دقیقه از زمان جاری جلوتر است و امکان ثبت وجود ندارد\r\n" + $"PersonNo:{PersonNo}  \r\n" + $"datetime:{DateTimeLogM}  \r\n" + $"DeviceId:{DeviceId_}  \r\n" + $"DeviceNo:{DeviceNo} \r\n" + "DeviceSerial:" + DeviceSerial_ + " \r\n" + $"IdTraffic:{TrafficId}  \r\n" + $"IsVirtual:{IsVirtual}  \r\n" + "TrafficType:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod) + "\r\nTrafficTypeVacationOrMission:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradodVacationOrMission) + "\r\ncaler:" + caller, "", "Validatetion", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 410, (StringBuilder)null);
			}
			if (DeviceId_ == 0)
			{
				val.AddError("دستگاهی که تردد از آن دریافت شده است مشخص نیست\r\n" + $"PersonNo:{PersonNo}\r\n" + $"datetime:{DateTimeLogM}\r\n" + $"DeviceId:{DeviceId_} \r\n" + $"DeviceNo:{DeviceNo} \r\n" + "DeviceSerial:" + DeviceSerial_ + " \r\n" + $"IdTraffic:{TrafficId} \r\n" + $"IsVirtual:{IsVirtual} \r\n" + "TrafficType:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod) + "\r\nTrafficTypeVacationOrMission:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradodVacationOrMission) + "\r\ncaler:" + caller, "", "Validatetion", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 422, (StringBuilder)null);
			}
			if (PersonNo == 0)
			{
				val.AddError("کد سخت افزاری شخص مشخص نیست\r\n" + $"PersonNo:{PersonNo} \r\n" + $"datetime:{DateTimeLogM} \r\n" + $"DeviceId:{DeviceId_} \r\n" + $"DeviceNo:{DeviceNo} \r\n" + "DeviceSerial:" + DeviceSerial_ + " \r\n" + $"IdTraffic:{TrafficId} \r\n" + $"IsVirtual:{IsVirtual} \r\n" + "TrafficType:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod) + "\r\nTrafficTypeVacationOrMission:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradodVacationOrMission) + "\r\ncaler:" + caller, "", "Validatetion", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 434, (StringBuilder)null);
			}
			if (DateTimeLogM < DateTime.Now.AddDays(-90.0))
			{
				val.AddWarning("زمان تردد حداقل 90 روز از زمان جاری عقب تر است. بررسی شود\r\n" + $"PersonNo:{PersonNo}\r\n" + $"datetime:{DateTimeLogM}\r\n" + $"DeviceId:{DeviceId_}\r\n" + $"DeviceNo:{DeviceNo} \r\n" + "DeviceSerial:" + DeviceSerial_ + " \r\n" + $"IdTraffic:{TrafficId} \r\n" + $"IsVirtual:{IsVirtual} \r\n" + "TrafficType:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod) + " \r\nTrafficTypeVacationOrMission:" + Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradodVacationOrMission) + "\r\ncaler:" + caller, "", "Validatetion", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 447);
			}
			if (TrafficId == 0L)
			{
				TrafficId = 1L;
			}
			if ((int)IsVirtual == 0)
			{
				IsVirtual = (SavedTaradodEnumType)5;
			}
			if ((int)TypeTaradod == 1)
			{
				TypeTaradod = (TypeTaradodEnumType)3;
			}
			if ((int)TypeTaradodVacationOrMission == 1)
			{
				TypeTaradodVacationOrMission = (TypeTaradodEnumType)3;
			}
		}
		catch (Exception ex)
		{
			val.AddError(ex.Message, "", "Validatetion", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 522, (StringBuilder)null);
			Log.Error(ex, "");
		}
		return val;
	}

	public static async Task<ImportTaradodResult> ImportFileAsync(string contextGuid, string caller, DeviceInfo device, StringBuilder sb, string fileName, CancellationToken token)
	{
		sb.AppendLine($"method public async Task<int> ImportFileAsync in {DateTime.Now}");
		ImportTaradodResult importTaradodResult = new ImportTaradodResult();
		try
		{
			short DeviceNo = device.DeviceNo;
			OutPutDevice outPutDevice = await OutPutDevice.FindStyleAsync(contextGuid, fileName, token);
			if (outPutDevice == null)
			{
				importTaradodResult.Result.AddError("هیچ نوع قالبی برابر با قالب اطلاعات فایل مورد نظر شما پیدا نشد . لذا امکان تجزیه و تحلیل داده های شما وجود ندارد ", "", "ImportFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 538, (StringBuilder)null);
				return importTaradodResult;
			}
			importTaradodResult.FileContent = ((new FileInfo(fileName).Extension.ToLower() == ".xls") ? new ExcelFile(contextGuid, fileName).ToString(outPutDevice.Seprator, outPutDevice.NextRecord) : File.ReadAllText(fileName, (outPutDevice.enCoding.ToLower() == "ascii") ? Encoding.ASCII : Encoding.UTF8));
			List<CLSLOG> list = outPutDevice.Process(importTaradodResult.FileContent, short.Parse(DeviceNo.ToString()));
			if (list == null)
			{
				importTaradodResult.Result.AddError("بعد از پردازش محتوای فایل، هیچ اطلاعات درستی پیدا نشد. محتوای فایل را بررسی نمائید", "", "ImportFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 548, (StringBuilder)null);
				return importTaradodResult;
			}
			importTaradodResult.CountKol = list.Count;
			int getTaradodFormCountDaysAgo = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid)).GetTaradodFormCountDaysAgo;
			if (getTaradodFormCountDaysAgo <= 0)
			{
				importTaradodResult.Result.AddError("در قسمت تنظیمات نرم افزار، تنظیم (از چند روز قبل تردد ها قابل دریافت باشند) را مشخص نمائید. سپس مجددا فایل را بارگزاری نمائید", "", "ImportFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 556, (StringBuilder)null);
				return importTaradodResult;
			}
			DateTime xDateTime = DateTime.Now.AddDays(-getTaradodFormCountDaysAgo);
			List<CLSLOG> list2 = list.Where((CLSLOG q) => q.DateM > DateTime.Now).ToList();
			if (list2.Count > 0)
			{
				importTaradodResult.CountInCorrect = list2.Count;
				importTaradodResult.Result.AddError($"تعداد ( {list2.Count} ) تردد از ترددهای داخل فایل به تاریخ آینده می باشند . لطفا بررسی نموده و مجددا تلاش نمائید", "", "ImportFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 564, (StringBuilder)null);
				return importTaradodResult;
			}
			List<CLSLOG> plst = list.Where((CLSLOG q) => q.DateM > xDateTime).ToList();
			List<DeviceLogOnPerson> allFromClslog = RecordAllDevice.GetAllFromClslog(contextGuid, plst);
			sb.AppendLine($"call var result = await SaveTaradodAsync(lst, new CancellationToken(false)); in {DateTime.Now}");
			SaveLogonPersonelResult saveLogonPersonelResult = await SaveTaradodsNewAsync(contextGuid, caller + "=>ImportFileAsync", (EnSystem)0, sb, allFromClslog, token, device);
			if (saveLogonPersonelResult == null || saveLogonPersonelResult.CountSavedLogs <= 0)
			{
				importTaradodResult.CountInCorrect = importTaradodResult.CountKol;
				importTaradodResult.Result.AddInformation("هیچ تردد جدیدی در فایل وجود نداشت.", "", "ImportFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 585);
				return importTaradodResult;
			}
			importTaradodResult.CountInCorrect = importTaradodResult.CountKol - saveLogonPersonelResult.CountSavedLogs;
			string text = "دریافت فایل تردد، تعداد ترددهای صحیح:" + $"( {saveLogonPersonelResult.CountSavedLogs} )" + "از دستگاه: ( " + string.Join(",", saveLogonPersonelResult.deviceIds) + " )در تاریخ: " + Calendar.MiladiToCalture(DateTime.Now);
			importTaradodResult.CountCorrect = saveLogonPersonelResult.CountSavedLogs;
			importTaradodResult.Result.AddInformation(text, "", "ImportFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 578);
			await LogUsers.RegisterNewLogAsync(contextGuid, text, "Taradod.TaradodReciveDataOffLine");
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return importTaradodResult;
	}

	public async Task DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonPersonel_DeleteRow", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)Id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static List<DeviceLogOnPerson> MapDataToRecordAllDevice(string contextGuid, List<WebAttendanceLogOnPerson> lstWebAtteandance)
	{
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			LogonPersonalMapper logonPersonalMapper = new LogonPersonalMapper();
			foreach (WebAttendanceLogOnPerson item in lstWebAtteandance)
			{
				list.Add(logonPersonalMapper.Map(contextGuid, item, null));
			}
			return list;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<SaveLogonPersonelResult> SaveTaradodsNewAsync(string contextGuid, string caller, EnSystem system, StringBuilder sb, List<DeviceLogOnPerson> plst, CancellationToken token, DeviceInfo device)
	{
		sb.AppendLine($"method public async Task<(int countTaradod, List<string> deviceNames)> SaveTaradodAsync in {DateTime.Now}");
		new List<string>();
		SaveLogonPersonelResult saveLogonPersonelResult = null;
		try
		{
			if (plst == null || plst.Count <= 0)
			{
				return saveLogonPersonelResult;
			}
			List<DeviceLogOnPerson> list = plst.Where((DeviceLogOnPerson q) => q.DateTimeLogM > DateTime.Now.AddHours(1.0)).ToList();
			if (list != null && list.Any())
			{
				string message = $"تعداد {list.Count} از تردد های دریافت شده مربوط به زمان آینده است احتمالا زمان و تاریخ دستگاه نادرست است";
				EventNotification onStatusErorTaradod = InstanceForEvents.OnStatusErorTaradod;
				if (onStatusErorTaradod != null)
				{
					object instanceForEvents = InstanceForEvents;
					onStatusErorTaradod(instanceForEvents, await EventArgsNotify.GetAsync(contextGuid, list.FirstOrDefault().IdTraffic, message, "زمان دستگاه نامعتبر است", (NotificationEnum)4, 3));
				}
			}
			if (plst.Count <= 0)
			{
				return saveLogonPersonelResult;
			}
			List<LogOnPersonel> list2 = await MapToLogonPersonel(contextGuid, plst, device, token);
			saveLogonPersonelResult = await Save(contextGuid, caller + "=>SaveTaradodNewAsync", list2, token);
			if (saveLogonPersonelResult != null)
			{
				if (saveLogonPersonelResult.CountSavedLogs <= 0)
				{
					EventNotification onStatusErorTaradod = InstanceForEvents.OnStatusErorTaradod;
					if (onStatusErorTaradod != null)
					{
						object instanceForEvents = InstanceForEvents;
						onStatusErorTaradod(instanceForEvents, await EventArgsNotify.GetAsync(contextGuid, 0L, "ترددی برای ثبت وجود نداشت", "ثبت تردد", (NotificationEnum)5, 3));
					}
				}
				else
				{
					foreach (PersonHaveNotGharardad item in await PersonHaveNotGharardad.GetAllAsync(contextGuid, saveLogonPersonelResult.SavedLogIds, token))
					{
						EventNotification onStatusErorTaradod = InstanceForEvents.OnStatusErorTaradod;
						if (onStatusErorTaradod != null)
						{
							object instanceForEvents = InstanceForEvents;
							onStatusErorTaradod(instanceForEvents, await EventArgsNotify.GetAsync(contextGuid, 0L, " " + item.FullName + " با کد تفصیلی  " + item.CodeTafsil + " قرارداد ندارد", Utilities.GetTranslate("[هشدار]"), (NotificationEnum)5, 3));
						}
					}
					foreach (HardWareCodeNotAssigned item2 in await HardWareCodeNotAssigned.GetAllAsync(contextGuid, saveLogonPersonelResult.SavedLogIds, token))
					{
						EventNotification onStatusErorTaradod = InstanceForEvents.OnStatusErorTaradod;
						if (onStatusErorTaradod != null)
						{
							object instanceForEvents = InstanceForEvents;
							onStatusErorTaradod(instanceForEvents, await EventArgsNotify.GetAsync(contextGuid, 0L, Utilities.GetTranslate($" [کد سخت افزار] {item2.PersonNO} [که در دستگاه تعریف شده است به شخصی نسبت داده نشده است]"), Utilities.GetTranslate("[هشدار]"), (NotificationEnum)5, 3));
						}
					}
					TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
					if (timeAttendanceSettings.IsRunAppAfterLogOnPerson && timeAttendanceSettings.AppRunAfterLogOnPerson.Trim() != "" && File.Exists(timeAttendanceSettings.AppRunAfterLogOnPerson))
					{
						Process.Start(timeAttendanceSettings.AppRunAfterLogOnPerson, string.Join(",", saveLogonPersonelResult.SavedLogIds));
					}
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		finally
		{
			ClsSms.ForceToSendSms();
		}
		return saveLogonPersonelResult;
	}

	public static async Task<List<LogOnPersonel>> MapToLogonPersonel(string contextGuid, List<DeviceLogOnPerson> recordAllDevices, DeviceInfo device, CancellationToken token)
	{
		List<LogOnPersonel> logs = new List<LogOnPersonel>();
		try
		{
			if (token.IsCancellationRequested)
			{
				return logs;
			}
			if (token.IsCancellationRequested)
			{
				return logs;
			}
			foreach (DeviceLogOnPerson recordAllDevice in recordAllDevices)
			{
				LogOnPersonel x = new LogOnPersonel(contextGuid)
				{
					DateTimeLogM = recordAllDevice.FixedDateM,
					DeviceId_ = device.Id,
					DeviceNo = device.DeviceNo,
					DeviceSerial_ = device.SerialNumberStr,
					TrafficId = recordAllDevice.IoMode,
					PersonNo = recordAllDevice.PersonNo
				};
				HardwareCodeCus hardwareCodeCus = await HardwareCodeCus.GetAsync_ByPersonNoDeviceId(contextGuid, recordAllDevice.PersonNo, device.Id, token);
				if (hardwareCodeCus != null && hardwareCodeCus.PersonId > 0)
				{
					x.PersonId = hardwareCodeCus.PersonId;
				}
				x.TypeTaradod = (TypeTaradodEnumType)3;
				x.IsVirtual = (SavedTaradodEnumType)1;
				x.TypeTaradodVacationOrMission = (TypeTaradodEnumType)1;
				logs.Add(x);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return logs;
	}

	public static async Task<SaveLogonPersonelResult> Save(string contextGuid, string caller, List<LogOnPersonel> list, CancellationToken token)
	{
		SaveLogonPersonelResult res = new SaveLogonPersonelResult();
		try
		{
			int maxid = await GetMaxId(contextGuid, token);
			int i = 0;
			_ = list.Count;
			foreach (LogOnPersonel log in list)
			{
				i++;
				if (token.IsCancellationRequested)
				{
					break;
				}
				try
				{
					int num = await log.SaveAsync(caller + "=>Save(list logonPersonel)", default(CancellationToken), "Save", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs");
					if (num > maxid)
					{
						maxid = num;
						res.SavedLogIds.Add(num);
						res.CountSavedLogs++;
						if (log.DeviceId_ > 0 && !res.deviceIds.Contains(log.DeviceId_))
						{
							res.deviceIds.Add(log.DeviceId_);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex, "");
				}
			}
			return res;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			Log.Error(ex3, "");
			throw;
		}
	}

	private static LogOnPersonel LoadData(string contextGuid, SqlDataReader dr)
	{
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Invalid comparison between Unknown and I4
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			LogOnPersonel logOnPersonel = new LogOnPersonel(contextGuid, DateTime.Now.Date);
			if (((DbDataReader)(object)dr)["Id"] != DBNull.Value)
			{
				logOnPersonel.Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["DateTimeLogM"] != DBNull.Value)
			{
				logOnPersonel.DateTimeLogM = (DateTime)((DbDataReader)(object)dr)["DateTimeLogM"];
			}
			if (((DbDataReader)(object)dr)["IDTraffic"] != DBNull.Value)
			{
				logOnPersonel.TrafficId = (long)((DbDataReader)(object)dr)["IDTraffic"];
			}
			if (((DbDataReader)(object)dr)["PersonNO"] != DBNull.Value)
			{
				logOnPersonel.PersonNo = (int)((DbDataReader)(object)dr)["PersonNO"];
			}
			if (((DbDataReader)(object)dr)["DeviceId"] != DBNull.Value)
			{
				logOnPersonel.DeviceId_ = (int)((DbDataReader)(object)dr)["DeviceId"];
			}
			if (((DbDataReader)(object)dr)["PersonID"] != DBNull.Value)
			{
				logOnPersonel.PersonId = (int)((DbDataReader)(object)dr)["PersonID"];
			}
			if (((DbDataReader)(object)dr)["TrafficType"] != DBNull.Value)
			{
				logOnPersonel.TypeTaradod = (TypeTaradodEnumType)((DbDataReader)(object)dr)["TrafficType"];
			}
			if ((int)logOnPersonel.TypeTaradod == 1)
			{
				logOnPersonel.TypeTaradod = (TypeTaradodEnumType)3;
			}
			if (((DbDataReader)(object)dr)["TrafficTypeVacationOrMission"] != DBNull.Value)
			{
				logOnPersonel.TypeTaradodVacationOrMission = (TypeTaradodEnumType)((DbDataReader)(object)dr)["TrafficTypeVacationOrMission"];
			}
			if (((DbDataReader)(object)dr)["Duration"] != DBNull.Value)
			{
				logOnPersonel.Duration = (short)((DbDataReader)(object)dr)["Duration"];
			}
			if (((DbDataReader)(object)dr)["IsVirtual"] != DBNull.Value)
			{
				logOnPersonel.IsVirtual = (SavedTaradodEnumType)((DbDataReader)(object)dr)["IsVirtual"];
			}
			if (((DbDataReader)(object)dr)["SmsRefrenceIDs"] != DBNull.Value)
			{
				logOnPersonel.SmsRefrenceIDs = ((DbDataReader)(object)dr)["SmsRefrenceIDs"].ToString();
			}
			return logOnPersonel;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<List<LogOnPersonel>> GetAllByPersonIdAsync(string contextGuid, int personId, DateTime dateM, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<LogOnPersonel> ret = new List<LogOnPersonel>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonPersonel_SelectByPersonId", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@Datem", (object)dateM);
					cmd.Parameters.AddWithValue("@PersonId", (object)personId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					while (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						ret.Add(LoadData(contextGuid, dr));
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					return ret;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<List<LogOnPersonel>> GetAllByPersonIdAsync(string contextGuid, int personId, DateTime fromDate, DateTime toDate, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<LogOnPersonel> ret = new List<LogOnPersonel>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonPersonel_SelectByPersonIdBetweenDate", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@FromDate", (object)fromDate);
					cmd.Parameters.AddWithValue("@ToDate", (object)toDate);
					cmd.Parameters.AddWithValue("@PersonId", (object)personId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					while (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						ret.Add(LoadData(contextGuid, dr));
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					return ret;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<List<LogOnPersonel>> GetAllAsync(string contextGuid, DateTime fromDate, DateTime toDate, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<LogOnPersonel> ret = new List<LogOnPersonel>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonPersonel_SelectBetweenDate", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@FromDate", (object)fromDate);
					cmd.Parameters.AddWithValue("@ToDate", (object)toDate);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					while (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						ret.Add(LoadData(contextGuid, dr));
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					return ret;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<List<LogOnPersonel>> GetAllForSendSmsTaradodAsync(string contextGuid, DateTime fromDate, int count, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<LogOnPersonel> ret = new List<LogOnPersonel>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonPersonel_SelectBySmsRefrenced", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@fromDate", (object)fromDate);
					cmd.Parameters.AddWithValue("@count", (object)count);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					while (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						ret.Add(LoadData(contextGuid, dr));
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					return ret;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<List<LogOnPersonel>> GetLastTaradodsAsync(string contextGuid, int personNo, short deviceId, int countLastTaradod, DateTime? date = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<LogOnPersonel> ret = new List<LogOnPersonel>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_LogonPersonel_SelectLastTaradod]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@DeviceId", (object)deviceId);
					cmd.Parameters.AddWithValue("@PersonNo", (object)personNo);
					cmd.Parameters.AddWithValue("@Count", (object)countLastTaradod);
					if (date.HasValue)
					{
						cmd.Parameters.AddWithValue("@Date", (object)date.Value);
					}
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					while (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						ret.Add(LoadData(contextGuid, dr));
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					return ret;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	internal static async Task DeleteAllAsync(string contextGuid, int personId, DateTime fromDate, DateTime toDate, SavedTaradodEnumType taradodType, CancellationToken token)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonPersonel_DeleteAll", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@FromDate", (object)fromDate);
					cmd.Parameters.AddWithValue("@ToDate", (object)toDate);
					cmd.Parameters.AddWithValue("@PersonId", (object)personId);
					cmd.Parameters.AddWithValue("@IsVirtual", (object)taradodType);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					TimeAttendanceDAL.Utility.Utility.ReloadDgridLastTaradodsAsync(contextGuid, token);
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	internal static async Task MoveDataDeviceToLogonPersonelAsync(string contextGuid, CancellationToken token)
	{
		_ = 7;
		try
		{
			List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(contextGuid, token);
			List<HardwareCodeCus> hardwareCodes = await HardwareCodeCus.GetAllAsync(contextGuid, token);
			List<LogOnPersonel> listLog = new List<LogOnPersonel>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_DataDevice_GetAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					while (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						listLog.Add(LoadDataFromTableDataDevice(contextGuid, dr, devices, hardwareCodes));
					}
					((DbDataReader)(object)dr).Close();
					((DbConnection)(object)cn).Close();
					await Save(contextGuid, "MoveDataDeviceToLogonPersonel", listLog, token);
					await TruncateTableDataDeviceAsync(contextGuid, token);
					await UpdeteDeviceIdAsync(contextGuid, token);
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private static async Task UpdeteDeviceIdAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			DeviceInfo device = (await DeviceInfo.GetAllAsync(contextGuid))?.FirstOrDefault();
			if (device == null)
			{
				return;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand cmd = new SqlCommand($"update LogonPersonel set DeviceId = {device.Id} ,DeviceSerial = '{device.SerialNumberStr}' where DeviceId is null", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
				((DbConnection)(object)cn).Close();
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private static async Task TruncateTableDataDeviceAsync(string contextGuid, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand cmd = new SqlCommand("IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataDevice]') AND type in (N'U')) DROP TABLE [dbo].[DataDevice]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
				((DbConnection)(object)cn).Close();
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private static LogOnPersonel LoadDataFromTableDataDevice(string contextGuid, SqlDataReader dr, List<DeviceInfo> devices, List<HardwareCodeCus> hardwareCodes)
	{
		try
		{
			LogOnPersonel logOnPersonels = new LogOnPersonel(contextGuid, DateTime.Now.Date);
			if (((DbDataReader)(object)dr)["DateM"] != DBNull.Value)
			{
				logOnPersonels.DateTimeLogM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["PersonNO"] != DBNull.Value)
			{
				logOnPersonels.PersonNo = (int)((DbDataReader)(object)dr)["PersonNO"];
			}
			if (((DbDataReader)(object)dr)["IdTraffic"] != DBNull.Value)
			{
				logOnPersonels.TrafficId = long.Parse(((DbDataReader)(object)dr)["IdTraffic"].ToString());
			}
			if (((DbDataReader)(object)dr)["DeviceNo"] != DBNull.Value)
			{
				logOnPersonels.DeviceNo = (int)((DbDataReader)(object)dr)["DeviceNo"];
			}
			if (logOnPersonels.DeviceId_ <= 0)
			{
				logOnPersonels.DeviceId_ = 1;
			}
			int? num = hardwareCodes.FirstOrDefault((HardwareCodeCus p) => p.HardwareCode == logOnPersonels.PersonNo.ToString() && p.DeviceId == logOnPersonels.DeviceId_)?.PersonId;
			if (num.HasValue)
			{
				logOnPersonels.PersonId = num.Value;
			}
			logOnPersonels.TypeTaradod = (TypeTaradodEnumType)3;
			logOnPersonels.TypeTaradodVacationOrMission = (TypeTaradodEnumType)3;
			logOnPersonels.IsVirtual = (SavedTaradodEnumType)1;
			logOnPersonels.IsDeleted = true;
			return logOnPersonels;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, WebAttLogOnPerson log)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			LogOnPersonel logOnPersonel = LogonPersonalMapper.Instance.Map(contextGuid, log);
			if (logOnPersonel != null && logOnPersonel.PersonNo > 0)
			{
				await logOnPersonel.SaveAsync(contextGuid, default(CancellationToken), "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			ret.AddError(ex.Message, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\LogOnPersonel.cs", 1112, (StringBuilder)null);
		}
		return ret;
	}
}
