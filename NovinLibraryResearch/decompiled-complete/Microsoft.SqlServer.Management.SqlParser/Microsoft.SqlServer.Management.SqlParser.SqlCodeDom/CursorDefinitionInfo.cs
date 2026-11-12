using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class CursorDefinitionInfo
{
	private readonly SqlSelectStatement statement;

	private readonly List<SqlCursorOption> extendedCursorOptions;

	private static SqlCursorOptionType[] conflictingOptionSet1 = new SqlCursorOptionType[2]
	{
		SqlCursorOptionType.Global,
		SqlCursorOptionType.Local
	};

	private static SqlCursorOptionType[] conflictingOptionSet2 = new SqlCursorOptionType[2]
	{
		SqlCursorOptionType.ForwardOnly,
		SqlCursorOptionType.Scroll
	};

	private static SqlCursorOptionType[] conflictingOptionSet3 = new SqlCursorOptionType[4]
	{
		SqlCursorOptionType.Static,
		SqlCursorOptionType.Keyset,
		SqlCursorOptionType.Dynamic,
		SqlCursorOptionType.FastForward
	};

	private static SqlCursorOptionType[] conflictingOptionSet4 = new SqlCursorOptionType[3]
	{
		SqlCursorOptionType.ReadOnly,
		SqlCursorOptionType.ScrollLocks,
		SqlCursorOptionType.Optimistic
	};

	public SqlSelectStatement SelectStatement => statement;

	public List<SqlCursorOption> ExtendedCursorOptions => extendedCursorOptions;

	public CursorDefinitionInfo(SqlSelectStatement statement, List<SqlCursorOption> extendedCursorOptions)
	{
		this.statement = statement;
		Dictionary<SqlCursorOptionType, SqlCursorOption> dictionary = new Dictionary<SqlCursorOptionType, SqlCursorOption>();
		foreach (SqlCursorOption extendedCursorOption in extendedCursorOptions)
		{
			if (extendedCursorOption.Type == SqlCursorOptionType.Insensitive)
			{
				extendedCursorOption.ReportParseError(SR.InvalidUsageOfCursorOption(extendedCursorOption.Sql));
			}
			dictionary[extendedCursorOption.Type] = extendedCursorOption;
		}
		ReportConflictingOptions(dictionary, conflictingOptionSet1);
		ReportConflictingOptions(dictionary, conflictingOptionSet2);
		ReportConflictingOptions(dictionary, conflictingOptionSet3);
		ReportConflictingOptions(dictionary, conflictingOptionSet4);
		this.extendedCursorOptions = extendedCursorOptions;
	}

	private void ReportConflictingOptions(Dictionary<SqlCursorOptionType, SqlCursorOption> options, SqlCursorOptionType[] conflictTypes)
	{
		SqlCursorOption[] array = new SqlCursorOption[conflictTypes.Length];
		int num = 0;
		foreach (SqlCursorOptionType key in conflictTypes)
		{
			if (options.TryGetValue(key, out array[num]))
			{
				num++;
			}
		}
		if (num > 1)
		{
			((array[0].StartLocation.CompareTo(array[1].StartLocation) > 0) ? array[0] : array[1]).ReportParseError(SR.ConflictingCursorOption(array[0].Sql, array[1].Sql));
		}
	}
}
