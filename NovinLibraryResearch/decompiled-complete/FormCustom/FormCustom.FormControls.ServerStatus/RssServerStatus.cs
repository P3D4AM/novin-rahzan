using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.FormControls.ServerStatus;

public class RssServerStatus
{
	public string title { get; set; }

	public string link { get; set; }

	public string description { get; set; }

	public DateTime pubDate { get; set; }

	public int Duration { get; set; }

	public bool IsUptime => description.ToLower().Contains("successful");

	public static async Task<ReturnedSaveFuncInfoWithValue<List<RssServerStatus>>> ReadRssContent(string url, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<RssServerStatus>> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<List<RssServerStatus>>
		{
			value = new List<RssServerStatus>()
		};
		try
		{
			using WebClient webClient = new WebClient();
			XDocument val = XDocument.Parse(webClient.DownloadString(url));
			if (token.IsCancellationRequested)
			{
				return returnedSaveFuncInfoWithValue.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ReadRssContent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\RssServerStatus.cs", 39);
			}
			foreach (XElement item in ((XContainer)val).Descendants(XName.op_Implicit("item")).ToList())
			{
				try
				{
					RssServerStatus rss = new RssServerStatus();
					rss.title = ((XContainer)item).Element(XName.op_Implicit("title")).Value ?? "";
					rss.link = ((XContainer)item).Element(XName.op_Implicit("link")).Value ?? "";
					rss.description = ((XContainer)item).Element(XName.op_Implicit("description")).Value ?? "";
					rss.pubDate = DateTime.Parse(((XContainer)item).Element(XName.op_Implicit("pubDate")).Value);
					if (returnedSaveFuncInfoWithValue.value.Count((RssServerStatus p) => p.link == rss.link) <= 0)
					{
						returnedSaveFuncInfoWithValue.value.Add(rss);
					}
				}
				catch (WebException exception)
				{
					returnedSaveFuncInfoWithValue.AddReturnedValue(exception, "", "ReadRssContent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\RssServerStatus.cs", 55);
				}
				catch (Exception ex)
				{
					IEnumerable<string> values = ((item == null) ? null : ((XContainer)item).Elements()?.Select((XElement p) => $"elementName:{((p != null) ? p.Name : null)} value is {((p != null) ? p.Value : null)}"));
					WebErrorLog.Instance.StartLog(ex, string.Format("nodeName:{0}//{1}", (item != null) ? item.Name : null, string.Join("**", values)), "ReadRssContent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\RssServerStatus.cs", 60);
				}
			}
		}
		catch (WebException exception2)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(exception2, "", "ReadRssContent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\RssServerStatus.cs", 66);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "333", "ReadRssContent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\RssServerStatus.cs", 70);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex2, "", "ReadRssContent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\RssServerStatus.cs", 71);
		}
		return returnedSaveFuncInfoWithValue;
	}
}
