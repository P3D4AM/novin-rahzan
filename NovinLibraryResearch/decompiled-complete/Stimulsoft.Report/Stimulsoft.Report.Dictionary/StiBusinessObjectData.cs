namespace Stimulsoft.Report.Dictionary;

public class StiBusinessObjectData
{
	public string Category { get; set; }

	public string Name { get; set; }

	public string Alias { get; set; }

	public object BusinessObjectValue { get; set; }

	public StiBusinessObjectData(string category, string name, object value)
		: this(category, name, name, value)
	{
	}

	public StiBusinessObjectData(string category, string name, string alias, object value)
	{
		Category = category;
		Name = name;
		Alias = alias;
		BusinessObjectValue = value;
	}
}
