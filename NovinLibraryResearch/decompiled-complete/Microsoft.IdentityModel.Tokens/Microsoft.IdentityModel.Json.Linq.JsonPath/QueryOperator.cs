namespace Microsoft.IdentityModel.Json.Linq.JsonPath;

internal enum QueryOperator
{
	None,
	Equals,
	NotEquals,
	Exists,
	LessThan,
	LessThanOrEquals,
	GreaterThan,
	GreaterThanOrEquals,
	And,
	Or,
	RegexEquals,
	StrictEquals,
	StrictNotEquals
}
