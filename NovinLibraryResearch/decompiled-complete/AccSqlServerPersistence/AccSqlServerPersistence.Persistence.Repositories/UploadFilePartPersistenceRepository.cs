using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Uploader;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class UploadFilePartPersistenceRepository : PersistenceRepository<UploadFilePartBusiness, UploadFilePart>, IUploadFilePartRepository, IRepository<UploadFilePartBusiness>, INoGuidRepository<UploadFilePartBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<UploadFilePartBusiness, UploadFilePart>, IItemAsyncMapper<UploadFilePart, UploadFilePartBusiness>
	{
		public Task<UploadFilePart> MapAsync(string contextGuid, UploadFilePartBusiness source, UploadFilePart destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
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
						destination = new UploadFilePart();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Order = source.Order;
					destination.FileId = source.FileId;
					destination.RetryCount = source.RetryCount;
					destination.IsSent = source.IsSent;
					destination.NextTry = source.NextTry;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 344);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<UploadFilePartBusiness> MapAsync(string contextGuid, UploadFilePart source, UploadFilePartBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
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
						destination = new UploadFilePartBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Order = source.Order;
					destination.FileId = source.FileId;
					destination.RetryCount = source.RetryCount;
					destination.IsSent = source.IsSent;
					destination.NextTry = source.NextTry;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 374);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<UploadFilePart>> MapListAsync(string contextGuid, IEnumerable<UploadFilePartBusiness> source, List<UploadFilePart> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<UploadFilePartBusiness>> MapListAsync(string contextGuid, IEnumerable<UploadFilePart> source, List<UploadFilePartBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<UploadFilePart, UploadFilePartBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<UploadFilePartBusiness, UploadFilePart> MapToDatabase => Mapper;

	public override IItemMapper<UploadFilePart, UploadFilePartBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal UploadFilePartPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public new async Task<UploadFilePartBusiness> GetAsync(Guid id, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 26));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			UploadFilePartBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					UploadFilePart source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UploadFilePart>((IQueryable<UploadFilePart>)c.UploadFilePart, (Expression<Func<UploadFilePart, bool>>)((UploadFilePart w) => w.Guid == id), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<UploadFilePartBusiness>> GetLastFilePartAsync(int top, bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 47));
		using (new JaegerMonitor("GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			IEnumerable<UploadFilePartBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<UploadFilePart> queryable = ((IQueryable<UploadFilePart>)c.UploadFilePart)?.Where((UploadFilePart w) => !w.IsSent && w.NextTry <= DateTime.Now);
					if (status.HasValue)
					{
						queryable = queryable?.Where((UploadFilePart q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.Take(top).ToList(), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 59);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task ResetSentAsync(Guid fileId, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ResetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 71));
		using (new JaegerMonitor("ResetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			try
			{
				UploadFilePart uploadFilePart = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UploadFilePart>((IQueryable<UploadFilePart>)c.UploadFilePart, (Expression<Func<UploadFilePart, bool>>)((UploadFilePart w) => w.FileId == fileId), token);
				uploadFilePart.IsSent = false;
				((DbContext)c).Update<UploadFilePart>(uploadFilePart);
			}
			catch (Exception ex)
			{
				if (tryCount > 0)
				{
					await Task.Delay(1000);
					UploadFilePartPersistenceRepository uploadFilePartPersistenceRepository = this;
					Guid fileId2 = fileId;
					short num = (short)(tryCount - 1);
					tryCount = num;
					await uploadFilePartPersistenceRepository.ResetSentAsync(fileId2, num, token, tranName);
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, "", "ResetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 87);
				}
			}
		}
	}

	public async Task<int> GetTotalPartCountAsync(Guid fileId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetTotalPartCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetTotalPartCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 93));
			int result;
			try
			{
				result = ((IQueryable<UploadFilePart>)global_ModelContext.UploadFilePart).Count((UploadFilePart w) => w.FileId == fileId);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetTotalPartCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 102);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await global_ModelContext.DisposeAsync("GetTotalPartCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> GetPartSentCountAsync(Guid fileId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetPartSentCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetPartSentCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 113));
			int result;
			try
			{
				result = ((IQueryable<UploadFilePart>)global_ModelContext.UploadFilePart).Count((UploadFilePart w) => w.FileId == fileId && w.IsSent);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetPartSentCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 122);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await global_ModelContext.DisposeAsync("GetPartSentCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<string>> GetFileOrderAsync(Guid fileId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetFileOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetFileOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 133));
			List<string> result;
			try
			{
				List<Guid> obj = (from o in ((IQueryable<UploadFilePart>)global_ModelContext.UploadFilePart)?.Where((UploadFilePart w) => w.FileId == fileId)
					orderby o.Order
					select o into s
					select s.Guid).ToList();
				List<string> list = new List<string>(obj.Count());
				foreach (Guid item in obj)
				{
					list.Add(item.ToString() + ".tmp");
				}
				result = list;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetFileOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 146);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await global_ModelContext.DisposeAsync("GetFileOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAllAsync(Guid fileid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 162));
				UploadFilePart uploadFilePart = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UploadFilePart>((IQueryable<UploadFilePart>)c.UploadFilePart, (Expression<Func<UploadFilePart, bool>>)((UploadFilePart w) => w.FileId == fileid), token);
				c.UploadFilePart.Remove(uploadFilePart);
			}
			catch (Exception ex)
			{
				if (tryCount > 0)
				{
					await Task.Delay(1000);
					UploadFilePartPersistenceRepository uploadFilePartPersistenceRepository = this;
					Guid fileid2 = fileid;
					short num = (short)(tryCount - 1);
					tryCount = num;
					return await uploadFilePartPersistenceRepository.RemoveAllAsync(fileid2, num, token, tranName);
				}
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 173);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 174);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSentAsync(Guid filePart, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 183));
		using (new JaegerMonitor("SetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				UploadFilePart uploadFilePart = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UploadFilePart>((IQueryable<UploadFilePart>)c.UploadFilePart, (Expression<Func<UploadFilePart, bool>>)((UploadFilePart w) => w.FileId == filePart), token);
				uploadFilePart.IsSent = true;
				((DbContext)c).Update<UploadFilePart>(uploadFilePart);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 194);
				ret.AddReturnedValue(ex, "", "SetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 195);
			}
			return ret;
		}
	}

	public async Task<Guid> GetFileIdAsync(Guid filePart, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			Global_ModelContext dbcontext = null;
			try
			{
				dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 208));
				using DbDataReader dbDataReader = await dbcontext.ExecuteReaderAsync($"SELECT FileId FROM Acc_UploadFilePart WHERE Guid='{filePart}'", null, tranName, token, CommandType.StoredProcedure, 30, "GetFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				if (dbDataReader.Read())
				{
					return (Guid)dbDataReader[0];
				}
				return Guid.Empty;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 218);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					dbcontext?.Dispose("GetFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				}
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(UploadFilePartBusiness item, byte[] bytes, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 235));
				UploadFilePart dbobject = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				UploadFilePart uploadFilePart = dbobject;
				byte[] array = bytes;
				if (array == null)
				{
					array = await GetBytesAsync(item.Guid, token, tranName);
				}
				uploadFilePart.Bytes = array;
				dbcontext.UploadFilePart.Add(dbobject);
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
				if (tryCount < 0)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 250);
					throw;
				}
				await Task.Delay(1000);
				UploadFilePartPersistenceRepository uploadFilePartPersistenceRepository = this;
				short num = (short)(tryCount - 1);
				tryCount = num;
				return await uploadFilePartPersistenceRepository.SaveAsync(item, bytes, num, token, tranName);
			}
			return ret;
		}
	}

	public async Task<byte[]> GetBytesAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 259));
			byte[] result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<UploadFilePart>((IQueryable<UploadFilePart>)c.UploadFilePart, (Expression<Func<UploadFilePart, bool>>)((UploadFilePart p) => p.Guid == guid), token))?.Bytes;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 269);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task SetRetryAsync(DateTime addMinutes, Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SetRetryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs"))
		{
			await dbContext.GetContextAsync(tranName, 60000, "SetRetryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 281);
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentException("tranname is null or empty!");
			}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePartPersistenceRepository.cs", 308);
		}
	}
}
