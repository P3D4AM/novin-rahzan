namespace Microsoft.EntityFrameworkCore;

public enum DeleteBehavior
{
	ClientSetNull,
	Restrict,
	SetNull,
	Cascade,
	ClientCascade,
	NoAction,
	ClientNoAction
}
