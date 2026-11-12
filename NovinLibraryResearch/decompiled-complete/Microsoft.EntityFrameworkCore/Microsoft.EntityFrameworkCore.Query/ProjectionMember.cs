using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Query;

[DebuggerDisplay("{ToString(), nq}")]
public class ProjectionMember
{
	private readonly IList<MemberInfo> _memberChain;

	public virtual MemberInfo Last => _memberChain.LastOrDefault();

	public ProjectionMember()
	{
		_memberChain = new List<MemberInfo>();
	}

	private ProjectionMember(IList<MemberInfo> memberChain)
	{
		_memberChain = memberChain;
	}

	public virtual ProjectionMember Append(MemberInfo member)
	{
		List<MemberInfo> list = _memberChain.ToList();
		list.Add(member);
		return new ProjectionMember(list);
	}

	public virtual ProjectionMember Prepend(MemberInfo member)
	{
		List<MemberInfo> list = _memberChain.ToList();
		list.Insert(0, member);
		return new ProjectionMember(list);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		for (int i = 0; i < _memberChain.Count; i++)
		{
			hashCode.Add(_memberChain[i]);
		}
		return hashCode.ToHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (obj is ProjectionMember other)
			{
				return Equals(other);
			}
			return false;
		}
		return false;
	}

	private bool Equals(ProjectionMember other)
	{
		if (_memberChain.Count != other._memberChain.Count)
		{
			return false;
		}
		for (int i = 0; i < _memberChain.Count; i++)
		{
			if (!object.Equals(_memberChain[i], other._memberChain[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		if (!_memberChain.Any())
		{
			return "EmptyProjectionMember";
		}
		return string.Join(".", _memberChain.Select((MemberInfo mi) => mi.Name));
	}
}
