using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Activation;
using Ninject.Activation.Blocks;
using Ninject.Activation.Caching;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;
using Ninject.Syntax;

namespace Ninject;

public abstract class KernelBase : BindingRoot, IKernel, IBindingRoot, IFluentSyntax, IResolutionRoot, IServiceProvider, IDisposableObject, IDisposable, INotifyWhenDisposed
{
	private readonly object handleMissingBindingLockObject = new object();

	private readonly Multimap<Type, IBinding> bindings = new Multimap<Type, IBinding>();

	private readonly Dictionary<Type, List<IBinding>> bindingCache = new Dictionary<Type, List<IBinding>>();

	private readonly Dictionary<string, INinjectModule> modules = new Dictionary<string, INinjectModule>();

	private readonly IBindingPrecedenceComparer bindingPrecedenceComparer;

	public INinjectSettings Settings { get; private set; }

	public IComponentContainer Components { get; private set; }

	protected KernelBase()
		: this(new ComponentContainer(), new NinjectSettings())
	{
	}

	protected KernelBase(params INinjectModule[] modules)
		: this(new ComponentContainer(), new NinjectSettings(), modules)
	{
	}

	protected KernelBase(INinjectSettings settings, params INinjectModule[] modules)
		: this(new ComponentContainer(), settings, modules)
	{
	}

	protected KernelBase(IComponentContainer components, INinjectSettings settings, params INinjectModule[] modules)
	{
		Ensure.ArgumentNotNull(components, "components");
		Ensure.ArgumentNotNull(settings, "settings");
		Ensure.ArgumentNotNull(modules, "modules");
		Settings = settings;
		Components = components;
		components.Kernel = this;
		AddComponents();
		bindingPrecedenceComparer = Components.Get<IBindingPrecedenceComparer>();
		Bind<IKernel>().ToConstant(this).InTransientScope();
		Bind<IResolutionRoot>().ToConstant(this).InTransientScope();
		if (Settings.LoadExtensions)
		{
			Load(Settings.ExtensionSearchPatterns);
		}
		Load(modules);
	}

	public override void Dispose(bool disposing)
	{
		if (disposing && !base.IsDisposed && Components != null)
		{
			Components.Get<ICache>().Clear();
			Components.Dispose();
		}
		base.Dispose(disposing);
	}

	public override void Unbind(Type service)
	{
		Ensure.ArgumentNotNull(service, "service");
		bindings.RemoveAll(service);
		lock (bindingCache)
		{
			bindingCache.Clear();
		}
	}

	public override void AddBinding(IBinding binding)
	{
		Ensure.ArgumentNotNull(binding, "binding");
		AddBindings(new IBinding[1] { binding });
	}

	public override void RemoveBinding(IBinding binding)
	{
		Ensure.ArgumentNotNull(binding, "binding");
		bindings.Remove(binding.Service, binding);
		lock (bindingCache)
		{
			bindingCache.Clear();
		}
	}

	public bool HasModule(string name)
	{
		Ensure.ArgumentNotNullOrEmpty(name, "name");
		return modules.ContainsKey(name);
	}

	public IEnumerable<INinjectModule> GetModules()
	{
		return modules.Values.ToArray();
	}

	public void Load(IEnumerable<INinjectModule> m)
	{
		Ensure.ArgumentNotNull(m, "modules");
		m = m.ToList();
		foreach (INinjectModule item in m)
		{
			if (string.IsNullOrEmpty(item.Name))
			{
				throw new NotSupportedException(ExceptionFormatter.ModulesWithNullOrEmptyNamesAreNotSupported());
			}
			if (modules.TryGetValue(item.Name, out var value))
			{
				throw new NotSupportedException(ExceptionFormatter.ModuleWithSameNameIsAlreadyLoaded(item, value));
			}
			item.OnLoad(this);
			modules.Add(item.Name, item);
		}
		foreach (INinjectModule item2 in m)
		{
			item2.OnVerifyRequiredModules();
		}
	}

	public void Load(IEnumerable<string> filePatterns)
	{
		Components.Get<IModuleLoader>().LoadModules(filePatterns);
	}

	public void Load(IEnumerable<Assembly> assemblies)
	{
		Load(assemblies.SelectMany((Assembly asm) => asm.GetNinjectModules()));
	}

	public void Unload(string name)
	{
		Ensure.ArgumentNotNullOrEmpty(name, "name");
		if (!modules.TryGetValue(name, out var value))
		{
			throw new NotSupportedException(ExceptionFormatter.NoModuleLoadedWithTheSpecifiedName(name));
		}
		value.OnUnload(this);
		modules.Remove(name);
	}

	public virtual void Inject(object instance, params IParameter[] parameters)
	{
		Ensure.ArgumentNotNull(instance, "instance");
		Ensure.ArgumentNotNull(parameters, "parameters");
		Type type = instance.GetType();
		IPlanner planner = Components.Get<IPlanner>();
		IPipeline pipeline = Components.Get<IPipeline>();
		Binding binding = new Binding(type);
		IRequest request = CreateRequest(type, null, parameters, isOptional: false, isUnique: false);
		IContext context = CreateContext(request, binding);
		context.Plan = planner.GetPlan(type);
		InstanceReference reference = new InstanceReference
		{
			Instance = instance
		};
		pipeline.Activate(context, reference);
	}

	public virtual bool Release(object instance)
	{
		Ensure.ArgumentNotNull(instance, "instance");
		return Components.Get<ICache>().Release(instance);
	}

	public virtual bool CanResolve(IRequest request)
	{
		Ensure.ArgumentNotNull(request, "request");
		return GetBindings(request.Service).Any(SatifiesRequest(request));
	}

	public virtual bool CanResolve(IRequest request, bool ignoreImplicitBindings)
	{
		Ensure.ArgumentNotNull(request, "request");
		return GetBindings(request.Service).Any((IBinding binding) => (!ignoreImplicitBindings || !binding.IsImplicit) && SatifiesRequest(request)(binding));
	}

	public virtual IEnumerable<object> Resolve(IRequest request)
	{
		return Resolve(request, handleMissingBindings: true, filterImplicitBindings: false);
	}

	public virtual IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique)
	{
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(parameters, "parameters");
		return new Request(service, constraint, parameters, null, isOptional, isUnique);
	}

	public virtual IActivationBlock BeginBlock()
	{
		return new ActivationBlock(this);
	}

	public virtual IEnumerable<IBinding> GetBindings(Type service)
	{
		Ensure.ArgumentNotNull(service, "service");
		lock (bindingCache)
		{
			if (!bindingCache.ContainsKey(service))
			{
				List<IBinding> list = Components.GetAll<IBindingResolver>().SelectMany((IBindingResolver resolver) => resolver.Resolve(bindings, service)).OrderByDescending((IBinding b) => b, bindingPrecedenceComparer)
					.ToList();
				bindingCache.Add(service, list);
				return list;
			}
			return bindingCache[service];
		}
	}

	object IServiceProvider.GetService(Type service)
	{
		if (!Settings.ThrowOnGetServiceNotFound)
		{
			return this.TryGet(service);
		}
		return this.Get(service);
	}

	protected virtual Func<IBinding, bool> SatifiesRequest(IRequest request)
	{
		return (IBinding binding) => binding.Matches(request) && request.Matches(binding);
	}

	protected abstract void AddComponents();

	protected virtual bool HandleMissingBinding(IRequest request)
	{
		Ensure.ArgumentNotNull(request, "request");
		List<IBinding> list = (from c in Components.GetAll<IMissingBindingResolver>()
			select c.Resolve(bindings, request).ToList()).FirstOrDefault((List<IBinding> b) => b.Any());
		if (list == null)
		{
			return false;
		}
		lock (handleMissingBindingLockObject)
		{
			if (!CanResolve(request))
			{
				list.Map(delegate(IBinding binding)
				{
					binding.IsImplicit = true;
				});
				AddBindings(list);
			}
		}
		return true;
	}

	protected virtual IContext CreateContext(IRequest request, IBinding binding)
	{
		Ensure.ArgumentNotNull(request, "request");
		Ensure.ArgumentNotNull(binding, "binding");
		return new Context(this, request, binding, Components.Get<ICache>(), Components.Get<IPlanner>(), Components.Get<IPipeline>());
	}

	private IEnumerable<object> Resolve(IRequest request, bool handleMissingBindings, bool filterImplicitBindings)
	{
		if (request.Service.IsArray)
		{
			Type elementType = request.Service.GetElementType();
			UpdateRequest(elementType);
			return new Array[1] { Resolve(request, handleMissingBindings: false, filterImplicitBindings: true).CastSlow(elementType).ToArraySlow(elementType) };
		}
		if (request.Service.IsGenericType)
		{
			Type genericTypeDefinition = request.Service.GetGenericTypeDefinition();
			if (genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(ICollection<>))
			{
				Type type = request.Service.GenericTypeArguments[0];
				UpdateRequest(type);
				return new IList[1] { Resolve(request, handleMissingBindings: false, filterImplicitBindings: true).CastSlow(type).ToListSlow(type) };
			}
			if (genericTypeDefinition == typeof(IEnumerable<>))
			{
				Type type2 = request.Service.GenericTypeArguments[0];
				UpdateRequest(type2);
				return new IEnumerable[1] { Resolve(request, handleMissingBindings: false, filterImplicitBindings: true).CastSlow(type2) };
			}
		}
		IEnumerable<IBinding> enumerable = GetBindings(request.Service).Where(SatifiesRequest(request));
		if (filterImplicitBindings)
		{
			enumerable = enumerable.Where((IBinding binding) => !binding.IsImplicit);
		}
		IEnumerator<IBinding> enumerator = enumerable.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			if (handleMissingBindings && HandleMissingBinding(request))
			{
				return Resolve(request, handleMissingBindings: false, filterImplicitBindings: false);
			}
			if (request.IsOptional)
			{
				return Enumerable.Empty<object>();
			}
			throw new ActivationException(ExceptionFormatter.CouldNotResolveBinding(request));
		}
		if (request.IsUnique)
		{
			IBinding current = enumerator.Current;
			if (enumerator.MoveNext() && bindingPrecedenceComparer.Compare(current, enumerator.Current) == 0)
			{
				if (request.IsOptional && !request.ForceUnique)
				{
					return Enumerable.Empty<object>();
				}
				IEnumerable<string> source = from binding in enumerable
					let context = CreateContext(request, binding)
					select binding.Format(context);
				throw new ActivationException(ExceptionFormatter.CouldNotUniquelyResolveBinding(request, source.ToArray()));
			}
			return new object[1] { CreateContext(request, current).Resolve() };
		}
		if (enumerable.Any((IBinding binding) => !binding.IsImplicit))
		{
			enumerable = enumerable.Where((IBinding binding) => !binding.IsImplicit);
		}
		return enumerable.Select((IBinding binding) => CreateContext(request, binding).Resolve());
		void UpdateRequest(Type service)
		{
			if (request.ParentRequest == null)
			{
				request = CreateRequest(service, null, request.Parameters.Where((IParameter p) => p.ShouldInherit), isOptional: true, isUnique: false);
			}
			else
			{
				request = request.ParentRequest.CreateChild(service, request.ParentContext, request.Target);
				request.IsOptional = true;
			}
		}
	}

	private void AddBindings(IEnumerable<IBinding> bindings)
	{
		bindings.Map(delegate(IBinding binding)
		{
			this.bindings.Add(binding.Service, binding);
		});
		lock (bindingCache)
		{
			bindingCache.Clear();
		}
	}

	Type IFluentSyntax.GetType()
	{
		return GetType();
	}
}
