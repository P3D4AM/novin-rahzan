using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global;

internal class IntroductionMethodPersistenceRepository : PersistenceSyncRepository<IntroductionMethodBusiness, IntroductionMethod>, IIntroductionMethodRepository, ISyncRepository<IntroductionMethodBusiness>, IEditableDelivery, IRepository<IntroductionMethodBusiness>, INoGuidRepository<IntroductionMethodBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<IntroductionMethodBusiness, IntroductionMethod>, IItemAsyncMapper<IntroductionMethod, IntroductionMethodBusiness>
	{
		public Task<IntroductionMethod> MapAsync(string contextGuid, IntroductionMethodBusiness source, IntroductionMethod destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new IntroductionMethod();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\IntroductionMethodPersistenceRepository.cs", 94);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<IntroductionMethodBusiness> MapAsync(string contextGuid, IntroductionMethod source, IntroductionMethodBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new IntroductionMethodBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\IntroductionMethodPersistenceRepository.cs", 124);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<IntroductionMethod>> MapListAsync(string contextGuid, IEnumerable<IntroductionMethodBusiness> source, List<IntroductionMethod> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<IntroductionMethodBusiness>> MapListAsync(string contextGuid, IEnumerable<IntroductionMethod> source, List<IntroductionMethodBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	public override bool HasModified => true;

	public override bool HasStatus => true;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<IntroductionMethod, IntroductionMethodBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<IntroductionMethod, IntroductionMethodBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<IntroductionMethodBusiness, IntroductionMethod> MapToDatabase => Mapper;

	public IntroductionMethodPersistenceRepository(ContextHolder db)
		: base(db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\IntroductionMethodPersistenceRepository.cs", 41);
		}
	}

	public async Task<IntroductionMethodBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)dbContext.GetContext(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\IntroductionMethodPersistenceRepository.cs", 45);
		IntroductionMethodBusiness result;
		try
		{
			_ = 1;
			try
			{
				IntroductionMethod introductionMethod = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<IntroductionMethod>((IQueryable<IntroductionMethod>)c.IntroductionMethod, (Expression<Func<IntroductionMethod, bool>>)((IntroductionMethod q) => q.Name.ToLower() == name.ToLower()), token);
				result = ((introductionMethod != null) ? (await MapFromDatabase.MapAsync(ContextGuid, introductionMethod, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\IntroductionMethodPersistenceRepository.cs", 57);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\IntroductionMethodPersistenceRepository.cs");
			}
		}
		return result;
	}
}
