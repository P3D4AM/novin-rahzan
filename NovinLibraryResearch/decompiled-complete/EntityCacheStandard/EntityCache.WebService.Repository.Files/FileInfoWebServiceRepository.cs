using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Files;

public class FileInfoWebServiceRepository : WebServiceRepository<FileInfoBusiness>, IDisposable
{
	private readonly string imagePath;

	public FileInfoWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<FileInfoBusiness>)cache.CacheRepository.IFileInfo, cache)
	{
		try
		{
			Cache = cache;
			imagePath = cache.ImagePath;
			imagePath = imagePath ?? throw new ArgumentNullException("imagePath");
			if (string.IsNullOrEmpty(imagePath))
			{
				throw new ArgumentException("Image path is empty", "imagePath");
			}
			cache.Context.WebFileInfo.OnReadResponse += FileInfoInstance_OnReadResponse;
			Cache.Context.WebFileInfo.OnSync += FileInfoInstance_OnSync;
			Cache.Context.WebFileInfo.OnSaveResponse += FileInstance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 31);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebFileInfo.OnReadResponse -= FileInfoInstance_OnReadResponse;
				Cache.Context.WebFileInfo.OnSync -= FileInfoInstance_OnSync;
				Cache.Context.WebFileInfo.OnSaveResponse -= FileInstance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 43);
		}
	}

	private async Task FileInfoInstance_OnSync(object sender, ConnectionEventArgs<WebFileInfo> e)
	{
		using (new JaegerMonitor("FileInfoInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveFilesAsync(e), ReturnedState.None, "FileInfoInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 53);
				if (!(Cache?.IsDisposed ?? true) && Cache.BufferEnable && !e.Connection.cancellationToken.IsCancellationRequested)
				{
					Cache.SendData(Sync.GenerateSyncDone(EntityPart.FileInformations, e.PartitionId, ret), Priority.High);
				}
			}
			catch (TimeoutException)
			{
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, $"Line:{line}", "FileInfoInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 59);
			}
		}
	}

	private async Task FileInfoInstance_OnReadResponse(object sender, ConnectionEventArgs<WebFileInfo> e)
	{
		using (new JaegerMonitor("FileInfoInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				await FileInfoMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData);
				await SaveFilesAsync(e);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "FileInfoInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 71);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveFilesAsync(ConnectionEventArgs<WebFileInfo> e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (e?.ConnectionData == null || !e.ConnectionData.Any() || e.Connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 89);
			}
			List<WebFileInfo> notChangedFiles = new List<WebFileInfo>();
			foreach (WebFileInfo item in e.ConnectionData)
			{
				FileInfoBusiness fileInfoBusiness = await FileInfoBusiness.GetAsync(base.ContextGuid, item.Guid, e.Connection.cancellationToken);
				if (fileInfoBusiness != null && fileInfoBusiness.Crc == item.Crc && fileInfoBusiness.Status == item.Status)
				{
					notChangedFiles.Add(item);
				}
			}
			if (notChangedFiles?.Any() ?? false)
			{
				e.ConnectionData = e.ConnectionData.Except(notChangedFiles).ToList();
			}
			if (!(e.ConnectionData?.Any() ?? false))
			{
				return ret;
			}
			List<FileInfoBusiness> fileBusinesses = await FileInfoMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData);
			if (e.Connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 102);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await FileInfoBusiness.RemoveAllAsync(e.Connection.ContextGuid, fileBusinesses?.Where((FileInfoBusiness w) => !w.Status), 10), ReturnedState.None, "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 103);
			if (e.Connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 104);
			}
			line = 8;
			bool send2Server = Cache.Source == ENSource.Accounting;
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await FileInfoBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, 10, fileBusinesses?.Where((FileInfoBusiness w) => w.Status), send2Server, Priority.Low, 10), ReturnedState.None, "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 108);
			if (e.Connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 109);
			}
			line = 13;
			if (ret.HasError)
			{
				return ret;
			}
			line = 14;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 116);
			ret.AddReturnedValue(ex, "", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 117);
		}
		return ret;
	}

	private async Task FileInstance_OnSaveResponse(object sende, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("FileInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (e == null || !(e.SaveResponses?.Count > 0))
				{
					return;
				}
				line = 2;
				foreach (SaveResponse resopnse in e.SaveResponses)
				{
					line = 3;
					if (resopnse != null)
					{
						line = 4;
						(await FileInfoBusiness.GetAsync(e.Connection.ContextGuid, resopnse.Guid))?.SetSaveResultAsync(resopnse, e.Connection.cancellationToken, 10);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "FileInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Files\\FileInfoWebServiceRepository.cs", 163);
			}
		}
	}
}
