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

internal class Scale_SellDetailPersistenceRepository : PersistenceRepository<Scale_SellDetailBusiness, Scale_SellDetail>, IScale_SellDetailRepository, IRepository<Scale_SellDetailBusiness>, INoGuidRepository<Scale_SellDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_SellDetail, Scale_SellDetailBusiness>, IItemAsyncMapper<Scale_SellDetailBusiness, Scale_SellDetail>
	{
		public Task<Scale_SellDetailBusiness> MapAsync(string contextGuid, Scale_SellDetail source, Scale_SellDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_SellDetailBusiness(contextGuid);
				}
				destination.PortNumber = source.PortNumber;
				destination.ScaleGuid = source.ScaleGuid;
				destination.FactorNumber = source.FactorNumber;
				destination.MemoryNumber = source.MemoryNumber;
				destination.Quantity = source.Quantity;
				destination.UnitPrice = source.UnitPrice;
				destination.Type_Weight = source.Type_Weight;
				destination.DateM = source.DateM;
				destination.DateSH_H = source.DateSH_H;
				destination.DateUpdate = source.DateUpdate;
				destination.ComputerName = source.ComputerName;
				destination.IsRead = source.IsRead;
				destination.Ghorfe = source.Ghorfe;
				destination.RowSum = source.RowSum;
				destination.RInt1 = source.RInt1;
				destination.RInt2 = source.RInt2;
				destination.RText1 = source.RText1;
				destination.RText2 = source.RText2;
				destination.RDbl1 = source.RDbl1;
				destination.RDbl2 = source.RDbl2;
				destination.IP = source.IP;
				destination.SC_Guid = source.SC_Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SellDetailPersistenceRepository.cs", 84);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_SellDetail> MapAsync(string contextGuid, Scale_SellDetailBusiness source, Scale_SellDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_SellDetail();
				}
				destination.PortNumber = source.PortNumber;
				destination.ScaleGuid = source.ScaleGuid;
				destination.FactorNumber = source.FactorNumber;
				destination.MemoryNumber = source.MemoryNumber;
				destination.Quantity = source.Quantity;
				destination.UnitPrice = source.UnitPrice;
				destination.Type_Weight = source.Type_Weight;
				destination.DateM = source.DateM;
				destination.DateSH_H = source.DateSH_H;
				destination.DateUpdate = source.DateUpdate;
				destination.ComputerName = source.ComputerName;
				destination.IsRead = source.IsRead;
				destination.Ghorfe = source.Ghorfe;
				destination.RowSum = source.RowSum;
				destination.RInt1 = source.RInt1;
				destination.RInt2 = source.RInt2;
				destination.RText1 = source.RText1;
				destination.RText2 = source.RText2;
				destination.RDbl1 = source.RDbl1;
				destination.RDbl2 = source.RDbl2;
				destination.IP = source.IP;
				destination.SC_Guid = source.SC_Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SellDetailPersistenceRepository.cs", 128);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_SellDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_SellDetail> source, List<Scale_SellDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_SellDetail>> MapListAsync(string contextGuid, IEnumerable<Scale_SellDetailBusiness> source, List<Scale_SellDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_SellDetail, Scale_SellDetailBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_SellDetail, Scale_SellDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_SellDetailBusiness, Scale_SellDetail> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_SellDetailPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SellDetailPersistenceRepository.cs", 39);
		}
	}
}
