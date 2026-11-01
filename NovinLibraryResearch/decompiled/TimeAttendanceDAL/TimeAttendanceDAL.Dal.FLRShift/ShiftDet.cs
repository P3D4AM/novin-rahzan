using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.FLRShift;

[Serializable]
[DoNotPruneType]
public class ShiftDet : IHasContext
{
	public enum FriDayType : short
	{
		EkhtiyariAndKarkard,
		EkhtiyariAndAdamKarkard,
		Elzami
	}

	private ShiftCodeing _clsShiftCodeing;

	private short _shiftCodingId;

	private CancellationTokenSource _loadDataShiftToken;

	public int ShiftId { get; set; }

	public short ShiftCodingId => _shiftCodingId;

	public ShiftCodeing ShiftCodeing => _clsShiftCodeing;

	public bool Sat { get; set; } = true;


	public bool Sun { get; set; } = true;


	public bool Mon { get; set; } = true;


	public bool Tue { get; set; } = true;


	public bool Wed { get; set; } = true;


	public bool Tur { get; set; }

	public bool Fri { get; set; }

	public FriDayType FriType { get; set; }

	public string Description { get; set; } = "";


	public DateTime DateStartM { get; set; } = DateTime.Now;


	public DateTime DateFinishM { get; set; } = DateTime.Now;


	public string DateStartCalture => Calendar.MiladiToCalture(DateStartM);

	public string DateFinishCalture => Calendar.MiladiToCalture(DateFinishM);

	public string ContextGuid { get; set; }

	public string StartTime
	{
		get
		{
			if (_clsShiftCodeing == null)
			{
				return "----";
			}
			return $"{_clsShiftCodeing.H1:00}:{_clsShiftCodeing.M1:00}";
		}
	}

	public string FinishTime
	{
		get
		{
			if (_clsShiftCodeing == null)
			{
				return "----";
			}
			return $"{_clsShiftCodeing.H2:00}:{_clsShiftCodeing.M2:00}";
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public async Task SetShiftCodingIdAsync(short shiftCodingId, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_shiftCodingId = shiftCodingId;
			_clsShiftCodeing = await ShiftCodeing.GetAsync(ContextGuid, ShiftCodingId, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public void SetClsShiftCodeing(ShiftCodeing value)
	{
		_clsShiftCodeing = value;
	}

	[JsonConstructor]
	public ShiftDet(string contextGuid, short shiftId, bool sat, bool sun, bool mon, bool tue, bool wed, bool tur, bool fri, FriDayType friType, string description, DateTime dateStartM, DateTime dateFinishM)
	{
		try
		{
			ContextGuid = contextGuid;
			ShiftId = shiftId;
			Sat = sat;
			Sun = sun;
			Mon = mon;
			Tue = tue;
			Wed = wed;
			Tur = tur;
			Fri = fri;
			FriType = friType;
			Description = description;
			DateStartM = dateStartM;
			DateFinishM = dateFinishM;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public ShiftDet(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			string text = Calendar.MiladiToCalture(DateStartM);
			text = text.Substring(0, 4) + "/01/01";
			DateStartM = Utilities.CalendarToMiladi(text);
			DateFinishM = DateStartM.AddYears(1).AddDays(-1.0);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<ShiftDet> Clone_ShiftDetAysnc(string distinationContextGuid, ShiftDet source, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				return null;
			}
			ShiftDet ret = new ShiftDet(distinationContextGuid)
			{
				ShiftId = source.ShiftId,
				Sat = source.Sat,
				Sun = source.Sun,
				Mon = source.Mon,
				Tue = source.Tue,
				Wed = source.Wed,
				Tur = source.Tur,
				Fri = source.Fri,
				FriType = source.FriType,
				Description = source.Description,
				DateStartM = source.DateStartM,
				DateFinishM = source.DateFinishM,
				ContextGuid = source.ContextGuid
			};
			await ret.SetShiftCodingIdAsync(source.ShiftCodingId);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<ShiftDet> LoadDataAsync(string contextGuid, SqlDataReader dr)
	{
		try
		{
			ShiftDet ret = new ShiftDet(contextGuid);
			if (((DbDataReader)(object)dr)["ShiftID"].ToString() != DBNull.Value.ToString())
			{
				ret.ShiftId = (short)((DbDataReader)(object)dr)["ShiftID"];
			}
			if (((DbDataReader)(object)dr)["ShiftCodingID"].ToString() != DBNull.Value.ToString())
			{
				await ret.SetShiftCodingIdAsync((short)((DbDataReader)(object)dr)["ShiftCodingID"]);
			}
			if (((DbDataReader)(object)dr)["Sat"].ToString() != DBNull.Value.ToString())
			{
				ret.Sat = (bool)((DbDataReader)(object)dr)["Sat"];
			}
			if (((DbDataReader)(object)dr)["Sun"].ToString() != DBNull.Value.ToString())
			{
				ret.Sun = (bool)((DbDataReader)(object)dr)["Sun"];
			}
			if (((DbDataReader)(object)dr)["Mon"].ToString() != DBNull.Value.ToString())
			{
				ret.Mon = (bool)((DbDataReader)(object)dr)["Mon"];
			}
			if (((DbDataReader)(object)dr)["Tue"].ToString() != DBNull.Value.ToString())
			{
				ret.Tue = (bool)((DbDataReader)(object)dr)["Tue"];
			}
			if (((DbDataReader)(object)dr)["Wed"].ToString() != DBNull.Value.ToString())
			{
				ret.Wed = (bool)((DbDataReader)(object)dr)["Wed"];
			}
			if (((DbDataReader)(object)dr)["Tur"].ToString() != DBNull.Value.ToString())
			{
				ret.Tur = (bool)((DbDataReader)(object)dr)["Tur"];
			}
			if (((DbDataReader)(object)dr)["Fri"].ToString() != DBNull.Value.ToString())
			{
				ret.Fri = (bool)((DbDataReader)(object)dr)["Fri"];
			}
			if (((DbDataReader)(object)dr)["FriType"].ToString() != DBNull.Value.ToString())
			{
				ret.FriType = (FriDayType)((DbDataReader)(object)dr)["FriType"];
			}
			if (((DbDataReader)(object)dr)["DateStartM"].ToString() != DBNull.Value.ToString())
			{
				ret.DateStartM = (DateTime)((DbDataReader)(object)dr)["DateStartM"];
			}
			if (((DbDataReader)(object)dr)["DateFinishM"].ToString() != DBNull.Value.ToString())
			{
				ret.DateFinishM = (DateTime)((DbDataReader)(object)dr)["DateFinishM"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				ret.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> SaveAsync(SqlTransaction tr, CancellationToken token)
	{
		try
		{
			SqlCommand cmd = tr.Connection.CreateCommand();
			try
			{
				cmd.Transaction = tr;
				((DbCommand)(object)cmd).CommandText = "sp_ShiftDet_Save";
				((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("ShiftID", (object)ShiftId);
				cmd.Parameters.AddWithValue("ShiftCodingID", (object)ShiftCodingId);
				cmd.Parameters.AddWithValue("Sat", (object)Sat);
				cmd.Parameters.AddWithValue("Sun", (object)Sun);
				cmd.Parameters.AddWithValue("Mon", (object)Mon);
				cmd.Parameters.AddWithValue("Tue", (object)Tue);
				cmd.Parameters.AddWithValue("Wed", (object)Wed);
				cmd.Parameters.AddWithValue("Tur", (object)Tur);
				cmd.Parameters.AddWithValue("Fri", (object)Fri);
				cmd.Parameters.AddWithValue("FriType", (object)FriType);
				cmd.Parameters.AddWithValue("DateStartM", (object)DateStartM);
				cmd.Parameters.AddWithValue("DateFinishM", (object)DateFinishM);
				cmd.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
				await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
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

	public static async Task<bool> DeleteAsync(string contextGuid, int pShiftId, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_ShiftDet_DeleteRowByShift", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("ShiftID", (object)pShiftId);
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

	public static async Task<List<ShiftDet>> GetAllAsync(string contextGuid, int pShiftId, CancellationToken token)
	{
		_ = 3;
		try
		{
			List<ShiftDet> ret = new List<ShiftDet>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_ShiftDet_SelectRowByShift]", cn);
				try
				{
					cmd.Parameters.AddWithValue("@ShiftID", (object)pShiftId);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							List<ShiftDet> list = ret;
							list.Add(await LoadDataAsync(contextGuid, dr));
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

	public static async Task<List<ShiftDet>> GetAllAsync(string contextGuid, CancellationToken token)
	{
		_ = 3;
		try
		{
			List<ShiftDet> ret = new List<ShiftDet>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_ShiftDet_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							List<ShiftDet> list = ret;
							list.Add(await LoadDataAsync(contextGuid, dr));
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
