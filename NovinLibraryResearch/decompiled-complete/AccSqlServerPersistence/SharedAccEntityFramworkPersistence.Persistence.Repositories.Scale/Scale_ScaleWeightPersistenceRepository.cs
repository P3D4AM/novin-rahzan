using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Scale;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Scale;

internal class Scale_ScaleWeightPersistenceRepository : PersistenceRepository<Scale_ScaleWeightBusiness, Scale_ScaleWeight>, IScale_ScaleWeightRepository, IRepository<Scale_ScaleWeightBusiness>, INoGuidRepository<Scale_ScaleWeightBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_ScaleWeight, Scale_ScaleWeightBusiness>, IItemAsyncMapper<Scale_ScaleWeightBusiness, Scale_ScaleWeight>
	{
		public Task<Scale_ScaleWeightBusiness> MapAsync(string contextGuid, Scale_ScaleWeight source, Scale_ScaleWeightBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_ScaleWeightBusiness(contextGuid);
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.Date = source.Date;
				destination.UnitPrice = source.UnitPrice;
				destination.UnitPriceText = source.UnitPriceText;
				destination.Weight = source.Weight;
				destination.WeightText = source.WeightText;
				destination.Totalprice = source.Totalprice;
				destination.TotalPriceText = source.TotalPriceText;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ScaleWeightPersistenceRepository.cs", 71);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_ScaleWeight> MapAsync(string contextGuid, Scale_ScaleWeightBusiness source, Scale_ScaleWeight destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_ScaleWeight();
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.Date = source.Date;
				destination.UnitPrice = source.UnitPrice;
				destination.UnitPriceText = source.UnitPriceText;
				destination.Weight = source.Weight;
				destination.WeightText = source.WeightText;
				destination.Totalprice = source.Totalprice;
				destination.TotalPriceText = source.TotalPriceText;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ScaleWeightPersistenceRepository.cs", 102);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_ScaleWeightBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_ScaleWeight> source, List<Scale_ScaleWeightBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_ScaleWeight>> MapListAsync(string contextGuid, IEnumerable<Scale_ScaleWeightBusiness> source, List<Scale_ScaleWeight> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_ScaleWeight, Scale_ScaleWeightBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_ScaleWeight, Scale_ScaleWeightBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_ScaleWeightBusiness, Scale_ScaleWeight> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_ScaleWeightPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ScaleWeightPersistenceRepository.cs", 39);
		}
	}
}
