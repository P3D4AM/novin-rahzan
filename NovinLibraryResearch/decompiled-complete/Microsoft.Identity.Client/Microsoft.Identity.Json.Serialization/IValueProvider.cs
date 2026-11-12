namespace Microsoft.Identity.Json.Serialization;

internal interface IValueProvider
{
	void SetValue(object target, object value);

	object GetValue(object target);
}
