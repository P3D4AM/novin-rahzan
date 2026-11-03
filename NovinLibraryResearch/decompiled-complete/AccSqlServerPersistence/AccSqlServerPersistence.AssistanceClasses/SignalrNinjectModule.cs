using System;
using Ninject.Modules;
using Ninject.Syntax;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using SignalRCoreConnectionCore;

namespace AccSqlServerPersistence.AssistanceClasses;

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
			if (!_injected)
			{
				_injected = true;
				((IBindingInSyntax<SignalRCoreConnection>)(object)((BindingRoot)this).Bind<IConnection>().To<SignalRCoreConnection>()).InTransientScope();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Load", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\AssistanceClasses\\SignalrNinjectModule.cs", 40);
		}
	}
}
