using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Note;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Note;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Note;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.NoteRepo;

internal class NoteGroupPerstenceRepository : PersistenceRepository<NoteGroupBusiness, NoteGroup>, INoteGroupRepository, IRepository<NoteGroupBusiness>, INoGuidRepository<NoteGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<NoteGroupBusiness, NoteGroup>, IItemAsyncMapper<NoteGroup, NoteGroupBusiness>
	{
		public Task<NoteGroup> MapAsync(string contextGuid, NoteGroupBusiness source, NoteGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new NoteGroup();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ParentGuid = source.ParentGuid;
				destination.Name = source.Name;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 131);
				throw;
			}
		}

		public Task<NoteGroupBusiness> MapAsync(string contextGuid, NoteGroup source, NoteGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new NoteGroupBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ParentGuid = source.ParentGuid;
				destination.Name = source.Name;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 157);
				throw;
			}
		}

		public Task<List<NoteGroup>> MapListAsync(string contextGuid, IEnumerable<NoteGroupBusiness> source, List<NoteGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<NoteGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<NoteGroup> source, List<NoteGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<NoteGroup, NoteGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<NoteGroupBusiness, NoteGroup> MapToDatabase => Mapper;

	public override IItemMapper<NoteGroup, NoteGroupBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal NoteGroupPerstenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public virtual async Task<NoteGroupBusiness> CheckNameAsync(Guid guid, string Name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 31));
		using (new JaegerMonitor("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs"))
		{
			NoteGroupBusiness result;
			try
			{
				_ = 1;
				try
				{
					NoteGroup source = ((IQueryable<NoteGroup>)c.NoteGroup).FirstOrDefault((NoteGroup x) => x.Name == Name && x.Guid != guid && x.Status);
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
					WebErrorLog.Instance.StartLog(ex3, "", "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 43);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<int> CountGroupChildsAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CountGroupChildsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 54));
		using (new JaegerMonitor("CountGroupChildsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs"))
		{
			int result;
			try
			{
				int num = ((IQueryable<NoteGroup>)acc_ModelContext.NoteGroup).Count((NoteGroup x) => x.ParentGuid == groupGuid);
				result = num;
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
				WebErrorLog.Instance.StartLog(ex3, "", "CountGroupChildsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 66);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("CountGroupChildsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<List<NoteGroupBusiness>> GetAllAsync(Guid parentGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 77));
		List<NoteGroupBusiness> result;
		try
		{
			_ = 1;
			try
			{
				List<NoteGroup> source = ((IQueryable<NoteGroup>)c.NoteGroup)?.Where((NoteGroup q) => q.ParentGuid == parentGuid).ToList();
				result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 87);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs");
			}
		}
		return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NoteGroupPerstenceRepository.cs", 105);
		}
	}
}
