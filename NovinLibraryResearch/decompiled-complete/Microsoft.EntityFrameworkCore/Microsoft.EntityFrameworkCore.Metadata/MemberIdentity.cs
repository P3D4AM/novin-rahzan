using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public readonly struct MemberIdentity
{
	private readonly object _nameOrMember;

	public static readonly MemberIdentity None = new MemberIdentity((object)null);

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return MemberInfo?.GetSimpleMemberName() ?? ((string)_nameOrMember);
		}
	}

	public MemberInfo MemberInfo
	{
		[DebuggerStepThrough]
		get
		{
			return _nameOrMember as MemberInfo;
		}
	}

	[DebuggerStepThrough]
	public MemberIdentity([JetBrains.Annotations.NotNull] string name)
		: this((object)name)
	{
	}

	[DebuggerStepThrough]
	public MemberIdentity([JetBrains.Annotations.NotNull] MemberInfo memberInfo)
		: this((object)memberInfo)
	{
	}

	[DebuggerStepThrough]
	private MemberIdentity([JetBrains.Annotations.CanBeNull] object nameOrMember)
	{
		_nameOrMember = nameOrMember;
	}

	public bool IsNone()
	{
		return _nameOrMember == null;
	}

	[DebuggerStepThrough]
	public static MemberIdentity Create([JetBrains.Annotations.CanBeNull] string name)
	{
		if (name != null)
		{
			return new MemberIdentity(name);
		}
		return None;
	}

	[DebuggerStepThrough]
	public static MemberIdentity Create([JetBrains.Annotations.CanBeNull] MemberInfo member)
	{
		if (!(member == null))
		{
			return new MemberIdentity(member);
		}
		return None;
	}

	private string DebuggerDisplay()
	{
		return Name ?? "NONE";
	}
}
