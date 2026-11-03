using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.TozieLists;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.TozieList;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.TozieList;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.TozieLists;

internal class TozieListPersistenceRepository : PersistenceRepository<TozieListBusiness, TozieList>, ITozieListRepository, IRepository<TozieListBusiness>, INoGuidRepository<TozieListBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TozieListBusiness, TozieList>, IItemAsyncMapper<TozieList, TozieListBusiness>
	{
		public Task<TozieList> MapAsync(string contextGuid, TozieListBusiness source, TozieList destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TozieList();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Number = source.Number;
				destination.DateM = source.DateM;
				destination.DriverName = source.DriverName;
				destination.DriverMobile1 = source.DriverMobile1;
				destination.DriverMobile2 = source.DriverMobile2;
				destination.CarNumber = source.CarNumber;
				destination.Description = source.Description;
				destination.CashierGuid = source.CashierGuid;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 225);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<TozieListBusiness> MapAsync(string contextGuid, TozieList source, TozieListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TozieListBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Number = source.Number;
				destination.DateM = source.DateM;
				destination.DriverName = source.DriverName;
				destination.DriverMobile1 = source.DriverMobile1;
				destination.DriverMobile2 = source.DriverMobile2;
				destination.CarNumber = source.CarNumber;
				destination.Description = source.Description;
				destination.CashierGuid = source.CashierGuid;
				destination.Status = source.Status;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 257);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<TozieList>> MapListAsync(string contextGuid, IEnumerable<TozieListBusiness> source, List<TozieList> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TozieListBusiness>> MapListAsync(string contextGuid, IEnumerable<TozieList> source, List<TozieListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private TozieListDetailsPersistenceRepository _detRepo;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TozieList, TozieListBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TozieListBusiness, TozieList> MapToDatabase => Mapper;

	public override IItemMapper<TozieList, TozieListBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public TozieListPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new TozieListDetailsPersistenceRepository(db);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 42);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs"))
		{
			_ = 2;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 52));
				TozieList tozieList = await EntityFrameworkQueryableExtensions.SingleAsync<TozieList>((IQueryable<TozieList>)dbcontext.TozieList, (Expression<Func<TozieList, bool>>)((TozieList p) => p.Guid == guid), token);
				if (tozieList == null)
				{
					return res;
				}
				dbcontext.TozieList.Remove(tozieList);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 56);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 60);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 61);
			}
			return res;
		}
	}

	public async Task<IEnumerable<TozieListRepostBusiness>> GetAllReportAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 67);
		IEnumerable<TozieListRepostBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<TozieListRepostBusiness> list = new List<TozieListRepostBusiness>();
				DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_TozieList_GetAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr, token));
				}
				dr.Close();
				result = list?.Where((TozieListRepostBusiness q) => q.Guid != Guid.Empty);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 82);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs");
			}
		}
		return result;
	}

	private TozieListRepostBusiness LoadData(DbDataReader dr, CancellationToken token)
	{
		try
		{
			TozieListRepostBusiness tozieListRepostBusiness = new TozieListRepostBusiness(ContextGuid);
			if (dr["Guid"] != DBNull.Value)
			{
				tozieListRepostBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				tozieListRepostBusiness.Number = (int)dr["Number"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				tozieListRepostBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (dr["DriverName"] != DBNull.Value)
			{
				tozieListRepostBusiness.DriverName = dr["DriverName"].ToString();
			}
			if (dr["DriverMobile1"] != DBNull.Value)
			{
				tozieListRepostBusiness.DriverMobile1 = dr["DriverMobile1"].ToString();
			}
			if (dr["DriverMobile2"] != DBNull.Value)
			{
				tozieListRepostBusiness.DriverMobile2 = dr["DriverMobile2"].ToString();
			}
			if (dr["CarNumber"] != DBNull.Value)
			{
				tozieListRepostBusiness.CarNumber = dr["CarNumber"].ToString();
			}
			if (dr["Description"] != DBNull.Value)
			{
				tozieListRepostBusiness.Description = dr["Description"].ToString();
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				tozieListRepostBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["CashierName"] != DBNull.Value)
			{
				tozieListRepostBusiness.CashierName = dr["CashierName"].ToString();
			}
			if (dr["SellCount"] != DBNull.Value)
			{
				tozieListRepostBusiness.SellCount = (int)dr["SellCount"];
			}
			if (dr["SellSum"] != DBNull.Value)
			{
				tozieListRepostBusiness.SellSum = (decimal)dr["SellSum"];
			}
			if (dr["BackSellCount"] != DBNull.Value)
			{
				tozieListRepostBusiness.BackSellCount = (int)dr["BackSellCount"];
			}
			if (dr["BackSellSum"] != DBNull.Value)
			{
				tozieListRepostBusiness.BackSellSum = (decimal)dr["BackSellSum"];
			}
			if (dr["ReceptionCount"] != DBNull.Value)
			{
				tozieListRepostBusiness.ReceptionCount = (int)dr["ReceptionCount"];
			}
			if (dr["ReceptionSum"] != DBNull.Value)
			{
				tozieListRepostBusiness.ReceptionSum = (decimal)dr["ReceptionSum"];
			}
			return tozieListRepostBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 118);
			throw;
		}
	}

	public async Task<int> GetNextNumberAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 124));
		using (new JaegerMonitor("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<TozieList>(((IQueryable<TozieList>)c.TozieList).OrderByDescending((TozieList x) => x.Number).Take(1), token))?.Number ?? 0) + 1;
				}
				catch (ThreadAbortException)
				{
					throw;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 136);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TozieListBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 153));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				TozieList tozieList = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(tozieList, tozieList.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 156);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 157);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetailList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 158);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 163);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 164);
			}
			return ret;
		}
	}

	public override async Task<TozieListBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 170));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs"))
		{
			TozieListBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					TozieList tozieList = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TozieList>((IQueryable<TozieList>)c.TozieList, (Expression<Func<TozieList, bool>>)((TozieList x) => x.Guid == guid), token);
					if (tozieList == null)
					{
						result = null;
					}
					else
					{
						TozieListBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, tozieList, null, tranName, token);
						await ret.AddRangeToListSanadAsync(await _detRepo.GetAllAsync(ret.Guid, token, tranName));
						result = ret;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs", 186);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
