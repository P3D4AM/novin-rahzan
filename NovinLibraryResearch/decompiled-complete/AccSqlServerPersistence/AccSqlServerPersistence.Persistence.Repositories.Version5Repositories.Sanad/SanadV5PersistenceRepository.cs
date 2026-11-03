using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Sanad;

public class SanadV5PersistenceRepository : ISanadRepository, ISyncRepository<SanadBusiness>, IEditableDelivery, IRepository<SanadBusiness>, INoGuidRepository<SanadBusiness>, IDisposable
{
	private bool _isDisposed;

	private static ConcurrentDictionary<string, SanadV5PersistenceRepository> _instance = new ConcurrentDictionary<string, SanadV5PersistenceRepository>();

	private SanadDetailV5PersistenceRepository _manualArticles;

	private GardeshTafsil_ViewPersistenceRepository _autoArticles;

	private SanadDetailV5PersistenceRepository sanadDetRepo;

	public string ContextGuid => Context?.Guid;

	public WebContextHolder Context => DatabaseContext.Context;

	public ContextHolder DatabaseContext { get; private set; }

	public IItemAsyncMapper<Sanads, SanadBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<SanadBusiness, Sanads> MapToDatabase { get; }

	public bool HasStatus { get; }

	public bool HasModified { get; }

	public bool IsDisposed => _isDisposed;

	public SanadV5PersistenceRepository(ContextHolder db)
	{
		_instance.TryAdd(db.Context.Guid, this);
		DatabaseContext = db;
		_manualArticles = new SanadDetailV5PersistenceRepository(db);
		_autoArticles = new GardeshTafsil_ViewPersistenceRepository(db);
	}

	public static SanadV5PersistenceRepository GetInstance(string contextGuid)
	{
		SanadV5PersistenceRepository sanadV5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 55);
			throw;
		}
	}

	public Task<SanadBusiness> GetEkhtetamieSanadAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Sanad_DetBusiness>> GetSanadTempAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<SanadBusiness> GetAsync(int sanadNo, CancellationToken token, string tranName)
	{
		SanadBusiness res = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 74);
		try
		{
			int num;
			_ = num - 1;
			_ = 5;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadNO", (object)sanadNo)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Sanad_SelectRow_SanadNO", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = await LoadDataAsync(dr, isLoadDets: true, token, tranName);
				}
				dr.Close();
				if (res == null)
				{
					List<WebSanad_Det> list = await SanadDetailV5PersistenceRepository.GetInstance(ContextGuid).GetAllSanadDetailsAsync(sanadNo, tranName);
					if (list != null && list.Any())
					{
						WebSanad_Det webSanad_Det = list.FirstOrDefault();
						res = new SanadBusiness(ContextGuid, webSanad_Det.SanadNo)
						{
							Description = webSanad_Det?.Description,
							Status = true,
							DateM = webSanad_Det.DateM,
							SanadSt = webSanad_Det.SanadST,
							Modified = DateTime.Now,
							UserGuid = webSanad_Det.CashierGuid
						};
						if ((webSanad_Det?.AsGuid ?? Guid.Empty) != Guid.Empty)
						{
							res.Guid = webSanad_Det.AsGuid;
						}
						else
						{
							res.Guid = Guid.NewGuid();
						}
						if (webSanad_Det.SanadNo != 0 && (list == null || !list.Any((WebSanad_Det q) => q.AsType == Document.Sanad)))
						{
							res.ChangeStatus = EntityChangeSt.Edited;
						}
						res.SanadNo = sanadNo;
						SanadBusiness sanadBusiness = res;
						await sanadBusiness.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(ContextGuid, list, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.None, appendCurrentItems: false, token);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 111);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<SanadSt> GetSanadStBySanadNoAsync(int SanadNO, CancellationToken token, string tranName)
	{
		SanadSt res = SanadSt.None;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 124);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadNo", (object)SanadNO)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Sanad_GetSanadStBySanadNo", parameters, tranName, token, 30, "GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					res = (SanadSt)obj.ToString().ParseToShort(0);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 134);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<int> UpdateVSanadDetailAsync(short mode, Guid guid, int sanadNo, int oldSanadNo, string oldDateSh, Guid cashierGuid, short tryCount, CancellationToken token, string tranName)
	{
		int res = 0;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 147);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ModifiedMode", (object)mode),
				new SqlParameter("@Guid", (object)guid),
				new SqlParameter("@SanadNO", (object)sanadNo),
				new SqlParameter("@OLDSanadNO", (object)oldSanadNo),
				new SqlParameter("@cashierGuid", (object)cashierGuid),
				new SqlParameter("@OLDDateSH", (object)oldDateSh)
			};
			object obj = await c.ExecuteScalarAsync<object>("SP_Sanad_Update_50_Manual", parameters, tranName, token, 30, "UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			if (obj != null)
			{
				res = (int)obj;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 164);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				c?.DisposeAsync("UpdateVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateVSanadDetailAsync_AnbarGardani(OrderMode mode, Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "UpdateVSanadDetailAsync_AnbarGardani", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 178);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ModifiedMode", (object)(short)mode),
				new SqlParameter("@Guid", (object)guid),
				new SqlParameter("@SanadNO", (object)sanadNo)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("SP_Sanad_Update_24_AnbarGardani", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateVSanadDetailAsync_AnbarGardani", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"), ReturnedState.None, "UpdateVSanadDetailAsync_AnbarGardani", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 187);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateVSanadDetailAsync_AnbarGardani", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 191);
			res.AddReturnedValue(ex, "", "UpdateVSanadDetailAsync_AnbarGardani", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 192);
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				c?.DisposeAsync("UpdateVSanadDetailAsync_AnbarGardani", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<int> GetSanadNoByGuidAsync(Guid Guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SanadSortAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 204);
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("SP_Sanad_Sort", null, tranName, token, "", CommandType.StoredProcedure, 30, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"), ReturnedState.None, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 208);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 212);
				res.AddReturnedValue(ex, "", "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 213);
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<int> NextIdAsync(CancellationToken token, string tranName)
	{
		int res = 0;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "NextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 225);
		try
		{
			_ = 1;
			try
			{
				res = await c.ExecuteScalarAsync<int>("sp_Sanad_NextSanadNO", null, tranName, token, 30, "NextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 232);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("NextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<bool> IsVSanadDetailAsync(int sanadNo, CancellationToken token, string tranName)
	{
		try
		{
			return (await SanadDetailV5PersistenceRepository.GetInstance(ContextGuid).GetAllSanadDetailsAsync(sanadNo, tranName)).Count > 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 252);
			throw;
		}
	}

	public async Task<SanadSt> GetSanadStByGuidTypeAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		SanadSt res = SanadSt.None;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetSanadStByGuidTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 259);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadGuid", (object)sanadGuid),
					new SqlParameter("@SanadType", (object)sanadType)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Sanad_GetSanadStById", parameters, tranName, token, 30, "GetSanadStByGuidTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					res = (SanadSt)obj.ToString().ParseToShort(0);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadStByGuidTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 274);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSanadStByGuidTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<SanadBusiness> GetAsync(Guid SanadGuid, Document SanadType, bool LoadDets, CancellationToken token, string tranName)
	{
		SanadBusiness item = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 287);
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
				using (DbDataReader dr = await c.ExecuteReaderAsync("sp_VSanadDetail_GetByGuidType", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"))
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
						if (dr["Date"] != DBNull.Value && !date.HasValue)
						{
							date = (DateTime)dr["Date"];
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
						using (DbDataReader Sanaddr = await c.ExecuteReaderAsync("sp_Sanad_SelectRow_SanadNO", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"))
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
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 338);
				throw;
			}
			return result;
			end_IL_00d7:;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return item;
	}

	public Task<SanadBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public async Task<IEnumerable<SanadReportBusiness>> GetAllAsync(SanadFilter filter, CancellationToken token, string tranName)
	{
		List<SanadReportBusiness> list = new List<SanadReportBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 352);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
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
					new SqlParameter("@HesabFullCode2", (object)filter.TafsilCodeTo)
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Sanad_Auto_Filter", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadSanadReportBusiness(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 390);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return list;
	}

	private SanadReportBusiness LoadSanadReportBusiness(DbDataReader dr)
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
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadSanadReportBusiness", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 421);
			throw;
		}
	}

	public async Task<List<SanadDetReportBusiness>> GetAllRoznameSanadDetAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		using (new JaegerMonitor("GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 430);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@D1", (object)date1),
						new SqlParameter("@D2", (object)date2)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_VRozname_SelectByDate", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
					int index = 0;
					while (await dr.ReadAsync())
					{
						SanadV5PersistenceRepository sanadV5PersistenceRepository = this;
						string contextGuid = ContextGuid;
						int num2 = index + 1;
						index = num2;
						list.Add(sanadV5PersistenceRepository.LoadDataReport(contextGuid, dr, num2, null, null, null, isFromGardesh: false, token));
					}
					dr.Close();
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 446);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllRoznameSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public async Task<List<SanadDetRoznameKolBusiness>> GetAllSanadDetRoznameKolAsync(EnDaftarRoznameReportType type, DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		List<SanadDetRoznameKolBusiness> list = new List<SanadDetRoznameKolBusiness>();
		using (new JaegerMonitor("GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 461);
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					string text = "";
					switch (type)
					{
					case EnDaftarRoznameReportType.Tajmiee_Days:
						text = "sp_RoznameKOL_Tajmiee_Days";
						break;
					case EnDaftarRoznameReportType.Tajmiee_Month:
						text = "sp_RoznameKOL_Tajmiee_Month";
						break;
					}
					if (string.IsNullOrEmpty(text))
					{
						return null;
					}
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Date1", (object)date1),
						new SqlParameter("@Date2", (object)date2)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync(text, parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
					while (await dr.ReadAsync())
					{
						token.ThrowIfCancellationRequested();
						list.Add(LoadDataRoznameKol(ContextGuid, dr));
					}
					dr.Close();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 488);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllSanadDetRoznameKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public async Task<List<MoeinTafsilReportModel>> GetMoein2TafsilsRptAsync(DateTime date1, DateTime date2, Guid moeinGuid, string tranName, CancellationToken token)
	{
		List<MoeinTafsilReportModel> list = new List<MoeinTafsilReportModel>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 502);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_RPT_Moein2Tafsils", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 522);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetMoein2TafsilsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return list;
	}

	public async Task<List<MoeinTafsilReportModel>> GetTafsil2MoeinsRptAsync(DateTime date1, DateTime date2, Guid tafsilGuid, string tranName, CancellationToken token)
	{
		List<MoeinTafsilReportModel> list = new List<MoeinTafsilReportModel>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 536);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_RPT_Tafsil2Moeins", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 556);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetTafsil2MoeinsRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return list;
	}

	public async Task<List<SanadDetRoznameKolBusiness>> GetTestTarazNameAsync(DateTime date, string tranName, CancellationToken token)
	{
		List<SanadDetRoznameKolBusiness> list = new List<SanadDetRoznameKolBusiness>();
		using (new JaegerMonitor("GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 570);
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
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Hesab_Select_AzTaraz", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
					while (await dr.ReadAsync())
					{
						token.ThrowIfCancellationRequested();
						list.Add(LoadDataRoznameKol(ContextGuid, dr));
					}
					dr.Close();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 585);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetTestTarazNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public Task<SanadBusiness> GetFromDbAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		return GetAsync(sanadGuid, sanadType, LoadDets: true, token, tranName);
	}

	public async Task<List<SanadDetReportBusiness>> GetAllGardeshSanadAsync(ENHesabLevel level, DateTime? date1, DateTime? date2, long? code1, long? code2, long? codeMoein, Guid? kolGuid, string tranName, int? maxCount, short maxParalelThreads, CancellationToken token)
	{
		List<SanadDetReportBusiness> result = new List<SanadDetReportBusiness>();
		using (new JaegerMonitor("GetAllGardeshSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"))
		{
			_ = 2;
			try
			{
				switch (level)
				{
				case ENHesabLevel.Kol:
					result = await GetKolGardeshAsync(code1, code2, date1, date2, maxCount, maxParalelThreads, tranName, token);
					break;
				case ENHesabLevel.Moein:
					result = await GetMoeinGardeshAsync(code1, code2, date1, date2, kolGuid, maxCount, maxParalelThreads, tranName, token);
					break;
				case ENHesabLevel.Tafsil:
					result = await GetTafsilGardeshAsync(code1, code2, codeMoein, date1, date2, maxCount, maxParalelThreads, tranName, token);
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllGardeshSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 618);
				throw;
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid asGuid, Document asType, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 626);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@AsGuid", (object)asGuid),
				new SqlParameter("@AsType", (object)asType)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Sanad_DeleteRowsByAsGuid_AsType", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 634);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 638);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 639);
		}
		return res;
	}

	public async Task<List<MoeinTafsilReportModel>> GetTafsilTajmeeReportAsync(DateTime date1, DateTime date2, string moeinCode1, string moeinCode2, string tafsilCode1, string tafsilCode2, CancellationToken token, string tranName)
	{
		List<MoeinTafsilReportModel> list = new List<MoeinTafsilReportModel>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 646);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_RPT_TajmieTafsili", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataMoeinTafsilReport(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 664);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetTafsilTajmeeReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return list;
	}

	public async Task<List<SanadDetReportBusiness>> GetMoeinTafsilGardeshAsync(DateTime d1, DateTime d2, Guid tafsilGuid, string moeinCode1, string moeinCode2, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		using (new JaegerMonitor("GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 678);
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
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_HesabTafsilMoeins_FillAll_New", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
					int index = 0;
					while (await dr.ReadAsync())
					{
						SanadV5PersistenceRepository sanadV5PersistenceRepository = this;
						string contextGuid = ContextGuid;
						int num2 = index + 1;
						index = num2;
						list.Add(sanadV5PersistenceRepository.LoadDataReport(contextGuid, dr, num2, null, null, tafsilGuid, isFromGardesh: true, token));
					}
					dr.Close();
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 697);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	public async Task<Guid> AsId_To_AsGuidAsync(int asId, Document asType, CancellationToken token, string tranName)
	{
		Guid asGuid = Guid.Empty;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "AsId_To_AsGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 710);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@AsId", (object)asId),
					new SqlParameter("@AsType", (object)asType)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Id_To_Guid", parameters, tranName, token, 30, "AsId_To_AsGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					asGuid = (Guid)obj;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AsId_To_AsGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 724);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("AsId_To_AsGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return asGuid;
	}

	public async Task<ReturnedSaveFuncInfo> ChangeSanadNoAsync(List<int> oldSanadNo, int newSanadNo, string description, DateTime date, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 737);
		try
		{
			int num = (await StimulQueryBusiness.SaveAsync(ContextGuid, oldSanadNo, token, tranName))?.value ?? 0;
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
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Sanad_ChangeSanadNO", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"), ReturnedState.None, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 749);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 753);
			res.AddReturnedValue(ex, "", "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 754);
		}
		return res;
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataMoeinTafsilReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 783);
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
			if (dr["SanadDesc"] != DBNull.Value)
			{
				sanadDetReportBusiness.Description = dr["SanadDesc"].ToString();
			}
			if (dr["Qt"] != DBNull.Value)
			{
				sanadDetReportBusiness.Qt = dr["Qt"].ToString().ParseTofloat(0f, 3);
			}
			if (dr["SanadNo"] != DBNull.Value && (int)dr["SanadNo"] > 0)
			{
				sanadDetReportBusiness.SanadNo = dr["SanadNo"].ToString();
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				sanadDetReportBusiness.UserGuid = (Guid)dr["CashierGuid"];
			}
			if (sanadDetReportBusiness.Credit > 0m)
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
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 832);
			throw;
		}
		return sanadDetReportBusiness;
	}

	private SanadDetRoznameKolBusiness LoadDataRoznameKol(string contextGuid, DbDataReader dr)
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
			if (dr["SanadNO"] != DBNull.Value)
			{
				sanadDetRoznameKolBusiness.SanadNo = (int)dr["SanadNO"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataRoznameKol", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 851);
			throw;
		}
		return sanadDetRoznameKolBusiness;
	}

	private async Task<List<SanadDetReportBusiness>> GetKolGardeshAsync(long? kolCode1, long? kolCode2, DateTime? date1, DateTime? date2, int? maxCount, short maxParalelThreads, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		try
		{
			List<Guid> guidList = new List<Guid>();
			List<KolBusiness> list2 = (await KolBusiness.GetAllBetweenCodesAsync(ContextGuid, kolCode1, kolCode2)).ToList();
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetKolGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 885);
			throw;
		}
	}

	private async Task<List<SanadDetReportBusiness>> GetMoeinGardeshAsync(long? moeinCode1, long? moeinCode2, DateTime? date1, DateTime? date2, Guid? kolGuid, int? maxCount, short maxParalelThreads, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> list = new List<SanadDetReportBusiness>();
		try
		{
			List<Guid> guidList = new List<Guid>();
			List<MoeinBusiness> list2 = (await MoeinBusiness.GetAllBetweenCodesAsync(ContextGuid, moeinCode1, moeinCode2))?.ToList();
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetMoeinGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 922);
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
			}, default(CancellationToken), "", "GetTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			if (enumerable != null && enumerable.Any())
			{
				tafsilGuidList.AddRange(enumerable.Select((TafsilBusiness q) => q.Guid));
			}
			if (moeinCode.HasValue && moeinCode != 0)
			{
				moeinGuid = (await MoeinBusiness.GetAsync(ContextGuid, moeinCode.GetValueOrDefault()))?.Guid;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 964);
			throw;
		}
	}

	private async Task<List<SanadDetReportBusiness>> GetReportAsync(Guid? tafsilGuid, Guid? moeinGuid, Guid? kolGuid, DateTime? date1, DateTime? date2, int? maxCount, string tranName, CancellationToken token)
	{
		List<SanadDetReportBusiness> ret = new List<SanadDetReportBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 972);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_GardeshTafsil_GetAllFromWebVeiw", list, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				int index = 0;
				while (await dr.ReadAsync(token))
				{
					token.ThrowIfCancellationRequested();
					SanadV5PersistenceRepository sanadV5PersistenceRepository = this;
					string contextGuid = ContextGuid;
					int num2 = index + 1;
					index = num2;
					ret.Add(sanadV5PersistenceRepository.LoadDataReport(contextGuid, dr, num2, kolGuid, moeinGuid, tafsilGuid, isFromGardesh: false, token));
				}
				dr.Close();
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 994);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return ret;
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
			if (dr["Date"] != DBNull.Value)
			{
				item.DateM = (DateTime)dr["Date"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["GUID"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["GUID"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				item.UserGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["Type"] != DBNull.Value)
			{
				item.SanadType = (SanadType)(short)dr["Type"];
			}
			item.ChangeStatus = EntityChangeSt.Edited;
			if (isLoadDets)
			{
				SanadDetailV5PersistenceRepository manualArticles = _manualArticles;
				List<WebSanad_Det> source = await manualArticles.GetAllSanadDetailsAsync(await item.GetSanadNoAsync(token, tranName), tranName);
				SanadBusiness sanadBusiness = item;
				await sanadBusiness.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(ContextGuid, source, new List<Sanad_DetBusiness>(), tranName), EnSystem.None);
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1028);
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
				item.Qt = (decimal)(double)dr["QT"];
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
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataVSanadDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1048);
			throw;
		}
		return item;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(int id, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1056);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ID", (object)id)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("SP_Sanad_DeleteWithID", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1060);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1064);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1065);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> ChangeSanadStatusAsync(string sansdNos, SanadSt st, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1072);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@SanadNOs", (object)sansdNos),
				new SqlParameter("@ST", (object)st)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("SP_Change_SanadST", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"), ReturnedState.None, "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1080);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1084);
			res.AddReturnedValue(ex, "", "ChangeSanadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1085);
		}
		return res;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				DatabaseContext?.Dispose();
				DatabaseContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1100);
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(SanadBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string caller, SanadBusiness item, EnSystem system, int oldSanadNo, CancellationToken token, string tranName, IEnumerable<ViewSanadDetailCoverBusiness> lst, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		int line = 0;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1114);
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
				throw new Exception("sanad rows contain invalid value(s) for Astype or AsGuid");
			}
			line = 2;
			SanadBusiness old = await GetAsync(oldSanadNo, token, tranName);
			line = 3;
			SanadBusiness sanadBusiness = await GetAsync(await item.GetSanadNoAsync(token, tranName), token, tranName);
			line = 4;
			if (sanadBusiness != null && item.ListSanad_Det.Any((Sanad_DetBusiness q) => !q.CheckIsContain(autoSanadItemsForSave)) && sanadBusiness.DateM.Date != item.DateM.Date)
			{
				if (system != EnSystem.Transfer)
				{
					throw new Exception("تاریخ برگه سند تجمیع شده، قابل ویرایش نیست\r\n" + $"ExistsDanadDate:{sanadBusiness.DateM.Date}\r\n" + $"Date:{item.DateM.Date}\r\n" + $"OldSanadNo:{oldSanadNo}\r\n" + $"syste:{system}\r\n" + "item:" + Json.ToStringJson(item, indented: true));
				}
				item.DateM = sanadBusiness.DateM;
			}
			line = 5;
			if (system != EnSystem.Transfer && (old?.SanadSt ?? SanadSt.None) == SanadSt.Daemi && system != EnSystem.Transfer)
			{
				throw new Exception("سند دائمی قابل ویرایش نیست.");
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
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _manualArticles.RemoveRangeAsync(autoSanadItemsForSave.Select((Tuple<Document, Guid> p) => p.Item2).Distinct().Single(), Document.Sanad, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1161);
				if (res.HasError)
				{
					return res;
				}
			}
			line = 8;
			IEnumerable<Task<ReturnedSaveFuncInfo>> tasks = autoSanadItemsForSave.Select((Tuple<Document, Guid> p) => _autoArticles.RemoveAutoArticlesAsync(p.Item1, p.Item2, tranName, token));
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1166);
			if (autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 == Document.Sanad))
			{
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
					new SqlParameter("@CashierGuid", (object)item.UserGuid)
				};
				line = 9;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Sanad_Save", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs"), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1183);
				line = 11;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _manualArticles.SaveRangeAsync(item.ListSanad_Det, item, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1185);
				line = 12;
				if (res.HasError)
				{
					return res;
				}
				line = 13;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _autoArticles.SaveRangeAsync(item, 0, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1191);
			line = 14;
			if (autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p.Item1 == Document.Sanad))
			{
				Guid guid = item.Guid;
				int sanadNo = await item.GetSanadNoAsync(token, tranName);
				int oldSanadNo2 = ((old != null) ? (await old.GetSanadNoAsync(token, tranName)) : 0);
				await UpdateVSanadDetailAsync(1, guid, sanadNo, oldSanadNo2, old?.DateSh, item.UserGuid, 10, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "SanadBusiness item, int oldSanadNo, string tranName, params Tuple<Document, Guid>[] autoSanadItemsForSave\r\n" + $"#Line_{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1198);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1200);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<SanadBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<SanadBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Please Use Another Methode With AsType , AsGuid");
	}

	public async Task<SanadBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		SanadBusiness res = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1227);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Sanad_SelectRow", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = await LoadDataAsync(dr, isLoadDets: true, token, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1238);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<SanadBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public Task<IEnumerable<SanadBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<int> CountOfAutoSanadWithDiffrentDateAsync(int sanadNo, DateTime dateM, Guid asGuid1, short asType1, Guid asGuid2, short asType2, CancellationToken token, string tranName)
	{
		int res = 0;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1256);
		int result;
		try
		{
			_ = 1;
			try
			{
				if (dateM < new DateTime(1900, 1, 1))
				{
					result = res;
					goto IL_032f;
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
				object obj = await c.ExecuteScalarAsync<object>("sp_Sanad_CountOFAutoSanadWithDiffrentDate", parameters, tranName, token, 30, "CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (obj == null)
				{
					result = res;
					goto IL_032f;
				}
				res = obj.ToString().ParseToInt();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1275);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CountOfAutoSanadWithDiffrentDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
		IL_032f:
		return result;
	}

	public async Task<bool> CheckGhestAsync(Guid sanadGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1287);
		bool res = false;
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadGuid", (object)sanadGuid)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Sanad_CheckGhest", parameters, tranName, token, 30, "CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (obj == null)
				{
					return false;
				}
				res = obj.ToString().ParseToBoolean();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1298);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CheckGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public SanadBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<decimal> GetBeforeAccountAsync(Guid tafsilGuid, Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		decimal res = default(decimal);
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1373);
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
				object obj = await c.ExecuteScalarAsync<object>("sp_GetBeforeAccount", parameters, tranName, token, 30, "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (obj != null)
				{
					res = obj.ToString().ParseToDecimal();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1388);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<Guid> GetGuidBySanadNoAsync(int sanadNo, CancellationToken token, string tranName)
	{
		Guid res = Guid.Empty;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1402);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadNo", (object)sanadNo)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Sanad_GetGuidBySanadNo", parameters, tranName, token, 30, "GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()) && Guid.TryParse(obj.ToString(), out var _))
				{
					res = Guid.Parse(obj.ToString());
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs", 1412);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<DateTime?> GetSanadBargashtDateAsync(decimal price, Guid debitTafsilGuid, Guid creditTafsilGuid, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Sanad_DetBusiness>> GetSanadTemporaryAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Sanad_DetBusiness>> GetSanadBardashtAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<SanadDetRoznameKolBusiness>> GetAllSanadDetRoznameKolAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<SanadGhestViewModel>> GetAllSanadGhestAsync(Guid sanadGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveDuplicateSanadNoAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> CheckMoeinValidationAsync(List<Guid> moeinGuidList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
