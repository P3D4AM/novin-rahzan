using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

namespace Ninject;

public static class ResolutionExtensions
{
	public static T Get<T>(this IResolutionRoot root, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, typeof(T), null, parameters, isOptional: false, isUnique: true).Cast<T>().Single();
	}

	public static T Get<T>(this IResolutionRoot root, string name, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, typeof(T), (IBindingMetadata b) => b.Name == name, parameters, isOptional: false, isUnique: true).Cast<T>().Single();
	}

	public static T Get<T>(this IResolutionRoot root, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, typeof(T), constraint, parameters, isOptional: false, isUnique: true).Cast<T>().Single();
	}

	public static T TryGet<T>(this IResolutionRoot root, params IParameter[] parameters)
	{
		return TryGet(() => GetResolutionIterator(root, typeof(T), null, parameters, isOptional: true, isUnique: true).Cast<T>());
	}

	public static T TryGet<T>(this IResolutionRoot root, string name, params IParameter[] parameters)
	{
		return TryGet(() => GetResolutionIterator(root, typeof(T), (IBindingMetadata b) => b.Name == name, parameters, isOptional: true, isUnique: true).Cast<T>());
	}

	public static T TryGet<T>(this IResolutionRoot root, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return TryGet(() => GetResolutionIterator(root, typeof(T), constraint, parameters, isOptional: true, isUnique: true).Cast<T>());
	}

	public static T TryGetAndThrowOnInvalidBinding<T>(this IResolutionRoot root, params IParameter[] parameters)
	{
		return DoTryGetAndThrowOnInvalidBinding<T>(root, null, parameters);
	}

	public static T TryGetAndThrowOnInvalidBinding<T>(this IResolutionRoot root, string name, params IParameter[] parameters)
	{
		return DoTryGetAndThrowOnInvalidBinding<T>(root, (IBindingMetadata b) => b.Name == name, parameters);
	}

	public static T TryGetAndThrowOnInvalidBinding<T>(this IResolutionRoot root, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return DoTryGetAndThrowOnInvalidBinding<T>(root, constraint, parameters);
	}

	public static IEnumerable<T> GetAll<T>(this IResolutionRoot root, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, typeof(T), null, parameters, isOptional: true, isUnique: false).Cast<T>();
	}

	public static IEnumerable<T> GetAll<T>(this IResolutionRoot root, string name, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, typeof(T), (IBindingMetadata b) => b.Name == name, parameters, isOptional: true, isUnique: false).Cast<T>();
	}

	public static IEnumerable<T> GetAll<T>(this IResolutionRoot root, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, typeof(T), constraint, parameters, isOptional: true, isUnique: false).Cast<T>();
	}

	public static object Get(this IResolutionRoot root, Type service, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, service, null, parameters, isOptional: false, isUnique: true).Single();
	}

	public static object Get(this IResolutionRoot root, Type service, string name, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, service, (IBindingMetadata b) => b.Name == name, parameters, isOptional: false, isUnique: true).Single();
	}

	public static object Get(this IResolutionRoot root, Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, service, constraint, parameters, isOptional: false, isUnique: true).Single();
	}

	public static object TryGet(this IResolutionRoot root, Type service, params IParameter[] parameters)
	{
		return TryGet(() => GetResolutionIterator(root, service, null, parameters, isOptional: true, isUnique: true));
	}

	public static object TryGet(this IResolutionRoot root, Type service, string name, params IParameter[] parameters)
	{
		return TryGet(() => GetResolutionIterator(root, service, (IBindingMetadata b) => b.Name == name, parameters, isOptional: true, isUnique: true));
	}

	public static object TryGet(this IResolutionRoot root, Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return TryGet(() => GetResolutionIterator(root, service, constraint, parameters, isOptional: true, isUnique: true));
	}

	public static IEnumerable<object> GetAll(this IResolutionRoot root, Type service, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, service, null, parameters, isOptional: true, isUnique: false);
	}

	public static IEnumerable<object> GetAll(this IResolutionRoot root, Type service, string name, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, service, (IBindingMetadata b) => b.Name == name, parameters, isOptional: true, isUnique: false);
	}

	public static IEnumerable<object> GetAll(this IResolutionRoot root, Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return GetResolutionIterator(root, service, constraint, parameters, isOptional: true, isUnique: false);
	}

	public static bool CanResolve<T>(this IResolutionRoot root, params IParameter[] parameters)
	{
		return CanResolve(root, typeof(T), null, parameters, isOptional: false, isUnique: true);
	}

	public static bool CanResolve<T>(this IResolutionRoot root, string name, params IParameter[] parameters)
	{
		return CanResolve(root, typeof(T), (IBindingMetadata b) => b.Name == name, parameters, isOptional: false, isUnique: true);
	}

	public static bool CanResolve<T>(this IResolutionRoot root, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return CanResolve(root, typeof(T), constraint, parameters, isOptional: false, isUnique: true);
	}

	public static bool CanResolve(this IResolutionRoot root, Type service, params IParameter[] parameters)
	{
		return CanResolve(root, service, null, parameters, isOptional: false, isUnique: true);
	}

	public static bool CanResolve(this IResolutionRoot root, Type service, string name, params IParameter[] parameters)
	{
		return CanResolve(root, service, (IBindingMetadata b) => b.Name == name, parameters, isOptional: false, isUnique: true);
	}

	public static bool CanResolve(this IResolutionRoot root, Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
	{
		return CanResolve(root, service, constraint, parameters, isOptional: false, isUnique: true);
	}

	private static bool CanResolve(IResolutionRoot root, Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique)
	{
		Ensure.ArgumentNotNull(root, "root");
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(parameters, "parameters");
		IRequest request = root.CreateRequest(service, constraint, parameters, isOptional, isUnique);
		return root.CanResolve(request);
	}

	private static IEnumerable<object> GetResolutionIterator(IResolutionRoot root, Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique)
	{
		Ensure.ArgumentNotNull(root, "root");
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(parameters, "parameters");
		IRequest request = root.CreateRequest(service, constraint, parameters, isOptional, isUnique);
		return root.Resolve(request);
	}

	private static IEnumerable<object> GetResolutionIterator(IResolutionRoot root, Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique, bool forceUnique)
	{
		Ensure.ArgumentNotNull(root, "root");
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(parameters, "parameters");
		IRequest request = root.CreateRequest(service, constraint, parameters, isOptional, isUnique);
		request.ForceUnique = forceUnique;
		return root.Resolve(request);
	}

	private static T TryGet<T>(Func<IEnumerable<T>> iterator)
	{
		try
		{
			return iterator().SingleOrDefault();
		}
		catch (ActivationException)
		{
			return default(T);
		}
	}

	private static T DoTryGetAndThrowOnInvalidBinding<T>(IResolutionRoot root, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters)
	{
		return GetResolutionIterator(root, typeof(T), constraint, parameters, isOptional: true, isUnique: true, forceUnique: true).Cast<T>().SingleOrDefault();
	}
}
