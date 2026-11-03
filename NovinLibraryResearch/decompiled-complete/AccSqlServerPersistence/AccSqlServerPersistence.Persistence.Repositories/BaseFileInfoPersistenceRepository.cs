using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

public class BaseFileInfoPersistenceRepository : PersistenceSyncRepository<FileInfoBusiness, FileInfo>, IFileInfoRepository, ISyncRepository<FileInfoBusiness>, IEditableDelivery, IRepository<FileInfoBusiness>, INoGuidRepository<FileInfoBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<FileInfoBusiness, FileInfo>, IItemAsyncMapper<FileInfo, FileInfoBusiness>
	{
		public async Task<FileInfo> MapAsync(string contextGuid, FileInfoBusiness source, FileInfo destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
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
						destination = new FileInfo();
					}
					destination.Guid = source.Guid;
					destination.Crc = source.Crc;
					destination.FileLen = source.FileLen;
					destination.FileName = source.FileName;
					destination.Part = source.Part;
					destination.SubDirectory = source.SubDirectory;
					destination.Status = source.Status;
					destination.EntityGuid = source.EntityGuid;
					destination.LocalFullPath = "";
					destination.FullPath = "";
					destination.Modified = source.Modified;
					destination.Url = source.Url;
					destination.FileContent = source.FileContent;
					destination.CompressMode = source.CompressMode;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.MetaData = "";
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 391);
					throw;
				}
				return destination;
			}
		}

		public Task<FileInfoBusiness> MapAsync(string contextGuid, FileInfo source, FileInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
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
						destination = new FileInfoBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Crc = source.Crc;
					destination.FileLen = source.FileLen;
					destination.FileName = source.FileName;
					destination.Part = source.Part;
					destination.SubDirectory = source.SubDirectory;
					destination.Status = source.Status;
					destination.EntityGuid = source.EntityGuid;
					destination.SetFullPath(source.FullPath);
					destination.Modified = source.Modified;
					destination.Url = source.Url;
					destination.FileContent = source.FileContent;
					destination.CompressMode = source.CompressMode;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 432);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<FileInfo>> MapListAsync(string contextGuid, IEnumerable<FileInfoBusiness> source, List<FileInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<FileInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<FileInfo> source, List<FileInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private SemaphoreSlim saveSemaphore = new SemaphoreSlim(1, 1);

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<FileInfo, FileInfoBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<FileInfoBusiness, FileInfo> MapToDatabase => Mapper;

	public override IItemMapper<FileInfo, FileInfoBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public BaseFileInfoPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<FileInfoBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (FileInfoBusiness item in items)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 40);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 44);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 45);
		}
		return ret;
	}

	public override Task<IEnumerable<FileInfoBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		return GetAllAsync(true, token, tranName);
	}

	public override Task<IEnumerable<FileInfoBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new InvalidOperationException("getAll for file info is not valid!");
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(FileInfoBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await saveSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			if (item.Part == FileManagement_Parts.PicPrd && item.Status)
			{
				FileInfoBusiness fileInfoBusiness = (await GetAsync(item.EntityGuid, FileManagement_Parts.PicPrd, justFromCache: false, token, tranName))?.SingleOrDefault((FileInfoBusiness p) => p.Status && p.Guid != item.Guid);
				if (fileInfoBusiness != null)
				{
					try
					{
						throw new InvalidOperationException("کالا در حال حاضر تصویر مرجع دارد و تصویر مرجع دیگری برای این کالا در حال درج شدن هست.".GetTranslate());
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 71);
					}
					fileInfoBusiness.Status = false;
					fileInfoBusiness.FileContent = null;
					fileInfoBusiness.ServerStatus = ServerStatusForClient.None;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await base.SaveAsync(fileInfoBusiness, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 76);
					await ClsCache.TransactionDestiny(ContextGuid, tranName, isRollBack: false, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 77);
				}
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 80);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 84);
			res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 85);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				saveSemaphore.Release();
			}
		}
		return res;
	}

	public async Task<List<FileInfoBusiness>> GetFilesByPartAsync(FileManagement_Parts part, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 93));
		using (new JaegerMonitor("GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
		{
			List<FileInfoBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<FileInfo> source = ((IQueryable<FileInfo>)c.FileInfoEntity)?.Where((FileInfo x) => x.Part == part && x.Status)?.ToList();
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 105);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<FileInfoBusiness> GetAsync(string name, string subDirectory, FileManagement_Parts part, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 116));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
		{
			FileInfoBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					FileInfo source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<FileInfo>((IQueryable<FileInfo>)c.FileInfoEntity, (Expression<Func<FileInfo, bool>>)((FileInfo p) => p.FileName == name && p.SubDirectory == subDirectory && p.Part == part && p.Status), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 128);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			await saveSemaphore.WaitAsync();
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 145);
			saveSemaphore.Release();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 150);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 151);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		WebErrorLog.Instance.StartLog("!!!!!! Remove Range FileInfo !!!!!!", "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 158);
		return new ReturnedSaveFuncInfo();
	}

	public override async Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		WebErrorLog.Instance.StartLog("!!!!!! Keep Range FileInfo !!!!!!", "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 163);
		return new ReturnedSaveFuncInfo();
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 178);
		}
	}

	public async Task<List<FileInfoBusiness>> GetAsync(Guid entityGuid, FileManagement_Parts part, bool justFromCache, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 183));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
		{
			List<FileInfoBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<FileInfo> source = await EntityFrameworkQueryableExtensions.ToListAsync<FileInfo>(((IQueryable<FileInfo>)c.FileInfoEntity).Where((FileInfo p) => p.EntityGuid == entityGuid && p.Part == part && p.Status), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 195);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<FileInfoBusiness>> GetAllCompressedImageAsync(CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllCompressedImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 206));
		using (new JaegerMonitor("GetAllCompressedImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
		{
			List<FileInfoBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<FileInfo> source = ((IQueryable<FileInfo>)c.FileInfoEntity).Where((FileInfo p) => p.CompressMode != ENCompressedMode.None && p.Status == true && p.FileContent != null && (p.Part == FileManagement_Parts.PicPrdOthers || p.Part == FileManagement_Parts.PicPrd || p.Part == FileManagement_Parts.PicCustomer || p.Part == FileManagement_Parts.PicSign || p.Part == FileManagement_Parts.PicPrdGroup)).ToList();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllCompressedImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 228);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllCompressedImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override FileInfoBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<FileInfoBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 240));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
		{
			FileInfoBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					FileInfo source = await c.FileInfoEntity.FindAsync(new object[1] { guid });
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 251);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<FileInfoBusiness>> GetWrongExtentionsAsync(CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetWrongExtentionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 262));
		using (new JaegerMonitor("GetWrongExtentionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
		{
			List<FileInfoBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<FileInfo> source = await EntityFrameworkQueryableExtensions.ToListAsync<FileInfo>(((IQueryable<FileInfo>)c.FileInfoEntity).Where((FileInfo p) => !(p.FileName.ToLower().EndsWith(".jpg") || p.FileName.ToLower().EndsWith(".jpeg") || p.FileName.ToLower().EndsWith(".gif") || p.FileName.ToLower().EndsWith(".png") || p.FileName.ToLower().EndsWith(".bmp")) && p.Status == true && p.FileContent != null && (p.Part == FileManagement_Parts.PicPrdOthers || p.Part == FileManagement_Parts.PicPrd || p.Part == FileManagement_Parts.PicCustomer || p.Part == FileManagement_Parts.PicSign || p.Part == FileManagement_Parts.PicPrdGroup)), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetWrongExtentionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 290);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetWrongExtentionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<FileInfoBusiness> GetNoImageAsync(FileManagement_Parts part, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNoImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 301));
		using (new JaegerMonitor("GetNoImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs"))
		{
			FileInfoBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					FileInfo fileInfo = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<FileInfo>((IQueryable<FileInfo>)c.FileInfoEntity, (Expression<Func<FileInfo, bool>>)((FileInfo q) => q.EntityGuid == Guid.Empty && q.Part == part && q.Status), token);
					result = ((fileInfo != null) ? (await MapFromDatabase.MapAsync(ContextGuid, fileInfo, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNoImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 313);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNoImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<Guid>> GetAllGuidListAsync(FileInfoFilter filter, CancellationToken token, string tranName)
	{
		Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 324));
		List<Guid> result;
		try
		{
			IQueryable<FileInfo> queryable = (IQueryable<FileInfo>)global_ModelContext.FileInfoEntity;
			if (filter != null)
			{
				if (filter.Status.HasValue)
				{
					queryable = queryable.Where((FileInfo q) => q.Status == filter.Status.Value);
				}
				if (filter.Part.HasValue)
				{
					queryable = queryable.Where((FileInfo q) => q.Part == filter.Part.Value);
				}
			}
			result = ((queryable != null && queryable.Any()) ? queryable?.Select((FileInfo q) => q.Guid)?.OrderBy((Guid q) => Guid.NewGuid())?.ToList() : null);
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs", 340);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await global_ModelContext.DisposeAsync("GetAllGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BaseFileInfoPersistenceRepository.cs");
			}
		}
		return result;
	}
}
