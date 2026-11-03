using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Global;

internal class TempBarcodePrintPersistenceRepository : PersistenceRepository<TempBarcodePrintBusiness, TempBarcodePrint>, ITempBarcodePrintRepository, IRepository<TempBarcodePrintBusiness>, INoGuidRepository<TempBarcodePrintBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TempBarcodePrintBusiness, TempBarcodePrint>, IItemAsyncMapper<TempBarcodePrint, TempBarcodePrintBusiness>
	{
		public Task<TempBarcodePrint> MapAsync(string contextGuid, TempBarcodePrintBusiness source, TempBarcodePrint destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TempBarcodePrint();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.ProductGuid = source.ProductGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.DetailGuid = source.DetailGuid;
				destination.Barcode = source.Barcode ?? "";
				destination.GuarantyCode = source.GuarantyCode ?? "";
				destination.AsType = source.AsType;
				destination.MasterDescription = source.MasterDescription ?? "";
				destination.DetailDescription = source.DetailDescription ?? "";
				destination.MasterDate = source.MasterDate;
				destination.GuarantyStartDate = source.GuarantyStartDate;
				destination.GuarantyEndDate = source.GuarantyEndDate;
				destination.MasterGuid = source.MasterGuid;
				destination.TempDescription = source.TempDescription;
				return Task.FromResult(destination);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 113);
				throw;
			}
		}

		public Task<TempBarcodePrintBusiness> MapAsync(string contextGuid, TempBarcodePrint source, TempBarcodePrintBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TempBarcodePrintBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.ProductGuid = source.ProductGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.DetailGuid = source.DetailGuid;
				destination.Barcode = source.Barcode;
				destination.GuarantyCode = source.GuarantyCode;
				destination.AsType = source.AsType;
				destination.MasterDescription = source.MasterDescription;
				destination.DetailDescription = source.DetailDescription;
				destination.MasterDate = source.MasterDate;
				destination.GuarantyStartDate = source.GuarantyStartDate;
				destination.GuarantyEndDate = source.GuarantyEndDate;
				destination.MasterGuid = source.MasterGuid;
				destination.TempDescription = source.TempDescription;
				return Task.FromResult(destination);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 150);
				throw;
			}
		}

		public Task<List<TempBarcodePrint>> MapListAsync(string contextGuid, IEnumerable<TempBarcodePrintBusiness> source, List<TempBarcodePrint> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TempBarcodePrintBusiness>> MapListAsync(string contextGuid, IEnumerable<TempBarcodePrint> source, List<TempBarcodePrintBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<TempBarcodePrint, TempBarcodePrintBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<TempBarcodePrint, TempBarcodePrintBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TempBarcodePrintBusiness, TempBarcodePrint> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public TempBarcodePrintPersistenceRepository(ContextHolder db)
		: base(db)
	{
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 39);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(TempBarcodePrintBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TempBarcodePrintBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 49));
				await ((DbContext)c).AddRangeAsync((IEnumerable<object>)(await MapToDatabase.MapListAsync(ContextGuid, items, null, tranName, token)), token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 55);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 56);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 67));
				List<TempBarcodePrint> list = ((IQueryable<TempBarcodePrint>)acc_ModelContext.TempBarcodePrint).Where((TempBarcodePrint q) => q.MasterGuid == masterGuid).ToList();
				if (list != null && list.Any())
				{
					((DbContext)acc_ModelContext).RemoveRange((IEnumerable<object>)list);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 73);
				res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\TempBarcodePrintPersistenceRepository.cs", 74);
			}
			return res;
		}
	}
}
