using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business.OrderDepot;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class AbadDetPersistanceRepository : PersistenceMasterRepository<AbadDetBusiness, AbadDet>, IAbadDetRepository, IPersistenceMasterRepository<AbadDetBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AbadDetBusiness, AbadDet>, IItemAsyncMapper<AbadDet, AbadDetBusiness>
	{
		public Task<AbadDet> MapAsync(string contextGuid, AbadDetBusiness source, AbadDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs"))
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
						destination = new AbadDet();
					}
					destination.X = source.X;
					destination.Y = source.Y;
					destination.Z = source.Z;
					destination.Count = source.Count;
					destination.ShapeType = source.ShapeType;
					destination.ShapeInformation = source.ShapeInformation;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs", 88);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<AbadDetBusiness> MapAsync(string contextGuid, AbadDet source, AbadDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs"))
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
						destination = new AbadDetBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.ContextGuid = contextGuid;
					destination.X = source.X;
					destination.Y = source.Y;
					destination.Z = source.Z;
					destination.Count = source.Count;
					destination.ShapeType = source.ShapeType;
					destination.ShapeInformation = source.ShapeInformation;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs", 121);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<AbadDet>> MapListAsync(string contextGuid, IEnumerable<AbadDetBusiness> source, List<AbadDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AbadDetBusiness>> MapListAsync(string contextGuid, IEnumerable<AbadDet> source, List<AbadDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<AbadDet, AbadDetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AbadDetBusiness, AbadDet> MapToDatabase => Mapper;

	public AbadDetPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs", 35);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<AbadDetBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!(items?.Any() ?? false))
				{
					return ret;
				}
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs", 45);
				foreach (AbadDetBusiness item in items)
				{
					AbadDet abadDet = await MapToDatabase.MapAsync(base.ContextGuid, item, new AbadDet(), tranName, token);
					abadDet.MasterGuid = masterGuid;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(abadDet, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs", 50);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs", 55);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\AbadDetPersistanceRepository.cs", 56);
			}
			return ret;
		}
	}
}
