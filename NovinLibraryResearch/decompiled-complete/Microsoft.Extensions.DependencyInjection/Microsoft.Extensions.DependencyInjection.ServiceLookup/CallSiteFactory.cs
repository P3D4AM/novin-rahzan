using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
internal sealed class CallSiteFactory : IServiceProviderIsService
{
	private struct ServiceDescriptorCacheItem
	{
		[DisallowNull]
		private ServiceDescriptor _item;

		[DisallowNull]
		private List<ServiceDescriptor> _items;

		public ServiceDescriptor Last
		{
			get
			{
				if (_items != null && _items.Count > 0)
				{
					return _items[_items.Count - 1];
				}
				return _item;
			}
		}

		public int Count
		{
			get
			{
				if (_item == null)
				{
					return 0;
				}
				return 1 + (_items?.Count ?? 0);
			}
		}

		public ServiceDescriptor this[int index]
		{
			get
			{
				if (index >= Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == 0)
				{
					return _item;
				}
				return _items[index - 1];
			}
		}

		public int GetSlot(ServiceDescriptor descriptor)
		{
			if (descriptor == _item)
			{
				return Count - 1;
			}
			if (_items != null)
			{
				int num = _items.IndexOf(descriptor);
				if (num != -1)
				{
					return _items.Count - (num + 1);
				}
			}
			throw new InvalidOperationException(System.SR.ServiceDescriptorNotExist);
		}

		public ServiceDescriptorCacheItem Add(ServiceDescriptor descriptor)
		{
			ServiceDescriptorCacheItem result = default(ServiceDescriptorCacheItem);
			if (_item == null)
			{
				result._item = descriptor;
			}
			else
			{
				result._item = _item;
				result._items = _items ?? new List<ServiceDescriptor>();
				result._items.Add(descriptor);
			}
			return result;
		}
	}

	private const int DefaultSlot = 0;

	private readonly ServiceDescriptor[] _descriptors;

	private readonly ConcurrentDictionary<ServiceCacheKey, ServiceCallSite> _callSiteCache = new ConcurrentDictionary<ServiceCacheKey, ServiceCallSite>();

	private readonly Dictionary<Type, ServiceDescriptorCacheItem> _descriptorLookup = new Dictionary<Type, ServiceDescriptorCacheItem>();

	private readonly ConcurrentDictionary<Type, object> _callSiteLocks = new ConcurrentDictionary<Type, object>();

	private readonly StackGuard _stackGuard;

	internal ServiceDescriptor[] Descriptors => _descriptors;

	public CallSiteFactory(ICollection<ServiceDescriptor> descriptors)
	{
		_stackGuard = new StackGuard();
		_descriptors = new ServiceDescriptor[descriptors.Count];
		descriptors.CopyTo(_descriptors, 0);
		Populate();
	}

	private void Populate()
	{
		ServiceDescriptor[] descriptors = _descriptors;
		foreach (ServiceDescriptor serviceDescriptor in descriptors)
		{
			Type serviceType = serviceDescriptor.ServiceType;
			if (serviceType.IsGenericTypeDefinition)
			{
				Type implementationType = serviceDescriptor.ImplementationType;
				if (implementationType == null || !implementationType.IsGenericTypeDefinition)
				{
					throw new ArgumentException(System.SR.Format(System.SR.OpenGenericServiceRequiresOpenGenericImplementation, serviceType), "descriptors");
				}
				if (implementationType.IsAbstract || implementationType.IsInterface)
				{
					throw new ArgumentException(System.SR.Format(System.SR.TypeCannotBeActivated, implementationType, serviceType));
				}
				Type[] genericArguments = serviceType.GetGenericArguments();
				Type[] genericArguments2 = implementationType.GetGenericArguments();
				if (genericArguments.Length != genericArguments2.Length)
				{
					throw new ArgumentException(System.SR.Format(System.SR.ArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementation, serviceType, implementationType), "descriptors");
				}
				if (ServiceProvider.VerifyOpenGenericServiceTrimmability)
				{
					ValidateTrimmingAnnotations(serviceType, genericArguments, implementationType, genericArguments2);
				}
			}
			else if (serviceDescriptor.ImplementationInstance == null && serviceDescriptor.ImplementationFactory == null)
			{
				Type implementationType2 = serviceDescriptor.ImplementationType;
				if (implementationType2.IsGenericTypeDefinition || implementationType2.IsAbstract || implementationType2.IsInterface)
				{
					throw new ArgumentException(System.SR.Format(System.SR.TypeCannotBeActivated, implementationType2, serviceType));
				}
			}
			Type key = serviceType;
			_descriptorLookup.TryGetValue(key, out var value);
			_descriptorLookup[key] = value.Add(serviceDescriptor);
		}
	}

	private static void ValidateTrimmingAnnotations(Type serviceType, Type[] serviceTypeGenericArguments, Type implementationType, Type[] implementationTypeGenericArguments)
	{
		for (int i = 0; i < serviceTypeGenericArguments.Length; i++)
		{
			Type type = serviceTypeGenericArguments[i];
			Type type2 = implementationTypeGenericArguments[i];
			DynamicallyAccessedMemberTypes dynamicallyAccessedMemberTypes = GetDynamicallyAccessedMemberTypes(type);
			DynamicallyAccessedMemberTypes dynamicallyAccessedMemberTypes2 = GetDynamicallyAccessedMemberTypes(type2);
			if (!AreCompatible(dynamicallyAccessedMemberTypes, dynamicallyAccessedMemberTypes2))
			{
				throw new ArgumentException(System.SR.Format(System.SR.TrimmingAnnotationsDoNotMatch, implementationType.FullName, serviceType.FullName));
			}
			bool flag = type.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);
			if (type2.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint) && !flag)
			{
				throw new ArgumentException(System.SR.Format(System.SR.TrimmingAnnotationsDoNotMatch_NewConstraint, implementationType.FullName, serviceType.FullName));
			}
		}
	}

	private static DynamicallyAccessedMemberTypes GetDynamicallyAccessedMemberTypes(Type serviceGenericType)
	{
		foreach (CustomAttributeData customAttributesDatum in serviceGenericType.GetCustomAttributesData())
		{
			if (customAttributesDatum.AttributeType.FullName == "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute" && customAttributesDatum.ConstructorArguments.Count == 1 && customAttributesDatum.ConstructorArguments[0].ArgumentType.FullName == "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes")
			{
				return (DynamicallyAccessedMemberTypes)(int)customAttributesDatum.ConstructorArguments[0].Value;
			}
		}
		return DynamicallyAccessedMemberTypes.None;
	}

	private static bool AreCompatible(DynamicallyAccessedMemberTypes serviceDynamicallyAccessedMembers, DynamicallyAccessedMemberTypes implementationDynamicallyAccessedMembers)
	{
		return serviceDynamicallyAccessedMembers.HasFlag(implementationDynamicallyAccessedMembers);
	}

	internal int? GetSlot(ServiceDescriptor serviceDescriptor)
	{
		if (_descriptorLookup.TryGetValue(serviceDescriptor.ServiceType, out var value))
		{
			return value.GetSlot(serviceDescriptor);
		}
		return null;
	}

	internal ServiceCallSite? GetCallSite(Type serviceType, CallSiteChain callSiteChain)
	{
		if (!_callSiteCache.TryGetValue(new ServiceCacheKey(serviceType, 0), out var value))
		{
			return CreateCallSite(serviceType, callSiteChain);
		}
		return value;
	}

	internal ServiceCallSite? GetCallSite(ServiceDescriptor serviceDescriptor, CallSiteChain callSiteChain)
	{
		if (_descriptorLookup.TryGetValue(serviceDescriptor.ServiceType, out var value))
		{
			return TryCreateExact(serviceDescriptor, serviceDescriptor.ServiceType, callSiteChain, value.GetSlot(serviceDescriptor));
		}
		return null;
	}

	private ServiceCallSite CreateCallSite(Type serviceType, CallSiteChain callSiteChain)
	{
		if (!_stackGuard.TryEnterOnCurrentStack())
		{
			return _stackGuard.RunOnEmptyStack(CreateCallSite, serviceType, callSiteChain);
		}
		object orAdd = _callSiteLocks.GetOrAdd(serviceType, (Type _) => new object());
		lock (orAdd)
		{
			callSiteChain.CheckCircularDependency(serviceType);
			return TryCreateExact(serviceType, callSiteChain) ?? TryCreateOpenGeneric(serviceType, callSiteChain) ?? TryCreateEnumerable(serviceType, callSiteChain);
		}
	}

	private ServiceCallSite TryCreateExact(Type serviceType, CallSiteChain callSiteChain)
	{
		if (_descriptorLookup.TryGetValue(serviceType, out var value))
		{
			return TryCreateExact(value.Last, serviceType, callSiteChain, 0);
		}
		return null;
	}

	private ServiceCallSite TryCreateOpenGeneric(Type serviceType, CallSiteChain callSiteChain)
	{
		if (serviceType.IsConstructedGenericType && _descriptorLookup.TryGetValue(serviceType.GetGenericTypeDefinition(), out var value))
		{
			return TryCreateOpenGeneric(value.Last, serviceType, callSiteChain, 0, throwOnConstraintViolation: true);
		}
		return null;
	}

	private ServiceCallSite TryCreateEnumerable(Type serviceType, CallSiteChain callSiteChain)
	{
		ServiceCacheKey serviceCacheKey = new ServiceCacheKey(serviceType, 0);
		if (_callSiteCache.TryGetValue(serviceCacheKey, out var value))
		{
			return value;
		}
		ServiceCallSite serviceCallSite4;
		try
		{
			callSiteChain.Add(serviceType);
			if (serviceType.IsConstructedGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				Type type = serviceType.GenericTypeArguments[0];
				CallSiteResultCacheLocation callSiteResultCacheLocation = CallSiteResultCacheLocation.Root;
				List<ServiceCallSite> list = new List<ServiceCallSite>();
				if (!type.IsConstructedGenericType && _descriptorLookup.TryGetValue(type, out var value2))
				{
					for (int i = 0; i < value2.Count; i++)
					{
						ServiceDescriptor descriptor = value2[i];
						int slot = value2.Count - i - 1;
						ServiceCallSite serviceCallSite = TryCreateExact(descriptor, type, callSiteChain, slot);
						callSiteResultCacheLocation = GetCommonCacheLocation(callSiteResultCacheLocation, serviceCallSite.Cache.Location);
						list.Add(serviceCallSite);
					}
				}
				else
				{
					int num = 0;
					for (int num2 = _descriptors.Length - 1; num2 >= 0; num2--)
					{
						ServiceDescriptor descriptor2 = _descriptors[num2];
						ServiceCallSite serviceCallSite2 = TryCreateExact(descriptor2, type, callSiteChain, num) ?? TryCreateOpenGeneric(descriptor2, type, callSiteChain, num, throwOnConstraintViolation: false);
						if (serviceCallSite2 != null)
						{
							num++;
							callSiteResultCacheLocation = GetCommonCacheLocation(callSiteResultCacheLocation, serviceCallSite2.Cache.Location);
							list.Add(serviceCallSite2);
						}
					}
					list.Reverse();
				}
				ResultCache cache = ResultCache.None;
				if (callSiteResultCacheLocation == CallSiteResultCacheLocation.Scope || callSiteResultCacheLocation == CallSiteResultCacheLocation.Root)
				{
					cache = new ResultCache(callSiteResultCacheLocation, serviceCacheKey);
				}
				serviceCallSite4 = (_callSiteCache[serviceCacheKey] = new IEnumerableCallSite(cache, type, list.ToArray()));
				serviceCallSite4 = serviceCallSite4;
			}
			else
			{
				serviceCallSite4 = null;
			}
		}
		finally
		{
			callSiteChain.Remove(serviceType);
		}
		return serviceCallSite4;
	}

	private static CallSiteResultCacheLocation GetCommonCacheLocation(CallSiteResultCacheLocation locationA, CallSiteResultCacheLocation locationB)
	{
		return (CallSiteResultCacheLocation)Math.Max((int)locationA, (int)locationB);
	}

	private ServiceCallSite TryCreateExact(ServiceDescriptor descriptor, Type serviceType, CallSiteChain callSiteChain, int slot)
	{
		if (serviceType == descriptor.ServiceType)
		{
			ServiceCacheKey key = new ServiceCacheKey(serviceType, slot);
			if (_callSiteCache.TryGetValue(key, out var value))
			{
				return value;
			}
			ResultCache resultCache = new ResultCache(descriptor.Lifetime, serviceType, slot);
			ServiceCallSite serviceCallSite;
			if (descriptor.ImplementationInstance != null)
			{
				serviceCallSite = new ConstantCallSite(descriptor.ServiceType, descriptor.ImplementationInstance);
			}
			else if (descriptor.ImplementationFactory != null)
			{
				serviceCallSite = new FactoryCallSite(resultCache, descriptor.ServiceType, descriptor.ImplementationFactory);
			}
			else
			{
				if (!(descriptor.ImplementationType != null))
				{
					throw new InvalidOperationException(System.SR.InvalidServiceDescriptor);
				}
				serviceCallSite = CreateConstructorCallSite(resultCache, descriptor.ServiceType, descriptor.ImplementationType, callSiteChain);
			}
			return _callSiteCache[key] = serviceCallSite;
		}
		return null;
	}

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2055:MakeGenericType", Justification = "MakeGenericType here is used to create a closed generic implementation type given the closed service type. Trimming annotations on the generic types are verified when 'Microsoft.Extensions.DependencyInjection.VerifyOpenGenericServiceTrimmability' is set, which is set by default when PublishTrimmed=true. That check informs developers when these generic types don't have compatible trimming annotations.")]
	private ServiceCallSite TryCreateOpenGeneric(ServiceDescriptor descriptor, Type serviceType, CallSiteChain callSiteChain, int slot, bool throwOnConstraintViolation)
	{
		if (serviceType.IsConstructedGenericType && serviceType.GetGenericTypeDefinition() == descriptor.ServiceType)
		{
			ServiceCacheKey key = new ServiceCacheKey(serviceType, slot);
			if (_callSiteCache.TryGetValue(key, out var value))
			{
				return value;
			}
			ResultCache lifetime = new ResultCache(descriptor.Lifetime, serviceType, slot);
			Type implementationType;
			try
			{
				implementationType = descriptor.ImplementationType.MakeGenericType(serviceType.GenericTypeArguments);
			}
			catch (ArgumentException)
			{
				if (throwOnConstraintViolation)
				{
					throw;
				}
				return null;
			}
			return _callSiteCache[key] = CreateConstructorCallSite(lifetime, serviceType, implementationType, callSiteChain);
		}
		return null;
	}

	private ServiceCallSite CreateConstructorCallSite(ResultCache lifetime, Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, CallSiteChain callSiteChain)
	{
		try
		{
			callSiteChain.Add(serviceType, implementationType);
			ConstructorInfo[] constructors = implementationType.GetConstructors();
			ServiceCallSite[] parameterCallSites = null;
			if (constructors.Length == 0)
			{
				throw new InvalidOperationException(System.SR.Format(System.SR.NoConstructorMatch, implementationType));
			}
			if (constructors.Length == 1)
			{
				ConstructorInfo constructorInfo = constructors[0];
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				if (parameters.Length == 0)
				{
					return new ConstructorCallSite(lifetime, serviceType, constructorInfo);
				}
				parameterCallSites = CreateArgumentCallSites(implementationType, callSiteChain, parameters, throwIfCallSiteNotFound: true);
				return new ConstructorCallSite(lifetime, serviceType, constructorInfo, parameterCallSites);
			}
			Array.Sort(constructors, (ConstructorInfo a, ConstructorInfo b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length));
			ConstructorInfo constructorInfo2 = null;
			HashSet<Type> hashSet = null;
			for (int i = 0; i < constructors.Length; i++)
			{
				ParameterInfo[] parameters2 = constructors[i].GetParameters();
				ServiceCallSite[] array = CreateArgumentCallSites(implementationType, callSiteChain, parameters2, throwIfCallSiteNotFound: false);
				if (array == null)
				{
					continue;
				}
				if (constructorInfo2 == null)
				{
					constructorInfo2 = constructors[i];
					parameterCallSites = array;
					continue;
				}
				if (hashSet == null)
				{
					hashSet = new HashSet<Type>();
					ParameterInfo[] parameters3 = constructorInfo2.GetParameters();
					foreach (ParameterInfo parameterInfo in parameters3)
					{
						hashSet.Add(parameterInfo.ParameterType);
					}
				}
				ParameterInfo[] array2 = parameters2;
				foreach (ParameterInfo parameterInfo2 in array2)
				{
					if (!hashSet.Contains(parameterInfo2.ParameterType))
					{
						throw new InvalidOperationException(string.Join(Environment.NewLine, System.SR.Format(System.SR.AmbiguousConstructorException, implementationType), constructorInfo2, constructors[i]));
					}
				}
			}
			if (constructorInfo2 == null)
			{
				throw new InvalidOperationException(System.SR.Format(System.SR.UnableToActivateTypeException, implementationType));
			}
			return new ConstructorCallSite(lifetime, serviceType, constructorInfo2, parameterCallSites);
		}
		finally
		{
			callSiteChain.Remove(serviceType);
		}
	}

	private ServiceCallSite[] CreateArgumentCallSites(Type implementationType, CallSiteChain callSiteChain, ParameterInfo[] parameters, bool throwIfCallSiteNotFound)
	{
		ServiceCallSite[] array = new ServiceCallSite[parameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			Type parameterType = parameters[i].ParameterType;
			ServiceCallSite serviceCallSite = GetCallSite(parameterType, callSiteChain);
			if (serviceCallSite == null && Microsoft.Extensions.Internal.ParameterDefaultValue.TryGetDefaultValue(parameters[i], out object defaultValue))
			{
				serviceCallSite = new ConstantCallSite(parameterType, defaultValue);
			}
			if (serviceCallSite == null)
			{
				if (throwIfCallSiteNotFound)
				{
					throw new InvalidOperationException(System.SR.Format(System.SR.CannotResolveService, parameterType, implementationType));
				}
				return null;
			}
			array[i] = serviceCallSite;
		}
		return array;
	}

	public void Add(Type type, ServiceCallSite serviceCallSite)
	{
		_callSiteCache[new ServiceCacheKey(type, 0)] = serviceCallSite;
	}

	public bool IsService(Type serviceType)
	{
		if ((object)serviceType == null)
		{
			throw new ArgumentNullException("serviceType");
		}
		if (serviceType.IsGenericTypeDefinition)
		{
			return false;
		}
		if (_descriptorLookup.ContainsKey(serviceType))
		{
			return true;
		}
		if (serviceType.IsConstructedGenericType)
		{
			Type genericTypeDefinition = serviceType.GetGenericTypeDefinition();
			if ((object)genericTypeDefinition != null)
			{
				if (!(genericTypeDefinition == typeof(IEnumerable<>)))
				{
					return _descriptorLookup.ContainsKey(genericTypeDefinition);
				}
				return true;
			}
		}
		if (!(serviceType == typeof(IServiceProvider)) && !(serviceType == typeof(IServiceScopeFactory)))
		{
			return serviceType == typeof(IServiceProviderIsService);
		}
		return true;
	}
}
