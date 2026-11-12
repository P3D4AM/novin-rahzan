using System;
using System.Reflection;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Planning.Directives;

namespace Ninject.Selection.Heuristics;

public class SpecificConstructorSelector : NinjectComponent, IConstructorScorer, INinjectComponent, IDisposable
{
	private readonly ConstructorInfo constructorInfo;

	public SpecificConstructorSelector(ConstructorInfo constructorInfo)
	{
		this.constructorInfo = constructorInfo;
	}

	public virtual int Score(IContext context, ConstructorInjectionDirective directive)
	{
		if (!directive.Constructor.Equals(constructorInfo))
		{
			return 0;
		}
		return 1;
	}
}
