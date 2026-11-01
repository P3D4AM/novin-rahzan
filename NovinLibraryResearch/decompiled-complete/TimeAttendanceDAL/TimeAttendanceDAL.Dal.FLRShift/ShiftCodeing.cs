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
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.Dal.FLRShift;

[Serializable]
[DoNotPruneType]
public class ShiftCodeing
{
	private DateTime _currDate;

	public short Id { get; set; }

	public short H1 { get; set; }

	public short M1 { get; set; }

	public short S1 { get; set; }

	public short H2 { get; set; }

	public short M2 { get; set; }

	public short S2 { get; set; }

	public bool NextDay { get; set; }

	public bool CalcEnteghalInSrcDate { get; set; }

	public bool NotCalcEKForShift { get; set; }

	public bool HasTashvighiInShift { get; set; }

	public bool HasTashvighIfNotKKInBeforeShift { get; set; }

	public int MazayaIdForTashvigh { get; set; }

	public int MaxTimeCanExit { get; set; }

	public short TajilKh { get; set; }

	public short TakhirKh { get; set; }

	public short TajilV { get; set; }

	public short TakhirV { get; set; }

	public short JarimeV { get; set; }

	public TypeShiftEnumType TypeShift { get; set; } = (TypeShiftEnumType)1;


	public string Description { get; set; } = "";


	public bool St { get; set; } = true;


	public string ShiftColor { get; set; } = "White";


	public bool CalcVacationInLunch { get; set; }

	public short PeakOvertime { get; set; }

	public DateTime CurrDate
	{
		get
		{
			_currDate = _currDate.Date;
			return _currDate;
		}
		set
		{
			_currDate = value.Date;
		}
	}

	public ShiftSeparationEnumType ShiftSeparation { get; set; }

	public bool HurryDelayPerform { get; set; }

	public bool ChkJarime { get; set; }

	public short LunchTime { get; set; }

	public short StartLunchTime { get; set; }

	[JsonIgnore]
	public DateTime VorodStart => Vorod.AddMinutes(-TajilV);

	[JsonIgnore]
	public DateTime VorodEnd => Vorod.AddMinutes(TakhirV);

	[JsonIgnore]
	public DateTime KhorojStart => Khoroj.AddMinutes(-TajilKh);

	[JsonIgnore]
	public DateTime KhorojEnd => Khoroj.AddMinutes(TakhirKh);

	[JsonIgnore]
	public DateTime Vorod => CurrDate.Date.AddHours(H1).AddMinutes(M1).AddSeconds(S1);

	[JsonIgnore]
	public DateTime Khoroj => CurrDate.Date.AddHours(H2).AddMinutes(M2).AddSeconds(S2);

	[JsonIgnore]
	public string StartShiftTime => H1.ToString("00") + ":" + M1.ToString("00");

	[JsonIgnore]
	public string EndShiftTime => H2.ToString("00") + ":" + M2.ToString("00");

	[JsonIgnore]
	public string NameShiftCodeing => "از " + Vorod.ToString("HH:mm") + " تا " + Khoroj.ToString("HH:mm") + " " + Description;

	[JsonIgnore]
	public DateTime PeakOvertimeTimeFormat => CurrDate.Date + TimeSpan.Parse((PeakOvertime != 0) ? Time.IntToTime((int)PeakOvertime) : "0");

	public TypeTaradodInShiftEnum TypeTaradodInShift { get; set; }

	public ShiftCodeing()
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public ShiftCodeing(ShiftCodeing pcls)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Id = pcls.Id;
			H1 = pcls.H1;
			M1 = pcls.M1;
			S1 = pcls.S1;
			if (H1 == 0 && M1 == 0 && S1 == 0)
			{
				S1 = 1;
			}
			H2 = pcls.H2;
			M2 = pcls.M2;
			S2 = pcls.S2;
			if (H2 == 24 && M2 == 0 && S2 == 0)
			{
				H2 = 23;
				M2 = 59;
				S2 = 59;
			}
			NextDay = pcls.NextDay;
			CalcEnteghalInSrcDate = pcls.CalcEnteghalInSrcDate;
			TypeTaradodInShift = pcls.TypeTaradodInShift;
			MaxTimeCanExit = pcls.MaxTimeCanExit;
			TakhirV = pcls.TakhirV;
			JarimeV = pcls.JarimeV;
			TakhirKh = pcls.TakhirKh;
			TajilV = pcls.TajilV;
			TajilKh = pcls.TajilKh;
			Description = pcls.Description;
			TypeShift = pcls.TypeShift;
			St = pcls.St;
			CurrDate = pcls.CurrDate;
			ShiftColor = pcls.ShiftColor;
			PeakOvertime = pcls.PeakOvertime;
			ShiftSeparation = pcls.ShiftSeparation;
			HurryDelayPerform = pcls.HurryDelayPerform;
			ChkJarime = pcls.ChkJarime;
			LunchTime = pcls.LunchTime;
			StartLunchTime = pcls.StartLunchTime;
			CalcVacationInLunch = pcls.CalcVacationInLunch;
			NotCalcEKForShift = pcls.NotCalcEKForShift;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\ShiftCodeing.cs", 170);
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
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
					cmd.Parameters.AddWithValue("H1", (object)H1);
					cmd.Parameters.AddWithValue("M1", (object)M1);
					cmd.Parameters.AddWithValue("H2", (object)H2);
					cmd.Parameters.AddWithValue("M2", (object)M2);
					cmd.Parameters.AddWithValue("NextDay", (object)NextDay);
					cmd.Parameters.AddWithValue("CalcEnteghalInSrcDate", (object)CalcEnteghalInSrcDate);
					cmd.Parameters.AddWithValue("typeTaradodInShift", (object)TypeTaradodInShift);
					cmd.Parameters.AddWithValue("MaxTimeCanExit", (object)MaxTimeCanExit);
					cmd.Parameters.AddWithValue("TajilKh", (object)TajilKh);
					cmd.Parameters.AddWithValue("TajilV", (object)TajilV);
					cmd.Parameters.AddWithValue("TakhirKh", (object)TakhirKh);
					cmd.Parameters.AddWithValue("Takhirv", (object)TakhirV);
					cmd.Parameters.AddWithValue("JarimeV", (object)JarimeV);
					cmd.Parameters.AddWithValue("TypeShift", (object)TypeShift);
					cmd.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
					cmd.Parameters.AddWithValue("St", (object)St);
					cmd.Parameters.AddWithValue("ShiftColor", (object)ShiftColor);
					cmd.Parameters.AddWithValue("PeakOvertime", (object)PeakOvertime);
					cmd.Parameters.AddWithValue("ShiftSeparation", (object)ShiftSeparation);
					cmd.Parameters.AddWithValue("HurryDelayPerform", (object)HurryDelayPerform);
					cmd.Parameters.AddWithValue("ChkJarime", (object)ChkJarime);
					cmd.Parameters.AddWithValue("LunchTime", (object)LunchTime);
					cmd.Parameters.AddWithValue("StartLunchTime", (object)StartLunchTime);
					cmd.Parameters.AddWithValue("CalcVacationInLunch", (object)CalcVacationInLunch);
					cmd.Parameters.AddWithValue("NotCalcEKForShift", (object)NotCalcEKForShift);
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "sp_ShiftCodeing_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							Id = Utilities.ParseToShort(obj.ToString(), (short)0);
						}
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_ShiftCodeing_Update";
						cmd.Parameters.AddWithValue("Id", (object)Id);
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

	public static async Task<ShiftCodeing> GetAsync(string contextGuid, short shiftCodingId, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return (await GetAllAsync(contextGuid, token))?.FirstOrDefault((ShiftCodeing p) => p.Id == shiftCodingId);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\ShiftCodeing.cs", 307);
			throw;
		}
	}

	public static async Task<List<ShiftCodeing>> GetAllAsync(string contextGuid, TypeShiftEnumType ptype, CancellationToken token = default(CancellationToken))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new List<ShiftCodeing>();
			return (await GetAllAsync(contextGuid, token))?.Where((ShiftCodeing p) => p.St && (p.TypeShift == ptype || (int)ptype == 0)).ToList();
		}
		catch (TaskCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<List<ShiftCodeing>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<ShiftCodeing> ret = new List<ShiftCodeing>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_ShiftCodeing_SelectAll]", cn);
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
							ret.Add(LoadData(contextGuid, (IDataRecord)dr));
							token.ThrowIfCancellationRequested();
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\ShiftCodeing.cs", 348);
			throw;
		}
	}

	private static ShiftCodeing LoadData(string contextGuid, IDataRecord dr)
	{
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ShiftCodeing shiftCodeing = new ShiftCodeing();
			if (dr["Id"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.Id = (short)dr["Id"];
			}
			if (dr["H1"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.H1 = (short)dr["H1"];
			}
			if (dr["M1"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.M1 = (short)dr["M1"];
			}
			if (dr["H2"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.H2 = (short)dr["H2"];
			}
			if (dr["M2"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.M2 = (short)dr["M2"];
			}
			if (dr["Description"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.Description = dr["Description"].ToString();
			}
			if (dr["NextDay"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.NextDay = (bool)dr["NextDay"];
			}
			if (dr["typeTaradodInShift"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.TypeTaradodInShift = (TypeTaradodInShiftEnum)dr["typeTaradodInShift"];
			}
			if (dr["CalcEnteghalInSrcDate"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.CalcEnteghalInSrcDate = (bool)dr["CalcEnteghalInSrcDate"];
			}
			if (dr["MaxTimeCanExit"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.MaxTimeCanExit = (int)dr["MaxTimeCanExit"];
			}
			if (dr["TajilKh"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.TajilKh = (short)dr["TajilKh"];
			}
			if (dr["TajilV"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.TajilV = (short)dr["TajilV"];
			}
			if (dr["TakhirKh"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.TakhirKh = (short)dr["TakhirKh"];
			}
			if (dr["TakhirV"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.TakhirV = (short)dr["TakhirV"];
			}
			if (dr["JarimeV"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.JarimeV = (short)dr["JarimeV"];
			}
			if (dr["TypeShift"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.TypeShift = (TypeShiftEnumType)dr["TypeShift"];
			}
			if (dr["St"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.St = (bool)dr["St"];
			}
			if (dr["ShiftColor"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.ShiftColor = dr["ShiftColor"].ToString();
			}
			if (dr["PeakOvertime"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.PeakOvertime = (short)dr["PeakOvertime"];
			}
			if (dr["ShiftSeparation"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.ShiftSeparation = (ShiftSeparationEnumType)dr["ShiftSeparation"];
			}
			if (dr["HurryDelayPerform"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.HurryDelayPerform = (bool)dr["HurryDelayPerform"];
			}
			if (dr["ChkJarime"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.ChkJarime = (bool)dr["ChkJarime"];
			}
			if (dr["LunchTime"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.LunchTime = (short)dr["LunchTime"];
			}
			if (dr["StartLunchTime"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.StartLunchTime = (short)dr["StartLunchTime"];
			}
			if (dr["CalcVacationInLunch"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.CalcVacationInLunch = (bool)dr["CalcVacationInLunch"];
			}
			if (dr["NotCalcEKForShift"].ToString() != DBNull.Value.ToString())
			{
				shiftCodeing.NotCalcEKForShift = (bool)dr["NotCalcEKForShift"];
			}
			return shiftCodeing;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\ShiftCodeing.cs", 383);
			throw;
		}
	}

	public static async Task<short> IsAlreadyExistAsync(string contextGuid, short h1, short m1, short h2, short m2, bool nextDay, TypeShiftEnumType typeShift, CancellationToken token)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return ((await GetAllAsync(contextGuid, token))?.FirstOrDefault((ShiftCodeing p) => p.H1 == h1 && p.M1 == m1 && p.H2 == h2 && p.M2 == m2 && p.NextDay == nextDay && p.TypeShift == typeShift))?.Id ?? (-1);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static ShiftCodeing Clone(ShiftCodeing clsShiftCoding)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return new ShiftCodeing
			{
				S1 = clsShiftCoding.S1,
				M1 = clsShiftCoding.M1,
				H1 = clsShiftCoding.H1,
				S2 = clsShiftCoding.S2,
				M2 = clsShiftCoding.M2,
				H2 = clsShiftCoding.H2,
				St = clsShiftCoding.St,
				NextDay = clsShiftCoding.NextDay,
				TakhirV = clsShiftCoding.TakhirV,
				JarimeV = clsShiftCoding.JarimeV,
				TakhirKh = clsShiftCoding.TakhirKh,
				CurrDate = clsShiftCoding.CurrDate,
				ChkJarime = clsShiftCoding.ChkJarime,
				TypeShift = clsShiftCoding.TypeShift,
				LunchTime = clsShiftCoding.LunchTime,
				ShiftColor = clsShiftCoding.ShiftColor,
				Description = clsShiftCoding.Description,
				PeakOvertime = clsShiftCoding.PeakOvertime,
				MaxTimeCanExit = clsShiftCoding.MaxTimeCanExit,
				StartLunchTime = clsShiftCoding.StartLunchTime,
				ShiftSeparation = clsShiftCoding.ShiftSeparation,
				HurryDelayPerform = clsShiftCoding.HurryDelayPerform,
				CalcEnteghalInSrcDate = clsShiftCoding.CalcEnteghalInSrcDate,
				TypeTaradodInShift = clsShiftCoding.TypeTaradodInShift,
				HasTashvighiInShift = clsShiftCoding.HasTashvighiInShift,
				MazayaIdForTashvigh = clsShiftCoding.MazayaIdForTashvigh,
				CalcVacationInLunch = clsShiftCoding.CalcVacationInLunch,
				HasTashvighIfNotKKInBeforeShift = clsShiftCoding.HasTashvighIfNotKKInBeforeShift,
				NotCalcEKForShift = clsShiftCoding.NotCalcEKForShift
			};
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
