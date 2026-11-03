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

internal class TolidMavadPersistenceRepository : PersistenceMasterRepository<TolidMavadBusiness, TolidMavad>, ITolidMavadRepository, IPersistenceMasterRepository<TolidMavadBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidMavad, TolidMavadBusiness>, IItemAsyncMapper<TolidMavadBusiness, TolidMavad>
	{
		public async Task<TolidMavadBusiness> MapAsync(string contextGuid, TolidMavad source, TolidMavadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			_ = 1;
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = new TolidMavadBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = DateTime.Now;
				destination.MasterGuid = source.MasterGuid;
				await destination.SetProductGuid(source.ProductGuid, EnSystem.Mapper, tranName);
				await destination.SetDepotGuid(source.DepotGuid, EnSystem.Mapper, tranName);
				destination.Quantity = source.Quantity;
				destination.Serial = source.Serial;
				destination.Price = source.Price;
				destination.Description = source.Description ?? "";
				destination.V2Guid = source.V2Guid;
				destination.V2Price = source.V2Price;
				destination.RowSum = source.RowSum;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 163);
				throw;
			}
		}

		public Task<TolidMavad> MapAsync(string contextGuid, TolidMavadBusiness source, TolidMavad destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidMavad();
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
				destination.RowSum = source.RowSum;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 198);
				throw;
			}
		}

		public Task<List<TolidMavadBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidMavad> source, List<TolidMavadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidMavad>> MapListAsync(string contextGuid, IEnumerable<TolidMavadBusiness> source, List<TolidMavad> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private PrdSerialPersistanceRepository _serialRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidMavad, TolidMavadBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidMavadBusiness, TolidMavad> MapToDatabase => Mapper;

	public TolidMavadPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
		_serialRepo = new PrdSerialPersistanceRepository(dbContext);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(Guid masterGuid, List<TolidMavadBusiness> list, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 35));
			IQueryable<TolidMavad> queryable = ((IQueryable<TolidMavad>)c.TolidMavads).Where((TolidMavad q) => q.MasterGuid == masterGuid);
			if (queryable != null && queryable.Any())
			{
				List<Guid> existGuidList = list?.Select((TolidMavadBusiness q) => q.Guid)?.ToList();
				if (existGuidList?.Any() ?? false)
				{
					IQueryable<TolidMavad> queryable2 = queryable.Where((TolidMavad q) => !existGuidList.Contains(q.Guid));
					if (queryable2 != null && queryable2.Any())
					{
						c.TolidMavads.RemoveRange((IEnumerable<TolidMavad>)queryable2);
					}
				}
			}
			if (res.HasError || !(list?.Any() ?? false))
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(list, masterGuid, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 48);
			if (res.HasError)
			{
				return res;
			}
			foreach (TolidMavadBusiness item in list)
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
				returnedSaveFuncInfo.AddReturnedValue(await _serialRepo.SaveRangeAsync(item.SerialStr, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 65);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 70);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 71);
		}
		return res;
	}

	public async Task GetAllAsync(TolidBusiness tolid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 79));
			try
			{
				int num;
				_ = num - 1;
				_ = 6;
				try
				{
					IQueryable<TolidMavad> queryable = ((IQueryable<TolidMavad>)c.TolidMavads).Where((TolidMavad x) => x.MasterGuid == tolid.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_012f;
					}
					foreach (TolidMavad item2 in queryable)
					{
						TolidMavadBusiness detmodel = await MapFromDatabase.MapAsync(base.Context.Guid, item2, new TolidMavadBusiness(base.ContextGuid), tranName, token);
						IQueryable<Guid> queryable2 = ((IQueryable<PrdSerialDetails>)c.PrdSerialDetails).Where((PrdSerialDetails q) => q.DetailGuid == detmodel.Guid && q.AsType == Document.Tolid_Mavad)?.Select((PrdSerialDetails q) => q.MasterGuid);
						if (queryable2 != null && queryable2.Any())
						{
							foreach (Guid item3 in queryable2)
							{
								PrdSerialBusiness prdSerialBusiness = await _serialRepo.GetAsync(item3, token, tranName);
								if (prdSerialBusiness != null)
								{
									await detmodel.SetSerialAsync(prdSerialBusiness, null, EnSystem.Mapper);
								}
							}
						}
						TolidMavadMapper instance = TolidMavadMapper.Instance;
						string contextGuid = base.ContextGuid;
						TolidMavadBusiness source = detmodel;
						string contextGuid2 = base.ContextGuid;
						OrderDetailBusiness item = await instance.MapAsync(contextGuid, source, await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid2, await tolid.GetMavadListAsync(EnSystem.Mapper, tranName, token), token, tranName), tranName, token);
						await tolid.AddMavadListAsync(EnSystem.Mapper, new List<OrderDetailBusiness> { item }, clearDets: false, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 106);
					throw;
				}
				return;
				end_IL_011f:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs");
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
				_serialRepo = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidMavadPersistenceRepository.cs", 127);
		}
	}
}
