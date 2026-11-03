using System;
using System.Collections.Generic;
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
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class UploadFilePersistenceRepository : PersistenceRepository<UploadFileBusiness, UploadFile>, IUploadFileRepository, IRepository<UploadFileBusiness>, INoGuidRepository<UploadFileBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<UploadFileBusiness, UploadFile>, IItemAsyncMapper<UploadFile, UploadFileBusiness>
	{
		public Task<UploadFile> MapAsync(string contextGuid, UploadFileBusiness source, UploadFile destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs"))
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
						destination = new UploadFile();
					}
					destination.Id = source.Id;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.Crc = source.Crc;
					destination.FileManagementParts = source.FileManagementParts;
					destination.MinChunk = source.MinChunk;
					destination.MaxChunk = source.MaxChunk;
					destination.UrlPath = source.UrlPath;
					destination.PartCount = source.PartCount;
					destination.PartSent = source.PartSent;
					destination.SubDirectory = source.SubDirectory;
					destination.RetryCount = source.RetryCount;
					destination.InsertTime = source.InsertTime;
					destination.EntityGuid = source.EntityGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 143);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<UploadFileBusiness> MapAsync(string contextGuid, UploadFile source, UploadFileBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs"))
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
						destination = new UploadFileBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Id = source.Id;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.Crc = source.Crc;
					destination.FileManagementParts = source.FileManagementParts;
					destination.MinChunk = source.MinChunk;
					destination.MaxChunk = source.MaxChunk;
					destination.UrlPath = source.UrlPath;
					destination.PartCount = source.PartCount;
					destination.PartSent = source.PartSent;
					destination.SubDirectory = source.SubDirectory;
					destination.RetryCount = source.RetryCount;
					destination.InsertTime = source.InsertTime;
					destination.EntityGuid = source.EntityGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 181);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<UploadFile>> MapListAsync(string contextGuid, IEnumerable<UploadFileBusiness> source, List<UploadFile> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<UploadFileBusiness>> MapListAsync(string contextGuid, IEnumerable<UploadFile> source, List<UploadFileBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<UploadFile, UploadFileBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<UploadFileBusiness, UploadFile> MapToDatabase => Mapper;

	public override IItemMapper<UploadFile, UploadFileBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public UploadFilePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<UploadFileBusiness> GetAsync(long crc, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 24));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs"))
		{
			UploadFileBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					UploadFile source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<UploadFile>((IQueryable<UploadFile>)c.UploadFile, (Expression<Func<UploadFile, bool>>)((UploadFile w) => w.Crc == crc), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 35);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<UploadFileBusiness> GetByFileIdAsync(Guid fileid, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 45));
		using (new JaegerMonitor("GetByFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs"))
		{
			UploadFileBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					UploadFile source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<UploadFile>((IQueryable<UploadFile>)c.UploadFile, (Expression<Func<UploadFile, bool>>)((UploadFile w) => w.Id == fileid), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 56);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByFileIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<bool> IsSentCompleteAsync(Guid fileId, CancellationToken token, string tranName)
	{
		Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "IsSentCompleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 66));
		using (new JaegerMonitor("IsSentCompleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs"))
		{
			bool result;
			try
			{
				result = ((IQueryable<UploadFilePart>)global_ModelContext.UploadFilePart).Count((UploadFilePart w) => w.Guid == fileId && w.IsSent == false) <= 0;
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
				WebErrorLog.Instance.StartLog(ex2, "", "IsSentCompleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 76);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await global_ModelContext.DisposeAsync("IsSentCompleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\UploadFilePersistenceRepository.cs", 98);
		}
	}
}
