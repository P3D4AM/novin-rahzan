using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Helpers;

internal struct StiElementLocationKey
{
	internal Rectangle RectKey { get; }

	internal int CountKey { get; }

	internal StiElementLocationKey(Rectangle rectKey, int countKey)
	{
		RectKey = rectKey;
		CountKey = countKey;
	}

	internal StiElementLocationKey(RectangleD rectKey, int countKey)
	{
		RectKey = rectKey.ToRectangle();
		CountKey = countKey;
	}
}
