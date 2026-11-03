using Ninject;
using Ninject.Modules;
using PacketParser.Entities;

namespace ClientHub;

public class NinjectLoggerKernel : StandardKernel
{
	public NinjectLoggerKernel(params INinjectModule[] modules)
		: base(modules)
	{
	}

	public NinjectLoggerKernel(INinjectSettings settings, params INinjectModule[] modules)
		: base(settings, modules)
	{
	}

	public override void Dispose(bool disposing)
	{
		if (disposing)
		{
			WebErrorLog.Instance.Error("Ninject kernel is disposed", "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NinjectLoggerKernel.cs", 18);
		}
	}
}
