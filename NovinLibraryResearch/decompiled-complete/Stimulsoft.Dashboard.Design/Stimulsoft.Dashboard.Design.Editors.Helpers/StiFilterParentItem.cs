using Stimulsoft.Dashboard.Components;

namespace Stimulsoft.Dashboard.Design.Editors.Helpers;

public class StiFilterParentItem
{
	public StiElement Element { get; set; }

	public override string ToString()
	{
		if (Element == null)
		{
			return "";
		}
		return Element.Name;
	}
}
