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
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ShortCutPersistenceRepository : PersistenceRepository<ShortCutBusiness, ShortCut>, IShortCutRepository, IRepository<ShortCutBusiness>, INoGuidRepository<ShortCutBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ShortCutBusiness, ShortCut>, IItemAsyncMapper<ShortCut, ShortCutBusiness>
	{
		public Task<ShortCut> MapAsync(string contextGuid, ShortCutBusiness source, ShortCut destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ShortCut();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.FormName = source.FormName;
				destination.Name = source.Name;
				destination.ShortCutKey = source.ShortCutKey;
				destination.Func = source.Func;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.BackColor = source.BackColor;
				destination.ForeColor = source.ForeColor;
				destination.ImageSrc = source.ImageSrc;
				destination.Enable = source.Enable;
				destination.Visable = source.Visable;
				destination.SubItems = source.SubItems;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ShortCutPersistenceRepository.cs", 99);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ShortCutBusiness> MapAsync(string contextGuid, ShortCut source, ShortCutBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ShortCutBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.FormName = source.FormName;
				destination.Name = source.Name;
				destination.ShortCutKey = source.ShortCutKey;
				destination.Func = source.Func;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.BackColor = source.BackColor;
				destination.ForeColor = source.ForeColor;
				destination.ImageSrc = source.ImageSrc;
				destination.Enable = source.Enable;
				destination.Visable = source.Visable;
				destination.SubItems = source.SubItems;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ShortCutPersistenceRepository.cs", 136);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ShortCut>> MapListAsync(string contextGuid, IEnumerable<ShortCutBusiness> source, List<ShortCut> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ShortCutBusiness>> MapListAsync(string contextGuid, IEnumerable<ShortCut> source, List<ShortCutBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ShortCut, ShortCutBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ShortCutBusiness, ShortCut> MapToDatabase => Mapper;

	public override IItemMapper<ShortCut, ShortCutBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ShortCutPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<ShortCutBusiness> GetAsync(string formName, string name, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ShortCutPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ShortCutPersistenceRepository.cs", 25));
			ShortCutBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					ShortCut source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ShortCut>((IQueryable<ShortCut>)c.ShortCuts, (Expression<Func<ShortCut, bool>>)((ShortCut x) => x.FormName == formName && x.Name == name), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ShortCutPersistenceRepository.cs", 36);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ShortCutPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ShortCutPersistenceRepository.cs", 57);
		}
	}
}
