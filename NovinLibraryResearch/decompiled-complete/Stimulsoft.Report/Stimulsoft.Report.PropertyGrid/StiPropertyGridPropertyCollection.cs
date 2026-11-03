namespace Stimulsoft.Report.PropertyGrid;

public sealed class StiPropertyGridPropertyCollection
{
	private int pos;

	private StiPropertyCategories[] categories;

	private StiPropertyObject[][] properties;

	public void Add(StiPropertyCategories cat, StiPropertyObject[] objs)
	{
		categories[pos] = cat;
		properties[pos] = objs;
		pos++;
	}

	public StiPropertyGridPropertyCollection(int size)
	{
		categories = new StiPropertyCategories[size];
		properties = new StiPropertyObject[size][];
	}
}
