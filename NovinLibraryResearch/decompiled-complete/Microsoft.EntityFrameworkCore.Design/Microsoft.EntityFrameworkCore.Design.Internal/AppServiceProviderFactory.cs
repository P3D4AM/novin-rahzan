using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class AppServiceProviderFactory
{
	private readonly Assembly _startupAssembly;

	private readonly IOperationReporter _reporter;

	public AppServiceProviderFactory([JetBrains.Annotations.NotNull] Assembly startupAssembly, [JetBrains.Annotations.NotNull] IOperationReporter reporter)
	{
		_startupAssembly = startupAssembly;
		_reporter = reporter;
	}

	public virtual IServiceProvider Create([JetBrains.Annotations.NotNull] string[] args)
	{
		_reporter.WriteVerbose(DesignStrings.FindingServiceProvider);
		return CreateFromHosting(args) ?? CreateEmptyServiceProvider();
	}

	private IServiceProvider CreateFromHosting(string[] args)
	{
		_reporter.WriteVerbose(DesignStrings.FindingHostingServices);
		Func<string[], IServiceProvider> func = HostFactoryResolver.ResolveServiceProviderFactory(_startupAssembly);
		if (func == null)
		{
			_reporter.WriteVerbose(DesignStrings.NoCreateHostBuilder);
			return null;
		}
		string text = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		if (text == null)
		{
			text = "Development";
			Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", text);
		}
		_reporter.WriteVerbose(DesignStrings.UsingEnvironment(text));
		try
		{
			IServiceProvider serviceProvider = func(args);
			if (serviceProvider == null)
			{
				_reporter.WriteWarning(DesignStrings.MalformedCreateHostBuilder);
				return null;
			}
			_reporter.WriteVerbose(DesignStrings.UsingHostingServices);
			return ServiceProviderServiceExtensions.CreateScope(serviceProvider).ServiceProvider;
		}
		catch (Exception innerException)
		{
			if (innerException is TargetInvocationException)
			{
				innerException = innerException.InnerException;
			}
			_reporter.WriteVerbose(innerException.ToString());
			_reporter.WriteWarning(DesignStrings.InvokeCreateHostBuilderFailed(innerException.Message));
			return null;
		}
	}

	private IServiceProvider CreateEmptyServiceProvider()
	{
		_reporter.WriteVerbose(DesignStrings.NoServiceProvider);
		return new ServiceCollection().BuildServiceProvider();
	}
}
