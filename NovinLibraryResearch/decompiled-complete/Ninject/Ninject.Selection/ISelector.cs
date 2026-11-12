using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Components;
using Ninject.Selection.Heuristics;

namespace Ninject.Selection;

public interface ISelector : INinjectComponent, IDisposable
{
	IConstructorScorer ConstructorScorer { get; }

	ICollection<IInjectionHeuristic> InjectionHeuristics { get; }

	IEnumerable<ConstructorInfo> SelectConstructorsForInjection(Type type);

	IEnumerable<PropertyInfo> SelectPropertiesForInjection(Type type);

	IEnumerable<MethodInfo> SelectMethodsForInjection(Type type);
}
