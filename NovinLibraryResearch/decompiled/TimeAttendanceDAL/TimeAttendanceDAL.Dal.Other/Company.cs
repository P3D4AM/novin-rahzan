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
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.Other;

[Serializable]
[DoNotPruneType]
public class Company : IAttCompany, IHasContext
{
	private static Company _companyDefault;

	public short Id { get; private set; }

	public string Code { get; set; } = "";


	public string Name { get; set; } = "";


	public string NameKarfarma { get; set; } = "";


	public string Address { get; set; } = "";


	public int NerkhBimeh { get; set; }

	public string NoPeyman { get; set; } = "";


	public string InsuranceBranch { get; set; }

	public string NameWorkShop { get; set; }

	public string AddressWorkShop { get; set; }

	public string ContextGuid { get; set; }

	public static Company CompanyDefault(string contextGuid)
	{
		return _companyDefault ?? (_companyDefault = new Company());
	}

	public static Company Reset()
	{
		return _companyDefault = null;
	}

	public static async Task<Company> GetAsync(string contextGuid, short id, CancellationToken token = default(CancellationToken))
	{
		Company ret = null;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Company_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)id);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					if (id == 0)
					{
						((DbCommand)(object)cmd).CommandText = "Select Top(1) * from [dbo].[Company] order by id";
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						((DbCommand)(object)cmd).CommandType = CommandType.Text;
					}
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(dr, contextGuid);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 88);
			throw;
		}
	}

	private static Company LoadData(SqlDataReader dr, string contextGuid)
	{
		Company company = new Company();
		try
		{
			company.ContextGuid = contextGuid;
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				company.Id = (short)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Code"].ToString() != DBNull.Value.ToString())
			{
				company.Code = ((DbDataReader)(object)dr)["Code"].ToString();
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				company.Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["NameKarfarma"].ToString() != DBNull.Value.ToString())
			{
				company.NameKarfarma = ((DbDataReader)(object)dr)["NameKarfarma"].ToString();
			}
			if (((DbDataReader)(object)dr)["Address"].ToString() != DBNull.Value.ToString())
			{
				company.Address = ((DbDataReader)(object)dr)["Address"].ToString();
			}
			if (((DbDataReader)(object)dr)["NerkhBimeh"].ToString() != DBNull.Value.ToString())
			{
				company.NerkhBimeh = (int)((DbDataReader)(object)dr)["NerkhBimeh"];
			}
			if (((DbDataReader)(object)dr)["NOPeyman"].ToString() != DBNull.Value.ToString())
			{
				company.NoPeyman = ((DbDataReader)(object)dr)["NOPeyman"].ToString();
			}
			if (((DbDataReader)(object)dr)["InsuranceBranch"].ToString() != DBNull.Value.ToString())
			{
				company.InsuranceBranch = ((DbDataReader)(object)dr)["InsuranceBranch"].ToString();
			}
			if (((DbDataReader)(object)dr)["NameWorkShop"].ToString() != DBNull.Value.ToString())
			{
				company.NameWorkShop = ((DbDataReader)(object)dr)["NameWorkShop"].ToString();
			}
			if (((DbDataReader)(object)dr)["AddressWorkShop"].ToString() != DBNull.Value.ToString())
			{
				company.AddressWorkShop = ((DbDataReader)(object)dr)["AddressWorkShop"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 113);
			throw;
		}
		return company;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 4;
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
					cmd.Parameters.AddWithValue("Code", (object)Text.FixString(Code));
					cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
					cmd.Parameters.AddWithValue("NameKarfarma", (object)Text.FixString(NameKarfarma));
					cmd.Parameters.AddWithValue("Address", (object)Text.FixString(Address));
					cmd.Parameters.AddWithValue("NerKhBimeh", (object)NerkhBimeh);
					cmd.Parameters.AddWithValue("NoPeyman", (object)Text.FixString(NoPeyman));
					cmd.Parameters.AddWithValue("InsuranceBranch", (object)Text.FixString(InsuranceBranch));
					cmd.Parameters.AddWithValue("NameWorkShop", (object)Text.FixString(NameWorkShop));
					cmd.Parameters.AddWithValue("AddressWorkShop", (object)Text.FixString(AddressWorkShop));
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_Company_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							Id = Convert.ToInt16(obj.ToString());
						}
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "افزودن اطلاعات یک شرکت جدید : " + Name + " و شماره کارگاه : " + Code, "Company.CompanyInsert", this, Id.ToString());
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_Company_Update";
						cmd.Parameters.AddWithValue("Id", (object)Id);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "ویرایش اطلاعات یک شرکت : " + Name + " و شماره کارگاه : " + Code, "Company.CompanyUpdate", this, Id.ToString());
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 159);
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 5;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Company_countUse_byCompanyId", cn);
				try
				{
					cmd.Parameters.AddWithValue("CompanyId", (object)Id);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						short countUsed = 0;
						if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["countUsed"].ToString() != DBNull.Value.ToString() && !string.IsNullOrEmpty(((DbDataReader)(object)dr)["countUsed"].ToString()))
						{
							countUsed = short.Parse(((DbDataReader)(object)dr)["countUsed"].ToString());
						}
						((DbConnection)(object)cn).Close();
						if (countUsed > 0)
						{
							TimeAttendanceDAL.Utility.Utility.MessageBoxShowError($"از نام این شرکت در  {countUsed} قرارداد استفاده شده لذا امکان حذف شرکت وجود ندارد", EntityCacheConfig.CacheInstance.ContextGuid);
							return false;
						}
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_Company_DeleteRow]";
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						cmd.Parameters.AddWithValue("Id", (object)Id);
						await ((DbConnection)(object)cn).OpenAsync(token);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
						((DbConnection)(object)cn).Close();
						string pDesc = "حذف اطلاعات یک شرکت " + Name + " و شماره کارگاه : " + Code;
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, pDesc, "Company.CompanyDelete", this, Id.ToString());
						return true;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 199);
		}
		return false;
	}

	public static async Task<List<Company>> SelectAllAsync(string contextGuid, string extItem = "", CancellationToken token = default(CancellationToken))
	{
		List<Company> ret = new List<Company>();
		if (extItem != "")
		{
			Company company = new Company();
			company.Name = extItem;
			ret.Add(company);
		}
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Company_SelectAll]", cn);
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
							token.ThrowIfCancellationRequested();
							ret.Add(LoadData(dr, contextGuid));
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SelectAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 236);
			throw;
		}
	}

	public static async Task<bool> UniqeNoPeymanAsync(string contextGuid, string noPeyman, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select Id from [Company]  where NoPeyman=" + noPeyman, cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					return obj == null;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "UniqeNoPeymanAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 258);
			throw;
		}
	}

	public static async Task<bool> UniqeCodeAsync(string contextGuid, string code, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select Id from [Company]  where Code=" + code, cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					return obj == null;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "UniqeCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 279);
			throw;
		}
	}

	public static async Task<short> Code2IdAsync(string contextGuid, string code, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select Id from [Company] where Code=" + code, cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					return Utilities.ParseToShort(obj.ToString(), (short)0);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "Code2IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 298);
			throw;
		}
	}

	public static async Task<string> IdToCodeAsync(string contextGuid, short id, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select Code from [Company] where Id=" + id, cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					return obj?.ToString() ?? "";
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "IdToCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Company.cs", 316);
			throw;
		}
	}
}
