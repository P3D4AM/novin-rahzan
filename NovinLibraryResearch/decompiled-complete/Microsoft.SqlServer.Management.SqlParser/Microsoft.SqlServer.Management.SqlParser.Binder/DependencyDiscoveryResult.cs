using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

public sealed class DependencyDiscoveryResult
{
	private readonly IEnumerable<ObjectDependency> dependencies;

	private readonly IEnumerable<Error> errors;

	public IEnumerable<IMetadataObject> References => dependencies.Select((ObjectDependency d) => d.TargetObject).Distinct();

	public IEnumerable<ObjectDependency> Dependencies => dependencies;

	public IEnumerable<Error> Errors => errors;

	internal DependencyDiscoveryResult(IEnumerable<ObjectDependency> dependencies, IEnumerable<Error> errors)
	{
		this.dependencies = dependencies;
		this.errors = errors;
	}
}
