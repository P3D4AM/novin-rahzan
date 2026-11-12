using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Infrastructure;
using Ninject.Planning.Directives;

namespace Ninject.Planning;

public class Plan : IPlan
{
	public Type Type { get; private set; }

	public ICollection<IDirective> Directives { get; private set; }

	public IList<ConstructorInjectionDirective> ConstructorInjectionDirectives { get; private set; }

	public Plan(Type type)
	{
		Ensure.ArgumentNotNull(type, "type");
		Type = type;
		Directives = new List<IDirective>();
		ConstructorInjectionDirectives = new List<ConstructorInjectionDirective>();
	}

	public void Add(IDirective directive)
	{
		Ensure.ArgumentNotNull(directive, "directive");
		if (directive is ConstructorInjectionDirective item)
		{
			ConstructorInjectionDirectives.Add(item);
		}
		Directives.Add(directive);
	}

	public bool Has<TDirective>() where TDirective : IDirective
	{
		return GetAll<TDirective>().Any();
	}

	public TDirective GetOne<TDirective>() where TDirective : IDirective
	{
		return GetAll<TDirective>().SingleOrDefault();
	}

	public IEnumerable<TDirective> GetAll<TDirective>() where TDirective : IDirective
	{
		return Directives.OfType<TDirective>();
	}
}
