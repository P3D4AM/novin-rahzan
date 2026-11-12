using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Common;

internal class DataTypeLookup
{
	private enum SpecifyNational
	{
		DontCare,
		Yes,
		No
	}

	private enum SpecifyVarying
	{
		DontCare,
		Yes,
		No
	}

	private class LookupSpec
	{
		public readonly DataTypeSpec DataType;

		public readonly SpecifyNational National;

		public readonly SpecifyVarying Varying;

		public LookupSpec(DataTypeSpec dataType, SpecifyNational national, SpecifyVarying varying)
		{
			DataType = dataType;
			National = national;
			Varying = varying;
		}
	}

	private static readonly DataTypeLookup Instance = new DataTypeLookup();

	private readonly Dictionary<StringToken, LookupSpec[]> lookupTable;

	public static DataTypeSpec Find(string name)
	{
		DataTypeErrors error;
		return Find(name, isNational: false, isVarying: false, out error);
	}

	public static DataTypeSpec Find(string name, bool isNational, bool isVarying, out DataTypeErrors error)
	{
		DataTypeSpec dataTypeSpec = null;
		error = DataTypeErrors.None;
		StringToken key = StringTokenLookup.Find(name);
		if (Instance.lookupTable.TryGetValue(key, out var value))
		{
			SpecifyNational specifyNational = (isNational ? SpecifyNational.Yes : SpecifyNational.No);
			SpecifyVarying specifyVarying = (isVarying ? SpecifyVarying.Yes : SpecifyVarying.No);
			foreach (LookupSpec lookupSpec in value)
			{
				if ((lookupSpec.National == SpecifyNational.DontCare || lookupSpec.National == specifyNational) && (lookupSpec.Varying == SpecifyVarying.DontCare || lookupSpec.Varying == specifyVarying))
				{
					dataTypeSpec = lookupSpec.DataType;
					if (isVarying && lookupSpec.Varying == SpecifyVarying.DontCare)
					{
						error |= DataTypeErrors.VaryingNotAllowed;
					}
					if (isNational && lookupSpec.National == SpecifyNational.DontCare)
					{
						error |= DataTypeErrors.NationalNotAllowed;
					}
					break;
				}
			}
			if (dataTypeSpec == null)
			{
				if (isVarying)
				{
					error |= DataTypeErrors.VaryingNotAllowed;
				}
				if (isNational)
				{
					error |= DataTypeErrors.NationalNotAllowed;
				}
			}
		}
		return dataTypeSpec;
	}

	private DataTypeLookup()
	{
		lookupTable = new Dictionary<StringToken, LookupSpec[]>(64);
		lookupTable.Add(StringToken.BigInt, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.BigInt, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Binary, new LookupSpec[2]
		{
			new LookupSpec(DataTypeSpec.Binary, SpecifyNational.DontCare, SpecifyVarying.No),
			new LookupSpec(DataTypeSpec.VarBinary, SpecifyNational.DontCare, SpecifyVarying.Yes)
		});
		lookupTable.Add(StringToken.Bit, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Bit, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Char, new LookupSpec[4]
		{
			new LookupSpec(DataTypeSpec.Char, SpecifyNational.No, SpecifyVarying.No),
			new LookupSpec(DataTypeSpec.VarChar, SpecifyNational.No, SpecifyVarying.Yes),
			new LookupSpec(DataTypeSpec.NChar, SpecifyNational.Yes, SpecifyVarying.No),
			new LookupSpec(DataTypeSpec.NVarChar, SpecifyNational.Yes, SpecifyVarying.Yes)
		});
		lookupTable.Add(StringToken.Character, new LookupSpec[4]
		{
			new LookupSpec(DataTypeSpec.Char, SpecifyNational.No, SpecifyVarying.No),
			new LookupSpec(DataTypeSpec.VarChar, SpecifyNational.No, SpecifyVarying.Yes),
			new LookupSpec(DataTypeSpec.NChar, SpecifyNational.Yes, SpecifyVarying.No),
			new LookupSpec(DataTypeSpec.NVarChar, SpecifyNational.Yes, SpecifyVarying.Yes)
		});
		lookupTable.Add(StringToken.Date, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Date, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.DateTime, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.DateTime, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.DateTime2, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.DateTime2, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.DateTimeOffset, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.DateTimeOffset, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Dec, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Decimal, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Decimal, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Decimal, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Float, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Float, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Image, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Image, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Int, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Int, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Integer, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Int, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Money, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Money, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.NChar, new LookupSpec[2]
		{
			new LookupSpec(DataTypeSpec.NChar, SpecifyNational.DontCare, SpecifyVarying.No),
			new LookupSpec(DataTypeSpec.NVarChar, SpecifyNational.DontCare, SpecifyVarying.Yes)
		});
		lookupTable.Add(StringToken.NCharacter, new LookupSpec[2]
		{
			new LookupSpec(DataTypeSpec.NChar, SpecifyNational.DontCare, SpecifyVarying.No),
			new LookupSpec(DataTypeSpec.NVarChar, SpecifyNational.DontCare, SpecifyVarying.Yes)
		});
		lookupTable.Add(StringToken.NText, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.NText, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Numeric, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Numeric, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.NVarChar, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.NVarChar, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Real, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Real, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.RowVersion, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Timestamp, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.SmallDateTime, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.SmallDateTime, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.SmallInt, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.SmallInt, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.SmallMoney, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.SmallMoney, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.SqlVariant, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Variant, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Text, new LookupSpec[2]
		{
			new LookupSpec(DataTypeSpec.Text, SpecifyNational.No, SpecifyVarying.DontCare),
			new LookupSpec(DataTypeSpec.NText, SpecifyNational.Yes, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Time, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Time, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Timestamp, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Timestamp, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.TinyInt, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.TinyInt, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.UniqueIdentifier, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.UniqueIdentifier, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.VarBinary, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.VarBinary, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.VarChar, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.VarChar, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Xml, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Xml, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Double, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Float, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Geography, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Geography, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.Geometry, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.Geometry, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.HierarchyId, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.HierarchyId, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
		lookupTable.Add(StringToken.SysName, new LookupSpec[1]
		{
			new LookupSpec(DataTypeSpec.SysName, SpecifyNational.DontCare, SpecifyVarying.DontCare)
		});
	}
}
