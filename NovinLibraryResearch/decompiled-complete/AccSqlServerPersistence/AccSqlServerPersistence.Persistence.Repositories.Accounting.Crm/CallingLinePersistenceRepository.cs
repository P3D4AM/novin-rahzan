using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Crm;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Crm;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Crm;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Crm;

internal class CallingLinePersistenceRepository : PersistenceRepository<CallingLineBusiness, CallingLine>, ICallingLineRepository, IRepository<CallingLineBusiness>, INoGuidRepository<CallingLineBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CallingLineBusiness, CallingLine>, IItemAsyncMapper<CallingLine, CallingLineBusiness>
	{
		public Task<CallingLine> MapAsync(string contextGuid, CallingLineBusiness source, CallingLine destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs"))
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
						destination = new CallingLine();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ComputerName = source.ComputerName;
					destination.LineNumber = source.LineNumber;
					destination.DeviceNumber = source.DeviceNumber;
					destination.IsCo = source.IsCo;
					destination.CashierGuid = source.CashierGuid;
					destination.Status = source.Status;
					destination.Number = source.Number ?? "";
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs", 119);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public async Task<CallingLineBusiness> MapAsync(string contextGuid, CallingLine source, CallingLineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs"))
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
						destination = await CallingLineBusiness.GenerateNewWithDefaultsAsync(contextGuid, EnSystem.Mapper, null, "", tranName);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ComputerName = source.ComputerName;
					destination.LineNumber = source.LineNumber;
					destination.DeviceNumber = source.DeviceNumber;
					destination.IsCo = source.IsCo;
					destination.CashierGuid = source.CashierGuid;
					destination.Status = source.Status;
					destination.Number = source.Number ?? "";
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs", 151);
					throw;
				}
				return destination;
			}
		}

		public Task<List<CallingLine>> MapListAsync(string contextGuid, IEnumerable<CallingLineBusiness> source, List<CallingLine> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CallingLineBusiness>> MapListAsync(string contextGuid, IEnumerable<CallingLine> source, List<CallingLineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<CallingLine, CallingLineBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CallingLineBusiness, CallingLine> MapToDatabase => Mapper;

	public override IItemMapper<CallingLine, CallingLineBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public CallingLinePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs", 39);
		}
	}

	public async Task<CallingLineBusiness> GetAsync(string ComputerName, short DeviceNumber, short LineNumber, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs", 44));
			CallingLineBusiness result;
			try
			{
				_ = 1;
				try
				{
					CallingLine source = ((IQueryable<CallingLine>)c.CallingLine).FirstOrDefault((CallingLine q) => (string.IsNullOrEmpty(ComputerName) || q.ComputerName == ComputerName) && q.DeviceNumber == DeviceNumber && q.LineNumber == LineNumber);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, "", token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs", 59);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CallingLineBusiness> GetAsync(string number, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs", 70));
			CallingLineBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CallingLine source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CallingLine>((IQueryable<CallingLine>)c.CallingLine, (Expression<Func<CallingLine, bool>>)((CallingLine q) => q.Number == number), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs", 81);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLinePersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
