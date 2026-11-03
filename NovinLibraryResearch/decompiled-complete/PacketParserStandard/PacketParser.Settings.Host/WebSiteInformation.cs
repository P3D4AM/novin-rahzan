using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Settings.Host;

public class WebSiteInformation
{
	private static List<WebSiteInformation> Catched = null;

	public string UserName { get; set; }

	public string Password { get; set; }

	public string Url { get; set; }

	public ENSource Source { get; set; } = ENSource.WebSite;


	public string SignalRUrl { get; set; } = "https://novinrank.ir/hubCore";


	public static string FileName { get; private set; } = "SiteConnectionConfig.txt";


	public static string PathServer { get; set; }

	public bool DataValid()
	{
		try
		{
			return !string.IsNullOrEmpty(UserName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DataValid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations.cs", 32);
			return false;
		}
	}

	public static List<WebSiteInformation> Get(string Path)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations.cs"))
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
					Catched = new List<WebSiteInformation>();
					return Catched;
				}
				string text = File.ReadAllText(Path).ToLower().Replace("https://webservice.novinp.ir/hub", "http://novinrank.ir:8040/hubCore")
					.Replace("https://webservice.novinp.ir/hub", "https://novinrank.ir/hubCore");
				if (string.IsNullOrEmpty(text))
				{
					Catched = new List<WebSiteInformation>();
					return Catched;
				}
				Catched = text.FromJson<List<WebSiteInformation>>("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations.cs", 55);
				if (Catched == null)
				{
					Catched = new List<WebSiteInformation>();
				}
				if (Catched != null && Catched.Any())
				{
					foreach (WebSiteInformation item in Catched)
					{
						item?.Url.ToLower().Replace("webservice.novinp.ir", "novinrank.ir");
					}
				}
				return Catched;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations.cs", 66);
				return new List<WebSiteInformation>();
			}
		}
	}

	public static void Save(string Path, List<WebSiteInformation> lst)
	{
		using (new JaegerMonitor("Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations.cs"))
		{
			try
			{
				Catched = lst;
				File.WriteAllText(Path, Json.ToStringJson(lst, indented: true));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Host\\WebSiteInformations.cs", 80);
			}
		}
	}
}
