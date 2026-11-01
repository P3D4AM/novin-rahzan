using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace TimeAttendanceDAL.Dal.FLRShift;

public class ShiftProgrammableDet : IHasContext
{
	private static SemaphoreSlim _semaphoreCalcPersonel = new SemaphoreSlim(1, 1);

	public string ContextGuid { get; set; }

	public int GharardadID { get; set; }

	public Guid Guid { get; set; }

	public short ShiftCodingID { get; set; }

	public string Desc { get; set; } = "";


	public DateTime DateStartM { get; set; } = DateTime.Now;


	public DateTime DateFinishM { get; set; } = DateTime.Now;


	public bool WorkOnHolidays { get; set; } = true;


	public ShiftDet.FriDayType FriDayType { get; set; }

	public ShiftProgrammableDet(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ShiftProgrammableDet> GetAsync(string contextGuid, int pGharardadID, short pShiftCodingID, CancellationToken token)
	{
		_ = 2;
		try
		{
			ShiftProgrammableDet res = new ShiftProgrammableDet();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_ShiftProgrammableDet_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("GharardadID", (object)pGharardadID);
					cmd.Parameters.AddWithValue("ShiftCodingID", (object)pShiftCodingID);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					if (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						res = LoadData(dr);
					}
					((DbConnection)(object)cn).Close();
					return res;
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

	public ShiftProgrammableDet()
	{
	}

	public static ShiftProgrammableDet LoadData(SqlDataReader dr)
	{
		try
		{
			ShiftProgrammableDet shiftProgrammableDet = new ShiftProgrammableDet();
			if (((DbDataReader)(object)dr)["GharardadID"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.GharardadID = (int)((DbDataReader)(object)dr)["GharardadID"];
			}
			if (((DbDataReader)(object)dr)["ShiftCodingID"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.ShiftCodingID = (short)((DbDataReader)(object)dr)["ShiftCodingID"];
			}
			if (((DbDataReader)(object)dr)["DateStartM"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.DateStartM = (DateTime)((DbDataReader)(object)dr)["DateStartM"];
			}
			if (((DbDataReader)(object)dr)["DateFinishM"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.DateFinishM = (DateTime)((DbDataReader)(object)dr)["DateFinishM"];
			}
			if (((DbDataReader)(object)dr)["Desc"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.Desc = ((DbDataReader)(object)dr)["Desc"].ToString();
			}
			if (((DbDataReader)(object)dr)["WorkOnHolidays"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.WorkOnHolidays = (bool)((DbDataReader)(object)dr)["WorkOnHolidays"];
			}
			if (((DbDataReader)(object)dr)["Guid"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["FriDayType"].ToString() != DBNull.Value.ToString())
			{
				shiftProgrammableDet.FriDayType = (ShiftDet.FriDayType)((DbDataReader)(object)dr)["FriDayType"];
			}
			return shiftProgrammableDet;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<bool> SaveListAsync(string contextGuid, List<ShiftProgrammableDet> programmableDets, int pPersonId, CancellationToken token)
	{
		_ = 3;
		try
		{
			DateTime minDate = DateTime.MaxValue;
			DateTime maxDate = DateTime.MinValue;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandText = "sp_ShiftProgrammableDet_Save";
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					foreach (ShiftProgrammableDet programmableDet in programmableDets)
					{
						if (minDate > programmableDet.DateStartM)
						{
							minDate = programmableDet.DateStartM;
						}
						if (maxDate < programmableDet.DateFinishM)
						{
							maxDate = programmableDet.DateFinishM;
						}
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						cmd.Parameters.AddWithValue("GharardadID", (object)programmableDet.GharardadID);
						cmd.Parameters.AddWithValue("ShiftCodingID", (object)programmableDet.ShiftCodingID);
						cmd.Parameters.AddWithValue("DateStartM", (object)programmableDet.DateStartM);
						cmd.Parameters.AddWithValue("DateFinishM", (object)programmableDet.DateFinishM);
						cmd.Parameters.AddWithValue("Description", (object)Text.FixString(programmableDet.Desc));
						cmd.Parameters.AddWithValue("WorkOnHolidays", (object)programmableDet.WorkOnHolidays);
						cmd.Parameters.AddWithValue("FriDayType", (object)programmableDet.FriDayType);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
					((DbCommand)(object)cmd).CommandText = " delete FROM [dbo].[LogonPersonel] where [IsVirtual] = 3 and \n";
					((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText + " PersonID = " + pPersonId;
					((DbCommand)(object)cmd).CommandText += " and (substring(convert(varchar, [DateTimeLogM], 120), 12, 8) = '23:59:59' or\n";
					((DbCommand)(object)cmd).CommandText += "substring(convert(varchar, [DateTimeLogM], 120), 12, 8) = '00:00:00' or\n";
					((DbCommand)(object)cmd).CommandText += "substring(convert(varchar, [DateTimeLogM], 120), 12, 8) = '00:00:01')\n";
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					((DbParameterCollection)(object)cmd.Parameters).Clear();
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbCommand)(object)cmd).CommandText = "update [dbo].[LogonPersonel] set Calced = 'False' where PersonID = " + pPersonId;
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					((DbParameterCollection)(object)cmd.Parameters).Clear();
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<bool> DeleteByGharardadAsync(string contextGuid, int pGharardadID, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_ShiftProgrammableDet_DeleteRowByGharardad", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("GharardadID", (object)pGharardadID);
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<ShiftProgrammableDet>> SelectRowByGharardadIDAsync(string contextGuid, int pGharardadID, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<ShiftProgrammableDet> ret = new List<ShiftProgrammableDet>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_ShiftProgrammableDet_SelectRowByGharardad]", cn);
				try
				{
					cmd.Parameters.AddWithValue("@GharardadID", (object)pGharardadID);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ShiftProgrammableDet item = LoadData(dr);
							ret.Add(item);
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
