using System;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.Data.SqlClient;

[Serializable]
[ListBindable(false)]
public sealed class SqlErrorCollection : ICollection, IEnumerable
{
	private ArrayList errors = new ArrayList();

	public int Count => errors.Count;

	object ICollection.SyncRoot => this;

	bool ICollection.IsSynchronized => false;

	public SqlError this[int index] => (SqlError)errors[index];

	internal SqlErrorCollection()
	{
	}

	public void CopyTo(Array array, int index)
	{
		errors.CopyTo(array, index);
	}

	public void CopyTo(SqlError[] array, int index)
	{
		errors.CopyTo(array, index);
	}

	public IEnumerator GetEnumerator()
	{
		return errors.GetEnumerator();
	}

	internal void Add(SqlError error)
	{
		errors.Add(error);
	}
}
