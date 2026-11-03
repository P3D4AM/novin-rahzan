using System;
using System.IO;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace BackUpDLL.Zip;

public class MasterZip
{
	public async Task<ReturnedSaveFuncInfoWithValue<string>> ExtractTempDirAsync(string pathf, string startup)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			new ReturnedSaveFuncInfoWithValue<string>();
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new GZip().ExtractTempDIR(pathf, startup);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "ExtractTempDirAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\MasterZip.cs", 35);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "ExtractTempDirAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\MasterZip.cs", 41);
			WebErrorLog.Instance.StartLog(ex, "", "ExtractTempDirAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\MasterZip.cs", 42);
		}
		return ret;
	}

	public static ReturnedSaveFuncInfoWithValue<string> Move2Temp(string filePath, string startup, short tryCount = 10)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		string text = "";
		int num = 0;
		try
		{
			string text2 = TempDirName(startup);
			num = 1;
			FileInfo fileInfo = new FileInfo(filePath);
			num = 2;
			if (!File.Exists(filePath))
			{
				num = 3;
				returnedSaveFuncInfoWithValue.AddReturnedValue(ReturnedState.Error, "path not exists : " + filePath, "", "Move2Temp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\MasterZip.cs", 67);
				num = 4;
				return returnedSaveFuncInfoWithValue;
			}
			text = Path.Combine(text2, fileInfo.Name);
			num = 5;
			File.Move(filePath, text);
			num = 6;
			returnedSaveFuncInfoWithValue.value = text2;
		}
		catch (Exception ex)
		{
			tryCount--;
			if (tryCount > 0)
			{
				Move2Temp(filePath, startup, tryCount);
				return returnedSaveFuncInfoWithValue;
			}
			string ex2 = $"line={num}, SourceFileName:{filePath}, startup:{startup},DestFilePath{text}\r\n{ex}";
			WebErrorLog.Instance.StartLog(ex2, "", "Move2Temp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\MasterZip.cs", 86);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "Move2Temp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\MasterZip.cs", 87);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static string TempDirName(string startup)
	{
		int num = 0;
		try
		{
			num = 1;
			string text = Path.Combine(startup, "Temp");
			num = 2;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			num = 3;
			text = Path.Combine(text, Guid.NewGuid().ToString());
			num = 4;
			Directory.CreateDirectory(text);
			num = 5;
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "TempDirName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\MasterZip.cs", 112);
			return "";
		}
	}
}
