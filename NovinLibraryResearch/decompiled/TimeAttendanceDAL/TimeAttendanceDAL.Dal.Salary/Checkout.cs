using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Dal.Salary;

public class Checkout : IHasContext
{
	private GharardadNew _gharardad;

	public string ContextGuid { get; set; }

	public int Id { get; private set; }

	public string Desccription { get; set; } = "";


	public bool St { get; set; } = true;


	public int GharardadId { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string DateSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateM);

	public int RestMission { get; set; }

	public decimal RewardMab { get; set; }

	public string RewardFormula { get; set; }

	public decimal SeverancepayMab { get; set; }

	public string SeverancepayFormula { get; set; }

	public decimal OtherBenefitMab { get; set; }

	public string OtherBenefitDesc { get; set; }

	public decimal OtherDeficitMab { get; set; }

	public string OtherDeficitDesc { get; set; }

	public async Task<GharardadNew> GetGharardadAsync(CancellationToken token)
	{
		try
		{
			GharardadNew gharardadNew = _gharardad;
			if (gharardadNew == null)
			{
				gharardadNew = (_gharardad = await GharardadNew.GetAsync(ContextGuid, GharardadId, token));
			}
			return gharardadNew;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Checkout()
	{
	}

	public static async Task<Checkout> GetAsync(string contextGuid, int id, CancellationToken token)
	{
		_ = 1;
		try
		{
			Checkout ret = new Checkout
			{
				ContextGuid = contextGuid
			};
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand cmd = new SqlCommand("sp_Checkout_SelectRow", cn);
			cmd.Parameters.AddWithValue("Id", (object)id);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlDataReader dr = cmd.ExecuteReader();
			if (await ((DbDataReader)(object)dr).ReadAsync(token))
			{
				ret.LoadData(dr);
			}
			((DbConnection)(object)cn).Close();
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<Checkout> GetAsync(string contextGuid, GharardadNew gharardad, CancellationToken token)
	{
		Checkout ret = new Checkout
		{
			ContextGuid = contextGuid
		};
		try
		{
			ret._gharardad = gharardad;
			ret.GharardadId = gharardad.ID;
			await ret.ComputingAsync(token);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Checkout(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				St = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["Desccription"].ToString() != DBNull.Value.ToString())
			{
				Desccription = ((DbDataReader)(object)dr)["Desccription"].ToString();
			}
			if (((DbDataReader)(object)dr)["GharardadId"].ToString() != DBNull.Value.ToString())
			{
				GharardadId = (int)((DbDataReader)(object)dr)["GharardadId"];
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["RestMission"].ToString() != DBNull.Value.ToString())
			{
				RestMission = (int)((DbDataReader)(object)dr)["RestMission"];
			}
			if (((DbDataReader)(object)dr)["RewardMab"].ToString() != DBNull.Value.ToString())
			{
				RewardMab = (decimal)((DbDataReader)(object)dr)["RewardMab"];
			}
			if (((DbDataReader)(object)dr)["RewardFormula"].ToString() != DBNull.Value.ToString())
			{
				RewardFormula = ((DbDataReader)(object)dr)["RewardFormula"].ToString();
			}
			if (((DbDataReader)(object)dr)["SeverancepayMab"].ToString() != DBNull.Value.ToString())
			{
				SeverancepayMab = (decimal)((DbDataReader)(object)dr)["SeverancepayMab"];
			}
			if (((DbDataReader)(object)dr)["SeverancepayFormula"].ToString() != DBNull.Value.ToString())
			{
				SeverancepayFormula = ((DbDataReader)(object)dr)["SeverancepayFormula"].ToString();
			}
			if (((DbDataReader)(object)dr)["OtherBenefitMab"].ToString() != DBNull.Value.ToString())
			{
				OtherBenefitMab = (decimal)((DbDataReader)(object)dr)["OtherBenefitMab"];
			}
			if (((DbDataReader)(object)dr)["OtherBenefitDesc"].ToString() != DBNull.Value.ToString())
			{
				OtherBenefitDesc = ((DbDataReader)(object)dr)["OtherBenefitDesc"].ToString();
			}
			if (((DbDataReader)(object)dr)["OtherDeficitMab"].ToString() != DBNull.Value.ToString())
			{
				OtherDeficitMab = (decimal)((DbDataReader)(object)dr)["OtherDeficitMab"];
			}
			if (((DbDataReader)(object)dr)["OtherDeficitDesc"].ToString() != DBNull.Value.ToString())
			{
				OtherDeficitDesc = ((DbDataReader)(object)dr)["OtherDeficitDesc"].ToString();
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Checkout.cs", 178);
			return false;
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand cmd = new SqlCommand("", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("St", (object)St.ToString());
			cmd.Parameters.AddWithValue("Desc", (object)Text.FixString(Desccription));
			cmd.Parameters.AddWithValue("GharardadId", (object)GharardadId);
			cmd.Parameters.AddWithValue("DateM", (object)DateM);
			cmd.Parameters.AddWithValue("DateSh", (object)Text.FixString(DateSh));
			cmd.Parameters.AddWithValue("RestMission", (object)RestMission);
			cmd.Parameters.AddWithValue("RewardMab", (object)RewardMab);
			cmd.Parameters.AddWithValue("RewardFormula", (object)Text.FixString(RewardFormula));
			cmd.Parameters.AddWithValue("SeverancepayMab", (object)SeverancepayMab);
			cmd.Parameters.AddWithValue("SeverancepayFormula", (object)Text.FixString(SeverancepayFormula));
			cmd.Parameters.AddWithValue("OtherBenefitMab", (object)OtherBenefitMab);
			cmd.Parameters.AddWithValue("OtherBenefitDesc", (object)Text.FixString(OtherBenefitDesc));
			cmd.Parameters.AddWithValue("OtherDeficitMab", (object)OtherDeficitMab);
			cmd.Parameters.AddWithValue("OtherDeficitDesc", (object)Text.FixString(OtherDeficitDesc));
			await ((DbConnection)(object)cn).OpenAsync(token);
			if (Id <= 0)
			{
				((DbCommand)(object)cmd).CommandText = "dbo.sp_Checkout_Insert";
				object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
				if (obj != null)
				{
					Id = Utilities.ParseToInt(obj.ToString(), 0);
				}
			}
			else
			{
				((DbCommand)(object)cmd).CommandText = "dbo.sp_Checkout_Update";
				cmd.Parameters.AddWithValue("Id", (object)Id);
				await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			}
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand cmd = new SqlCommand("[dbo].[sp_Checkout_DeleteRow]", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("Id", (object)Id);
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

	public static async Task<List<Checkout>> SelectAllAsync(string contextGuid, CancellationToken token)
	{
		_ = 1;
		try
		{
			List<Checkout> ret = new List<Checkout>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand cmd = new SqlCommand("[sp_Checkout_SelectAll]", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlDataReader dr = cmd.ExecuteReader();
			while (await ((DbDataReader)(object)dr).ReadAsync(token))
			{
				ret.Add(new Checkout(contextGuid, dr));
			}
			((DbConnection)(object)cn).Close();
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ComputingAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			await CalcMissionAsync(token);
			if (string.IsNullOrEmpty(RewardFormula))
			{
				SetDefaultFormuleForReward();
			}
			if (string.IsNullOrEmpty(SeverancepayFormula))
			{
				SetDefaultFormuleForSeverancepay();
			}
			await CalculateFormulaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ComputingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Checkout.cs", 295);
		}
		return result;
	}

	public async Task CalculateFormulaAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			string contextGuid = ContextGuid;
			await FormulaList.GetAsync(contextGuid, null, await GetGharardadAsync(token), token);
			FormulaClass formulaClass = new FormulaClass(RewardFormula, 1, RewardFormula);
			if (RewardMab != 0m)
			{
				formulaClass.Value = RewardMab;
			}
			else
			{
				RewardMab = formulaClass.Value;
			}
			FormulaClass formulaClass2 = new FormulaClass(SeverancepayFormula, 1, RewardFormula);
			if (SeverancepayMab != 0m)
			{
				formulaClass2.Value = SeverancepayMab;
			}
			else
			{
				SeverancepayMab = formulaClass2.Value;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task CalcMissionAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> tuple = await Morakhasidaily.CalcLeaveBalanceAsync(ContextGuid, _gharardad, token);
			if (_gharardad == null)
			{
				_gharardad = tuple.Item4;
			}
			double num = tuple.Item3 * 60.0;
			double num2 = num * (double)(await GetGharardadAsync(token)).LeaveMinutes;
			RestMission = (int)num2;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalcMissionAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Checkout.cs", 336);
		}
	}

	private void SetDefaultFormuleForSeverancepay()
	{
		SeverancepayFormula = " [میزان حقوق]  * 30";
	}

	private void SetDefaultFormuleForReward()
	{
		RewardFormula = " [میزان حقوق]  * 60";
	}

	public ReturnedSaveFuncInfo CheckValidation()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Desccription))
			{
				val.AddReturnedValue((ReturnedState)2, "توضیحات نمیتواند خالی باشد", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Checkout.cs", 359);
			}
			if (GharardadId <= 0)
			{
				val.AddReturnedValue((ReturnedState)2, "قرارداد نمیتواند خالی باشد", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Checkout.cs", 361);
			}
			return val;
		}
		catch (Exception ex)
		{
			val.AddReturnedValue((ReturnedState)2, ex.Message, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Checkout.cs", 366);
			return val;
		}
	}
}
