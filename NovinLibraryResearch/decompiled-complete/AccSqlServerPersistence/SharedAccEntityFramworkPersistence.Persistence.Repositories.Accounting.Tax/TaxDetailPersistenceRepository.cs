using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Tax;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tax;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Tax;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Tax;

internal class TaxDetailPersistenceRepository : PersistenceMasterRepository<TaxDetailBusiness, TaxClassDetail>, ITaxDetailRepository, IPersistenceMasterRepository<TaxDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TaxDetailBusiness, TaxClassDetail>, IItemAsyncMapper<TaxClassDetail, TaxDetailBusiness>
	{
		public Task<TaxClassDetail> MapAsync(string contextGuid, TaxDetailBusiness source, TaxClassDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TaxClassDetail();
				}
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.FeeTaxPercent = source.FeeTaxPercent;
				destination.ProductTaxPercent = source.ProductTaxPercent;
				destination.ProfitTaxPercent = source.ProfitTaxPercent;
				destination.Ordinal = source.Ordinal;
				destination.Description = source.Description ?? "";
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxDetailPersistenceRepository.cs", 94);
				throw;
			}
		}

		public Task<TaxDetailBusiness> MapAsync(string contextGuid, TaxClassDetail source, TaxDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			if (destination == null)
			{
				throw new ArgumentException("Please Pass New Destination");
			}
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.FeeTaxPercent = source.FeeTaxPercent;
				destination.ProductTaxPercent = source.ProductTaxPercent;
				destination.ProfitTaxPercent = source.ProfitTaxPercent;
				destination.Ordinal = source.Ordinal;
				destination.Description = source.Description;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxDetailPersistenceRepository.cs", 123);
				throw;
			}
		}

		public Task<List<TaxClassDetail>> MapListAsync(string contextGuid, IEnumerable<TaxDetailBusiness> source, List<TaxClassDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TaxDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxClassDetail> source, List<TaxDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TaxClassDetail, TaxDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TaxDetailBusiness, TaxClassDetail> MapToDatabase => Mapper;

	public TaxDetailPersistenceRepository(ContextHolder dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxDetailPersistenceRepository.cs", 37);
		}
	}

	public async Task GetAllAsync(TaxBusiness master, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxDetailPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxDetailPersistenceRepository.cs", 43));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					IQueryable<TaxClassDetail> queryable = ((IQueryable<TaxClassDetail>)c.TaxDetail).Where((TaxClassDetail x) => x.MasterGuid == master.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_011f;
					}
					foreach (TaxClassDetail item in queryable)
					{
						IItemAsyncMapper<TaxClassDetail, TaxDetailBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = base.ContextGuid;
						TaxClassDetail source = item;
						TaxDetailBusiness detail = await mapFromDatabase.MapAsync(contextGuid, source, await TaxDetailBusiness.GetFactoryAsync(master, token, tranName), tranName, token);
						await master.AddDetailListAsync(detail, token);
					}
					goto end_IL_010f;
					end_IL_011f:;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxDetailPersistenceRepository.cs", 59);
					throw;
				}
				return;
				end_IL_010f:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxDetailPersistenceRepository.cs");
				}
			}
		}
	}
}
