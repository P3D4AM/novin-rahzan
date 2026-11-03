using System;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using Ninject.Syntax;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using SignalRCoreConnectionCore;

namespace EntityCache;

public class SignalrNinjectModule : NinjectModule
{
	private class NestedSignalRNinjectModule
	{
		internal static readonly SignalrNinjectModule Instance;

		static NestedSignalRNinjectModule()
		{
			Instance = new SignalrNinjectModule();
		}
	}

	private static bool _injected;

	public static SignalrNinjectModule Instance => NestedSignalRNinjectModule.Instance;

	public override void Load()
	{
		try
		{
			if ((DI.Kernel == null || ((DisposableObject)DI.Kernel).IsDisposed) && _injected)
			{
				_injected = false;
				WebErrorLog.Instance.StartLog("عمو کرنل دیسپوز شده!", "", "Load", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\SignalrNinjectModule.cs", 25);
			}
			if (!_injected)
			{
				_injected = true;
				((IBindingInSyntax<SignalRCoreConnection>)(object)((BindingRoot)this).Bind<IConnection>().To<SignalRCoreConnection>()).InTransientScope();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Load", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\SignalrNinjectModule.cs", 38);
		}
	}
}
