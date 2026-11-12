namespace Stimulsoft.Editor;

internal class ListboxItem
{
	public string String;

	public int Index;

	public ListboxItem(string s, int index)
	{
		String = s;
		Index = index;
	}

	public override string ToString()
	{
		return String;
	}
}
