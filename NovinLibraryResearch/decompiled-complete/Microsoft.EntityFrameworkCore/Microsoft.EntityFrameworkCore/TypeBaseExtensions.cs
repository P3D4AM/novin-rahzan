using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class TypeBaseExtensions
{
	public static PropertyAccessMode GetPropertyAccessMode([JetBrains.Annotations.NotNull] this ITypeBase typeBase)
	{
		return ((PropertyAccessMode?)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeBase, "typeBase")["PropertyAccessMode"]) ?? typeBase.Model.GetPropertyAccessMode();
	}

	public static PropertyAccessMode GetNavigationAccessMode([JetBrains.Annotations.NotNull] this ITypeBase typeBase)
	{
		return ((PropertyAccessMode?)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeBase, "typeBase")["NavigationAccessMode"]) ?? typeBase.GetPropertyAccessMode();
	}
}
