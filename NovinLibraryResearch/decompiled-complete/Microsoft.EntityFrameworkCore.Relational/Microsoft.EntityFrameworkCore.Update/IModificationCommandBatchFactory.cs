namespace Microsoft.EntityFrameworkCore.Update;

public interface IModificationCommandBatchFactory
{
	ModificationCommandBatch Create();
}
