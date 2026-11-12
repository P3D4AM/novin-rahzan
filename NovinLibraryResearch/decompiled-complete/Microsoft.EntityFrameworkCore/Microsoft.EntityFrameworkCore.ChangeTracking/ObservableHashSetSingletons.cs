using System;
using System.ComponentModel;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

internal static class ObservableHashSetSingletons
{
	public static readonly PropertyChangedEventArgs _countPropertyChanged = new PropertyChangedEventArgs("Count");

	public static readonly PropertyChangingEventArgs _countPropertyChanging = new PropertyChangingEventArgs("Count");

	public static readonly object[] _noItems = Array.Empty<object>();
}
