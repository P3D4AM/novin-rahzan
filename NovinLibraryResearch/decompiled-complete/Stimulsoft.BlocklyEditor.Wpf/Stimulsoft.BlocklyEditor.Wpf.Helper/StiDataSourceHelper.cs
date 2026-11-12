using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.BlocklyEditor.Wpf.Helper;

public static class StiDataSourceHelper
{
	public static List<string> LoadDataSource(StiReport report)
	{
		return (from v in report.Dictionary.DataSources.ToList()
			select v.Name).ToList();
	}

	public static List<string> GetDataSourceProperties()
	{
		return new List<string> { "Count", "IsBof", "IsConnected", "IsEmpty", "IsEof", "Position" };
	}

	public static List<string> GetDataSourceMethods()
	{
		return new List<string> { "Connect", "Disconnect", "First", "Last", "Next", "Prior" };
	}
}
