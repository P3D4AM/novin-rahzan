namespace Microsoft.EntityFrameworkCore.Query;

public interface IQueryCompilationContextFactory
{
	QueryCompilationContext Create(bool async);
}
