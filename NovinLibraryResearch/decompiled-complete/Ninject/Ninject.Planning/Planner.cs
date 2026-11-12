using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Planning.Strategies;

namespace Ninject.Planning;

public class Planner : NinjectComponent, IPlanner, INinjectComponent, IDisposable
{
	private readonly ReaderWriterLockSlim plannerLock = new ReaderWriterLockSlim();

	private readonly Dictionary<Type, IPlan> plans = new Dictionary<Type, IPlan>();

	public IList<IPlanningStrategy> Strategies { get; private set; }

	public Planner(IEnumerable<IPlanningStrategy> strategies)
	{
		Ensure.ArgumentNotNull(strategies, "strategies");
		Strategies = strategies.ToList();
	}

	public IPlan GetPlan(Type type)
	{
		Ensure.ArgumentNotNull(type, "type");
		plannerLock.EnterUpgradeableReadLock();
		try
		{
			IPlan value;
			return plans.TryGetValue(type, out value) ? value : CreateNewPlan(type);
		}
		finally
		{
			plannerLock.ExitUpgradeableReadLock();
		}
	}

	protected virtual IPlan CreateEmptyPlan(Type type)
	{
		Ensure.ArgumentNotNull(type, "type");
		return new Plan(type);
	}

	private IPlan CreateNewPlan(Type type)
	{
		plannerLock.EnterWriteLock();
		try
		{
			if (plans.TryGetValue(type, out var plan))
			{
				return plan;
			}
			plan = CreateEmptyPlan(type);
			plans.Add(type, plan);
			Strategies.Map(delegate(IPlanningStrategy s)
			{
				s.Execute(plan);
			});
			return plan;
		}
		finally
		{
			plannerLock.ExitWriteLock();
		}
	}
}
