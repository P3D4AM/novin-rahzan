using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public static class MetadataProviderUtils
{
	public static class Names
	{
		public static class Database
		{
			public const string Master = "master";

			public const string Model = "model";

			public const string Msdb = "msdb";

			public const string TempDb = "tempdb";
		}

		public static class Schema
		{
			public const string Dbo = "dbo";

			public const string InformationSchema = "INFORMATION_SCHEMA";

			public const string Sys = "sys";
		}
	}

	public static IMetadataOrderedCollection<IParameter> GetStoredProcParameters(string storedProcText, IMetadataFactory metadataFactory, ISystemDataTypeLookup dataTypeLookup, CollationInfo collationInfo, ParseOptions parseOptions)
	{
		if (storedProcText == null)
		{
			throw new ArgumentNullException("storedProcText");
		}
		if (metadataFactory == null)
		{
			throw new ArgumentNullException("metadataFactory");
		}
		if (dataTypeLookup == null)
		{
			throw new ArgumentNullException("dataTypeLookup");
		}
		if (collationInfo == null)
		{
			throw new ArgumentNullException("collationInfo");
		}
		if (parseOptions == null)
		{
			throw new ArgumentNullException("parseOptions");
		}
		IMetadataOrderedCollection<IParameter> metadataOrderedCollection = null;
		SqlParameterDeclarationCollection sqlParameterDeclarationCollection = ParseUtils.RetrieveStoredProcParameters(Microsoft.SqlServer.Management.SqlParser.Parser.Parser.Parse(storedProcText, parseOptions));
		if (sqlParameterDeclarationCollection != null)
		{
			IParameter[] array = new IParameter[sqlParameterDeclarationCollection.Count];
			metadataOrderedCollection = new ParameterCollection(sqlParameterDeclarationCollection.Count, collationInfo);
			for (int i = 0; i < sqlParameterDeclarationCollection.Count; i++)
			{
				SqlParameterDeclaration sqlParameterDeclaration = sqlParameterDeclarationCollection[i];
				SqlDataTypeSpecification type = sqlParameterDeclaration.Type;
				if (type.IsCursor)
				{
					array[i] = metadataFactory.Parameter.CreateCursorParameter(sqlParameterDeclaration.Name);
					continue;
				}
				IDataType dataType = null;
				DataTypeSpec typeSpec = type.DataType.GetTypeSpec();
				int? argument = type.Argument1;
				int? argument2 = type.Argument2;
				bool isMaximum = type.IsMaximum;
				dataType = ((!argument.HasValue) ? dataTypeLookup.Find(typeSpec, isMaximum) : (argument2.HasValue ? dataTypeLookup.Find(typeSpec, argument.Value, argument2.Value) : dataTypeLookup.Find(typeSpec, argument.Value)));
				string defaultValue = ((sqlParameterDeclaration.Value != null) ? sqlParameterDeclaration.Value.Sql : null);
				if (dataType.IsScalar)
				{
					array[i] = metadataFactory.Parameter.CreateScalarParameter(sqlParameterDeclaration.Name, dataType as IScalarDataType, sqlParameterDeclaration.IsOutput, defaultValue);
				}
				else if (dataType.IsTable)
				{
					array[i] = metadataFactory.Parameter.CreateTableParameter(sqlParameterDeclaration.Name, dataType as ITableDataType);
				}
			}
			return Collection<IParameter>.CreateOrderedCollection(collationInfo, array);
		}
		return Collection<IParameter>.EmptyOrdered;
	}
}
