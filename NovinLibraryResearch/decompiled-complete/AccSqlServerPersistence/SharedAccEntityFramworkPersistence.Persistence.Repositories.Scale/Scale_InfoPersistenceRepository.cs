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

internal class Scale_InfoPersistenceRepository : PersistenceRepository<Scale_InfoBusiness, Scale_Info>, IScale_InfoRepository, IRepository<Scale_InfoBusiness>, INoGuidRepository<Scale_InfoBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Info, Scale_InfoBusiness>, IItemAsyncMapper<Scale_InfoBusiness, Scale_Info>
	{
		public Task<Scale_InfoBusiness> MapAsync(string contextGuid, Scale_Info source, Scale_InfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_InfoBusiness(contextGuid);
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.PortNumber = source.PortNumber;
				destination.TimerInterval = source.TimerInterval;
				destination.Name = source.Name;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Deleted = source.Deleted;
				destination.IP = source.IP;
				destination.EndMessage = source.EndMessage;
				destination.ShopName = source.ShopName;
				destination.AccountingMemory = source.AccountingMemory;
				destination.AccountingDivision = source.AccountingDivision;
				destination.AccountingIP = source.AccountingIP;
				destination.MNGLock = source.MNGLock;
				destination.FactorNumber = source.FactorNumber;
				destination.ConnectionType = source.ConnectionType;
				destination.Model = source.IP;
				destination.Serial = source.Serial;
				destination.Description = source.Description;
				destination.AccountingDate = source.AccountingDate;
				destination.Guid = source.Guid;
				destination.AccountingID = source.AccountingID;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_InfoPersistenceRepository.cs", 86);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Info> MapAsync(string contextGuid, Scale_InfoBusiness source, Scale_Info destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Info();
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.PortNumber = source.PortNumber;
				destination.TimerInterval = source.TimerInterval;
				destination.Name = source.Name;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Deleted = source.Deleted;
				destination.IP = source.IP;
				destination.EndMessage = source.EndMessage;
				destination.ShopName = source.ShopName;
				destination.AccountingMemory = source.AccountingMemory;
				destination.AccountingDivision = source.AccountingDivision;
				destination.AccountingIP = source.AccountingIP;
				destination.MNGLock = source.MNGLock;
				destination.FactorNumber = source.FactorNumber;
				destination.ConnectionType = source.ConnectionType;
				destination.Model = source.IP;
				destination.Serial = source.Serial;
				destination.Description = source.Description;
				destination.AccountingDate = source.AccountingDate;
				destination.Guid = source.Guid;
				destination.AccountingID = source.AccountingID;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_InfoPersistenceRepository.cs", 128);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_InfoBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Info> source, List<Scale_InfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Info>> MapListAsync(string contextGuid, IEnumerable<Scale_InfoBusiness> source, List<Scale_Info> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Info, Scale_InfoBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Info, Scale_InfoBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_InfoBusiness, Scale_Info> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_InfoPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex2, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_InfoPersistenceRepository.cs", 42);
		}
	}
}
