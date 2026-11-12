namespace Microsoft.EntityFrameworkCore;

public enum EntityState
{
	Detached,
	Unchanged,
	Deleted,
	Modified,
	Added
}
