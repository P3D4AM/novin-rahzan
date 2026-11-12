using System;
using System.Reflection;

namespace Ninject.Planning.Targets;

public class ParameterTarget : Target<ParameterInfo>
{
	public override string Name => base.Site.Name;

	public override Type Type => base.Site.ParameterType;

	public override bool HasDefaultValue => base.Site.HasDefaultValue;

	public override object DefaultValue
	{
		get
		{
			if (!HasDefaultValue)
			{
				return base.DefaultValue;
			}
			return base.Site.DefaultValue;
		}
	}

	public ParameterTarget(MethodBase method, ParameterInfo site)
		: base((MemberInfo)method, site)
	{
	}
}
