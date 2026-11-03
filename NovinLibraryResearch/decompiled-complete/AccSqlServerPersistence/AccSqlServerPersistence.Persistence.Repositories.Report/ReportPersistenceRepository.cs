using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using EntityCache.Business.Report;
using EntityCache.Models.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.Entities.DepartmentEntitySummery.Entities;
using PacketParser.Filters;
using PacketParser.Records;
using PacketParser.Records.Kardex;
using PacketParser.Records.OccasionCalendar;

namespace AccSqlServerPersistence.Persistence.Repositories.Report;

internal class ReportPersistenceRepository : ReportPersistenceRepositoryBase
{
	public ReportPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<IEnumerable<AccountTreeModel>> GetAllAccountTreeAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAccountTreeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 30);
		IEnumerable<AccountTreeModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<AccountTreeModel> list = new List<AccountTreeModel>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_AccountTree", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllAccountTreeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataAccountTree(dr));
				}
				dr.Close();
				result = list;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAccountTreeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 44);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAccountTreeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<List<TarazHesabBusiness>> GetAllTarazHesabAsync(CancellationToken token, TarazHesabFilter filter, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetAllTarazHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 55);
		List<TarazHesabBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<TarazHesabBusiness> list = new List<TarazHesabBusiness>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@TarazType", (object)filter.Type),
					new SqlParameter("@Date1", (object)filter.Date1),
					new SqlParameter("@Date2", (object)filter.Date2),
					new SqlParameter("@CodeKol1", (object)filter.KolCode1),
					new SqlParameter("@CodeKol2", (object)filter.KolCode2),
					new SqlParameter("@CodeMoein1", (object)filter.MoeinCode1),
					new SqlParameter("@CodeMoein2", (object)filter.MoeinCode2),
					new SqlParameter("@CodeTafsil1", (object)filter.TafsilCode1),
					new SqlParameter("@CodeTafsil2", (object)filter.TafsilCode2),
					new SqlParameter("@SanadNumberRefrencedId", (object)filter.SanadNumberRefrencedId),
					new SqlParameter("@SanadTypesRefrencedId", (object)filter.SanadTypesRefrencedId)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_RPT_Hesab_Taraz", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllTarazHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync())
					{
						if (token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadData(dr, filter.Type));
						continue;
					}
					dr.Close();
					result = list;
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllTarazHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 86);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllTarazHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<SumKolBusiness>> GetSumKolListAsync(SumKolFilter filter, CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetSumKolListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 97);
		IEnumerable<SumKolBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SumKolBusiness> list = new List<SumKolBusiness>();
				List<SqlParameter> list2 = null;
				if (filter != null)
				{
					list2 = new List<SqlParameter>
					{
						new SqlParameter("@CodeKol1", (object)filter.CodeFrom),
						new SqlParameter("@CodeKol2", (object)filter.CodeTo)
					};
					if (filter.DateFrom.HasValue)
					{
						list2.Add(new SqlParameter("@dateM1", (object)filter.DateFrom));
					}
					if (filter.DateTo.HasValue)
					{
						list2.Add(new SqlParameter("@dateM2", (object)filter.DateTo));
					}
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_SumDaftarKol", list2, tranName, token, CommandType.StoredProcedure, 30, "GetSumKolListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
				if (token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					while (true)
					{
						if (await dr.ReadAsync())
						{
							if (token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							list.Add(LoadDataSumKol(dr));
							continue;
						}
						result = list;
						break;
					}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetSumKolListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 125);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSumKolListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<WebDepartmentEntitySummery> GetDepartmentEntitySummeryAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetDepartmentEntitySummeryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 136);
		WebDepartmentEntitySummery result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				WebDepartmentEntitySummery item = new WebDepartmentEntitySummery();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_DepartmentEntitySummery", null, tranName, token, CommandType.StoredProcedure, 30, "GetDepartmentEntitySummeryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					item = LoadDataDepartmentEntitySummery(dr);
				}
				dr.Close();
				result = item;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetDepartmentEntitySummeryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 149);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetDepartmentEntitySummeryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<List<OccasionCalendarRecord>> GetOccasionCalendarListAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetOccasionCalendarListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 160);
		List<OccasionCalendarRecord> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<OccasionCalendarRecord> list = new List<OccasionCalendarRecord>();
				string commandText = "select Value,EntityGuid from Global_MetaData where EntityType=4 and [Key]=18 and ISNULL(Value,'')<>''";
				using DbDataReader dr = await c.ExecuteReaderAsync(commandText, null, tranName, token, CommandType.Text, 30, "GetOccasionCalendarListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
				while (await dr.ReadAsync(token))
				{
					List<OccasionCalendarRecord> list2 = LoadOccasionCalendar(dr);
					if (list2 != null && list2.Any())
					{
						list.AddRange(list2);
					}
				}
				result = list;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetOccasionCalendarListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 177);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetOccasionCalendarListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<List<ServerStatusReportModel>> GetServerStatusAsync(ENSource source, List<ServerStatusForClient> serverStatuses, CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 188);
		List<ServerStatusReportModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<ServerStatusReportModel> itemlst = new List<ServerStatusReportModel>();
				IEnumerable<int> lst = serverStatuses.Select((ServerStatusForClient q) => (int)q);
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, lst, token, tranName);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					result = null;
				}
				else
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@RefrenceId", (object)returnedSaveFuncInfoWithValue.value),
						new SqlParameter("@Source", (object)(short)source)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_GetAllEntityNotSendToServer", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							itemlst.Add(LoadServerStatus(dr));
							continue;
						}
						result = itemlst;
						break;
					}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 212);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<KardexAndAccountConflictRecord>> GetAllKardexAndAccountConflictAsync(Guid moeinGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetAllKardexAndAccountConflictAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 223);
		IEnumerable<KardexAndAccountConflictRecord> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<KardexAndAccountConflictRecord> itemlst = new List<KardexAndAccountConflictRecord>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@MoeinGuid", (object)moeinGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Moghayerat_Kardex_and_Account", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllKardexAndAccountConflictAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						itemlst.Add(LoadKardexAndAccountConflictRecord(dr));
						continue;
					}
					result = itemlst;
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllKardexAndAccountConflictAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 240);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllKardexAndAccountConflictAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<DocumentRoundMismatchRecord>> GetAllDocumentRoundMismatchAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetAllDocumentRoundMismatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 251);
		IEnumerable<DocumentRoundMismatchRecord> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<DocumentRoundMismatchRecord> itemlst = new List<DocumentRoundMismatchRecord>();
				short valueOrDefault = (ClsCache.GetCache(base.ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DigitAfterPoint", (object)valueOrDefault)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_DocumentRoundMismatch", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllDocumentRoundMismatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						itemlst.Add(LoadDocumentRoundMismatchRecord(dr));
						continue;
					}
					result = itemlst?.OrderBy((DocumentRoundMismatchRecord q) => q.DateM);
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllDocumentRoundMismatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 269);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllDocumentRoundMismatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	private TarazHesabBusiness LoadData(DbDataReader dr, TarazType type)
	{
		try
		{
			TarazHesabBusiness tarazHesabBusiness = new TarazHesabBusiness(base.ContextGuid, type);
			if (dr["KolGuid"] != DBNull.Value)
			{
				tarazHesabBusiness.KolGuid = (Guid)dr["KolGuid"];
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				tarazHesabBusiness.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				tarazHesabBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["KolCode"] != DBNull.Value)
			{
				tarazHesabBusiness.KolCode = (int)dr["KolCode"];
			}
			if (dr["MoeinCode"] != DBNull.Value)
			{
				tarazHesabBusiness.MoeinCode = (long)dr["MoeinCode"];
			}
			if (dr["TafsilCode"] != DBNull.Value)
			{
				tarazHesabBusiness.TafsilCode = (long)dr["TafsilCode"];
			}
			if (dr["KolName"] != DBNull.Value)
			{
				tarazHesabBusiness.KolName = dr["KolName"].ToString();
			}
			else
			{
				tarazHesabBusiness.KolName = "";
			}
			if (dr["MoeinName"] != DBNull.Value)
			{
				tarazHesabBusiness.MoeinName = dr["MoeinName"].ToString();
			}
			else
			{
				tarazHesabBusiness.MoeinName = "";
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				tarazHesabBusiness.TafsilName = dr["TafsilName"].ToString();
			}
			else
			{
				tarazHesabBusiness.TafsilName = "";
			}
			if (dr["SD_Debit"] != DBNull.Value)
			{
				tarazHesabBusiness.SD_Debit = (decimal)dr["SD_Debit"];
			}
			if (dr["SD_Credit"] != DBNull.Value)
			{
				tarazHesabBusiness.SD_Credit = (decimal)dr["SD_Credit"];
			}
			if (dr["DD_Debit"] != DBNull.Value)
			{
				tarazHesabBusiness.DD_Debit = (decimal)dr["DD_Debit"];
			}
			if (dr["DD_Credit"] != DBNull.Value)
			{
				tarazHesabBusiness.DD_Credtit = (decimal)dr["DD_Credit"];
			}
			if (dr["ED_Debit"] != DBNull.Value)
			{
				tarazHesabBusiness.ED_Debit = (decimal)dr["ED_Debit"];
			}
			if (dr["ED_Credit"] != DBNull.Value)
			{
				tarazHesabBusiness.ED_Credit = (decimal)dr["ED_Credit"];
			}
			if (dr["IsHead"] != DBNull.Value)
			{
				tarazHesabBusiness.IsHead = (bool)dr["IsHead"];
			}
			return tarazHesabBusiness;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 306);
			throw;
		}
	}

	private AccountTreeModel LoadDataAccountTree(DbDataReader dr)
	{
		try
		{
			AccountTreeModel accountTreeModel = new AccountTreeModel();
			if (dr["Guid"] != DBNull.Value)
			{
				accountTreeModel.Guid = (Guid)dr["Guid"];
			}
			if (dr["MasterGuid"] != DBNull.Value)
			{
				accountTreeModel.MasterGuid = (Guid)dr["MasterGuid"];
			}
			if (dr["Name"] != DBNull.Value)
			{
				accountTreeModel.Name = dr["Name"].ToString();
			}
			if (dr["Code"] != DBNull.Value)
			{
				accountTreeModel.Code = (long)dr["Code"];
			}
			if (dr["Level"] != DBNull.Value)
			{
				accountTreeModel.Level = (ENHesabLevel)(int)dr["Level"];
			}
			if (dr["Account"] != DBNull.Value)
			{
				accountTreeModel.Account = (decimal)dr["Account"];
			}
			return accountTreeModel;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAccountTree", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 327);
			throw;
		}
	}

	private SumKolBusiness LoadDataSumKol(DbDataReader dr)
	{
		try
		{
			SumKolBusiness sumKolBusiness = new SumKolBusiness(base.ContextGuid);
			if (dr["KolGuid"] != DBNull.Value)
			{
				sumKolBusiness.KolGuid = (Guid)dr["KolGuid"];
			}
			if (dr["Debit"] != DBNull.Value)
			{
				sumKolBusiness.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				sumKolBusiness.Credit = (decimal)dr["Credit"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				sumKolBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Name"] != DBNull.Value)
			{
				sumKolBusiness.KolName = dr["Name"].ToString();
			}
			if (dr["Code"] != DBNull.Value)
			{
				sumKolBusiness.KolCode = (int)dr["Code"];
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				sumKolBusiness.SanadType = (SanadType)(int)dr["SanadType"];
			}
			return sumKolBusiness;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataSumKol", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 349);
			throw;
		}
	}

	private WebDepartmentEntitySummery LoadDataDepartmentEntitySummery(DbDataReader dr)
	{
		try
		{
			WebDepartmentEntitySummery webDepartmentEntitySummery = new WebDepartmentEntitySummery
			{
				IntroductionMethod = new WebIntroductionMethodSummery()
			};
			if (dr["AghsatMCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.AghsatM.Count = (int)dr["AghsatMCount"];
			}
			if (dr["AghsatShCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.AghsatSh.Count = (int)dr["AghsatShCount"];
			}
			if (dr["ChequeMCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ChequeM.Count = (int)dr["ChequeMCount"];
			}
			if (dr["ChequeShCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ChequeSh.Count = (int)dr["ChequeShCount"];
			}
			if (dr["CustomerCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Customer.Count = (int)dr["CustomerCount"];
			}
			if (dr["CustomerGroupCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.CustomerGroup.Count = (int)dr["CustomerGroupCount"];
			}
			if (dr["CustomerTitleCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.CustomerTitle.Count = (int)dr["CustomerTitleCount"];
			}
			if (dr["KolCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Kol.Count = (int)dr["KolCount"];
			}
			if (dr["MoeinCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Moein.Count = (int)dr["MoeinCount"];
			}
			if (dr["TafsilCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Tafsil.Count = (int)dr["TafsilCount"];
			}
			if (dr["TafsilGroupCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.TafsilGroup.Count = (int)dr["TafsilGroupCount"];
			}
			if (dr["LastPriceCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.LastPrice.Count = (int)dr["LastPriceCount"];
			}
			if (dr["UserCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.User.Count = (int)dr["UserCount"];
			}
			if (dr["ProductCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Product.Count = (int)dr["ProductCount"];
			}
			if (dr["ProductGroupCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ProductGroup.Count = (int)dr["ProductGroupCount"];
			}
			if (dr["ProductVahedCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ProductVahed.Count = (int)dr["ProductVahedCount"];
			}
			if (dr["FileInfoCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.FileInfo.Count = (int)dr["FileInfoCount"];
			}
			if (dr["IntroductionMethodCount"] != DBNull.Value)
			{
				webDepartmentEntitySummery.IntroductionMethod.Count = (int)dr["IntroductionMethodCount"];
			}
			if (dr["AghsatMMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.AghsatM.MinModified = (DateTime)dr["AghsatMMinModified"];
			}
			if (dr["AghsatShMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.AghsatSh.MinModified = (DateTime)dr["AghsatShMinModified"];
			}
			if (dr["ChequeMMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ChequeM.MinModified = (DateTime)dr["ChequeMMinModified"];
			}
			if (dr["ChequeShMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ChequeSh.MinModified = (DateTime)dr["ChequeShMinModified"];
			}
			if (dr["CustomerMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Customer.MinModified = (DateTime)dr["CustomerMinModified"];
			}
			if (dr["CustomerGroupMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.CustomerGroup.MinModified = (DateTime)dr["CustomerGroupMinModified"];
			}
			if (dr["CustomerTitleMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.CustomerTitle.MinModified = (DateTime)dr["CustomerTitleMinModified"];
			}
			if (dr["KolMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Kol.MinModified = (DateTime)dr["KolMinModified"];
			}
			if (dr["MoeinMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Moein.MinModified = (DateTime)dr["MoeinMinModified"];
			}
			if (dr["TafsilMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Tafsil.MinModified = (DateTime)dr["TafsilMinModified"];
			}
			if (dr["TafsilGroupMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.TafsilGroup.MinModified = (DateTime)dr["TafsilGroupMinModified"];
			}
			if (dr["LastPriceMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.LastPrice.MinModified = (DateTime)dr["LastPriceMinModified"];
			}
			if (dr["UserMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.User.MinModified = (DateTime)dr["UserMinModified"];
			}
			if (dr["ProductMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.Product.MinModified = (DateTime)dr["ProductMinModified"];
			}
			if (dr["ProductGroupMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ProductGroup.MinModified = (DateTime)dr["ProductGroupMinModified"];
			}
			if (dr["ProductVahedMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.ProductVahed.MinModified = (DateTime)dr["ProductVahedMinModified"];
			}
			if (dr["FileInfoMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.FileInfo.MinModified = (DateTime)dr["FileInfoMinModified"];
			}
			if (dr["IntroductionMethodMinModified"] != DBNull.Value)
			{
				webDepartmentEntitySummery.IntroductionMethod.MinModified = (DateTime)dr["IntroductionMethodMinModified"];
			}
			return webDepartmentEntitySummery;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataDepartmentEntitySummery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 403);
			throw;
		}
	}

	private List<OccasionCalendarRecord> LoadOccasionCalendar(DbDataReader dr)
	{
		try
		{
			if (dr["Value"] == DBNull.Value || dr["EntityGuid"] == DBNull.Value)
			{
				return null;
			}
			string text = dr["Value"].ToString();
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			List<OccasionCalendarRecord> result = new List<OccasionCalendarRecord>();
			List<OccasionCalendarModel> list = text.FromJson<List<OccasionCalendarModel>>("LoadOccasionCalendar", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 415);
			if (list != null && list.Any())
			{
				result = list.Select((OccasionCalendarModel q) => new OccasionCalendarRecord
				{
					Model = q,
					ParentGuid = (Guid)dr["EntityGuid"]
				})?.ToList();
			}
			return result;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadOccasionCalendar", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 428);
			throw;
		}
	}

	private ServerStatusReportModel LoadServerStatus(DbDataReader dr)
	{
		try
		{
			ServerStatusReportModel serverStatusReportModel = new ServerStatusReportModel();
			if (dr["AsGuid"] != DBNull.Value)
			{
				serverStatusReportModel.Guid = (Guid)dr["AsGuid"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				serverStatusReportModel.Date = (DateTime)dr["Date"];
			}
			if (dr["AsType"] != DBNull.Value)
			{
				serverStatusReportModel.Type = (EntityPart)(int)dr["AsType"];
			}
			if (dr["Id2"] != DBNull.Value)
			{
				serverStatusReportModel.Id2 = (long)dr["Id2"];
			}
			if (dr["TotalSum"] != DBNull.Value)
			{
				serverStatusReportModel.TotalSum = (decimal)dr["TotalSum"];
			}
			if (dr["ServerStatus"] != DBNull.Value)
			{
				serverStatusReportModel.ServerStatus = (ServerStatusForClient)dr["ServerStatus"];
			}
			if (dr["DeliveryMessage"] != DBNull.Value)
			{
				serverStatusReportModel.DeliveryMessage = (string)dr["DeliveryMessage"];
			}
			if (dr["DeliveryShortMessage"] != DBNull.Value)
			{
				serverStatusReportModel.DeliveryShortMessage = (string)dr["DeliveryShortMessage"];
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				serverStatusReportModel.TafsilName = (string)dr["TafsilName"];
			}
			return serverStatusReportModel;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadServerStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 452);
			throw;
		}
	}

	private KardexAndAccountConflictRecord LoadKardexAndAccountConflictRecord(DbDataReader dr)
	{
		try
		{
			KardexAndAccountConflictRecord kardexAndAccountConflictRecord = new KardexAndAccountConflictRecord();
			if (dr["Date"] != DBNull.Value)
			{
				kardexAndAccountConflictRecord.DateM = (DateTime)dr["Date"];
			}
			if (dr["Account"] != DBNull.Value)
			{
				kardexAndAccountConflictRecord.Account = (decimal)dr["Account"];
			}
			if (dr["KardexAccount"] != DBNull.Value)
			{
				kardexAndAccountConflictRecord.KardexAccount = (decimal)dr["KardexAccount"];
			}
			if (dr["Moghayerat"] != DBNull.Value)
			{
				kardexAndAccountConflictRecord.Conflict = (decimal)dr["Moghayerat"];
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				kardexAndAccountConflictRecord.TafsilName = (string)dr["TafsilName"];
			}
			return kardexAndAccountConflictRecord;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadKardexAndAccountConflictRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 472);
			throw;
		}
	}

	private DocumentRoundMismatchRecord LoadDocumentRoundMismatchRecord(DbDataReader dr)
	{
		try
		{
			DocumentRoundMismatchRecord documentRoundMismatchRecord = new DocumentRoundMismatchRecord();
			if (dr["DateM"] != DBNull.Value)
			{
				documentRoundMismatchRecord.DateM = (DateTime)dr["DateM"];
			}
			if (dr["SanadGuid"] != DBNull.Value)
			{
				documentRoundMismatchRecord.AsGuid = (Guid)dr["SanadGuid"];
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				documentRoundMismatchRecord.AsType = (Document)dr["SanadType"];
			}
			if (dr["Id2"] != DBNull.Value)
			{
				documentRoundMismatchRecord.Id2 = (int)dr["Id2"];
			}
			return documentRoundMismatchRecord;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDocumentRoundMismatchRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepository.cs", 491);
			throw;
		}
	}
}
