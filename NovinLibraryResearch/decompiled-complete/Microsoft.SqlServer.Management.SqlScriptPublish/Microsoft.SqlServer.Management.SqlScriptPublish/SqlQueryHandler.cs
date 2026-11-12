using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Diagnostics;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

internal class SqlQueryHandler
{
	private class QueryInfoCollection : KeyedCollection<DatabaseObjectType, QueryInfo>
	{
		protected override DatabaseObjectType GetKeyForItem(QueryInfo info)
		{
			return info.DatabaseObjectType;
		}
	}

	private class QueryInfo
	{
		[Flags]
		public enum EngineType
		{
			None = 0,
			Singleton = 1,
			Cloud = 2,
			All = 0xFFFF
		}

		[Flags]
		public enum CloudEngineEdition
		{
			None = 0,
			SqlDatabase = 1,
			Synapse = 2,
			OnDemand = 4,
			All = 0xFFFF
		}

		public const int SQL70 = 7;

		public const int SQL80 = 8;

		public const int SQL90 = 9;

		public const int SQL100 = 10;

		public const int SQL110 = 11;

		public const int SQL120 = 12;

		public const int SQL130 = 13;

		public const int SQL140 = 14;

		public const int SQL150 = 15;

		public const int SQL160 = 16;

		private string partialUrn;

		private bool schemaBased;

		private int minimumSingletonVersion = 8;

		private DatabaseObjectType dbObjectType;

		private EngineType validTypes;

		public CloudEngineEdition Editions { get; private set; }

		public Type SmoType { get; private set; }

		public string PartialUrn => partialUrn;

		public bool SchemaBased => schemaBased;

		public DatabaseObjectType DatabaseObjectType => dbObjectType;

		public EngineType ValidEngineTypes => validTypes;

		public OrderBy[] OrderByList
		{
			get
			{
				if (!schemaBased)
				{
					return new OrderBy[1]
					{
						new OrderBy("Name", OrderBy.Direction.Asc)
					};
				}
				return new OrderBy[2]
				{
					new OrderBy("Schema", OrderBy.Direction.Asc),
					new OrderBy("Name", OrderBy.Direction.Asc)
				};
			}
		}

		public string[] Fields
		{
			get
			{
				if (!schemaBased)
				{
					return new string[2] { "Urn", "Name" };
				}
				return new string[3] { "Urn", "Name", "Schema" };
			}
		}

		public QueryInfo(DatabaseObjectType type, Type smoType, string urn)
			: this(type, smoType, urn, schemaBased: true, 8, EngineType.All, CloudEngineEdition.All)
		{
		}

		public QueryInfo(DatabaseObjectType type, Type smoType, string urn, bool schemaBased)
			: this(type, smoType, urn, schemaBased, 8, EngineType.All, CloudEngineEdition.All)
		{
		}

		public QueryInfo(DatabaseObjectType type, Type smoType, string urn, bool schemaBased, int minimumSingletonVersion)
			: this(type, smoType, urn, schemaBased, minimumSingletonVersion, EngineType.All, CloudEngineEdition.All)
		{
		}

		public QueryInfo(DatabaseObjectType type, Type smoType, string urn, bool schemaBased, int minimumSingletonVersion, EngineType types, CloudEngineEdition editions)
		{
			dbObjectType = type;
			partialUrn = urn;
			this.schemaBased = schemaBased;
			this.minimumSingletonVersion = minimumSingletonVersion;
			validTypes = types;
			SmoType = smoType;
			Editions = editions;
		}

		public QueryInfo(DatabaseObjectType type, Type smoType, string urn, bool schemaBased, int minimumSingletonVersion, CloudEngineEdition editions)
		{
			dbObjectType = type;
			partialUrn = urn;
			this.schemaBased = schemaBased;
			this.minimumSingletonVersion = minimumSingletonVersion;
			validTypes = EngineType.All;
			SmoType = smoType;
			Editions = editions;
		}
	}

	private static readonly QueryInfoCollection dicTypes;

	private SqlScriptPublishModel model;

	private Dictionary<DatabaseObjectType, List<KeyValuePair<string, string>>> cache;

	static SqlQueryHandler()
	{
		dicTypes = new QueryInfoCollection();
		InitializeSupportedObjectTypes();
	}

	public SqlQueryHandler(SqlScriptPublishModel model)
	{
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		this.model = model;
		cache = new Dictionary<DatabaseObjectType, List<KeyValuePair<string, string>>>(dicTypes.Count);
	}

	public IEnumerable<DatabaseObjectType> GetDatabaseObjectTypes()
	{
		if (cache.Count == 0)
		{
			foreach (QueryInfo dicType in dicTypes)
			{
				EnumChildrenForDatabaseObjectType(dicType.DatabaseObjectType);
			}
		}
		return cache.Keys;
	}

	public IEnumerable<KeyValuePair<string, string>> EnumChildrenForDatabaseObjectType(DatabaseObjectType objectType)
	{
		List<KeyValuePair<string, string>> value = null;
		if (!cache.TryGetValue(objectType, out value))
		{
			value = new List<KeyValuePair<string, string>>();
			QueryInfo queryInfo = dicTypes[objectType];
			SqlScriptPublishModelTraceHelper.Assert(queryInfo != null, "Unsupported object type:" + objectType);
			if (queryInfo == null)
			{
				throw new SqlScriptPublishException(SR.InvalidObjectType(objectType.ToString()));
			}
			if (ValidObjectType(queryInfo))
			{
				SfcObjectQuery sfcObjectQuery = new SfcObjectQuery(model.Server);
				SfcQueryExpression query = new SfcQueryExpression(string.Format(CultureInfo.InvariantCulture, "Server/Database[@Name='{0}']/{1}", Urn.EscapeString(model.DatabaseName), queryInfo.PartialUrn));
				foreach (DataRow row in sfcObjectQuery.ExecuteDataTable(query, queryInfo.Fields, queryInfo.OrderByList).Rows)
				{
					string text = row["Name"].ToString();
					if (queryInfo.SchemaBased)
					{
						text = row["Schema"].ToString() + "." + text;
					}
					KeyValuePair<string, string> item = new KeyValuePair<string, string>(text, row["Urn"].ToString());
					value.Add(item);
				}
			}
			if (value.Count > 0)
			{
				cache.Add(objectType, value);
			}
		}
		return value;
	}

	public IEnumerable<DatabaseObjectType> InvalidObjectTypesForAzure(DatabaseEngineEdition edition)
	{
		QueryInfo.CloudEngineEdition myEdition = QueryInfo.CloudEngineEdition.SqlDatabase;
		switch (edition)
		{
		case DatabaseEngineEdition.SqlOnDemand:
			myEdition = QueryInfo.CloudEngineEdition.OnDemand;
			break;
		case DatabaseEngineEdition.SqlDataWarehouse:
			myEdition = QueryInfo.CloudEngineEdition.OnDemand;
			break;
		}
		return from d in dicTypes
			where (d.ValidEngineTypes & QueryInfo.EngineType.Cloud) == 0 || (d.Editions & myEdition) == 0
			select d.DatabaseObjectType;
	}

	private bool ValidObjectType(QueryInfo info)
	{
		return model.Server.IsSupportedObject(info.SmoType);
	}

	private static void InitializeSupportedObjectTypes()
	{
		dicTypes.Add(new QueryInfo(DatabaseObjectType.Table, typeof(Table), "Table[@IsSystemObject=false()]"));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.View, typeof(View), "View[@IsSystemObject=false()]"));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.StoredProcedure, typeof(StoredProcedure), "StoredProcedure[@IsSystemObject=false()]"));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.UserDefinedFunction, typeof(UserDefinedFunction), "UserDefinedFunction[@IsSystemObject=false()]"));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.UserDefinedDataType, typeof(UserDefinedDataType), "UserDefinedDataType", schemaBased: true, 8));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.User, typeof(User), "User[@IsSystemObject=false()]", schemaBased: false));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.Default, typeof(Default), "Default", schemaBased: true, 8, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.Rule, typeof(Microsoft.SqlServer.Management.Smo.Rule), "Rule", schemaBased: true, 8, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.DatabaseRole, typeof(DatabaseRole), "Role[@IsFixedRole = false() and @Name != 'public']", schemaBased: false));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.ApplicationRole, typeof(ApplicationRole), "ApplicationRole", schemaBased: false, 8, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.SqlAssembly, typeof(SqlAssembly), "SqlAssembly[@IsSystemObject=false()]", schemaBased: false, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.DdlTrigger, typeof(DatabaseDdlTrigger), "DdlTrigger[@IsSystemObject=false()]", schemaBased: false, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.Synonym, typeof(Synonym), "Synonym", schemaBased: true, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.XmlSchemaCollection, typeof(XmlSchemaCollection), "XmlSchemaCollection", schemaBased: false, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.Schema, typeof(Schema), "Schema[(@ID > 4 and @ID < 16384) or (@ID > 16400)]", schemaBased: false, 9));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.PlanGuide, typeof(PlanGuide), "PlanGuide", schemaBased: false, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.UserDefinedType, typeof(UserDefinedType), "UserDefinedType", schemaBased: true, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.UserDefinedAggregate, typeof(UserDefinedAggregate), "UserDefinedAggregate", schemaBased: true, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.FullTextCatalog, typeof(FullTextCatalog), "FullTextCatalog", schemaBased: false, 9, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.UserDefinedTableType, typeof(UserDefinedTableType), "UserDefinedTableType", schemaBased: true, 10, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase));
		dicTypes.Add(new QueryInfo(DatabaseObjectType.SecurityPolicy, typeof(SecurityPolicy), "SecurityPolicy", schemaBased: true, 13, QueryInfo.EngineType.All, QueryInfo.CloudEngineEdition.SqlDatabase | QueryInfo.CloudEngineEdition.Synapse));
	}
}
