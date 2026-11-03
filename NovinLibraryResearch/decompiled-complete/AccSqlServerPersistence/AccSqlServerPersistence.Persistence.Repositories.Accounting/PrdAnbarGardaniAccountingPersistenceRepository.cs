using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Records.AnbarGardani;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class PrdAnbarGardaniAccountingPersistenceRepository : PrdAnbarGardaniPersistenceRepositoryBase
{
	private BarcodePersistenceRepository _barcodeRepo;

	public PrdAnbarGardaniAccountingPersistenceRepository(ContextHolder db)
		: base(db, new PrdAnbarGardaniDetAccountingPersistenceRepository(db, new PrdSerialPersistanceRepository(db)))
	{
		_barcodeRepo = new BarcodePersistenceRepository(db);
	}

	public override async Task<List<PrdAnbarGardaniDetBusiness>> GetAllAsync(PrdAnbarGardaniBusiness master, AnbarGardaniFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs"))
		{
			List<PrdAnbarGardaniDetBusiness> list = new List<PrdAnbarGardaniDetBusiness>();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 32));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				List<PrdAnbarGardaniDetBusiness> result;
				try
				{
					IQueryable<Barcode> listSecondaryBarcods = (IQueryable<Barcode>)c.Barcodes;
					List<SqlParameter> list2 = new List<SqlParameter>
					{
						new SqlParameter("@DateM1", (object)filter.DateFrom),
						new SqlParameter("@MasterGuid", (object)(master?.Guid ?? Guid.Empty))
					};
					if (filter.Type.HasValue)
					{
						list2.Add(new SqlParameter("@Type", (object)filter.Type));
					}
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_ProductsAnbarGardani_Details", list2, tranName, token, CommandType.StoredProcedure, 300, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							List<PrdAnbarGardaniDetBusiness> list3 = list;
							list3.Add(await LoadDataAsync(master, dr, token, tranName, listSecondaryBarcods?.Any() ?? false));
							continue;
						}
						dr.Close();
						if (token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						if ((filter?.DepotGuid ?? Guid.Empty) != Guid.Empty)
						{
							list = list?.Where((PrdAnbarGardaniDetBusiness q) => q.DptGuid == filter.DepotGuid.Value)?.ToList();
						}
						goto end_IL_0104;
					}
				}
				catch (SqlException val)
				{
					SqlException val2 = val;
					if (!((Exception)(object)val2).Message.Contains("By User"))
					{
						WebErrorLog.Instance.StartLog((Exception)(object)val2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 58);
					}
					goto end_IL_0104;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 64);
					throw;
				}
				return result;
				end_IL_0104:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<List<PrdAnbarGardaniReportBusiness>> GetAllAsync(AnbarGardaniFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 76));
		List<PrdAnbarGardaniReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<PrdAnbarGardani> queryable = (IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani;
				if (!string.IsNullOrEmpty(filter.Description))
				{
					foreach (string word in filter.Description?.FixString()?.SplitString())
					{
						if (!string.IsNullOrEmpty(word))
						{
							queryable = queryable?.Where((PrdAnbarGardani p) => p.Description.Contains(word) || p.Number == word.ParseToInt());
						}
					}
				}
				if (filter.DateFrom.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => q.DateM >= filter.DateFrom);
				}
				if (filter.DateTo.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => q.DateM <= filter.DateTo);
				}
				if (filter.Type.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => (int?)q.Type == (int?)filter.Type);
				}
				IQueryable<PrdAnbarGardaniReportBusiness> queryable2 = (queryable?.OrderBy((PrdAnbarGardani x) => x.DateM))?.Select((PrdAnbarGardani q) => new PrdAnbarGardaniReportBusiness
				{
					DateM = q.DateM,
					Description = q.Description,
					EzafeSum = q.EzafeSum,
					Guid = q.Guid,
					KasrSum = q.KasrSum,
					Number = q.Number,
					SanadNo = q.SanadNo,
					ServerStatus = q.ServerStatus,
					UserName = q.CashierItem.Name,
					StatusId = q.StatusId,
					Type = q.Type
				});
				List<PrdAnbarGardaniReportBusiness> list = ((!token.IsCancellationRequested) ? (await ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<PrdAnbarGardaniReportBusiness>(queryable2, token) : null)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 113);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<PrdAnbarGardaniReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 124));
		PrdAnbarGardaniReportBusiness result;
		try
		{
			_ = 1;
			try
			{
				PrdAnbarGardani prdAnbarGardani = await c.PrdAnbarGardani.FindAsync(new object[1] { guid });
				if (prdAnbarGardani == null)
				{
					result = null;
				}
				else
				{
					PrdAnbarGardaniReportBusiness prdAnbarGardaniReportBusiness = new PrdAnbarGardaniReportBusiness
					{
						DateM = prdAnbarGardani.DateM,
						Description = prdAnbarGardani.Description,
						EzafeSum = prdAnbarGardani.EzafeSum,
						Guid = prdAnbarGardani.Guid,
						KasrSum = prdAnbarGardani.KasrSum,
						Number = prdAnbarGardani.Number,
						SanadNo = prdAnbarGardani.SanadNo,
						ServerStatus = prdAnbarGardani.ServerStatus,
						UserName = (prdAnbarGardani.CashierItem?.Name ?? ""),
						StatusId = prdAnbarGardani.StatusId,
						Type = prdAnbarGardani.Type
					};
					result = prdAnbarGardaniReportBusiness;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 149);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<List<AnbarGardaniEkhtetamRecord>> GetAllEkhtetamRecordsAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllEkhtetamRecordsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 160);
		List<AnbarGardaniEkhtetamRecord> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<AnbarGardaniEkhtetamRecord> list = new List<AnbarGardaniEkhtetamRecord>();
				short valueOrDefault = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DigitAfterPoint", (object)valueOrDefault)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_AnbarGardani_GetEkhtetamRecords", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllEkhtetamRecordsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadData(dr));
						continue;
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllEkhtetamRecordsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 178);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllEkhtetamRecordsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs");
			}
		}
		return result;
	}

	private async Task<PrdAnbarGardaniDetBusiness> LoadDataAsync(PrdAnbarGardaniBusiness master, DbDataReader dr, CancellationToken token, string tranName, bool loadSecondaryBarods)
	{
		try
		{
			PrdAnbarGardaniDetBusiness res = PrdAnbarGardaniDetBusiness.GetFactory(ContextGuid, master);
			res.Guid = Guid.NewGuid();
			res.ContextGuid = ContextGuid;
			if (dr["ISAnbargardani"] != DBNull.Value)
			{
				res.Status = (bool)dr["ISAnbargardani"];
			}
			res.MasterGuid = master?.Guid ?? Guid.Empty;
			if (dr["PRDGuid"] != DBNull.Value)
			{
				res.PrdGuid = (Guid)dr["PRDGuid"];
			}
			res.Modified = DateTime.Now;
			if (dr["UnitPrice"] != DBNull.Value)
			{
				res.UnitPrice = (decimal)dr["UnitPrice"];
			}
			if (dr["DPTGuid"] != DBNull.Value)
			{
				res.DptGuid = (Guid)dr["DPTGuid"];
			}
			if (dr["amount"] != DBNull.Value)
			{
				res.Amount = (decimal)(double)dr["amount"];
			}
			if (dr["RealAmount"] != DBNull.Value)
			{
				res.QtRealAmount = (decimal)(double)dr["RealAmount"];
			}
			if (dr["Ezafe"] != DBNull.Value)
			{
				res.QtEzafe = (decimal)(double)dr["Ezafe"];
			}
			if (dr["Kasr"] != DBNull.Value)
			{
				res.QtKasr = (decimal)(double)dr["Kasr"];
			}
			if (dr["PRDCode"] != DBNull.Value)
			{
				res.PrdCode = dr["PRDCode"].ToString();
			}
			if (dr["DPTName"] != DBNull.Value)
			{
				res.PrdDepotName = dr["DPTName"].ToString();
			}
			if (dr["GroupName"] != DBNull.Value)
			{
				res.PrdGroupName = dr["GroupName"].ToString();
			}
			if (dr["PRDName"] != DBNull.Value)
			{
				res.PrdName = dr["PRDName"].ToString();
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				res.PrdVahedName = dr["VahedName"].ToString();
			}
			if (dr["Guid"] != DBNull.Value)
			{
				res.Guid = (Guid)dr["Guid"];
			}
			if (dr["LastAnbarGardaniDate"] != DBNull.Value)
			{
				res.LastAnbarGardaniDate = (DateTime)dr["LastAnbarGardaniDate"];
			}
			else
			{
				res.Guid = Guid.NewGuid();
			}
			if (loadSecondaryBarods && dr["SecondaryBarcodeCount"].ToString().ParseToInt() != 0)
			{
				PrdAnbarGardaniDetBusiness prdAnbarGardaniDetBusiness = res;
				prdAnbarGardaniDetBusiness.PrdBarcodeList = await _barcodeRepo.GetByPrdGuidAsync(new List<Guid> { res.PrdGuid }, token, tranName);
			}
			if (dr["FirstSanadDate"] != DBNull.Value)
			{
				res.FirstSanadDate = (DateTime)dr["FirstSanadDate"];
			}
			if (dr["LastSanadDate"] != DBNull.Value)
			{
				res.LastSanadDate = (DateTime)dr["LastSanadDate"];
			}
			return res;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 223);
			throw;
		}
	}

	private AnbarGardaniEkhtetamRecord LoadData(DbDataReader dr)
	{
		try
		{
			AnbarGardaniEkhtetamRecord anbarGardaniEkhtetamRecord = new AnbarGardaniEkhtetamRecord();
			if (dr["Amount"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.Amount = (decimal)dr["Amount"];
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["IdProduct"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.ProductCode = dr["IdProduct"].ToString();
			}
			if (dr["DepotName"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.DepotName = dr["DepotName"].ToString();
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["ProductName"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.ProductName = dr["ProductName"].ToString();
			}
			if (dr["UnitName"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.UnitName = dr["UnitName"].ToString();
			}
			if (dr["GuidGroup"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.GroupGuid = (Guid)dr["GuidGroup"];
			}
			if (dr["UnitPrice"] != DBNull.Value)
			{
				anbarGardaniEkhtetamRecord.UnitPrice = (decimal)dr["UnitPrice"];
			}
			return anbarGardaniEkhtetamRecord;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniAccountingPersistenceRepository.cs", 247);
			throw;
		}
	}
}
