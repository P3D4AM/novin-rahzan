using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class TypeMappedPropertyRelationalParameter : TypeMappedRelationalParameter
{
	private readonly IClrPropertyGetter _clrPropertyGetter;

	public TypeMappedPropertyRelationalParameter([JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] RelationalTypeMapping relationalTypeMapping, [JetBrains.Annotations.NotNull] IProperty property)
		: base(invariantName, name, relationalTypeMapping, property.IsNullable)
	{
		_clrPropertyGetter = property.GetGetter();
	}

	public override void AddDbParameter(DbCommand command, object value)
	{
		base.AddDbParameter(command, _clrPropertyGetter.GetClrValue(value));
	}
}
