using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache;
using EntityCache.Business.Accounting;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Enroll;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Mapper;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDevices;

namespace TimeAttendanceDAL.Dal.FLRPerson;

[Serializable]
[DoNotPruneType]
public class Person : PersonBase, IHasContext
{
	public static List<Person> Persons = null;

	public static DateTime LastUpdateTime = DateTime.Now.AddSeconds(-120.0);

	private static CancellationTokenSource _sendPersonsTokenSource;

	private string _strEnrollDatas;

	private string _groupName;

	private static SemaphoreSlim _semaphoreCalcPersonId = new SemaphoreSlim(1, 1);

	private List<HardwareCodeCUSView> _hardWareCodes;

	private List<EnrollData> _enrollDataObj;

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public override DateTime DateMStartWork { get; set; } = ConstantValues.MinDateM;


	public override DateTime DateMBirthday { get; set; } = ConstantValues.MinDateM;


	public override DateTime DateMFinishW { get; set; } = ConstantValues.MinDateM;


	public bool Check { get; set; }

	public byte[] HashPassword { get; set; }

	public string UserName { get; set; }

	public byte[] ServerStatusImage
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Invalid comparison between Unknown and I4
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Invalid comparison between Unknown and I4
			ServerStatusForClient val = sentToServerStatus;
			if (((int)val != 2 && (int)val != 5) || 1 == 0)
			{
				if ((int)sentToServerStatus != 4)
				{
					if ((int)sentToServerStatus != 1)
					{
						if ((int)sentToServerStatus != 3)
						{
							return ImageResourceManager.ServerStatusNone;
						}
						return ImageResourceManager.ServerStatusSentError;
					}
					return ImageResourceManager.ServerStatusSent;
				}
				return ImageResourceManager.ServerStatusDeliveryFailed;
			}
			return ImageResourceManager.ServerStatusDelivered;
		}
	}

	public ServerStatusForClient sentToServerStatus { get; set; }

	public static event Func<Task> OnServerStatusChanged;

	public async Task<List<HardwareCodeCUSView>> GetHardWareCodesAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (((PersonBase)this).Id > 0 && _hardWareCodes == null)
			{
				_hardWareCodes = await HardwareCodeCUSView.GetAsync(ContextGuid, ((PersonBase)this).Id, token);
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
		return _hardWareCodes;
	}

	public async Task<string> GetGroupNameAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			string text = _groupName;
			if (text == null)
			{
				text = (_groupName = (await PersonGroup.GetAsync(EntityCacheConfig.CacheInstance.ContextGuid, ((PersonBase)this).GroupId, token)).Name);
			}
			return text;
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

	public async Task<string> GetStrEnrollDatasAsync(CancellationToken token)
	{
		try
		{
			string text = _strEnrollDatas;
			if (text == null)
			{
				text = (_strEnrollDatas = await LoadEnrollDataAsync(token));
			}
			return text;
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

	public void SetStrEnrollDatas(string value)
	{
		_strEnrollDatas = value;
	}

	public async Task<List<EnrollData>> GetEnrollDataObjAsync(CancellationToken token)
	{
		try
		{
			List<EnrollData> list = _enrollDataObj;
			if (list == null)
			{
				list = (_enrollDataObj = JsonConvert.DeserializeObject<List<EnrollData>>(await GetStrEnrollDatasAsync(token)));
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

	public void SetEnrollDataObj(List<EnrollData> value)
	{
		_enrollDataObj = value;
		SetStrEnrollDatas(JsonConvert.SerializeObject((object)_enrollDataObj));
	}

	public Person(string contextGuid)
	{
		if (contextGuid.Length < 10)
		{
			Thread.Sleep(2000);
		}
		ContextGuid = contextGuid;
	}

	public static async Task<Person> GetByPersonIdAsync(string contextGuid, int personId, CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<Person> list = await GetAllAsync(contextGuid, new List<int>(1) { personId }, personStatus: true, token);
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			return list.SingleOrDefault((Person p) => ((PersonBase)p).Id == personId);
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

	public static async Task<Person> GetBypersonGuidAsync(string contextGuid, Guid personGuid, CancellationToken token)
	{
		try
		{
			List<Person> list = await GetAllAsync(contextGuid, new List<int>(), personStatus: true);
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			return list.SingleOrDefault((Person p) => ((PersonBase)p).Guid == personGuid);
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

	public static async Task<Person> GetByPersonCodeTafsilAsync(string contextGuid, long personCodeTafsil, CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<Person> list = await GetAllAsync(contextGuid, new List<int>(), personStatus: true);
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			return list.SingleOrDefault((Person p) => ((PersonBase)p).CodeTafsil == personCodeTafsil);
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

	public static async Task<Person> GetbyDeviceIdPersonNoAsync(string contextGuid, int deviceId, int personNo, CancellationToken token)
	{
		_ = 2;
		try
		{
			Person ret = new Person(contextGuid);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_Person_SelectBy_deviceIdPersonNo]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@DeviceID", (object)deviceId);
					cmd.Parameters.AddWithValue("@PersonNo", (object)personNo);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
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

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 4;
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
					cmd.Parameters.AddWithValue("GroupID", (object)((PersonBase)this).GroupId);
					cmd.Parameters.AddWithValue("TitleID", (object)((PersonBase)this).TitleId);
					cmd.Parameters.AddWithValue("Hid", (object)((PersonBase)this).Hid);
					cmd.Parameters.AddWithValue("HType", (object)((PersonBase)this).HType);
					cmd.Parameters.AddWithValue("Code", (object)" ");
					cmd.Parameters.AddWithValue("Type", (object)((PersonBase)this).Type);
					cmd.Parameters.AddWithValue("FullCode", (object)Text.FixString(((PersonBase)this).FullCode));
					cmd.Parameters.AddWithValue("Name", (object)Text.FixString(((PersonBase)this).Name));
					cmd.Parameters.AddWithValue("LatinFullName", (object)Text.FixString(((PersonBase)this).LatinFullName));
					cmd.Parameters.AddWithValue("Family", (object)Text.FixString(((PersonBase)this).Family));
					cmd.Parameters.AddWithValue("FullName", (object)Text.FixString(((PersonBase)this).FullName));
					cmd.Parameters.AddWithValue("FatherName", (object)Text.FixString(((PersonBase)this).FatherName));
					cmd.Parameters.AddWithValue("DateMStartWork", (object)((PersonBase)this).DateMStartWork);
					cmd.Parameters.AddWithValue("DateMFinishW", (object)((PersonBase)this).DateMFinishW);
					cmd.Parameters.AddWithValue("DateMBirthday", (object)((PersonBase)this).DateMBirthday);
					cmd.Parameters.AddWithValue("CodeMeli", (object)Text.FixString(((PersonBase)this).CodeMeli));
					cmd.Parameters.AddWithValue("ZipCode", (object)Text.FixString(((PersonBase)this).ZipCode));
					cmd.Parameters.AddWithValue("Adress", (object)Text.FixString(((PersonBase)this).Address));
					cmd.Parameters.AddWithValue("Mob1", (object)Text.FixString(((PersonBase)this).Mob1));
					cmd.Parameters.AddWithValue("Mob2", (object)Text.FixString(((PersonBase)this).Mob2));
					cmd.Parameters.AddWithValue("Tel1", (object)Text.FixString(((PersonBase)this).Tel1));
					cmd.Parameters.AddWithValue("Tel2", (object)Text.FixString(((PersonBase)this).Tel2));
					cmd.Parameters.AddWithValue("St", (object)((PersonBase)this).St);
					cmd.Parameters.AddWithValue("Guid", (object)((PersonBase)this).Guid);
					cmd.Parameters.AddWithValue("PicPath", (object)Text.FixString(((PersonBase)this).PicPath));
					cmd.Parameters.AddWithValue("Description", (object)Text.FixString(((PersonBase)this).Description));
					cmd.Parameters.AddWithValue("NOBimeh", (object)Text.FixString(((PersonBase)this).NoBimeh));
					cmd.Parameters.AddWithValue("SodorSH", (object)Text.FixString(((PersonBase)this).SodorSh));
					cmd.Parameters.AddWithValue("MTavalod", (object)Text.FixString(((PersonBase)this).MTavalod));
					cmd.Parameters.AddWithValue("CodeTafsil", (object)((PersonBase)this).CodeTafsil);
					cmd.Parameters.AddWithValue("CodeMoein", (object)((PersonBase)this).CodeMoein);
					cmd.Parameters.AddWithValue("EisarID", (object)((PersonBase)this).EisarId);
					cmd.Parameters.AddWithValue("SHSH", (object)Text.FixString(((PersonBase)this).Shsh));
					cmd.Parameters.AddWithValue("SerialSH", (object)Text.FixString(((PersonBase)this).SerialSh));
					cmd.Parameters.AddWithValue("Jensiat", (object)((PersonBase)this).Jensiat);
					cmd.Parameters.AddWithValue("MadrakTahsili", (object)Text.FixString(((PersonBase)this).MadrakTahsili));
					cmd.Parameters.AddWithValue("UserName", (object)(UserName ?? ""));
					cmd.Parameters.AddWithValue("HashPassword", (object)(HashPassword ?? new byte[50]));
					cmd.Parameters.AddWithValue("sentToServerStatus", (object)sentToServerStatus);
					cmd.Parameters.AddWithValue("DepartmentGuid", (object)((PersonBase)this).GuidDepartment);
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (((PersonBase)this).Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_Person_Insert]";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							((PersonBase)this).Id = Convert.ToInt32(obj.ToString());
						}
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "[افزودن اطلاعات یک شخص جدید] : " + ((PersonBase)this).FullName + " [با کد تفصیلی  ] : " + ((PersonBase)this).CodeTafsil, "Personnel.PersonnelInsert", this, ((PersonBase)this).Id.ToString());
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_Person_Update]";
						cmd.Parameters.AddWithValue("ID", (object)((PersonBase)this).Id);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "[ویرایش اطلاعات پرسنلی] : " + ((PersonBase)this).FullName + " [با کد تفصیلی  ] : " + ((PersonBase)this).CodeTafsil, "Personnel.PersonnelUpdate", this, ((PersonBase)this).Id.ToString());
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

	public async Task<bool> DeleteAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Person_DeleteRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("ID", (object)((PersonBase)this).Id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					string pDesc = "[حذف اطلاعات پرسنلی] : " + ((PersonBase)this).FullName + " [با کد تفصیلی  ] : " + ((PersonBase)this).CodeTafsil;
					await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, pDesc, "Personnel.PersonnelDelete", this, ((PersonBase)this).Id.ToString());
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

	public async Task<string> LoadEnrollDataAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			byte[] result = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("Select EnrollData from Person Where ID = " + ((PersonBase)this).Id, cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["EnrollData"].ToString() != DBNull.Value.ToString())
						{
							result = (byte[])((DbDataReader)(object)dr)["EnrollData"];
						}
						((DbConnection)(object)cn).Close();
						return (result != null) ? Encoding.UTF8.GetString(result) : "";
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

	public async Task GetEnrollDataAsync(CancellationToken token)
	{
		try
		{
			List<int> personIdList = new List<int> { ((PersonBase)this).Id };
			SetEnrollDataObj(await EnrollManage.GetEnrollManage(EntityCacheConfig.CacheInstance.ContextGuid).GetEnrollDataAsync(personIdList, token));
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

	public async Task PutEnrollDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			EnrollManage enrollManage = EnrollManage.GetEnrollManage(EntityCacheConfig.CacheInstance.ContextGuid);
			await enrollManage.PutEnrollDataAsync(await GetEnrollDataObjAsync(token), token);
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

	public async Task<bool> SaveEnrollDataAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			ASCIIEncoding aSCIIEncoding2 = aSCIIEncoding;
			byte[] bytes = aSCIIEncoding2.GetBytes(await GetStrEnrollDatasAsync(token));
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_Person_SaveEnrollData", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("ID", (object)((PersonBase)this).Id);
					cmd.Parameters.AddWithValue("serializeObj", (object)bytes);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
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

	public static async Task<List<EnrollData>> LoadEnrollDataAsync(string contextGuid, string personIDs = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<EnrollData> xret = new List<EnrollData>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Person_LoadEnrollData]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					cmd.Parameters.AddWithValue("personIDs", (object)personIDs);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							int num = 0;
							if (((DbDataReader)(object)dr)["ID"].ToString() != DBNull.Value.ToString())
							{
								num = (int)((DbDataReader)(object)dr)["ID"];
							}
							string text = "";
							if (((DbDataReader)(object)dr)["EnrollData"].ToString() != DBNull.Value.ToString())
							{
								text = ((DbDataReader)(object)dr)["EnrollData"].ToString();
							}
							if (!(text == ""))
							{
								EnrollData enrollData = JsonConvert.DeserializeObject<EnrollData>(text);
								if (enrollData.PersonId == num)
								{
									xret.Add(enrollData);
								}
							}
						}
						return xret;
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

	public static async Task<bool> SaveEnrollDataAsync(string contextGuid, List<EnrollData> enrollDataList, CancellationToken token)
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Person_SaveEnrollData]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					foreach (EnrollData enrollData in enrollDataList)
					{
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						cmd.Parameters.AddWithValue("ID", (object)enrollData.PersonId);
						string text = JsonConvert.SerializeObject((object)enrollData, (Formatting)1);
						cmd.Parameters.AddWithValue("serializeObj", (object)text);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
					await LogUsers.RegisterNewLogAsync(contextGuid, "[ذخیره اطلاعات اثر انگشت تعدادی از پرسنل] : ", "Personnel.PersonnelSaveEnrollData", enrollDataList, "");
					((DbConnection)(object)cn).Close();
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

	public async Task<string> GenerateUrlQueryStringAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		// [SANITIZED] ساخت لینک ورود ابری حذف شد: نام کاربری و هش رمز شخص به سرور سازنده ارسال می‌شد.
		return "---";
	}

	private static Person LoadData(string contextGuid, SqlDataReader dr)
	{
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Person person = new Person(contextGuid);
			if (((DbDataReader)(object)dr)["Id"] != DBNull.Value)
			{
				((PersonBase)person).Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["CodeTafsil"] != DBNull.Value)
			{
				((PersonBase)person).CodeTafsil = long.Parse(((DbDataReader)(object)dr)["CodeTafsil"].ToString());
			}
			if (((DbDataReader)(object)dr)["CodeMoein"] != DBNull.Value)
			{
				((PersonBase)person).CodeMoein = long.Parse(((DbDataReader)(object)dr)["CodeMoein"].ToString());
			}
			if (((DbDataReader)(object)dr)["GroupID"] != DBNull.Value)
			{
				((PersonBase)person).GroupId = (short)((DbDataReader)(object)dr)["GroupID"];
			}
			if (((DbDataReader)(object)dr)["TitleID"] != DBNull.Value)
			{
				((PersonBase)person).TitleId = (short)((DbDataReader)(object)dr)["TitleID"];
			}
			if (((DbDataReader)(object)dr)["Name"] != DBNull.Value)
			{
				((PersonBase)person).Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["LatinFullName"] != DBNull.Value)
			{
				((PersonBase)person).LatinFullName = ((DbDataReader)(object)dr)["LatinFullName"].ToString();
			}
			if (((DbDataReader)(object)dr)["Family"] != DBNull.Value)
			{
				((PersonBase)person).Family = ((DbDataReader)(object)dr)["Family"].ToString();
			}
			if (((DbDataReader)(object)dr)["Mob1"] != DBNull.Value)
			{
				((PersonBase)person).Mob1 = ((DbDataReader)(object)dr)["Mob1"].ToString().Trim();
			}
			if (((DbDataReader)(object)dr)["Mob2"] != DBNull.Value)
			{
				((PersonBase)person).Mob2 = ((DbDataReader)(object)dr)["Mob2"].ToString().Trim();
			}
			if (((DbDataReader)(object)dr)["Tel1"] != DBNull.Value)
			{
				((PersonBase)person).Tel1 = ((DbDataReader)(object)dr)["Tel1"].ToString().Trim();
			}
			if (((DbDataReader)(object)dr)["Tel2"] != DBNull.Value)
			{
				((PersonBase)person).Tel2 = ((DbDataReader)(object)dr)["Tel2"].ToString().Trim();
			}
			if (((DbDataReader)(object)dr)["Adress"] != DBNull.Value)
			{
				((PersonBase)person).Address = ((DbDataReader)(object)dr)["Adress"].ToString();
			}
			if (((DbDataReader)(object)dr)["Description"] != DBNull.Value)
			{
				((PersonBase)person).Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["CodeMeli"] != DBNull.Value)
			{
				((PersonBase)person).CodeMeli = ((DbDataReader)(object)dr)["CodeMeli"].ToString();
			}
			if (((DbDataReader)(object)dr)["ZipCode"] != DBNull.Value)
			{
				((PersonBase)person).ZipCode = ((DbDataReader)(object)dr)["ZipCode"].ToString();
			}
			if (((DbDataReader)(object)dr)["SHSH"] != DBNull.Value)
			{
				((PersonBase)person).Shsh = ((DbDataReader)(object)dr)["SHSH"].ToString();
			}
			if (((DbDataReader)(object)dr)["SodorSH"] != DBNull.Value)
			{
				((PersonBase)person).SodorSh = ((DbDataReader)(object)dr)["SodorSH"].ToString();
			}
			if (((DbDataReader)(object)dr)["FatherName"] != DBNull.Value)
			{
				((PersonBase)person).FatherName = ((DbDataReader)(object)dr)["FatherName"].ToString();
			}
			if (((DbDataReader)(object)dr)["SerialSH"] != DBNull.Value)
			{
				((PersonBase)person).SerialSh = ((DbDataReader)(object)dr)["SerialSH"].ToString();
			}
			if (((DbDataReader)(object)dr)["Jensiat"] != DBNull.Value)
			{
				((PersonBase)person).Jensiat = (short)((DbDataReader)(object)dr)["Jensiat"];
			}
			if (((DbDataReader)(object)dr)["DateMBirthday"] != DBNull.Value)
			{
				((PersonBase)person).DateMBirthday = (DateTime)((DbDataReader)(object)dr)["DateMBirthday"];
			}
			if (((DbDataReader)(object)dr)["NOBimeh"] != DBNull.Value)
			{
				((PersonBase)person).NoBimeh = ((DbDataReader)(object)dr)["NOBimeh"].ToString();
			}
			if (((DbDataReader)(object)dr)["MadrakTahsili"] != DBNull.Value)
			{
				((PersonBase)person).MadrakTahsili = ((DbDataReader)(object)dr)["MadrakTahsili"].ToString();
			}
			if (((DbDataReader)(object)dr)["DateMStartWork"] != DBNull.Value)
			{
				((PersonBase)person).DateMStartWork = (DateTime)((DbDataReader)(object)dr)["DateMStartWork"];
			}
			if (((DbDataReader)(object)dr)["DateMFinishW"] != DBNull.Value)
			{
				((PersonBase)person).DateMFinishW = (DateTime)((DbDataReader)(object)dr)["DateMFinishW"];
			}
			if (((DbDataReader)(object)dr)["EisarID"] != DBNull.Value)
			{
				((PersonBase)person).EisarId = (short)((DbDataReader)(object)dr)["EisarID"];
			}
			if (((DbDataReader)(object)dr)["Hid"] != DBNull.Value)
			{
				((PersonBase)person).Hid = (int)((DbDataReader)(object)dr)["Hid"];
			}
			if (((DbDataReader)(object)dr)["HType"] != DBNull.Value)
			{
				((PersonBase)person).HType = (short)((DbDataReader)(object)dr)["HType"];
			}
			if (((DbDataReader)(object)dr)["Type"] != DBNull.Value)
			{
				((PersonBase)person).Type = (short)((DbDataReader)(object)dr)["Type"];
			}
			if (((DbDataReader)(object)dr)["FullCode"] != DBNull.Value)
			{
				((PersonBase)person).FullCode = ((DbDataReader)(object)dr)["FullCode"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"] != DBNull.Value)
			{
				((PersonBase)person).St = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["Guid"] != DBNull.Value)
			{
				((PersonBase)person).Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["PicPath"] != DBNull.Value)
			{
				((PersonBase)person).PicPath = ((DbDataReader)(object)dr)["PicPath"].ToString();
			}
			if (((DbDataReader)(object)dr)["MTavalod"] != DBNull.Value)
			{
				((PersonBase)person).MTavalod = ((DbDataReader)(object)dr)["MTavalod"].ToString();
			}
			if (((DbDataReader)(object)dr)["UserName"] != DBNull.Value)
			{
				person.UserName = ((DbDataReader)(object)dr)["UserName"].ToString();
			}
			if (((DbDataReader)(object)dr)["HashPassword"] != DBNull.Value)
			{
				person.HashPassword = (byte[])((DbDataReader)(object)dr)["HashPassword"];
			}
			if (((DbDataReader)(object)dr)["sentToServerStatus"] != DBNull.Value)
			{
				person.sentToServerStatus = (ServerStatusForClient)((DbDataReader)(object)dr)["sentToServerStatus"];
			}
			if (((DbDataReader)(object)dr)["DepartmentGuid"] != DBNull.Value)
			{
				((PersonBase)person).GuidDepartment = (Guid)((DbDataReader)(object)dr)["DepartmentGuid"];
			}
			return person;
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

	public static async Task<List<Person>> GetAllBySentToServerStatusAsync(string contextGuid, ServerStatusForClient sentToServerStatus, CancellationToken token = default(CancellationToken))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_ = 2;
		try
		{
			List<Person> ret = new List<Person>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_Person_SelectAll_BySentToServerStatus]", cn);
				try
				{
					cmd.Parameters.AddWithValue("sentToServerStatus", (object)sentToServerStatus);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("personIDs", (object)"");
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							token.ThrowIfCancellationRequested();
							ret.Add(LoadData(contextGuid, dr));
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						return ret;
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

	public static void Reset()
	{
		LastUpdateTime = DateTime.Now.AddSeconds(-120.0);
	}

	public static async Task<List<Person>> GetAllAsync(string contextGuid, List<int> listPersonId, bool personStatus, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (LastUpdateTime.AddSeconds(60.0) < DateTime.Now)
			{
				LastUpdateTime = DateTime.Now;
				List<Person> ret = new List<Person>();
				SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
				try
				{
					SqlCommand cmd = new SqlCommand("[sp_Person_SelectAll]", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("personIDs", (object)"");
						await ((DbConnection)(object)cn).OpenAsync(token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
						try
						{
							while (await ((DbDataReader)(object)dr).ReadAsync(token))
							{
								token.ThrowIfCancellationRequested();
								ret.Add(LoadData(contextGuid, dr));
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)cn).Close();
							Persons = ret;
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
			List<Person> list = Persons;
			if (personStatus)
			{
				list = Persons.Where((Person p) => p != null && ((PersonBase)p).St).ToList();
			}
			if (listPersonId?.Any() ?? false)
			{
				list = list?.Where((Person p) => listPersonId.Contains(((PersonBase)p).Id))?.ToList();
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

	public static async Task<long> NextCodeMoeinAsync(string contextGuid, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select (isnull((select max(CodeMoein) from [Person] where [St]='true'),0))", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					long result;
					return long.TryParse(obj.ToString(), out result) ? (result + 1) : 1;
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

	public static async Task<long> NextCodeTafsilAsync(string contextGuid, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select (isnull((select max(CodeTafsil) from [Person] where [St]='true'),0))", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					long result;
					return long.TryParse(obj.ToString(), out result) ? (result + 1) : 1;
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

	public static async Task<int> CodeT2IdAsync(string contextGuid, string codeT, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select ID from [Person] where [St]='true' and CodeTafsil=" + Text.RemoveNoNumbers(codeT, ""), cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					return (obj != null) ? int.Parse(obj.ToString()) : 0;
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

	public static async Task<int> GetCountByCodeMAsync(string contextGuid, int id, string codeM, string codeT, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("", cn);
				try
				{
					cmd.Parameters.AddWithValue("@ID", (object)id);
					cmd.Parameters.AddWithValue("@CodeMoein", (object)codeM);
					cmd.Parameters.AddWithValue("@CodeTafsil", (object)codeT);
					((DbCommand)(object)cmd).CommandText = "Select Count(*) from Person Where CodeMoein=@CodeMoein and CodeTafsil=@CodeTafsil and ID<>@ID and St='True'";
					((DbCommand)(object)cmd).CommandText = "Select isnull((" + ((DbCommand)(object)cmd).CommandText + "),0)";
					await ((DbConnection)(object)cn).OpenAsync(token);
					string? s = (await ((DbCommand)(object)cmd).ExecuteScalarAsync(token)).ToString();
					((DbConnection)(object)cn).Close();
					return int.Parse(s);
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

	public static async Task SendWebPersonToServerAsync(string contextGuid, CancellationToken baseToken = default(CancellationToken))
	{
		_sendPersonsTokenSource?.Cancel();
		_sendPersonsTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { baseToken });
		CancellationToken token = _sendPersonsTokenSource.Token;
		while (!token.IsCancellationRequested)
		{
			try
			{
				_ = 1;
				try
				{
					await Task.Delay(1800000, token);
					ClsCache cache = ClsCache.GetCache(contextGuid);
					if (cache == null || !cache.BufferEnable)
					{
						goto end_IL_00d0;
					}
					LoginResponseEventArgs bufferLoginState = cache.BufferLoginState;
					if (((bufferLoginState != null) ? ((int)bufferLoginState.Status) : 0) != 1)
					{
						goto end_IL_00d0;
					}
					if (!token.IsCancellationRequested)
					{
						await SendWebPersonToServerAsync_(contextGuid, cache.BufferCancellationToken);
					}
					goto end_IL_0063;
					end_IL_00d0:;
				}
				catch (OperationCanceledException)
				{
					goto end_IL_0063;
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
					goto end_IL_0063;
				}
				continue;
				end_IL_0063:;
			}
			finally
			{
				await Task.Delay(300000);
			}
			int num;
			if (num == 2)
			{
				break;
			}
		}
	}

	private static async Task SendWebPersonToServerAsync_(string contextGuid, CancellationToken token)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"in {DateTime.Now} method SendWebPersonToServerAsync ");
		try
		{
			DateTime dateStart = DateTime.Now.AddMonths(-3);
			DateTime dateEnd = DateTime.Now.AddDays(7.0);
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
			string npUserName = timeAttendanceSettings?.WebServiceKey?.NPACCUserName;
			string npPassword = timeAttendanceSettings?.WebServiceKey?.NPACCPassword;
			if (string.IsNullOrEmpty(npUserName) || string.IsNullOrEmpty(npPassword))
			{
				return;
			}
			await Task.Delay(new Random().Next(60000), token);
			if (token.IsCancellationRequested)
			{
				return;
			}
			foreach (Person item in await GetAllBySentToServerStatusAsync(contextGuid, (ServerStatusForClient)0, token))
			{
				token.ThrowIfCancellationRequested();
				if (item.HashPassword != null && item.HashPassword.Length > 1 && !string.IsNullOrEmpty(item.UserName))
				{
					await SendWebPersonToServerAsync(sb, npUserName, npPassword, contextGuid, item, dateStart, dateEnd, token);
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
	}

	private async Task SetServerStatusAsync(ServerStatusForClient sent, CancellationToken token)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_ = 1;
		try
		{
			sentToServerStatus = sent;
			await SaveAsync(token);
			Person.OnServerStatusChanged?.Invoke();
			await Task.CompletedTask;
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

	private static async Task SendWebPersonToServerAsync(StringBuilder sb, string npUserName, string npPassword, string contextGuid, Person person, DateTime dateStart, DateTime dateEnd, CancellationToken token)
	{
		sb.AppendLine($"in {DateTime.Now} method SendWebPersonToServerAsync");
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			WebAttPerson val = await GetFullWebPersonInfoWithTaradodsAsync(sb, contextGuid, ((PersonBase)person).Id, dateStart, dateEnd, token);
			if (!token.IsCancellationRequested)
			{
				ReturnedSaveFuncInfo val2 = await WebAttPerson.PostToApiAsync(npUserName, npPassword, val, token);
				if (!val2.HasError)
				{
					await person.SetServerStatusAsync((ServerStatusForClient)1, token);
				}
				else
				{
					Log.Error("SendWebPersonToServerAsync\r\n error:" + val2.ErrorMessage);
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
	}

	private static async Task<WebAttPerson> GetFullWebPersonInfoWithTaradodsAsync(StringBuilder sb, string contextGuid, int personId, DateTime dateStart, DateTime dateEnd, CancellationToken token)
	{
		sb.AppendLine($"in {DateTime.Now} method GetFullWebPersonInfoWithTaradodsAsync");
		try
		{
			Person person = await GetByPersonIdAsync(contextGuid, personId, token);
			token.ThrowIfCancellationRequested();
			GharardadNew lastContract = await GharardadNew.GetAsync(contextGuid, personId, DateTime.Now, token);
			token.ThrowIfCancellationRequested();
			List<TaradodFull> listTaradodFull = new List<TaradodFull>();
			string text = LanguageAndCalendar.Instance.CaltureCalender.Today();
			DateTime pDate = Calendar.StartDayOfPersianMonth(text);
			DateTime pDate2 = Calendar.EndDayOfPersianMonth(text);
			TaradodFull clsTaradodFull2 = await TaradodFull.CreateAsync(contextGuid, pDate, pDate2, person, null, checkValidationVorodAndKHoroj: false, token);
			await clsTaradodFull2.FillTaradodFullPropertysAsync(token);
			if (clsTaradodFull2 != null)
			{
				await clsTaradodFull2.FillTaradodFullPropertysAsync(token);
				listTaradodFull.Add(clsTaradodFull2);
			}
			for (int i = -1; i > -3; i--)
			{
				string text2 = Calendar.AddMonth(LanguageAndCalendar.Instance.CaltureCalender.Today(), i);
				pDate = Calendar.StartDayOfPersianMonth(text2);
				pDate2 = Calendar.EndDayOfPersianMonth(text2);
				token.ThrowIfCancellationRequested();
				clsTaradodFull2 = await TaradodFull.CreateAsync(contextGuid, pDate, pDate2, person, null, checkValidationVorodAndKHoroj: false, token);
				if (clsTaradodFull2 != null)
				{
					await clsTaradodFull2.FillTaradodFullPropertysAsync(token);
					listTaradodFull.Add(clsTaradodFull2);
				}
			}
			token.ThrowIfCancellationRequested();
			Morakhasi morakhasis = await Morakhasi.GetAsync(contextGuid, lastContract?.ID ?? 0, token);
			List<Fish> fishes = await Fish.GetAllAsync(contextGuid, new List<int>(1) { personId }, dateStart, dateEnd, null, loadClsTaradod: true, token);
			return await MapToWebPersonAsync(contextGuid, person, new _003C_003Ez__ReadOnlySingleElementList<GharardadNew>(lastContract), listTaradodFull, morakhasis, fishes, token);
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

	private static async Task<WebAttPerson> MapToWebPersonAsync(string contextGuid, Person person, IEnumerable<GharardadNew> contracts, List<TaradodFull> taradodFulls, Morakhasi morakhasi, List<Fish> fishes, CancellationToken token, string tranName = "")
	{
		_ = 5;
		try
		{
			new WebAttPerson();
			token.ThrowIfCancellationRequested();
			if (person == null)
			{
				throw new NullReferenceException("person");
			}
			WebAttPerson webPerson = await PersonMapper.Instance.MapAsync(contextGuid, person);
			token.ThrowIfCancellationRequested();
			WebAttPerson val = webPerson;
			val.WebAttContracts = await ContractMapper.Instance.MapListAsync(contextGuid, contracts);
			token.ThrowIfCancellationRequested();
			val = webPerson;
			val.Morakhasi = await MorakhasiMapper.Instance.MapAsync(contextGuid, morakhasi);
			token.ThrowIfCancellationRequested();
			val = webPerson;
			val.TaradodFulls = await TaradodFullMapper.Instance.MapListAsync(contextGuid, taradodFulls, null, tranName);
			foreach (WebAttTaradodFull taraddod in webPerson.TaradodFulls)
			{
				IEnumerable<Fish> enumerable = fishes?.Where((Fish q) => q.DateStartM == taraddod.DateMin && q.DateEndM == taraddod.DateMax);
				if (enumerable != null && enumerable.Any())
				{
					WebAttTaradodFull val2 = taraddod;
					val2.Fishes = await FishMapper.Instance.MapListAsync(contextGuid, enumerable, null, "", token);
				}
			}
			await CalculateVacationsAsync(contextGuid, webPerson, token);
			return webPerson;
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

	public async Task SendToServerAsync(string contextGuid, CancellationToken token)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"in {DateTime.Now} method SendToServerAsync");
		try
		{
			TimeAttendanceSettings obj = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
			string npUserName = obj?.WebServiceKey?.NPACCUserName;
			string npPassword = obj?.WebServiceKey?.NPACCPassword;
			DateTime dateStart = DateTime.Now.AddMonths(-3);
			DateTime dateEnd = DateTime.Now.AddDays(7.0);
			await SendWebPersonToServerAsync(sb, npUserName, npPassword, contextGuid, this, dateStart, dateEnd, default(CancellationToken));
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

	public static async Task<Person> FactoryAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			Person person = new Person(contextGuid);
			Person person2 = person;
			((PersonBase)person2).CodeMoein = await NextCodeMoeinAsync(contextGuid, token);
			Person person3 = person;
			((PersonBase)person3).CodeTafsil = await NextCodeTafsilAsync(contextGuid, token);
			((PersonBase)person).Jensiat = 0;
			((PersonBase)person).GroupId = 1;
			((PersonBase)person).TitleId = 1;
			return person;
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

	private static async Task CalculateVacationsAsync(string contextGuid, WebAttPerson person, CancellationToken token)
	{
		try
		{
			if (person == null)
			{
				return;
			}
			List<WebAttTempVacations> tempVacationList = new List<WebAttTempVacations>();
			List<Morakhasidaily> list = await Morakhasidaily.GetAllAsync(contextGuid, person.Id, token);
			if (list != null && list.Any())
			{
				List<WebAttTempVacations> list2 = tempVacationList;
				list2.AddRange(await TempVacationMapper.Instance.MapListAsync(contextGuid, list, null, "", token));
			}
			WebAttPerson obj = person;
			if (((obj == null) ? null : obj.TaradodFulls?.Any()).GetValueOrDefault())
			{
				List<WebAttTaradodDet> list3 = new List<WebAttTaradodDet>();
				foreach (WebAttTaradodFull taradodFull in person.TaradodFulls)
				{
					if (!((taradodFull == null) ? null : taradodFull.ListTaradodDets?.Any()).GetValueOrDefault())
					{
						continue;
					}
					foreach (WebAttTaradodFullDet listTaradodDet in taradodFull.ListTaradodDets)
					{
						int num;
						if (listTaradodDet == null)
						{
							num = 1;
						}
						else
						{
							WebAttTaradodDet taradodDet = listTaradodDet.TaradodDet;
							num = ((!((taradodDet != null) ? taradodDet.VacationStart : null).HasValue) ? 1 : 0);
						}
						if (num == 0)
						{
							int num2;
							if (listTaradodDet == null)
							{
								num2 = 1;
							}
							else
							{
								WebAttTaradodDet taradodDet2 = listTaradodDet.TaradodDet;
								num2 = ((!((taradodDet2 != null) ? taradodDet2.VacationEnd : null).HasValue) ? 1 : 0);
							}
							if (num2 == 0)
							{
								list3.Add(listTaradodDet.TaradodDet);
							}
						}
					}
				}
				if (list3 != null && list3.Any())
				{
					IEnumerable<WebAttTempVacations> collection = ((IEnumerable<WebAttTaradodDet>)list3).Select((Func<WebAttTaradodDet, WebAttTempVacations>)((WebAttTaradodDet q) => new WebAttTempVacations
					{
						DateFrom = q.VacationStart.Value,
						DateTo = q.VacationEnd.Value,
						Guid = Guid.NewGuid(),
						PersonGuid = ((WebAttPersonCompact)person).Guid,
						Reason = q.VacationDesc,
						Type = (EnPersonLeaveType)0,
						Kind = (EnPersonLeaveKind)2
					}));
					tempVacationList.AddRange(collection);
				}
			}
			if (tempVacationList?.Any() ?? false)
			{
				person.TempVacationList = (from q in tempVacationList
					orderby q.DateFrom, q.DateTo
					select q).ToList();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public void ResetHardWareCodes()
	{
		_hardWareCodes = null;
	}
}
