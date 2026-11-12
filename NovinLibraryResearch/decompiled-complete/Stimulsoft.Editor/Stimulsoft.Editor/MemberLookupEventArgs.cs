using System;

namespace Stimulsoft.Editor;

public class MemberLookupEventArgs : EventArgs
{
	public object Member;

	public string Name;

	public object Result;

	public StaticScope Scope;

	public MemberLookupEventArgs(object member, string name)
	{
		Member = member;
		Name = name;
	}
}
