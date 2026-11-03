using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Files;

public class CsvManager
{
	public static async Task<ReturnedSaveFuncInfoWithValue<List<string>>> ReadFromCvs(string file_name)
	{
		List<string> list = new List<string>();
		ReturnedSaveFuncInfoWithValue<List<string>> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<List<string>>();
		try
		{
			if (File.Exists(file_name))
			{
				using StreamReader streamReader = new StreamReader(file_name);
				string item;
				while ((item = streamReader.ReadLine()) != null)
				{
					list.Add(item);
				}
				returnedSaveFuncInfoWithValue.value = list;
				streamReader.Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadFromCvs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\CsvManager.cs", 31);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ReadFromCvs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\CsvManager.cs", 32);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static async Task<ReturnedSaveFuncInfo> WritetoCvs(string file_name, List<string> lst)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			StreamWriter streamWriter = new StreamWriter(new FileStream(file_name, FileMode.OpenOrCreate));
			foreach (string item in lst)
			{
				streamWriter.WriteLine(item);
			}
			streamWriter.Flush();
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WritetoCvs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\CsvManager.cs", 55);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "WritetoCvs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\CsvManager.cs", 56);
		}
		return returnedSaveFuncInfo;
	}
}
