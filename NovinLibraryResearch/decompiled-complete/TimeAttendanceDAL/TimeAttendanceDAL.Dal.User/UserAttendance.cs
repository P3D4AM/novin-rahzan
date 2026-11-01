using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendanceDAL.Dal.User;

[Serializable]
[DoNotPruneType]
public class UserAttendance : UserPermission, IHasContext
{
	[JsonIgnore]
	public new string ContextGuid { get; set; }

	public static UserAttendance SelectedUser { get; set; }

	public short ID { get; private set; }

	public string Name { get; set; } = "";


	public int Code { get; set; }

	public bool ST { get; set; } = true;


	public string OldPassword { get; set; } = "";


	public string Description { get; set; } = "";


	public string UserGUID { get; set; } = "";


	public Guid Guid
	{
		get
		{
			if (string.IsNullOrEmpty(UserGUID))
			{
				return Guid.Empty;
			}
			Guid.TryParse(UserGUID, out var result);
			return result;
		}
		set
		{
			UserGUID = value.ToString();
		}
	}

	public UserAttSetting UserSetting { get; set; } = new UserAttSetting();


	public UserAttendance(string contextGuid)
		: base(contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<UserAttendance> GetAsync(string contextGuid, short pId, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			UserAttendance ret = new UserAttendance(contextGuid)
			{
				ContextGuid = contextGuid
			};
			SqlConnection cn = new SqlConnection(ClsConnection.connectionString);
			try
			{
				SqlCommand cmd = new SqlCommand("sp_UserAttendance_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)pId);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.LoadData(dr);
						}
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<UserAttendance> GetAsync(string contextGuid, int pCode, CancellationToken token)
	{
		_ = 2;
		try
		{
			UserAttendance ret = new UserAttendance(contextGuid)
			{
				ContextGuid = contextGuid
			};
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ret.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_UserAttendance_SelectRowCode", cn);
				try
				{
					cmd.Parameters.AddWithValue("Code", (object)pCode);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.LoadData(dr);
						}
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public UserAttendance(string contextGuid, SqlDataReader dr)
		: base(contextGuid)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private void LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				ID = (short)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Code"].ToString() != DBNull.Value.ToString())
			{
				Code = (int)((DbDataReader)(object)dr)["Code"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["Password"].ToString() != DBNull.Value.ToString())
			{
				OldPassword = ((DbDataReader)(object)dr)["Password"].ToString();
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				ST = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["UserGUID"].ToString() != DBNull.Value.ToString())
			{
				UserGUID = ((DbDataReader)(object)dr)["UserGUID"].ToString();
			}
			if (((DbDataReader)(object)dr)["UserPermissionJson"].ToString() != DBNull.Value.ToString())
			{
				_userPermissionJson = (byte[])((DbDataReader)(object)dr)["UserPermissionJson"];
			}
			if (((DbDataReader)(object)dr)["UserSetting"].ToString() != DBNull.Value.ToString())
			{
				UserSetting = JsonConvert.DeserializeObject<UserAttSetting>(((DbDataReader)(object)dr)["UserSetting"].ToString());
			}
			ConstructUserPermission(OldPassword, ID);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 130);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(string confirmPass, CancellationToken token)
	{
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			if (ID == 0 && Code != 0 && (await GetAsync(ContextGuid, Code, token)).ID > 0)
			{
				result.AddReturnedValue((ReturnedState)2, "کد تکراری است.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 148);
			}
			if (Code == 0)
			{
				result.AddReturnedValue((ReturnedState)2, "وارد کردن کد کاربر الزامی است.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 152);
			}
			if (Name == "")
			{
				result.AddReturnedValue((ReturnedState)2, "لطفا نام كاربري را وارد كنيد.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 154);
			}
			if (!CheckPassWord(confirmPass))
			{
				result.AddReturnedValue((ReturnedState)2, "رمز عبور و تکرار آن مطابقت ندارد لطفا دوباره هر دو کادر را وارد کنید.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 156);
			}
			return result;
		}
		catch (Exception ex)
		{
			result.AddReturnedValue((ReturnedState)2, ex.Message, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 161);
		}
		return result;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Code", (object)Code);
					cmd.Parameters.AddWithValue("Name", (object)(Text.FixString(Name) ?? ""));
					cmd.Parameters.AddWithValue("Description", (object)(Text.FixString(Description) ?? ""));
					cmd.Parameters.AddWithValue("St", (object)ST);
					ConvertToJsonAndEncrypt();
					cmd.Parameters.AddWithValue("UserPermissionJson", (object)_userPermissionJson);
					OldPassword = " ";
					cmd.Parameters.AddWithValue("Password", (object)(OldPassword ?? ""));
					Guid guid = Guid.NewGuid();
					if (UserGUID != "")
					{
						guid = Guid.Parse(UserGUID);
					}
					cmd.Parameters.AddWithValue("UserGUID", (object)guid);
					cmd.Parameters.AddWithValue("UserSetting", (object)JsonConvert.SerializeObject((object)UserSetting));
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (ID <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_UserAttendance_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							ID = Convert.ToInt16(obj.ToString());
						}
						PermissionUserID = ID;
						((DbConnection)(object)cn).Close();
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_UserAttendance_Update";
						cmd.Parameters.AddWithValue("Id", (object)ID);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<UserAttendance>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		List<UserAttendance> ret = new List<UserAttendance>();
		try
		{
			_ = 2;
			try
			{
				SqlConnection cn = new SqlConnection(ClsConnection.connectionString);
				try
				{
					SqlCommand cmd = new SqlCommand("[sp_UserAttendance_SelectAll]", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)cn).OpenAsync(token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
						try
						{
							while (await ((DbDataReader)(object)dr).ReadAsync(token))
							{
								ret.Add(new UserAttendance(contextGuid, dr));
							}
							((DbConnection)(object)cn).Close();
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
				WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 230);
				throw;
			}
		}
		finally
		{
			if (!(ret?.Any() ?? false))
			{
				await AddTestUserAsync(contextGuid, ret);
			}
		}
		return ret;
	}

	private static async Task AddTestUserAsync(string contextGuid, List<UserAttendance> ret, CancellationToken token = default(CancellationToken))
	{
		try
		{
			UserAttendance _cls = new UserAttendance(contextGuid)
			{
				Name = "مدیر"
			};
			_cls.SetPassword("");
			_cls.Code = 1;
			_cls.ST = true;
			await _cls.SaveAsync(token);
			ret.Add(_cls);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<bool> CheckInfoAsync(string contextGuid, short id, string pass, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return (await GetAsync(contextGuid, id, token)).CheckPassWord(pass);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 267);
		}
		return false;
	}

	public bool CheckPassWord(string pass)
	{
		return CheckPassWord(pass, ID);
	}

	public static async Task<string> NextCodeAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select max(Code) from [UserAttendance]", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					int num = int.Parse((await ((DbCommand)(object)cmd).ExecuteScalarAsync(token)).ToString()) + 1;
					((DbConnection)(object)cn).Close();
					return num.ToString();
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "NextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 292);
			throw;
		}
	}

	public string GetPassword()
	{
		return GetPassword(ID);
	}

	public void SetPassword(string pass)
	{
		SetPassword(pass, isForce: true);
	}

	public static async Task<ReturnedSaveFuncInfo> MoveSecurityToPermisiionAsync(string contextGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			string text = await SettingAttendance.GetValueAsync(contextGuid, "MoveSecurityToPermisiion", 0, token);
			if (text.Trim() == "" || text != "MoveSecurityToPermisiion is ok")
			{
				foreach (UserAttendance item in await GetAllAsync(contextGuid, token))
				{
					await item.SaveAsync(token);
				}
				await SettingAttendance.SetValueAsync(contextGuid, "MoveSecurityToPermisiion", "MoveSecurityToPermisiion is ok", 0, token);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue((ReturnedState)2, ex.Message, "", "MoveSecurityToPermisiionAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 323);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MoveSecurityToPermisiionAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 324);
		}
		return ret;
	}

	public static short GetUserIdPassEmpty(List<UserAttendance> lstAttendances)
	{
		short result = 0;
		if (lstAttendances == null)
		{
			throw new NullReferenceException("lstAttendances");
		}
		try
		{
			foreach (UserAttendance lstAttendance in lstAttendances)
			{
				if (lstAttendance.CheckPassWord(""))
				{
					result = lstAttendance.ID;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetUserIdPassEmpty", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendance.cs", 342);
		}
		return result;
	}
}
