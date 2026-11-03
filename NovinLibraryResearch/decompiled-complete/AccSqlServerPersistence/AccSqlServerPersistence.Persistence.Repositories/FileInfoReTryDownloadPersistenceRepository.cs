using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class FileInfoReTryDownloadPersistenceRepository : PersistenceRepository<FileInfoReTryDownloadBusiness, FileInfoReTryDownload>, IFileInfoReTryDownloadRepository, IRepository<FileInfoReTryDownloadBusiness>, INoGuidRepository<FileInfoReTryDownloadBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<FileInfoReTryDownloadBusiness, FileInfoReTryDownload>, IItemAsyncMapper<FileInfoReTryDownload, FileInfoReTryDownloadBusiness>
	{
		public async Task<FileInfoReTryDownload> MapAsync(string contextGuid, FileInfoReTryDownloadBusiness source, FileInfoReTryDownload destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			_ = 1;
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = new FileInfoReTryDownload();
				}
				destination.Guid = source.Guid;
				FileInfoReTryDownload fileInfoReTryDownload = destination;
				fileInfoReTryDownload.Crc = await source.GetCrcAsync();
				fileInfoReTryDownload = destination;
				fileInfoReTryDownload.FileLen = await source.GetFileLenAsync();
				destination.FileName = source.FileName;
				destination.Part = source.Part;
				destination.SubDirectory = source.SubDirectory;
				destination.Status = source.Status;
				destination.EntityGuid = source.EntityGuid;
				destination.LocalFullPath = source.FullPath;
				destination.FullPath = source.FullPath;
				destination.Modified = source.Modified;
				destination.Url = source.Url;
				destination.ReTryCount = source.ReTryCount;
				destination.NextTryCount = source.NextTryCount;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 151);
				throw;
			}
			return destination;
		}

		public Task<FileInfoReTryDownloadBusiness> MapAsync(string contextGuid, FileInfoReTryDownload source, FileInfoReTryDownloadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new FileInfoReTryDownloadBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.SetCrc(source.Crc);
				destination.SetFileLen(source.FileLen);
				destination.FileName = source.FileName;
				destination.Part = source.Part;
				destination.SubDirectory = source.SubDirectory;
				destination.Status = source.Status;
				destination.EntityGuid = source.EntityGuid;
				destination.FullPath = source.FullPath;
				destination.Modified = source.Modified;
				destination.Url = source.Url;
				destination.ReTryCount = source.ReTryCount;
				destination.NextTryCount = source.NextTryCount;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 186);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<FileInfoReTryDownload>> MapListAsync(string contextGuid, IEnumerable<FileInfoReTryDownloadBusiness> source, List<FileInfoReTryDownload> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<FileInfoReTryDownloadBusiness>> MapListAsync(string contextGuid, IEnumerable<FileInfoReTryDownload> source, List<FileInfoReTryDownloadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<FileInfoReTryDownload, FileInfoReTryDownloadBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<FileInfoReTryDownloadBusiness, FileInfoReTryDownload> MapToDatabase => Mapper;

	public override IItemMapper<FileInfoReTryDownload, FileInfoReTryDownloadBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public FileInfoReTryDownloadPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<FileInfoReTryDownloadBusiness>> GetFilesByPartAsync(FileManagement_Parts part, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 24));
		using (new JaegerMonitor("GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs"))
		{
			List<FileInfoReTryDownloadBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<FileInfoReTryDownload> source = ((IQueryable<FileInfoReTryDownload>)c.FileInfoReTryDownload)?.Where((FileInfoReTryDownload x) => x.Part == part).ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 36);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<FileInfoReTryDownloadBusiness> GetAsync(string name, string subDirectory, FileManagement_Parts part, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 47));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs"))
		{
			FileInfoReTryDownloadBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					FileInfoReTryDownload source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<FileInfoReTryDownload>((IQueryable<FileInfoReTryDownload>)c.FileInfoReTryDownload, (Expression<Func<FileInfoReTryDownload, bool>>)((FileInfoReTryDownload p) => p.FileName == name && p.Part == part && p.SubDirectory == subDirectory), token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 60);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<FileInfoReTryDownloadBusiness>> GetAllForDownloadAsync(short tryCount, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 71));
		using (new JaegerMonitor("GetAllForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs"))
		{
			List<FileInfoReTryDownloadBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<FileInfoReTryDownload> list = ((IQueryable<FileInfoReTryDownload>)c.FileInfoReTryDownload)?.Where((FileInfoReTryDownload p) => p.ReTryCount >= 10).ToList();
					c.FileInfoReTryDownload.RemoveRange((IEnumerable<FileInfoReTryDownload>)list);
					List<FileInfoReTryDownload> source = ((IQueryable<FileInfoReTryDownload>)c.FileInfoReTryDownload).Where((FileInfoReTryDownload p) => p.ReTryCount < 10 && p.NextTryCount < DateTime.Now).ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 86);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FileInfoReTryDownloadPersistenceRepository.cs", 108);
		}
	}
}
