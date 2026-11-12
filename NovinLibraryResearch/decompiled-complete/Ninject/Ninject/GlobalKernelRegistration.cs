using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ninject;

public abstract class GlobalKernelRegistration
{
	private class Registration
	{
		public ReaderWriterLockSlim KernelLock { get; private set; }

		public IList<WeakReference> Kernels { get; private set; }

		public Registration()
		{
			KernelLock = new ReaderWriterLockSlim();
			Kernels = new List<WeakReference>();
		}
	}

	private static readonly ReaderWriterLockSlim KernelRegistrationsLock = new ReaderWriterLockSlim();

	private static readonly IDictionary<Type, Registration> KernelRegistrations = new Dictionary<Type, Registration>();

	internal static void RegisterKernelForType(IKernel kernel, Type type)
	{
		Registration registrationForType = GetRegistrationForType(type);
		registrationForType.KernelLock.EnterWriteLock();
		try
		{
			registrationForType.Kernels.Add(new WeakReference(kernel));
		}
		finally
		{
			registrationForType.KernelLock.ExitWriteLock();
		}
	}

	internal static void UnregisterKernelForType(IKernel kernel, Type type)
	{
		Registration registrationForType = GetRegistrationForType(type);
		RemoveKernels(registrationForType, registrationForType.Kernels.Where((WeakReference reference) => reference.Target == kernel || !reference.IsAlive));
	}

	protected void MapKernels(Action<IKernel> action)
	{
		bool flag = false;
		Registration registrationForType = GetRegistrationForType(GetType());
		registrationForType.KernelLock.EnterReadLock();
		try
		{
			foreach (WeakReference kernel in registrationForType.Kernels)
			{
				if (kernel.Target is IKernel obj)
				{
					action(obj);
				}
				else
				{
					flag = true;
				}
			}
		}
		finally
		{
			registrationForType.KernelLock.ExitReadLock();
		}
		if (flag)
		{
			RemoveKernels(registrationForType, registrationForType.Kernels.Where((WeakReference reference) => !reference.IsAlive));
		}
	}

	private static void RemoveKernels(Registration registration, IEnumerable<WeakReference> references)
	{
		registration.KernelLock.EnterWriteLock();
		try
		{
			WeakReference[] array = references.ToArray();
			foreach (WeakReference item in array)
			{
				registration.Kernels.Remove(item);
			}
		}
		finally
		{
			registration.KernelLock.ExitWriteLock();
		}
	}

	private static Registration GetRegistrationForType(Type type)
	{
		KernelRegistrationsLock.EnterUpgradeableReadLock();
		try
		{
			if (KernelRegistrations.TryGetValue(type, out var value))
			{
				return value;
			}
			return CreateNewRegistration(type);
		}
		finally
		{
			KernelRegistrationsLock.ExitUpgradeableReadLock();
		}
	}

	private static Registration CreateNewRegistration(Type type)
	{
		KernelRegistrationsLock.EnterWriteLock();
		try
		{
			if (KernelRegistrations.TryGetValue(type, out var value))
			{
				return value;
			}
			value = new Registration();
			KernelRegistrations.Add(type, value);
			return value;
		}
		finally
		{
			KernelRegistrationsLock.ExitWriteLock();
		}
	}
}
