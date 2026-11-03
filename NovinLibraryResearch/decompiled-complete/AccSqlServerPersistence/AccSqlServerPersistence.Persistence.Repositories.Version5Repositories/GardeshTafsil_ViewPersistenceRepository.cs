using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Mapping;
using Serilog;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories;

internal class GardeshTafsil_ViewPersistenceRepository : PersistenceRepository<GardeshTafsilBusiness, GardeshTafsil>, IGardeshTafsilRepository, IRepository<GardeshTafsilBusiness>, INoGuidRepository<GardeshTafsilBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<GardeshTafsilBusiness, GardeshTafsil>, IItemAsyncMapper<GardeshTafsil, GardeshTafsilBusiness>
	{
		public Task<GardeshTafsil> MapAsync(string contextGuid, GardeshTafsilBusiness source, GardeshTafsil destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new GardeshTafsil();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Date = source.Date;
					destination.DepotGuid = source.DepotGuid;
					destination.Description = source.Description;
					destination.AsGuid = source.AsGuid;
					destination.AsType = source.AsType;
					destination.EnAsType = source.EnAsType;
					destination.AsTypeEnum = source.AsTypeEnum;
					destination.Debit = source.Debit;
					destination.Credit = source.Credit;
					destination.SanadNO = source.SanadNO;
					destination.SanadSt = source.SanadSt;
					destination.Serial = source.Serial;
					destination.Price = source.Price;
					destination.Discount = source.Discount;
					destination.DiscountPercent = source.DiscountPercent;
					destination.Id2 = source.Id2;
					destination.QT = source.QT;
					destination.QT2 = source.QT2;
					destination.Bazaryab = source.Bazaryab;
					destination.VGuid = source.VGuid;
					destination.PrdGuid = source.GetPrdGuid();
					destination.SanadDesc = source.SanadDesc;
					destination.DateMoaser = source.DateMoaser;
					destination.Rem = source.Rem;
					destination.DecimalGuid = source.DecimalGuid;
					destination.RowDesc = source.RowDesc;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.KolGuid = source.KolGuid;
					destination.UGuid = source.UGuid;
					destination.DecimalPrice = source.DecimalPrice;
					destination.DecimalQt = source.DecimalQt;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 501);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public async Task<GardeshTafsilBusiness> MapAsync(string contextGuid, GardeshTafsil source, GardeshTafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new GardeshTafsilBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Date = source.Date;
					destination.DepotGuid = source.DepotGuid;
					destination.Description = source.Description;
					destination.AsGuid = source.AsGuid;
					destination.AsType = source.AsType;
					destination.EnAsType = source.EnAsType;
					destination.AsTypeEnum = source.AsTypeEnum;
					destination.Debit = source.Debit;
					destination.Credit = source.Credit;
					destination.SanadNO = source.SanadNO;
					destination.SanadSt = source.SanadSt;
					destination.Serial = source.Serial;
					destination.Price = source.Price;
					destination.Discount = source.Discount;
					destination.Id2 = source.Id2;
					destination.QT = source.QT;
					destination.QT2 = source.QT2;
					destination.Bazaryab = source.Bazaryab;
					destination.VGuid = source.VGuid;
					await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.None, setProduct: false);
					destination.SanadDesc = source.SanadDesc;
					destination.DateMoaser = source.DateMoaser;
					destination.Rem = source.Rem;
					destination.DecimalGuid = source.DecimalGuid;
					destination.RowDesc = source.RowDesc;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.KolGuid = source.KolGuid;
					destination.UGuid = source.UGuid;
					destination.DecimalPrice = source.DecimalPrice;
					destination.DecimalQt = source.DecimalQt;
					destination.DecimalQt = source.DecimalQt;
					destination.RowIndex = source.RowIndex;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 559);
					throw;
				}
				return destination;
			}
		}

		public Task<List<GardeshTafsil>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> source, List<GardeshTafsil> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<GardeshTafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsil> source, List<GardeshTafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<GardeshTafsil, GardeshTafsilBusiness> MapFromDatabase { get; }

	public override IItemAsyncMapper<GardeshTafsilBusiness, GardeshTafsil> MapToDatabase { get; }

	public override IItemMapper<GardeshTafsil, GardeshTafsilBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public GardeshTafsil_ViewPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<GardeshTafsilBusiness>> GetAllAsync(GardeshTafsiliFilter filter, EnSystem system, string tranName, CancellationToken token)
	{
		int line = 0;
		List<GardeshTafsilBusiness> list = new List<GardeshTafsilBusiness>();
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
		{
			Stopwatch sw = Stopwatch.StartNew();
			try
			{
				filter.MoeinGuids = filter?.MoeinGuids?.Where((Guid q) => q != Guid.Empty)?.ToList();
				SqlConnection cn = new SqlConnection(ContextGuid.GetSqlConnectionString());
				try
				{
					line = 1;
					SqlCommand val = new SqlCommand("sp_GardeshTafsil_GetAllFromWebVeiw", cn);
					((DbCommand)val).CommandType = CommandType.StoredProcedure;
					SqlCommand cmd = val;
					try
					{
						if (filter.TafsilGuid.HasValue)
						{
							cmd.Parameters.AddWithValue("@TafsilGuid", (object)filter.TafsilGuid);
						}
						if (filter.MoeinGuids?.Any() ?? false)
						{
							cmd.Parameters.AddWithValue("@MoeinGuid", (object)filter.MoeinGuids.FirstOrDefault());
						}
						if (filter.KolGuid.HasValue)
						{
							cmd.Parameters.AddWithValue("@KolGuid", (object)filter.KolGuid);
						}
						line = 5;
						if (filter.Date1.HasValue)
						{
							cmd.Parameters.AddWithValue("@Date1", (object)filter.Date1);
						}
						if (filter.Date2.HasValue)
						{
							cmd.Parameters.AddWithValue("@Date2", (object)filter.Date2);
						}
						cmd.Parameters.AddWithValue("@MaxCount", (object)filter.MaxCount.GetValueOrDefault(999999));
						cmd.Parameters.AddWithValue("@UpdateSood", (object)filter.IsLoadSood);
						if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
						{
							cmd.Parameters.AddWithValue("@CashierGuid", (object)filter.CashierGuid.Value);
						}
						if ((filter.TafsilGuid2 ?? Guid.Empty) != Guid.Empty)
						{
							cmd.Parameters.AddWithValue("@TafsilGuid2", (object)filter.TafsilGuid2.Value);
						}
						if ((filter.TafsilGuid3 ?? Guid.Empty) != Guid.Empty)
						{
							cmd.Parameters.AddWithValue("@TafsilGuid3", (object)filter.TafsilGuid3.Value);
						}
						if ((filter.TafsilGuid4 ?? Guid.Empty) != Guid.Empty)
						{
							cmd.Parameters.AddWithValue("@TafsilGuid4", (object)filter.TafsilGuid4.Value);
						}
						line = 10;
						((DbCommand)(object)cmd).CommandTimeout = 180000;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						line = 11;
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							line = 12;
							int rowIndex = 0;
							while (await ((DbDataReader)(object)dr).ReadAsync(token))
							{
								line = 13;
								if (token.IsCancellationRequested)
								{
									return null;
								}
								GardeshTafsilBusiness gardeshTafsilBusiness = LoadDataFromDataReader((DbDataReader)(object)dr, isLoadSood: true, loadIsEnteghalHesab: true);
								rowIndex = (gardeshTafsilBusiness.RowIndex = rowIndex + 1);
								list.Add(gardeshTafsilBusiness);
								line = 14;
							}
							line = 15;
							((DbDataReader)(object)dr).Close();
							line = 16;
							((DbConnection)(object)cn).Close();
							list.Where((GardeshTafsilBusiness q) => q.SanadNO <= 0).ToList();
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
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line} filter:{Json.ToStringJson(filter)}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 93);
				throw;
			}
			finally
			{
				ILogger obj = LogCache.Instance.Value.Get("UiLogger");
				if (obj != null)
				{
					obj.Debug($"GardeshTafsilViewPersistenceRepository.GetAllAsync finiehsed on {sw.ElapsedMilliseconds}(ms) records({list?.Count() ?? 0})rows");
				}
			}
			return list;
		}
	}

	public async Task<IEnumerable<GardeshTafsilBusiness>> GetAllFromVSanadDetAsync(List<Guid> listTafsilGuid, EnSystem system, int top, DateTime? date1, DateTime? date2, List<Guid> listMoeinGuid, Guid? kolGuid, bool? status, string tranName, CancellationToken token)
	{
		List<GardeshTafsilBusiness> list = new List<GardeshTafsilBusiness>();
		using (new JaegerMonitor("GetAllFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
		{
			_ = 3;
			try
			{
				if (listTafsilGuid == null || listTafsilGuid.Count <= 0)
				{
					return new List<GardeshTafsilBusiness>();
				}
				if (listMoeinGuid == null || listMoeinGuid.Count <= 0)
				{
					listMoeinGuid = new List<Guid>();
				}
				string text = string.Join(",", listTafsilGuid);
				string text2 = ((listMoeinGuid.Count > 0) ? string.Join(",", listMoeinGuid) : "");
				SqlConnection cn = new SqlConnection(ContextGuid.GetSqlConnectionString());
				try
				{
					SqlCommand val = new SqlCommand("sp_GetGardeshTafsil_FromWebView", cn);
					((DbCommand)val).CommandType = CommandType.StoredProcedure;
					SqlCommand cmd = val;
					try
					{
						cmd.Parameters.AddWithValue("@ListTafsilGuid", (object)text);
						cmd.Parameters.AddWithValue("@ListMoeinGuid", (object)text2);
						cmd.Parameters.AddWithValue("@Date1", (object)date1);
						cmd.Parameters.AddWithValue("@Date2", (object)date2);
						cmd.Parameters.AddWithValue("@Top", (object)top);
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						int rowIndex = 0;
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							if (token.IsCancellationRequested)
							{
								return null;
							}
							GardeshTafsilBusiness gardeshTafsilBusiness = LoadDataFromDataReader((DbDataReader)(object)dr, isLoadSood: false, loadIsEnteghalHesab: false);
							rowIndex = (gardeshTafsilBusiness.RowIndex = rowIndex + 1);
							list.Add(gardeshTafsilBusiness);
						}
						((DbConnection)(object)cn).Close();
						if (system == EnSystem.UserInterface)
						{
							await Task.WhenAll(list.Select((GardeshTafsilBusiness p) => p.SetPrdAsync(system)));
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
				WebErrorLog.Instance.StartLog(ex, "", "GetAllFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 137);
				throw;
			}
			return list;
		}
	}

	public async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> kolGuid, IEnumerable<Guid> moeinGuid, IEnumerable<Guid?> tafsilGuid, short tryCount, string tranName, CancellationToken token)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 148));
			try
			{
				if (tafsilGuid == null || !tafsilGuid.Any())
				{
					return ret;
				}
				foreach (Guid? item in tafsilGuid)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 154);
					}
					if (item.HasValue)
					{
						await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)dbcontext).Database, "EXECUTE sp_Acc_GardeshTafsil_DeleteByTafsilGuid {0}", (IEnumerable<object>)new List<object> { item }, default(CancellationToken));
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 159);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 162);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 163);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(SanadBusiness sanadMaster, short tryCount, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 170);
		try
		{
			if (sanadMaster == null)
			{
				return res;
			}
			if (!(sanadMaster.ListSanad_Det?.Any() ?? false))
			{
				return res;
			}
			foreach (Sanad_DetBusiness item in sanadMaster.ListSanad_Det)
			{
				if (item.AsType == Document.CheckSanad && sanadMaster.SanadNo <= 0)
				{
					sanadMaster.SanadNo = await sanadMaster.GetSanadNoAsync(token, tranName);
				}
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@date", (object)sanadMaster.DateM),
					new SqlParameter("@desc", (object)item.RowDesc),
					new SqlParameter("@asType", (object)(short)item.AsType),
					new SqlParameter("@debit", (object)item.Debit),
					new SqlParameter("@credit", (object)item.Credit),
					new SqlParameter("@sanadNo", (object)sanadMaster.SanadNo),
					new SqlParameter("@sanadSt", (object)(short)sanadMaster.SanadSt),
					new SqlParameter("@guid", (object)item.Guid),
					new SqlParameter("@qt", (object)item.Qt),
					new SqlParameter("@rowDesc", (object)item.RowDesc),
					new SqlParameter("@moeinGuid", (object)item.MoeinGuid),
					new SqlParameter("@cashierGuid", (object)sanadMaster.UserGuid)
				};
				if (item.DecimalGuid.HasValue && item.DecimalGuid != Guid.Empty)
				{
					list.Add(new SqlParameter("@decimalGuid", (object)item.DecimalGuid));
				}
				if (item.TafsilGuid.HasValue && item.TafsilGuid != Guid.Empty)
				{
					list.Add(new SqlParameter("@tafsilGuid", (object)item.TafsilGuid));
				}
				_ = item.AsGuid;
				if (item.AsGuid == Guid.Empty)
				{
					throw new ArgumentException("asGuid is not valid!");
				}
				list.Add(new SqlParameter("@asGuid", (object)item.AsGuid));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_VSanadDetail_Insert", list, tranName, token, "", CommandType.StoredProcedure, 30, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 200);
				token.ThrowIfCancellationRequested();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "sp_VSanadDetail_Insert ", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 206);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 207);
		}
		return res;
	}

	public async Task<decimal> GardeshBallanceAsync(Guid? tafsilGuid, Guid? moeinGuid, DateTime? date1, DateTime? date2, string tranName, CancellationToken token)
	{
		decimal res = default(decimal);
		using (new JaegerMonitor("GardeshBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
		{
			_ = 1;
			try
			{
				SqlConnection cn = new SqlConnection(ContextGuid.GetSqlConnectionString());
				try
				{
					SqlCommand cmd = new SqlCommand("SELECT SUM(ISNULL(DEBIT, 0) - ISNULL(CREDIT, 0)) From VSanadDet_Web Where " + Where(tafsilGuid, moeinGuid, date1, date2), cn);
					try
					{
						await ((DbConnection)(object)cn).OpenAsync(token);
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (!string.IsNullOrEmpty(obj.ToString()))
						{
							res = obj.ToString().ParseToDecimal();
						}
						((DbConnection)(object)cn).Close();
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
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GardeshBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 229);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAutoArticlesAsync(Document asType, Guid asGuid, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 237);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@AsGuid", (object)asGuid),
				new SqlParameter("@AsType", (object)(short)asType)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_VSanadDetail_RemoveRange", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"), ReturnedState.None, "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 245);
			token.ThrowIfCancellationRequested();
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 250);
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 251);
		}
		return ret;
	}

	public async Task<GardeshTafsilBusiness> GetLastTransactionAsync(Guid tafsilGuid, EnSystem system, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
		{
			_ = 2;
			try
			{
				BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 260);
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@TafsilGuid", (object)tafsilGuid)
				};
				using DbDataReader dr = await obj.ExecuteReaderAsync("sp_GardeshTafsil_GetLastTransactionFromWebVeiw", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs");
				GardeshTafsilBusiness data = null;
				if (await dr.ReadAsync(token))
				{
					token.ThrowIfCancellationRequested();
					data = LoadDataFromDataReader(dr, isLoadSood: false, loadIsEnteghalHesab: false);
				}
				dr.Close();
				return data;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (SqlException val)
			{
				SqlException val2 = val;
				if (((Exception)(object)val2).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val2, "SqlException", "GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 277);
				throw;
			}
			catch (SqlException val3)
			{
				SqlException val4 = val3;
				if (((Exception)(object)val4).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val4, "SqlException", "GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 283);
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "other exception", "GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 288);
				throw;
			}
		}
	}

	private string Where(Guid? tafsilGuid, Guid? moeinGuid, DateTime? d1, DateTime? d2)
	{
		string text = "";
		using (new JaegerMonitor("Where", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs"))
		{
			try
			{
				if (tafsilGuid.HasValue)
				{
					string[] obj = new string[6] { text, "((DEBIT>0 and DebitTafsilGuid='", null, null, null, null };
					Guid? guid = tafsilGuid;
					obj[2] = guid.ToString();
					obj[3] = "') or (CREDIT > 0 and CreditTafsilGuid = '";
					guid = tafsilGuid;
					obj[4] = guid.ToString();
					obj[5] = "'))";
					text = string.Concat(obj);
				}
				if (moeinGuid.HasValue)
				{
					if (text != "")
					{
						text += " And ";
					}
					string[] obj2 = new string[6] { text, "((DEBIT>0 and DebitMoeinGuid='", null, null, null, null };
					Guid? guid = moeinGuid;
					obj2[2] = guid.ToString();
					obj2[3] = "') or (CREDIT > 0 and CreditMoeinGuid = '";
					guid = moeinGuid;
					obj2[4] = guid.ToString();
					obj2[5] = "'))";
					text = string.Concat(obj2);
				}
				if (d1.HasValue)
				{
					if (text != "")
					{
						text += " And ";
					}
					string text2 = text;
					DateTime? dateTime = d1;
					text = text2 + "DATE>='" + dateTime.ToString() + "'";
				}
				if (d2.HasValue)
				{
					if (text != "")
					{
						text += " And ";
					}
					string text3 = text;
					DateTime? dateTime = d2;
					text = text3 + "DATE<='" + dateTime.ToString() + "'";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Where", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 319);
				throw;
			}
			return text;
		}
	}

	private GardeshTafsilBusiness LoadDataFromDataReader(DbDataReader dr, bool isLoadSood, bool loadIsEnteghalHesab)
	{
		try
		{
			GardeshTafsilBusiness gardeshTafsilBusiness = new GardeshTafsilBusiness(ContextGuid);
			if (dr["ASTYPE"] != DBNull.Value)
			{
				gardeshTafsilBusiness.AsType = (short)(int)dr["ASTYPE"];
			}
			if (dr["DATE"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Date = (DateTime)dr["DATE"];
			}
			else if (gardeshTafsilBusiness.AsType <= 0)
			{
				gardeshTafsilBusiness.Date = DateTime.MinValue;
			}
			if (dr["ASGuid"] != DBNull.Value)
			{
				gardeshTafsilBusiness.AsGuid = (Guid)dr["ASGuid"];
			}
			if (dr["DEBIT"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Debit = dr["DEBIT"].ToString().ParseToDecimal(0m, ClsCache.GetCache(ContextGuid).MultiLanguageConfig.DigitAfterPoint);
			}
			if (dr["CREDIT"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Credit = dr["CREDIT"].ToString().ParseToDecimal(0m, ClsCache.GetCache(ContextGuid).MultiLanguageConfig.DigitAfterPoint);
			}
			if (gardeshTafsilBusiness.Debit > 0m)
			{
				_ = dr["ASTYPE"];
				if (dr["DebitTafsilGuid"] != DBNull.Value)
				{
					gardeshTafsilBusiness.TafsilGuid = (Guid)dr["DebitTafsilGuid"];
				}
				if (dr["DebitMoeinGuid"] != DBNull.Value)
				{
					gardeshTafsilBusiness.MoeinGuid = (Guid)dr["DebitMoeinGuid"];
				}
			}
			else
			{
				if (dr["CreditTafsilGuid"] != DBNull.Value)
				{
					gardeshTafsilBusiness.TafsilGuid = (Guid)dr["CreditTafsilGuid"];
				}
				if (dr["CreditMoeinGuid"] != DBNull.Value)
				{
					gardeshTafsilBusiness.MoeinGuid = (Guid)dr["CreditMoeinGuid"];
				}
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				gardeshTafsilBusiness.UGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["SANADNO"] != DBNull.Value)
			{
				gardeshTafsilBusiness.SanadNO = (int)dr["SANADNO"];
			}
			if (dr["SANADST"] != DBNull.Value)
			{
				gardeshTafsilBusiness.SanadSt = dr["SANADST"].ToString().ParseToInt();
			}
			if (dr["ID2"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Id2 = (int)dr["ID2"];
			}
			if (dr["QT"] != DBNull.Value)
			{
				gardeshTafsilBusiness.QT = dr["QT"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["Price"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Price = (decimal)dr["Price"];
			}
			if (dr["QT2"] != DBNull.Value)
			{
				gardeshTafsilBusiness.QT2 = dr["QT2"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["RDiscount"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Discount = (decimal)dr["RDiscount"];
			}
			if (dr["RBazaryab"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Bazaryab = (decimal)dr["RBazaryab"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Serial = dr["Serial"].ToString();
			}
			if (dr["SanadDesc"] != DBNull.Value)
			{
				gardeshTafsilBusiness.SanadDesc = dr["SanadDesc"].ToString();
			}
			if (dr["RDesc"] != DBNull.Value)
			{
				gardeshTafsilBusiness.RowDesc = dr["RDesc"].ToString();
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				gardeshTafsilBusiness.SetPrdGuid((Guid)dr["ProductGuid"]);
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				gardeshTafsilBusiness.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["VahedGuid"] != DBNull.Value)
			{
				gardeshTafsilBusiness.VGuid = (Guid)dr["VahedGuid"];
			}
			if (dr["DateMoaser"] != DBNull.Value)
			{
				gardeshTafsilBusiness.DateMoaser = (DateTime)dr["DateMoaser"];
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				gardeshTafsilBusiness.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (dr["DecimalPrice"] != DBNull.Value)
			{
				gardeshTafsilBusiness.DecimalPrice = dr["DecimalPrice"].ToString().ParseToDecimal(0m, ClsCache.GetCache(ContextGuid).MultiLanguageConfig.DigitAfterPoint);
			}
			if (dr["DecimalQT"] != DBNull.Value)
			{
				gardeshTafsilBusiness.DecimalQt = dr["DecimalQT"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["DetailGuid"] != DBNull.Value)
			{
				gardeshTafsilBusiness.DetailGuid = (Guid)dr["DetailGuid"];
			}
			if (loadIsEnteghalHesab && dr["IsEnteghalHesab"] != DBNull.Value)
			{
				gardeshTafsilBusiness.IsEnteghalHesab = (bool)dr["IsEnteghalHesab"];
			}
			if (isLoadSood && dr["Sood"] != DBNull.Value)
			{
				gardeshTafsilBusiness.Sood = (decimal)dr["Sood"];
			}
			return gardeshTafsilBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataFromDataReader", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 378);
			throw;
		}
	}

	public async Task<List<WebSanad_Det>> GetAllSanadDetailsAsync(int sanadNo, string tranName = "")
	{
		List<WebSanad_Det> list = new List<WebSanad_Det>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 385);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadNo", (object)sanadNo)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_VSanadDetail_SelectRow_SanadNO", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDetails(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 396);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs");
			}
		}
		return list;
	}

	private WebSanad_Det LoadDetails(DbDataReader dr)
	{
		WebSanad_Det webSanad_Det = new WebSanad_Det();
		try
		{
			if (dr["Date"] != DBNull.Value)
			{
				webSanad_Det.DateM = (DateTime)dr["Date"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				webSanad_Det.Description = dr["Description"].ToString();
			}
			if (dr["Debit"] != DBNull.Value)
			{
				webSanad_Det.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				webSanad_Det.Credit = (decimal)dr["Credit"];
			}
			if (dr["SanadNO"] != DBNull.Value)
			{
				webSanad_Det.SanadNo = (int)dr["SanadNO"];
			}
			if (dr["SanadST"] != DBNull.Value)
			{
				webSanad_Det.SanadST = (SanadSt)(int)dr["SanadST"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				webSanad_Det.Guid = (Guid)dr["GUID"];
			}
			if (dr["QT"] != DBNull.Value)
			{
				webSanad_Det.Qt = (decimal)(double)dr["QT"];
			}
			if (dr["RowDesc"] != DBNull.Value)
			{
				webSanad_Det.RowDesc = dr["RowDesc"].ToString();
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				webSanad_Det.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				webSanad_Det.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				webSanad_Det.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["AsGuid"] != DBNull.Value)
			{
				webSanad_Det.AsGuid = (Guid)dr["AsGuid"];
			}
			webSanad_Det.SanadGuid = webSanad_Det.Guid;
			if (dr["ASType"] != DBNull.Value)
			{
				webSanad_Det.AsType = (Document)(int)dr["ASType"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDetails", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 429);
			throw;
		}
		return webSanad_Det;
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\GardeshTafsil_ViewPersistenceRepository.cs", 444);
		}
	}
}
