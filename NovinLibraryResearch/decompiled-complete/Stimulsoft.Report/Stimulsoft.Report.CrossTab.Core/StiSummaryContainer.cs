using System.Collections;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiSummaryContainer
{
	private int level;

	private Hashtable dataCol = new Hashtable();

	internal StiSummary GetSummary(StiColumn col, StiRow row)
	{
		return GetSummary(col, row, create: true);
	}

	public StiSummary GetSummary(StiColumn col, StiRow row, bool create)
	{
		Hashtable hashtable = dataCol[col] as Hashtable;
		if (hashtable == null)
		{
			if (!create)
			{
				return null;
			}
			dataCol[col] = new Hashtable();
			hashtable = dataCol[col] as Hashtable;
		}
		StiSummary stiSummary = hashtable[row] as StiSummary;
		if (stiSummary == null)
		{
			if (!create)
			{
				return null;
			}
			stiSummary = (StiSummary)(hashtable[row] = new StiSummary(level));
		}
		return stiSummary;
	}

	public Hashtable GetDataCol()
	{
		return dataCol;
	}

	public StiSummaryContainer(int level)
	{
		this.level = level;
	}
}
