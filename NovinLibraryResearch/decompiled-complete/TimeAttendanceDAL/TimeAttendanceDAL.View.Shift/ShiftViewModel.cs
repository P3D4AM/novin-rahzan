using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRShift;

namespace TimeAttendanceDAL.View.Shift;

[Serializable]
[DoNotPruneType]
public class ShiftViewModel : IHasContext
{
	public string ContextGuid { get; set; }

	public short ShiftId { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }

	public bool St { get; set; }

	public bool StTatili { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }

	public PackageEnumType PackageType { get; set; }

	public short Periodic { get; set; }

	public Guid PackageGuid { get; set; }

	public DateTime DateStartM { get; set; }

	public DateTime DateFinishM { get; set; }

	public string DateStartSh => Calendar.MiladiToCalture(DateStartM);

	public string DateFinishSh => Calendar.MiladiToCalture(DateFinishM);

	public string StartTimeSat { get; set; }

	public string EndTimeSat { get; set; }

	public string StartTimeSun { get; set; }

	public string EndTimeSun { get; set; }

	public string StartTimeMon { get; set; }

	public string EndTimeMon { get; set; }

	public string StartTimeTue { get; set; }

	public string EndTimeTue { get; set; }

	public string StartTimeWed { get; set; }

	public string EndTimeWed { get; set; }

	public string StartTimeThu { get; set; }

	public string EndTimeThu { get; set; }

	public string StartTimeFri { get; set; }

	public string EndTimeFri { get; set; }

	public bool Sat { get; set; }

	public bool Sun { get; set; }

	public bool Mon { get; set; }

	public bool Tue { get; set; }

	public bool Wed { get; set; }

	public bool Thu { get; set; }

	public bool Fri { get; set; }

	public ShiftDet.FriDayType FriType { get; set; }

	public bool NextDay { get; set; }

	public TypeShiftEnumType TypeShift { get; set; }

	public string PackageName => Utilities.GetDisplay<PackageEnumType>(PackageType);

	public string DescOfDays
	{
		get
		{
			string text = "";
			if (Sat && Sun && Mon && Tue && Wed && Thu)
			{
				text = "هر روز هفته";
			}
			else if (Sat && Mon && Wed)
			{
				text = "روزهای زوج";
			}
			else if (Sun && Tue && Thu)
			{
				text = "روزهای فرد";
			}
			else
			{
				if (Sat)
				{
					text = "شنبه ";
				}
				if (Sun)
				{
					text += ", یکشنبه ";
				}
				if (Mon)
				{
					text += ", دوشنبه ";
				}
				if (Tue)
				{
					text += ", سه شنبه ";
				}
				if (Wed)
				{
					text += ", چهارشنبه ";
				}
				if (Thu)
				{
					text += ", پنجشنبه ";
				}
				if (Fri)
				{
					text += ", جمعه ";
				}
			}
			return text;
		}
	}

	public ShiftViewModel(string contextGuid)
	{
		ContextGuid = contextGuid;
		ShiftId = 0;
		Name = "";
		Code = "";
		St = true;
		StTatili = false;
		Description = "";
		Price = 0m;
		PackageType = (PackageEnumType)51;
		Periodic = 0;
		DateStartM = DateTime.Now;
		DateFinishM = DateTime.Now;
		StartTimeSat = "";
		EndTimeSat = "";
		StartTimeSun = "";
		EndTimeSun = "";
		StartTimeMon = "";
		EndTimeMon = "";
		StartTimeTue = "";
		EndTimeTue = "";
		StartTimeWed = "";
		EndTimeWed = "";
		StartTimeThu = "";
		EndTimeThu = "";
		StartTimeFri = "";
		EndTimeFri = "";
		Sat = true;
		Sun = true;
		Mon = true;
		Tue = true;
		Wed = true;
		Thu = true;
		Fri = false;
		FriType = ShiftDet.FriDayType.EkhtiyariAndKarkard;
		NextDay = false;
		TypeShift = (TypeShiftEnumType)1;
	}

	public static async Task<ShiftViewModel> GetAsync(string contextGuid, short pid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			ShiftViewModel shiftview = new ShiftViewModel
			{
				ContextGuid = contextGuid
			};
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(shiftview.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Shifts_ShiftViewModel", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)pid);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							shiftview.LoadData(dr);
						}
						((DbConnection)(object)cn).Close();
						return shiftview;
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

	public ShiftViewModel(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	public ShiftViewModel()
	{
	}

	private void LoadData(SqlDataReader dr)
	{
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((DbDataReader)(object)dr)["ShiftID"].ToString() != DBNull.Value.ToString())
			{
				ShiftId = (short)((DbDataReader)(object)dr)["ShiftID"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["Code"].ToString() != DBNull.Value.ToString())
			{
				Code = ((DbDataReader)(object)dr)["Code"].ToString();
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				St = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["STTatili"].ToString() != DBNull.Value.ToString())
			{
				StTatili = (bool)((DbDataReader)(object)dr)["STTatili"];
			}
			if (((DbDataReader)(object)dr)["PackageType"].ToString() != DBNull.Value.ToString())
			{
				PackageType = (PackageEnumType)((DbDataReader)(object)dr)["PackageType"];
			}
			if (((DbDataReader)(object)dr)["Periodic"].ToString() != DBNull.Value.ToString())
			{
				Periodic = (short)((DbDataReader)(object)dr)["Periodic"];
			}
			if (((DbDataReader)(object)dr)["PackageGuid"].ToString() != DBNull.Value.ToString())
			{
				PackageGuid = (Guid)((DbDataReader)(object)dr)["PackageGuid"];
			}
			if (((DbDataReader)(object)dr)["DateStartM"].ToString() != DBNull.Value.ToString())
			{
				DateStartM = (DateTime)((DbDataReader)(object)dr)["DateStartM"];
			}
			if (((DbDataReader)(object)dr)["DateFinishM"].ToString() != DBNull.Value.ToString())
			{
				DateFinishM = (DateTime)((DbDataReader)(object)dr)["DateFinishM"];
			}
			if (((DbDataReader)(object)dr)["Sat"].ToString() != DBNull.Value.ToString())
			{
				Sat = (bool)((DbDataReader)(object)dr)["Sat"];
			}
			if (((DbDataReader)(object)dr)["Sun"].ToString() != DBNull.Value.ToString())
			{
				Sun = (bool)((DbDataReader)(object)dr)["Sun"];
			}
			if (((DbDataReader)(object)dr)["Mon"].ToString() != DBNull.Value.ToString())
			{
				Mon = (bool)((DbDataReader)(object)dr)["Mon"];
			}
			if (((DbDataReader)(object)dr)["Tue"].ToString() != DBNull.Value.ToString())
			{
				Tue = (bool)((DbDataReader)(object)dr)["Tue"];
			}
			if (((DbDataReader)(object)dr)["Wed"].ToString() != DBNull.Value.ToString())
			{
				Wed = (bool)((DbDataReader)(object)dr)["Wed"];
			}
			if (((DbDataReader)(object)dr)["Tur"].ToString() != DBNull.Value.ToString())
			{
				Thu = (bool)((DbDataReader)(object)dr)["Tur"];
			}
			if (((DbDataReader)(object)dr)["Fri"].ToString() != DBNull.Value.ToString())
			{
				Fri = (bool)((DbDataReader)(object)dr)["Fri"];
			}
			if (((DbDataReader)(object)dr)["FriType"].ToString() != DBNull.Value.ToString())
			{
				FriType = (ShiftDet.FriDayType)((DbDataReader)(object)dr)["FriType"];
			}
			if (((DbDataReader)(object)dr)["NextDay"].ToString() != DBNull.Value.ToString())
			{
				NextDay = (bool)((DbDataReader)(object)dr)["NextDay"];
			}
			if (((DbDataReader)(object)dr)["TypeShift"].ToString() != DBNull.Value.ToString())
			{
				TypeShift = (TypeShiftEnumType)((DbDataReader)(object)dr)["TypeShift"];
			}
			short num = 0;
			short num2 = 0;
			short num3 = 0;
			short num4 = 0;
			if (((DbDataReader)(object)dr)["h1"].ToString() != DBNull.Value.ToString())
			{
				num = (short)((DbDataReader)(object)dr)["h1"];
			}
			if (((DbDataReader)(object)dr)["h2"].ToString() != DBNull.Value.ToString())
			{
				num2 = (short)((DbDataReader)(object)dr)["h2"];
			}
			if (((DbDataReader)(object)dr)["m1"].ToString() != DBNull.Value.ToString())
			{
				num3 = (short)((DbDataReader)(object)dr)["m1"];
			}
			if (((DbDataReader)(object)dr)["m2"].ToString() != DBNull.Value.ToString())
			{
				num4 = (short)((DbDataReader)(object)dr)["m2"];
			}
			if (Sat)
			{
				StartTimeSat = $"{num:00}:{num3:00}";
				EndTimeSat = $"{num2:00}:{num4:00}";
			}
			if (Sun)
			{
				StartTimeSun = $"{num:00}:{num3:00}";
				EndTimeSun = $"{num2:00}:{num4:00}";
			}
			if (Mon)
			{
				StartTimeMon = $"{num:00}:{num3:00}";
				EndTimeMon = $"{num2:00}:{num4:00}";
			}
			if (Tue)
			{
				StartTimeTue = $"{num:00}:{num3:00}";
				EndTimeTue = $"{num2:00}:{num4:00}";
			}
			if (Wed)
			{
				StartTimeWed = $"{num:00}:{num3:00}";
				EndTimeWed = $"{num2:00}:{num4:00}";
			}
			if (Thu)
			{
				StartTimeThu = $"{num:00}:{num3:00}";
				EndTimeThu = $"{num2:00}:{num4:00}";
			}
			if (Fri)
			{
				StartTimeFri = $"{num:00}:{num3:00}";
				EndTimeFri = $"{num2:00}:{num4:00}";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Shift\\ShiftViewModel.cs", 238);
		}
	}

	private void LoadDataExtra(SqlDataReader dr)
	{
		try
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			if (((DbDataReader)(object)dr)["Sat"].ToString() != DBNull.Value.ToString())
			{
				flag = (bool)((DbDataReader)(object)dr)["Sat"];
			}
			if (((DbDataReader)(object)dr)["Sun"].ToString() != DBNull.Value.ToString())
			{
				flag2 = (bool)((DbDataReader)(object)dr)["Sun"];
			}
			if (((DbDataReader)(object)dr)["Mon"].ToString() != DBNull.Value.ToString())
			{
				flag3 = (bool)((DbDataReader)(object)dr)["Mon"];
			}
			if (((DbDataReader)(object)dr)["Tue"].ToString() != DBNull.Value.ToString())
			{
				flag4 = (bool)((DbDataReader)(object)dr)["Tue"];
			}
			if (((DbDataReader)(object)dr)["Wed"].ToString() != DBNull.Value.ToString())
			{
				flag5 = (bool)((DbDataReader)(object)dr)["Wed"];
			}
			if (((DbDataReader)(object)dr)["Tur"].ToString() != DBNull.Value.ToString())
			{
				flag6 = (bool)((DbDataReader)(object)dr)["Tur"];
			}
			if (((DbDataReader)(object)dr)["Fri"].ToString() != DBNull.Value.ToString())
			{
				flag7 = (bool)((DbDataReader)(object)dr)["Fri"];
			}
			short num = 0;
			short num2 = 0;
			short num3 = 0;
			short num4 = 0;
			if (((DbDataReader)(object)dr)["h1"].ToString() != DBNull.Value.ToString())
			{
				num = (short)((DbDataReader)(object)dr)["h1"];
			}
			if (((DbDataReader)(object)dr)["h2"].ToString() != DBNull.Value.ToString())
			{
				num2 = (short)((DbDataReader)(object)dr)["h2"];
			}
			if (((DbDataReader)(object)dr)["m1"].ToString() != DBNull.Value.ToString())
			{
				num3 = (short)((DbDataReader)(object)dr)["m1"];
			}
			if (((DbDataReader)(object)dr)["m2"].ToString() != DBNull.Value.ToString())
			{
				num4 = (short)((DbDataReader)(object)dr)["m2"];
			}
			if (flag)
			{
				Sat = true;
				StartTimeSat = $"{num3:00} : {num:00}";
				EndTimeSat = $"{num4:00} : {num2:00}";
			}
			if (flag2)
			{
				Sun = flag2;
				StartTimeSun = $"{num3:00} : {num:00}";
				EndTimeSun = $"{num4:00} : {num2:00}";
			}
			if (flag3)
			{
				Mon = flag3;
				StartTimeMon = $"{num3:00} : {num:00}";
				EndTimeMon = $"{num4:00} : {num2:00}";
			}
			if (flag4)
			{
				Tue = flag4;
				StartTimeTue = $"{num3:00} : {num:00}";
				EndTimeTue = $"{num4:00} : {num2:00}";
			}
			if (flag5)
			{
				Wed = flag5;
				StartTimeWed = $"{num3:00} : {num:00}";
				EndTimeWed = $"{num4:00} : {num2:00}";
			}
			if (flag6)
			{
				Thu = flag6;
				StartTimeThu = $"{num3:00} : {num:00}";
				EndTimeThu = $"{num4:00} : {num2:00}";
			}
			if (flag7)
			{
				Fri = flag7;
				StartTimeFri = $"{num3:00} : {num:00}";
				EndTimeFri = $"{num4:00} : {num2:00}";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataExtra", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Shift\\ShiftViewModel.cs", 311);
		}
	}

	public static async Task<List<ShiftViewModel>> SelectAllAsync(string contextGuid, PackageEnumType package, CancellationToken token = default(CancellationToken))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		List<ShiftViewModel> ret = new List<ShiftViewModel>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_Shifts_ShiftViewModelAll]", cn);
				try
				{
					cmd.Parameters.AddWithValue("PackageType", (object)package);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						short xshiftid = -1;
						ShiftViewModel xcls = null;
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							short num = -2;
							if (((DbDataReader)(object)dr)["ShiftID"].ToString() != DBNull.Value.ToString())
							{
								num = (short)((DbDataReader)(object)dr)["ShiftID"];
							}
							if (xshiftid != num)
							{
								xshiftid = num;
								xcls = new ShiftViewModel(contextGuid, dr);
								ret.Add(xcls);
							}
							else
							{
								xcls?.LoadDataExtra(dr);
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

	public async Task<Shifts> ConvertToShiftsAsync(CancellationToken token)
	{
		Shifts ret = new Shifts(ContextGuid, ShiftId, Name, Code, Description, PackageType, Periodic, PackageGuid, St, StTatili);
		try
		{
			ret.LstShiftDet = new List<ShiftDet>();
			ShiftDet shiftDet = new ShiftDet(ContextGuid, ShiftId, Sat, Sun, Mon, Tue, Wed, Thu, Fri, FriType, Description, DateStartM, DateFinishM);
			ret.LstShiftDet.Add(shiftDet);
			if (Sat)
			{
				ShiftDet shiftDet2 = await InitShiftCodingAsync(StartTimeSat, EndTimeSat, shiftDet, ret.LstShiftDet, token);
				if (shiftDet2 != null)
				{
					ret.LstShiftDet.Add(shiftDet2);
				}
				shiftDet = null;
			}
			if (Sun)
			{
				ShiftDet shiftDet2 = await InitShiftCodingAsync(StartTimeSun, EndTimeSun, shiftDet, ret.LstShiftDet, token);
				if (shiftDet2 != null)
				{
					ret.LstShiftDet.Add(shiftDet2);
				}
				shiftDet = null;
			}
			if (Mon)
			{
				ShiftDet shiftDet2 = await InitShiftCodingAsync(StartTimeMon, EndTimeMon, shiftDet, ret.LstShiftDet, token);
				if (shiftDet2 != null)
				{
					ret.LstShiftDet.Add(shiftDet2);
				}
				shiftDet = null;
			}
			if (Tue)
			{
				ShiftDet shiftDet2 = await InitShiftCodingAsync(StartTimeTue, EndTimeTue, shiftDet, ret.LstShiftDet, token);
				if (shiftDet2 != null)
				{
					ret.LstShiftDet.Add(shiftDet2);
				}
				shiftDet = null;
			}
			if (Wed)
			{
				ShiftDet shiftDet2 = await InitShiftCodingAsync(StartTimeWed, EndTimeWed, shiftDet, ret.LstShiftDet, token);
				if (shiftDet2 != null)
				{
					ret.LstShiftDet.Add(shiftDet2);
				}
				shiftDet = null;
			}
			if (Thu)
			{
				ShiftDet shiftDet2 = await InitShiftCodingAsync(StartTimeThu, EndTimeThu, shiftDet, ret.LstShiftDet, token);
				if (shiftDet2 != null)
				{
					ret.LstShiftDet.Add(shiftDet2);
				}
				shiftDet = null;
			}
			if (Fri)
			{
				ShiftDet shiftDet2 = await InitShiftCodingAsync(StartTimeFri, EndTimeFri, shiftDet, ret.LstShiftDet, token);
				if (shiftDet2 != null)
				{
					ret.LstShiftDet.Add(shiftDet2);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConvertToShiftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Shift\\ShiftViewModel.cs", 417);
		}
		return ret;
	}

	private async Task<ShiftDet> InitShiftCodingAsync(string pStartTime, string pEndTime, ShiftDet pDet, List<ShiftDet> plist, CancellationToken token)
	{
		ShiftDet xclShiftDet = pDet;
		try
		{
			if (pStartTime.Length < 5)
			{
				pStartTime = "00:01";
			}
			if (pEndTime.Length < 5)
			{
				pEndTime = "23:59";
			}
			short h1 = Utilities.ParseToShort(pStartTime.Substring(0, 2), (short)0);
			short m1 = Utilities.ParseToShort(pStartTime.Substring(3, 2), (short)0);
			short h2 = Utilities.ParseToShort(pEndTime.Substring(0, 2), (short)0);
			short m2 = Utilities.ParseToShort(pEndTime.Substring(3, 2), (short)0);
			short codeid = await ShiftCodeing.IsAlreadyExistAsync(ContextGuid, h1, m1, h2, m2, NextDay, TypeShift, token);
			if (codeid > 0 && plist.Any((ShiftDet q) => q.ShiftCodingId.Equals(codeid)))
			{
				return null;
			}
			if (xclShiftDet == null)
			{
				xclShiftDet = new ShiftDet(ContextGuid, ShiftId, Sat, Sun, Mon, Tue, Wed, Thu, Fri, FriType, Description, DateStartM, DateFinishM);
			}
			if (codeid == -1)
			{
				ShiftCodeing shiftCoding = xclShiftDet.ShiftCodeing;
				shiftCoding.H1 = h1;
				shiftCoding.M1 = m1;
				shiftCoding.H2 = h2;
				shiftCoding.M2 = m2;
				shiftCoding.Description = Description;
				shiftCoding.NextDay = NextDay;
				if (NextDay)
				{
					shiftCoding.MaxTimeCanExit = h2 * 60 + m2 + 60;
				}
				shiftCoding.TajilKh = 0;
				shiftCoding.TakhirKh = 0;
				shiftCoding.TajilV = 0;
				shiftCoding.TakhirV = 0;
				shiftCoding.JarimeV = 0;
				shiftCoding.TypeShift = (TypeShiftEnumType)1;
				shiftCoding.Description = Description;
				shiftCoding.St = true;
				await shiftCoding.SaveAsync(token);
				codeid = shiftCoding.Id;
			}
			await xclShiftDet.SetShiftCodingIdAsync(codeid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitShiftCodingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Shift\\ShiftViewModel.cs", 464);
		}
		return xclShiftDet;
	}
}
