using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Business.Core;

public class BaseFileAttachmentBusiness
{
	private List<FileInfoBusiness> _addedFileList = new List<FileInfoBusiness>();

	private List<FileInfoBusiness> _removedFileList = new List<FileInfoBusiness>();

	public Task AddFileAsync(FileInfoBusiness file)
	{
		return AddFileAsync(file, _addedFileList);
	}

	internal static Task AddFileAsync(FileInfoBusiness file, List<FileInfoBusiness> addedList)
	{
		if (addedList == null)
		{
			throw new NullReferenceException("addedList");
		}
		try
		{
			if (file == null)
			{
				return Task.CompletedTask;
			}
			addedList.Add(file);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 28);
			throw;
		}
	}

	public Task RemoveFileAsync(FileInfoBusiness file)
	{
		return RemoveFileAsync(file, _removedFileList);
	}

	internal static Task RemoveFileAsync(FileInfoBusiness file, List<FileInfoBusiness> removedList)
	{
		if (removedList == null)
		{
			throw new NullReferenceException("removedList");
		}
		try
		{
			if (file == null)
			{
				return Task.CompletedTask;
			}
			removedList.Add(file);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 44);
			throw;
		}
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, IEnumerable<FileInfoBusiness> addedFiles, IEnumerable<FileInfoBusiness> removedFiles, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 2;
			try
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 56), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 56);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0495;
				}
				transactionHasBeenCreated = true;
				if (addedFiles?.Any() ?? false)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.SaveRangeAsync(addedFiles, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 60);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0495;
				}
				if (removedFiles?.Any() ?? false)
				{
					foreach (FileInfoBusiness removedFile in removedFiles)
					{
						removedFile.ClearFile();
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.SaveRangeAsync(removedFiles, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 65);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 70);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 71);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 76), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseFileAttachmentBusiness.cs", 76);
			}
		}
		return ret;
		IL_0495:
		return result;
	}
}
