namespace DevComponents.DotNetBar.Design;

public class FlagCheckedListBoxItem
{
	public int value;

	public string caption;

	public bool IsFlag => (value & (value - 1)) == 0;

	public FlagCheckedListBoxItem(int v, string c)
	{
		value = v;
		caption = c;
	}

	public override string ToString()
	{
		return caption;
	}

	public bool IsMemberFlag(FlagCheckedListBoxItem composite)
	{
		if (IsFlag)
		{
			return (value & composite.value) == value;
		}
		return false;
	}
}
