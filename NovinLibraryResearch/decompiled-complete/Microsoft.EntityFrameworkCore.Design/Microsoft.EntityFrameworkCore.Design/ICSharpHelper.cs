using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design;

public interface ICSharpHelper
{
	string Fragment([JetBrains.Annotations.NotNull] MethodCallCodeFragment fragment);

	string Identifier([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] ICollection<string> scope = null);

	string Lambda([JetBrains.Annotations.NotNull] IReadOnlyList<string> properties);

	string Literal([JetBrains.Annotations.NotNull] object[,] values);

	string Literal<T>(T? value) where T : struct;

	string Literal(bool value);

	string Literal(byte value);

	string Literal(char value);

	string Literal(DateTime value);

	string Literal(DateTimeOffset value);

	string Literal(decimal value);

	string Literal(double value);

	string Literal([JetBrains.Annotations.NotNull] Enum value);

	string Literal(float value);

	string Literal(Guid value);

	string Literal(int value);

	string Literal(long value);

	string Literal(sbyte value);

	string Literal(short value);

	string Literal([JetBrains.Annotations.NotNull] string value);

	string Literal(TimeSpan value);

	string Literal(uint value);

	string Literal(ulong value);

	string Literal(ushort value);

	string Literal<T>([JetBrains.Annotations.NotNull] T[] values, bool vertical = false);

	string Namespace([JetBrains.Annotations.NotNull] params string[] name);

	string Reference([JetBrains.Annotations.NotNull] Type type);

	string UnknownLiteral([JetBrains.Annotations.CanBeNull] object value);
}
