using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using Serilog;

namespace TimeAttendanceDAL.Dal.Taradod;

public class ATT_TaradodDet : IHasContext, IAttTaradodDet
{
	public string ContextGuid { get; set; }

	public int PersonId { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public int VacationTime { get; set; }

	public DateTime? VacationStart { get; set; }

	public DateTime? VacationEnd { get; set; }

	public string VacationDesc { get; set; } = "";


	public int EKTime { get; set; }

	public DateTime? EKStart { get; set; }

	public DateTime? EKEnd { get; set; }

	public string EKDesc { get; set; } = "";


	public int KKTime { get; set; }

	public DateTime? KKStart { get; set; }

	public DateTime? KKEnd { get; set; }

	public string KKDesc { get; set; } = "";


	public int MissionTime { get; set; }

	public DateTime? MissionStart { get; set; }

	public DateTime? MissionEnd { get; set; }

	public string MissionDesc { get; set; } = "";


	public int FridayTime { get; set; }

	public int TatilTime { get; set; }

	public DateTime? FridayStart { get; set; }

	public DateTime? FridayEnd { get; set; }

	public string FridayDesc { get; set; } = "";


	public int ShiftTime1 { get; set; }

	public int ShiftTime2 { get; set; }

	public int ShiftTime3 { get; set; }

	public DateTime? ShiftStart1 { get; set; }

	public DateTime? ShiftStart2 { get; set; }

	public DateTime? ShiftStart3 { get; set; }

	public DateTime? ShiftEnd1 { get; set; }

	public DateTime? ShiftEnd2 { get; set; }

	public DateTime? ShiftEnd3 { get; set; }

	public string ShiftDesc1 { get; set; } = "";


	public string ShiftDesc2 { get; set; } = "";


	public string ShiftDesc3 { get; set; } = "";


	public int AbsentTime { get; set; }

	public DateTime? AbsendStart { get; set; }

	public DateTime? AbsentEnd { get; set; }

	public string AbsentDesc { get; set; } = "";


	public int NightTime { get; set; }

	public DateTime? NightStart { get; set; }

	public DateTime? NightEnd { get; set; }

	public string NightDesc { get; set; } = "";


	public ATT_TaradodDet(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ATT_TaradodDet> GetAsync(string contextGuid, int personId, DateTime dateM, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return await LoadDataAsync(personId, dateM, contextGuid, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public ATT_TaradodDet(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	public static async Task<ATT_TaradodDet> LoadDataAsync(int pPersonId, DateTime pDateM, string contextGuid, CancellationToken token)
	{
		_ = 2;
		try
		{
			ATT_TaradodDet ret = new ATT_TaradodDet(contextGuid)
			{
				PersonId = pPersonId,
				DateM = pDateM
			};
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_ATT_TaradodDet_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("@PersonID", (object)pPersonId);
					cmd.Parameters.AddWithValue("@DateM", (object)pDateM);
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

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["PersonID"].ToString() != "")
			{
				PersonId = (int)((DbDataReader)(object)dr)["PersonID"];
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != "")
			{
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["VacationTime"].ToString() != "")
			{
				VacationTime = (int)((DbDataReader)(object)dr)["VacationTime"];
			}
			if (((DbDataReader)(object)dr)["VacationStart"].ToString() != "")
			{
				VacationStart = (DateTime)((DbDataReader)(object)dr)["VacationStart"];
			}
			if (((DbDataReader)(object)dr)["VacationEnd"].ToString() != "")
			{
				VacationEnd = (DateTime)((DbDataReader)(object)dr)["VacationEnd"];
			}
			if (((DbDataReader)(object)dr)["VacationDesc"].ToString() != "")
			{
				VacationDesc = ((DbDataReader)(object)dr)["VacationDesc"].ToString();
			}
			if (((DbDataReader)(object)dr)["EKTime"].ToString() != "")
			{
				EKTime = (int)((DbDataReader)(object)dr)["EKTime"];
			}
			if (((DbDataReader)(object)dr)["EKStart"].ToString() != "")
			{
				EKStart = (DateTime)((DbDataReader)(object)dr)["EKStart"];
			}
			if (((DbDataReader)(object)dr)["EKEnd"].ToString() != "")
			{
				EKEnd = (DateTime)((DbDataReader)(object)dr)["EKEnd"];
			}
			if (((DbDataReader)(object)dr)["EKDesc"].ToString() != "")
			{
				EKDesc = ((DbDataReader)(object)dr)["EKDesc"].ToString();
			}
			if (((DbDataReader)(object)dr)["KKTime"].ToString() != "")
			{
				KKTime = (int)((DbDataReader)(object)dr)["KKTime"];
			}
			if (((DbDataReader)(object)dr)["KKStart"].ToString() != "")
			{
				KKStart = (DateTime)((DbDataReader)(object)dr)["KKStart"];
			}
			if (((DbDataReader)(object)dr)["KKEnd"].ToString() != "")
			{
				KKEnd = (DateTime)((DbDataReader)(object)dr)["KKEnd"];
			}
			if (((DbDataReader)(object)dr)["KKDesc"].ToString() != "")
			{
				KKDesc = ((DbDataReader)(object)dr)["KKDesc"].ToString();
			}
			if (((DbDataReader)(object)dr)["MissionTime"].ToString() != "")
			{
				MissionTime = (int)((DbDataReader)(object)dr)["MissionTime"];
			}
			if (((DbDataReader)(object)dr)["MissionStart"].ToString() != "")
			{
				MissionStart = (DateTime)((DbDataReader)(object)dr)["MissionStart"];
			}
			if (((DbDataReader)(object)dr)["MissionEnd"].ToString() != "")
			{
				MissionEnd = (DateTime)((DbDataReader)(object)dr)["MissionEnd"];
			}
			if (((DbDataReader)(object)dr)["MissionDesc"].ToString() != "")
			{
				MissionDesc = ((DbDataReader)(object)dr)["MissionDesc"].ToString();
			}
			if (((DbDataReader)(object)dr)["FridayTime"].ToString() != "")
			{
				FridayTime = (int)((DbDataReader)(object)dr)["FridayTime"];
			}
			if (((DbDataReader)(object)dr)["FridayStart"].ToString() != "")
			{
				FridayStart = (DateTime)((DbDataReader)(object)dr)["FridayStart"];
			}
			if (((DbDataReader)(object)dr)["FridayEnd"].ToString() != "")
			{
				FridayEnd = (DateTime)((DbDataReader)(object)dr)["FridayEnd"];
			}
			if (((DbDataReader)(object)dr)["FridayDesc"].ToString() != "")
			{
				FridayDesc = ((DbDataReader)(object)dr)["FridayDesc"].ToString();
			}
			if (((DbDataReader)(object)dr)["ShiftTime1"].ToString() != "")
			{
				ShiftTime1 = (int)((DbDataReader)(object)dr)["ShiftTime1"];
			}
			if (((DbDataReader)(object)dr)["ShiftStart1"].ToString() != "")
			{
				ShiftStart1 = (DateTime)((DbDataReader)(object)dr)["ShiftStart1"];
			}
			if (((DbDataReader)(object)dr)["ShiftEnd1"].ToString() != "")
			{
				ShiftEnd1 = (DateTime)((DbDataReader)(object)dr)["ShiftEnd1"];
			}
			if (((DbDataReader)(object)dr)["ShiftDesc1"].ToString() != "")
			{
				ShiftDesc1 = ((DbDataReader)(object)dr)["ShiftDesc1"].ToString();
			}
			if (((DbDataReader)(object)dr)["ShiftTime2"].ToString() != "")
			{
				ShiftTime2 = (int)((DbDataReader)(object)dr)["ShiftTime2"];
			}
			if (((DbDataReader)(object)dr)["ShiftStart2"].ToString() != "")
			{
				ShiftStart2 = (DateTime)((DbDataReader)(object)dr)["ShiftStart2"];
			}
			if (((DbDataReader)(object)dr)["ShiftEnd2"].ToString() != "")
			{
				ShiftEnd2 = (DateTime)((DbDataReader)(object)dr)["ShiftEnd2"];
			}
			if (((DbDataReader)(object)dr)["ShiftDesc2"].ToString() != "")
			{
				ShiftDesc2 = ((DbDataReader)(object)dr)["ShiftDesc2"].ToString();
			}
			if (((DbDataReader)(object)dr)["ShiftTime3"].ToString() != "")
			{
				ShiftTime3 = (int)((DbDataReader)(object)dr)["ShiftTime3"];
			}
			if (((DbDataReader)(object)dr)["ShiftStart3"].ToString() != "")
			{
				ShiftStart3 = (DateTime)((DbDataReader)(object)dr)["ShiftStart3"];
			}
			if (((DbDataReader)(object)dr)["ShiftEnd3"].ToString() != "")
			{
				ShiftEnd3 = (DateTime)((DbDataReader)(object)dr)["ShiftEnd3"];
			}
			if (((DbDataReader)(object)dr)["ShiftDesc3"].ToString() != "")
			{
				ShiftDesc3 = ((DbDataReader)(object)dr)["ShiftDesc3"].ToString();
			}
			if (((DbDataReader)(object)dr)["AbsentTime"].ToString() != "")
			{
				AbsentTime = (int)((DbDataReader)(object)dr)["AbsentTime"];
			}
			if (((DbDataReader)(object)dr)["AbsendStart"].ToString() != "")
			{
				AbsendStart = (DateTime)((DbDataReader)(object)dr)["AbsendStart"];
			}
			if (((DbDataReader)(object)dr)["AbsentEnd"].ToString() != "")
			{
				AbsentEnd = (DateTime)((DbDataReader)(object)dr)["AbsentEnd"];
			}
			if (((DbDataReader)(object)dr)["AbsentDesc"].ToString() != "")
			{
				AbsentDesc = ((DbDataReader)(object)dr)["AbsentDesc"].ToString();
			}
			if (((DbDataReader)(object)dr)["NightTime"].ToString() != "")
			{
				NightTime = (int)((DbDataReader)(object)dr)["NightTime"];
			}
			if (((DbDataReader)(object)dr)["NightStart"].ToString() != "")
			{
				NightStart = (DateTime)((DbDataReader)(object)dr)["NightStart"];
			}
			if (((DbDataReader)(object)dr)["NightEnd"].ToString() != "")
			{
				NightEnd = (DateTime)((DbDataReader)(object)dr)["NightEnd"];
			}
			if (((DbDataReader)(object)dr)["NightDesc"].ToString() != "")
			{
				NightDesc = ((DbDataReader)(object)dr)["NightDesc"].ToString();
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\ATT_TaradodDet.cs", 178);
			return false;
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand cmd = new SqlCommand("sp_ATT_TaradodDet_Save", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@PersonID", (object)PersonId);
			cmd.Parameters.AddWithValue("@DateM", (object)DateM);
			cmd.Parameters.AddWithValue("@VacationTime", (object)VacationTime);
			cmd.Parameters.AddWithValue("@VacationStart", (object)VacationStart);
			cmd.Parameters.AddWithValue("@VacationEnd", (object)VacationEnd);
			cmd.Parameters.AddWithValue("@VacationDesc", (object)VacationDesc);
			cmd.Parameters.AddWithValue("@EKTime", (object)EKTime);
			cmd.Parameters.AddWithValue("@EKStart", (object)EKStart);
			cmd.Parameters.AddWithValue("@EKEnd", (object)EKEnd);
			cmd.Parameters.AddWithValue("@EKDesc", (object)EKDesc);
			cmd.Parameters.AddWithValue("@KKTime", (object)KKTime);
			cmd.Parameters.AddWithValue("@KKStart", (object)KKStart);
			cmd.Parameters.AddWithValue("@KKEnd", (object)KKEnd);
			cmd.Parameters.AddWithValue("@KKDesc", (object)KKDesc);
			cmd.Parameters.AddWithValue("@MissionTime", (object)MissionTime);
			cmd.Parameters.AddWithValue("@MissionStart", (object)MissionStart);
			cmd.Parameters.AddWithValue("@MissionEnd", (object)MissionEnd);
			cmd.Parameters.AddWithValue("@MissionDesc", (object)MissionDesc);
			cmd.Parameters.AddWithValue("@FridayTime", (object)FridayTime);
			cmd.Parameters.AddWithValue("@FridayStart", (object)FridayStart);
			cmd.Parameters.AddWithValue("@FridayEnd", (object)FridayEnd);
			cmd.Parameters.AddWithValue("@FridayDesc", (object)FridayDesc);
			cmd.Parameters.AddWithValue("@ShiftTime1", (object)ShiftTime1);
			cmd.Parameters.AddWithValue("@ShiftTime2", (object)ShiftTime2);
			cmd.Parameters.AddWithValue("@ShiftTime3", (object)ShiftTime3);
			cmd.Parameters.AddWithValue("@ShiftStart1", (object)ShiftStart1);
			cmd.Parameters.AddWithValue("@ShiftStart2", (object)ShiftStart2);
			cmd.Parameters.AddWithValue("@ShiftStart3", (object)ShiftStart3);
			cmd.Parameters.AddWithValue("@ShiftEnd1", (object)ShiftEnd1);
			cmd.Parameters.AddWithValue("@ShiftEnd2", (object)ShiftEnd2);
			cmd.Parameters.AddWithValue("@ShiftEnd3", (object)ShiftEnd3);
			cmd.Parameters.AddWithValue("@ShiftDesc1", (object)ShiftDesc1);
			cmd.Parameters.AddWithValue("@ShiftDesc2", (object)ShiftDesc2);
			cmd.Parameters.AddWithValue("@ShiftDesc3", (object)ShiftDesc3);
			cmd.Parameters.AddWithValue("@AbsentTime", (object)AbsentTime);
			cmd.Parameters.AddWithValue("@AbsendStart", (object)AbsendStart);
			cmd.Parameters.AddWithValue("@AbsentEnd", (object)AbsentEnd);
			cmd.Parameters.AddWithValue("@AbsentDesc", (object)AbsentDesc);
			cmd.Parameters.AddWithValue("@NightTime", (object)NightTime);
			cmd.Parameters.AddWithValue("@NightStart", (object)NightStart);
			cmd.Parameters.AddWithValue("NightEnd", (object)NightEnd);
			cmd.Parameters.AddWithValue("@NightDesc", (object)NightDesc);
			await ((DbConnection)(object)cn).OpenAsync(token);
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<ATT_TaradodDet>> GetAllAsync(string contextGuid, int personId, DateTime dateM1, DateTime dateM2, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<ATT_TaradodDet> ret = new List<ATT_TaradodDet>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_ATT_TaradodDet_SelectALL]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@PersonID", (object)personId);
					cmd.Parameters.AddWithValue("@DateM1", (object)dateM1.Date);
					cmd.Parameters.AddWithValue("@DateM2", (object)dateM2.Date);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new ATT_TaradodDet(contextGuid, dr));
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

	public static async Task<List<ATT_TaradodDet>> GetAllAsync(string contextGuid, DateTime dateM1, DateTime dateM2, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<ATT_TaradodDet> ret = new List<ATT_TaradodDet>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_ATT_TaradodDet_GetAllBetweenDate]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@DateM1", (object)dateM1.Date);
					cmd.Parameters.AddWithValue("@DateM2", (object)dateM2.Date);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new ATT_TaradodDet(contextGuid, dr));
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
}
