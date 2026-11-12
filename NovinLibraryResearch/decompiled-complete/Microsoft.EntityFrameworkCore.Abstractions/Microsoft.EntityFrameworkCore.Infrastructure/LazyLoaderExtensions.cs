using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class LazyLoaderExtensions
{
	public static TRelated Load<TRelated>([CanBeNull] this ILazyLoader loader, [NotNull] object entity, [CanBeNull] ref TRelated navigationField, [NotNull][CallerMemberName] string navigationName = null) where TRelated : class
	{
		loader?.Load(entity, navigationName);
		return navigationField;
	}
}
