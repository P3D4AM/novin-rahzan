using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Helpers;

internal static class StiSimpleShadowCache
{
	private static object lockObject = new object();

	private static Dictionary<string, Bitmap> cache = new Dictionary<string, Bitmap>();

	private static string GetCacheKey(IStiApp app, StiSimpleShadow shadow, StiCornerRadius cornerRadius, StiBorderSides borderSide, bool isCircle, Size size, float scale)
	{
		if (app != null)
		{
			((IStiAppCell)app).SetKey(StiKeyHelper.GetOrGeneratedKey(((IStiAppCell)app).GetKey()));
		}
		string arg = ((app != null) ? ((IStiAppCell)app).GetKey() : null) ?? string.Empty;
		int num = 0;
		if (shadow != null)
		{
			num = (num * 397) ^ shadow.GetUniqueCode();
		}
		if (cornerRadius != null)
		{
			num = (num * 397) ^ cornerRadius.GetUniqueCode();
		}
		num = (num * 397) ^ ((object)(StiBorderSides)(ref borderSide)).GetHashCode();
		num = (num * 397) ^ isCircle.GetHashCode();
		num = (num * 397) ^ size.Width.GetHashCode();
		num = (num * 397) ^ size.Height.GetHashCode();
		num = (num * 397) ^ scale.GetHashCode();
		return $"{arg}.{num}";
	}

	public static Bitmap GetFromCache(IStiApp app, StiSimpleShadow shadow, StiCornerRadius cornerRadius, StiBorderSides borderSide, bool isCircle, Size size, float scale)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(app, shadow, cornerRadius, borderSide, isCircle, size, scale);
			return cache.ContainsKey(cacheKey) ? cache[cacheKey] : null;
		}
	}

	public static void AddToCache(IStiApp app, StiSimpleShadow shadow, Bitmap shadowBitmap, StiCornerRadius cornerRadius, StiBorderSides borderSide, bool isCircle, Size size, float scale)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (shadow == null || shadowBitmap == null)
		{
			return;
		}
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(app, shadow, cornerRadius, borderSide, isCircle, size, scale);
			cache[cacheKey] = shadowBitmap;
		}
	}

	public static void Clean(string appKey)
	{
		lock (cache)
		{
			IEnumerable<string> enumerable;
			if (appKey != null)
			{
				enumerable = cache.Keys.Where((string k) => k.StartsWith(appKey));
			}
			else
			{
				IEnumerable<string> keys = cache.Keys;
				enumerable = keys;
			}
			IEnumerable<string> source = enumerable;
			source.ToList().ForEach(delegate(string k)
			{
				Bitmap val = cache[k];
				cache.Remove(k);
				((Image)val).Dispose();
			});
		}
	}
}
