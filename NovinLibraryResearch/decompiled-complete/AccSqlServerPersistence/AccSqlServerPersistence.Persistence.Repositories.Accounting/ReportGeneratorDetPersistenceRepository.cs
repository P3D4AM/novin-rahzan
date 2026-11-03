using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ReportGeneratorDetPersistenceRepository : PersistenceMasterRepository<ReportGeneratorDetBusiness, ReportGeneratorDet>, IReportGeneratorDetRepository, IPersistenceMasterRepository<ReportGeneratorDetBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReportGeneratorDetBusiness, ReportGeneratorDet>, IItemAsyncMapper<ReportGeneratorDet, ReportGeneratorDetBusiness>
	{
		public Task<ReportGeneratorDet> MapAsync(string contextGuid, ReportGeneratorDetBusiness source, ReportGeneratorDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReportGeneratorDet();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ParameterName = source.ParameterName;
				destination.OrderIndex = source.OrderIndex;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorDetPersistenceRepository.cs", 64);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ReportGeneratorDetBusiness> MapAsync(string contextGuid, ReportGeneratorDet source, ReportGeneratorDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReportGeneratorDetBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ParameterName = source.ParameterName;
				destination.OrderIndex = source.OrderIndex;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorDetPersistenceRepository.cs", 92);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ReportGeneratorDet>> MapListAsync(string contextGuid, IEnumerable<ReportGeneratorDetBusiness> source, List<ReportGeneratorDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReportGeneratorDetBusiness>> MapListAsync(string contextGuid, IEnumerable<ReportGeneratorDet> source, List<ReportGeneratorDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ReportGeneratorDet, ReportGeneratorDetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReportGeneratorDetBusiness, ReportGeneratorDet> MapToDatabase => Mapper;

	public ReportGeneratorDetPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorDetPersistenceRepository.cs", 32);
		}
	}
}
