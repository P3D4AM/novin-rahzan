namespace Stimulsoft.Editor;

internal class MemberItem
{
	public string String;

	public int Index;

	public MemberItem(string s, int index)
	{
		String = s;
		Index = index;
	}

	public override string ToString()
	{
		return String;
	}
}
