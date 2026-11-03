using System;
using System.IO;
using PacketParser.Entities;

namespace PacketParser.Settings.Host;

public class WebSiteInformations_old : Serializable<WebSiteInformations_old>
{
	private static WebSiteInformations_old Catched = null;

	public string UserName { get; set; }

	public string Password { get; set; }

	public string Url { get; set; }

	public ENSource Source { get; set; } = ENSource.WebSite;


	public string SignalRUrl { get; set; } = "https://novinrank.ir/hubCore";


	public static string FileName { get; private set; } = "Site.txt";


	public static string PathServer { get; set; }

	private WebSiteInformations_old()
	{
	}

	public bool DataValid()
	{
		try
		{
			return !string.IsNullOrEmpty(UserName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DataValid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations_old.cs", 30);
			return false;
		}
	}

	public static WebSiteInformations_old Get(string Path)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations_old.cs"))
		{
			try
			{
				PathServer = Path.Replace(FileName, "");
				if (Catched != null)
				{
					return Catched;
				}
				if (!File.Exists(Path))
				{
					Catched = new WebSiteInformations_old();
					return Catched;
				}
				string text = File.ReadAllText(Path);
				if (string.IsNullOrEmpty(text))
				{
					Catched = new WebSiteInformations_old();
					return Catched;
				}
				Catched = Serializable<WebSiteInformations_old>.FromString(text);
				if (Catched == null)
				{
					Catched = new WebSiteInformations_old();
				}
				return Catched;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations_old.cs", 60);
				return new WebSiteInformations_old();
			}
		}
	}

	public void Save(string Path)
	{
		using (new JaegerMonitor("Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations_old.cs"))
		{
			try
			{
				Catched = this;
				File.WriteAllText(Path, ToString(isIndented: true));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations_old.cs", 74);
			}
		}
	}
}
