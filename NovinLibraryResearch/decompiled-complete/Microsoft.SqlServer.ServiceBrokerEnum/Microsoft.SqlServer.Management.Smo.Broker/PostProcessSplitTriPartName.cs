using System.Collections.Specialized;

namespace Microsoft.SqlServer.Management.Smo.Broker;

internal class PostProcessSplitTriPartName : PostProcess
{
	private StringCollection m_listNames;

	public override object GetColumnData(string name, object data, DataProvider dp)
	{
		if (m_listNames == null)
		{
			string triggeredString = GetTriggeredString(dp, 0);
			if (triggeredString == null)
			{
				return data;
			}
			m_listNames = Util.SplitNames(triggeredString);
		}
		int num = 0;
		if (!(name == "ProcedureSchema"))
		{
			if (name == "ProcedureDatabase")
			{
				num = 2;
			}
		}
		else
		{
			num = 1;
		}
		if (num >= m_listNames.Count)
		{
			return data;
		}
		return m_listNames[num];
	}

	public override void CleanRowData()
	{
		m_listNames = null;
	}
}
