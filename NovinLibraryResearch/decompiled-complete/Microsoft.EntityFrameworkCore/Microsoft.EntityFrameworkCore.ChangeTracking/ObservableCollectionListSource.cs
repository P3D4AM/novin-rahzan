using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class ObservableCollectionListSource<T> : ObservableCollection<T>, IListSource where T : class
{
	private IBindingList _bindingList;

	bool IListSource.ContainsListCollection => false;

	public ObservableCollectionListSource()
	{
	}

	public ObservableCollectionListSource([JetBrains.Annotations.NotNull] IEnumerable<T> collection)
		: base(collection)
	{
	}

	public ObservableCollectionListSource([JetBrains.Annotations.NotNull] List<T> list)
		: base(list)
	{
	}

	IList IListSource.GetList()
	{
		return _bindingList ?? (_bindingList = this.ToBindingList());
	}
}
