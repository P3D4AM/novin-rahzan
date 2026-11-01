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
using PacketParser.Attendance.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.User;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class AgreedDeduction : IHasContext
{
	private List<AgreedDeductionDet> _agreedDeductionDets;

	private string _agreedDeductionDetsJson;

	private Person _clsPerson;

	private FishHesab _clsFishHesab;

	public string ContextGuid { get; set; }

	public int Id { get; set; }

	public int HesabId { get; set; }

	public int PersonId { get; set; }

	public DateTime DateCreatedM { get; set; } = DateTime.Now;


	public string PersonName { get; set; }

	public string HesabTitle { get; set; }

	public decimal AmountPayable { get; set; }

	public decimal MonthlyInstallment { get; set; }

	public string Description { get; set; } = "";


	[JsonIgnore]
	public List<AgreedDeductionDet> AgreedDeductionDets => _agreedDeductionDets ?? (_agreedDeductionDets = (string.IsNullOrEmpty(_agreedDeductionDetsJson) ? new List<AgreedDeductionDet>() : JsonConvert.DeserializeObject<List<AgreedDeductionDet>>(_agreedDeductionDetsJson)));

	public decimal Remined => AmountPayable - AgreedDeductionDets.Distinct(new AgreedDeductionDetComparer()).Sum((AgreedDeductionDet q) => q.Price);

	public async Task<bool> GetTaxableAsync(CancellationToken token)
	{
		try
		{
			return (await GetClsFishHesabAsync(token)).Taxable;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> GetInsuranceAsync(CancellationToken token)
	{
		try
		{
			return (await GetClsFishHesabAsync(token)).Insurance;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<Person> GetClsPersonAsync(CancellationToken token)
	{
		try
		{
			Person person = _clsPerson;
			if (person == null)
			{
				person = (_clsPerson = await Person.GetByPersonIdAsync(ContextGuid, PersonId, token));
			}
			return person;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<string> GetFullNameAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			Person obj = await GetClsPersonAsync(token);
			return (obj != null) ? ((PersonBase)obj).FullName : null;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<FishHesab> GetClsFishHesabAsync(CancellationToken token)
	{
		FishHesab fishHesab = _clsFishHesab;
		if (fishHesab == null)
		{
			fishHesab = (_clsFishHesab = await FishHesab.GetAsync(ContextGuid, HesabId, token));
		}
		return fishHesab;
	}

	public async Task<string> GetHesabTitleAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			return (await GetClsFishHesabAsync(token))?.Name;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public AgreedDeduction(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<AgreedDeduction> GetAsync(string contextGuid, int id, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			AgreedDeduction ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_AgreedDeduction_SelectRow", cn);
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

	private static AgreedDeduction LoadData(string contextGuid, SqlDataReader dr)
	{
		try
		{
			AgreedDeduction agreedDeduction = new AgreedDeduction(contextGuid);
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction.Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["HesabId"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction.HesabId = (int)((DbDataReader)(object)dr)["HesabId"];
			}
			if (((DbDataReader)(object)dr)["PersonId"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction.PersonId = (int)((DbDataReader)(object)dr)["PersonId"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["DateCreatedM"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction.DateCreatedM = (DateTime)((DbDataReader)(object)dr)["DateCreatedM"];
			}
			if (((DbDataReader)(object)dr)["AmountPayable"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction.AmountPayable = (decimal)((DbDataReader)(object)dr)["AmountPayable"];
			}
			if (((DbDataReader)(object)dr)["MonthlyInstallment"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction.MonthlyInstallment = (decimal)((DbDataReader)(object)dr)["MonthlyInstallment"];
			}
			if (((DbDataReader)(object)dr)["AgreedDeductionDetsJson"].ToString() != DBNull.Value.ToString())
			{
				agreedDeduction._agreedDeductionDetsJson = ((DbDataReader)(object)dr)["AgreedDeductionDetsJson"].ToString();
			}
			return agreedDeduction;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 7;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("HesabId", (object)HesabId);
					cmd.Parameters.AddWithValue("PersonId", (object)PersonId);
					cmd.Parameters.AddWithValue("Description", (object)Description);
					cmd.Parameters.AddWithValue("DateCreatedM", (object)DateCreatedM);
					cmd.Parameters.AddWithValue("AmountPayable", (object)AmountPayable);
					cmd.Parameters.AddWithValue("MonthlyInstallment", (object)MonthlyInstallment);
					_agreedDeductionDetsJson = JsonConvert.SerializeObject((object)AgreedDeductionDets);
					cmd.Parameters.AddWithValue("AgreedDeductionDetsJson", (object)_agreedDeductionDetsJson);
					await ((DbConnection)(object)cn).OpenAsync(token);
					string fullName = await GetFullNameAsync(token);
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_AgreedDeduction_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							Id = Convert.ToInt16(obj.ToString());
						}
						string text = fullName;
						string text2 = await GetHesabTitleAsync(token);
						string pDesc = "  [افزودن کسورات توافقی] : " + text + " [و شرح حساب ] : " + text2 + " [مبلغ] " + AmountPayable.ToString("N0") + " [و مبلغ هر قسط] " + MonthlyInstallment.ToString("N0") + " [و تاریخ شروع] " + Calendar.MiladiToCalture(DateCreatedM);
						await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc, "AgreedDeduction.AgreedDeductionInsert", Id.ToString(), JsonConvert.SerializeObject((object)this));
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_AgreedDeduction_Update";
						cmd.Parameters.AddWithValue("Id", (object)Id);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
						string text = fullName;
						string text2 = await GetHesabTitleAsync(token);
						string pDesc2 = "  [ویرایش کسورات توافقی ] : " + text + " [و شرح حساب ] : " + text2 + " [مبلغ] " + AmountPayable.ToString("N0") + " [و مبلغ هر قسط] " + MonthlyInstallment.ToString("N0") + " [و تاریخ شروع] " + Calendar.MiladiToCalture(DateCreatedM);
						await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc2, "AgreedDeduction.AgreedDeductionUpdate", Id.ToString(), JsonConvert.SerializeObject((object)this));
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

	public async Task<bool> DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[dbo].[sp_AgreedDeduction_DeleteRow]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Id", (object)Id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					string text = await GetFullNameAsync(token);
					string text2 = await GetHesabTitleAsync(token);
					string pDesc = "[حذف کسورات توافقی] : " + text + " [و شرح حساب ] : " + text2 + " [مبلغ] " + AmountPayable.ToString("N0") + " [و مبلغ هر قسط] " + MonthlyInstallment.ToString("N0") + " [و تاریخ شروع] " + Calendar.MiladiToCalture(DateCreatedM);
					await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc, "AgreedDeduction.AgreedDeductionDelete", Id.ToString(), JsonConvert.SerializeObject((object)this));
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

	public static async Task<List<AgreedDeduction>> GetAllAsync(string contextGuid, string personIDs, DateTime startDate, DateTime endDate, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<AgreedDeduction> ret = new List<AgreedDeduction>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_AgreedDeduction_SelectAll]", cn);
				try
				{
					cmd.Parameters.AddWithValue("personIDs", (object)personIDs);
					cmd.Parameters.AddWithValue("startDate", (object)startDate);
					cmd.Parameters.AddWithValue("endDate", (object)endDate);
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

	public static async Task<List<AgreedDeduction>> GetAllRemnantAsync(string contextGuid, string personIDs, DateTime startDate, DateTime endDate, CancellationToken token)
	{
		return (await GetAllAsync(contextGuid, personIDs, startDate, endDate, token)).Where((AgreedDeduction q) => q.Remined > 0m).ToList();
	}
}
