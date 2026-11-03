using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.TTMS;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.TTMS;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.TTMS;

internal class TTMSDetailPersistenceRepository : PersistenceMasterRepository<TTMSDetBusiness, TTMSDetail>, ITTMSDetailBusiness, IPersistenceMasterRepository<TTMSDetBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TTMSDetBusiness, TTMSDetail>, IItemAsyncMapper<TTMSDetail, TTMSDetBusiness>
	{
		public Task<TTMSDetail> MapAsync(string contextGuid, TTMSDetBusiness source, TTMSDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TTMSDetail();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSDetailPersistenceRepository.cs", 59);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<TTMSDetBusiness> MapAsync(string contextGuid, TTMSDetail source, TTMSDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TTMSDetBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSDetailPersistenceRepository.cs", 86);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<TTMSDetail>> MapListAsync(string contextGuid, IEnumerable<TTMSDetBusiness> source, List<TTMSDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TTMSDetBusiness>> MapListAsync(string contextGuid, IEnumerable<TTMSDetail> source, List<TTMSDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	public override IItemAsyncMapper<TTMSDetail, TTMSDetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TTMSDetBusiness, TTMSDetail> MapToDatabase => Mapper;

	private mapper Mapper { get; set; } = new mapper();


	public TTMSDetailPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TTMS\\TTMSDetailPersistenceRepository.cs", 32);
		}
	}
}
