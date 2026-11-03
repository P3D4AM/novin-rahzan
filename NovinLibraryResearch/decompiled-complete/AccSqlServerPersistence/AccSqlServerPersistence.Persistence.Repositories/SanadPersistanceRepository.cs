using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.AssistanceClasses;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Sanad;
using AccSqlServerPersistence.Properties;
using EntityCache;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Global;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Events;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class SanadPersistanceRepository : SanadPersistanceRepositoryBase
{
	private ViewSanadDetailCoverPersistenceRepository _coverRepo;

	public SanadPersistanceRepository(ContextHolder db)
		: base(db)
	{
		_coverRepo = new ViewSanadDetailCoverPersistenceRepository(db);
	}

	public override async Task<ReturnedSaveFuncInfo> ChangeSanadNoAsync(List<int> oldSanadNo, int newSanadNo, string description, DateTime date, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 39);
		try
		{
			int num = (await StimulQueryBusiness.SaveAsync(ContextGuid, oldSanadNo))?.value ?? 0;
			List<SqlParameter> list = new List<SqlParameter>
			{
				new SqlParameter("@refId", (object)num),
				new SqlParameter("@SanadNO", (object)newSanadNo),
				new SqlParameter("@Description", (object)description),
				new SqlParameter("@DateM", (object)date)
			};
			List<SqlParameter> list2 = list;
			list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
			List<SqlParameter> parameters = list;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Sanad_ChangeSanadNO", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"), ReturnedState.None, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 51);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 55);
			res.AddReturnedValue(ex, "", "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 56);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> ChangeSanadStatusAsync(string sansdNos, SanadSt st, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 63);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@SanadNOs", (object)sansdNos),
				new SqlParameter("@ST", (object)st)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("SP_Change_SanadST", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"), ReturnedState.None, "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 71);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 75);
			res.AddReturnedValue(ex, "", "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 76);
		}
		return res;
	}

	public override async Task<bool> CheckGhestAsync(Guid sanadGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 82);
		bool result;
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadGuid", (object)sanadGuid)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Sanad_CheckGhest", parameters, tranName, token, 30, "CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				result = obj != null && obj.ToString().ParseToInt() > 0;
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
				WebErrorLog.Instance.StartLog(ex2, "", "CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 94);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<int> CountOfAutoSanadWithDiffrentDateAsync(int sanadNo, DateTime dateM, Guid asGuid1, short asType1, Guid asGuid2, short asType2, CancellationToken token, string tranName)
	{
		int res = 0;
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 106);
		int result;
		try
		{
			_ = 1;
			try
			{
				if (dateM < new DateTime(1900, 1, 1))
				{
					result = res;
					goto IL_0345;
				}
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadNO", (object)sanadNo),
					new SqlParameter("@DateM", (object)dateM),
					new SqlParameter("@AsGuid1", (object)asGuid1),
					new SqlParameter("@ASType1", (object)asType1),
					new SqlParameter("@AsGuid2", (object)asGuid2),
					new SqlParameter("@ASType2", (object)asType2)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Sanad_CountOFAutoSanadWithDiffrentDate", parameters, tranName, token, 30, "CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				if (obj == null)
				{
					result = res;
					goto IL_0345;
				}
				res = obj.ToString().ParseToInt();
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
				WebErrorLog.Instance.StartLog(ex2, "", "CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 127);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return res;
		IL_0345:
		return result;
	}

	public override async Task<IEnumerable<SanadReportBusiness>> GetAllAsync(SanadFilter filter, CancellationToken token, string tranName)
	{
		List<SanadReportBusiness> list = new List<SanadReportBusiness>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 140);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			IEnumerable<SanadReportBusiness> result;
			try
			{
				int num2 = 0;
				if ((filter?.NumberList?.Any()).GetValueOrDefault())
				{
					num2 = (await StimulQueryBusiness.SaveAsync(ContextGuid, filter.NumberList, token, tranName))?.value ?? 0;
				}
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("@Number_1", (object)filter.NumberFrom),
					new SqlParameter("@Number_2", (object)filter.NumberTo),
					new SqlParameter("@Sum1", (object)filter.SumFrom),
					new SqlParameter("@Sum2", (object)filter.SumTo),
					new SqlParameter("@Description", (object)filter.Description),
					new SqlParameter("@Debit_Min", (object)filter.DebitFrom),
					new SqlParameter("@Debit_Max", (object)filter.DebitTo),
					new SqlParameter("@Credit_Min", (object)filter.CreditFrom),
					new SqlParameter("@Credit_Max", (object)filter.CreditTo),
					new SqlParameter("@RowDescription", (object)filter.RowDescription),
					new SqlParameter("@HesabCode1", (object)filter.MoeinCodeFrom),
					new SqlParameter("@HesabCode2", (object)filter.MoeinCodeTo),
					new SqlParameter("@HesabFullCode1", (object)filter.TafsilCodeFrom),
					new SqlParameter("@HesabFullCode2", (object)filter.TafsilCodeTo),
					new SqlParameter("@RefrencedId", (object)num2),
					new SqlParameter("@StatusId", (object)(filter?.StatusId).GetValueOrDefault())
				};
				if (filter.CashierGuid.HasValue && filter.CashierGuid != Guid.Empty)
				{
					list2.Add(new SqlParameter("@cashierGuid", (object)filter.CashierGuid));
				}
				if (filter.MoeinGuid.HasValue && filter.MoeinGuid != Guid.Empty)
				{
					list2.Add(new SqlParameter("@MoeinGuid", (object)filter.MoeinGuid));
				}
				if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
				{
					list2.Add(new SqlParameter("@tafsilGuid", (object)filter.TafsilGuid));
				}
				if (filter.DateFrom.HasValue)
				{
					list2.Add(new SqlParameter("@Date1", (object)filter.DateFrom));
				}
				if (filter.DateTo.HasValue)
				{
					list2.Add(new SqlParameter("@Date2", (object)filter.DateTo));
				}
				if (filter.SanadStatus.HasValue)
				{
					list2.Add(new SqlParameter("@SanadST", (object)filter.SanadStatus));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Sanad_Auto_Filter", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				while (true)
				{
					if (!(await dr.ReadAsync()))
					{
						if ((filter?.SanadTypeList?.Any()).GetValueOrDefault())
						{
							list = list?.Where((SanadReportBusiness q) => filter.SanadTypeList.Contains(q.SanadType))?.ToList();
						}
						dr.Close();
						break;
					}
					if (!base.IsDisposed && !token.IsCancellationRequested)
					{
						list.Add(LoadSanadReportBusiness(dr, token));
						continue;
					}
					result = null;
					goto end_IL_0000;
				}
				goto end_IL_00e3;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 188);
				throw;
			}
			return result;
			end_IL_00e3:;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<List<SanadDetReportBusiness>> GetAllGardeshSanadAsync(ENHesabLevel level, DateTime? date1, DateTime? date2, long? code1, long? code2, long? codeMoein, Guid? kolGuid, string tranName, int? maxCount, short maxParalelThreads, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		using (new JaegerMonitor("GetAllGardeshSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"))
		{
			_ = 2;
			try
			{
				switch (level)
				{
				case ENHesabLevel.Kol:
					list = await GetKolGardeshAsync(code1, code2, date1, date2, maxCount, maxParalelThreads, tranName, token);
					break;
				case ENHesabLevel.Moein:
					list = await GetMoeinGardeshAsync(code1, code2, date1, date2, kolGuid, maxCount, maxParalelThreads, tranName, token);
					break;
				case ENHesabLevel.Tafsil:
					list = await GetTafsilGardeshAsync(code1, code2, codeMoein, date1, date2, maxCount, maxParalelThreads, tranName, token);
					break;
				}
				return list?.OrderBy((SanadDetReportBusiness q) => q.DateM)?.ThenByDescending((SanadDetReportBusiness q) => q.RowIndex)?.ToList();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllGardeshSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 222);
				throw;
			}
		}
	}

	private async Task<List<SanadDetReportBusiness>> GetKolGardeshAsync(long? kolCode1, long? kolCode2, DateTime? date1, DateTime? date2, int? maxCount, short maxParalelThreads, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		try
		{
			List<Guid> guidList = new List<Guid>();
			List<KolBusiness> list2 = (await KolBusiness.GetAllBetweenCodesAsync(ContextGuid, kolCode1, kolCode2, token)).ToList();
			if (list2.Count > 0)
			{
				guidList.AddRange(list2.Select((KolBusiness q) => q.Guid));
			}
			List<Task<List<SanadDetReportBusiness>>> tasks = new List<Task<List<SanadDetReportBusiness>>>();
			foreach (Guid kolGuid in guidList)
			{
				tasks.Add(Task.Run(() => GetReportAsync(null, null, kolGuid, date1, date2, maxCount, tranName, token)));
				if (tasks.Count > maxParalelThreads)
				{
					Task<List<SanadDetReportBusiness>> task = await Task.WhenAny(tasks);
					tasks.Remove(task);
					list.AddRange(await task);
				}
			}
			while (tasks.Any())
			{
				Task<List<SanadDetReportBusiness>> task2 = await Task.WhenAny(tasks);
				tasks.Remove(task2);
				list.AddRange(await task2);
			}
			return list;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetKolGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 257);
			throw;
		}
	}

	private async Task<List<SanadDetReportBusiness>> GetMoeinGardeshAsync(long? moeinCode1, long? moeinCode2, DateTime? date1, DateTime? date2, Guid? kolGuid, int? maxCount, short maxParalelThreads, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		try
		{
			List<Guid> guidList = new List<Guid>();
			List<MoeinBusiness> list2 = (await MoeinBusiness.GetAllBetweenCodesAsync(ContextGuid, moeinCode1, moeinCode2, token))?.ToList();
			if (list2.Count > 0)
			{
				if (kolGuid.HasValue && kolGuid != Guid.Empty)
				{
					list2 = list2?.Where(delegate(MoeinBusiness q)
					{
						Guid kolGuid2 = q.KolGuid;
						Guid? guid = kolGuid;
						return kolGuid2 == guid;
					})?.ToList();
				}
				guidList.AddRange(list2?.Select((MoeinBusiness q) => q.Guid));
			}
			List<Task<List<SanadDetReportBusiness>>> tasks = new List<Task<List<SanadDetReportBusiness>>>();
			foreach (Guid moeinGuid in guidList)
			{
				tasks.Add(Task.Run(() => GetReportAsync(null, moeinGuid, null, date1, date2, maxCount, tranName, token)));
				if (tasks.Count > maxParalelThreads)
				{
					Task<List<SanadDetReportBusiness>> task = await Task.WhenAny(tasks);
					tasks.Remove(task);
					List<SanadDetReportBusiness> list3 = list;
					list3.AddRange(await task);
				}
			}
			while (tasks.Any())
			{
				Task<List<SanadDetReportBusiness>> task2 = await Task.WhenAny(tasks);
				tasks.Remove(task2);
				List<SanadDetReportBusiness> list3 = list;
				list3.AddRange(await task2);
			}
			return list;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetMoeinGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 295);
			throw;
		}
	}

	private async Task<List<SanadDetReportBusiness>> GetTafsilGardeshAsync(long? tafsilCode1, long? tafsilCode2, long? moeinCode, DateTime? date1, DateTime? date2, int? maxCount, short maxParalelThreads, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		try
		{
			List<Guid> tafsilGuidList = new List<Guid>();
			Guid? moeinGuid = null;
			IEnumerable<TafsilBusiness> enumerable = await TafsilBusiness.GetAllAsync(ContextGuid, EnSystem.None, new TafsilFilter
			{
				StartCode = tafsilCode1,
				EndCode = tafsilCode2
			}, token, "", "GetTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			if (enumerable != null && enumerable.Any())
			{
				tafsilGuidList.AddRange(enumerable.Select((TafsilBusiness q) => q.Guid));
			}
			if (moeinCode.HasValue && moeinCode != 0)
			{
				moeinGuid = (await MoeinBusiness.GetAsync(ContextGuid, moeinCode.GetValueOrDefault(), token))?.Guid;
			}
			List<Task<List<SanadDetReportBusiness>>> tasks = new List<Task<List<SanadDetReportBusiness>>>();
			foreach (Guid tafsilGuid in tafsilGuidList)
			{
				tasks.Add(Task.Run(() => GetReportAsync(tafsilGuid, moeinGuid, null, date1, date2, maxCount, tranName, token)));
				if (tasks.Count > maxParalelThreads)
				{
					Task<List<SanadDetReportBusiness>> task = await Task.WhenAny(tasks);
					tasks.Remove(task);
					List<SanadDetReportBusiness> list2 = list;
					list2.AddRange(await task);
				}
			}
			while (tasks.Any())
			{
				Task<List<SanadDetReportBusiness>> task2 = await Task.WhenAny(tasks);
				tasks.Remove(task2);
				List<SanadDetReportBusiness> list2 = list;
				list2.AddRange(await task2);
			}
			return list;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 338);
			throw;
		}
	}

	private async Task<List<SanadDetReportBusiness>> GetReportAsync(Guid? tafsilGuid, Guid? moeinGuid, Guid? kolGuid, DateTime? date1, DateTime? date2, int? maxCount, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> ret = new List<SanadDetReportBusiness>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 346);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> list = new List<SqlParameter>();
				if (tafsilGuid.HasValue)
				{
					list.Add(new SqlParameter("@TafsilGuid", (object)tafsilGuid));
				}
				if (moeinGuid.HasValue)
				{
					list.Add(new SqlParameter("@MoeinGuid", (object)moeinGuid));
				}
				if (kolGuid.HasValue)
				{
					list.Add(new SqlParameter("@KolGuid", (object)kolGuid));
				}
				if (date1.HasValue)
				{
					list.Add(new SqlParameter("@Date1", (object)date1));
				}
				if (date2.HasValue)
				{
					list.Add(new SqlParameter("@Date2", (object)date2));
				}
				if (maxCount.HasValue)
				{
					list.Add(new SqlParameter("@MaxCount", (object)maxCount));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_GardeshTafsil_GetAllFromWebVeiw", list, tranName, token, CommandType.StoredProcedure, 30, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				int index = 0;
				while (await dr.ReadAsync(token))
				{
					token.ThrowIfCancellationRequested();
					SanadPersistanceRepository sanadPersistanceRepository = this;
					string contextGuid = ContextGuid;
					int num2 = index + 1;
					index = num2;
					ret.Add(sanadPersistanceRepository.LoadDataReport(contextGuid, dr, num2, kolGuid, moeinGuid, tafsilGuid, isFromGardesh: false, token));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 369);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return ret;
	}

	public override async Task<List<SanadDetReportBusiness>> GetAllRoznameSanadDetAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		using (new JaegerMonitor("GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"))
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 300000, "GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 383);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				List<SanadDetReportBusiness> result;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@D1", (object)date1),
						new SqlParameter("@D2", (object)date2)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_VRozname_SelectByDate", parameters, tranName, token, CommandType.StoredProcedure, 300, "GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
					int index = 0;
					while (true)
					{
						if (!(await dr.ReadAsync(token)))
						{
							dr.Close();
							break;
						}
						if (!base.IsDisposed && !token.IsCancellationRequested)
						{
							SanadPersistanceRepository sanadPersistanceRepository = this;
							string contextGuid = ContextGuid;
							int num2 = index + 1;
							index = num2;
							list.Add(sanadPersistanceRepository.LoadDataReport(contextGuid, dr, num2, null, null, null, isFromGardesh: false, token));
							continue;
						}
						result = null;
						goto end_IL_01cc;
					}
					goto end_IL_00e2;
					end_IL_01cc:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 404);
					throw;
				}
				return result;
				end_IL_00e2:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<List<SanadDetRoznameKolBusiness>> GetAllSanadDetRoznameKolAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		List<SanadDetRoznameKolBusiness> list = new List<SanadDetRoznameKolBusiness>();
		using (new JaegerMonitor("GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"))
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 418);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Date1", (object)date1),
						new SqlParameter("@Date2", (object)date2)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_RoznameKOL_TajmieeByDate", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
					while (await dr.ReadAsync())
					{
						token.ThrowIfCancellationRequested();
						list.Add(LoadDataRoznameKol(ContextGuid, dr, isLoadSanadType: true));
					}
					dr.Close();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 438);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<SanadBusiness> GetAsync(Guid SanadGuid, Document SanadType, bool LoadDets, CancellationToken token, string tranName)
	{
		SanadBusiness item = null;
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 451);
		try
		{
			int num;
			_ = num - 1;
			_ = 7;
			SanadBusiness result;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@AsGuid", (object)SanadGuid),
					new SqlParameter("@AsType", (object)(short)SanadType)
				};
				using (DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_ViewSanadDetail_GetByGuidType", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"))
				{
					List<Sanad_DetBusiness> DetList = new List<Sanad_DetBusiness>();
					Guid cashierGuid = Guid.Empty;
					DateTime? date = null;
					int sanadNo = 0;
					int ordinal = 0;
					while (await dr.ReadAsync())
					{
						if (dr["CashierGuid"] != DBNull.Value && cashierGuid == Guid.Empty)
						{
							cashierGuid = (Guid)dr["CashierGuid"];
						}
						if (dr["DateM"] != DBNull.Value && !date.HasValue)
						{
							date = (DateTime)dr["DateM"];
						}
						List<Sanad_DetBusiness> list = DetList;
						list.Add(await LoadDataVSanadDetail(dr, SanadGuid, SanadType, ordinal++, token, tranName));
						if (dr["SanadNO"] != DBNull.Value)
						{
							sanadNo = (int)dr["SanadNO"];
						}
					}
					dr.Close();
					if (DetList.Count > 0)
					{
						Sanad_DetBusiness det = DetList.FirstOrDefault();
						parameters = new List<SqlParameter>
						{
							new SqlParameter("@SanadNO", (object)sanadNo)
						};
						using (DbDataReader Sanaddr = await c.ExecuteReaderAsync("sp_Acc_Sanad_SelectRow_SanadNO", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"))
						{
							if (await Sanaddr.ReadAsync())
							{
								item = await LoadDataAsync(Sanaddr, isLoadDets: false, token, tranName);
							}
							Sanaddr.Close();
							if (item == null)
							{
								item = new SanadBusiness(ContextGuid, sanadNo)
								{
									Guid = Guid.NewGuid(),
									Description = det?.RowDesc,
									Status = true,
									Modified = DateTime.Now,
									DateM = date.Value,
									UserGuid = cashierGuid
								};
								if (sanadNo != 0)
								{
									item.ChangeStatus = EntityChangeSt.Edited;
								}
								else
								{
									item.ChangeStatus = EntityChangeSt.New;
								}
								item.SanadNo = sanadNo;
							}
							SanadBusiness sanadBusiness = item;
							await sanadBusiness.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(ContextGuid, DetList, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.None, appendCurrentItems: false, token);
						}
						goto end_IL_01dd;
					}
					result = item;
					goto end_IL_00ee;
					end_IL_01dd:;
				}
				goto end_IL_00d7;
				end_IL_00ee:;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 504);
				throw;
			}
			return result;
			end_IL_00d7:;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return item;
	}

	public override async Task<decimal> GetBeforeAccountAsync(Guid tafsilGuid, Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		decimal res = default(decimal);
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 517);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@TafsilGuid", (object)tafsilGuid),
					new SqlParameter("@AsGuid", (object)asGuid),
					new SqlParameter("@AsType", (object)(short)asType)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_GetBeforeAccount", parameters, tranName, token, 30, "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				if (obj != null)
				{
					res = obj.ToString().ParseToDecimal(0m, ClsCache.GetCache(ContextGuid).MultiLanguageConfig.DigitAfterPoint);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 534);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return res;
	}

	public override async Task<List<MoeinTafsilReportModel>> GetMoein2TafsilsRptAsync(DateTime date1, DateTime date2, Guid moeinGuid, string tranName, CancellationToken token)
	{
		List<MoeinTafsilReportModel> list = new List<MoeinTafsilReportModel>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 547);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Date1", (object)date1),
					new SqlParameter("@Date2", (object)date2),
					new SqlParameter("@MoeinGuid", (object)moeinGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_RPT_Moein2Tafsils", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				while (await dr.ReadAsync())
				{
					token.ThrowIfCancellationRequested();
					list.Add(LoadDataMoeinTafsilReport(dr));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 568);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<List<SanadDetReportBusiness>> GetMoeinTafsilGardeshAsync(DateTime d1, DateTime d2, Guid tafsilGuid, string moeinCode1, string moeinCode2, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		using (new JaegerMonitor("GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"))
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 582);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Date1", (object)d1),
						new SqlParameter("@Date2", (object)d2),
						new SqlParameter("@TafsilGuid", (object)tafsilGuid),
						new SqlParameter("@CodeMoein1", (object)moeinCode1),
						new SqlParameter("@CodeMoein2", (object)moeinCode2)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_HesabTafsilMoeins_FillAll_New", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
					int index = 0;
					while (await dr.ReadAsync())
					{
						SanadPersistanceRepository sanadPersistanceRepository = this;
						string contextGuid = ContextGuid;
						int num2 = index + 1;
						index = num2;
						list.Add(sanadPersistanceRepository.LoadDataReport(contextGuid, dr, num2, null, null, tafsilGuid, isFromGardesh: true, token));
					}
					dr.Close();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 603);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<SanadBusiness> GetStaticEkhtetamieSanadAsync(ContextHolder dbContext, CancellationToken token, string tranName)
	{
		SanadBusiness ret = null;
		Acc_ModelContext c = null;
		try
		{
			_ = 6;
			try
			{
				c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetStaticEkhtetamieSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 619));
				_ = ClsCache.GetCache(dbContext.Context.Guid).Source;
				DbDataReader dr = await c.ExecuteReaderAsync("Sanad_BastaneMovaghatAndEkhtetamie", null, tranName, token, CommandType.StoredProcedure, 30, "GetStaticEkhtetamieSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				while (await dr.ReadAsync())
				{
					if (ret == null)
					{
						string guid = dbContext.Context.Guid;
						SanadType? type = SanadType.Ekhtetamiye;
						ret = await SanadBusiness.GenerateNewWithDefaultsAsync(guid, "", null, default(CancellationToken), tranName, type);
					}
					Sanad_DetBusiness det = await ret.AddToListSanadAsync(new Sanad_DetBusiness(dbContext.Context.Guid, EnSystem.None, ret.Guid, Document.Sanad, ret.NextOrdinal()), EnSystem.None, token);
					decimal num = (decimal)dr["Account"];
					if (num > 0m)
					{
						det.Debit = num;
						det.Credit = 0m;
					}
					else if (num < 0m)
					{
						det.Debit = 0m;
						det.Credit = -num;
					}
					await det.SetMoeinGuidAsync((Guid)dr["MoeinGuid"], token, tranName);
					if (dr["TafsilGuid"] != DBNull.Value)
					{
						await det.SetTafsilGuidAsync((Guid)dr["TafsilGuid"], tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetStaticEkhtetamieSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 645);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetStaticEkhtetamieSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return ret;
	}

	public override async Task<List<Sanad_DetBusiness>> GetStaticSanadTempAsync(ContextHolder dbContext, CancellationToken token, string tranName)
	{
		List<Sanad_DetBusiness> list = new List<Sanad_DetBusiness>();
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetStaticSanadTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 658));
		try
		{
			int num;
			_ = num - 1;
			_ = 4;
			try
			{
				List<Guid> lst = new List<Guid>
				{
					ParentDefaults.HesabTypeGuids.Darayi,
					ParentDefaults.HesabTypeGuids.Bedehi,
					ParentDefaults.HesabTypeGuids.Sarmaye,
					ParentDefaults.HesabTypeGuids.HesabHayeEntezami
				};
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(ContextGuid, lst, token, tranName);
				returnedSaveFuncInfoWithValue.throwExceptionIfError();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@TypeGuidsRefrencedId", (object)returnedSaveFuncInfoWithValue.value)
				};
				DbDataReader dr = await c.ExecuteReaderAsync("SP_Transfer_SanadBastaneMovaghat", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetStaticSanadTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				int ordinal = 0;
				while (await dr.ReadAsync())
				{
					Sanad_DetBusiness det = new Sanad_DetBusiness(dbContext.Context.Guid, EnSystem.None, Guid.Empty, Document.Sanad, ordinal++);
					decimal num2 = (decimal)dr["SumDebit"];
					decimal num3 = (decimal)dr["SumCredit"] - num2;
					if (num3 < 0m)
					{
						det.Debit = -num3;
					}
					else
					{
						if (!(num3 > 0m))
						{
							continue;
						}
						det.Credit = num3;
					}
					await det.SetMoeinGuidAsync((Guid)dr["MoeinGuid"], token, tranName);
					if (dr["TafsilGuid"] != DBNull.Value)
					{
						await det.SetTafsilGuidAsync((Guid)dr["TafsilGuid"], tranName, token);
					}
					list.Add(det);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetStaticSanadTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 694);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetStaticSanadTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<List<MoeinTafsilReportModel>> GetTafsil2MoeinsRptAsync(DateTime date1, DateTime date2, Guid tafsilGuid, string tranName, CancellationToken token)
	{
		List<MoeinTafsilReportModel> list = new List<MoeinTafsilReportModel>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 707);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Date1", (object)date1),
					new SqlParameter("@Date2", (object)date2),
					new SqlParameter("@TafsilGuid", (object)tafsilGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_RPT_Tafsil2Moeins", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				while (await dr.ReadAsync())
				{
					token.ThrowIfCancellationRequested();
					list.Add(LoadDataMoeinTafsilReport(dr));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 728);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<List<MoeinTafsilReportModel>> GetTafsilTajmeeReportAsync(DateTime date1, DateTime date2, string moeinCode1, string moeinCode2, string tafsilCode1, string tafsilCode2, CancellationToken token, string tranName)
	{
		List<MoeinTafsilReportModel> list = new List<MoeinTafsilReportModel>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 741);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@date1", (object)date1),
					new SqlParameter("@Date2", (object)date2),
					new SqlParameter("@CodeTafsil1", (object)tafsilCode1),
					new SqlParameter("@CodeTafsil2", (object)tafsilCode2),
					new SqlParameter("@CodeMoein1", (object)moeinCode1),
					new SqlParameter("@CodeMoein2", (object)moeinCode2)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_RPT_TajmieTafsili", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataMoeinTafsilReport(dr));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 761);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<List<SanadDetRoznameKolBusiness>> GetTestTarazNameAsync(DateTime date, string tranName, CancellationToken token)
	{
		List<SanadDetRoznameKolBusiness> list = new List<SanadDetRoznameKolBusiness>();
		using (new JaegerMonitor("GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"))
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 775);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Date", (object)date)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Hesab_Select_AzTaraz", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
					while (await dr.ReadAsync())
					{
						token.ThrowIfCancellationRequested();
						list.Add(LoadDataRoznameKol(ContextGuid, dr, isLoadSanadType: false));
					}
					dr.Close();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 791);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid asGuid, Document asType, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 804);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@AsGuid", (object)asGuid),
				new SqlParameter("@AsType", (object)asType)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Acc_Sanad_DeleteRowsByAsGuid_AsType", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 812);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 816);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 817);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> SanadSortAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 600000, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 823);
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("SP_Sanad_Sort", null, tranName, token, "", CommandType.StoredProcedure, 30, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"), ReturnedState.None, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 827);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 831);
				res.AddReturnedValue(ex, "", "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 832);
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(string caller, SanadBusiness item, EnSystem system, int oldSanadNo, CancellationToken token, string tranName, IEnumerable<ViewSanadDetailCoverBusiness> lstCover, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		int line = 0;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 1800000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 845);
		if (item == null)
		{
			throw new NullReferenceException("SanadBusiness is null!!");
		}
		if (item?.ListSanad_Det == null)
		{
			throw new NullReferenceException("SanadBusiness.ListSanad_Det is null!!");
		}
		try
		{
			if (autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 == Document.Sanad) && autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 != Document.Sanad))
			{
				throw new Exception("یا باید اقلام کلا غیر اتومات باشند یا اینکه همه اتومات باشند.");
			}
			line = 10000;
			if (item.ListSanad_Det.Any((Sanad_DetBusiness p) => !p.CheckIsContain(autoSanadItemsForSave)))
			{
				throw new Exception($"sanad rows contain invalid value(s) for Astype or AsGuid \r\n SanadNo:{item.SanadNo}");
			}
			line = 2;
			SanadBusiness old = null;
			if (oldSanadNo != 0)
			{
				old = await GetAsync(oldSanadNo, token, tranName);
			}
			line = 3;
			int sanadNo = await item.GetSanadNoAsync(token, tranName);
			SanadBusiness sanadBusiness = await GetAsync(sanadNo, token, tranName);
			line = 4;
			if (sanadBusiness != null && item.ListSanad_Det.Any((Sanad_DetBusiness q) => !q.CheckIsContain(autoSanadItemsForSave)) && sanadBusiness.DateM.Date != item.DateM.Date)
			{
				if (system != EnSystem.Transfer && system != EnSystem.TransferMaliYear)
				{
					throw new Exception("تاریخ برگه سند تجمیع شده، قابل ویرایش نیست".GetTranslate() + "\r\n" + $"ExistsDanadDate:{sanadBusiness.DateM.Date}\r\n" + $"Date:{item.DateM.Date}\r\n" + $"OldSanadNo:{oldSanadNo}\r\n" + $"syste:{system}\r\n" + "item:" + Json.ToStringJson(item, indented: true));
				}
				item.DateM = sanadBusiness.DateM;
			}
			line = 5;
			if (system != EnSystem.Transfer && system != EnSystem.TransferMaliYear && (old?.SanadSt ?? SanadSt.None) == SanadSt.Daemi)
			{
				throw new Exception("سند دائمی قابل ویرایش نیست.".GetTranslate());
			}
			line = 6;
			if (old != null && autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 != Document.Sanad))
			{
				item.Description = old.Description;
				item.SanadSt = old.SanadSt;
				item.UserGuid = old.UserGuid;
			}
			line = 7;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			if (old != null && autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 == Document.Sanad))
			{
				item.Guid = old.Guid;
				Guid asGuid = autoSanadItemsForSave.Select((Tuple<Document, Guid> p) => p.Item2).Distinct().Single();
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _manualArticles.RemoveRangeAsync(asGuid, Document.Sanad, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 893);
			}
			if (res.HasError)
			{
				return res;
			}
			line = 8;
			Tuple<Document, Guid>[] array = autoSanadItemsForSave;
			foreach (Tuple<Document, Guid> p2 in array)
			{
				IEnumerable<Sanad_DetBusiness> containsItems = item.ListSanad_Det.Where((Sanad_DetBusiness q) => q.AsType == p2.Item1);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _autoArticles.RemoveAutoArticlesAsync(p2.Item1, p2.Item2, containsItems, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 900);
				if (res.HasError)
				{
					return res;
				}
				if (system != EnSystem.Transfer && p2.Item2 != Guid.Empty && item.SanadType != SanadType.Note)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _coverRepo.RemoveArticlesAsync(p2.Item2, p2.Item1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 903);
				}
			}
			if (res.HasError)
			{
				return res;
			}
			if (item.SanadNo <= 0)
			{
				item.SanadNo = await item.GetSanadNoAsync(token, tranName);
			}
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@SanadNo", (object)item.SanadNo),
				new SqlParameter("@SanadSt", (object)(short)item.SanadSt),
				new SqlParameter("@Date", (object)item.DateM),
				new SqlParameter("@Desc", (object)(item.Description ?? "")),
				new SqlParameter("@Type", (object)(short)item.SanadType),
				new SqlParameter("@Guid", (object)item.Guid),
				new SqlParameter("@CashierGuid", (object)item.UserGuid),
				new SqlParameter("@StatusId", (object)item.StatusId)
			};
			line = 9;
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Acc_Sanad_Save", parameters, tranName, token, "", CommandType.StoredProcedure, 1800, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 920);
			if (autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 == Document.Sanad))
			{
				line = 11;
				if ((item?.ListSanad_Det?.Any()).GetValueOrDefault())
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _manualArticles.SaveRangeAsync(item.ListSanad_Det, item, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 926);
				}
				line = 12;
				if (res.HasError)
				{
					return res;
				}
				line = 13;
			}
			if (item.SanadType != SanadType.Note)
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _autoArticles.SaveRangeAsync(caller + "=>SanadPersistanceRepository.SaveAsync", item, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 932);
			}
			line = 14;
			if (res.HasError)
			{
				return res;
			}
			if ((lstCover?.Any() ?? false) && item.SanadType != SanadType.Note)
			{
				foreach (ViewSanadDetailCoverBusiness item2 in lstCover)
				{
					if (item2.SanadNo <= 0)
					{
						item2.SanadNo = sanadNo;
					}
					if (item2.AsTypeDocument == Document.Sanad && item2.Id2 <= 0)
					{
						item2.Id2 = sanadNo;
					}
				}
			}
			if (system != EnSystem.Transfer && system != EnSystem.TransferMaliYear && item.SanadType != SanadType.Note)
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _coverRepo.SaveArticlesAsync(lstCover, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 942);
			}
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 944), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 944);
			if (res.HasError)
			{
				return res;
			}
			if (autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 == Document.Sanad))
			{
				int i = await item.GetSanadNoAsync(token, tranName);
				int num = ((old != null) ? (await old.GetSanadNoAsync(token, tranName)) : 0);
				int oldSanadNo2 = num;
				if (res.HasError)
				{
					return res;
				}
				await UpdateVSanadDetailAsync(1, item.Guid, i, oldSanadNo2, old?.DateSh, item.UserGuid, 10, token, tranName);
			}
			if (res.HasError || system == EnSystem.Transfer || system == EnSystem.TransferMaliYear)
			{
				return res;
			}
			SanadEventAggregator aggregator = ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.Sanad;
			if (aggregator.HasEventHandler && (autoSanadItemsForSave?.Any() ?? false))
			{
				Task.Factory.StartNew(delegate
				{
					try
					{
						Tuple<Document, Guid>[] array2 = autoSanadItemsForSave;
						foreach (Tuple<Document, Guid> tuple in array2)
						{
							aggregator.RaiseSanadChanged(tuple.Item2, tuple.Item1, (old == null) ? ENAction.Add : ENAction.Edit);
						}
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (Exception ex3)
					{
						if (ex3.Message.Contains("Operation cancelled by user."))
						{
							throw new OperationCanceledException();
						}
						WebErrorLog.Instance.StartLog(ex3, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 968);
						throw;
					}
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"SanadBusiness item, int oldSanadNo, string tranName, params Tuple<Document, Guid>[] autoSanadItemsForSave \r\n #Line_{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 976);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 977);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveDuplicateSanadNoAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			string query = "delete Acc_Sanad from Acc_Sanad left outer join Acc_ViewSanadDetail on Acc_ViewSanadDetail.SanadNo=Acc_Sanad.SanadNo where Acc_ViewSanadDetail.Guid is null and Acc_Sanad.SanadType<>8";
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 987);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync(query, null, tranName, token, "", CommandType.Text, 30, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs"), ReturnedState.None, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 988);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 990), ReturnedState.None, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 990);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 994);
			res.AddReturnedValue(ex, "", "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 995);
		}
		return res;
	}

	public override async Task<int> UpdateVSanadDetailAsync(short mode, Guid asGuid, int sanadNo, int oldSanadNo, string oldDateSh, Guid cashierGuid, short tryCount, CancellationToken token, string tranName)
	{
		int res = 0;
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1002);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ModifiedMode", (object)mode),
				new SqlParameter("@Guid", (object)asGuid),
				new SqlParameter("@SanadNO", (object)sanadNo),
				new SqlParameter("@OLDSanadNO", (object)oldSanadNo),
				new SqlParameter("@cashierGuid", (object)cashierGuid),
				new SqlParameter("@OLDDateSH", (object)oldDateSh)
			};
			object obj = await c.ExecuteScalarAsync<object>("SP_Acc_Sanad_Update_50_Manual", parameters, tranName, token, 1800, "UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			if (obj != null)
			{
				res = (int)obj;
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
			WebErrorLog.Instance.StartLog(ex2, "", "UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1021);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				c?.DisposeAsync("UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return res;
	}

	private SanadReportBusiness LoadSanadReportBusiness(DbDataReader dr, CancellationToken token)
	{
		SanadReportBusiness sanadReportBusiness = new SanadReportBusiness(ContextGuid);
		try
		{
			if (dr["SANADNO"] != DBNull.Value)
			{
				sanadReportBusiness.SanadNo = (int)dr["SANADNO"];
			}
			if (dr["SANADST"] != DBNull.Value)
			{
				sanadReportBusiness.SanadSt = (SanadSt)dr["SANADST"];
			}
			if (dr["SANADID"] != DBNull.Value)
			{
				sanadReportBusiness.SanadId = (int)dr["SANADID"];
			}
			if (dr["SANADTYPE"] != DBNull.Value)
			{
				switch ((short)dr["SANADTYPE"])
				{
				case 1:
					sanadReportBusiness.SanadType = SanadType.Eftetahiye;
					break;
				case 2:
					sanadReportBusiness.SanadType = SanadType.Movaghat;
					break;
				case 3:
					sanadReportBusiness.SanadType = SanadType.CostAndBenefit;
					break;
				case 4:
					sanadReportBusiness.SanadType = SanadType.Automatic;
					break;
				case 6:
					sanadReportBusiness.SanadType = SanadType.Ekhtetamiye;
					break;
				case 7:
					sanadReportBusiness.SanadType = SanadType.Draft;
					break;
				case 8:
					sanadReportBusiness.SanadType = SanadType.Note;
					break;
				default:
					sanadReportBusiness.SanadType = SanadType.Movaghat;
					break;
				}
			}
			else
			{
				sanadReportBusiness.SanadType = SanadType.Movaghat;
			}
			if (dr["DATEM"] != DBNull.Value)
			{
				sanadReportBusiness.DateM = (DateTime)dr["DATEM"];
			}
			if (dr["DESCRIPTION"] != DBNull.Value)
			{
				sanadReportBusiness.Description = dr["DESCRIPTION"].ToString();
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				sanadReportBusiness.UserGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["SUM"] != DBNull.Value)
			{
				sanadReportBusiness.Sum = (decimal)dr["SUM"];
			}
			if (dr["DateSH"] != DBNull.Value)
			{
				sanadReportBusiness.DateSh = dr["DateSH"].ToString();
			}
			if (string.IsNullOrEmpty(sanadReportBusiness.DateSh.Trim()))
			{
				sanadReportBusiness.DateSh = sanadReportBusiness.DateM.MiladiToCalture();
			}
			sanadReportBusiness.IsChecked = false;
			if (dr["Mode"] != DBNull.Value)
			{
				sanadReportBusiness.Mode = (SanadMode)(int)dr["Mode"];
			}
			if (dr["StatusId"] != DBNull.Value)
			{
				sanadReportBusiness.StatusId = (short)dr["StatusId"];
			}
			if (dr["UserName"] != DBNull.Value)
			{
				sanadReportBusiness.UserName = dr["UserName"].ToString();
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadSanadReportBusiness", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1072);
			throw;
		}
		return sanadReportBusiness;
	}

	private SanadDetRoznameKolBusiness LoadDataRoznameKol(string contextGuid, DbDataReader dr, bool isLoadSanadType)
	{
		SanadDetRoznameKolBusiness sanadDetRoznameKolBusiness = new SanadDetRoznameKolBusiness(contextGuid);
		try
		{
			if (dr["KolName"] != DBNull.Value)
			{
				sanadDetRoznameKolBusiness.KolName = dr["KolName"].ToString();
			}
			if (dr["CodeKol"] != DBNull.Value)
			{
				sanadDetRoznameKolBusiness.CodeKol = dr["CodeKol"].ToString().ParseToLong(0L);
			}
			if (dr["Debit"] != DBNull.Value)
			{
				sanadDetRoznameKolBusiness.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				sanadDetRoznameKolBusiness.Credit = (decimal)dr["Credit"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				sanadDetRoznameKolBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (isLoadSanadType && dr["SanadType"] != DBNull.Value)
			{
				sanadDetRoznameKolBusiness.SanadType = (SanadType)(int)dr["SanadType"];
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataRoznameKol", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1094);
			throw;
		}
		return sanadDetRoznameKolBusiness;
	}

	private MoeinTafsilReportModel LoadDataMoeinTafsilReport(DbDataReader dr)
	{
		MoeinTafsilReportModel moeinTafsilReportModel = new MoeinTafsilReportModel();
		try
		{
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				moeinTafsilReportModel.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				moeinTafsilReportModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["Debit_FD"] != DBNull.Value)
			{
				moeinTafsilReportModel.Debit_FD = (decimal)dr["Debit_FD"];
			}
			if (dr["Credit_FD"] != DBNull.Value)
			{
				moeinTafsilReportModel.Credit_FD = (decimal)dr["Credit_FD"];
			}
			if (dr["Count_FD"] != DBNull.Value)
			{
				moeinTafsilReportModel.Count_FD = dr["Count_FD"].ToString().ParseToInt();
			}
			if (dr["Debit_TD"] != DBNull.Value)
			{
				moeinTafsilReportModel.Debit_TD = (decimal)dr["Debit_TD"];
			}
			if (dr["Credit_TD"] != DBNull.Value)
			{
				moeinTafsilReportModel.Credit_TD = (decimal)dr["Credit_TD"];
			}
			if (dr["Count_TD"] != DBNull.Value)
			{
				moeinTafsilReportModel.Count_TD = dr["Count_TD"].ToString().ParseToInt();
			}
			if (dr["Debit_ED"] != DBNull.Value)
			{
				moeinTafsilReportModel.Debit_ED = (decimal)dr["Debit_ED"];
			}
			if (dr["Credit_ED"] != DBNull.Value)
			{
				moeinTafsilReportModel.Credit_ED = (decimal)dr["Credit_ED"];
			}
			if (dr["Count_ED"] != DBNull.Value)
			{
				moeinTafsilReportModel.Count_ED = dr["Count_ED"].ToString().ParseToInt();
			}
			if (dr["Moein_Code"] != DBNull.Value)
			{
				moeinTafsilReportModel.MoeinCode = (long)dr["Moein_Code"];
			}
			if (dr["Moein_Name"] != DBNull.Value)
			{
				moeinTafsilReportModel.MoeinName = dr["Moein_Name"].ToString();
			}
			if (dr["Moein_Account"] != DBNull.Value)
			{
				moeinTafsilReportModel.MoeinAccount = (decimal)dr["Moein_Account"];
			}
			if (dr["Tafsil_Code"] != DBNull.Value)
			{
				moeinTafsilReportModel.TafsilCode = (long)dr["Tafsil_Code"];
			}
			if (dr["Tafsil_Name"] != DBNull.Value)
			{
				moeinTafsilReportModel.TafsilName = dr["Tafsil_Name"].ToString();
			}
			if (dr["Tafsil_Account"] != DBNull.Value)
			{
				moeinTafsilReportModel.TafsilAccount = (decimal)dr["Tafsil_Account"];
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataMoeinTafsilReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1126);
			throw;
		}
		return moeinTafsilReportModel;
	}

	private SanadDetReportBusiness LoadDataReport(string contextGuid, DbDataReader dr, int index, Guid? kolGuid, Guid? moeinGuid, Guid? tafsilGuid, bool isFromGardesh, CancellationToken token)
	{
		SanadDetReportBusiness sanadDetReportBusiness = new SanadDetReportBusiness(contextGuid);
		try
		{
			token.ThrowIfCancellationRequested();
			sanadDetReportBusiness.RowIndex = index;
			sanadDetReportBusiness.Guid = Guid.NewGuid();
			if (dr["AsGuid"] != DBNull.Value)
			{
				sanadDetReportBusiness.AsGuid = (Guid)dr["AsGuid"];
			}
			if (dr["AsType"] != DBNull.Value)
			{
				sanadDetReportBusiness.AsType = dr["AsType"].ToString().ParseToShort(0);
			}
			if (dr["Credit"] != DBNull.Value)
			{
				sanadDetReportBusiness.Credit = (decimal)dr["Credit"];
			}
			if (dr["Debit"] != DBNull.Value)
			{
				sanadDetReportBusiness.Debit = (decimal)dr["Debit"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				sanadDetReportBusiness.DateM = (DateTime)dr["Date"];
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				sanadDetReportBusiness.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (!isFromGardesh)
			{
				if (dr["RDesc"] != DBNull.Value)
				{
					sanadDetReportBusiness.Description = dr["RDesc"].ToString();
				}
				if (string.IsNullOrEmpty(sanadDetReportBusiness.Description.Trim()) && dr["SanadDesc"] != DBNull.Value)
				{
					sanadDetReportBusiness.Description = dr["SanadDesc"].ToString();
				}
			}
			else if (dr["SanadDesc"] != DBNull.Value)
			{
				sanadDetReportBusiness.Description = dr["SanadDesc"].ToString();
			}
			if (dr["Qt"] != DBNull.Value)
			{
				sanadDetReportBusiness.Qt = dr["Qt"].ToString().ParseTofloat(0f, 3);
			}
			if (dr["SanadNo"] != DBNull.Value && (int)dr["SanadNo"] > 0 && sanadDetReportBusiness.AsType != 206)
			{
				sanadDetReportBusiness.SanadNo = dr["SanadNo"].ToString();
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				sanadDetReportBusiness.UserGuid = (Guid)dr["CashierGuid"];
			}
			if (sanadDetReportBusiness.Credit > 0m || sanadDetReportBusiness.AsType == 206)
			{
				if (dr["CreditTafsilGuid"] != DBNull.Value)
				{
					sanadDetReportBusiness.TafsilGuid = (Guid)dr["CreditTafsilGuid"];
				}
				if (dr["CreditMoeinGuid"] != DBNull.Value)
				{
					sanadDetReportBusiness.MoeinGuid = (Guid)dr["CreditMoeinGuid"];
				}
			}
			else
			{
				if (dr["DebitTafsilGuid"] != DBNull.Value)
				{
					sanadDetReportBusiness.TafsilGuid = (Guid)dr["DebitTafsilGuid"];
				}
				if (dr["DebitMoeinGuid"] != DBNull.Value)
				{
					sanadDetReportBusiness.MoeinGuid = (Guid)dr["DebitMoeinGuid"];
				}
			}
			if (isFromGardesh)
			{
				if (dr["MoeinGuid"] != DBNull.Value)
				{
					sanadDetReportBusiness.MoeinGuid = (Guid)dr["MoeinGuid"];
				}
				if (dr["TafsilGuid"] != DBNull.Value)
				{
					sanadDetReportBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
				}
				if (dr["AsGuid"] != DBNull.Value)
				{
					sanadDetReportBusiness.AsGuid = (Guid)dr["AsGuid"];
				}
			}
			if (sanadDetReportBusiness.KolGuid == Guid.Empty && kolGuid.HasValue)
			{
				sanadDetReportBusiness.KolGuid = kolGuid.Value;
			}
			if (sanadDetReportBusiness.MoeinGuid == Guid.Empty && moeinGuid.HasValue)
			{
				sanadDetReportBusiness.MoeinGuid = moeinGuid.Value;
			}
			if (sanadDetReportBusiness.TafsilGuid == Guid.Empty && tafsilGuid.HasValue)
			{
				sanadDetReportBusiness.TafsilGuid = tafsilGuid.Value;
			}
			if (dr["UserName"] != DBNull.Value)
			{
				sanadDetReportBusiness.UserName = dr["UserName"].ToString();
			}
			return sanadDetReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1185);
			throw;
		}
	}

	private async Task<SanadBusiness> LoadDataAsync(DbDataReader dr, bool isLoadDets, CancellationToken token, string tranName = "")
	{
		_ = 4;
		try
		{
			int num = (int)dr["SanadNo"];
			if (num <= 0)
			{
				num = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
			}
			SanadBusiness item = new SanadBusiness(ContextGuid, num);
			if (dr["SanadNo"] != DBNull.Value)
			{
				item.SanadNo = num;
			}
			if (dr["SanadST"] != DBNull.Value)
			{
				item.SanadSt = (SanadSt)dr["SanadST"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				item.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["GUID"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["GUID"];
			}
			if (dr["UserGuid"] != DBNull.Value)
			{
				item.UserGuid = (Guid)dr["UserGuid"];
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				item.SanadType = (SanadType)(short)dr["SanadType"];
			}
			item.ChangeStatus = EntityChangeSt.Edited;
			if (isLoadDets)
			{
				ViewSanadDetailPersistenceRepositoryBase autoArticles = _autoArticles;
				List<WebSanad_Det> source = await autoArticles.GetAllSanadDetailsAsync(await item.GetSanadNoAsync(token, tranName), token, tranName);
				SanadBusiness sanadBusiness = item;
				await sanadBusiness.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(ContextGuid, source, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.None, appendCurrentItems: false, token);
			}
			return item;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1215);
			throw;
		}
	}

	private async Task<Sanad_DetBusiness> LoadDataVSanadDetail(DbDataReader dr, Guid asGuid, Document asType, int ordinal, CancellationToken token, string tranName)
	{
		Sanad_DetBusiness item = new Sanad_DetBusiness(ContextGuid, EnSystem.None, asGuid, asType, ordinal);
		try
		{
			if (dr["Description"] != DBNull.Value)
			{
				item.RowDesc = dr["Description"].ToString();
			}
			if (dr["Debit"] != DBNull.Value)
			{
				item.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				item.Credit = (decimal)dr["Credit"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["GUID"];
			}
			if (dr["QT"] != DBNull.Value)
			{
				item.Qt = dr["QT"].ToString().ParseToDecimal();
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				item.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				await item.SetMoeinGuidAsync((Guid)dr["MoeinGuid"], token, tranName);
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				await item.SetTafsilGuidAsync((Guid)dr["TafsilGuid"], tranName, token);
			}
			if (dr["TafsilGuid2"] != DBNull.Value)
			{
				await item.SetTafsil2GuidAsync((Guid)dr["TafsilGuid2"], tranName, token);
			}
			if (dr["TafsilGuid3"] != DBNull.Value)
			{
				await item.SetTafsil3GuidAsync((Guid)dr["TafsilGuid3"], tranName, token);
			}
			if (dr["TafsilGuid4"] != DBNull.Value)
			{
				await item.SetTafsil4GuidAsync((Guid)dr["TafsilGuid4"], tranName, token);
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataVSanadDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1240);
			throw;
		}
		return item;
	}

	public override async Task<DateTime?> GetSanadBargashtDateAsync(decimal price, Guid debitTafsilGuid, Guid creditTafsilGuid, int sanadNo, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetSanadBargashtDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1247);
		DateTime? result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				if (!((await SqlServerTableInformation.GetAllAsync(ContextGuid.GetSqlConnectionString(), "VSanadDetail"))?.Any() ?? false))
				{
					result = null;
				}
				else if ((await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(ContextGuid), "GetBargashDate", checkCrcBeforeExecution: true, ENSource.None, Resources.GetBargashtDate, ContextGuid.GetSqlConnectionString())).HasError)
				{
					result = null;
				}
				else
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Price", (object)price),
						new SqlParameter("@DebitTafsilGuid", (object)debitTafsilGuid),
						new SqlParameter("@CreditTafsilGuid", (object)creditTafsilGuid)
					};
					object obj = await c.ExecuteScalarAsync<object>("sp_ViewSanadDetail_GetBargashtDate", parameters, tranName, token, 30, "GetSanadBargashtDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
					result = ((obj == null || string.IsNullOrEmpty(obj.ToString())) ? null : new DateTime?((DateTime)obj));
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetSanadBargashtDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1272);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSanadBargashtDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<List<Sanad_DetBusiness>> GetSanadTemporaryAsync(CancellationToken token, string tranName)
	{
		List<Sanad_DetBusiness> list = new List<Sanad_DetBusiness>();
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetSanadTemporaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1284));
		try
		{
			int num;
			_ = num - 1;
			_ = 4;
			try
			{
				List<Guid> lst = new List<Guid>
				{
					ParentDefaults.HesabTypeGuids.Darayi,
					ParentDefaults.HesabTypeGuids.Bedehi,
					ParentDefaults.HesabTypeGuids.Sarmaye,
					ParentDefaults.HesabTypeGuids.HesabHayeEntezami
				};
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(ContextGuid, lst, token, tranName);
				returnedSaveFuncInfoWithValue.throwExceptionIfError();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@TypeGuidsRefrencedId", (object)returnedSaveFuncInfoWithValue.value)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_Transfer_SanadCloseTemporaryAccounts", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetSanadTemporaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				int ordinal = 0;
				while (await dr.ReadAsync())
				{
					Sanad_DetBusiness det = new Sanad_DetBusiness(dbContext.Context.Guid, EnSystem.None, Guid.Empty, Document.Sanad, ordinal++);
					decimal num2 = (decimal)dr["SumDebit"];
					decimal num3 = (decimal)dr["SumCredit"] - num2;
					if (num3 < 0m)
					{
						det.Debit = -num3;
					}
					else
					{
						if (!(num3 > 0m))
						{
							continue;
						}
						det.Credit = num3;
					}
					await det.SetMoeinGuidAsync((Guid)dr["MoeinGuid"], token, tranName);
					if (dr["TafsilGuid"] != DBNull.Value)
					{
						await det.SetTafsilGuidAsync((Guid)dr["TafsilGuid"], tranName, token);
					}
					list.Add(det);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetSanadTemporaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1320);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSanadTemporaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<IEnumerable<SanadGhestViewModel>> GetAllSanadGhestAsync(Guid sanadGuid, CancellationToken token, string tranName)
	{
		List<SanadGhestViewModel> list = new List<SanadGhestViewModel>();
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllSanadGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1333));
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			IEnumerable<SanadGhestViewModel> result;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadGuid", (object)sanadGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Sanad_Ghest_TopGrid", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllSanadGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
				while (true)
				{
					if (!(await dr.ReadAsync()))
					{
						dr.Close();
						break;
					}
					if (!token.IsCancellationRequested && !base.IsDisposed)
					{
						list.Add(LoadDataSanadGhest(dr));
						continue;
					}
					result = null;
					goto end_IL_0000;
				}
				goto end_IL_00c8;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllSanadGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1349);
				throw;
			}
			return result;
			end_IL_00c8:;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllSanadGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs");
			}
		}
		return list;
	}

	private SanadGhestViewModel LoadDataSanadGhest(DbDataReader dr)
	{
		try
		{
			SanadGhestViewModel sanadGhestViewModel = new SanadGhestViewModel();
			if (dr["Guid"] != DBNull.Value)
			{
				sanadGhestViewModel.TafsilGuid = (Guid)dr["Guid"];
			}
			if (dr["Code"] != DBNull.Value)
			{
				sanadGhestViewModel.TafsilCode = (long)dr["Code"];
			}
			if (dr["Name"] != DBNull.Value)
			{
				sanadGhestViewModel.TafsilName = dr["Name"].ToString();
			}
			if (dr["AghsatCount"] != DBNull.Value)
			{
				sanadGhestViewModel.AghsatCount = (int)dr["AghsatCount"];
			}
			if (dr["TotalAghsatSum"] != DBNull.Value)
			{
				sanadGhestViewModel.TotalAghsatSum = (decimal)dr["TotalAghsatSum"];
			}
			if (dr["SanadCreditSum"] != DBNull.Value)
			{
				sanadGhestViewModel.SanadCreditSum = (decimal)dr["SanadCreditSum"];
			}
			if (dr["SanadDebitSum"] != DBNull.Value)
			{
				sanadGhestViewModel.SanadDebitSum = (decimal)dr["SanadDebitSum"];
			}
			return sanadGhestViewModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataSanadGhest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepository.cs", 1377);
			throw;
		}
	}
}
