using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData.Hotel;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class ReceiptDetailPersistenceRepository : PersistenceMasterRepository<ReceiptDetailBusiness, ReceiptDetail>, IReceiptDetailRepository, IPersistenceMasterRepository<ReceiptDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReceiptDetailBusiness, ReceiptDetail>, IItemAsyncMapper<ReceiptDetail, ReceiptDetailBusiness>
	{
		public Task<ReceiptDetail> MapAsync(string contextGuid, ReceiptDetailBusiness source, ReceiptDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceiptDetail();
				}
				destination.Guid = source.Guid;
				destination.PlacementGuid = source.RoomGuid;
				destination.MasterGuid = source.MasterGuid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.StartDate = source.StartDate;
				destination.EndDate = source.EndDate;
				destination.TotalPrice = source.TotalPrice;
				destination.Tax = source.Tax;
				destination.TaxPercent = source.TaxPercent;
				destination.Avarez = source.Avarez;
				destination.AvarezPercent = source.AvarezPercent;
				destination.ExtraCount = source.ExtraCount;
				destination.ExtraPrice = source.ExtraPrice;
				destination.Discount = source.Discount;
				destination.DiscountPercent = source.DiscountPercent;
				destination.PricePerNight = source.PricePerNight;
				destination.Penalty = source.Penalty;
				destination.MetaData = Json.ToStringJson(source.MetaData);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 151);
				throw;
			}
			return Task.FromResult(destination);
		}

		public async Task<ReceiptDetailBusiness> MapAsync(string contextGuid, ReceiptDetail source, ReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			if (destination == null)
			{
				throw new NullReferenceException("Please New kon destination ");
			}
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				await destination.SetRoomGuidAsync(EnSystem.Mapper, source.PlacementGuid, token, tranName);
				destination.MasterGuid = source.MasterGuid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.StartDate = source.StartDate;
				destination.EndDate = source.EndDate;
				destination.Tax = source.Tax;
				destination.TaxPercent = source.TaxPercent;
				destination.Avarez = source.Avarez;
				destination.AvarezPercent = source.AvarezPercent;
				destination.ExtraCount = source.ExtraCount;
				destination.ExtraPrice = source.ExtraPrice;
				destination.Discount = source.Discount;
				destination.DiscountPercent = source.DiscountPercent;
				destination.PricePerNight = source.PricePerNight;
				destination.Penalty = source.Penalty;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<ReceiptDetailMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 185);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 191);
				throw;
			}
			return destination;
		}

		public Task<List<ReceiptDetail>> MapListAsync(string contextGuid, IEnumerable<ReceiptDetailBusiness> source, List<ReceiptDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceiptDetail> source, List<ReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ReceiptDetail, ReceiptDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReceiptDetailBusiness, ReceiptDetail> MapToDatabase => Mapper;

	public ReceiptDetailPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 40);
		}
	}

	public override Task<List<ReceiptDetailBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad");
	}

	public async Task GetAllAsync(ReceiptBusiness master, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 47));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					IQueryable<ReceiptDetail> queryable = ((IQueryable<ReceiptDetail>)c.ReciptDetals).Where((ReceiptDetail q) => q.MasterGuid == master.Guid);
					if (queryable == null)
					{
						return;
					}
					foreach (ReceiptDetail item in queryable)
					{
						ReceiptDetailBusiness destination = await ReceiptDetailBusiness.ReciptDetalFactoryAsync(base.ContextGuid, master, token, tranName);
						ReceiptDetailBusiness receiptDetail = await MapFromDatabase.MapAsync(base.ContextGuid, item, destination, tranName, token);
						await master.AddReciptDetalAsync(receiptDetail);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 60);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs");
				}
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceiptDetailBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (items?.Any((ReceiptDetailBusiness p) => p.Guid == Guid.Empty) ?? false)
			{
				throw new ArgumentException("one or more items in list contains empty value for field Guid");
			}
			try
			{
				if (items?.Any() ?? false)
				{
					List<ReceiptDetail> des = await MapToDatabase.MapListAsync(base.ContextGuid, items, null, tranName, token);
					BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 78);
					foreach (ReceiptDetail item in des)
					{
						item.MasterGuid = masterGuid;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 82);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 89);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 90);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 101));
				List<ReceiptDetail> list = ((IQueryable<ReceiptDetail>)acc_ModelContext.ReciptDetals).Where((ReceiptDetail x) => x.MasterGuid == masterGuid).ToList();
				if (list != null && list.Any())
				{
					acc_ModelContext.ReciptDetals.RemoveRange((IEnumerable<ReceiptDetail>)list);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 109);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptDetailPersistenceRepository.cs", 110);
			}
			return ret;
		}
	}
}
