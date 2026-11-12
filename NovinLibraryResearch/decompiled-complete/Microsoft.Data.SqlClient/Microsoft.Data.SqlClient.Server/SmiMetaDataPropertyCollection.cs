using System.Collections.Generic;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

internal class SmiMetaDataPropertyCollection
{
	private const int SelectorCount = 3;

	private SmiMetaDataProperty[] _properties;

	private bool _isReadOnly;

	internal static readonly SmiMetaDataPropertyCollection EmptyInstance;

	private static readonly SmiDefaultFieldsProperty __emptyDefaultFields;

	private static readonly SmiOrderProperty __emptySortOrder;

	private static readonly SmiUniqueKeyProperty __emptyUniqueKey;

	internal SmiMetaDataProperty this[SmiPropertySelector key]
	{
		get
		{
			return _properties[(int)key];
		}
		set
		{
			if (value == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			EnsureWritable();
			_properties[(int)key] = value;
		}
	}

	internal bool IsReadOnly => _isReadOnly;

	internal IEnumerable<SmiMetaDataProperty> Values => new List<SmiMetaDataProperty>(_properties);

	static SmiMetaDataPropertyCollection()
	{
		__emptyDefaultFields = new SmiDefaultFieldsProperty(new List<bool>());
		__emptySortOrder = new SmiOrderProperty(new List<SmiOrderProperty.SmiColumnOrder>());
		__emptyUniqueKey = new SmiUniqueKeyProperty(new List<bool>());
		EmptyInstance = new SmiMetaDataPropertyCollection();
		EmptyInstance.SetReadOnly();
	}

	internal SmiMetaDataPropertyCollection()
	{
		_properties = new SmiMetaDataProperty[3];
		_isReadOnly = false;
		_properties[0] = __emptyDefaultFields;
		_properties[1] = __emptySortOrder;
		_properties[2] = __emptyUniqueKey;
	}

	internal void SetReadOnly()
	{
		_isReadOnly = true;
	}

	private void EnsureWritable()
	{
		if (IsReadOnly)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
		}
	}
}
