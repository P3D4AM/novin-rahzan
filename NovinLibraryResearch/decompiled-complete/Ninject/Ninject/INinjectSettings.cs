using System;
using Ninject.Activation;

namespace Ninject;

public interface INinjectSettings
{
	Type InjectAttribute { get; }

	TimeSpan CachePruningInterval { get; }

	Func<IContext, object> DefaultScopeCallback { get; }

	bool LoadExtensions { get; }

	string[] ExtensionSearchPatterns { get; }

	bool UseReflectionBasedInjection { get; }

	bool InjectNonPublic { get; set; }

	bool InjectParentPrivateProperties { get; set; }

	bool ActivationCacheDisabled { get; set; }

	bool AllowNullInjection { get; set; }

	bool ThrowOnGetServiceNotFound { get; set; }

	T Get<T>(string key, T defaultValue);

	void Set(string key, object value);
}
