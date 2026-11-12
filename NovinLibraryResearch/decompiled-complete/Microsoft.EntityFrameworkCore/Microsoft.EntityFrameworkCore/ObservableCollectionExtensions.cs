using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class ObservableCollectionExtensions
{
	public static BindingList<T> ToBindingList<T>([JetBrains.Annotations.NotNull] this ObservableCollection<T> source) where T : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return new ObservableBackedBindingList<T>(source);
	}
}
