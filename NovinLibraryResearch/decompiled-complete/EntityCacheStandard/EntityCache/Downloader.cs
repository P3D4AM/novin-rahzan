using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using EntityCache.Business;
using Force.Crc32;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache;

public class Downloader
{
	private static ConcurrentDictionary<string, Downloader> _downloaders = new ConcurrentDictionary<string, Downloader>();

	public string ContextGuid { get; private set; }

	public Downloader()
	{
		throw new NotImplementedException();
	}

	public Downloader(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Downloader GetInstance(string contextGuid)
	{
		try
		{
			if (_downloaders.TryGetValue(contextGuid, out var value))
			{
				return value;
			}
			_downloaders.TryAdd(contextGuid.ToString(), new Downloader(contextGuid.ToString()));
			_downloaders.TryGetValue(contextGuid, out value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 50);
			return null;
		}
	}

	private ReturnedSaveFuncInfo CheckValidation(FileInfoBusiness fileInfo, string directory)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (!Path.IsPathRooted(directory))
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "directory is relative path. directory is:" + directory, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 61);
			}
			if (fileInfo == null)
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "fileInfo is null. fileInfo", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 63);
			}
			if (string.IsNullOrEmpty(directory))
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "directory is null or empty", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 65);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 69);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 70);
		}
		return returnedSaveFuncInfo;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> IsDataReadyForDownloadAsync(EnSystem system, FileInfoBusiness fileInfo, string directory, CancellationToken token, short tryCount = 10)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			string downloadPath = Path.Combine(directory, fileInfo.FileName);
			if (File.Exists(downloadPath))
			{
				await FileUtility.Instance.WaitForFileAsync(downloadPath, token);
				if (fileInfo.Crc == Crc32Algorithm.Compute(File.ReadAllBytes(downloadPath)))
				{
					res.AddInformation("فایل".GetTranslate() + "  " + fileInfo.FileName + " " + " به دلیل تطابق Crc دانلود نشد".GetTranslate(), "", "IsDataReadyForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 84);
				}
				else if (fileInfo.Modified < File.GetLastAccessTime(downloadPath))
				{
					res.AddInformation("فایل ".GetTranslate() + " " + fileInfo.FileName + "  " + "به دلیل بروز بودن دانلود نشد".GetTranslate(), "", "IsDataReadyForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 86);
				}
			}
			else
			{
				res.AddReturnedValue(ReturnedState.Information, downloadPath + " " + " موجود نیست must be downloaded!".GetTranslate(), "", "IsDataReadyForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 89);
			}
			ClsCache cache = ClsCache.GetCache(ContextGuid);
			if (cache.Source == ENSource.Android_HesabYar || cache.Source == ENSource.Android_SefareshGir)
			{
				res.AddError(system, "sefareshgitr download is disabled!", "IsDataReadyForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 92);
			}
			res.value = downloadPath + ".tmp";
		}
		catch (Exception ex)
		{
			if (tryCount > 0)
			{
				await Task.Delay(1000);
				Downloader downloader = this;
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await downloader.IsDataReadyForDownloadAsync(system, fileInfo, directory, token, num);
			}
			WebErrorLog.Instance.StartLog(ex, "", "IsDataReadyForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 102);
			res.AddReturnedValue(ex, "", "IsDataReadyForDownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 103);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> AfterDownload(EnSystem system, FileInfoBusiness fileInfo, string downloadPathTemp, string directory)
	{
		int line = 0;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			await Task.Delay(1000);
			line = 1;
			string downloadPath = Path.Combine(directory, fileInfo.FileName);
			if (!File.Exists(downloadPathTemp))
			{
				res.AddError(system, "111File not exists:" + downloadPathTemp, "", "AfterDownload", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 120);
				return res;
			}
			line = 2;
			try
			{
				if (!fileInfo.FileName.ToLower().EndsWith(".ico.tmp"))
				{
					throw new NotImplementedException("bitmap is not valid on mono platform!");
				}
			}
			catch (Exception exception)
			{
				res.AddReturnedValue(exception, "", "AfterDownload", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 133);
			}
			if (res.HasError)
			{
				return res;
			}
			await Task.Delay(100);
			if (!File.Exists(downloadPathTemp))
			{
				res.AddError(system, "333File not exists:" + downloadPathTemp, "", "AfterDownload", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 138);
				return res;
			}
			if (fileInfo.Crc != Crc32Algorithm.Compute(File.ReadAllBytes(downloadPathTemp)))
			{
				line = 4;
				File.Delete(downloadPathTemp);
				line = 5;
				res.AddReturnedValue(ReturnedState.Error, "crc not matched", "", "AfterDownload", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 146);
				line = 6;
				return res;
			}
			line = 7;
			if (File.Exists(downloadPath))
			{
				line = 8;
				File.Delete(downloadPath);
				line = 9;
			}
			line = 10;
			File.Move(downloadPathTemp, downloadPath);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "AfterDownload", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 162);
			res.AddReturnedValue(ex, "", "AfterDownload", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 163);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> DownloadAsync(EnSystem system, FileInfoBusiness fileInfo, string directory, CancellationToken token)
	{
		using (new JaegerMonitor("DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ReturnedSaveFuncInfoWithValue<string> dataReadyForDownload = new ReturnedSaveFuncInfoWithValue<string>();
			using SemaphoreSlim semaphor = await Locks.LocksInstance.GetSemaphoreAsync(fileInfo.FileName);
			try
			{
				int num;
				_ = num - 1;
				_ = 6;
				ReturnedSaveFuncInfo result;
				try
				{
					line = 1;
					await semaphor.WaitAsync(token);
					line = 2;
					ret.AddReturnedValue(CheckValidation(fileInfo, directory), ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 181);
					line = 3;
					if (ret.HasError)
					{
						result = ret;
						goto end_IL_0122;
					}
					line = 4;
					using (WebClient webClient = new WebClient())
					{
						line = 5;
						if (!Directory.Exists(directory))
						{
							Directory.CreateDirectory(directory);
						}
						line = 6;
						if (!string.IsNullOrEmpty(fileInfo.SubDirectory))
						{
							directory = Path.Combine(directory, fileInfo.SubDirectory);
						}
						line = 7;
						if (!Directory.Exists(directory))
						{
							Directory.CreateDirectory(directory);
						}
						line = 8;
						if (token.IsCancellationRequested)
						{
							line = 9;
							ret.AddReturnedValue(ReturnedState.Error, "canceel!", "", "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 199);
							result = ret;
						}
						else
						{
							line = 10;
							ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfo = await IsDataReadyForDownloadAsync(system, fileInfo, directory, token, 10);
							dataReadyForDownload.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 205);
							ReturnedSaveFuncInfo returnedSaveFuncInfo2 = ret;
							returnedSaveFuncInfo2.AddReturnedValue(await FileUtility.Instance.FixeFileAccessAsync(dataReadyForDownload.value, 10), ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 206);
							line = 11;
							if (dataReadyForDownload.HasError || string.IsNullOrEmpty(dataReadyForDownload.value))
							{
								line = 12;
								ret.AddReturnedValue(dataReadyForDownload, ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 212);
								result = ret;
							}
							else
							{
								line = 13;
								if (!File.Exists(dataReadyForDownload.value))
								{
									goto IL_05a8;
								}
								returnedSaveFuncInfo2 = ret;
								returnedSaveFuncInfo2.AddReturnedValue(await FileUtility.Instance.WaitForFileAsync(dataReadyForDownload.value, token), ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 218);
								if (!ret.HasError)
								{
									File.Delete(dataReadyForDownload.value);
									goto IL_05a8;
								}
								result = ret;
							}
						}
						goto end_IL_0122;
						IL_05a8:
						webClient.DownloadFile(fileInfo.Url, dataReadyForDownload.value);
						line = 141;
						ReturnedSaveFuncInfo downloadResult2 = await AfterDownload(system, fileInfo, dataReadyForDownload.value, directory);
						line = 142;
						if (!downloadResult2.HasError)
						{
							goto IL_0889;
						}
						line = 143;
						string text = HttpHelpers.ApiUrl + "Department/File/";
						line = 144;
						ReturnedSaveFuncInfoWithValue<WebResponse<string>> returnedSaveFuncInfoWithValue = await (text + "GetNewLink/" + HttpUtility.UrlEncode(fileInfo.Url)).GetFromApi<string>(token, 3);
						line = 145;
						ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 234);
						if (!ret.HasError)
						{
							if (returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode == HttpStatusCode.OK)
							{
								line = 146;
								webClient.DownloadFile(fileInfo.Url, dataReadyForDownload.value);
								line = 14;
								downloadResult2 = await AfterDownload(system, fileInfo, dataReadyForDownload.value, directory);
							}
							else
							{
								ret.AddError(system, "File download failed for firs try. in second try on GetNewLink error is :" + returnedSaveFuncInfoWithValue.value.Description, "", "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 245);
							}
							goto IL_0889;
						}
						result = ret;
						goto end_IL_0122;
						IL_0889:
						ret.AddReturnedValue(downloadResult2, ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 249);
					}
					goto end_IL_010e;
					end_IL_0122:;
				}
				catch (TimeoutException exception)
				{
					ret.AddReturnedValue(exception, "", "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 252);
					goto end_IL_010e;
				}
				catch (OperationCanceledException exception2)
				{
					ret.AddReturnedValue(exception2, "", "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 253);
					goto end_IL_010e;
				}
				catch (WebException exception3)
				{
					ret.AddReturnedValue(exception3, "", "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 254);
					goto end_IL_010e;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"line:{line}//fileUrl:{fileInfo?.Url}//filePath:{directory}", "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 257);
					ret.AddReturnedValue(ex, "", "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 258);
					goto end_IL_010e;
				}
				return result;
				end_IL_010e:;
			}
			finally
			{
				if (!string.IsNullOrEmpty(dataReadyForDownload?.value) && !ret.HasError)
				{
					ReturnedSaveFuncInfo downloadResult2 = ret;
					downloadResult2.AddReturnedValue(await FileUtility.Instance.WaitForFileAsync(dataReadyForDownload.value, token), ReturnedState.None, "DownloadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 264);
					if (!ret.HasError)
					{
						Task.Run(() => DeleteFileAsync(dataReadyForDownload.value, 10));
					}
				}
			}
			return ret;
		}
	}

	[DebuggerNonUserCode]
	private async Task DeleteFileAsync(string downloadPathTemp, short tryCount = 10)
	{
		try
		{
			if (!string.IsNullOrEmpty(downloadPathTemp) && File.Exists(downloadPathTemp) && !(await FileUtility.Instance.WaitForFileAsync(downloadPathTemp, default(CancellationToken))).HasError)
			{
				File.Delete(downloadPathTemp);
			}
		}
		catch (Exception ex)
		{
			if (tryCount > 0)
			{
				await Task.Delay(10000);
				Downloader downloader = this;
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				await downloader.DeleteFileAsync(downloadPathTemp, num);
			}
			WebErrorLog.Instance.StartLog(ex, "", "DeleteFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Download\\Downloader.cs", 289);
		}
	}
}
