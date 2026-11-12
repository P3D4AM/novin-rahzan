using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;

public class SqlServerDesignTimeServices : IDesignTimeServices
{
	public virtual void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<LoggingDefinitions, SqlServerLoggingDefinitions>().AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>().AddSingleton<IDatabaseModelFactory, SqlServerDatabaseModelFactory>()
			.AddSingleton<IProviderConfigurationCodeGenerator, SqlServerCodeGenerator>()
			.AddSingleton<IAnnotationCodeGenerator, SqlServerAnnotationCodeGenerator>();
	}
}
