namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public enum CascadeTiming
{
	Immediate,
	OnSaveChanges,
	Never
}
