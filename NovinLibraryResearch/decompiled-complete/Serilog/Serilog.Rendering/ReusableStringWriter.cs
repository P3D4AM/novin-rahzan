using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Serilog.Rendering;

internal class ReusableStringWriter : StringWriter
{
	[ThreadStatic]
	private static ReusableStringWriter? _pooledWriter;

	private const int StringBuilderCapacityThreshold = 32768;

	public static StringWriter GetOrCreate(IFormatProvider? formatProvider = null)
	{
		IFormatProvider objB = formatProvider ?? CultureInfo.CurrentCulture;
		ReusableStringWriter reusableStringWriter = _pooledWriter;
		_pooledWriter = null;
		if (reusableStringWriter == null || !object.Equals(reusableStringWriter.FormatProvider, objB))
		{
			reusableStringWriter = new ReusableStringWriter(formatProvider);
		}
		return reusableStringWriter;
	}

	private ReusableStringWriter(IFormatProvider? formatProvider)
		: base(formatProvider ?? CultureInfo.CurrentCulture)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposing)
		{
			base.Dispose(disposing);
			return;
		}
		StringBuilder stringBuilder = GetStringBuilder();
		if (stringBuilder.Capacity > 32768)
		{
			base.Dispose(disposing);
			return;
		}
		stringBuilder.Clear();
		_pooledWriter = this;
	}
}
