namespace Microsoft.AspNetCore.Routing;

public interface INamedRouter : IRouter
{
	string Name { get; }
}
