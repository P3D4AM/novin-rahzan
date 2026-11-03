using System;

namespace Stimulsoft.Report.Dictionary;

[Serializable]
public class StiData
{
	private string name;

	public object ViewData { get; set; }

	public object Data { get; set; }

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			if (!(name == value))
			{
				if (Alias == name)
				{
					Alias = value;
				}
				name = value;
			}
		}
	}

	public string Alias { get; set; }

	public bool IsReportData { get; set; }

	public bool IsBusinessObjectData { get; set; }

	internal object OriginalConnectionState { get; set; }

	public override string ToString()
	{
		if (!(Name == Alias))
		{
			return Name + "(" + Alias + ")";
		}
		return Name + "(" + ViewData.GetType().Name + ")";
	}

	public StiData(string name, object data)
		: this(name, data, data)
	{
	}

	public StiData(string name, object data, object viewData)
	{
		this.name = name;
		Data = data;
		ViewData = viewData;
	}
}
