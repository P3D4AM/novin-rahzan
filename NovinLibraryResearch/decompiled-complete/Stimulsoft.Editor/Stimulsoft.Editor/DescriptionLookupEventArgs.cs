using System;

namespace Stimulsoft.Editor;

public class DescriptionLookupEventArgs : EventArgs
{
	public object Member;

	public string Name;

	public string Description;

	public DescriptionLookupEventArgs(object member, string name)
	{
		Member = member;
		Name = name;
	}
}
