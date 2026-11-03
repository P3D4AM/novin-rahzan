using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser;

public static class Json
{
	private static JsonSerializerSettings indentedOptions;

	public static JsonSerializerSettings DefaultOptions { get; private set; }

	static Json()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		DefaultOptions = new JsonSerializerSettings
		{
			DefaultValueHandling = (DefaultValueHandling)1
		};
		indentedOptions = new JsonSerializerSettings
		{
			DefaultValueHandling = (DefaultValueHandling)1,
			Formatting = (Formatting)1,
			ContractResolver = DefaultOptions.ContractResolver
		};
	}

	public static string ToStringJson(object item, bool indented = false)
	{
		try
		{
			if (item == null)
			{
				return "";
			}
			JsonSerializerSettings val = (indented ? indentedOptions : DefaultOptions);
			val.MaxDepth = 10;
			val.ReferenceLoopHandling = (ReferenceLoopHandling)1;
			return JsonConvert.SerializeObject(item, val);
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
			WebErrorLog.Instance.StartLog(ex3, "ItemType:" + item.GetType(), "ToStringJson", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Json.cs", 47);
			throw;
		}
	}

	public static string ToStringJson(object item, JsonSerializerSettings option)
	{
		try
		{
			if (item == null)
			{
				return "";
			}
			if (option == null)
			{
				option = DefaultOptions;
			}
			return JsonConvert.SerializeObject(item, option);
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
			WebErrorLog.Instance.StartLog(ex3, "ItemType:" + item.GetType().ToString(), "ToStringJson", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Json.cs", 64);
		}
		return "";
	}

	public static byte[] ToBytesJson(object item)
	{
		try
		{
			if (item == null)
			{
				return null;
			}
			string s = JsonConvert.SerializeObject(item, DefaultOptions);
			return Encoding.UTF8.GetBytes(s);
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
			WebErrorLog.Instance.StartLog(ex3, "", "ToBytesJson", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Json.cs", 89);
			return null;
		}
	}

	public static T FromJson<T>(this string data, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		return data.FromJson<T>(sendErrorIfException: true, memberName, sourceFilePath, sourceLineNumber);
	}

	public static T FromJson<T>(this string data, bool sendErrorIfException, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		return data.FromJson<T>(sendErrorIfException, null, memberName, sourceFilePath, sourceLineNumber);
	}

	public static T FromJson<T>(this string data, bool sendErrorIfException, JsonSerializerSettings options, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			string text = ",\"taxes\":{\"total\":[],\"subtotal\":[]}";
			if (data == null)
			{
				data = "";
			}
			if (data.Contains(text))
			{
				data = data.Replace(text, "");
			}
			if (string.IsNullOrEmpty(data) || data.Trim() == "" || data.StartsWith("<!DOCTYPE html>"))
			{
				return default(T);
			}
			if (options == null)
			{
				options = DefaultOptions;
			}
			T val = JsonConvert.DeserializeObject<T>(data, options);
			if (val != null)
			{
				return val;
			}
			throw new NullReferenceException();
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (sendErrorIfException)
			{
				WebErrorLog.Instance.StartLog(ex2, "from string\r\nmemberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}\r\n" + "//data:" + data, "FromJson", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Json.cs", 158);
				throw;
			}
			return default(T);
		}
	}

	public static T FromJson<T>(string data, JsonSerializerSettings options, bool sendErrorIfException = true, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (string.IsNullOrEmpty(data) || data.Trim() == "")
			{
				return default(T);
			}
			if (options == null)
			{
				options = DefaultOptions;
			}
			return JsonConvert.DeserializeObject<T>(data, options);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (sendErrorIfException)
			{
				WebErrorLog.Instance.StartLog(ex2, "string+converter\r\nmemberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}\r\n" + "//data:" + data, "FromJson", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Json.cs", 190);
				throw;
			}
			return default(T);
		}
	}

	public static T FromJson<T>(byte[] bytes, bool sendErrorIfException = true, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (bytes != null)
			{
				return Encoding.UTF8.GetString(bytes, 0, bytes.Length).FromJson<T>(sendErrorIfException, "FromJson", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Json.cs", 212);
			}
			return default(T);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (sendErrorIfException)
			{
				WebErrorLog.Instance.StartLog(ex2, "from bytes\r\nmemberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "FromJson", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Json.cs", 221);
				throw;
			}
			return default(T);
		}
	}
}
