using System;
using System.Runtime.Remoting;
using System.Security;

namespace Ninject.Activation;

public class InstanceReference
{
	public object Instance { get; set; }

	[SecuritySafeCritical]
	public bool Is<T>()
	{
		if (RemotingServices.IsTransparentProxy(Instance) && ((object)RemotingServices.GetRealProxy(Instance)).GetType().Name == "RemotingProxy")
		{
			return typeof(T).IsAssignableFrom(Instance.GetType());
		}
		return Instance is T;
	}

	public T As<T>()
	{
		return (T)Instance;
	}

	public void IfInstanceIs<T>(Action<T> action)
	{
		if (Is<T>())
		{
			action((T)Instance);
		}
	}
}
