using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public interface IValueConverterSelector
{
	IEnumerable<ValueConverterInfo> Select([JetBrains.Annotations.NotNull] Type modelClrType, [JetBrains.Annotations.CanBeNull] Type providerClrType = null);
}
