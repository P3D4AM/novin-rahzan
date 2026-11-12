using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal struct UserDefinedTypeUnion<R, S, T, U> where R : class, IUserDefinedDataType, U where S : class, IUserDefinedTableType, U where T : class, IUserDefinedClrType, U where U : class, IUserDefinedType
{
	public readonly R UserDefinedDataType;

	public readonly S UserDefinedTableType;

	public readonly T UserDefinedClrType;

	public bool IsNull
	{
		get
		{
			if (UserDefinedDataType == null && UserDefinedTableType == null)
			{
				return UserDefinedClrType == null;
			}
			return false;
		}
	}

	public bool IsUserDefinedDataType => UserDefinedDataType != null;

	public bool IsUserDefinedTableType => UserDefinedTableType != null;

	public bool IsUserDefinedClrType => UserDefinedClrType != null;

	public U AsUserDefinedType
	{
		get
		{
			if (UserDefinedDataType != null)
			{
				return (U)UserDefinedDataType;
			}
			if (UserDefinedTableType != null)
			{
				return (U)UserDefinedTableType;
			}
			return (U)UserDefinedClrType;
		}
	}

	public UserDefinedTypeUnion(R userDefinedDataType)
	{
		UserDefinedDataType = userDefinedDataType;
		UserDefinedTableType = null;
		UserDefinedClrType = null;
	}

	public UserDefinedTypeUnion(S userDefinedTableType)
	{
		UserDefinedDataType = null;
		UserDefinedTableType = userDefinedTableType;
		UserDefinedClrType = null;
	}

	public UserDefinedTypeUnion(T userDefinedClrType)
	{
		UserDefinedDataType = null;
		UserDefinedTableType = null;
		UserDefinedClrType = userDefinedClrType;
	}
}
