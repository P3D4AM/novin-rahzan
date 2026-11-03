using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidExportPersistenceRepository : PersistenceMasterRepository<TolidExportBusiness, TolidExport>, ITolidExportRepository, IPersistenceMasterRepository<TolidExportBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidExport, TolidExportBusiness>, IItemAsyncMapper<TolidExportBusiness, TolidExport>
	{
		public async Task<TolidExportBusiness> MapAsync(string contextGuid, TolidExport source, TolidExportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidExportBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = DateTime.Now;
				destination.MasterGuid = source.MasterGuid;
				await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
				destination.DepotGuid = source.DepotGuid;
				destination.Quantity = source.Quantity;
				destination.Serial = source.Serial;
				destination.Price = source.Price;
				destination.Description = source.Description ?? "";
				destination.V2Guid = source.V2Guid;
				destination.V2Price = source.V2Price;
				destination.V2Qt = source.V2Qt;
				destination.TolidFormulaGuid = source.TolidFormulaGuid;
				destination.TolidFormulaIndex = source.TolidFormulaIndex;
				return destination;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 159);
				throw;
			}
		}

		public Task<TolidExport> MapAsync(string contextGuid, TolidExportBusiness source, TolidExport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidExport();
				}
				destination.Guid = source.Guid;
				destination.Modified = DateTime.Now;
				destination.MasterGuid = source.MasterGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.DepotGuid = source.DepotGuid;
				destination.Quantity = source.Quantity;
				destination.Serial = source.Serial;
				destination.Price = source.Price;
				destination.Description = source.Description ?? "";
				destination.V2Guid = source.V2Guid;
				destination.V2Price = source.V2Price;
				destination.V2Qt = source.V2Qt;
				destination.TolidFormulaGuid = source.TolidFormulaGuid;
				destination.TolidFormulaIndex = source.TolidFormulaIndex;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 194);
				throw;
			}
		}

		public Task<List<TolidExportBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidExport> source, List<TolidExportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidExport>> MapListAsync(string contextGuid, IEnumerable<TolidExportBusiness> source, List<TolidExport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private PrdSerialPersistanceRepository _serialRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidExport, TolidExportBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidExportBusiness, TolidExport> MapToDatabase => Mapper;

	public TolidExportPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
		_serialRepo = new PrdSerialPersistanceRepository(dbContext);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(Guid masterGuid, List<TolidExportBusiness> list, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 34));
			IQueryable<TolidExport> queryable = ((IQueryable<TolidExport>)c.TolidExports).Where((TolidExport q) => q.MasterGuid == masterGuid);
			if (queryable != null && queryable.Any())
			{
				List<Guid> existGuidList = list?.Select((TolidExportBusiness q) => q.Guid)?.ToList();
				if (existGuidList?.Any() ?? false)
				{
					IQueryable<TolidExport> queryable2 = queryable.Where((TolidExport q) => !existGuidList.Contains(q.Guid));
					if (queryable2 != null && queryable2.Any())
					{
						c.TolidExports.RemoveRange((IEnumerable<TolidExport>)queryable2);
					}
				}
			}
			if (res.HasError || !(list?.Any() ?? false))
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(list, masterGuid, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 47);
			if (res.HasError)
			{
				return res;
			}
			foreach (TolidExportBusiness item in list)
			{
				if (!(item?.SerialStr?.Any()).GetValueOrDefault())
				{
					continue;
				}
				IQueryable<PrdSerialDetails> queryable3 = ((IQueryable<PrdSerialDetails>)c.PrdSerialDetails).Where((PrdSerialDetails q) => q.DetailGuid == item.Guid);
				if (queryable3 != null && queryable3.Any())
				{
					IQueryable<PrdSerialDetails> queryable4 = queryable3.Where((PrdSerialDetails q) => !item.SerialStr.Select((PrdSerialBusiness q) => q.Guid).Contains(q.MasterGuid));
					if (queryable4 != null && queryable4.Any())
					{
						c.PrdSerialDetails.RemoveRange((IEnumerable<PrdSerialDetails>)queryable4);
					}
				}
				foreach (PrdSerialBusiness item2 in item.SerialStr)
				{
					if (!(item2?.DetList?.Any()).GetValueOrDefault())
					{
						continue;
					}
					foreach (PrdSerialDetailBusiness det in item2.DetList)
					{
						det.DetailGuid = item.Guid;
					}
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _serialRepo.SaveRangeAsync(item.SerialStr, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 64);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 69);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 70);
		}
		return res;
	}

	public async Task GetAllAsync(TolidBusiness tolid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 78));
			try
			{
				int num;
				_ = num - 1;
				_ = 6;
				try
				{
					IQueryable<TolidExport> queryable = ((IQueryable<TolidExport>)c.TolidExports).Where((TolidExport x) => x.MasterGuid == tolid.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_012f;
					}
					foreach (TolidExport item in queryable)
					{
						TolidExportBusiness detmodel = await MapFromDatabase.MapAsync(base.Context.Guid, item, new TolidExportBusiness(base.ContextGuid), tranName, token);
						IQueryable<Guid> queryable2 = ((IQueryable<PrdSerialDetails>)c.PrdSerialDetails).Where((PrdSerialDetails q) => q.DetailGuid == detmodel.Guid && q.AsType == Document.Tolid_Mahsul)?.Select((PrdSerialDetails q) => q.MasterGuid);
						if (queryable2 != null && queryable2.Any())
						{
							foreach (Guid item2 in queryable2)
							{
								PrdSerialBusiness prdSerialBusiness = await _serialRepo.GetAsync(item2, token, tranName);
								if (prdSerialBusiness != null)
								{
									await detmodel.SetSerialAsync(prdSerialBusiness, null, EnSystem.Mapper);
								}
							}
						}
						TolidExportMapper instance = TolidExportMapper.Instance;
						string contextGuid = base.ContextGuid;
						TolidExportBusiness source = detmodel;
						string contextGuid2 = base.ContextGuid;
						OrderDetailBusiness obj = await instance.MapAsync(contextGuid, source, await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid2, await tolid.GetExportListAsync(EnSystem.Mapper, tranName, token), token, tranName), tranName, token);
						await tolid.AddExportAsync(EnSystem.Mapper, obj, token, tranName);
					}
					goto end_IL_011f;
					end_IL_012f:;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 105);
					throw;
				}
				return;
				end_IL_011f:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs");
				}
			}
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidExportPersistenceRepository.cs", 123);
		}
	}
}
