using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report;

public class StiEditableItem
{
	[StiSerializable]
	public int PageIndex { get; set; } = -1;


	[StiSerializable]
	public int Position { get; set; } = -1;


	[StiSerializable]
	public string ComponentName { get; set; }

	[StiSerializable]
	public string TextValue { get; set; }

	public StiEditableItem()
	{
	}

	public StiEditableItem(int pageIndex, int position, string componentName, string textValue)
	{
		PageIndex = pageIndex;
		Position = position;
		ComponentName = componentName;
		TextValue = textValue;
	}
}
