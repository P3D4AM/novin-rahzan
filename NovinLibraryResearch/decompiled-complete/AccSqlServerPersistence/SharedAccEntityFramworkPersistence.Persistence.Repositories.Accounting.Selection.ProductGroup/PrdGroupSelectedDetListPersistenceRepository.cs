using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Selection.ProductGroup;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Selection.ProductGroup;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Selection.ProductGroup;

internal class PrdGroupSelectedDetListPersistenceRepository : PersistenceMasterRepository<PrdGroupSelectedDetListBusiness, ProductGroupSelectedDetails>, IPrdGroupSelectedDetListRepository, IPersistenceMasterRepository<PrdGroupSelectedDetListBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdGroupSelectedDetListBusiness, ProductGroupSelectedDetails>, IItemAsyncMapper<ProductGroupSelectedDetails, PrdGroupSelectedDetListBusiness>
	{
		public Task<ProductGroupSelectedDetails> MapAsync(string contextGuid, PrdGroupSelectedDetListBusiness source, ProductGroupSelectedDetails destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductGroupSelectedDetails();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.GroupGuid = source.GetGroupGuid();
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description ?? "";
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedDetListPersistenceRepository.cs", 59);
				throw;
			}
			return Task.FromResult(destination);
		}

		public async Task<PrdGroupSelectedDetListBusiness> MapAsync(string contextGuid, ProductGroupSelectedDetails source, PrdGroupSelectedDetListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdGroupSelectedDetListBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				await destination.SetGroupGuid(source.GroupGuid);
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedDetListPersistenceRepository.cs", 85);
				throw;
			}
			return destination;
		}

		public Task<List<ProductGroupSelectedDetails>> MapListAsync(string contextGuid, IEnumerable<PrdGroupSelectedDetListBusiness> source, List<ProductGroupSelectedDetails> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdGroupSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductGroupSelectedDetails> source, List<PrdGroupSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ProductGroupSelectedDetails, PrdGroupSelectedDetListBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdGroupSelectedDetListBusiness, ProductGroupSelectedDetails> MapToDatabase => Mapper;

	public PrdGroupSelectedDetListPersistenceRepository(ContextHolder dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedDetListPersistenceRepository.cs", 34);
		}
	}
}
