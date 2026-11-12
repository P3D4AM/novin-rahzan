using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class DesignTimeServicesBuilder
{
	private readonly Assembly _assembly;

	private readonly Assembly _startupAssembly;

	private readonly IOperationReporter _reporter;

	private readonly string[] _args;

	public DesignTimeServicesBuilder([JetBrains.Annotations.NotNull] Assembly assembly, [JetBrains.Annotations.NotNull] Assembly startupAssembly, [JetBrains.Annotations.NotNull] IOperationReporter reporter, [JetBrains.Annotations.NotNull] string[] args)
	{
		_startupAssembly = startupAssembly;
		_reporter = reporter;
		_args = args;
		_assembly = assembly;
	}

	public virtual IServiceProvider Build([JetBrains.Annotations.NotNull] DbContext context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(context, "context");
		IServiceCollection services = new ServiceCollection().AddEntityFrameworkDesignTimeServices(_reporter).AddDbContextDesignTimeServices(context);
		string name = context.GetService<IDatabaseProvider>().Name;
		ConfigureProviderServices(name, services);
		ConfigureReferencedServices(services, name);
		ConfigureUserServices(services);
		return services.BuildServiceProvider();
	}

	public virtual IServiceProvider Build([JetBrains.Annotations.NotNull] string provider)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(provider, "provider");
		IServiceCollection services = new ServiceCollection().AddEntityFrameworkDesignTimeServices(_reporter, GetApplicationServices);
		ConfigureProviderServices(provider, services, throwOnError: true);
		ConfigureReferencedServices(services, provider);
		ConfigureUserServices(services);
		return services.BuildServiceProvider();
	}

	private IServiceProvider GetApplicationServices()
	{
		return new AppServiceProviderFactory(_startupAssembly, _reporter).Create(_args);
	}

	private void ConfigureUserServices(IServiceCollection services)
	{
		_reporter.WriteVerbose(DesignStrings.FindingDesignTimeServices(_startupAssembly.GetName().Name));
		Type type = (from t in _startupAssembly.GetLoadableDefinedTypes()
			where typeof(IDesignTimeServices).GetTypeInfo().IsAssignableFrom(t)
			select t.AsType()).FirstOrDefault();
		if (type == null)
		{
			_reporter.WriteVerbose(DesignStrings.NoDesignTimeServices);
			return;
		}
		_reporter.WriteVerbose(DesignStrings.UsingDesignTimeServices(type.ShortDisplayName()));
		ConfigureDesignTimeServices(type, services);
	}

	private void ConfigureReferencedServices(IServiceCollection services, string provider)
	{
		_reporter.WriteVerbose(DesignStrings.FindingReferencedServices(_startupAssembly.GetName().Name));
		List<DesignTimeServicesReferenceAttribute> list = _startupAssembly.GetCustomAttributes<DesignTimeServicesReferenceAttribute>().Concat(_assembly.GetCustomAttributes<DesignTimeServicesReferenceAttribute>()).Distinct()
			.ToList();
		if (list.Count == 0)
		{
			_reporter.WriteVerbose(DesignStrings.NoReferencedServices);
			return;
		}
		foreach (DesignTimeServicesReferenceAttribute item in list)
		{
			if (item.ForProvider == null || string.Equals(item.ForProvider, provider, StringComparison.OrdinalIgnoreCase))
			{
				Type type = Type.GetType(item.TypeName, throwOnError: true);
				_reporter.WriteVerbose(DesignStrings.UsingReferencedServices(type.Assembly.GetName().Name));
				ConfigureDesignTimeServices(type, services);
			}
		}
	}

	private void ConfigureProviderServices(string provider, IServiceCollection services, bool throwOnError = false)
	{
		_reporter.WriteVerbose(DesignStrings.FindingProviderServices(provider));
		Assembly assembly;
		try
		{
			assembly = Assembly.Load(new AssemblyName(provider));
		}
		catch (Exception innerException)
		{
			string message = DesignStrings.CannotFindRuntimeProviderAssembly(provider);
			if (!throwOnError)
			{
				_reporter.WriteVerbose(message);
				return;
			}
			throw new OperationException(message, innerException);
		}
		DesignTimeProviderServicesAttribute customAttribute = assembly.GetCustomAttribute<DesignTimeProviderServicesAttribute>();
		if (customAttribute == null)
		{
			string message2 = DesignStrings.CannotFindDesignTimeProviderAssemblyAttribute("DesignTimeProviderServicesAttribute", provider);
			if (throwOnError)
			{
				throw new InvalidOperationException(message2);
			}
			_reporter.WriteVerbose(message2);
		}
		else
		{
			Type? type = assembly.GetType(customAttribute.TypeName, throwOnError: true, ignoreCase: false);
			_reporter.WriteVerbose(DesignStrings.UsingProviderServices(provider));
			ConfigureDesignTimeServices(type, services);
		}
	}

	private static void ConfigureDesignTimeServices(Type designTimeServicesType, IServiceCollection services)
	{
		((IDesignTimeServices)Activator.CreateInstance(designTimeServicesType)).ConfigureDesignTimeServices(services);
	}
}
