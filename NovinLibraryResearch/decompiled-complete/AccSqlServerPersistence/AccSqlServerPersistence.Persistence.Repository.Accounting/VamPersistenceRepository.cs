using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository.Accounting;

internal class VamPersistenceRepository : PersistenceRepository<VamBusiness, Vam>, IVamRepository, IRepository<VamBusiness>, INoGuidRepository<VamBusiness>, IDisposable
{
	public class mapper : IItemAsyncMapper<VamBusiness, Vam>, IItemAsyncMapper<Vam, VamBusiness>
	{
		public Task<Vam> MapAsync(string contextGuid, VamBusiness source, Vam destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Vam();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Id2 = source.Id2;
				destination.Zarib = source.Zarib;
				destination.Description = source.Description;
				destination.DateM = source.DateM;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.VamMoeinGuid = source.VamMoeinGuid;
				destination.VamTafsilGuid = source.VamTafsilGuid;
				destination.SepordeMoeinGuid = source.SepordeMoeinGuid;
				destination.SepordeTafsilGuid = source.SepordeTafsilGuid;
				destination.Status = source.Status;
				destination.AghsatGuid = source.AghsatGuid;
				destination.Sum = source.Sum;
				destination.ZamenGuid1 = source.ZamenGuid1;
				destination.ZamenGuid2 = source.ZamenGuid2;
				destination.ZamenGuid3 = source.ZamenGuid3;
				destination.ZamenGuid4 = source.ZamenGuid4;
				destination.Date1 = source.Date1;
				destination.Date2 = source.Date2;
				destination.Average = source.Average;
				destination.AverageSum = source.AverageSum;
				destination.AverageType = source.AverageType;
				destination.ProfitPercent = source.ProfitPercent;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs", 138);
				throw;
			}
		}

		public Task<VamBusiness> MapAsync(string contextGuid, Vam source, VamBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new VamBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Id2 = source.Id2;
				destination.Zarib = source.Zarib;
				destination.Description = source.Description;
				destination.DateM = source.DateM;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.VamMoeinGuid = source.VamMoeinGuid;
				destination.VamTafsilGuid = source.VamTafsilGuid;
				destination.SepordeMoeinGuid = source.SepordeMoeinGuid;
				destination.SepordeTafsilGuid = source.SepordeTafsilGuid;
				destination.Status = source.Status;
				destination.AghsatGuid = source.AghsatGuid;
				destination.Sum = source.Sum;
				destination.ZamenGuid1 = source.ZamenGuid1;
				destination.ZamenGuid2 = source.ZamenGuid2;
				destination.ZamenGuid3 = source.ZamenGuid3;
				destination.ZamenGuid4 = source.ZamenGuid4;
				destination.Date1 = source.Date1;
				destination.Date2 = source.Date2;
				destination.Average = source.Average;
				destination.AverageSum = source.AverageSum;
				destination.AverageType = source.AverageType;
				destination.ProfitPercent = source.ProfitPercent;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs", 184);
				throw;
			}
		}

		public Task<List<Vam>> MapListAsync(string contextGuid, IEnumerable<VamBusiness> source, List<Vam> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<VamBusiness>> MapListAsync(string contextGuid, IEnumerable<Vam> source, List<VamBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<Vam, VamBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<VamBusiness, Vam> MapToDatabase => Mapper;

	public override IItemMapper<Vam, VamBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public VamPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs", 40);
		}
	}

	public async Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs", 44));
		int result;
		try
		{
			_ = 1;
			try
			{
				result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Vam>(((IQueryable<Vam>)c.Vam).OrderByDescending((Vam x) => x.Id2).Take(1), token))?.Id2 ?? 0) + 1;
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
				WebErrorLog.Instance.StartLog(ex2, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs", 54);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<VamModel>> GetAllModelAsync(bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs", 65));
		using (new JaegerMonitor("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs"))
		{
			IEnumerable<VamModel> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Vam> queryable = (IQueryable<Vam>)EntityFrameworkQueryableExtensions.Include<Vam, Tafsil>((IQueryable<Vam>)c.Vam, (Expression<Func<Vam, Tafsil>>)((Vam q) => q.VamGirandeTafsil));
					if (status.HasValue)
					{
						queryable = queryable.Where((Vam p) => p.Status == ((bool?)status).Value);
					}
					if (token.IsCancellationRequested || queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IQueryable<VamModel> queryable2 = queryable.Select((Vam q) => new VamModel
						{
							DateM = q.DateM,
							Description = q.Description,
							Guid = q.Guid,
							Id2 = q.Id2,
							Price = q.Sum,
							TafsilName = q.VamGirandeTafsil.Name
						});
						object obj;
						if (queryable2 == null)
						{
							obj = null;
						}
						else
						{
							IOrderedQueryable<VamModel> orderedQueryable = queryable2.OrderBy((VamModel q) => q.DateM);
							if (orderedQueryable == null)
							{
								obj = null;
							}
							else
							{
								IOrderedQueryable<VamModel> orderedQueryable2 = orderedQueryable.ThenBy((VamModel q) => q.Id2);
								obj = ((orderedQueryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<VamModel>((IQueryable<VamModel>)orderedQueryable2, token) : null);
							}
						}
						result = await (Task<List<VamModel>>)obj;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs", 86);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\VamPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
