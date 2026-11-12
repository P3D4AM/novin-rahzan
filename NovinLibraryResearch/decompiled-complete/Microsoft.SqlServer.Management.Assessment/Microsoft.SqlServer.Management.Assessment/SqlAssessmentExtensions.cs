using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Assessment;

public static class SqlAssessmentExtensions
{
	public static readonly Engine Engine = new Engine();

	public static IEnumerable<ICheck> GetAssessmentItems(this SqlSmoObject smoObject, params string[] checkIdsOrTags)
	{
		if (smoObject == null)
		{
			throw new ArgumentNullException("smoObject");
		}
		if (smoObject is Microsoft.SqlServer.Management.Smo.Server server)
		{
			return server.GetAssessmentItems(checkIdsOrTags);
		}
		if (smoObject is Database database)
		{
			return database.GetAssessmentItems(checkIdsOrTags);
		}
		if (smoObject is FileGroup fileGroup)
		{
			return fileGroup.GetAssessmentItems(checkIdsOrTags);
		}
		if (smoObject is AvailabilityGroup availabilityGroup)
		{
			return availabilityGroup.GetAssessmentItems(checkIdsOrTags);
		}
		return Array.Empty<ICheck>();
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this SqlSmoObject smoObject, params string[] checkIdsOrTags)
	{
		if (smoObject == null)
		{
			throw new ArgumentNullException("smoObject");
		}
		if (smoObject is Microsoft.SqlServer.Management.Smo.Server server)
		{
			return server.GetAssessmentResultsList(checkIdsOrTags);
		}
		if (smoObject is Database database)
		{
			return database.GetAssessmentResultsList(checkIdsOrTags);
		}
		if (smoObject is FileGroup fileGroup)
		{
			return fileGroup.GetAssessmentResultsList(checkIdsOrTags);
		}
		if (smoObject is AvailabilityGroup availabilityGroup)
		{
			return availabilityGroup.GetAssessmentResultsList(checkIdsOrTags);
		}
		return Task.FromResult(new List<IAssessmentResult>());
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this SqlSmoObject smoObject, IEnumerable<ICheck> checks)
	{
		if (smoObject == null)
		{
			throw new ArgumentNullException("smoObject");
		}
		if (smoObject is Microsoft.SqlServer.Management.Smo.Server server)
		{
			return server.GetAssessmentResultsList(checks);
		}
		if (smoObject is Database database)
		{
			return database.GetAssessmentResultsList(checks);
		}
		if (smoObject is FileGroup fileGroup)
		{
			return fileGroup.GetAssessmentResultsList(checks);
		}
		if (smoObject is AvailabilityGroup availabilityGroup)
		{
			return availabilityGroup.GetAssessmentResultsList(checks);
		}
		return Task.FromResult(new List<IAssessmentResult>());
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this SqlSmoObject smoObject, params string[] checkIdsOrTags)
	{
		if (smoObject == null)
		{
			throw new ArgumentNullException("smoObject");
		}
		return smoObject.GetAssessmentResultsList(checkIdsOrTags).Result;
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this SqlSmoObject smoObject, IEnumerable<ICheck> checks)
	{
		if (smoObject == null)
		{
			throw new ArgumentNullException("smoObject");
		}
		return smoObject.GetAssessmentResultsList(checks).Result;
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this AvailabilityGroup availabilityGroup, params string[] checkIdsOrTags)
	{
		if (availabilityGroup == null)
		{
			throw new ArgumentNullException("availabilityGroup");
		}
		return availabilityGroup.GetAssessmentResultsList(checkIdsOrTags).Result;
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this AvailabilityGroup availabilityGroup, IEnumerable<ICheck> checks)
	{
		if (availabilityGroup == null)
		{
			throw new ArgumentNullException("availabilityGroup");
		}
		return availabilityGroup.GetAssessmentResultsList(checks).Result;
	}

	public static IEnumerable<ICheck> GetAssessmentItems(this AvailabilityGroup availabilityGroup, params string[] checkIdsOrTags)
	{
		if (availabilityGroup == null)
		{
			throw new ArgumentNullException("availabilityGroup");
		}
		return GetAssessmentItems(GetLocator(availabilityGroup), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this AvailabilityGroup availabilityGroup, params string[] checkIdsOrTags)
	{
		if (availabilityGroup == null)
		{
			throw new ArgumentNullException("availabilityGroup");
		}
		return GetAssessmentResultsList(GetLocator(availabilityGroup), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this AvailabilityGroup availabilityGroup, IEnumerable<ICheck> checks)
	{
		if (availabilityGroup == null)
		{
			throw new ArgumentNullException("availabilityGroup");
		}
		ISqlObjectLocator locator = GetLocator(availabilityGroup);
		ICheck[] checksCopy = checks.ToArray();
		return GetAssessmentResultsList(locator, checksCopy);
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this Database database, params string[] checkIdsOrTags)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		return database.GetAssessmentResultsList(checkIdsOrTags).Result;
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this Database database, IEnumerable<ICheck> checks)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		return database.GetAssessmentResultsList(checks).Result;
	}

	public static IEnumerable<ICheck> GetAssessmentItems(this Database database, params string[] checkIdsOrTags)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		return GetAssessmentItems(GetLocator(database), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this Database database, params string[] checkIdsOrTags)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		return GetAssessmentResultsList(GetLocator(database), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this Database database, IEnumerable<ICheck> checks)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		ISqlObjectLocator locator = GetLocator(database);
		ICheck[] checksCopy = checks.ToArray();
		return GetAssessmentResultsList(locator, checksCopy);
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this FileGroup fileGroup, params string[] checkIdsOrTags)
	{
		if (fileGroup == null)
		{
			throw new ArgumentNullException("fileGroup");
		}
		return fileGroup.GetAssessmentResultsList(checkIdsOrTags).Result;
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this FileGroup fileGroup, IEnumerable<ICheck> checks)
	{
		if (fileGroup == null)
		{
			throw new ArgumentNullException("fileGroup");
		}
		return fileGroup.GetAssessmentResultsList(checks).Result;
	}

	public static IEnumerable<ICheck> GetAssessmentItems(this FileGroup fileGroup, params string[] checkIdsOrTags)
	{
		if (fileGroup == null)
		{
			throw new ArgumentNullException("fileGroup");
		}
		return GetAssessmentItems(GetLocator(fileGroup), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this FileGroup fileGroup, params string[] checkIdsOrTags)
	{
		if (fileGroup == null)
		{
			throw new ArgumentNullException("fileGroup");
		}
		return GetAssessmentResultsList(GetLocator(fileGroup), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this FileGroup fileGroup, IEnumerable<ICheck> checks)
	{
		if (fileGroup == null)
		{
			throw new ArgumentNullException("fileGroup");
		}
		ISqlObjectLocator locator = GetLocator(fileGroup);
		ICheck[] checksCopy = checks.ToArray();
		return GetAssessmentResultsList(locator, checksCopy);
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this Microsoft.SqlServer.Management.Smo.Server server, params string[] checkIdsOrTags)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		return server.GetAssessmentResultsList(checkIdsOrTags).Result;
	}

	public static IEnumerable<IAssessmentResult> GetAssessmentResults(this Microsoft.SqlServer.Management.Smo.Server server, IEnumerable<ICheck> checks)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		return server.GetAssessmentResultsList(checks).Result;
	}

	public static IEnumerable<ICheck> GetAssessmentItems(this Microsoft.SqlServer.Management.Smo.Server server, params string[] checkIdsOrTags)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		return GetAssessmentItems(GetLocator(server), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this Microsoft.SqlServer.Management.Smo.Server server, params string[] checkIdsOrTags)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		return GetAssessmentResultsList(GetLocator(server), checkIdsOrTags);
	}

	public static Task<List<IAssessmentResult>> GetAssessmentResultsList(this Microsoft.SqlServer.Management.Smo.Server server, IEnumerable<ICheck> checks)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		ISqlObjectLocator locator = GetLocator(server);
		ICheck[] checksCopy = checks.ToArray();
		return GetAssessmentResultsList(locator, checksCopy);
	}

	private static Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator locator, ICheck[] checksCopy)
	{
		if (checksCopy != null && checksCopy.Length != 0)
		{
			return Engine.GetAssessmentResultsList(locator, checksCopy);
		}
		return Engine.GetAssessmentResultsList(locator);
	}

	private static Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator locator, string[] checkIdsOrTags)
	{
		if (checkIdsOrTags != null && checkIdsOrTags.Length != 0)
		{
			return Engine.GetAssessmentResultsList(locator, checkIdsOrTags);
		}
		return Engine.GetAssessmentResultsList(locator);
	}

	private static IEnumerable<ICheck> GetAssessmentItems(ISqlObjectLocator locator, string[] checkIdsOrTags)
	{
		if (checkIdsOrTags != null && checkIdsOrTags.LongLength != 0L)
		{
			return Engine.GetChecks(locator, checkIdsOrTags);
		}
		return Engine.GetChecks(locator);
	}

	private static SqlObjectLocator GetGenericLocator(Microsoft.SqlServer.Management.Smo.Server server)
	{
		return new SqlObjectLocator
		{
			Connection = (DbConnection)((ICloneable)server.ConnectionContext.SqlConnectionObject).Clone(),
			Platform = server.HostPlatform,
			Version = server.Version,
			EngineEdition = TranslateEdition(server.EngineEdition),
			ServerName = server.Name
		};
	}

	private static ISqlObjectLocator GetLocator(AvailabilityGroup availabilityGroup)
	{
		SqlObjectLocator genericLocator = GetGenericLocator(availabilityGroup.Parent);
		genericLocator.Name = availabilityGroup.Name;
		genericLocator.Urn = availabilityGroup.Urn;
		genericLocator.Type = SqlObjectType.AvailabilityGroup;
		return genericLocator;
	}

	private static ISqlObjectLocator GetLocator(Database database)
	{
		SqlObjectLocator genericLocator = GetGenericLocator(database.Parent);
		genericLocator.Urn = database.Urn;
		genericLocator.Name = database.Name;
		genericLocator.Type = SqlObjectType.Database;
		return genericLocator;
	}

	private static ISqlObjectLocator GetLocator(FileGroup fileGroup)
	{
		SqlObjectLocator genericLocator = GetGenericLocator(fileGroup.Parent.Parent);
		genericLocator.Urn = fileGroup.Urn;
		genericLocator.Name = fileGroup.Name;
		genericLocator.Type = SqlObjectType.FileGroup;
		return genericLocator;
	}

	private static ISqlObjectLocator GetLocator(Microsoft.SqlServer.Management.Smo.Server server)
	{
		SqlObjectLocator genericLocator = GetGenericLocator(server);
		genericLocator.Urn = server.Urn;
		genericLocator.Name = server.Name;
		genericLocator.Type = SqlObjectType.Server;
		return genericLocator;
	}

	private static SqlEngineEdition TranslateEdition(Edition edition)
	{
		if (edition <= Edition.Unknown)
		{
			return SqlEngineEdition.None;
		}
		return (SqlEngineEdition)(1 << (int)(edition - 1));
	}
}
