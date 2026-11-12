using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

[Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ListBindable(false)]
public sealed class SqlParameterCollection : DbParameterCollection
{
	private List<SqlParameter> _items;

	private bool _isDirty;

	public new SqlParameter this[int index]
	{
		get
		{
			return (SqlParameter)GetParameter(index);
		}
		set
		{
			SetParameter(index, value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new SqlParameter this[string parameterName]
	{
		get
		{
			return (SqlParameter)GetParameter(parameterName);
		}
		set
		{
			SetParameter(parameterName, value);
		}
	}

	public override int Count => _items?.Count ?? 0;

	public override bool IsFixedSize => ((IList)InnerList).IsFixedSize;

	public override bool IsReadOnly => ((IList)InnerList).IsReadOnly;

	public override bool IsSynchronized => ((ICollection)InnerList).IsSynchronized;

	public override object SyncRoot => ((ICollection)InnerList).SyncRoot;

	private List<SqlParameter> InnerList
	{
		get
		{
			List<SqlParameter> list = _items;
			if (list == null)
			{
				list = (_items = new List<SqlParameter>());
			}
			return list;
		}
	}

	internal bool IsDirty
	{
		get
		{
			return _isDirty;
		}
		set
		{
			_isDirty = value;
		}
	}

	internal SqlParameterCollection()
	{
	}

	public SqlParameter Add(SqlParameter value)
	{
		Add((object)value);
		return value;
	}

	public SqlParameter AddWithValue(string parameterName, object value)
	{
		return Add(new SqlParameter(parameterName, value));
	}

	public SqlParameter Add(string parameterName, SqlDbType sqlDbType)
	{
		return Add(new SqlParameter(parameterName, sqlDbType));
	}

	public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size)
	{
		return Add(new SqlParameter(parameterName, sqlDbType, size));
	}

	public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, string sourceColumn)
	{
		return Add(new SqlParameter(parameterName, sqlDbType, size, sourceColumn));
	}

	public void AddRange(SqlParameter[] values)
	{
		AddRange((Array)values);
	}

	public override bool Contains(string value)
	{
		return IndexOf(value) != -1;
	}

	public bool Contains(SqlParameter value)
	{
		return IndexOf(value) != -1;
	}

	public void CopyTo(SqlParameter[] array, int index)
	{
		CopyTo((Array)array, index);
	}

	public int IndexOf(SqlParameter value)
	{
		return IndexOf((object)value);
	}

	public void Insert(int index, SqlParameter value)
	{
		Insert(index, (object)value);
	}

	public void Remove(SqlParameter value)
	{
		Remove((object)value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int Add(object value)
	{
		IsDirty = true;
		ValidateType(value);
		Validate(-1, value);
		InnerList.Add((SqlParameter)value);
		return Count - 1;
	}

	public override void AddRange(Array values)
	{
		IsDirty = true;
		if (values == null)
		{
			throw ADP.ArgumentNull("values");
		}
		foreach (object value in values)
		{
			ValidateType(value);
		}
		foreach (SqlParameter value2 in values)
		{
			Validate(-1, value2);
			InnerList.Add(value2);
		}
	}

	public override void Clear()
	{
		IsDirty = true;
		List<SqlParameter> innerList = InnerList;
		if (innerList == null)
		{
			return;
		}
		foreach (SqlParameter item in innerList)
		{
			item.ResetParent();
		}
		innerList.Clear();
	}

	public override bool Contains(object value)
	{
		return IndexOf(value) != -1;
	}

	public override void CopyTo(Array array, int index)
	{
		((ICollection)InnerList).CopyTo(array, index);
	}

	public override IEnumerator GetEnumerator()
	{
		return ((IEnumerable)InnerList).GetEnumerator();
	}

	public override int IndexOf(string parameterName)
	{
		List<SqlParameter> innerList = InnerList;
		if (innerList != null)
		{
			int num = 0;
			foreach (SqlParameter item in innerList)
			{
				if (parameterName == item.ParameterName)
				{
					return num;
				}
				num++;
			}
			num = 0;
			foreach (SqlParameter item2 in innerList)
			{
				if (CultureInfo.CurrentCulture.CompareInfo.Compare(parameterName, item2.ParameterName, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
				{
					return num;
				}
				num++;
			}
		}
		return -1;
	}

	public override int IndexOf(object value)
	{
		if (value != null)
		{
			ValidateType(value);
			List<SqlParameter> innerList = InnerList;
			if (innerList != null)
			{
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					if (value == innerList[i])
					{
						return i;
					}
				}
			}
		}
		return -1;
	}

	public override void Insert(int index, object value)
	{
		IsDirty = true;
		ValidateType(value);
		Validate(-1, (SqlParameter)value);
		InnerList.Insert(index, (SqlParameter)value);
	}

	public override void Remove(object value)
	{
		IsDirty = true;
		ValidateType(value);
		int num = IndexOf(value);
		if (num != -1)
		{
			RemoveIndex(num);
		}
		else if (((SqlParameter)value).CompareExchangeParent(null, this) != this)
		{
			throw ADP.CollectionRemoveInvalidObject(typeof(SqlParameter), this);
		}
	}

	public override void RemoveAt(int index)
	{
		IsDirty = true;
		RangeCheck(index);
		RemoveIndex(index);
	}

	public override void RemoveAt(string parameterName)
	{
		IsDirty = true;
		int index = CheckName(parameterName);
		RemoveIndex(index);
	}

	protected override void SetParameter(int index, DbParameter value)
	{
		IsDirty = true;
		RangeCheck(index);
		Replace(index, value);
	}

	protected override void SetParameter(string parameterName, DbParameter value)
	{
		IsDirty = true;
		int num = IndexOf(parameterName);
		if (num < 0)
		{
			throw ADP.ParametersSourceIndex(parameterName, this, typeof(SqlParameter));
		}
		Replace(num, value);
	}

	protected override DbParameter GetParameter(int index)
	{
		RangeCheck(index);
		return InnerList[index];
	}

	protected override DbParameter GetParameter(string parameterName)
	{
		int num = IndexOf(parameterName);
		if (num < 0)
		{
			throw ADP.ParametersSourceIndex(parameterName, this, typeof(SqlParameter));
		}
		return InnerList[num];
	}

	private void RangeCheck(int index)
	{
		if (index < 0 || Count <= index)
		{
			throw ADP.ParametersMappingIndex(index, this);
		}
	}

	private int CheckName(string parameterName)
	{
		int num = IndexOf(parameterName);
		if (num < 0)
		{
			throw ADP.ParametersSourceIndex(parameterName, this, typeof(SqlParameter));
		}
		return num;
	}

	private void RemoveIndex(int index)
	{
		List<SqlParameter> innerList = InnerList;
		SqlParameter sqlParameter = innerList[index];
		innerList.RemoveAt(index);
		sqlParameter.ResetParent();
	}

	private void Replace(int index, object newValue)
	{
		List<SqlParameter> innerList = InnerList;
		ValidateType(newValue);
		Validate(index, newValue);
		SqlParameter sqlParameter = innerList[index];
		innerList[index] = (SqlParameter)newValue;
		sqlParameter.ResetParent();
	}

	private void Validate(int index, object value)
	{
		if (value == null)
		{
			throw ADP.ParameterNull("value", this, typeof(SqlParameter));
		}
		object obj = ((SqlParameter)value).CompareExchangeParent(this, null);
		if (obj != null)
		{
			if (this != obj)
			{
				throw ADP.ParametersIsNotParent(typeof(SqlParameter), this);
			}
			if (index != IndexOf(value))
			{
				throw ADP.ParametersIsParent(typeof(SqlParameter), this);
			}
		}
		string parameterName = ((SqlParameter)value).ParameterName;
		if (parameterName.Length == 0)
		{
			index = 1;
			do
			{
				parameterName = "Parameter" + index.ToString(CultureInfo.CurrentCulture);
				index++;
			}
			while (-1 != IndexOf(parameterName));
			((SqlParameter)value).ParameterName = parameterName;
		}
	}

	private void ValidateType(object value)
	{
		if (value == null)
		{
			throw ADP.ParameterNull("value", this, typeof(SqlParameter));
		}
		if (!typeof(SqlParameter).IsInstanceOfType(value))
		{
			throw ADP.InvalidParameterType(this, typeof(SqlParameter), value);
		}
	}
}
