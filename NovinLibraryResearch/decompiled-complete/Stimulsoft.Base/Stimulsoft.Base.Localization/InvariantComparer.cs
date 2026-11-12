using System.Collections;
using System.Globalization;

namespace Stimulsoft.Base.Localization;

internal class InvariantComparer : IComparer
{
	internal static readonly InvariantComparer Default;

	private CompareInfo compareInfo;

	public int Compare(object a, object b)
	{
		string text = a as string;
		string text2 = b as string;
		if (text == null || text2 == null)
		{
			return Comparer.Default.Compare(a, b);
		}
		return compareInfo.Compare(text, text2);
	}

	internal InvariantComparer()
	{
		compareInfo = CultureInfo.InvariantCulture.CompareInfo;
	}

	static InvariantComparer()
	{
		Default = new InvariantComparer();
	}
}
