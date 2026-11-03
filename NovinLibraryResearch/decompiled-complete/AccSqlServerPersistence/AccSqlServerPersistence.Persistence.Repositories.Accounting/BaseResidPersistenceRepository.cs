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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal abstract class BaseResidPersistenceRepository : PersistenceRepository<ResidBusiness, Resid>, IResidRepository, IRepository<ResidBusiness>, INoGuidRepository<ResidBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ResidBusiness, Resid>, IItemAsyncMapper<Resid, ResidBusiness>
	{
		public Task<Resid> MapAsync(string contextGuid, ResidBusiness source, Resid destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Resid();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ID2 = source.ID2;
				destination.DateM = source.DateM;
				destination.Description = source.Description ?? "";
				destination.Date1 = source.Date1;
				destination.Date2 = source.Date2;
				destination.GuidVahed = source.GuidVahed;
				destination.Price = source.Price;
				destination.Qt = source.Qt;
				destination.Mandeh = source.Mandeh;
				destination.PrdGuid = source.GetPrdGuid();
				destination.PrdModel = source.PrdModel ?? "";
				destination.PrdColor = source.PrdColor ?? "";
				destination.PrdSerial = source.PrdSerial ?? "";
				destination.TafsilGuid = source.TafsilGuid;
				destination.Problems = source.Problems ?? "";
				destination.Darkhast = source.Darkhast ?? "";
				destination.StatusId = source.StatusId;
				destination.CashierGuid = source.CashierGuid;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.Reserve = source.Reserve ?? "";
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 247);
				throw;
			}
			return Task.FromResult(destination);
		}

		public async Task<ResidBusiness> MapAsync(string contextGuid, Resid source, ResidBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			_ = 3;
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = await ResidBusiness.GetFactoryAsync(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ID2 = source.ID2;
				destination.DateM = source.DateM;
				destination.Description = source.Description;
				destination.Date1 = source.Date1;
				destination.Date2 = source.Date2;
				await destination.SetGuidVahedAsync(source.GuidVahed);
				destination.Price = source.Price;
				destination.Qt = source.Qt;
				destination.Mandeh = source.Mandeh;
				await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper);
				destination.PrdModel = source.PrdModel;
				destination.PrdColor = source.PrdColor;
				destination.PrdSerial = source.PrdSerial;
				await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper);
				destination.Problems = source.Problems;
				destination.Darkhast = source.Darkhast;
				destination.StatusId = source.StatusId;
				destination.CashierGuid = source.CashierGuid;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.Reserve = source.Reserve;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 292);
				throw;
			}
			return destination;
		}

		public Task<List<Resid>> MapListAsync(string contextGuid, IEnumerable<ResidBusiness> source, List<Resid> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ResidBusiness>> MapListAsync(string contextGuid, IEnumerable<Resid> source, List<ResidBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Resid, ResidBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ResidBusiness, Resid> MapToDatabase => Mapper;

	public override IItemMapper<Resid, ResidBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal BaseResidPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<ResidBusiness> GetAsync(long Id2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 34));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs"))
		{
			ResidBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Resid source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Resid>((IQueryable<Resid>)c.Resid, (Expression<Func<Resid, bool>>)((Resid p) => p.ID2 == Id2), default(CancellationToken));
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 43);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override Task<IEnumerable<ResidBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<ResidCompactBusiness>> GetAllCompactAsync(ResidFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<long> GetNextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 56));
		using (new JaegerMonitor("GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs"))
		{
			long result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					if (statusId.GetValueOrDefault() <= 0)
					{
						goto IL_02ea;
					}
					SanadStatusReport status = await c.SanadStatusReport.FindAsync(new object[1] { statusId.Value });
					if ((status?.Id2From).GetValueOrDefault() <= 0)
					{
						goto IL_02ea;
					}
					List<long> list = await EntityFrameworkQueryableExtensions.ToListAsync<long>(((IQueryable<Resid>)c.Resid).Select((Resid q) => q.ID2), token);
					long num2 = status.Id2From.Value;
					while (true)
					{
						if (num2 < status.Id2To.Value)
						{
							if (!list.Contains((int)num2))
							{
								result = (int)num2;
								break;
							}
							num2++;
							continue;
						}
						result = (int)status.Id2From.Value;
						break;
					}
					goto end_IL_00f3;
					IL_02ea:
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Resid>(((IQueryable<Resid>)c.Resid).OrderByDescending((Resid x) => x.ID2).Take(1), token))?.ID2 ?? 0) + 1;
					end_IL_00f3:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 80);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs");
				}
			}
			return result;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 99);
		}
	}

	public async Task<ResidBusiness> GetByOrderGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 103));
		using (new JaegerMonitor("GetByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs"))
		{
			ResidBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Resid resid = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Resid>((IQueryable<Resid>)c.Resid, (Expression<Func<Resid, bool>>)((Resid x) => x.OrderGuid == guid), token);
					result = ((resid != null) ? (await MapFromDatabase.MapAsync(ContextGuid, resid, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 115);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ResidBusiness> GetByReceptionGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 126));
		using (new JaegerMonitor("GetByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs"))
		{
			ResidBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Resid resid = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Resid>((IQueryable<Resid>)c.Resid, (Expression<Func<Resid, bool>>)((Resid x) => x.ReceptionGuid == guid), token);
					result = ((resid != null) ? (await MapFromDatabase.MapAsync(ContextGuid, resid, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 138);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<string>> GetSuggestionListAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetSuggestionListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 149));
		List<string> result;
		try
		{
			IQueryable<Resid> resid = (IQueryable<Resid>)acc_ModelContext.Resid;
			if (resid == null || !resid.Any())
			{
				result = null;
			}
			else
			{
				List<string> list = new List<string>();
				IQueryable<string> queryable = resid.Select((Resid q) => q.PrdModel)?.Where((string q) => !string.IsNullOrEmpty(q));
				IQueryable<string> queryable2 = resid.Select((Resid q) => q.PrdColor)?.Where((string q) => !string.IsNullOrEmpty(q));
				IQueryable<string> queryable3 = resid.Select((Resid q) => q.PrdSerial)?.Where((string q) => !string.IsNullOrEmpty(q));
				IQueryable<string> queryable4 = resid.Select((Resid q) => q.Problems)?.Where((string q) => !string.IsNullOrEmpty(q));
				IQueryable<string> queryable5 = resid.Select((Resid q) => q.Darkhast)?.Where((string q) => !string.IsNullOrEmpty(q));
				IQueryable<string> queryable6 = resid.Select((Resid q) => q.Reserve)?.Where((string q) => !string.IsNullOrEmpty(q));
				if (queryable != null && queryable.Any())
				{
					list.AddRange(queryable);
				}
				if (queryable2 != null && queryable2.Any())
				{
					list.AddRange(queryable2);
				}
				if (queryable3 != null && queryable3.Any())
				{
					list.AddRange(queryable3);
				}
				if (queryable4 != null && queryable4.Any())
				{
					list.AddRange(queryable4);
				}
				if (queryable5 != null && queryable5.Any())
				{
					list.AddRange(queryable5);
				}
				if (queryable6 != null && queryable6.Any())
				{
					list.AddRange(queryable6);
				}
				result = list;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetSuggestionListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 175);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetSuggestionListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 191));
				Resid resid = await context.Resid.FindAsync(new object[1] { guid });
				if (resid == null)
				{
					return res;
				}
				resid.StatusId = (short)statusId;
				resid.Modified = DateTime.Now;
				((DbContext)context).Update<Resid>(resid);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 201);
				res.AddReturnedValue(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\BaseResidPersistenceRepository.cs", 202);
			}
			return res;
		}
	}
}
