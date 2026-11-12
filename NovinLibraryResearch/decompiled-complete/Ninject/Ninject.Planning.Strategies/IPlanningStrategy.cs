using System;
using Ninject.Components;

namespace Ninject.Planning.Strategies;

public interface IPlanningStrategy : INinjectComponent, IDisposable
{
	void Execute(IPlan plan);
}
