using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class LazyLoaderParameterBindingFactory : ServiceParameterBindingFactory
{
	private static readonly MethodInfo _loadMethod = typeof(ILazyLoader).GetMethod("Load");

	private static readonly MethodInfo _loadAsyncMethod = typeof(ILazyLoader).GetMethod("LoadAsync");

	public LazyLoaderParameterBindingFactory([JetBrains.Annotations.NotNull] LazyLoaderParameterBindingFactoryDependencies dependencies)
		: base(typeof(ILazyLoader))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public override bool CanBind(Type parameterType, string parameterName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterType, "parameterType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(parameterName, "parameterName");
		if (!IsLazyLoader(parameterType) && !IsLazyLoaderMethod(parameterType, parameterName))
		{
			return IsLazyLoaderAsyncMethod(parameterType, parameterName);
		}
		return true;
	}

	public override ParameterBinding Bind(IMutableEntityType entityType, Type parameterType, string parameterName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterType, "parameterType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(parameterName, "parameterName");
		IMutableEntityType mutableEntityType = entityType;
		do
		{
			mutableEntityType.SetNavigationAccessMode(PropertyAccessMode.Field);
			mutableEntityType = mutableEntityType.BaseType;
		}
		while (mutableEntityType != null);
		return Bind(entityType, parameterType);
	}

	public override ParameterBinding Bind(IConventionEntityType entityType, Type parameterType, string parameterName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterType, "parameterType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(parameterName, "parameterName");
		IConventionEntityType conventionEntityType = entityType;
		do
		{
			conventionEntityType.SetNavigationAccessMode(PropertyAccessMode.Field);
			conventionEntityType = conventionEntityType.BaseType;
		}
		while (conventionEntityType != null);
		return Bind(entityType, parameterType);
	}

	private static ParameterBinding Bind(IEntityType entityType, Type parameterType)
	{
		if (!(parameterType == typeof(ILazyLoader)))
		{
			if (!(parameterType == typeof(Action<object, string>)))
			{
				return new DependencyInjectionMethodParameterBinding(typeof(Func<object, CancellationToken, string, Task>), typeof(ILazyLoader), _loadAsyncMethod, entityType.GetServiceProperties().FirstOrDefault((IServiceProperty p) => IsLazyLoaderAsyncMethod(p.ClrType, p.Name)));
			}
			return new DependencyInjectionMethodParameterBinding(typeof(Action<object, string>), typeof(ILazyLoader), _loadMethod, entityType.GetServiceProperties().FirstOrDefault((IServiceProperty p) => IsLazyLoaderMethod(p.ClrType, p.Name)));
		}
		return new DependencyInjectionParameterBinding(typeof(ILazyLoader), typeof(ILazyLoader), entityType.GetServiceProperties().FirstOrDefault((IServiceProperty p) => IsLazyLoader(p.ClrType)));
	}

	private static bool IsLazyLoader(Type type)
	{
		return type == typeof(ILazyLoader);
	}

	private static bool IsLazyLoaderMethod(Type type, string name)
	{
		if (type == typeof(Action<object, string>))
		{
			return name.Equals("lazyLoader", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static bool IsLazyLoaderAsyncMethod(Type type, string name)
	{
		if (type == typeof(Func<object, CancellationToken, string, Task>))
		{
			return name.Equals("lazyLoader", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}
}
