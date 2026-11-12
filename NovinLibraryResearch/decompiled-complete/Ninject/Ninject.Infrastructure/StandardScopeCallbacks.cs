using System;
using System.Threading;
using Ninject.Activation;

namespace Ninject.Infrastructure;

public class StandardScopeCallbacks
{
	public static readonly Func<IContext, object> Transient = (IContext ctx) => (object)null;

	public static readonly Func<IContext, object> Singleton = (IContext ctx) => ctx.Kernel;

	public static readonly Func<IContext, object> Thread = (IContext ctx) => System.Threading.Thread.CurrentThread;
}
