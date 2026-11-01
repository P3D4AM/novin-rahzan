using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceDAL.Dal.Taradod;

[Serializable]
[DoNotPruneType]
public class MorakhasiSaati : IHasContext, IAttPersonLeave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public string ContextGuid { get; set; }

	public string DateCalutre => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateFrom);

	public int PersonID { get; set; }

	public int Duration { get; set; }

	public string DurationTime => $"{Duration % 60:D2} : {Duration / 60:D2}";

	public string DayOfWeek => LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(DateFrom);

	public string PersonFullName { get; set; } = "";


	public Guid PersonGuid { get; set; }

	public DateTime RequestDate { get; set; } = DateTime.Today;


	public string Reason { get; set; }

	public DateTime DateFrom { get; set; } = DateTime.Today;


	public DateTime DateTo { get; set; }

	public EnPersonLeaveType Type { get; set; }

	public EnPersonLeaveStatus LeaveStatus { get; set; }

	public EnPersonLeaveKind Kind { get; set; } = (EnPersonLeaveKind)2;


	public string ManagerMessage { get; set; } = "";


	public DateTime? ManagerDate { get; set; } = DateTime.Today;


	public EnPersonLeaveQuiddity Quiddity { get; set; }

	public DateTime Modified { get; set; } = DateTime.Today;


	public Guid Guid { get; set; }

	public bool Status { get; }

	public MorakhasiSaati(string contextGuid)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
	}

	public MorakhasiSaati(string contextGuid, SqlDataReader dr)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["DateM"].ToString() != "")
			{
				DateTime dateFrom = (DateTo = (DateTime)((DbDataReader)(object)dr)["DateM"]);
				DateFrom = dateFrom;
			}
			if (((DbDataReader)(object)dr)["PersonID"].ToString() != "")
			{
				PersonID = (int)((DbDataReader)(object)dr)["PersonID"];
			}
			if (((DbDataReader)(object)dr)["Duration"].ToString() != "")
			{
				Duration = (int)((DbDataReader)(object)dr)["Duration"];
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\MorakhasiSaati.cs", 69);
			return false;
		}
	}

	public static async Task<List<MorakhasiSaati>> GetSpecificDataAsync(string contextGuid, int personID, DateTime pDate1, DateTime pDate2, TypeTaradodEnumType ptype, CancellationToken token)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		List<MorakhasiSaati> ret = new List<MorakhasiSaati>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_MorakhasiSaati_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@personID", (object)personID);
					cmd.Parameters.AddWithValue("@Date1", (object)pDate1);
					cmd.Parameters.AddWithValue("@Date2", (object)pDate2);
					cmd.Parameters.AddWithValue("@TrafficType", (object)ptype);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (dr != null && ((DbDataReader)(object)dr).HasRows)
						{
							Person person = await Person.GetByPersonIdAsync(contextGuid, personID, token);
							string fullName = ((person != null) ? ((PersonBase)person).FullName : null) ?? "";
							while (await ((DbDataReader)(object)dr).ReadAsync(token))
							{
								MorakhasiSaati obj = new MorakhasiSaati(contextGuid, dr)
								{
									PersonFullName = fullName
								};
								obj.PersonGuid = ((person != null) ? ((PersonBase)person).Guid : Guid.Empty);
								MorakhasiSaati item = obj;
								ret.Add(item);
							}
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
