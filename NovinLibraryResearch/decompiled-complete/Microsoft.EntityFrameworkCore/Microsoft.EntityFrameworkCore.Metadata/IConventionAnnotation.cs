using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionAnnotation : IAnnotation
{
	ConfigurationSource GetConfigurationSource();
}
