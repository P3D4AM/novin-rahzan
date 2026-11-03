using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.TTMS;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.TTMS;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.TTMS;

internal class TTMSPersistenceRepository : PersistenceRepository<TTMSBusiness, SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS>, ITTMSRepository, IRepository<TTMSBusiness>, INoGuidRepository<TTMSBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TTMSBusiness, SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS>, IItemAsyncMapper<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS, TTMSBusiness>
	{
		public Task<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS> MapAsync(string contextGuid, TTMSBusiness source, SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.DateM = source.DateM;
				destination.FilePath = source.FilePath;
				destination.Priod = source.Priod;
				destination.Year = source.Year;
				destination.State = source.State;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 227);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<TTMSBusiness> MapAsync(string contextGuid, SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS source, TTMSBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TTMSBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.DateM = source.DateM;
				destination.FilePath = source.FilePath;
				destination.Priod = source.Priod;
				destination.Year = source.Year;
				destination.State = source.State;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 256);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS>> MapListAsync(string contextGuid, IEnumerable<TTMSBusiness> source, List<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TTMSBusiness>> MapListAsync(string contextGuid, IEnumerable<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS> source, List<TTMSBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private TTMSDetailPersistenceRepository _detRepo;

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS, TTMSBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS, TTMSBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TTMSBusiness, SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public TTMSPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new TTMSDetailPersistenceRepository(db);
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 42));
				SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS tTMS = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS>((IQueryable<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS>)c.TTMS, (Expression<Func<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS, bool>>)((SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS w) => w.Guid == item), token);
				if (tTMS == null)
				{
					return ret;
				}
				c.TTMS.Remove(tTMS);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await _detRepo.RemoveAsync(item, tryCount, token, tranName);
				if (returnedSaveFuncInfo.HasError)
				{
					return returnedSaveFuncInfo;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 51);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 52);
			}
			return ret;
		}
	}

	public override async Task<TTMSBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 58));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs"))
		{
			TTMSBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS tTMS = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS>((IQueryable<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS>)c.TTMS, (Expression<Func<SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS, bool>>)((SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS x) => x.Guid == guid), token);
					if (tTMS == null)
					{
						result = null;
					}
					else
					{
						TTMSBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, tTMS, null, tranName, token);
						await ret.AddRangeToListSanadAsync(await _detRepo.GetAllAsync(ret.Guid, token, tranName));
						ret.ChangeStatus = EntityChangeSt.Edited;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 75);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TTMSBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 92));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS tTMS = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(tTMS, tTMS.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 95);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 96);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetailList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 97);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 102);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 103);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TTMSBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (TTMSBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 117);
					if (ret.HasError)
					{
						return ret;
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 125);
				return new ReturnedSaveFuncInfo(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 126);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<TTMSReportBusiness>> GetAllReportAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 133);
		IEnumerable<TTMSReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<TTMSReportBusiness> list = new List<TTMSReportBusiness>();
				DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_TTMS_GetAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr, token));
				}
				dr.Close();
				result = list?.Where((TTMSReportBusiness q) => q.Guid != Guid.Empty);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 148);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs");
			}
		}
		return result;
	}

	private TTMSReportBusiness LoadData(DbDataReader dr, CancellationToken token)
	{
		try
		{
			TTMSReportBusiness tTMSReportBusiness = new TTMSReportBusiness(ContextGuid);
			if (dr["Guid"] != DBNull.Value)
			{
				tTMSReportBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				tTMSReportBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (dr["FilePath"] != DBNull.Value)
			{
				tTMSReportBusiness.FilePath = dr["FilePath"].ToString();
			}
			if (dr["Priod"] != DBNull.Value)
			{
				tTMSReportBusiness.Priod = (EnSeason)dr["Priod"];
			}
			if (dr["Year"] != DBNull.Value)
			{
				tTMSReportBusiness.Year = (short)dr["Year"];
			}
			if (dr["State"] != DBNull.Value)
			{
				tTMSReportBusiness.State = (short)dr["State"];
			}
			if (dr["SellCount"] != DBNull.Value)
			{
				tTMSReportBusiness.SellCount = (int)dr["SellCount"];
			}
			if (dr["SellSum"] != DBNull.Value)
			{
				tTMSReportBusiness.SellSum = (decimal)dr["SellSum"];
			}
			if (dr["BackSellCount"] != DBNull.Value)
			{
				tTMSReportBusiness.BackSellCount = (int)dr["BackSellCount"];
			}
			if (dr["BackSellSum"] != DBNull.Value)
			{
				tTMSReportBusiness.BackSellSum = (decimal)dr["BackSellSum"];
			}
			if (dr["BuyCount"] != DBNull.Value)
			{
				tTMSReportBusiness.BuyCount = (int)dr["BuyCount"];
			}
			if (dr["BuySum"] != DBNull.Value)
			{
				tTMSReportBusiness.BuySum = (decimal)dr["BuySum"];
			}
			if (dr["BackBuyCount"] != DBNull.Value)
			{
				tTMSReportBusiness.BackBuyCount = (int)dr["BackBuyCount"];
			}
			if (dr["BackBuySum"] != DBNull.Value)
			{
				tTMSReportBusiness.BackBuySum = (decimal)dr["BackBuySum"];
			}
			return tTMSReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 182);
			throw;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				_detRepo?.Dispose();
				_detRepo = null;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSPersistenceRepository.cs", 198);
		}
	}
}
