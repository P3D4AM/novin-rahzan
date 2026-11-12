namespace Microsoft.EntityFrameworkCore;

public enum PropertyAccessMode
{
	Field,
	FieldDuringConstruction,
	Property,
	PreferField,
	PreferFieldDuringConstruction,
	PreferProperty
}
