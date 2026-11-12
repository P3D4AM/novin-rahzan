namespace Microsoft.Identity.Json;

internal enum WriteState
{
	Error,
	Closed,
	Object,
	Array,
	Constructor,
	Property,
	Start
}
