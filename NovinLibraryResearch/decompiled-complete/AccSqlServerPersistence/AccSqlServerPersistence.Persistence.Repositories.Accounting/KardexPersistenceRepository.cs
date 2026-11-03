using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.AssistanceClasses;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Records.Kardex;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class KardexPersistenceRepository : KardexPersistenceRepositoryBase
{
	internal KardexPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> ProgressRemAsync(Guid prdGuid, Guid dptGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new Exception("tranName is null or empty!!");
		}
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 30));
		using (new JaegerMonitor("ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_ = 1;
				try
				{
					ClsCache cache = ClsCache.GetCache(base.ContextGuid);
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@PrdGuid", (object)prdGuid),
						new SqlParameter("@DptGuid", (object)dptGuid),
						new SqlParameter("@DigitForRound", (object)cache.MultiLanguageConfig.DigitForRound),
						new SqlParameter("@DigitAfterPoint", (object)cache.MultiLanguageConfig.DigitAfterPoint)
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_acc_kardex_UpdatePrdocustWithOneDepot", parameters, tranName, token, "", CommandType.StoredProcedure, 300000, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"), ReturnedState.None, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 43);
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex, $"exec sp_acc_kardex_UpdatePrdocustWithOneDepot @PrdGuid='{prdGuid}',@DptGuid='{dptGuid}'\r\n" + $"ContextIsNull:{c == null}\r\n" + $"contextIsDisposed:{c?.IsDisposed ?? true}\r\n" + "tranName:" + tranName + "\r\ninnerException:" + ex?.InnerException?.Message + "\r\ncontext.ConnectionString:" + c.ConnectionString, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 48);
					ret.AddReturnedValue(ex, "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 54);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				}
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> FillAsync(Guid? sanadGuid, Document? sanadType, DateTime? newDate, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			Acc_ModelContext c = null;
			try
			{
				_ = 2;
				try
				{
					if (string.IsNullOrEmpty(tranName))
					{
						await AccSqlServerPersistence.AssistanceClasses.Locks.KardexRefreshSemaphore.WaitAsync();
					}
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 1800000, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 70));
					List<SqlParameter> list = new List<SqlParameter>();
					if (sanadGuid.HasValue)
					{
						list.Add(new SqlParameter("@SanadGuid", (object)sanadGuid));
					}
					if (sanadType.HasValue)
					{
						list.Add(new SqlParameter("@SanadType", (object)(short)sanadType.Value));
					}
					if (newDate.HasValue)
					{
						list.Add(new SqlParameter("@newDate", (object)newDate));
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Acc_Kardex_FillAll", list, tranName, token, "", CommandType.StoredProcedure, 1800, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"), ReturnedState.None, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 76);
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex, "", "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 81);
					ret.AddReturnedValue(ex, "", "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 82);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
					AccSqlServerPersistence.AssistanceClasses.Locks.KardexRefreshSemaphore.Release();
				}
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(KardexBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 103));
				Kardex temp = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				temp.Invalidated = true;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(temp, temp.ProductGuid, temp.DepotGuid, temp.DateM, temp.DetailGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 106);
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@prdGuid", (object)temp.ProductGuid),
					new SqlParameter("@depotGuid", (object)temp.DepotGuid),
					new SqlParameter("@dateM", (object)temp.DateM)
				};
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.ExecuteNoneQueryAsync("sp_Acc_Kardex_Update", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 113);
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 119);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 120);
			}
			return ret;
		}
	}

	public override async Task<List<KardexViewModel>> GetAllViewModelAsync(CancellationToken token, DateTime? date1, DateTime? date2, bool GardeshManfi, bool NoghteSegaresh, bool? kardexAffective, string tranName)
	{
		using (new JaegerMonitor("GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			List<KardexViewModel> list = new List<KardexViewModel>();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 128));
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				List<KardexViewModel> result;
				try
				{
					date1.GetValueOrDefault();
					if (!date1.HasValue)
					{
						DateTime value = new DateTime(1900, 1, 1);
						date1 = value;
					}
					date2.GetValueOrDefault();
					if (!date2.HasValue)
					{
						DateTime value = new DateTime(2099, 1, 1);
						date2 = value;
					}
					List<clsKardexAccess> kardexAccessList = new List<clsKardexAccess>();
					CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token);
					short digitForRound = (ClsCache.GetCache(base.ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
					if (cashierBusiness != null)
					{
						UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token, tranName);
						if (userAccBusiness != null)
						{
							kardexAccessList = userAccBusiness.UserAccess.KardexAccessList;
						}
					}
					List<SqlParameter> list2 = new List<SqlParameter>
					{
						new SqlParameter("@NoghteSefaresh", (object)NoghteSegaresh),
						new SqlParameter("@GardeshManfi", (object)GardeshManfi),
						new SqlParameter("@Date1", (object)date1),
						new SqlParameter("@Date2", (object)date2)
					};
					if (kardexAffective.HasValue)
					{
						list2.Add(new SqlParameter("@KardexAffective", (object)kardexAffective.Value));
					}
					using (DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Kardex_Sum", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
					{
						while (await dr.ReadAsync())
						{
							if (!token.IsCancellationRequested)
							{
								list.Add(LoadDataModel(dr, kardexAccessList, digitForRound));
								continue;
							}
							result = null;
							goto end_IL_010e;
						}
					}
					goto end_IL_00ef;
					end_IL_010e:;
				}
				catch (OperationCanceledException)
				{
					list = null;
					goto end_IL_00ef;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 161);
					throw;
				}
				return result;
				end_IL_00ef:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<List<KardexCustomerModel>> GetAllCustomerGardeshBetweenAsync(CancellationToken token, DateTime date1, DateTime date2, bool? kardexAffective, string tranName)
	{
		using (new JaegerMonitor("GetAllCustomerGardeshBetweenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			List<KardexCustomerModel> list = new List<KardexCustomerModel>();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllCustomerGardeshBetweenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 175));
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				List<KardexCustomerModel> result;
				try
				{
					ClsCache cache = ClsCache.GetCache(base.ContextGuid);
					List<SqlParameter> list2 = new List<SqlParameter>
					{
						new SqlParameter("@Date1", (object)date1),
						new SqlParameter("@Date2", (object)date2),
						new SqlParameter("@DigitForRound", (object)cache.MultiLanguageConfig.DigitForRound),
						new SqlParameter("@DigitAfterPoint", (object)cache.MultiLanguageConfig.DigitAfterPoint)
					};
					if (kardexAffective.HasValue)
					{
						list2.Add(new SqlParameter("@KardexAffective", (object)kardexAffective.Value));
					}
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_KardexCustomer_GardeshBetween", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllCustomerGardeshBetweenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
					while (true)
					{
						if (!(await dr.ReadAsync(token)))
						{
							dr.Close();
							break;
						}
						if (!token.IsCancellationRequested)
						{
							list.Add(LoadKardexCustomer(dr));
							continue;
						}
						result = null;
						goto end_IL_0000;
					}
					goto end_IL_00e7;
					end_IL_0000:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllCustomerGardeshBetweenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 199);
					throw;
				}
				return result;
				end_IL_00e7:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllCustomerGardeshBetweenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<List<KardexTajmieReportBusiness>> GetAllTajmieProductAsync(CancellationToken token, TajmieeProductFilter filter, string tranName)
	{
		using (new JaegerMonitor("GetAllTajmieProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			List<KardexTajmieReportBusiness> list = new List<KardexTajmieReportBusiness>();
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllTajmieProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 213);
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				List<KardexTajmieReportBusiness> result;
				try
				{
					List<SqlParameter> @params = new List<SqlParameter>
					{
						new SqlParameter("@discount1", (object)filter.Discount1),
						new SqlParameter("@discount2", (object)filter.Discount2),
						new SqlParameter("@sum1", (object)filter.Sum1),
						new SqlParameter("@sum2", (object)filter.Sum2),
						new SqlParameter("@price1", (object)filter.Price1),
						new SqlParameter("@price2", (object)filter.Price2),
						new SqlParameter("@visitorPrice1", (object)filter.VisitorPrice1),
						new SqlParameter("@visitorPrice2", (object)filter.VisitorPrice2),
						new SqlParameter("@number1", (object)filter.Number1),
						new SqlParameter("@number2", (object)filter.Number2),
						new SqlParameter("@tax1", (object)filter.Tax1),
						new SqlParameter("@tax2", (object)filter.Tax2),
						new SqlParameter("@avarez1", (object)filter.Avarez1),
						new SqlParameter("@avarez2", (object)filter.Avarez2),
						new SqlParameter("@Type", (object)(short)filter.Type)
					};
					if (filter.Date1.HasValue)
					{
						@params.Add(new SqlParameter("@Date1", (object)filter.Date1));
					}
					if (filter.Date1.HasValue)
					{
						@params.Add(new SqlParameter("@Date2", (object)filter.Date2));
					}
					if (!string.IsNullOrEmpty(filter.RowDescription) && filter.RowDescription != "%%")
					{
						@params.Add(new SqlParameter("@rowDesc", (object)filter.RowDescription));
					}
					if (!string.IsNullOrEmpty(filter.Description) && filter.Description != "%%")
					{
						@params.Add(new SqlParameter("@desc", (object)filter.Description));
					}
					if (filter.CashierGuid.HasValue && filter.CashierGuid != Guid.Empty)
					{
						@params.Add(new SqlParameter("@cashierGuid", (object)filter.CashierGuid));
					}
					if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
					{
						@params.Add(new SqlParameter("@tafsilGuid", (object)filter.TafsilGuid));
					}
					if (filter.CustomerSelectedListGuid.HasValue && filter.CustomerSelectedListGuid != Guid.Empty)
					{
						@params.Add(new SqlParameter("@selectedCusListGuid", (object)filter.CustomerSelectedListGuid));
					}
					if (filter.ProductSelectedListGuid.HasValue && filter.ProductSelectedListGuid != Guid.Empty)
					{
						@params.Add(new SqlParameter("@selectedPrdListGuid", (object)filter.ProductSelectedListGuid));
					}
					if (filter.CustomerGroupGuid.HasValue && filter.CustomerGroupGuid != Guid.Empty)
					{
						@params.Add(new SqlParameter("@cusGroupGuid", (object)filter.CustomerGroupGuid));
					}
					if (filter.VisitorGuid.HasValue && filter.VisitorGuid != Guid.Empty)
					{
						@params.Add(new SqlParameter("@visitorGuid", (object)filter.VisitorGuid));
					}
					if (!(filter.SelectedNumbers?.Any() ?? false))
					{
						goto IL_0751;
					}
					ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, filter.SelectedNumbers, token, tranName);
					if (!returnedSaveFuncInfoWithValue.HasError)
					{
						@params.Add(new SqlParameter("@refId", (object)returnedSaveFuncInfoWithValue.value));
						goto IL_0751;
					}
					result = list;
					goto end_IL_0109;
					IL_096d:
					using (DbDataReader dr = await c.ExecuteReaderAsync("sp_Kardex_GetAllTajmieeProduct", @params, tranName, token, CommandType.StoredProcedure, 30, "GetAllTajmieProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
					{
						while (await dr.ReadAsync(token))
						{
							if (!token.IsCancellationRequested)
							{
								list.Add(LoadKardexTajmieProduct(dr));
								continue;
							}
							result = null;
							goto end_IL_0109;
						}
					}
					goto end_IL_00ea;
					IL_0751:
					if (filter.KardexAffective.HasValue)
					{
						@params.Add(new SqlParameter("@KardexAffective", (object)filter.KardexAffective.Value));
					}
					if ((filter.DepotGuid ?? Guid.Empty) != Guid.Empty)
					{
						@params.Add(new SqlParameter("@DepotGuid", (object)filter.DepotGuid.Value));
					}
					if (!(filter?.ProductGroupGuidList?.Any()).GetValueOrDefault())
					{
						goto IL_096d;
					}
					ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue2 = await StimulQueryBusiness.SaveAsync(base.ContextGuid, filter.ProductGroupGuidList, token, tranName);
					if (!returnedSaveFuncInfoWithValue2.HasError)
					{
						@params.Add(new SqlParameter("@PrdGroupRefId", (object)returnedSaveFuncInfoWithValue2.value));
						goto IL_096d;
					}
					result = list;
					end_IL_0109:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllTajmieProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 270);
					throw;
				}
				return result;
				end_IL_00ea:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllTajmieProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<List<KardexTajmieReportBusiness>> GetAllGardeshProductAsync(DateTime date1, DateTime date2, bool? kardexAffective, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllGardeshProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			List<KardexTajmieReportBusiness> list = new List<KardexTajmieReportBusiness>();
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllGardeshProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 284);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					ClsCache cache = ClsCache.GetCache(base.ContextGuid);
					List<SqlParameter> list2 = new List<SqlParameter>
					{
						new SqlParameter("@Date1", (object)date1),
						new SqlParameter("@Date2", (object)date2),
						new SqlParameter("@DigitForRound", (object)cache.MultiLanguageConfig.DigitForRound),
						new SqlParameter("@DigitAfterPoint", (object)cache.MultiLanguageConfig.DigitAfterPoint)
					};
					if (kardexAffective.HasValue)
					{
						list2.Add(new SqlParameter("@KardexAffective", (object)kardexAffective.Value));
					}
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Kardex_RPT_GardeshBetween", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllGardeshProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
					while (await dr.ReadAsync(token))
					{
						list.Add(LoadKardexGaedeshProduct(dr));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllGardeshProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 304);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllGardeshProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<List<OrderMoghayeratModel>> GetAllOrderMoghayeratAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllOrderMoghayeratAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			List<OrderMoghayeratModel> list = new List<OrderMoghayeratModel>();
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllOrderMoghayeratAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 318);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Order_GetSellMoghayerat", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllOrderMoghayeratAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
					while (await dr.ReadAsync())
					{
						list.Add(LoadDataOrderMoghayerat(dr));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllOrderMoghayeratAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 329);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllOrderMoghayeratAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<decimal> LastVorodPriceAvgAsync(Guid prdGuid, DateTime? Date, Guid? depotGuid, bool? kardexAffective, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("LastVorodPriceAvgAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "LastVorodPriceAvgAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 342));
			decimal valueOrDefault;
			try
			{
				valueOrDefault = (((IQueryable<Kardex>)acc_ModelContext.Kardex).OrderByDescending((Kardex p) => p.DateM).FirstOrDefault((Kardex x) => x.ProductGuid == prdGuid && (Date == null || x.DateM < ((DateTime?)Date).Value) && (depotGuid == null || x.DepotGuid == ((Guid?)depotGuid).Value) && (kardexAffective == null || x.KardexAffective == ((bool?)kardexAffective).Value))?.RemUnitPrice).GetValueOrDefault();
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
				WebErrorLog.Instance.StartLog(ex2, "", "LastVorodPriceAvgAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 357);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("LastVorodPriceAvgAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				}
			}
			return valueOrDefault;
		}
	}

	public override async Task<double> GetPrdAmountAsync(Guid prdGuid, Guid depotGuid, DateTime date, bool? kardexAffective, CancellationToken token, string tranName)
	{
		double res = 0.0;
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 370);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@prdGuid", (object)prdGuid),
					new SqlParameter("@depotGuid", (object)depotGuid),
					new SqlParameter("@date", (object)date)
				};
				if (kardexAffective.HasValue)
				{
					list.Add(new SqlParameter("@KardexAffective", (object)kardexAffective.Value));
				}
				object obj = await c.ExecuteScalarAsync<object>("sp_Kardex_GetPrdAmount", list, tranName, token, 30, "GetPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				if (obj != null)
				{
					res = (double)obj;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 387);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
			}
		}
		return res;
	}

	public override async Task<IEnumerable<KardexLinkedOrder>> GetAllLinkSellAsync(Guid tafsilGuid, Guid productGuid, DateTime dateM, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllLinkSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 399);
		IEnumerable<KardexLinkedOrder> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<KardexLinkedOrder> list = new List<KardexLinkedOrder>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@TafsilGuid", (object)tafsilGuid),
					new SqlParameter("@ProductGuid", (object)productGuid),
					new SqlParameter("@DateM", (object)dateM)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Kardex_LinkSellToBackSell", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllLinkSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							list.Add(LoadDataLinkedModel(dr));
							continue;
						}
						result = list?.Where((KardexLinkedOrder q) => q.RemainQuantity > 0m);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllLinkSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 422);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllLinkSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<KardexConflictRecord>> GetAllConflictsAsync(MaliYearSanadType sanadType, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 433);
		IEnumerable<KardexConflictRecord> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<KardexConflictRecord> list = new List<KardexConflictRecord>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@MaliYearSanadType", (object)(short)sanadType)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Kardex_Conflict", parameters, tranName, token, CommandType.StoredProcedure, 600, "GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (token.IsCancellationRequested || base.IsDisposed)
						{
							result = null;
							break;
						}
						list.Add(LoadDataConflictRecord(dr));
						continue;
					}
					result = list?.OrderBy((KardexConflictRecord q) => q.DateM);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 450);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs");
			}
		}
		return result;
	}

	private OrderMoghayeratModel LoadDataOrderMoghayerat(DbDataReader dr)
	{
		OrderMoghayeratModel orderMoghayeratModel = new OrderMoghayeratModel();
		try
		{
			if (dr["ProductName"] != DBNull.Value)
			{
				orderMoghayeratModel.ProductName = dr["ProductName"].ToString();
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				orderMoghayeratModel.TafsilName = dr["TafsilName"].ToString();
			}
			if (dr["SellSum"] != DBNull.Value)
			{
				orderMoghayeratModel.SellSum = (decimal)dr["SellSum"];
			}
			if (dr["SellCount"] != DBNull.Value)
			{
				orderMoghayeratModel.SellCount = (int)dr["SellCount"];
			}
			if (dr["SellQuantity"] != DBNull.Value)
			{
				orderMoghayeratModel.SellQuantity = dr["SellQuantity"].ToString().ParseToDecimal();
			}
			if (dr["FirstSellDate"] != DBNull.Value)
			{
				orderMoghayeratModel.FirstSellDate = (DateTime)dr["FirstSellDate"];
			}
			if (dr["LastSellDate"] != DBNull.Value)
			{
				orderMoghayeratModel.LastSellDate = (DateTime)dr["LastSellDate"];
			}
			if (dr["BackSellSum"] != DBNull.Value)
			{
				orderMoghayeratModel.BackSellSum = (decimal)dr["BackSellSum"];
			}
			if (dr["BackSellCount"] != DBNull.Value)
			{
				orderMoghayeratModel.BackSellCount = (int)dr["BackSellCount"];
			}
			if (dr["BackSellQuantity"] != DBNull.Value)
			{
				orderMoghayeratModel.BackSellQuantity = dr["BackSellQuantity"].ToString().ParseToDecimal();
			}
			if (dr["FirstBackSell"] != DBNull.Value)
			{
				orderMoghayeratModel.FirstBackSell = (DateTime)dr["FirstBackSell"];
			}
			if (dr["LastBackSell"] != DBNull.Value)
			{
				orderMoghayeratModel.LastBackSell = (DateTime)dr["LastBackSell"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				orderMoghayeratModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			return orderMoghayeratModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataOrderMoghayerat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 485);
			throw;
		}
	}

	private KardexTajmieReportBusiness LoadKardexGaedeshProduct(DbDataReader dr)
	{
		KardexTajmieReportBusiness kardexTajmieReportBusiness = new KardexTajmieReportBusiness(base.ContextGuid);
		try
		{
			if (dr["PRDGuid"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ProductGuid = (Guid)dr["PRDGuid"];
			}
			if (dr["DPTGuid"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DepotGuid = (Guid)dr["DPTGuid"];
			}
			if (dr["SD_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Debit = (decimal)dr["SD_Debit"];
			}
			if (dr["SD_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Credit = (decimal)dr["SD_Credit"];
			}
			if (dr["SD_Sood"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Sood = (decimal)dr["SD_Sood"];
			}
			if (dr["SD_VorodQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_VorodQT = dr["SD_VorodQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_KhorojQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_KhorojQT = dr["SD_KhorojQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_BuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_BuyQT = dr["SD_BuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_BuyPrice"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_BuyPrice = (decimal)dr["SD_BuyPrice"];
			}
			if (dr["SD_SellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_SellQT = dr["SD_SellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_SellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_SellSum = (decimal)dr["SD_SellSum"];
			}
			if (dr["SD_BBuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_BBuyQT = dr["SD_BBuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_BBuySum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_BBuySum = (decimal)dr["SD_BBuySum"];
			}
			if (dr["SD_BSellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_BSellQT = dr["SD_BSellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_BSellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_BSellSum = (decimal)dr["SD_BSellSum"];
			}
			if (dr["SD_Tolid_in_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Tolid_in_QT = dr["SD_Tolid_in_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_Tolid_in_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Tolid_in_Sum = (decimal)dr["SD_Tolid_in_Sum"];
			}
			if (dr["SD_Tolid_Out_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Tolid_Out_QT = dr["SD_Tolid_Out_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_Tolid_Out_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Tolid_Out_Sum = (decimal)dr["SD_Tolid_Out_Sum"];
			}
			if (dr["SD_Zayeat_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Zayeat_QT = dr["SD_Zayeat_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SD_Zayeat_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Zayeat_Sum = (decimal)dr["SD_Zayeat_Sum"];
			}
			if (dr["SD_Discount_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Discount_Credit = (decimal)dr["SD_Discount_Credit"];
			}
			if (dr["SD_Discount_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Discount_Debit = (decimal)dr["SD_Discount_Debit"];
			}
			if (dr["SD_Keraye_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Keraye_Credit = (decimal)dr["SD_Keraye_Credit"];
			}
			if (dr["SD_Keraye_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Keraye_Debit = (decimal)dr["SD_Keraye_Debit"];
			}
			if (dr["SD_Bazaryab_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_Bazaryab_Credit = (decimal)dr["SD_Bazaryab_Credit"];
			}
			if (dr["SD_BazaryabDebit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SD_BazaryabDebit = (decimal)dr["SD_BazaryabDebit"];
			}
			if (dr["DD_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Debit = (decimal)dr["DD_Debit"];
			}
			if (dr["DD_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Credit = (decimal)dr["DD_Credit"];
			}
			if (dr["DD_Sood"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Sood = (decimal)dr["DD_Sood"];
			}
			if (dr["DD_VorodQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_VorodQT = dr["DD_VorodQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_KhorojQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_KhorojQT = dr["DD_KhorojQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_BuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BuyQt = dr["DD_BuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_BuyPrice"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BuySum = (decimal)dr["DD_BuyPrice"];
			}
			if (dr["DD_SellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SellQt = dr["DD_SellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_SellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SellSum = (decimal)dr["DD_SellSum"];
			}
			if (dr["DD_BBuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackBuyQt = dr["DD_BBuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_BBuySum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackBuySum = (decimal)dr["DD_BBuySum"];
			}
			if (dr["DD_BSellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackSellQt = dr["DD_BSellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_BSellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackSellSum = (decimal)dr["DD_BSellSum"];
			}
			if (dr["DD_Tolid_in_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidVorodQt = dr["DD_Tolid_in_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_Tolid_in_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidVorodSum = (decimal)dr["DD_Tolid_in_Sum"];
			}
			if (dr["DD_Tolid_Out_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidKhorojQt = dr["DD_Tolid_Out_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_Tolid_Out_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidKhorojSum = (decimal)dr["DD_Tolid_Out_Sum"];
			}
			if (dr["DD_Zayeat_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ZayeatQt = dr["DD_Zayeat_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DD_Zayeat_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ZayeatSum = (decimal)dr["DD_Zayeat_Sum"];
			}
			if (dr["DD_Discount_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Discount_Credit = (decimal)dr["DD_Discount_Credit"];
			}
			if (dr["DD_Discount_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Discount_Debit = (decimal)dr["DD_Discount_Debit"];
			}
			if (dr["DD_Keraye_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Keraye_Credit = (decimal)dr["DD_Keraye_Credit"];
			}
			if (dr["DD_Keraye_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Keraye_Debit = (decimal)dr["DD_Keraye_Debit"];
			}
			if (dr["DD_Bazaryab_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_Bazaryab_Credit = (decimal)dr["DD_Bazaryab_Credit"];
			}
			if (dr["DD_BazaryabDebit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DD_BazaryabDebit = (decimal)dr["DD_BazaryabDebit"];
			}
			if (dr["ED_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Debit = (decimal)dr["ED_Debit"];
			}
			if (dr["ED_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Credit = (decimal)dr["ED_Credit"];
			}
			if (dr["ED_Sood"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Sood = (decimal)dr["ED_Sood"];
			}
			if (dr["ED_VorodQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_VorodQT = dr["ED_VorodQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_KhorojQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_KhorojQT = dr["ED_KhorojQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_BuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_BuyQT = dr["ED_BuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_BuyPrice"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_BuyPrice = (decimal)dr["ED_BuyPrice"];
			}
			if (dr["ED_SellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_SellQT = dr["ED_SellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_SellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_SellSum = (decimal)dr["ED_SellSum"];
			}
			if (dr["ED_BBuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_BBuyQT = dr["ED_BBuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_BBuySum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_BBuySum = (decimal)dr["ED_BBuySum"];
			}
			if (dr["ED_BSellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_BSellQT = dr["ED_BSellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_BSellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_BSellSum = (decimal)dr["ED_BSellSum"];
			}
			if (dr["ED_Tolid_in_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Tolid_in_QT = dr["ED_Tolid_in_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_Tolid_in_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Tolid_in_Sum = (decimal)dr["ED_Tolid_in_Sum"];
			}
			if (dr["ED_Tolid_Out_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Tolid_Out_QT = dr["ED_Tolid_Out_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_Tolid_Out_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Tolid_Out_Sum = (decimal)dr["ED_Tolid_Out_Sum"];
			}
			if (dr["ED_Zayeat_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Zayeat_QT = dr["ED_Zayeat_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["ED_Zayeat_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Zayeat_Sum = (decimal)dr["ED_Zayeat_Sum"];
			}
			if (dr["ED_Discount_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Discount_Credit = (decimal)dr["ED_Discount_Credit"];
			}
			if (dr["ED_Discount_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Discount_Debit = (decimal)dr["ED_Discount_Debit"];
			}
			if (dr["ED_Keraye_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Keraye_Credit = (decimal)dr["ED_Keraye_Credit"];
			}
			if (dr["ED_Keraye_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Keraye_Debit = (decimal)dr["ED_Keraye_Debit"];
			}
			if (dr["ED_Bazaryab_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_Bazaryab_Credit = (decimal)dr["ED_Bazaryab_Credit"];
			}
			if (dr["ED_BazaryabDebit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ED_BazaryabDebit = (decimal)dr["ED_BazaryabDebit"];
			}
			if (dr["Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Credit = (decimal)dr["Credit"];
			}
			if (dr["Sood"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Sood = (decimal)dr["Sood"];
			}
			if (dr["VorodQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_VorodQT = dr["VorodQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["KhorojQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_KhorojQT = dr["KhorojQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_BuyQT = dr["BuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BuyPrice"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_BuyPrice = (decimal)dr["BuyPrice"];
			}
			if (dr["SellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_SellQT = dr["SellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["SellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_SellSum = (decimal)dr["SellSum"];
			}
			if (dr["BBuyQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_BBuyQT = dr["BBuyQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BBuySum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_BBuySum = (decimal)dr["BBuySum"];
			}
			if (dr["BSellQT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_BSellQT = dr["BSellQT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BSellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_BSellSum = (decimal)dr["BSellSum"];
			}
			if (dr["Tolid_in_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Tolid_in_QT = dr["Tolid_in_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["Tolid_in_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Tolid_in_Sum = (decimal)dr["Tolid_in_Sum"];
			}
			if (dr["Tolid_Out_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Tolid_Out_QT = dr["Tolid_Out_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["Tolid_Out_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Tolid_Out_Sum = (decimal)dr["Tolid_Out_Sum"];
			}
			if (dr["Zayeat_QT"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Zayeat_QT = dr["Zayeat_QT"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["Zayeat_Sum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Zayeat_Sum = (decimal)dr["Zayeat_Sum"];
			}
			if (dr["Discount_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Discount_Credit = (decimal)dr["Discount_Credit"];
			}
			if (dr["Discount_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Discount_Debit = (decimal)dr["Discount_Debit"];
			}
			if (dr["Keraye_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Keraye_Credit = (decimal)dr["Keraye_Credit"];
			}
			if (dr["Keraye_Debit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Keraye_Debit = (decimal)dr["Keraye_Debit"];
			}
			if (dr["Bazaryab_Credit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_Bazaryab_Credit = (decimal)dr["Bazaryab_Credit"];
			}
			if (dr["BazaryabDebit"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Rem_BazaryabDebit = (decimal)dr["BazaryabDebit"];
			}
			if (dr["ProductName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ProductName = dr["ProductName"].ToString();
			}
			if (dr["Code"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ProductCode = dr["Code"].ToString();
			}
			if (dr["GroupName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.GroupName = dr["GroupName"].ToString();
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.VahedName = dr["VahedName"].ToString();
			}
			if (dr["DepotName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DepotName = dr["DepotName"].ToString();
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadKardexGaedeshProduct", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 611);
			throw;
		}
		return kardexTajmieReportBusiness;
	}

	private KardexTajmieReportBusiness LoadKardexTajmieProduct(DbDataReader dr)
	{
		KardexTajmieReportBusiness kardexTajmieReportBusiness = new KardexTajmieReportBusiness(base.ContextGuid);
		try
		{
			if (dr["ProductGuid"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["SellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SellSum = (decimal)dr["SellSum"];
			}
			if (dr["SellCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SellQt = dr["SellCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BackSellSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackSellSum = (decimal)dr["BackSellSum"];
			}
			if (dr["BackSellCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackSellQt = dr["BackSellCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BuySum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BuySum = (decimal)dr["BuySum"];
			}
			if (dr["BuyCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BuyQt = dr["BuyCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BackBuySum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackBuySum = (decimal)dr["BackBuySum"];
			}
			if (dr["BackBuyCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackBuyQt = dr["BackBuyCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["BackSellZian"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BackSellZian = (decimal)dr["BackSellZian"];
			}
			if (dr["SellSood"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.SellSood = (decimal)dr["SellSood"];
			}
			if (dr["ZayeatSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ZayeatSum = (decimal)dr["ZayeatSum"];
			}
			if (dr["ZayeatCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ZayeatQt = dr["ZayeatCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["TolidVorodSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidVorodSum = (decimal)dr["TolidVorodSum"];
			}
			if (dr["TolidVorodCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidVorodQt = dr["TolidVorodCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["TolidKhorojSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidKhorojSum = (decimal)dr["TolidKhorojSum"];
			}
			if (dr["TolidKhorojCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TolidKhorojQt = dr["TolidKhorojCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["PishFactorSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.PishFactorSum = (decimal)dr["PishFactorSum"];
			}
			if (dr["PishFactorCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.PishFactorCount = dr["PishFactorCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["T1"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo1 = dr["T1"].ToString();
			}
			if (dr["T2"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo2 = dr["T2"].ToString();
			}
			if (dr["T3"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo3 = dr["T3"].ToString();
			}
			if (dr["T4"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo4 = dr["T4"].ToString();
			}
			if (dr["T5"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo5 = dr["T5"].ToString();
			}
			if (dr["T6"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo6 = dr["T6"].ToString();
			}
			if (dr["T7"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo7 = dr["T7"].ToString();
			}
			if (dr["T8"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.TechnicalInfo8 = dr["T8"].ToString();
			}
			if (dr["Code"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ProductCode = dr["Code"].ToString();
			}
			if (dr["ProductName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.ProductName = dr["ProductName"].ToString();
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.VahedName = dr["VahedName"].ToString();
			}
			if (dr["GroupName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.GroupName = dr["GroupName"].ToString();
			}
			if (dr["AmaniVorodSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.AmaniVorodSum = (decimal)dr["AmaniVorodSum"];
			}
			if (dr["AmaniVorodCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.AmaniVorodCount = dr["AmaniVorodCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["AmaniKhorojSum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.AmaniKhorojSum = (decimal)dr["AmaniKhorojSum"];
			}
			if (dr["AmaniKhorojCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.AmaniKhorojCount = dr["AmaniKhorojCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["PreBuySum"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.PreBuySum = (decimal)dr["PreBuySum"];
			}
			if (dr["PreBuyCount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.PreBuyCount = dr["PreBuyCount"].ToString().ParseToDouble(0.0, 3);
			}
			if (dr["DepotName"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DepotName = dr["DepotName"].ToString();
			}
			if (dr["Amount"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.Amount = dr["Amount"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["BPercent1"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BPercent1 = (decimal)dr["BPercent1"];
			}
			if (dr["BPercent2"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BPercent2 = (decimal)dr["BPercent2"];
			}
			if (dr["BPercent3"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BPercent3 = (decimal)dr["BPercent3"];
			}
			if (dr["BPercent4"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.BPercent4 = (decimal)dr["BPercent4"];
			}
			if (dr["DiscountPercent"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.DiscountPercent = (decimal)dr["DiscountPercent"];
			}
			if (dr["V2Qt"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V2Qt = (decimal)dr["V2Qt"];
			}
			if (dr["V3Qt"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V3Qt = (decimal)dr["V3Qt"];
			}
			if (dr["V4Qt"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V4Qt = (decimal)dr["V4Qt"];
			}
			if (dr["V2Guid"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V2Guid = (Guid)dr["V2Guid"];
			}
			if (dr["V3Guid"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V3Guid = (Guid)dr["V3Guid"];
			}
			if (dr["V4Guid"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V4Guid = (Guid)dr["V4Guid"];
			}
			if (dr["V2Name"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V2Name = dr["V2Name"].ToString();
			}
			if (dr["V3Name"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V3Name = dr["V3Name"].ToString();
			}
			if (dr["V4Name"] != DBNull.Value)
			{
				kardexTajmieReportBusiness.V4Name = dr["V4Name"].ToString();
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadKardexTajmieProduct", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 680);
			throw;
		}
		return kardexTajmieReportBusiness;
	}

	private KardexCustomerModel LoadKardexCustomer(DbDataReader dr)
	{
		KardexCustomerModel kardexCustomerModel = new KardexCustomerModel();
		try
		{
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				kardexCustomerModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["Code"] != DBNull.Value)
			{
				kardexCustomerModel.TafsilCode = (long)dr["Code"];
			}
			if (dr["Name"] != DBNull.Value)
			{
				kardexCustomerModel.TafsilName = dr["Name"].ToString();
			}
			if (dr["GuidGroup"] != DBNull.Value)
			{
				kardexCustomerModel.GroupGuid = (Guid)dr["GuidGroup"];
			}
			if (dr["SD_Debit"] != DBNull.Value)
			{
				kardexCustomerModel.PreDebit = (decimal)dr["SD_Debit"];
			}
			if (dr["SD_Credit"] != DBNull.Value)
			{
				kardexCustomerModel.PreCredit = (decimal)dr["SD_Credit"];
			}
			if (dr["SD_Sood"] != DBNull.Value)
			{
				kardexCustomerModel.PreSood = (decimal)dr["SD_Sood"];
			}
			if (dr["SD_VorodQT"] != DBNull.Value)
			{
				kardexCustomerModel.PreVorodQt = (double)dr["SD_VorodQT"];
			}
			if (dr["SD_KhorojQT"] != DBNull.Value)
			{
				kardexCustomerModel.PreKhorojQt = (double)dr["SD_KhorojQT"];
			}
			if (dr["SD_BuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.PreBuyQt = (double)dr["SD_BuyQT"];
			}
			if (dr["SD_BuyPrice"] != DBNull.Value)
			{
				kardexCustomerModel.PreBuySum = (decimal)dr["SD_BuyPrice"];
			}
			if (dr["SD_SellQT"] != DBNull.Value)
			{
				kardexCustomerModel.PreSellQt = (double)dr["SD_SellQT"];
			}
			if (dr["SD_SellSum"] != DBNull.Value)
			{
				kardexCustomerModel.PreSellSum = (decimal)dr["SD_SellSum"];
			}
			if (dr["SD_BBuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.PreBackBuyQt = (double)dr["SD_BBuyQT"];
			}
			if (dr["SD_BBuySum"] != DBNull.Value)
			{
				kardexCustomerModel.PreBackBuySum = (decimal)dr["SD_BBuySum"];
			}
			if (dr["SD_BSellQT"] != DBNull.Value)
			{
				kardexCustomerModel.PreBackSellQt = (double)dr["SD_BSellQT"];
			}
			if (dr["SD_BSellSum"] != DBNull.Value)
			{
				kardexCustomerModel.PreBackSellSum = (decimal)dr["SD_BSellSum"];
			}
			if (dr["DD_Debit"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenDebit = (decimal)dr["DD_Debit"];
			}
			if (dr["DD_Credit"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenCredit = (decimal)dr["DD_Credit"];
			}
			if (dr["DD_Sood"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenSood = (decimal)dr["DD_Sood"];
			}
			if (dr["DD_VorodQT"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenVorodQt = (double)dr["DD_VorodQT"];
			}
			if (dr["DD_KhorojQT"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenKhorojQt = (double)dr["DD_KhorojQT"];
			}
			if (dr["DD_BuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenBuyQt = (double)dr["DD_BuyQT"];
			}
			if (dr["DD_BuyPrice"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenBuySum = (decimal)dr["DD_BuyPrice"];
			}
			if (dr["DD_SellQT"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenSellQt = (double)dr["DD_SellQT"];
			}
			if (dr["DD_SellSum"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenSellSum = (decimal)dr["DD_SellSum"];
			}
			if (dr["DD_BBuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenBackBuyQt = (double)dr["DD_BBuyQT"];
			}
			if (dr["DD_BBuySum"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenBackBuySum = (decimal)dr["DD_BBuySum"];
			}
			if (dr["DD_BSellQT"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenBackSellQt = (double)dr["DD_BSellQT"];
			}
			if (dr["DD_BSellSum"] != DBNull.Value)
			{
				kardexCustomerModel.BetweenBackSellSum = (decimal)dr["DD_BSellSum"];
			}
			if (dr["ED_Debit"] != DBNull.Value)
			{
				kardexCustomerModel.AfterDebit = (decimal)dr["ED_Debit"];
			}
			if (dr["ED_Credit"] != DBNull.Value)
			{
				kardexCustomerModel.AfterCredit = (decimal)dr["ED_Credit"];
			}
			if (dr["ED_Sood"] != DBNull.Value)
			{
				kardexCustomerModel.AfterSood = (decimal)dr["ED_Sood"];
			}
			if (dr["ED_VorodQT"] != DBNull.Value)
			{
				kardexCustomerModel.AfterVorodQt = (double)dr["ED_VorodQT"];
			}
			if (dr["ED_KhorojQT"] != DBNull.Value)
			{
				kardexCustomerModel.AfterKhorojQt = (double)dr["ED_KhorojQT"];
			}
			if (dr["ED_BuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.AfterBuyQt = (double)dr["ED_BuyQT"];
			}
			if (dr["ED_BuyPrice"] != DBNull.Value)
			{
				kardexCustomerModel.AfterBuySum = (decimal)dr["ED_BuyPrice"];
			}
			if (dr["ED_SellQT"] != DBNull.Value)
			{
				kardexCustomerModel.AfterSellQt = (double)dr["ED_SellQT"];
			}
			if (dr["ED_SellSum"] != DBNull.Value)
			{
				kardexCustomerModel.AfterSellSum = (decimal)dr["ED_SellSum"];
			}
			if (dr["ED_BBuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.AfterBackBuyQt = (double)dr["ED_BBuyQT"];
			}
			if (dr["ED_BBuySum"] != DBNull.Value)
			{
				kardexCustomerModel.AfterBackBuySum = (decimal)dr["ED_BBuySum"];
			}
			if (dr["ED_BSellQT"] != DBNull.Value)
			{
				kardexCustomerModel.AfterBackSellQt = (double)dr["ED_BSellQT"];
			}
			if (dr["ED_BSellSum"] != DBNull.Value)
			{
				kardexCustomerModel.AfterBackSellSum = (decimal)dr["ED_BSellSum"];
			}
			if (dr["Debit"] != DBNull.Value)
			{
				kardexCustomerModel.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				kardexCustomerModel.Credit = (decimal)dr["Credit"];
			}
			if (dr["Sood"] != DBNull.Value)
			{
				kardexCustomerModel.Sood = (decimal)dr["Sood"];
			}
			if (dr["VorodQT"] != DBNull.Value)
			{
				kardexCustomerModel.VorodQt = (double)dr["VorodQT"];
			}
			if (dr["KhorojQT"] != DBNull.Value)
			{
				kardexCustomerModel.KhorojQt = (double)dr["KhorojQT"];
			}
			if (dr["RemQT"] != DBNull.Value)
			{
				kardexCustomerModel.RemQt = (double)dr["RemQT"];
			}
			if (dr["BuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.BuyQt = (double)dr["BuyQT"];
			}
			if (dr["BuyPrice"] != DBNull.Value)
			{
				kardexCustomerModel.BuySum = (decimal)dr["BuyPrice"];
			}
			if (dr["SellQT"] != DBNull.Value)
			{
				kardexCustomerModel.SellQt = (double)dr["SellQT"];
			}
			if (dr["SellSum"] != DBNull.Value)
			{
				kardexCustomerModel.SellSum = (decimal)dr["SellSum"];
			}
			if (dr["BBuyQT"] != DBNull.Value)
			{
				kardexCustomerModel.BackBuyQt = (double)dr["BBuyQT"];
			}
			if (dr["BBuySum"] != DBNull.Value)
			{
				kardexCustomerModel.BackBuySum = (decimal)dr["BBuySum"];
			}
			if (dr["BSellQT"] != DBNull.Value)
			{
				kardexCustomerModel.BackSellQt = (double)dr["BSellQT"];
			}
			if (dr["BSellSum"] != DBNull.Value)
			{
				kardexCustomerModel.BackSellSum = (decimal)dr["BSellSum"];
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadKardexCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 756);
			throw;
		}
		return kardexCustomerModel;
	}

	private KardexViewModel LoadDataModel(DbDataReader dr, List<clsKardexAccess> accessList, short digitForRound)
	{
		try
		{
			KardexViewModel kardexViewModel = new KardexViewModel(null, digitForRound);
			if (dr["ProductGuid"] != DBNull.Value)
			{
				kardexViewModel.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				kardexViewModel.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["PrdName"] != DBNull.Value)
			{
				kardexViewModel.ProductName = dr["PrdName"].ToString();
			}
			if (dr["PrdCode"] != DBNull.Value)
			{
				kardexViewModel.ProductCode = dr["PrdCode"].ToString();
			}
			if (dr["OrderPoint"] != DBNull.Value)
			{
				kardexViewModel.ProductOrderPoint = (decimal)dr["OrderPoint"];
			}
			if (dr["DepotName"] != DBNull.Value)
			{
				kardexViewModel.DepotName = dr["DepotName"].ToString();
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				kardexViewModel.ProductVahedName = dr["VahedName"].ToString();
			}
			if (dr["PreVorodQt"] != DBNull.Value)
			{
				kardexViewModel.PreVorrodQt = (double)dr["PreVorodQt"];
			}
			if (dr["PreKhorojQt"] != DBNull.Value)
			{
				kardexViewModel.PreKhoroojQt = (double)dr["PreKhorojQt"];
			}
			if (dr["PreRemUnitPrice"] != DBNull.Value)
			{
				kardexViewModel.PreRemUnitPrice = (decimal)dr["PreRemUnitPrice"];
			}
			if (dr["PreDiscount"] != DBNull.Value)
			{
				kardexViewModel.PreDiscount = (decimal)dr["PreDiscount"];
			}
			if (dr["PreTotalSood"] != DBNull.Value)
			{
				kardexViewModel.PreTotalSood = (decimal)dr["PreTotalSood"];
			}
			if (dr["PreRemTotalPrice"] != DBNull.Value)
			{
				kardexViewModel.PreRemTotalPrice = (decimal)dr["PreRemTotalPrice"];
			}
			if (dr["PreVisitorPrice"] != DBNull.Value)
			{
				kardexViewModel.PreVisitorPrice = (decimal)dr["PreVisitorPrice"];
			}
			if (dr["PreDate"] != DBNull.Value)
			{
				kardexViewModel.PreDate = (DateTime)dr["PreDate"];
			}
			if (dr["PreVorodSum"] != DBNull.Value)
			{
				kardexViewModel.PreVorrodSum = (decimal)dr["PreVorodSum"];
			}
			if (dr["PreKhorojSum"] != DBNull.Value)
			{
				kardexViewModel.PreKhorojSum = (decimal)dr["PreKhorojSum"];
			}
			if (dr["BetweenVorodQt"] != DBNull.Value)
			{
				kardexViewModel.BetweenVorrodQt = (double)dr["BetweenVorodQt"];
			}
			if (dr["BetweenKhorojQt"] != DBNull.Value)
			{
				kardexViewModel.BetweenKhoroojQt = (double)dr["BetweenKhorojQt"];
			}
			if (dr["BetweenRemUnitPrice"] != DBNull.Value)
			{
				kardexViewModel.BetweenRemUnitPrice = (decimal)dr["BetweenRemUnitPrice"];
			}
			if (dr["BetweenDiscount"] != DBNull.Value)
			{
				kardexViewModel.BetweenDiscount = (decimal)dr["BetweenDiscount"];
			}
			if (dr["BetweenTotalSood"] != DBNull.Value)
			{
				kardexViewModel.BetweenTotalSood = (decimal)dr["BetweenTotalSood"];
			}
			if (dr["BetweenRemTotalPrice"] != DBNull.Value)
			{
				kardexViewModel.BetweenRemTotalPrice = (decimal)dr["BetweenRemTotalPrice"];
			}
			if (dr["BetweenVisitorPrice"] != DBNull.Value)
			{
				kardexViewModel.BetweenVisitorPrice = (decimal)dr["BetweenVisitorPrice"];
			}
			if (dr["BetweenDate"] != DBNull.Value)
			{
				kardexViewModel.BetweenDate = (DateTime)dr["BetweenDate"];
			}
			if (dr["BetweenVorodSum"] != DBNull.Value)
			{
				kardexViewModel.BetweenVorrodSum = (decimal)dr["BetweenVorodSum"];
			}
			if (dr["BetweenKhorojSum"] != DBNull.Value)
			{
				kardexViewModel.BetweenKhorojSum = (decimal)dr["BetweenKhorojSum"];
			}
			if (dr["AfterVorodQt"] != DBNull.Value)
			{
				kardexViewModel.AfterVorrodQt = (double)dr["AfterVorodQt"];
			}
			if (dr["AfterKhorojQt"] != DBNull.Value)
			{
				kardexViewModel.AfterKhoroojQt = (double)dr["AfterKhorojQt"];
			}
			if (dr["AfterRemUnitPrice"] != DBNull.Value)
			{
				kardexViewModel.AfterRemUnitPrice = (decimal)dr["AfterRemUnitPrice"];
			}
			if (dr["AfterDiscount"] != DBNull.Value)
			{
				kardexViewModel.AfterDiscount = (decimal)dr["AfterDiscount"];
			}
			if (dr["AfterTotalSood"] != DBNull.Value)
			{
				kardexViewModel.AfterTotalSood = (decimal)dr["AfterTotalSood"];
			}
			if (dr["AfterRemTotalPrice"] != DBNull.Value)
			{
				kardexViewModel.AfterRemTotalPrice = (decimal)dr["AfterRemTotalPrice"];
			}
			if (dr["AfterVisitorPrice"] != DBNull.Value)
			{
				kardexViewModel.AfterVisitorPrice = (decimal)dr["AfterVisitorPrice"];
			}
			if (dr["AfterDate"] != DBNull.Value)
			{
				kardexViewModel.AfterDate = (DateTime)dr["AfterDate"];
			}
			if (dr["AfterVorodSum"] != DBNull.Value)
			{
				kardexViewModel.AfterVorrodSum = (decimal)dr["AfterVorodSum"];
			}
			if (dr["AfterKhorojSum"] != DBNull.Value)
			{
				kardexViewModel.AfterKhorojSum = (decimal)dr["AfterKhorojSum"];
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				kardexViewModel.AsType = (Document)dr["SanadType"];
			}
			if (dr["V2Guid"] != DBNull.Value)
			{
				kardexViewModel.ProductV2Guid = (Guid)dr["V2Guid"];
			}
			if (dr["V2Qt"] != DBNull.Value)
			{
				kardexViewModel.ProductV2Qt = (decimal)dr["V2Qt"];
			}
			if (dr["V2Name"] != DBNull.Value)
			{
				kardexViewModel.ProductV2Name = dr["V2Name"].ToString();
			}
			if (dr["V3Guid"] != DBNull.Value)
			{
				kardexViewModel.ProductV3Guid = (Guid)dr["V3Guid"];
			}
			if (dr["V3Qt"] != DBNull.Value)
			{
				kardexViewModel.ProductV3Qt = (decimal)dr["V3Qt"];
			}
			if (dr["V3Name"] != DBNull.Value)
			{
				kardexViewModel.ProductV3Name = dr["V3Name"].ToString();
			}
			if (dr["V4Guid"] != DBNull.Value)
			{
				kardexViewModel.ProductV4Guid = (Guid)dr["V4Guid"];
			}
			if (dr["V4Qt"] != DBNull.Value)
			{
				kardexViewModel.ProductV4Qt = (decimal)dr["V4Qt"];
			}
			if (dr["V4Name"] != DBNull.Value)
			{
				kardexViewModel.ProductV4Name = dr["V4Name"].ToString();
			}
			return kardexViewModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 819);
			throw;
		}
	}

	private KardexLinkedOrder LoadDataLinkedModel(DbDataReader dr)
	{
		try
		{
			KardexLinkedOrder kardexLinkedOrder = new KardexLinkedOrder();
			if (dr["ID2"] != DBNull.Value)
			{
				kardexLinkedOrder.Id2 = dr["ID2"].ToString();
			}
			if (dr["DateM"] != DBNull.Value)
			{
				kardexLinkedOrder.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				kardexLinkedOrder.Description = dr["Description"].ToString();
			}
			if (dr["KhorojQT"] != DBNull.Value)
			{
				kardexLinkedOrder.SellQuantity = (decimal)dr["KhorojQT"];
			}
			if (dr["UnitPrice"] != DBNull.Value)
			{
				kardexLinkedOrder.SellUnitPrice = (decimal)dr["UnitPrice"];
			}
			if (dr["RemTotalPrice"] != DBNull.Value)
			{
				kardexLinkedOrder.SellTotalPrice = (decimal)dr["RemTotalPrice"];
			}
			if (dr["DetailGuid"] != DBNull.Value)
			{
				kardexLinkedOrder.DetailGuid = (Guid)dr["DetailGuid"];
			}
			if (dr["SanadGuid"] != DBNull.Value)
			{
				kardexLinkedOrder.OrderGuid = (Guid)dr["SanadGuid"];
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				kardexLinkedOrder.OrderType = (Document)(short)dr["SanadType"];
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				kardexLinkedOrder.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["BackSellQuantity"] != DBNull.Value)
			{
				kardexLinkedOrder.BackSellQuantity = (decimal)dr["BackSellQuantity"];
			}
			if (dr["RemainQuantity"] != DBNull.Value)
			{
				kardexLinkedOrder.RemainQuantity = (decimal)dr["RemainQuantity"];
			}
			if (dr["V2Guid"] != DBNull.Value)
			{
				kardexLinkedOrder.V2Guid = (Guid)dr["V2Guid"];
			}
			if (dr["V2Qt"] != DBNull.Value)
			{
				kardexLinkedOrder.V2Quantity = (decimal)dr["V2Qt"];
			}
			if (dr["Avarez"] != DBNull.Value)
			{
				kardexLinkedOrder.Avarez = (decimal)dr["Avarez"];
			}
			if (dr["Discount"] != DBNull.Value)
			{
				kardexLinkedOrder.Discount = (decimal)dr["Discount"];
			}
			if (dr["Tax"] != DBNull.Value)
			{
				kardexLinkedOrder.Tax = (decimal)dr["Tax"];
			}
			return kardexLinkedOrder;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataLinkedModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 851);
			throw;
		}
	}

	private KardexConflictRecord LoadDataConflictRecord(DbDataReader dr)
	{
		try
		{
			KardexConflictRecord kardexConflictRecord = new KardexConflictRecord();
			if (dr["ProductName"] != DBNull.Value)
			{
				kardexConflictRecord.ProductName = dr["ProductName"].ToString();
			}
			if (dr["ProductCode"] != DBNull.Value)
			{
				kardexConflictRecord.ProductCode = dr["ProductCode"].ToString();
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				kardexConflictRecord.SanadType = (Document)dr["SanadType"];
			}
			if (dr["SanadGuid"] != DBNull.Value)
			{
				kardexConflictRecord.SanadGuid = (Guid)dr["SanadGuid"];
			}
			if (dr["ID2"] != DBNull.Value)
			{
				kardexConflictRecord.Id2 = dr["ID2"].ToString().ParseToInt();
			}
			if (dr["DateM"] != DBNull.Value)
			{
				kardexConflictRecord.DateM = (DateTime)dr["DateM"];
			}
			if (dr["RemUnitPrice_trust"] != DBNull.Value)
			{
				kardexConflictRecord.TrustUnitPrice = (decimal)dr["RemUnitPrice_trust"];
			}
			if (dr["RemUnitPrice_Current"] != DBNull.Value)
			{
				kardexConflictRecord.CurrentUnitPrice = (decimal)dr["RemUnitPrice_Current"];
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				kardexConflictRecord.ProductGuid = (Guid)dr["ProductGuid"];
			}
			return kardexConflictRecord;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataConflictRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepository.cs", 875);
			throw;
		}
	}
}
