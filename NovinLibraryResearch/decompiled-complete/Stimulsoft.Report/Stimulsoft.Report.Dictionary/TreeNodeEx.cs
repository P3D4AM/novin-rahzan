using System.Collections.Generic;
using System.Windows.Forms;

namespace Stimulsoft.Report.Dictionary;

public class TreeNodeEx : TreeNode
{
	private int level;

	private List<StiDataSource> datas;

	internal void FillFromNodeEx(ref StiDataSourcesCollection datas, ref int level)
	{
		if (this.datas != null && this.datas.Count > 0)
		{
			foreach (StiDataSource data in this.datas)
			{
				datas.Add(data);
			}
		}
		level = this.level;
	}

	internal void FillNodeEx(StiDataSourcesCollection datas, int level)
	{
		this.level = level;
		if (datas.Count > 0)
		{
			this.datas = new List<StiDataSource>(datas.Count);
			{
				foreach (StiDataSource data in datas)
				{
					this.datas.Add(data);
				}
				return;
			}
		}
		this.datas = null;
	}

	public TreeNodeEx()
	{
	}

	public TreeNodeEx(string text)
		: base(text)
	{
	}
}
