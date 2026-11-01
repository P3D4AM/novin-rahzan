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
using PacketParser.Entities.Interfaces.Attendance;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.Taradod;

[Serializable]
[DoNotPruneType]
public class Morakhasidaily : IHasContext, IAttPersonLeave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public static SemaphoreSlim semafore_SaveAsync = new SemaphoreSlim(1, 1);

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public int Id { get; private set; }

	public string DateCalture => Calendar.MiladiToCalture(DateFrom);

	public int PersonId { get; set; }

	public Guid PersonGuid { get; set; }

	public string Desc { get; set; } = "";


	public short TypeId { get; set; }

	public string PersonFullName { get; private set; }

	public long PersonCodeTafsil { get; private set; }

	public string TypeMorakhasiName { get; private set; }

	public string Dow => LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(DateFrom);

	public bool HasHoghogh { get; internal set; }

	public bool HasBimeh { get; internal set; }

	public bool HasMaliyat { get; internal set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public DateTime RequestDate { get; set; } = DateTime.Now;


	public string Reason { get; set; } = "";


	public DateTime DateFrom { get; set; } = DateTime.Today;


	public DateTime DateTo { get; set; } = DateTime.Today;


	public EnPersonLeaveType Type { get; set; } = (EnPersonLeaveType)1;


	public EnPersonLeaveStatus LeaveStatus { get; set; }

	public EnPersonLeaveKind Kind { get; set; } = (EnPersonLeaveKind)2;


	public string ManagerMessage { get; set; } = "";


	public DateTime? ManagerDate { get; set; } = DateTime.Today;


	public EnPersonLeaveQuiddity Quiddity { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; }

	public Guid Guid { get; set; }

	public Morakhasidaily(string contextGuid)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
	}

	public static async Task<Morakhasidaily> GetAsync(string contextGuid, DateTime dateM, int personId, CancellationToken token)
	{
		_ = 2;
		try
		{
			Morakhasidaily ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_MorakhasiDaily_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("@PersonID", (object)personId);
					cmd.Parameters.AddWithValue("@DateM", (object)dateM.Date);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
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

	public static async Task<Morakhasidaily> GetAsync(string contextGuid, int morakhasiId, CancellationToken token)
	{
		_ = 2;
		try
		{
			Morakhasidaily ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_MorakhasiDaily_SelectRowID", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)morakhasiId);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
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

	private static Morakhasidaily LoadData(string contextGuid, SqlDataReader dr)
	{
		try
		{
			Morakhasidaily morakhasidaily = new Morakhasidaily(contextGuid);
			if (((DbDataReader)(object)dr)["Id"] != DBNull.Value)
			{
				morakhasidaily.Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["DateM"] != DBNull.Value)
			{
				DateTime dateFrom = (morakhasidaily.DateTo = (DateTime)((DbDataReader)(object)dr)["DateM"]);
				morakhasidaily.DateFrom = dateFrom;
			}
			if (((DbDataReader)(object)dr)["PersonID"] != DBNull.Value)
			{
				morakhasidaily.PersonId = (int)((DbDataReader)(object)dr)["PersonID"];
			}
			if (((DbDataReader)(object)dr)["Desc"] != DBNull.Value)
			{
				morakhasidaily.Desc = ((DbDataReader)(object)dr)["Desc"].ToString();
			}
			if (((DbDataReader)(object)dr)["TypeID"] != DBNull.Value)
			{
				morakhasidaily.TypeId = (short)((DbDataReader)(object)dr)["TypeID"];
				morakhasidaily.Kind = (EnPersonLeaveKind)morakhasidaily.TypeId;
			}
			if (((DbDataReader)(object)dr)["TypeMorakhasiName"] != DBNull.Value)
			{
				morakhasidaily.TypeMorakhasiName = ((DbDataReader)(object)dr)["TypeMorakhasiName"].ToString();
			}
			if (((DbDataReader)(object)dr)["PersonCodeTafsil"] != DBNull.Value)
			{
				morakhasidaily.PersonCodeTafsil = (long)((DbDataReader)(object)dr)["PersonCodeTafsil"];
			}
			if (((DbDataReader)(object)dr)["PersonFullName"] != DBNull.Value)
			{
				morakhasidaily.PersonFullName = ((DbDataReader)(object)dr)["PersonFullName"].ToString();
			}
			if (((DbDataReader)(object)dr)["HasHoghogh"] != DBNull.Value)
			{
				morakhasidaily.HasHoghogh = (bool)((DbDataReader)(object)dr)["HasHoghogh"];
			}
			if (((DbDataReader)(object)dr)["HasMaliyat"] != DBNull.Value)
			{
				morakhasidaily.HasMaliyat = (bool)((DbDataReader)(object)dr)["HasMaliyat"];
			}
			if (((DbDataReader)(object)dr)["HasBimeh"] != DBNull.Value)
			{
				morakhasidaily.HasBimeh = (bool)((DbDataReader)(object)dr)["HasBimeh"];
			}
			if (((DbDataReader)(object)dr)["PersonGuid"] != DBNull.Value)
			{
				morakhasidaily.PersonGuid = (Guid)((DbDataReader)(object)dr)["PersonGuid"];
			}
			morakhasidaily.Guid = Guid.NewGuid();
			morakhasidaily.Type = (EnPersonLeaveType)1;
			return morakhasidaily;
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
		bool semaphoreHasBeenTaken = false;
		try
		{
			await semafore_SaveAsync.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					if (DateFrom.TimeOfDay.TotalSeconds > 0.0)
					{
						await Task.Delay(100);
					}
					cmd.Parameters.AddWithValue("DateM", (object)DateFrom.Date);
					cmd.Parameters.AddWithValue("PersonID", (object)PersonId);
					cmd.Parameters.AddWithValue("Desc", (object)Text.FixString(Desc));
					cmd.Parameters.AddWithValue("TypeID", (object)TypeId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					((DbCommand)(object)cmd).CommandText = "dbo.sp_MorakhasiDaily_Save";
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj == null)
					{
						TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)3, "[ثبت مرخصی با خطا مواجه شد]", "", 3);
						return false;
					}
					Id = Convert.ToInt16(obj.ToString());
					LogUsers.RegisterNewLogAsync(ContextGuid, "[افزودن مرخصی روزانه] " + PersonFullName + " [در تاریخ] " + Calendar.MiladiToCalture(DateFrom) + " [و نوع مرخصی] : " + TypeMorakhasiName, "Vacation.VacationInsert", this, Id.ToString());
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
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				semafore_SaveAsync.Release();
			}
		}
	}

	public static async Task<bool> DeleteAsync(string contextGuid, Morakhasidaily pMorakhasidaily, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_MorakhasiDaily_Delete]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)pMorakhasidaily.Id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					LogUsers.RegisterNewLogAsync(contextGuid, "[حذف مرخصی] : " + pMorakhasidaily.PersonFullName + " [در تاریخ] : " + Calendar.MiladiToCalture(pMorakhasidaily.DateFrom) + " [و نوع مرخصی] : " + pMorakhasidaily.TypeMorakhasiName, "Vacation.VacationUpdate", pMorakhasidaily, pMorakhasidaily.Id.ToString());
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

	public static async Task<Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>>> CalcLeaveBalanceAsync(string contextGuid, GharardadNew gharardad, CancellationToken token = default(CancellationToken))
	{
		GharardadNew xclsGharardad = gharardad;
		double totalVacation2 = 0.0;
		int vacationTimeSaati = 0;
		List<MorakhasiSaati> list = new List<MorakhasiSaati>();
		int xdayvacation = 0;
		try
		{
			if (xclsGharardad != null)
			{
				List<Morakhasidaily> list2 = await GetAllAsync(contextGuid, gharardad.PersonId, token);
				if (list2 != null && list2.Any())
				{
					list2 = list2.Where((Morakhasidaily p) => (int)p.Kind == 2 && p.DateFrom >= xclsGharardad.DateStartM && p.DateFrom <= xclsGharardad.DateFinishM).ToList();
					xdayvacation = list2.Count();
				}
				list = await MorakhasiSaati.GetSpecificDataAsync(contextGuid, xclsGharardad.PersonId, xclsGharardad.DateStartM, xclsGharardad.DateFinishM, (TypeTaradodEnumType)5, token);
				if (list != null)
				{
					vacationTimeSaati = list.Select((MorakhasiSaati e1) => e1.Duration).Sum();
					totalVacation2 = xclsGharardad.LeaveSaved + xclsGharardad.TotalLeaveDaysInGharardad;
					totalVacation2 = ((xclsGharardad.LeaveMinutes <= 0) ? (totalVacation2 - ((double)xdayvacation + (double)vacationTimeSaati / (double)xclsGharardad.LeaveMinutes)) : (totalVacation2 - ((double)xdayvacation + (double)vacationTimeSaati / (double)xclsGharardad.LeaveMinutes)));
				}
			}
			return new Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>>(xdayvacation, vacationTimeSaati, totalVacation2, xclsGharardad, list);
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

	public static async Task<List<Morakhasidaily>> GetAllAsync(string contextGuid, int? personId = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<Morakhasidaily> ret = new List<Morakhasidaily>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_MorakhasiDaily_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					personId.GetValueOrDefault();
					if (!personId.HasValue)
					{
						int value = 0;
						personId = value;
					}
					cmd.Parameters.AddWithValue("@PersonId", (object)personId.Value);
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

	public static async Task<List<Morakhasidaily>> GetSpecificDataAsync(string contextGuid, List<int> personIdList, DateTime pDate1, DateTime pDate2, string pDesc, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<Morakhasidaily> ret = new List<Morakhasidaily>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_MorakhasiDaily_Show]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@personIDs", (object)string.Join(",", personIdList));
					cmd.Parameters.AddWithValue("@Date1", (object)pDate1);
					cmd.Parameters.AddWithValue("@Date2", (object)pDate2);
					cmd.Parameters.AddWithValue("@Desc", (object)("%" + pDesc + "%"));
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
}
