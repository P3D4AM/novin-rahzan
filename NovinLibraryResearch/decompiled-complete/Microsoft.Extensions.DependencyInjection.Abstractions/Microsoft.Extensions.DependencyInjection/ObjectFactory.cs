using System;

namespace Microsoft.Extensions.DependencyInjection;

public delegate object ObjectFactory(IServiceProvider serviceProvider, object?[]? arguments);
public delegate T ObjectFactory<T>(IServiceProvider serviceProvider, object?[]? arguments);
