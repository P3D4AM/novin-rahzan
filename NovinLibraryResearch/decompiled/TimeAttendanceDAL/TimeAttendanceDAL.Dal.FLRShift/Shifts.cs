using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.Dal.FLRShift;

[Serializable]
[DoNotPruneType]
public class Shifts : IHasContext
{
	[JsonIgnore]
	public string ContextGuid { get; set; }

	public int Id { get; private set; }

	public string Name { get; set; } = "";


	public string Code { get; set; } = "";


	public bool St { get; set; } = true;


	public bool StTatili { get; set; }

	public string Description { get; set; } = "";


	public PackageEnumType PackageType { get; set; }

	public short Periodic { get; set; }

	public Guid PackageGuid { get; set; }

	public List<ShiftDet> LstShiftDet { get; set; } = new List<ShiftDet>();


	internal Shifts(string contextGuid, short id, string name, string code, string description, PackageEnumType packageType, short periodic, Guid packageGuid, bool st = true, bool stTatili = false)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		Id = id;
		Name = name;
		Code = code;
		Description = description;
		PackageType = packageType;
		Periodic = periodic;
		PackageGuid = packageGuid;
		St = st;
		StTatili = stTatili;
	}

	public static async Task<Shifts> FactoryAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		Shifts ret = new Shifts(contextGuid);
		try
		{
			Shifts shifts = ret;
			shifts.Code = (await NextCodeAsync(contextGuid, token)).ToString();
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	[JsonConstructor]
	public Shifts(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<Shifts> GetAsync(string contextGuid, int id, CancellationToken token = default(CancellationToken))
	{
		Shifts ret = null;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Shifts_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)id);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(contextGuid, dr);
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

	private static Shifts LoadData(string contextGuid, SqlDataReader dr)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		Shifts shifts = new Shifts(contextGuid);
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				shifts.Id = (short)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				shifts.Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["Code"].ToString() != DBNull.Value.ToString())
			{
				shifts.Code = ((DbDataReader)(object)dr)["Code"].ToString();
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				shifts.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				shifts.St = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["STTatili"].ToString() != DBNull.Value.ToString())
			{
				shifts.StTatili = (bool)((DbDataReader)(object)dr)["STTatili"];
			}
			if (((DbDataReader)(object)dr)["PackageType"].ToString() != DBNull.Value.ToString())
			{
				shifts.PackageType = (PackageEnumType)((DbDataReader)(object)dr)["PackageType"];
			}
			if (((DbDataReader)(object)dr)["Periodic"].ToString() != DBNull.Value.ToString())
			{
				shifts.Periodic = (short)((DbDataReader)(object)dr)["Periodic"];
			}
			if (((DbDataReader)(object)dr)["PackageGuid"].ToString() != DBNull.Value.ToString())
			{
				shifts.PackageGuid = (Guid)((DbDataReader)(object)dr)["PackageGuid"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\Shifts.cs", 126);
			throw;
		}
		return shifts;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
		try
		{
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlTransaction tr = cn.BeginTransaction();
			try
			{
				bool result = await SaveAsync(tr, token);
				if (result && LstShiftDet != null)
				{
					foreach (ShiftDet item in LstShiftDet)
					{
						item.ShiftId = Id;
						await item.SaveAsync(tr, token);
					}
				}
				((DbTransaction)(object)tr).Commit();
				((DbConnection)(object)cn).Close();
				return result;
			}
			catch (Exception ex)
			{
				((DbTransaction)(object)tr).Rollback();
				((DbConnection)(object)cn).Close();
				Log.Error(ex, "");
				throw;
			}
		}
		finally
		{
			((IDisposable)cn)?.Dispose();
		}
	}

	private async Task<bool> SaveAsync(SqlTransaction tr, CancellationToken token)
	{
		_ = 4;
		try
		{
			SqlCommand cmd = tr.Connection.CreateCommand();
			try
			{
				cmd.Transaction = tr;
				((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
				cmd.Parameters.AddWithValue("Code", (object)Text.FixString(Code));
				cmd.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
				cmd.Parameters.AddWithValue("St", (object)St);
				cmd.Parameters.AddWithValue("STTatili", (object)StTatili);
				cmd.Parameters.AddWithValue("PackageType", (object)PackageType);
				cmd.Parameters.AddWithValue("Periodic", (object)Periodic);
				cmd.Parameters.AddWithValue("PackageGuid", (object)PackageGuid);
				if (Id <= 0)
				{
					((DbCommand)(object)cmd).CommandText = "sp_Shifts_Insert";
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						Id = Convert.ToInt16(obj.ToString());
					}
					await LogUsers.RegisterNewLogAsync(ContextGuid, "[افزودن شیفت جدید] : " + Name, "Shift.ShiftInsert", this, Id.ToString());
				}
				else
				{
					((DbCommand)(object)cmd).CommandText = "sp_Shifts_Update";
					cmd.Parameters.AddWithValue("Id", (object)Id);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					if (!St)
					{
						await LogUsers.RegisterNewLogAsync(ContextGuid, "[حذف شیفت]: " + Name, "Shift.ShiftDelete", this, Id.ToString());
					}
					else
					{
						await LogUsers.RegisterNewLogAsync(ContextGuid, "[ویرایش شیفت]: " + Name, "Shift.ShiftUpdate", this, Id.ToString());
					}
				}
				return true;
			}
			finally
			{
				((IDisposable)cmd)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task CreateNewShiftsAndCloseOldShiftsAsync(string contextGuid, DateTime date1, DateTime date2, string perfixDesc, CancellationToken token)
	{
		_ = 3;
		try
		{
			List<Shifts> lstshifts = await SelectAllAsync(contextGuid, st: true, (PackageEnumType)1, "", token);
			foreach (Shifts shifte in lstshifts)
			{
				Shifts shifts = shifte;
				shifts.LstShiftDet = await ShiftDet.GetAllAsync(contextGuid, shifte.Id, token);
				foreach (ShiftDet item in shifte.LstShiftDet)
				{
					item.DateFinishM = date1.Date.AddSeconds(-1.0);
				}
				await shifte.SaveAsync(token);
			}
			foreach (Shifts item2 in lstshifts)
			{
				foreach (ShiftDet item3 in item2.LstShiftDet)
				{
					item3.DateStartM = date1.Date;
					item3.DateFinishM = date2.Date.AddDays(1.0).AddSeconds(-1.0);
				}
				if (!item2.Name.Contains(perfixDesc))
				{
					item2.Name = item2.Name + " " + perfixDesc;
				}
				item2.Id = 0;
				await item2.SaveAsync(token);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<Shifts>> SelectAllAsync(string contextGuid, bool st, PackageEnumType package, string extItem = "", CancellationToken token = default(CancellationToken))
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_ = 2;
		try
		{
			List<Shifts> ret = new List<Shifts>();
			if (extItem != "")
			{
				Shifts item = new Shifts(contextGuid)
				{
					Name = extItem,
					PackageType = (PackageEnumType)1
				};
				ret.Add(item);
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_Shifts_SelectAll]", cn);
				try
				{
					cmd.Parameters.AddWithValue("St", (object)st);
					cmd.Parameters.AddWithValue("PackageType", (object)package);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(LoadData(contextGuid, dr));
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

	public static async Task<int> NextCodeAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select (isnull((select max([dbo].[STRING2INT](Code)) from [Shifts] WHERE St='TRUE'),0))", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					int result = int.Parse((await ((DbCommand)(object)cmd).ExecuteScalarAsync(token)).ToString()) + 1;
					((DbConnection)(object)cn).Close();
					return result;
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

	public static async Task<int> UniqeCodeAsync(string contextGuid, string code, CancellationToken token)
	{
		_ = 1;
		try
		{
			if (string.IsNullOrEmpty(code))
			{
				code = "0";
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select Id from [shifts]  where [St]='true' and Code='" + code + "'", cn);
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<bool> StTatiliShiftAsync_ByPersonNoDeviceId_deviceNoAyaBod(string contextGuid, int personNo, int deviceId, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_STTatilShift_Calc]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@PersonNO", (object)personNo);
					cmd.Parameters.AddWithValue("@DeviceId", (object)deviceId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					bool result = bool.Parse((await ((DbCommand)(object)cmd).ExecuteScalarAsync(token)).ToString());
					((DbConnection)(object)cn).Close();
					return result;
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

	public static async Task<ShiftDetView> GetShiftAsync(IEnumerable<ShiftDetView> shiftDetViews, DateTime ptime, CancellationToken token)
	{
		try
		{
			if (shiftDetViews == null)
			{
				return null;
			}
			bool flag = true;
			List<ShiftDetView> list = shiftDetViews.ToList();
			foreach (ShiftDetView item in list)
			{
				ShiftCodeing clsShiftCoding = item.ClsShiftCoding;
				if (flag && ptime < clsShiftCoding.VorodStart)
				{
					return item;
				}
				if (ptime >= clsShiftCoding.VorodStart && (ptime <= clsShiftCoding.KhorojEnd || ptime.TimeOfDay <= new TimeSpan(0, clsShiftCoding.MaxTimeCanExit, 0)))
				{
					return item;
				}
				flag = false;
			}
			if (list.Any() && list[list.Count() - 1].ClsShiftCoding.VorodStart.TimeOfDay > new TimeSpan(0, 0, 1))
			{
				return list[list.Count() - 1];
			}
			if (list.Any() && (int)list[list.Count() - 1].ClsShiftCoding.TypeShift == 2)
			{
				return list[list.Count() - 1];
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\Shifts.cs", 367);
			throw;
		}
		return null;
	}

	public static ShiftDetView GetShiftOfCouple(IEnumerable<ShiftDetView> shiftDetViews, CoupleTaradod couple)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		try
		{
			if (shiftDetViews == null)
			{
				return null;
			}
			foreach (ShiftDetView shiftDetView in shiftDetViews)
			{
				ShiftCodeing clsShiftCoding = shiftDetView.ClsShiftCoding;
				if ((int)clsShiftCoding.TypeShift == 2)
				{
					return shiftDetView;
				}
				if ((couple.Vtaradod.DateTimeLogM >= clsShiftCoding.VorodStart && couple.Vtaradod.DateTimeLogM < clsShiftCoding.KhorojEnd) || (couple.Ktaradod.DateTimeLogM > clsShiftCoding.VorodStart && couple.Vtaradod.DateTimeLogM <= clsShiftCoding.KhorojEnd))
				{
					return shiftDetView;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetShiftOfCouple", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\Shifts.cs", 388);
		}
		return null;
	}
}
