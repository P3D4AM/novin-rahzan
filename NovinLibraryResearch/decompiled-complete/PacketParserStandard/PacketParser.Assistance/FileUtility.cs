using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public class FileUtility
{
	public static FileUtility Instance { get; private set; } = new FileUtility();


	public static ReturnedSaveFuncInfoWithValue<string> CreateFile(string basePath, string fileName, byte[] fileContent)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			if (!Directory.Exists(basePath))
			{
				CreateDirectory(basePath);
			}
			if (!File.Exists(Path.Combine(basePath, fileName)))
			{
				File.WriteAllBytes(Path.Combine(basePath, fileName), fileContent);
				returnedSaveFuncInfoWithValue.value = Path.Combine(basePath, fileName);
				return returnedSaveFuncInfoWithValue;
			}
			for (int i = 0; i < 1000; i = checked(i + 1))
			{
				string text = Path.Combine(basePath, i.ToString());
				if (!Directory.Exists(text))
				{
					return CreateFile(text, fileName, fileContent);
				}
				if (!File.Exists(Path.Combine(text, fileName)))
				{
					return CreateFile(text, fileName, fileContent);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 37);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "CreateFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 38);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo CreateDirectory(string path, bool sendErrorToServer = true)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (directoryInfo.Exists)
			{
				return returnedSaveFuncInfo;
			}
			if (!string.IsNullOrEmpty(directoryInfo.Parent.FullName))
			{
				returnedSaveFuncInfo.AddReturnedValue(CreateDirectory(directoryInfo.Parent.FullName, sendErrorToServer), ReturnedState.None, "CreateDirectory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 51);
			}
			if (!Directory.Exists(path) && !Directory.CreateDirectory(path).Exists)
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "Directory Not Create", "CreateDirectory", "CreateDirectory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 55);
			}
		}
		catch (Exception ex)
		{
			if (sendErrorToServer)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CreateDirectory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 60);
			}
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CreateDirectory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 61);
		}
		return returnedSaveFuncInfo;
	}

	public async Task<ReturnedSaveFuncInfo> FixeFileAccessAsync(string fileName, short tryCount = 10)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (!File.Exists(fileName))
			{
				return returnedSaveFuncInfo.AddWarning("444File not exists:'" + fileName + "'", "fileName", "FixeFileAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 70);
			}
			FileAttributes fileAttributes = File.GetAttributes(fileName);
			bool flag = false;
			if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				flag = true;
				fileAttributes &= ~FileAttributes.ReadOnly;
				File.SetAttributes(fileName, fileAttributes);
			}
			if (flag)
			{
				File.GetAttributes(fileName);
				if ((fileAttributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
				{
					returnedSaveFuncInfo.AddInformation("Ha Ha Ha Ha !!!! File Attribute readOnly changed from true to false value!", "", "FixeFileAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 86);
					WebErrorLog.Instance.StartLog("Ha Ha Ha Ha !!!! File Attribute readOnly changed from true to false value!", "", "FixeFileAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 87);
				}
			}
		}
		catch (Exception ex)
		{
			if (tryCount > 0)
			{
				await Task.Delay(5000);
				FileUtility fileUtility = this;
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await fileUtility.FixeFileAccessAsync(fileName, num);
			}
			WebErrorLog.Instance.StartLog(ex, "", "FixeFileAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 98);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "FixeFileAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 99);
		}
		return returnedSaveFuncInfo;
	}

	[DebuggerNonUserCode]
	public async Task<ReturnedSaveFuncInfo> IsFileReadyAsync(string filename, short tryCount = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			await Task.Delay(100);
			using FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			if (fileStream.Length <= 0)
			{
				ret.AddError(EnSystem.None, "file open as read returned no data!", "filename", "IsFileReadyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 118);
			}
			fileStream?.Close();
		}
		catch (Exception ex)
		{
			await FixeFileAccessAsync(filename, 10);
			if (tryCount > 0)
			{
				await Task.Delay(5000);
				FileUtility fileUtility = this;
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await fileUtility.IsFileReadyAsync(filename, num);
			}
			WebErrorLog.Instance.StartLog(ex, "", "IsFileReadyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 130);
			ret.AddReturnedValue(ex, "", "IsFileReadyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 131);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> WaitForFileAsync(string filename, CancellationToken token, int timeOutSeconds = 3000)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		DateTime timeOut = DateTime.Now.AddSeconds(timeOutSeconds);
		int line = 0;
		try
		{
			while (!token.IsCancellationRequested)
			{
				line = 1;
				if (!File.Exists(filename))
				{
					return ret;
				}
				line = 2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await IsFileReadyAsync(filename, 10);
				line = 3;
				if (DateTime.Now > timeOut)
				{
					line = 4;
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "WaitForFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 152);
					line = 5;
					return ret.AddError(EnSystem.None, "Timeout", "filename", "WaitForFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 154);
				}
				line = 6;
				if (!returnedSaveFuncInfo.HasError)
				{
					line = 7;
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "WaitForFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 160);
					line = 8;
					return ret;
				}
				line = 9;
				await Task.Delay(1000);
				line = 11;
				if (token.IsCancellationRequested)
				{
					line = 12;
					return ret.AddErrorTokenIsCancelled("", "", "WaitForFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 170);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "WaitForFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 176);
			ret.AddReturnedValue(ex, "", "WaitForFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\FileUtility.cs", 177);
		}
		return ret;
	}
}
