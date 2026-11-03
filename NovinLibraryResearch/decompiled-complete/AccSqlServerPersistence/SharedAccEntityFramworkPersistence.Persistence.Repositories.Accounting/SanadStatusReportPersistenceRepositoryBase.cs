using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Report;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Sanad;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

internal abstract class SanadStatusReportPersistenceRepositoryBase : PersistenceSyncRepository<SanadStatusReportBusiness, SanadStatusReport>, ISanadStatusReportRepository, ISyncRepository<SanadStatusReportBusiness>, IEditableDelivery, IRepository<SanadStatusReportBusiness>, INoGuidRepository<SanadStatusReportBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SanadStatusReportBusiness, SanadStatusReport>, IItemAsyncMapper<SanadStatusReport, SanadStatusReportBusiness>
	{
		public Task<SanadStatusReport> MapAsync(string contextGuid, SanadStatusReportBusiness source, SanadStatusReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadStatusReport();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Id = source.Id;
				destination.AsType = source.AsType;
				destination.Name = source.Name;
				destination.Description = source.Description;
				destination.MetaData = Json.ToStringJson(source.MetaData);
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.ErrorMinutes = source.ErrorMinutes;
				destination.WarningMinutes = source.WarningMinutes;
				destination.Id2From = source.Id2From;
				destination.Id2To = source.Id2To;
				destination.InactivityDays = source.InactivityDays;
				destination.TargetStatusId = source.TargetStatusId;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 320);
				throw;
			}
		}

		public Task<SanadStatusReportBusiness> MapAsync(string contextGuid, SanadStatusReport source, SanadStatusReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadStatusReportBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Id = source.Id;
				destination.AsType = source.AsType;
				destination.Name = source.Name;
				destination.Description = source.Description;
				destination.MetaData = source.MetaData.FromJson<SanadStatusReportMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 340);
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.ErrorMinutes = source.ErrorMinutes;
				destination.WarningMinutes = source.WarningMinutes;
				destination.Id2From = source.Id2From;
				destination.Id2To = source.Id2To;
				destination.InactivityDays = source.InactivityDays;
				destination.TargetStatusId = source.TargetStatusId;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 361);
				throw;
			}
		}

		public Task<List<SanadStatusReport>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReportBusiness> source, List<SanadStatusReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SanadStatusReportBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReport> source, List<SanadStatusReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<SanadStatusReport, SanadStatusReportBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SanadStatusReport, SanadStatusReportBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SanadStatusReportBusiness, SanadStatusReport> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public SanadStatusReportPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = false;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 46);
		}
	}

	public async Task<SanadStatusReportBusiness> GetAsync(int id, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 50));
		SanadStatusReportBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				SanadStatusReport sanadStatusReport = await c.SanadStatusReport.FindAsync(new object[1] { id });
				result = ((sanadStatusReport != null) ? (await MapFromDatabase.MapAsync(ContextGuid, sanadStatusReport, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 62);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public override async Task<SanadStatusReportBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 73));
		SanadStatusReportBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				SanadStatusReport sanadStatusReport = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<SanadStatusReport>((IQueryable<SanadStatusReport>)c.SanadStatusReport, (Expression<Func<SanadStatusReport, bool>>)((SanadStatusReport q) => q.Guid == guid), token);
				result = ((sanadStatusReport != null) ? (await MapFromDatabase.MapAsync(ContextGuid, sanadStatusReport, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 85);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(SanadStatusReportBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 101));
				SanadStatusReport sanadStatusReport = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(sanadStatusReport, sanadStatusReport.Id), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 103);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 107);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 108);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<SanadStatusReportBusiness>> GetAllAsync(Document? asType, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 114));
		IEnumerable<SanadStatusReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<SanadStatusReport> queryable = (IQueryable<SanadStatusReport>)c.SanadStatusReport;
				if (asType.HasValue && asType != Document.None)
				{
					queryable = queryable?.Where((SanadStatusReport q) => q.AsType == ((Document?)asType).Value);
				}
				if (status.HasValue)
				{
					queryable = queryable?.Where((SanadStatusReport q) => q.Status == ((bool?)status).Value);
				}
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable.ToList(), null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 128);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public override async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 139));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				List<SanadStatusReport> list = await EntityFrameworkQueryableExtensions.ToListAsync<SanadStatusReport>(((IQueryable<SanadStatusReport>)c.SanadStatusReport).Where((SanadStatusReport q) => q.ServerStatus == ServerStatusForClient.None || q.ServerStatus == ServerStatusForClient.SendError), token);
				result = ((list != null && list.Any()) ? list.Select((SanadStatusReport q) => q.Guid).ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 150);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<int> GetNextIdAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 161));
		using (new JaegerMonitor("GetNextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<SanadStatusReport> queryable = ((IQueryable<SanadStatusReport>)c.SanadStatusReport).OrderByDescending((SanadStatusReport x) => x.Id).Take(1);
					SanadStatusReport sanadStatusReport = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<SanadStatusReport>(queryable, token) : null);
					result = ((sanadStatusReport == null) ? 1 : (sanadStatusReport.Id + 1));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 173);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 185));
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				SanadStatusReport sanadStatusReport = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<SanadStatusReport>((IQueryable<SanadStatusReport>)context.SanadStatusReport, (Expression<Func<SanadStatusReport, bool>>)((SanadStatusReport q) => q.Guid == item.Guid), token);
				if (sanadStatusReport == null)
				{
					return ret;
				}
				sanadStatusReport.HostRefrenced = item.HostRefrenced;
				sanadStatusReport.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				sanadStatusReport.DeliveryMessage = item.Message;
				sanadStatusReport.DeliveryShortMessage = item.Shortmessage;
				((DbContext)context).Update<SanadStatusReport>(sanadStatusReport);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 199);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 202);
				ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 203);
			}
			return ret;
		}
	}

	public virtual Task<IEnumerable<SanadStatusReportErrorTimeModel>> GetAllErrorTimeAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<int> GetMinErrorTimeAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetMinErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 210));
		int result;
		try
		{
			_ = 1;
			try
			{
				result = (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<SanadStatusReport>((from q in (IQueryable<SanadStatusReport>)c.SanadStatusReport
					where q.ErrorMinutes != 0
					select q into x
					orderby x.ErrorMinutes
					select x).Take(1), token))?.ErrorMinutes ?? 0;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetMinErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 220);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetMinErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<SanadStatusReportBusiness> GetAsync(string name, Document asType, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 231));
		SanadStatusReportBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				SanadStatusReport sanadStatusReport = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<SanadStatusReport>((IQueryable<SanadStatusReport>)c.SanadStatusReport, (Expression<Func<SanadStatusReport, bool>>)((SanadStatusReport q) => q.Name == name && q.AsType == asType), default(CancellationToken));
				result = ((sanadStatusReport != null) ? (await MapFromDatabase.MapAsync(ContextGuid, sanadStatusReport, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 243);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<List<SanadStatusReportBusiness>> IsDuplicateCodeAsync(long minCode, long maxCode, Document asType, Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 254));
		using (new JaegerMonitor("IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs"))
		{
			List<SanadStatusReportBusiness> result2;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (minCode == 0L && maxCode == 0L)
					{
						result2 = null;
					}
					else
					{
						List<SanadStatusReport> list = await EntityFrameworkQueryableExtensions.ToListAsync<SanadStatusReport>(((IQueryable<SanadStatusReport>)c.SanadStatusReport).Where((SanadStatusReport q) => q.Guid != guid && q.AsType == asType && q.Id2From != null && q.Id2To != null), token);
						if (list == null || !list.Any())
						{
							result2 = null;
						}
						else
						{
							List<SanadStatusReportBusiness> result = new List<SanadStatusReportBusiness>();
							foreach (SanadStatusReport item in list)
							{
								if ((minCode >= item.Id2From.Value && minCode <= item.Id2To.Value) || (maxCode >= item.Id2From.Value && maxCode <= item.Id2To.Value) || (minCode <= item.Id2From.Value && maxCode >= item.Id2To.Value))
								{
									List<SanadStatusReportBusiness> list2 = result;
									list2.Add(await MapFromDatabase.MapAsync(ContextGuid, item, null, tranName, token));
								}
							}
							result2 = result;
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
					WebErrorLog.Instance.StartLog(ex2, "", "IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs", 271);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportPersistenceRepositoryBase.cs");
				}
			}
			return result2;
		}
	}

	public virtual Task<IEnumerable<SanadStatusReportForceUpdateModel>> GetAllForceUpdateAsync(DateTime dateMoasser, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
