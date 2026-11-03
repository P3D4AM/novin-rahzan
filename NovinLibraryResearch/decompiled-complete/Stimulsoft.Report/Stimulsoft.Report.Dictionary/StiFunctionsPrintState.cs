using System;

namespace Stimulsoft.Report.Dictionary;

public class StiFunctionsPrintState
{
	public static bool IsNull(object dataSource, string dataColumn)
	{
		if (!(dataSource is StiDataSource) && !(dataSource is StiBusinessObject))
		{
			throw new ArgumentNullException("Function IsNull: First argument can't be equal to null. Please provide name of Data Source or name of Business Object.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function IsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		if (dataSource is StiDataSource stiDataSource)
		{
			object obj = stiDataSource[dataColumn];
			if (obj != null)
			{
				return obj == DBNull.Value;
			}
			return true;
		}
		StiBusinessObject stiBusinessObject = dataSource as StiBusinessObject;
		object obj2 = stiBusinessObject[dataColumn];
		if (obj2 != null)
		{
			return obj2 == DBNull.Value;
		}
		return true;
	}

	public static bool NextIsNull(object dataSource, string dataColumn)
	{
		if (!(dataSource is StiDataSource) && !(dataSource is StiBusinessObject))
		{
			throw new ArgumentNullException("Function NextIsNull: First argument can't be equal to null. Please provide name of Data Source or name of Business Object.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function NextIsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		object obj = Next(dataSource, dataColumn);
		if (obj != null)
		{
			return obj == DBNull.Value;
		}
		return true;
	}

	public static bool PreviousIsNull(object dataSource, string dataColumn)
	{
		if (!(dataSource is StiDataSource) && !(dataSource is StiBusinessObject))
		{
			throw new ArgumentNullException("Function PreviousIsNull: First argument can't be equal to null. Please provide name of Data Source or name of Business Object.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function PreviousIsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		object obj = Previous(dataSource, dataColumn);
		if (obj != null)
		{
			return obj == DBNull.Value;
		}
		return true;
	}

	public static object Next(object dataSource, string dataColumn)
	{
		if (!(dataSource is StiDataSource) && !(dataSource is StiBusinessObject))
		{
			throw new ArgumentNullException("Function IsNull: First argument can't be equal to null. Please provide name of Data Source or name of Business Object.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function IsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		StiDataSource stiDataSource = dataSource as StiDataSource;
		StiBusinessObject stiBusinessObject = dataSource as StiBusinessObject;
		string stateName = Guid.NewGuid().ToString();
		stiDataSource?.SaveState(stateName);
		stiBusinessObject?.SaveState(stateName);
		object result = null;
		try
		{
			if (stiDataSource != null)
			{
				stiDataSource.Next();
				if (stiDataSource.IsEof)
				{
					return null;
				}
				result = stiDataSource[dataColumn];
			}
			if (stiBusinessObject != null)
			{
				stiBusinessObject.Next();
				if (stiBusinessObject.IsEof)
				{
					return null;
				}
				result = stiBusinessObject[dataColumn];
			}
		}
		finally
		{
			stiDataSource?.RestoreState(stateName);
			stiBusinessObject?.RestoreState(stateName);
		}
		return result;
	}

	public static object Previous(object dataSource, string dataColumn)
	{
		if (!(dataSource is StiDataSource) && !(dataSource is StiBusinessObject))
		{
			throw new ArgumentNullException("Function IsNull: First argument can't be equal to null. Please provide name of Data Source or name of Business Object.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function IsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		StiDataSource stiDataSource = dataSource as StiDataSource;
		StiBusinessObject stiBusinessObject = dataSource as StiBusinessObject;
		string stateName = Guid.NewGuid().ToString();
		stiDataSource?.SaveState(stateName);
		stiBusinessObject?.SaveState(stateName);
		object result = null;
		try
		{
			if (stiDataSource != null)
			{
				stiDataSource.Prior();
				if (stiDataSource.IsBof)
				{
					return null;
				}
				result = stiDataSource[dataColumn];
			}
			if (stiBusinessObject != null)
			{
				stiBusinessObject.Prior();
				if (stiBusinessObject.IsBof)
				{
					return null;
				}
				result = stiBusinessObject[dataColumn];
			}
		}
		finally
		{
			stiDataSource?.RestoreState(stateName);
			stiBusinessObject?.RestoreState(stateName);
		}
		return result;
	}
}
