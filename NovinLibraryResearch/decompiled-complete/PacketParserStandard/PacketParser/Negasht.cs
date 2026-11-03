using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser;

public class Negasht
{
	private const string _LocalPath = "d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice";

	private const string _LocalPathNpsms = "D:\\HostingSpaces\\admin\\npsms.ir\\";

	private const string _Url = "http://www.novinp.ir/webservice";

	private const string _UrlNpsms = "http://npsms.ir/";

	private readonly FileManagement_Parts _Part;

	private static ConcurrentDictionary<string, string> dic_WindowsPath = new ConcurrentDictionary<string, string>();

	public string DirName => _Part switch
	{
		FileManagement_Parts.Slider => "Slider", 
		FileManagement_Parts.LogoIconBanner => "Logo_Icon", 
		FileManagement_Parts.PicPrd => "Products", 
		FileManagement_Parts.PicCustomer => "Customers", 
		FileManagement_Parts.PicSign => "Customers_Sign", 
		FileManagement_Parts.Ticket => "Ticket", 
		FileManagement_Parts.Other => "Other", 
		FileManagement_Parts.PicPrdGroup => "ProductsGroup", 
		FileManagement_Parts.NPBackupService => "NPAccBackup", 
		FileManagement_Parts.Temp => "Temp", 
		FileManagement_Parts.Accounting => "Accounting", 
		FileManagement_Parts.Baje => "Baje", 
		FileManagement_Parts.Global => "Global", 
		FileManagement_Parts.TimeAttendance => "Hozor", 
		FileManagement_Parts.ScaleDal => "Sacale", 
		FileManagement_Parts.Stimulsoft2013 => "Stimulsoft2013", 
		FileManagement_Parts.Stimulsoft2018 => "Stimulsoft2018", 
		_ => "unknown", 
	};

	public Negasht(FileManagement_Parts Part)
	{
		_Part = Part;
	}

	private static async Task CreateDirectoryAsync(string path)
	{
		using (new JaegerMonitor("CreateDirectoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs"))
		{
			_ = 3;
			try
			{
				if (!string.IsNullOrEmpty(path))
				{
					DirectoryInfo dirINF = new DirectoryInfo(path);
					if (!Directory.Exists(await GetUrlAsync(dirINF.FullName)))
					{
						await CreateDirectoryAsync(await GetUrlAsync(dirINF.Parent.FullName));
						Directory.CreateDirectory(await GetUrlAsync(dirINF.FullName));
					}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "CreateDirectoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 40);
			}
		}
	}

	public static async Task<string> GetUrlAsync(string WindowsPath)
	{
		try
		{
			if (string.IsNullOrEmpty(WindowsPath))
			{
				return null;
			}
			string text = WindowsPath.ToLower().Replace("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice", "").Replace("\\", "/");
			text = "http://www.NovinP.ir/webservice/" + text;
			CheckAndThrowIfIsNotUrl(text);
			return text;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 58);
			return "";
		}
	}

	public static string GetUrl(string WindowsPath)
	{
		try
		{
			if (string.IsNullOrEmpty(WindowsPath))
			{
				return null;
			}
			string text = WindowsPath.ToLower().Replace("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice", "").Replace("\\", "/");
			text = "http://www.NovinP.ir/webservice" + text;
			CheckAndThrowIfIsNotUrl(text);
			return text;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetUrl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 77);
			return "";
		}
	}

	private static void CheckAndThrowIfIsNotUrl(string path)
	{
		if (!Uri.TryCreate(path, UriKind.Absolute, out Uri result) || (!(result.Scheme == Uri.UriSchemeHttp) && !(result.Scheme == Uri.UriSchemeHttps)))
		{
			throw new ArgumentException("fileName(" + path + ") is not valid url!");
		}
	}

	[Obsolete("Just for old soap services")]
	public static string GetUrl(FileManagement_Parts PartID, short DPTID, string FileName)
	{
		try
		{
			if (string.IsNullOrEmpty(FileName))
			{
				return null;
			}
			string text = (WindowsPath(PartID, DPTID) + "/" + FileName).Replace("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice", "http://www.novinp.ir/webservice").Replace("\\", "/");
			CheckAndThrowIfIsNotUrl(text);
			return text;
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetUrl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 109);
			return "";
		}
	}

	public static async Task<string> GetOldUrlAsync(FileManagement_Parts PartID, short DPTID, string FileName)
	{
		try
		{
			if (string.IsNullOrEmpty(FileName))
			{
				return "";
			}
			string text = (await WindowsPathAsync(PartID, DPTID) + "/" + FileName).Replace("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice", "http://www.novinp.ir/webservice").Replace("\\", "/");
			CheckAndThrowIfIsNotUrl(text);
			return text;
		}
		catch (ArgumentException)
		{
			return "";
		}
		catch (OperationCanceledException)
		{
			return "";
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetOldUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 130);
			return "";
		}
	}

	public static async Task<string> GetNewUrlAsync(FileManagement_Parts PartID, Guid entityGuid, short DPTID, string FileName)
	{
		try
		{
			if (string.IsNullOrEmpty(FileName))
			{
				return null;
			}
			string text = (await WindowsPathAsync(PartID, DPTID) + "/" + FileName).Replace("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice", "http://www.novinp.ir/webservice").Replace("\\", "/");
			CheckAndThrowIfIsNotUrl(text);
			return text;
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetNewUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 151);
			return "";
		}
	}

	public static Task<string> WindowsPathNpsmsAsync(string url)
	{
		try
		{
			if (string.IsNullOrEmpty(url))
			{
				return null;
			}
			string text = url.ToLower().Replace("http://npsms.ir/", "").Replace("/", "\\");
			return Task.FromResult("D:\\HostingSpaces\\admin\\npsms.ir\\" + text);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WindowsPathNpsmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 167);
			return Task.FromResult("");
		}
	}

	public static Task<string> WindowsPathAsync(string url)
	{
		try
		{
			if (string.IsNullOrEmpty(url))
			{
				return null;
			}
			string text = url.ToLower().Replace("http://www.novinp.ir/webservice", "").Replace("/", "\\");
			return Task.FromResult("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice" + text);
		}
		catch (OperationCanceledException)
		{
			return Task.FromResult("");
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WindowsPathAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 183);
			return Task.FromResult("");
		}
	}

	public static string WindowsPath(string url)
	{
		try
		{
			if (string.IsNullOrEmpty(url))
			{
				return null;
			}
			string text = url.ToLower().Replace("http://www.novinp.ir/webservice", "").Replace("/", "\\");
			return "d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice" + text;
		}
		catch (OperationCanceledException)
		{
			return "";
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WindowsPath", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 198);
			return "";
		}
	}

	public static string WindowsPath(FileManagement_Parts PartID, short DPTID)
	{
		try
		{
			string key = $"{DPTID}/{(short)PartID}";
			if (dic_WindowsPath.TryGetValue(key, out var value))
			{
				return value;
			}
			if (DPTID <= 0)
			{
				return "";
			}
			if (!Directory.Exists("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice"))
			{
				Directory.CreateDirectory("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice");
			}
			string text = Path.Combine("d:\\hostingspaces\\admin\\novinp.ir\\wwwroot\\webservice", "Files");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string text2 = Path.Combine(text, DPTID.ToString());
			string text3 = "";
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string dirName = new Negasht(PartID).DirName;
			text3 = Path.Combine(text2, dirName);
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			dic_WindowsPath.TryAdd(key, text3);
			return text3;
		}
		catch (OperationCanceledException)
		{
			Task.FromResult("");
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WindowsPath", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Negasht.cs", 235);
		}
		return "";
	}

	public static Task<string> WindowsPathAsync(FileManagement_Parts PartID, short DPTID)
	{
		return Task.FromResult(WindowsPath(PartID, DPTID));
	}
}
