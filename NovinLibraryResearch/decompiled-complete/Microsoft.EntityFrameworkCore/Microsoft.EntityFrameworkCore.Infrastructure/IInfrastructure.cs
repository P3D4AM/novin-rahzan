namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IInfrastructure<out T>
{
	T Instance { get; }
}
