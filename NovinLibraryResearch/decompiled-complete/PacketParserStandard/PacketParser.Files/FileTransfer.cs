using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;

namespace PacketParser.Files;

public class FileTransfer : IDisposable
{
	public ReturnedSaveFuncInfo ErrorList;

	private string UserName;

	private string Password;

	private string HostAddress;

	private List<WebDepartment> LstDep;

	public string BaseDirectoryAddress { get; private set; } = "D:\\DownLoadBackup";


	public FileTransfer(string userName, string password, string hostAddress, IReturnedSaveFuncInfo iReturnedSaveFuncInfo, string destinationAddress = "")
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 46);
			ErrorList.AddReturnedValue(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 47);
		}
	}

	public async Task<ReturnedSaveFuncInfo> BackUpDeleteOnServer(long maxCapcity, CancellationToken token, short maxParalelDownloads = 5)
	{
		try
		{
			if (maxCapcity <= 0)
			{
				throw new ArgumentException(string.Format("value({0}) is ouf of range {1}", maxCapcity, "maxCapcity"));
			}
			if (ErrorList.HasError)
			{
				return ErrorList;
			}
			foreach (WebDepartment dep in LstDep)
			{
				ErrorList.AddInformation($"دپارتمان {dep.Name}({dep.Id})", "", "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 63);
				List<Task<ReturnedSaveFuncInfo>> downloadList = new List<Task<ReturnedSaveFuncInfo>>();
				if (token.IsCancellationRequested)
				{
					return ErrorList.AddErrorTokenIsCancelled("", "", "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 65);
				}
				ReturnedSaveFuncInfoWithValue<List<WebFileInfo>> returnedSaveFuncInfoWithValue = await WebFileInfoClient.GetFilesOutOfcapcity(dep.Id, FileManagement_Parts.NPBackupService, maxCapcity, HostAddress, UserName, Password, token);
				ErrorList.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 67);
				ReturnedSaveFuncInfo errorList;
				if ((returnedSaveFuncInfoWithValue?.value?.Any()).GetValueOrDefault())
				{
					ErrorList.AddInformation($"دپارتمان {dep.Name}({dep.Id}) تعداد({returnedSaveFuncInfoWithValue.value.Count}) فایل جهت دانلود موجود است.", "", "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 70);
					foreach (WebFileInfo webFile in returnedSaveFuncInfoWithValue.value)
					{
						downloadList.Add(Task.Run(() => DownloadFileAsync(dep.Id, webFile, token, 10), token));
						if (downloadList.Count >= maxParalelDownloads)
						{
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(downloadList);
							downloadList.Remove(task);
							errorList = ErrorList;
							errorList.AddReturnedValue(await task, ReturnedState.None, "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 78);
						}
					}
				}
				errorList = ErrorList;
				errorList.AddReturnedValue(await Task.WhenAll(downloadList), ReturnedState.None, "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 83);
				ErrorList.AddInformation($"DepId:{dep.Id} Transfer backups more than({maxParalelDownloads:N0} bytes) finished", "", "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 84);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 89);
			ErrorList.AddReturnedValue(ex, "", "BackUpDeleteOnServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 90);
		}
		finally
		{
			_ = 0;
		}
		return ErrorList;
	}

	private async Task<ReturnedSaveFuncInfo> DownloadFileAsync(short depId, WebFileInfo webFile, CancellationToken token, short tryCount = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 104);
				}
				if (string.IsNullOrEmpty(webFile.Url))
				{
					ret.AddWarning(" Url Address Is Empty", "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 106);
				}
				if (string.IsNullOrEmpty(webFile.FileName))
				{
					ret.AddWarning("File Name Is Empty", "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 108);
				}
				if (webFile.Crc <= 0)
				{
					ret.AddWarning("CrcNot Valid ", "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 110);
				}
				else
				{
					ReturnedSaveFuncInfoWithValue<DownLoadStatus> downloadResult = await new Uri(webFile.Url).DownnLoadFile(Path.Combine(BaseDirectoryAddress, depId.ToString(), webFile.FileName), token, webFile.Crc, FileDamgeHasError: false);
					if (downloadResult.HasError)
					{
						if (tryCount > 0 && !token.IsCancellationRequested)
						{
							await Task.Delay(60000);
							if (!token.IsCancellationRequested)
							{
								FileTransfer fileTransfer = this;
								short depId2 = depId;
								CancellationToken token2 = token;
								short num = (short)(tryCount - 1);
								tryCount = num;
								return await fileTransfer.DownloadFileAsync(depId2, webFile, token2, num);
							}
						}
						return ret.AddError(EnSystem.None, " Fiel " + webFile.FullPath + " Not DownLoad " + downloadResult.value.ToString() + " =>" + downloadResult.ErrorList.FirstOrDefault(), "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 124);
					}
					if (downloadResult.value != 0)
					{
						return ret.AddInformation(" Fiel Not Exsist " + webFile.FullPath + " For Download Or Download Later", "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 127);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await WebFileInfoClient.DeleteWebFileInforClient(webFile, HostAddress, UserName, Password), ReturnedState.None, "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 128);
				}
			}
			catch (Exception ex)
			{
				if (tryCount > 0 && !token.IsCancellationRequested)
				{
					await Task.Delay(60000);
					if (!token.IsCancellationRequested)
					{
						FileTransfer fileTransfer2 = this;
						short depId3 = depId;
						CancellationToken token3 = token;
						short num = (short)(tryCount - 1);
						tryCount = num;
						return await fileTransfer2.DownloadFileAsync(depId3, webFile, token3, num);
					}
				}
				WebErrorLog.Instance.StartLog(ex, "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 139);
				ret.AddReturnedValue(ex, "", "DownloadFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\FileTransfer.cs", 140);
			}
			return ret;
		}
	}

	public void Dispose()
	{
	}
}
