using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Selection.Heuristics;

namespace Ninject.Selection;

public class Selector : NinjectComponent, ISelector, INinjectComponent, IDisposable
{
	private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

	public IConstructorScorer ConstructorScorer { get; private set; }

	public ICollection<IInjectionHeuristic> InjectionHeuristics { get; private set; }

	protected virtual BindingFlags Flags
	{
		get
		{
			if (!base.Settings.InjectNonPublic)
			{
				return BindingFlags.Instance | BindingFlags.Public;
			}
			return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		}
	}

	public Selector(IConstructorScorer constructorScorer, IEnumerable<IInjectionHeuristic> injectionHeuristics)
	{
		Ensure.ArgumentNotNull(constructorScorer, "constructorScorer");
		Ensure.ArgumentNotNull(injectionHeuristics, "injectionHeuristics");
		ConstructorScorer = constructorScorer;
		InjectionHeuristics = injectionHeuristics.ToList();
	}

	public virtual IEnumerable<ConstructorInfo> SelectConstructorsForInjection(Type type)
	{
		Ensure.ArgumentNotNull(type, "type");
		if (type.IsSubclassOf(typeof(MulticastDelegate)))
		{
			return null;
		}
		ConstructorInfo[] constructors = type.GetConstructors(Flags);
		if (constructors.Length != 0)
		{
			return constructors;
		}
		return null;
	}

	public virtual IEnumerable<PropertyInfo> SelectPropertiesForInjection(Type type)
	{
		Ensure.ArgumentNotNull(type, "type");
		List<PropertyInfo> list = new List<PropertyInfo>();
		list.AddRange(from p in type.GetProperties(Flags)
			select p.GetPropertyFromDeclaredType(p, Flags) into p
			where InjectionHeuristics.Any((IInjectionHeuristic h) => p != null && h.ShouldInject(p))
			select p);
		if (base.Settings.InjectParentPrivateProperties)
		{
			Type baseType = type.BaseType;
			while (baseType != null)
			{
				list.AddRange(GetPrivateProperties(baseType));
				baseType = baseType.BaseType;
			}
		}
		return list;
	}

	public virtual IEnumerable<MethodInfo> SelectMethodsForInjection(Type type)
	{
		Ensure.ArgumentNotNull(type, "type");
		return from m in type.GetMethods(Flags)
			where InjectionHeuristics.Any((IInjectionHeuristic h) => h.ShouldInject(m))
			select m;
	}

	private IEnumerable<PropertyInfo> GetPrivateProperties(Type type)
	{
		return from p in type.GetProperties(Flags)
			where p.DeclaringType == type && p.IsPrivate()
			where InjectionHeuristics.Any((IInjectionHeuristic h) => h.ShouldInject(p))
			select p;
	}
}
