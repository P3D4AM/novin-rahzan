using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Gift;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Gift;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Gift;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Gift;

internal class GiftPersistanceRepository : PersistenceSyncRepository<GiftBusiness, SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift>, IGiftRepository, ISyncRepository<GiftBusiness>, IEditableDelivery, IRepository<GiftBusiness>, INoGuidRepository<GiftBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift, GiftBusiness>, IItemAsyncMapper<GiftBusiness, SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift>
	{
		public async Task<GiftBusiness> MapAsync(string contextGuid, SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift source, GiftBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new GiftBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Discription = source.Discription;
				destination.Type = source.Type;
				if (!string.IsNullOrEmpty(source.DetailList))
				{
					List<GiftDetailBusiness> list = source.DetailList.FromJson<List<GiftDetailBusiness>>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Gift\\GiftPersistanceRepository.cs", 64);
					await destination.AddRangeToDetailListAsync(list, token);
				}
				if (!string.IsNullOrEmpty(source?.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<GiftMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Gift\\GiftPersistanceRepository.cs", 68);
				}
				return destination;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Gift\\GiftPersistanceRepository.cs", 74);
				throw;
			}
		}

		public Task<SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift> MapAsync(string contextGuid, GiftBusiness source, SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift();
				}
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Discription = source.Discription;
				destination.Type = source.Type;
				destination.DetailList = Json.ToStringJson(source.DetailList);
				destination.MetaData = Json.ToStringJson(source.MetaData);
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Gift\\GiftPersistanceRepository.cs", 100);
				throw;
			}
		}

		public Task<List<GiftBusiness>> MapListAsync(string contextGuid, IEnumerable<SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift> source, List<GiftBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift>> MapListAsync(string contextGuid, IEnumerable<GiftBusiness> source, List<SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift, GiftBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift, GiftBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<GiftBusiness, SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public GiftPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Gift\\GiftPersistanceRepository.cs", 38);
		}
	}
}
