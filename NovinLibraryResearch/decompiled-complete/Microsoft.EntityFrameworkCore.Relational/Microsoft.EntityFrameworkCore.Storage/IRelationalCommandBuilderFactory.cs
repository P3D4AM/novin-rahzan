namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalCommandBuilderFactory
{
	IRelationalCommandBuilder Create();
}
