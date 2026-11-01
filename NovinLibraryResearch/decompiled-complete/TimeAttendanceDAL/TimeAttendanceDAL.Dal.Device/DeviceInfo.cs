using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Attendance.SDK.LAN_VisibleLight;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Mapper;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDevices;
using TimeAttendanceDevices.Eizon;
using TimeAttendanceDevices.FLREbio;
using TimeAttendanceDevices.FLRHysoon;
using TimeAttendanceDevices.FLRZDC2911;
using TimeAttendanceDevices.FLRZk;

namespace TimeAttendanceDAL.Dal.Device;

[Serializable]
public class DeviceInfo : IDeviceInfo, IHasContext, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public static List<DeviceInfo> Devices = null;

	public static DateTime LastUpdateTime = DateTime.Now.AddSeconds(-120.0);

	public static SemaphoreSlim semafore_GetAll = new SemaphoreSlim(1, 1);

	private DeviceModelEnumType _model = (DeviceModelEnumType)9201;

	private string _communicationPass = "0";

	public static List<string> spamSerialList = new List<string>();

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public short DeviceNo { get; set; } = 1;


	public string CommunicationPass
	{
		get
		{
			string text = Text.RemoveNoNumbers(_communicationPass, "");
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return "0";
		}
		set
		{
			_communicationPass = Text.RemoveNoNumbers(value, "");
		}
	}

	public string Ip { get; set; } = "";


	public string CallBackIp { get; set; } = "";


	public int TcpPort { get; set; }

	public int PortRealtime { get; set; }

	public bool GetNewTaradodOnly { get; set; } = true;


	public TypeConnection TransferMode { get; set; } = (TypeConnection)1;


	public DeviceManufacture TypeDeviceManufacture { get; set; }

	public DateTime RdEnrollDataTime { get; set; } = DateTime.Now.AddDays(-1.0);


	public bool AutoSetDateTime { get; set; } = true;


	public string SerialNumberStr { get; set; } = "";


	public DeviceModelEnumType Model
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _model;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected I4, but got Unknown
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Invalid comparison between Unknown and I4
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Invalid comparison between Unknown and I4
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			_model = value;
			try
			{
				if ((int)value <= 9204)
				{
					if (value - 9001 > 2)
					{
						if (value - 9101 > 1)
						{
							if (value - 9201 <= 3)
							{
								TypeDeviceManufacture = (DeviceManufacture)2;
							}
						}
						else
						{
							TypeDeviceManufacture = (DeviceManufacture)1;
						}
					}
					else
					{
						TypeDeviceManufacture = (DeviceManufacture)0;
					}
					return;
				}
				switch (value - 9301)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 7:
					TypeDeviceManufacture = (DeviceManufacture)3;
					return;
				case 6:
					TypeDeviceManufacture = (DeviceManufacture)5;
					return;
				case 8:
					TypeDeviceManufacture = (DeviceManufacture)6;
					return;
				}
				if ((int)value != 9400)
				{
					if ((int)value == 9500)
					{
					}
				}
				else
				{
					TypeDeviceManufacture = (DeviceManufacture)4;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Model", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 124);
			}
		}
	}

	public string DeviceName { get; set; } = "";


	public string DeviceType { get; set; } = "";


	public string MacAddress { get; set; } = "";


	public string SubnetMask { get; set; } = "";


	public bool UseDhcp { get; set; } = true;


	public string EngineVersion { get; set; } = "";


	public string FwVersion { get; set; } = "";


	public string HostIp { get; set; } = "";


	public string DefultGetWay { get; set; } = "";


	public int MinCheck { get; set; }

	public bool AutoRecieve { get; set; }

	public bool UseRealTime { get; set; }

	public string Desc { get; set; } = "";


	public DateTime SyncTime { get; set; } = DateTime.Now;


	[JsonIgnore]
	public object SourceObj { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status => true;

	public short Id { get; set; }

	public string DeviceColor { get; set; } = "Black";


	[JsonIgnore]
	public bool IsChecked { get; set; }

	public event EventNotification OnReceiveData;

	public event EventNotification OnFinishGetData;

	public event EventNotification OnDisconnected;

	public DeviceInfo(string contextGuid)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		try
		{
			Desc = "";
			Model = (DeviceModelEnumType)9201;
			TransferMode = (TypeConnection)1;
			Ip = "192.168.1.224";
			TcpPort = 5005;
			CommunicationPass = "1";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 164);
		}
	}

	public static async Task<DeviceInfo> GetBySerialAsync(string contextGuid, string deviceSerialNumber, CancellationToken token = default(CancellationToken))
	{
		return (await GetAllAsync(contextGuid, token))?.SingleOrDefault((DeviceInfo p) => p.SerialNumberStr == deviceSerialNumber);
	}

	public static async Task<DeviceInfo> GetByIdAsync(string contextGuid, int id, CancellationToken token = default(CancellationToken))
	{
		return (await GetAllAsync(contextGuid, token))?.SingleOrDefault((DeviceInfo p) => p.Id == id);
	}

	public static async Task<DeviceInfo> GetByDeviceNoAsync(string contextGuid, int deviceNo, CancellationToken token = default(CancellationToken))
	{
		return (await GetAllAsync(contextGuid, token))?.SingleOrDefault((DeviceInfo p) => p.DeviceNo == deviceNo);
	}

	public static void Reset()
	{
		LastUpdateTime = DateTime.Now.AddSeconds(-120.0);
	}

	public static async Task<List<DeviceInfo>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			if (LastUpdateTime.AddSeconds(60.0) > DateTime.Now)
			{
				List<DeviceInfo> devices = Devices;
				if (devices != null && devices.Count > 0)
				{
					return Devices;
				}
			}
			await semafore_GetAll.WaitAsync(token).ConfigureAwait(continueOnCapturedContext: false);
			semaphoreHasBeenTaken = true;
			LastUpdateTime = DateTime.Now;
			List<DeviceInfo> ret = new List<DeviceInfo>();
			SqlConnection cn = new SqlConnection(ClsConnection.connectionString);
			try
			{
				SqlCommand val = new SqlCommand("[sp_DeviceInfo_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token).ConfigureAwait(continueOnCapturedContext: false);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token).ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token).ConfigureAwait(continueOnCapturedContext: false))
						{
							ret.Add(LoadData(contextGuid, (IDataRecord)dr));
						}
						Devices = ret;
						return Devices;
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 221);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				semafore_GetAll.Release();
			}
		}
	}

	private static DeviceInfo LoadData(string contextGuid, IDataRecord dr)
	{
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DeviceInfo deviceInfo = new DeviceInfo(contextGuid);
			if (dr["Id"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.Id = (short)dr["Id"];
			}
			if (dr["Guid"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.Guid = (Guid)dr["Guid"];
			}
			if (dr["SerialNumberStr"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.SerialNumberStr = dr["SerialNumberStr"].ToString();
			}
			if (dr["IP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.Ip = dr["IP"].ToString().Trim();
			}
			if (dr["CallBackIP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.CallBackIp = dr["CallBackIP"].ToString().Trim();
			}
			if (dr["TCPPort"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.TcpPort = (int)dr["TCPPort"];
			}
			if (dr["TCPPort"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.PortRealtime = (int)dr["PortRealTime"];
			}
			if (dr["GetNewTaradodOnly"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.GetNewTaradodOnly = (bool)dr["GetNewTaradodOnly"];
			}
			if (dr["DeviceName"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.DeviceName = dr["DeviceName"].ToString();
			}
			if (dr["DeviceType"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.DeviceType = dr["DeviceType"].ToString();
			}
			if (dr["MACAddress"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.MacAddress = dr["MACAddress"].ToString();
			}
			if (dr["SubnetMask"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.SubnetMask = dr["SubnetMask"].ToString();
			}
			if (dr["UseDHCP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.UseDhcp = (bool)dr["UseDHCP"];
			}
			if (dr["UseDHCP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.UseRealTime = (bool)dr["UseRealTime"];
			}
			if (dr["EngineVersion"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.EngineVersion = dr["EngineVersion"].ToString();
			}
			if (dr["FWVersion"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.FwVersion = dr["FWVersion"].ToString();
			}
			if (dr["TransferMode"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.TransferMode = (TypeConnection)dr["TransferMode"];
			}
			if (dr["HostIP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.HostIp = dr["HostIP"].ToString().Trim();
			}
			if (dr["DefultGetWay"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.DefultGetWay = dr["DefultGetWay"].ToString();
			}
			if (dr["CommunicationPass"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.CommunicationPass = dr["CommunicationPass"].ToString();
			}
			if (dr["DeviceNO"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.DeviceNo = (short)dr["DeviceNO"];
			}
			if (dr["Desc"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.Desc = dr["Desc"].ToString();
			}
			if (dr["Autorecieve"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.AutoRecieve = (bool)dr["Autorecieve"];
			}
			if (dr["MinCheck"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.MinCheck = (int)dr["MinCheck"];
			}
			deviceInfo.SyncTime = ((dr["SyncTime"].ToString() != DBNull.Value.ToString()) ? ((DateTime)dr["SyncTime"]) : DateTime.Now);
			if (dr["Model"].ToString() != DBNull.Value.ToString() && (short)dr["Model"] >= 0)
			{
				deviceInfo.Model = (DeviceModelEnumType)(short)dr["Model"];
			}
			if (dr["RdEnrollDataTime"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.RdEnrollDataTime = (DateTime)dr["RdEnrollDataTime"];
			}
			if (dr["AutoSetDateTime"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.AutoSetDateTime = (bool)dr["AutoSetDateTime"];
			}
			if (dr["DeviceColor"].ToString() != DBNull.Value.ToString())
			{
				deviceInfo.DeviceColor = dr["DeviceColor"].ToString();
			}
			return deviceInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 266);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<DeviceInfo>> CheckAndGetDeviceInfoAsync(string contextGuid, string serialNumber, short deviceNo, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<DeviceInfo> ret = new ReturnedSaveFuncInfoWithValue<DeviceInfo>
		{
			value = null
		};
		try
		{
			if (serialNumber == null)
			{
				serialNumber = "";
			}
			List<string> list = spamSerialList;
			if (list != null && list.Count > 0 && spamSerialList.Contains(serialNumber))
			{
				ret.AddError((EnSystem)2, "the device is not define for you...", "CheckAndGetDeviceInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 282);
				Log.Error("the device is not define for you...");
				return ret;
			}
			if (!string.IsNullOrEmpty(serialNumber))
			{
				ReturnedSaveFuncInfoWithValue<DeviceInfo> val = ret;
				val.value = await GetBySerialAsync(contextGuid, serialNumber, token);
				if (ret.value == null || ret.value.SerialNumberStr == null)
				{
					Log.Error("serial was incorrect !!\r\n serialNumber is :" + serialNumber);
				}
			}
			if (ret.value == null)
			{
				ReturnedSaveFuncInfoWithValue<DeviceInfo> val = ret;
				val.value = await GetByDeviceNoAsync(contextGuid, deviceNo, token);
				if (ret.value == null)
				{
					if (!string.IsNullOrEmpty(serialNumber))
					{
						spamSerialList?.Add(serialNumber);
					}
					string text = $"دستگاهی با سریال: {serialNumber} , شناسه: {deviceNo}  \r\n در لیست دستگاههای شما شناسایی نشد\r\n لذا تردد های دستگاه مذکور دریافت نخواهند شد";
					Log.Error(text);
					TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)5, text, "", 6);
					string arg = $"device with serial: {serialNumber} and deviceId: {deviceNo} \r\n not found in system of customer with lkSerial:\r\n {VersionManagment.LkSerial}";
					// [SANITIZED] ارسال سریال دستگاه و سریال قفل به ربات تلگرام حذف شد
					ret.AddError((EnSystem)2, text, "CheckAndGetDeviceInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 306);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return ret;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("SerialNumberStr", (object)SerialNumberStr);
					cmd.Parameters.AddWithValue("IP", (object)Ip.Trim());
					cmd.Parameters.AddWithValue("CallBackIP", (object)(CallBackIp?.Trim() ?? ""));
					cmd.Parameters.AddWithValue("TCPPort", (object)TcpPort);
					cmd.Parameters.AddWithValue("PortRealTime", (object)PortRealtime);
					cmd.Parameters.AddWithValue("GetNewTaradodOnly", (object)GetNewTaradodOnly);
					cmd.Parameters.AddWithValue("DeviceName", (object)Text.FixString(DeviceName));
					cmd.Parameters.AddWithValue("DeviceType", (object)Text.FixString(DeviceType));
					cmd.Parameters.AddWithValue("MACAddress", (object)Text.FixString(MacAddress));
					cmd.Parameters.AddWithValue("SubnetMask", (object)Text.FixString(SubnetMask));
					cmd.Parameters.AddWithValue("UseDHCP", (object)UseDhcp);
					cmd.Parameters.AddWithValue("EngineVersion", (object)Text.FixString(EngineVersion));
					cmd.Parameters.AddWithValue("FWVersion", (object)Text.FixString(FwVersion));
					cmd.Parameters.AddWithValue("TransferMode", (object)TransferMode);
					cmd.Parameters.AddWithValue("HostIP", (object)Text.FixString(HostIp.Trim()));
					cmd.Parameters.AddWithValue("DefultGetWay", (object)Text.FixString(DefultGetWay));
					cmd.Parameters.AddWithValue("CommunicationPass", (object)CommunicationPass);
					cmd.Parameters.AddWithValue("DeviceNO", (object)DeviceNo);
					cmd.Parameters.AddWithValue("AutoRecieve", (object)AutoRecieve);
					cmd.Parameters.AddWithValue("UseRealTime", (object)UseRealTime);
					cmd.Parameters.AddWithValue("MinCheck", (object)MinCheck);
					cmd.Parameters.AddWithValue("Desc", (object)Text.FixString(Desc));
					cmd.Parameters.AddWithValue("SyncTime", (object)SyncTime);
					cmd.Parameters.AddWithValue("Model", (object)Model);
					cmd.Parameters.AddWithValue("RdEnrollDataTime", (object)RdEnrollDataTime);
					cmd.Parameters.AddWithValue("AutoSetDateTime", (object)AutoSetDateTime);
					cmd.Parameters.AddWithValue("DeviceColor", (object)DeviceColor);
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "sp_DeviceInfo_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							Id = Convert.ToInt16(obj.ToString());
						}
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "[افزودن اطلاعات یک دستگاه جدید] : " + DeviceName + " [با شماره دستگاه ] : " + DeviceNo, "Device.DeviceInsert", this, Id.ToString());
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "sp_DeviceInfo_Update";
						cmd.Parameters.AddWithValue("Id", (object)Id);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
					((DbConnection)(object)cn).Close();
					Reset();
					return true;
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

	public async Task SyncDeviceAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("UPDATE DeviceInfo SET SyncTime = GETDATE() WHERE id = @Id", cn);
				try
				{
					cmd.Parameters.AddWithValue("@Id", (object)Id);
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
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
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task DeleteAsync(CancellationToken token)
	{
		_ = 5;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_HardWareCode_countUse_byDeviceId", cn);
				try
				{
					cmd.Parameters.AddWithValue("DeviceId", (object)Id);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						short countUsed = 0;
						if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["countUsed"].ToString() != DBNull.Value.ToString() && !string.IsNullOrEmpty(((DbDataReader)(object)dr)["countUsed"].ToString()))
						{
							countUsed = short.Parse(((DbDataReader)(object)dr)["countUsed"].ToString());
						}
						((DbConnection)(object)cn).Close();
						if (countUsed > 0)
						{
							TimeAttendanceDAL.Utility.Utility.MessageBoxShowError($"[در تعریف اشخاص، تعداد] {countUsed} [نفر از کد این دستگاه استفاده می کنند لذا امکان حذف دستگاه وجود ندارد]", EntityCacheConfig.CacheInstance.ContextGuid);
							return;
						}
						((DbCommand)(object)cmd).CommandText = "sp_DeviceInfo_DeleteRow";
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						cmd.Parameters.AddWithValue("Id", (object)Id);
						await ((DbConnection)(object)cn).OpenAsync(token);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
						((DbConnection)(object)cn).Close();
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "[حذف اطلاعات دستگاه] : " + DeviceName + " [با شماره دستگاه ]  : " + DeviceNo, "Device.DeviceDelete", this, Id.ToString());
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
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

	public async Task<ReturnedSaveFuncInfo> ReceiveDataAsync(StringBuilder sb, FrmEbio ifDeviceIsEbio, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReciveData)
			{
				await ret.AddErrorAsync((EnSystem)2, "[سطح دسترسی نرم افزار امکان دریافت اتوماتیک تردد را ندارد] ", "", "ReceiveDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 443, (StringBuilder)null);
				return ret;
			}
			if ((int)TypeDeviceManufacture != 2)
			{
				await ret.AddReturnedValueAsync(await ImportAllDataFromDeviceAsync((EnSystem)2, ifDeviceIsEbio, sb, token), (ReturnedState)0, "ReceiveDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 451);
			}
			else
			{
				ReturnedSaveFuncInfo val = ret;
				val.AddReturnedValue(await ImportAllDataFromDeviceAsync((EnSystem)2, ifDeviceIsEbio, sb, token), (ReturnedState)0, "ReceiveDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 447);
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
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ImportAllDataFromDeviceAsync(EnSystem system, FrmEbio ifDeviceIsEbio, StringBuilder sb, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (token.IsCancellationRequested)
		{
			ret.AddError("توکن لغو و عملیات متوقف شد", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 469, (StringBuilder)null);
			return null;
		}
		try
		{
			ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>> res0 = await ReceiveDataFromDeviceAsync(ifDeviceIsEbio).ConfigureAwait(continueOnCapturedContext: false);
			await ret.AddReturnedValueAsync((ReturnedSaveFuncInfo)(object)res0, (ReturnedState)0, "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 479).ConfigureAwait(continueOnCapturedContext: false);
			List<DeviceLogOnPerson> list = res0.value;
			if (list == null)
			{
				ret.AddReturnedValue(TimeAttendanceDAL.Utility.Utility.Ping(Ip), (ReturnedState)0, "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 484);
				return ret.AddInformation("ارتباط با دستگاه (" + DeviceName + ") برقرار نشد", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 485);
			}
			if (!list.Any())
			{
				return ret.AddInformation("تردد جدیدی روی دستگاه (" + DeviceName + ") وجود ندارد", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 487);
			}
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			List<DeviceInfo> allDeviceList = await GetAllAsync(ContextGuid, token).ConfigureAwait(continueOnCapturedContext: false);
			List<HardwareCodeCus> allHardwareCodes = await HardwareCodeCus.GetAllAsync(ContextGuid, token).ConfigureAwait(continueOnCapturedContext: false);
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token).ConfigureAwait(continueOnCapturedContext: false);
			if (setting == null)
			{
				ret.AddError("خواندن تنظیمات نرم\u200cافزار با خطا مواجه شد", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 496, (StringBuilder)null);
				return ret;
			}
			if (list.Count() > 1000)
			{
				ret.AddInformation($"تعداد ترددها روی دستگاه بیشتر از {list.Count()} عدد است که باعث کندی می\u200cشود. توصیه می\u200cشود بعد از دریافت ترددها، ترددها در دستگاه حذف شوند.", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 501);
			}
			ret.AddInformation($"ترددهای مربوط به {setting.GetTaradodFormCountDaysAgo} روز اخیر دریافت می\u200cشود.", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 503);
			List<DeviceLogOnPerson> list2 = (from x in list
				where x.DateTimeLogM > DateTime.Now.AddDays(-setting.GetTaradodFormCountDaysAgo)
				orderby x.DateTimeLogM descending
				select x).ToList();
			Dictionary<string, DeviceInfo> deviceBySerial = (from p in allDeviceList
				where !string.IsNullOrEmpty(p.SerialNumberStr)
				group p by p.SerialNumberStr).ToDictionary((IGrouping<string, DeviceInfo> g) => g.Key, (IGrouping<string, DeviceInfo> g) => g.First());
			Dictionary<short, DeviceInfo> deviceByNo = (from p in allDeviceList
				group p by p.DeviceNo).ToDictionary((IGrouping<short, DeviceInfo> g) => g.Key, (IGrouping<short, DeviceInfo> g) => g.First());
			int countTaradod = 0;
			HashSet<int> listdeviceNos = new HashSet<int>();
			int i = 0;
			foreach (DeviceLogOnPerson item in list2)
			{
				if (token.IsCancellationRequested)
				{
					return ret;
				}
				DeviceInfo device = null;
				DeviceInfo value2;
				if (!string.IsNullOrEmpty(item.DeviceSerial) && deviceBySerial.TryGetValue(item.DeviceSerial, out var value))
				{
					device = value;
				}
				else if (deviceByNo.TryGetValue((short)item.DeviceNo, out value2))
				{
					device = value2;
				}
				if (device == null)
				{
					Log.Error($"دستگاه مورد نظر پیدا نشد: deviceSerial={item.DeviceSerial}, deviceNo={item.DeviceNo}, PersonNo={item.PersonNo}, DateTimeLogM={item.DateTimeLogM}, caller:ImportAllDataFromDeviceAsync");
					i++;
					continue;
				}
				int personId = (allHardwareCodes?.SingleOrDefault((HardwareCodeCus p) => p.DeviceId == device.Id && p.HardwareCode == item.PersonNo.ToString()))?.PersonId ?? 0;
				LogOnPersonel x2 = new LogOnPersonel(ContextGuid)
				{
					DeviceId_ = device.Id,
					DeviceNo = device.DeviceNo,
					DeviceSerial_ = device.SerialNumberStr,
					PersonId = personId,
					DateTimeLogM = item.FixedDateM,
					TrafficId = item.IoMode,
					PersonNo = item.PersonNo,
					TypeTaradod = (TypeTaradodEnumType)3,
					IsVirtual = (SavedTaradodEnumType)1,
					TypeTaradodVacationOrMission = (TypeTaradodEnumType)1
				};
				LogonTypeTaradod logonTypeTaradod = await LogonTypeTaradod.GetAsync(ContextGuid, (short)x2.TrafficId, device.Model, token).ConfigureAwait(continueOnCapturedContext: false);
				if (logonTypeTaradod != null)
				{
					x2.TypeTaradod = logonTypeTaradod.ConvertedTypeTaradod;
				}
				int maxid = await LogOnPersonel.GetMaxId(ContextGuid, token).ConfigureAwait(continueOnCapturedContext: false);
				if (await x2.SaveAsync("ImportAllDataFromDeviceAsync", default(CancellationToken), "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs").ConfigureAwait(continueOnCapturedContext: false) > maxid)
				{
					countTaradod++;
					if (x2.DeviceNo > 0 && !listdeviceNos.Contains(x2.DeviceNo))
					{
						listdeviceNos.Add(x2.DeviceNo);
					}
				}
				i++;
			}
			if (countTaradod > 0)
			{
				if (listdeviceNos.Count > 0)
				{
					string arg = string.Join(",", listdeviceNos);
					ret.AddInformation($"تعداد {countTaradod} تردد جدید از دستگاه\u200cهای {arg} دریافت شد.", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 586);
				}
				else
				{
					ret.AddInformation($"تعداد {countTaradod} تردد از دستگاه ناشناس دریافت شد", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 590);
				}
			}
			else
			{
				ret.AddInformation("تردد جدیدی جهت ثبت وجود ندارد", "", "ImportAllDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 595);
			}
			if (this.OnFinishGetData != null)
			{
				string message = "ترددها دریافت شدند.";
				EventNotification onFinishGetData = this.OnFinishGetData;
				if (onFinishGetData != null)
				{
					object sourceObj = SourceObj;
					onFinishGetData(sourceObj, await EventArgsNotify.GetAsync(EntityCacheConfig.CacheInstance.ContextGuid, Id, message, "پایان عملیات", (NotificationEnum)7, 3).ConfigureAwait(continueOnCapturedContext: false));
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
		return ret;
	}

	public static async Task<List<DeviceInfo>> List_Devices_Should_UpdateAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		new List<DeviceInfo>();
		try
		{
			return await List_Devices_Should_UpdateAsync(contextGuid, token);
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

	public async Task<int> LastDeviceNoAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("select max([DeviceNO]) + 1 as DeviceNo from [dbo].[DeviceInfo]", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						return Utilities.ParseToInt(obj.ToString(), 0);
					}
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
		return 1;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>> ReceiveDataFromDeviceAsync(FrmEbio ifDeviceIsEbio)
	{
		ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>> ret = new ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>();
		List<DeviceLogOnPerson> result = null;
		try
		{
			DeviceManufacture typeDeviceManufacture = TypeDeviceManufacture;
			switch ((int)typeDeviceManufacture)
			{
			case 0:
			{
				ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>> val = await (((int)TransferMode == 1) ? new ClsEizon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass, Ip, TcpPort.ToString()) : new ClsEizon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass)).GetAllAttendanceRecordAsync();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "ReceiveDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 680);
				result = val.value;
				break;
			}
			case 1:
			{
				ZDC2911Realand zDC2911Realand = new ZDC2911Realand(EntityCacheConfig.CacheInstance.ContextGuid, (int)TransferMode == 1, DeviceNo, CommunicationPass, Ip, TcpPort);
				result = zDC2911Realand.GetAll();
				break;
			}
			case 2:
			{
				if (ifDeviceIsEbio == null)
				{
					ret.AddError("فرم ورودی تابع نال است . بررسی شود", "ReceiveDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 690);
					throw new NullReferenceException("فرم ورودی تابع نال است . بررسی شود");
				}
				FrmEbio xclsEbio;
				bool flag;
				if ((int)TransferMode == 1)
				{
					xclsEbio = ifDeviceIsEbio;
					flag = await xclsEbio.ConnectLocalAsync();
				}
				else
				{
					xclsEbio = new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo);
					flag = await xclsEbio.ConnectSerialAsync();
				}
				if (!flag)
				{
					ret.AddError("ارتباط با دستگاه برقرار نشد", "ReceiveDataFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 708);
					return ret;
				}
				if (AutoSetDateTime)
				{
					xclsEbio.SetDateTime(DateTime.Now);
				}
				result = xclsEbio.GetAllLogOnPublic(deleteAfter: false, GetNewTaradodOnly);
				xclsEbio.Disconnect();
				break;
			}
			case 3:
			{
				clsHysoon clsHysoon = (((int)TransferMode == 1) ? new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, int.Parse(CommunicationPass), Ip, TcpPort, GetNewTaradodOnly) : new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo));
				if (!clsHysoon.Conected)
				{
					return ret;
				}
				if (AutoSetDateTime)
				{
					clsHysoon.SetDateTime(DateTime.Now);
				}
				result = clsHysoon.GetAllLogOnPublic();
				clsHysoon.Disconnect();
				break;
			}
			case 4:
				result = await new ClsZK(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass, Ip, TcpPort).GetAll(GetNewTaradodOnly);
				break;
			case 5:
			{
				ClsDynamicHysoon clsDynamicHysoon = new ClsDynamicHysoon(Ip, TcpPort, Utilities.ParseToInt(CommunicationPass, 0));
				if (clsDynamicHysoon.Connect())
				{
					List<DynamicHysoonInfo.Logs> logData = clsDynamicHysoon.GetLogData(DateTime.Now.AddDays(-365.0), DateTime.Now, GetNewTaradodOnly, clearNewLogs: true);
					clsDynamicHysoon.DisConnect();
					result = RecordAllDevice.GetAllFromDynamicHysoon(ContextGuid, logData, DeviceNo);
				}
				if (AutoSetDateTime)
				{
					clsDynamicHysoon.SetDateTime(DateTime.Now);
				}
				break;
			}
			case 6:
			{
				VisibleLightLanSDK sdk = new VisibleLightLanSDK(Ip, CommunicationPass);
				if (!(await sdk.IsConnectAsync()))
				{
					break;
				}
				(int, int) tuple = await sdk.GetLogCountAsync();
				int newTaradod = tuple.Item1;
				int allTaradod = tuple.Item2;
				List<Logdata> totalLogs = new List<Logdata>();
				if (GetNewTaradodOnly)
				{
					for (int startIndex = 0; startIndex < newTaradod; startIndex += 100)
					{
						List<Logdata> list = await sdk.GetLogInfoAsync(100, startIndex, GetNewTaradodOnly);
						if (list != null && list.Count > 0)
						{
							totalLogs.AddRange(list);
						}
					}
				}
				else
				{
					for (int startIndex = 0; startIndex < allTaradod; startIndex += 100)
					{
						List<Logdata> list2 = await sdk.GetLogInfoAsync(100, startIndex, false);
						if (list2 != null && list2.Count > 0)
						{
							totalLogs.AddRange(list2);
						}
					}
				}
				if (totalLogs.Count > 0)
				{
					List<DeviceLogOnPerson> list3 = new List<DeviceLogOnPerson>();
					foreach (Logdata item in totalLogs)
					{
						list3.Add(LogonPersonalMapper.Instance.Map(ContextGuid, item));
					}
					result = list3;
				}
				if (AutoSetDateTime)
				{
					sdk.SetTimeAsync(DateTime.Now);
				}
				break;
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
		ret.value = result;
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> TestConnectionAsync()
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		bool value = false;
		try
		{
			if (IsValidObjects())
			{
				DeviceManufacture typeDeviceManufacture = TypeDeviceManufacture;
				switch ((int)typeDeviceManufacture)
				{
				case 0:
				{
					ReturnedSaveFuncInfoWithValue<ClsEizon.Systemcase> val = await (((int)TransferMode == 1) ? new ClsEizon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass, Ip, TcpPort.ToString()) : new ClsEizon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass)).SystemCheckAsync();
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "TestConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 823);
					value = val.value == ClsEizon.Systemcase.Ready;
					break;
				}
				case 1:
					value = new ZDC2911Realand(EntityCacheConfig.CacheInstance.ContextGuid, (int)TransferMode == 1, DeviceNo, CommunicationPass, Ip, TcpPort).TestConnection();
					break;
				case 2:
					if ((int)TransferMode == 1)
					{
						FrmEbio xclsEbio = new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, Utilities.ParseToInt(CommunicationPass, 0), Ip, TcpPort);
						value = await xclsEbio.ConnectLocalAsync();
						xclsEbio.Disconnect();
					}
					else
					{
						FrmEbio xclsEbio = new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo);
						value = await xclsEbio.ConnectSerialAsync();
						xclsEbio.Disconnect();
					}
					break;
				case 3:
				{
					clsHysoon obj = (((int)TransferMode == 1) ? new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, Utilities.ParseToInt(CommunicationPass, 0), Ip, TcpPort, GetNewTaradodOnly) : new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo));
					value = obj.TestConnection();
					obj.Disconnect();
					break;
				}
				case 4:
					value = new ClsZK(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass, Ip, TcpPort).TestConnection();
					break;
				case 5:
					value = new ClsDynamicHysoon(Ip, TcpPort, Utilities.ParseToInt(CommunicationPass, 0)).CheckConnectionToDevice();
					break;
				case 6:
					value = await new VisibleLightLanSDK(Ip, "").IsConnectAsync();
					break;
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
		ret.value = value;
		return ret;
	}

	protected bool IsValidObjects()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		bool flag = true;
		try
		{
			if ((int)TransferMode == 1)
			{
				if (Enumerable.Contains(Ip, '.'))
				{
					string[] array = Ip.Split(new char[1] { '.' });
					if (array.Length != 4)
					{
						flag = false;
					}
					else if (Utilities.ParseToInt(array[0].Trim(), 0) <= 0)
					{
						flag = false;
					}
					else if (Utilities.ParseToInt(array[1].Trim(), 0) < 0)
					{
						flag = false;
					}
					else if (Utilities.ParseToInt(array[2].Trim(), 0) < 0)
					{
						flag = false;
					}
					else if (Utilities.ParseToInt(array[3].Trim(), 0) < 0)
					{
						flag = false;
					}
					if (flag && TcpPort <= 0)
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsValidObjects", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 909);
		}
		return flag;
	}

	public async Task<ReturnedSaveFuncInfo> SyncDateTimeOnDeviceAsync()
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			DeviceManufacture typeDeviceManufacture = TypeDeviceManufacture;
			switch ((int)typeDeviceManufacture)
			{
			case 0:
				ret.AddReturnedValue(await (((int)TransferMode == 1) ? new ClsEizon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass, Ip, TcpPort.ToString()) : new ClsEizon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, CommunicationPass)).SetSystemTimeAsync(DateTime.Now), (ReturnedState)0, "SyncDateTimeOnDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 924);
				break;
			case 2:
				(((int)TransferMode == 1) ? new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, int.Parse(CommunicationPass), Ip, TcpPort) : new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo)).SetDateTime(DateTime.Now);
				break;
			case 3:
			{
				clsHysoon obj = (((int)TransferMode == 1) ? new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, int.Parse(CommunicationPass), Ip, TcpPort, GetNewTaradodOnly) : new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo));
				obj.SetDateTime(DateTime.Now);
				obj.Disconnect();
				break;
			}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SyncDateTimeOnDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 941);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SyncDateTimeOnDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 942);
		}
		return ret;
	}

	public async Task<List<EnrollData>> GetEnrollDatasAsync(List<int> personIds = null, CancellationToken token = default(CancellationToken))
	{
		new List<EnrollData>();
		try
		{
			throw new NotImplementedException();
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

	public int ClearEnrollData()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		int result = 0;
		try
		{
			DeviceManufacture typeDeviceManufacture = TypeDeviceManufacture;
			if ((int)typeDeviceManufacture != 2)
			{
				if ((int)typeDeviceManufacture == 3)
				{
					result = (((int)TransferMode == 1) ? new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, Utilities.ParseToInt(CommunicationPass, 0), Ip, TcpPort, GetNewTaradodOnly) : new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo)).FuncClearEnrollData();
				}
			}
			else
			{
				result = ((((int)TransferMode == 1) ? new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, int.Parse(CommunicationPass), Ip, TcpPort) : new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo)).ClearEnrollData() ? 1 : 0);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ClearEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 1012);
		}
		return result;
	}

	public void PutEnrollData(List<EnrollData> pEnrollDatas)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		try
		{
			DeviceManufacture typeDeviceManufacture = TypeDeviceManufacture;
			if ((int)typeDeviceManufacture != 2)
			{
				if ((int)typeDeviceManufacture == 3)
				{
					(((int)TransferMode == 1) ? new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, int.Parse(CommunicationPass), Ip, TcpPort, GetNewTaradodOnly) : new clsHysoon(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo)).FuncPutEnrollData(pEnrollDatas.Where((EnrollData q) => q.DeviceId.Equals(Id)).ToList());
				}
			}
			else
			{
				(((int)TransferMode == 1) ? new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo, int.Parse(CommunicationPass), Ip, TcpPort) : new FrmEbio(EntityCacheConfig.CacheInstance.ContextGuid, DeviceNo)).PutAllEnrollData(pEnrollDatas.Where((EnrollData q) => q.DeviceId.Equals(Id)).ToList());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "PutEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 1036);
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<DeviceInfo>> CheckAndGetDeviceInfoAsync(string contextGuid, List<string> spamSerialList, Dictionary<string, DeviceInfo> deviceDictionary, string serialNumber, short deviceNo, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<DeviceInfo> ret = new ReturnedSaveFuncInfoWithValue<DeviceInfo>();
		try
		{
			if (spamSerialList != null && spamSerialList.Count > 0 && spamSerialList.Contains(serialNumber))
			{
				ret.AddError((EnSystem)2, "the device is not define for you...", "CheckAndGetDeviceInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 1098);
				return ret;
			}
			DeviceInfo deviceInfo = null;
			deviceDictionary?.TryGetValue(serialNumber, out deviceInfo);
			if (deviceInfo == null)
			{
				DeviceInfo deviceInfo2 = await GetByDeviceNoAsync(contextGuid, deviceNo, token);
				if (deviceInfo2 == null || deviceInfo2.SerialNumberStr != serialNumber)
				{
					spamSerialList?.Add(serialNumber);
					string text = $"دستگاهی با سریال: {serialNumber} , شناسه: {deviceNo}  \r\n در لیست دستگاههای شما شناسایی نشد\r\n لذا تردد های دستگاه مذکور دریافت نخواهند شد";
					TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)5, text, "", 6);
					string arg = $"device with serial: {serialNumber} and deviceId: {deviceNo} \r\n not found in system of customer with lkSerial:\r\n {VersionManagment.LkSerial}";
					// [SANITIZED] ارسال سریال دستگاه و سریال قفل به ربات تلگرام حذف شد
					ret.AddError((EnSystem)2, text, "CheckAndGetDeviceInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 1113);
					return ret;
				}
				if (deviceDictionary != null && !deviceDictionary.ContainsKey(serialNumber))
				{
					deviceDictionary?.Add(serialNumber, deviceInfo2);
				}
				deviceDictionary?.TryGetValue(serialNumber, out deviceInfo);
			}
			ret.value = deviceInfo;
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
		return ret;
	}

	public MicroDeviceInfo MappToMicro()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		MicroDeviceInfo result = default(MicroDeviceInfo);
		((MicroDeviceInfo)(ref result)).Id = Id;
		return result;
	}

	public static async Task<string> GetUserNameAsync(string contextGuid, DeviceInfo device, HardwareCodeCus hardwareCode)
	{
		try
		{
			if (device == null)
			{
				return null;
			}
			if (hardwareCode == null)
			{
				return null;
			}
			_ = device.Model;
			_ = 9304;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return null;
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetFromDeviceHysoon587Async(string contextGuid, DeviceInfo device, int hardwareCode)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		ReturnedSaveFuncInfoWithValue<string> val = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			if (device == null)
			{
				return null;
			}
			if (hardwareCode <= 0)
			{
				return null;
			}
			clsHysoon clsHysoon = (((int)device.TransferMode == 1) ? new clsHysoon(contextGuid, device.DeviceNo, int.Parse(device.CommunicationPass), device.Ip, device.TcpPort, getNewTaradodOnly: true) : new clsHysoon(contextGuid, device.DeviceNo));
			if (!clsHysoon.Conected)
			{
				val.AddError("ارتباط با دستگاه امکان پذیر نیست", "GetFromDeviceHysoon587Async", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\DeviceInfo.cs", 1161);
				return val;
			}
			val = clsHysoon.GetUserName(hardwareCode);
			clsHysoon.Disconnect();
			return val;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
