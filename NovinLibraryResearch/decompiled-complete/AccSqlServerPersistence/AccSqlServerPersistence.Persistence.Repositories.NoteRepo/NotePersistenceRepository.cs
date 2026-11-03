using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Note;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Note;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Note;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.NoteRepo;

internal class NotePersistenceRepository : PersistenceSyncRepository<NoteBusiness, Note>, INoteRepository, ISyncRepository<NoteBusiness>, IEditableDelivery, IRepository<NoteBusiness>, INoGuidRepository<NoteBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<NoteBusiness, Note>, IItemAsyncMapper<Note, NoteBusiness>
	{
		public Task<Note> MapAsync(string contextGuid, NoteBusiness source, Note destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Note();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Text = source.Text;
				destination.DateM = source.DateM;
				destination.DateSarResidM = source.DateSarResidM;
				destination.Pirority = source.Pirority;
				destination.Status = source.Status;
				destination.CashierGuid = source.CashierGuid;
				destination.GroupGuid = source.GroupGuid;
				destination.DisplayState = source.DisplayState;
				destination.EntityGuid = source.EntityGuid;
				destination.EntityType = source.EntityType;
				destination.PirorityName = source.PirorityName;
				destination.StatusName = source.StatusName;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DateUtc = destination.DateM.ToUniversalTime();
				destination.TafsilGuid = source.TafsilGuid;
				if (source.MetaData != null)
				{
					destination.MetaData = Json.ToStringJson(source.MetaData);
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 179);
				throw;
			}
		}

		public Task<NoteBusiness> MapAsync(string contextGuid, Note source, NoteBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new NoteBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Text = source.Text;
				destination.DateM = source.DateM;
				destination.DateSarResidM = source.DateSarResidM;
				destination.Pirority = source.Pirority;
				destination.Status = source.Status;
				destination.CashierGuid = source.CashierGuid;
				destination.GroupGuid = source.GroupGuid;
				destination.DisplayState = source.DisplayState;
				destination.EntityGuid = source.EntityGuid;
				destination.EntityType = source.EntityType;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DateUtc = source.DateUtc;
				destination.TafsilGuid = source.TafsilGuid;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<NoteMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 217);
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 224);
				throw;
			}
		}

		public Task<List<Note>> MapListAsync(string contextGuid, IEnumerable<NoteBusiness> source, List<Note> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<NoteBusiness>> MapListAsync(string contextGuid, IEnumerable<Note> source, List<NoteBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Note, NoteBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<NoteBusiness, Note> MapToDatabase => Mapper;

	public override IItemMapper<Note, NoteBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public NotePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<int> CountNumbersInGroupAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CountNumbersInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 33));
		using (new JaegerMonitor("CountNumbersInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs"))
		{
			int result;
			try
			{
				int num = ((IQueryable<Note>)acc_ModelContext.Note).Count((Note x) => x.GroupGuid == groupGuid);
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
				WebErrorLog.Instance.StartLog(ex3, "", "CountNumbersInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 45);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("CountNumbersInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<NoteBusiness>> GetAllAsync(Guid groupGuid, NoteStatus? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 56));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs"))
		{
			IEnumerable<NoteBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Note> queryable = ((IQueryable<Note>)c.Note)?.Where((Note q) => q.GroupGuid == groupGuid);
					if (status.HasValue)
					{
						queryable = queryable?.Where((Note q) => (int?)q.Status == (int?)status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<Note, NoteBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<Note>(queryable, token) : null), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 69);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<NoteBusiness>> GetAllAsync(CancellationToken token, NoteFilter filter, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 80));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs"))
		{
			List<NoteBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<string> list = filter?.Search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<Note> queryable = (IQueryable<Note>)c.Note;
						if (list != null && list.Any())
						{
							foreach (string word in list)
							{
								if (!string.IsNullOrEmpty(word))
								{
									queryable = queryable.Where((Note p) => DbFunctionsExtensions.Like(EF.Functions, p.Text, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Title, $"%{word}%"));
								}
							}
						}
						if (filter != null)
						{
							if (filter.Date1.HasValue)
							{
								queryable = queryable.Where((Note q) => q.DateM >= filter.Date1);
							}
							if (filter.Date2.HasValue)
							{
								queryable = queryable.Where((Note q) => q.DateM <= filter.Date2);
							}
							if (filter.CashierGuid.HasValue && filter.CashierGuid != Guid.Empty)
							{
								queryable = queryable.Where((Note q) => q.CashierGuid == filter.CashierGuid);
							}
							if (filter.Pirority.HasValue)
							{
								queryable = queryable.Where((Note q) => (int?)q.Pirority == (int?)filter.Pirority);
							}
							if (filter.GroupGuids?.Any() ?? false)
							{
								queryable = queryable.Where((Note q) => filter.GroupGuids.Contains(q.GroupGuid));
							}
							if (filter.Status.HasValue)
							{
								queryable = queryable.Where((Note p) => p.Status == filter.Status.Value);
							}
							if ((filter.AsGuid ?? Guid.Empty) != Guid.Empty)
							{
								queryable = queryable.Where((Note p) => p.EntityGuid == filter.AsGuid.Value);
							}
							if (filter.AsType.GetValueOrDefault() != 0)
							{
								queryable = queryable.Where((Note p) => (int?)p.EntityType == (int?)filter.AsType.Value);
							}
							if (filter.DateSarresidFrom.HasValue)
							{
								queryable = queryable.Where((Note q) => q.DateSarResidM != null && q.DateSarResidM >= filter.DateSarresidFrom);
							}
							if (filter.DateSarresidTo.HasValue)
							{
								queryable = queryable.Where((Note q) => q.DateSarResidM != null && q.DateSarResidM <= filter.DateSarresidTo);
							}
						}
						if (token.IsCancellationRequested || queryable == null || !queryable.Any())
						{
							result = null;
						}
						else
						{
							IItemAsyncMapper<Note, NoteBusiness> mapFromDatabase = MapFromDatabase;
							string contextGuid = ContextGuid;
							List<NoteBusiness> list2 = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<Note>(queryable, token), null, tranName, token);
							result = ((!token.IsCancellationRequested) ? list2?.OrderByDescending((NoteBusiness q) => q.DateSarResidM)?.ToList() : null);
						}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 115);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NoteRepo\\NotePersistenceRepository.cs", 133);
		}
	}
}
