using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public abstract class DbContextOptions : IDbContextOptions
{
	private readonly IReadOnlyDictionary<Type, IDbContextOptionsExtension> _extensions;

	public virtual IEnumerable<IDbContextOptionsExtension> Extensions => _extensions.Values;

	public abstract Type ContextType { get; }

	public virtual bool IsFrozen { get; private set; }

	protected DbContextOptions([JetBrains.Annotations.NotNull] IReadOnlyDictionary<Type, IDbContextOptionsExtension> extensions)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(extensions, "extensions");
		_extensions = extensions;
	}

	public virtual TExtension FindExtension<TExtension>() where TExtension : class, IDbContextOptionsExtension
	{
		if (!_extensions.TryGetValue(typeof(TExtension), out var value))
		{
			return null;
		}
		return (TExtension)value;
	}

	public virtual TExtension GetExtension<TExtension>() where TExtension : class, IDbContextOptionsExtension
	{
		return FindExtension<TExtension>() ?? throw new InvalidOperationException(CoreStrings.OptionsExtensionNotFound(typeof(TExtension).ShortDisplayName()));
	}

	public abstract DbContextOptions WithExtension<TExtension>([JetBrains.Annotations.NotNull] TExtension extension) where TExtension : class, IDbContextOptionsExtension;

	public virtual void Freeze()
	{
		IsFrozen = true;
	}
}
public class DbContextOptions<TContext> : DbContextOptions where TContext : DbContext
{
	public override Type ContextType => typeof(TContext);

	public DbContextOptions()
		: base(new Dictionary<Type, IDbContextOptionsExtension>())
	{
	}

	public DbContextOptions([JetBrains.Annotations.NotNull] IReadOnlyDictionary<Type, IDbContextOptionsExtension> extensions)
		: base(extensions)
	{
	}

	public override DbContextOptions WithExtension<TExtension>(TExtension extension)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(extension, "extension");
		Dictionary<Type, IDbContextOptionsExtension> dictionary = Extensions.ToDictionary((IDbContextOptionsExtension p) => p.GetType(), (IDbContextOptionsExtension p) => p);
		dictionary[typeof(TExtension)] = extension;
		return new DbContextOptions<TContext>(dictionary);
	}
}
