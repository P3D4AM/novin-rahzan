using System;
using System.Reflection;

namespace Ninject.Planning.Targets;

public class PropertyTarget : Target<PropertyInfo>
{
	public override string Name => base.Site.Name;

	public override Type Type => base.Site.PropertyType;

	public PropertyTarget(PropertyInfo site)
		: base((MemberInfo)site, site)
	{
	}
}
