using System;
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;

namespace Ninject.Selection.Heuristics;

public class StandardInjectionHeuristic : NinjectComponent, IInjectionHeuristic, INinjectComponent, IDisposable
{
	public virtual bool ShouldInject(MemberInfo member)
	{
		Ensure.ArgumentNotNull(member, "member");
		if (member is PropertyInfo propertyInfo)
		{
			bool injectNonPublic = base.Settings.InjectNonPublic;
			MethodInfo setMethod = propertyInfo.GetSetMethod(injectNonPublic);
			if (member.HasAttribute(base.Settings.InjectAttribute))
			{
				return setMethod != null;
			}
			return false;
		}
		return member.HasAttribute(base.Settings.InjectAttribute);
	}
}
