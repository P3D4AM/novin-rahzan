using System;
using System.Reflection;
using Ninject.Components;

namespace Ninject.Selection.Heuristics;

public interface IInjectionHeuristic : INinjectComponent, IDisposable
{
	bool ShouldInject(MemberInfo member);
}
