using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDevices;

namespace TimeAttendanceDAL.Dal.Enroll;

public class ValidGharardad : IHasContext
{
	private List<LogPerson> _logPersons;

	private List<EnrollData> _enrollData;

	public string ContextGuid { get; set; }

	public int GharardadId { get; set; }

	public string GharardadNo { get; set; }

	public int PersonId { get; set; }

	public short ShiftId { get; set; }

	public DateTime DateFinishMGharardad { get; set; }

	public DateTime DateStartMGharardad { get; set; }

	public string FullName { get; set; }

	public string CodeMeli { get; set; }

	public string ShiftName { get; set; }

	public decimal Price { get; set; }

	public PackageEnumType PackageType { get; set; }

	public short Periodic { get; set; }

	public Guid PackageGuid { get; set; }

	public short ShiftCodingId { get; set; }

	public string DateStartShShift { get; set; }

	public string DateFinishShShift { get; set; }

	public bool Sat { get; set; }

	public bool Sun { get; set; }

	public bool Mon { get; set; }

	public bool Tue { get; set; }

	public bool Wed { get; set; }

	public bool Tur { get; set; }

	public bool Fri { get; set; }

	public short H1 { get; set; }

	public short M1 { get; set; }

	public short H2 { get; set; }

	public short M2 { get; set; }

	public short TajilKh { get; set; }

	public short TajilV { get; set; }

	public short TakhirV { get; set; }

	public short JarimeV { get; set; }

	public short TakhirKh { get; set; }

	public TypeShiftEnumType TypeShift { get; set; }

	public string EnrollDataStr { get; set; } = "";


	public List<LogPerson> LogPersons => _logPersons ?? (_logPersons = LogPerson.GetSessionNo(ContextGuid, PersonId, DateStartMGharardad, DateFinishMGharardad));

	public List<EnrollData> EnrollDataObj
	{
		get
		{
			if (_enrollData == null)
			{
				_enrollData = new List<EnrollData>();
				EnrollData item = JsonConvert.DeserializeObject<EnrollData>(EnrollDataStr);
				_enrollData.Add(item);
			}
			return _enrollData;
		}
	}

	public ValidGharardad(string contextGuid, IDataRecord dr)
	{
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		try
		{
			if (dr["GharardadId"].ToString() != DBNull.Value.ToString())
			{
				GharardadId = (int)dr["GharardadId"];
			}
			if (dr["GharardadNo"].ToString() != DBNull.Value.ToString())
			{
				GharardadNo = dr["GharardadNo"].ToString();
			}
			if (dr["PersonId"].ToString() != DBNull.Value.ToString())
			{
				PersonId = (int)dr["PersonId"];
			}
			if (dr["ShiftId"].ToString() != DBNull.Value.ToString())
			{
				ShiftId = (short)dr["ShiftId"];
			}
			if (dr["DateFinishM"].ToString() != DBNull.Value.ToString())
			{
				DateFinishMGharardad = (DateTime)dr["DateFinishM"];
			}
			if (dr["DateStartM"].ToString() != DBNull.Value.ToString())
			{
				DateStartMGharardad = (DateTime)dr["DateStartM"];
			}
			if (dr["FullName"].ToString() != DBNull.Value.ToString())
			{
				FullName = dr["FullName"].ToString();
			}
			if (dr["CodeMeli"].ToString() != DBNull.Value.ToString())
			{
				CodeMeli = dr["CodeMeli"].ToString();
			}
			if (dr["ShiftName"].ToString() != DBNull.Value.ToString())
			{
				ShiftName = dr["ShiftName"].ToString();
			}
			if (dr["PackageType"].ToString() != DBNull.Value.ToString())
			{
				PackageType = (PackageEnumType)dr["PackageType"];
			}
			if (dr["Periodic"].ToString() != DBNull.Value.ToString())
			{
				Periodic = (short)dr["Periodic"];
			}
			if (dr["PackageGuid"].ToString() != DBNull.Value.ToString())
			{
				PackageGuid = (Guid)dr["PackageGuid"];
			}
			if (dr["ShiftCodingId"].ToString() != DBNull.Value.ToString())
			{
				ShiftCodingId = (short)dr["ShiftCodingId"];
			}
			if (dr["Sat"].ToString() != DBNull.Value.ToString())
			{
				Sat = (bool)dr["Sat"];
			}
			if (dr["Sun"].ToString() != DBNull.Value.ToString())
			{
				Sun = (bool)dr["Sun"];
			}
			if (dr["Mon"].ToString() != DBNull.Value.ToString())
			{
				Mon = (bool)dr["Mon"];
			}
			if (dr["Tue"].ToString() != DBNull.Value.ToString())
			{
				Tue = (bool)dr["Tue"];
			}
			if (dr["Wed"].ToString() != DBNull.Value.ToString())
			{
				Wed = (bool)dr["Wed"];
			}
			if (dr["Tur"].ToString() != DBNull.Value.ToString())
			{
				Tur = (bool)dr["Tur"];
			}
			if (dr["Fri"].ToString() != DBNull.Value.ToString())
			{
				Fri = (bool)dr["Fri"];
			}
			if (dr["H1"].ToString() != DBNull.Value.ToString())
			{
				H1 = (short)dr["H1"];
			}
			if (dr["M1"].ToString() != DBNull.Value.ToString())
			{
				M1 = (short)dr["M1"];
			}
			if (dr["H2"].ToString() != DBNull.Value.ToString())
			{
				H2 = (short)dr["H2"];
			}
			if (dr["M2"].ToString() != DBNull.Value.ToString())
			{
				M2 = (short)dr["M2"];
			}
			if (dr["TajilKh"].ToString() != DBNull.Value.ToString())
			{
				TajilKh = (short)dr["TajilKh"];
			}
			if (dr["TajilV"].ToString() != DBNull.Value.ToString())
			{
				TajilV = (short)dr["TajilV"];
			}
			if (dr["TakhirV"].ToString() != DBNull.Value.ToString())
			{
				TakhirV = (short)dr["TakhirV"];
			}
			if (dr["JarimeV"].ToString() != DBNull.Value.ToString())
			{
				JarimeV = (short)dr["JarimeV"];
			}
			if (dr["TakhirKh"].ToString() != DBNull.Value.ToString())
			{
				TakhirKh = (short)dr["TakhirKh"];
			}
			if (dr["TypeShift"].ToString() != DBNull.Value.ToString())
			{
				TypeShift = (TypeShiftEnumType)dr["TypeShift"];
			}
			if (dr["EnrollData"].ToString() != DBNull.Value.ToString())
			{
				byte[] bytes = (byte[])dr["EnrollData"];
				EnrollDataStr = Encoding.UTF8.GetString(bytes);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public static async Task<List<ValidGharardad>> GetValidGharardadsAsync(string contextGuid, DateTime dateM, int deviceId, CancellationToken token)
	{
		List<ValidGharardad> ret = new List<ValidGharardad>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Gharardad_Valid]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("CurrDate", (object)dateM.Date);
					cmd.Parameters.AddWithValue("DeviceId", (object)deviceId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new ValidGharardad(contextGuid, (IDataRecord)dr));
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
			return ret;
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
