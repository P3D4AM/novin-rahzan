using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Design;

public interface IDesignTimeServices
{
	void ConfigureDesignTimeServices([JetBrains.Annotations.NotNull] IServiceCollection serviceCollection);
}
