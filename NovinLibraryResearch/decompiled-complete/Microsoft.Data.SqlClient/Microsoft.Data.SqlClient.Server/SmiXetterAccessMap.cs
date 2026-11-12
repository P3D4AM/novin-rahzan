namespace Microsoft.Data.SqlClient.Server;

internal class SmiXetterAccessMap
{
	private const bool X = true;

	private const bool _ = false;

	private static bool[,] __isGetterAccessValid = new bool[35, 17]
	{
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, true,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, false, true, true
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, true, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, true, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, true
		}
	};

	private static bool[,] __isSetterAccessValid = new bool[35, 17]
	{
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, true,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, false, true, true
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, true, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, true
		}
	};

	internal static bool IsGetterAccessValid(SmiMetaData metaData, SmiXetterTypeCode xetterType)
	{
		return __isGetterAccessValid[(int)metaData.SqlDbType, (int)xetterType];
	}

	internal static bool IsSetterAccessValid(SmiMetaData metaData, SmiXetterTypeCode xetterType)
	{
		return __isSetterAccessValid[(int)metaData.SqlDbType, (int)xetterType];
	}
}
