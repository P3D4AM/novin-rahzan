using System;
using System.Linq.Expressions;
using System.Reflection;
using Ninject.Activation;
using Ninject.Activation.Providers;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Syntax;

namespace Ninject.Planning.Bindings;

public class BindingBuilder
{
	private class ConstructorArgumentSyntax : IConstructorArgumentSyntax, IFluentSyntax
	{
		public IContext Context { get; private set; }

		public ConstructorArgumentSyntax(IContext context)
		{
			Context = context;
		}

		public T1 Inject<T1>()
		{
			throw new InvalidOperationException("This method is for declaration that a parameter shall be injected only! Never call it directly.");
		}

		Type IFluentSyntax.GetType()
		{
			return GetType();
		}
	}

	public IBindingConfiguration BindingConfiguration { get; private set; }

	public IKernel Kernel { get; private set; }

	protected string ServiceNames { get; private set; }

	public BindingBuilder(IBindingConfiguration bindingConfiguration, IKernel kernel, string serviceNames)
	{
		Ensure.ArgumentNotNull(bindingConfiguration, "binding");
		Ensure.ArgumentNotNull(kernel, "kernel");
		BindingConfiguration = bindingConfiguration;
		Kernel = kernel;
		ServiceNames = serviceNames;
		BindingConfiguration.ScopeCallback = kernel.Settings.DefaultScopeCallback;
	}

	protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalTo<TImplementation>()
	{
		return InternalTo<TImplementation>(typeof(TImplementation));
	}

	protected IBindingWhenInNamedWithOrOnSyntax<T> InternalTo<T>(Type implementation)
	{
		BindingConfiguration.ProviderCallback = StandardProvider.GetCreationCallback(implementation);
		BindingConfiguration.Target = BindingTarget.Type;
		return new BindingConfigurationBuilder<T>(BindingConfiguration, ServiceNames, Kernel);
	}

	protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToConfiguration<TImplementation>(TImplementation value)
	{
		BindingConfiguration.ProviderCallback = (IContext ctx) => new ConstantProvider<TImplementation>(value);
		BindingConfiguration.Target = BindingTarget.Constant;
		BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Singleton;
		return new BindingConfigurationBuilder<TImplementation>(BindingConfiguration, ServiceNames, Kernel);
	}

	protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToMethod<TImplementation>(Func<IContext, TImplementation> method)
	{
		BindingConfiguration.ProviderCallback = (IContext ctx) => new CallbackProvider<TImplementation>(method);
		BindingConfiguration.Target = BindingTarget.Method;
		return new BindingConfigurationBuilder<TImplementation>(BindingConfiguration, ServiceNames, Kernel);
	}

	protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToProvider<TImplementation>(IProvider<TImplementation> provider)
	{
		BindingConfiguration.ProviderCallback = (IContext ctx) => provider;
		BindingConfiguration.Target = BindingTarget.Provider;
		return new BindingConfigurationBuilder<TImplementation>(BindingConfiguration, ServiceNames, Kernel);
	}

	protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProviderInternal<TProvider, TImplementation>() where TProvider : IProvider
	{
		BindingConfiguration.ProviderCallback = (IContext ctx) => ctx.Kernel.Get<TProvider>(new IParameter[0]);
		BindingConfiguration.Target = BindingTarget.Provider;
		return new BindingConfigurationBuilder<TImplementation>(BindingConfiguration, ServiceNames, Kernel);
	}

	protected IBindingWhenInNamedWithOrOnSyntax<T> ToProviderInternal<T>(Type providerType)
	{
		BindingConfiguration.ProviderCallback = (IContext ctx) => ctx.Kernel.Get(providerType) as IProvider;
		BindingConfiguration.Target = BindingTarget.Provider;
		return new BindingConfigurationBuilder<T>(BindingConfiguration, ServiceNames, Kernel);
	}

	protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression)
	{
		if (!(newExpression.Body is NewExpression newExpression2))
		{
			throw new ArgumentException("The expression must be a constructor call.", "newExpression");
		}
		BindingConfiguration.ProviderCallback = StandardProvider.GetCreationCallback(newExpression2.Type, newExpression2.Constructor);
		BindingConfiguration.Target = BindingTarget.Type;
		AddConstructorArguments(newExpression2, newExpression.Parameters[0]);
		return new BindingConfigurationBuilder<TImplementation>(BindingConfiguration, ServiceNames, Kernel);
	}

	private void AddConstructorArguments(NewExpression ctorExpression, ParameterExpression constructorArgumentSyntaxParameterExpression)
	{
		ParameterInfo[] parameters = ctorExpression.Constructor.GetParameters();
		for (int i = 0; i < ctorExpression.Arguments.Count; i++)
		{
			Expression argument = ctorExpression.Arguments[i];
			string name = parameters[i].Name;
			AddConstructorArgument(argument, name, constructorArgumentSyntaxParameterExpression);
		}
	}

	private void AddConstructorArgument(Expression argument, string argumentName, ParameterExpression constructorArgumentSyntaxParameterExpression)
	{
		if (!(argument is MethodCallExpression methodCallExpression) || !methodCallExpression.Method.IsGenericMethod || methodCallExpression.Method.GetGenericMethodDefinition().DeclaringType != typeof(IConstructorArgumentSyntax))
		{
			Delegate compiledExpression = Expression.Lambda(argument, constructorArgumentSyntaxParameterExpression).Compile();
			BindingConfiguration.Parameters.Add(new ConstructorArgument(argumentName, (IContext ctx) => compiledExpression.DynamicInvoke(new ConstructorArgumentSyntax(ctx))));
		}
	}
}
public class BindingBuilder<T1, T2, T3, T4> : BindingBuilder, IBindingToSyntax<T1, T2, T3, T4>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	public BindingBuilder(IBindingConfiguration bindingConfigurationConfiguration, IKernel kernel, string serviceNames)
		: base(bindingConfigurationConfiguration, kernel, serviceNames)
	{
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1, T2, T3, T4
	{
		return InternalTo<TImplementation>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation)
	{
		return InternalTo<object>(implementation);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1, T2, T3, T4
	{
		return InternalToConstructor(newExpression);
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>() where TProvider : IProvider
	{
		return ToProviderInternal<TProvider, object>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>() where TProvider : IProvider<TImplementation> where TImplementation : T1, T2, T3, T4
	{
		return ToProviderInternal<TProvider, TImplementation>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType)
	{
		return ToProviderInternal<object>(providerType);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1, T2, T3, T4
	{
		return InternalToProvider(provider);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1, T2, T3
	{
		return InternalToMethod(method);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1, T2, T3, T4
	{
		return InternalToConfiguration(value);
	}

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
public class BindingBuilder<T1, T2, T3> : BindingBuilder, IBindingToSyntax<T1, T2, T3>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	public BindingBuilder(IBindingConfiguration bindingConfigurationConfiguration, IKernel kernel, string serviceNames)
		: base(bindingConfigurationConfiguration, kernel, serviceNames)
	{
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1, T2, T3
	{
		return InternalTo<TImplementation>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation)
	{
		return InternalTo<object>(implementation);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1, T2, T3
	{
		return InternalToConstructor(newExpression);
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>() where TProvider : IProvider
	{
		return ToProviderInternal<TProvider, object>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>() where TProvider : IProvider<TImplementation> where TImplementation : T1, T2, T3
	{
		return ToProviderInternal<TProvider, TImplementation>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType)
	{
		return ToProviderInternal<object>(providerType);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1, T2, T3
	{
		return InternalToProvider(provider);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1, T2, T3
	{
		return InternalToMethod(method);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1, T2, T3
	{
		return InternalToConfiguration(value);
	}

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
public class BindingBuilder<T1, T2> : BindingBuilder, IBindingToSyntax<T1, T2>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	public BindingBuilder(IBindingConfiguration bindingConfigurationConfiguration, IKernel kernel, string serviceNames)
		: base(bindingConfigurationConfiguration, kernel, serviceNames)
	{
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1, T2
	{
		return InternalTo<TImplementation>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation)
	{
		return InternalTo<object>(implementation);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1, T2
	{
		return InternalToConstructor(newExpression);
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>() where TProvider : IProvider
	{
		return ToProviderInternal<TProvider, object>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>() where TProvider : IProvider<TImplementation> where TImplementation : T1, T2
	{
		return ToProviderInternal<TProvider, TImplementation>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType)
	{
		return ToProviderInternal<object>(providerType);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1, T2
	{
		return InternalToProvider(provider);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1, T2
	{
		return InternalToMethod(method);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1, T2
	{
		return InternalToConfiguration(value);
	}

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
public class BindingBuilder<T1> : BindingBuilder, IBindingToSyntax<T1>, IBindingSyntax, IHaveBindingConfiguration, IHaveKernel, IFluentSyntax
{
	public IBinding Binding { get; private set; }

	public BindingBuilder(IBinding binding, IKernel kernel, string serviceNames)
		: base(binding.BindingConfiguration, kernel, serviceNames)
	{
		Ensure.ArgumentNotNull(binding, "binding");
		Ensure.ArgumentNotNull(kernel, "kernel");
		Binding = binding;
	}

	public IBindingWhenInNamedWithOrOnSyntax<T1> ToSelf()
	{
		Binding.ProviderCallback = StandardProvider.GetCreationCallback(Binding.Service);
		Binding.Target = BindingTarget.Self;
		return new BindingConfigurationBuilder<T1>(Binding.BindingConfiguration, base.ServiceNames, base.Kernel);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1
	{
		return InternalTo<TImplementation>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<T1> To(Type implementation)
	{
		return InternalTo<T1>(implementation);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : T1
	{
		return InternalToConstructor(newExpression);
	}

	public IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider<TProvider>() where TProvider : IProvider
	{
		return ToProviderInternal<TProvider, T1>();
	}

	public IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider(Type providerType)
	{
		return ToProviderInternal<T1>(providerType);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : T1
	{
		return InternalToProvider(provider);
	}

	public IBindingWhenInNamedWithOrOnSyntax<T1> ToMethod(Func<IContext, T1> method)
	{
		return InternalToMethod(method);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : T1
	{
		return InternalToMethod(method);
	}

	public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : T1
	{
		return InternalToConfiguration(value);
	}

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
