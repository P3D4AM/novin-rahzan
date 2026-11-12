namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface IInterceptors
{
	TInterceptor Aggregate<TInterceptor>() where TInterceptor : class, IInterceptor;
}
