using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.IO;

public class CsvFile
{
	public static ReturnedSaveFuncInfoWithValue<List<string>> Read(string file_name)
	{
		ReturnedSaveFuncInfoWithValue<List<string>> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<List<string>>();
		returnedSaveFuncInfoWithValue.value = new List<string>();
		try
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(Exsist(file_name), ReturnedState.None, "Read", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 21);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return returnedSaveFuncInfoWithValue;
			}
			returnedSaveFuncInfoWithValue.value = File.ReadAllLines(file_name)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Read", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 27);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "Read", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 28);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static async Task<ReturnedSaveFuncInfo> WriteAsync(string fileName, List<string> lst)
	{
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(Exsist(fileName), ReturnedState.None, "WriteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 38);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return returnedSaveFuncInfoWithValue;
			}
			File.WriteAllLines(fileName, lst, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 44);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "WriteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 45);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static async Task<ReturnedSaveFuncInfo> AppendAsync(string fileName, List<string> lst)
	{
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(Exsist(fileName), ReturnedState.None, "AppendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 55);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return returnedSaveFuncInfoWithValue;
			}
			StreamWriter streamWriter = new StreamWriter(new FileStream(fileName, FileMode.Append), Encoding.UTF8);
			foreach (string item in lst)
			{
				streamWriter.WriteLine(item);
			}
			streamWriter.Flush();
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AppendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 69);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "AppendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 70);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static async Task<ReturnedSaveFuncInfo> CrerateNewAsync(string fileName, List<string> lst)
	{
		if (File.Exists(fileName))
		{
			throw new Exception(fileName + " already exists. cannot createNew");
		}
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			using StreamWriter streamWriter = new StreamWriter(new FileStream(fileName, FileMode.CreateNew), Encoding.UTF8);
			foreach (string item in lst)
			{
				streamWriter.WriteLine(item);
			}
			streamWriter.Flush();
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CrerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 90);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "CrerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 91);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo Exsist(string fileName)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (!File.Exists(fileName))
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "File(" + fileName + ") Not Found", "fileName", "Exsist", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 101);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Exsist", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 105);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "Exsist", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 106);
		}
		return returnedSaveFuncInfo;
	}

	public static async Task<ReturnedSaveFuncInfo> Delete(string fileName)
	{
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			else
			{
				returnedSaveFuncInfoWithValue.AddError(EnSystem.None, "File Not Found", "Delete", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 123);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Delete", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 128);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "Delete", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FileUititlity\\CsvFile.cs", 129);
		}
		return returnedSaveFuncInfoWithValue;
	}
}
