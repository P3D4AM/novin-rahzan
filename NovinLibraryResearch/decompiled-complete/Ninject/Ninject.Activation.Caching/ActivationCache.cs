using System;
using System.Collections.Generic;
using Ninject.Components;
using Ninject.Infrastructure;

namespace Ninject.Activation.Caching;

public class ActivationCache : NinjectComponent, IActivationCache, INinjectComponent, IDisposable, IPruneable
{
	private readonly HashSet<object> activatedObjects = new HashSet<object>(new WeakReferenceEqualityComparer());

	private readonly HashSet<object> deactivatedObjects = new HashSet<object>(new WeakReferenceEqualityComparer());

	public int ActivatedObjectCount => activatedObjects.Count;

	public int DeactivatedObjectCount => deactivatedObjects.Count;

	public ActivationCache(ICachePruner cachePruner)
	{
		Ensure.ArgumentNotNull(cachePruner, "cachePruner");
		cachePruner.Start(this);
	}

	public void Clear()
	{
		lock (activatedObjects)
		{
			activatedObjects.Clear();
		}
		lock (deactivatedObjects)
		{
			deactivatedObjects.Clear();
		}
	}

	public void AddActivatedInstance(object instance)
	{
		lock (activatedObjects)
		{
			activatedObjects.Add(new ReferenceEqualWeakReference(instance));
		}
	}

	public void AddDeactivatedInstance(object instance)
	{
		lock (deactivatedObjects)
		{
			deactivatedObjects.Add(new ReferenceEqualWeakReference(instance));
		}
	}

	public bool IsActivated(object instance)
	{
		return activatedObjects.Contains(instance);
	}

	public bool IsDeactivated(object instance)
	{
		return deactivatedObjects.Contains(instance);
	}

	public void Prune()
	{
		lock (activatedObjects)
		{
			RemoveDeadObjects(activatedObjects);
		}
		lock (deactivatedObjects)
		{
			RemoveDeadObjects(deactivatedObjects);
		}
	}

	private static void RemoveDeadObjects(HashSet<object> objects)
	{
		objects.RemoveWhere((object reference) => !((ReferenceEqualWeakReference)reference).IsAlive);
	}
}
