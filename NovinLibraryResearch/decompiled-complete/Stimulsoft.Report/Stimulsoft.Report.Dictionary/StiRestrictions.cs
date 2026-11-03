using System.Collections;
using System.Collections.Generic;

namespace Stimulsoft.Report.Dictionary;

public sealed class StiRestrictions
{
	private Hashtable restrictionsDataSource;

	private Hashtable restrictionsDataRelation;

	private Hashtable restrictionsDataColumn;

	private Hashtable restrictionsDatabase;

	private Hashtable restrictionsVariable;

	private Hashtable restrictionsTotal;

	private Hashtable restrictionsBusinessObject;

	public void Clear()
	{
		if (restrictionsDataSource != null)
		{
			restrictionsDataSource.Clear();
			restrictionsDataSource = null;
		}
		if (restrictionsDataRelation != null)
		{
			restrictionsDataRelation.Clear();
			restrictionsDataRelation = null;
		}
		if (restrictionsDataColumn != null)
		{
			restrictionsDataColumn.Clear();
			restrictionsDataColumn = null;
		}
		if (restrictionsDatabase != null)
		{
			restrictionsDatabase.Clear();
			restrictionsDatabase = null;
		}
		if (restrictionsVariable != null)
		{
			restrictionsVariable.Clear();
			restrictionsVariable = null;
		}
		if (restrictionsTotal != null)
		{
			restrictionsTotal.Clear();
			restrictionsTotal = null;
		}
		if (restrictionsBusinessObject != null)
		{
			restrictionsBusinessObject.Clear();
			restrictionsBusinessObject = null;
		}
	}

	private Hashtable GetHashtable(StiDataType dataType, bool create)
	{
		switch (dataType)
		{
		case StiDataType.Database:
			if (restrictionsDatabase == null && create)
			{
				restrictionsDatabase = new Hashtable();
			}
			return restrictionsDatabase;
		case StiDataType.DataColumn:
			if (restrictionsDataColumn == null && create)
			{
				restrictionsDataColumn = new Hashtable();
			}
			return restrictionsDataColumn;
		case StiDataType.DataRelation:
			if (restrictionsDataRelation == null && create)
			{
				restrictionsDataRelation = new Hashtable();
			}
			return restrictionsDataRelation;
		case StiDataType.DataSource:
			if (restrictionsDataSource == null && create)
			{
				restrictionsDataSource = new Hashtable();
			}
			return restrictionsDataSource;
		case StiDataType.BusinessObject:
			if (restrictionsBusinessObject == null && create)
			{
				restrictionsBusinessObject = new Hashtable();
			}
			return restrictionsBusinessObject;
		case StiDataType.Total:
			if (restrictionsTotal == null && create)
			{
				restrictionsTotal = new Hashtable();
			}
			return restrictionsTotal;
		default:
			if (restrictionsVariable == null && create)
			{
				restrictionsVariable = new Hashtable();
			}
			return restrictionsVariable;
		}
	}

	public void Add(string name, StiDataType dataType, StiRestrictionTypes type)
	{
		Hashtable hashtable = GetHashtable(dataType, create: true);
		List<StiRestrictionTypes> list = hashtable[name] as List<StiRestrictionTypes>;
		if (list == null)
		{
			list = (List<StiRestrictionTypes>)(hashtable[name] = new List<StiRestrictionTypes>());
		}
		list.Add(type);
	}

	public bool IsAllowEdit(string name, StiDataType dataType)
	{
		Hashtable hashtable = GetHashtable(dataType, create: false);
		if (hashtable == null || hashtable[name] == null)
		{
			return true;
		}
		if (hashtable[name] is List<StiRestrictionTypes> list)
		{
			foreach (StiRestrictionTypes item in list)
			{
				if ((item & StiRestrictionTypes.DenyEdit) != 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsAllowDelete(string name, StiDataType dataType)
	{
		Hashtable hashtable = GetHashtable(dataType, create: false);
		if (hashtable == null || hashtable[name] == null)
		{
			return true;
		}
		if (hashtable[name] is List<StiRestrictionTypes> list)
		{
			foreach (StiRestrictionTypes item in list)
			{
				if ((item & StiRestrictionTypes.DenyDelete) != 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsAllowShow(string name, StiDataType dataType)
	{
		Hashtable hashtable = GetHashtable(dataType, create: false);
		if (hashtable == null || hashtable[name] == null)
		{
			return true;
		}
		if (hashtable[name] is List<StiRestrictionTypes> list)
		{
			foreach (StiRestrictionTypes item in list)
			{
				if ((item & StiRestrictionTypes.DenyShow) != 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsAllowMove(string name, StiDataType dataType)
	{
		Hashtable hashtable = GetHashtable(dataType, create: false);
		if (hashtable == null || hashtable[name] == null)
		{
			return true;
		}
		if (hashtable[name] is List<StiRestrictionTypes> list)
		{
			foreach (StiRestrictionTypes item in list)
			{
				if ((item & StiRestrictionTypes.DenyMove) != 0)
				{
					return false;
				}
			}
		}
		return true;
	}
}
