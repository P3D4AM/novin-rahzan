using System;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Planning.Directives;

namespace Ninject.Selection.Heuristics;

public interface IConstructorScorer : INinjectComponent, IDisposable
{
	int Score(IContext context, ConstructorInjectionDirective directive);
}
