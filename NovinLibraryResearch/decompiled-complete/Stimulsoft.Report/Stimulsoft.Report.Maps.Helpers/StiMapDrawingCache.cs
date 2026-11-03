using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Maps.Helpers;

public static class StiMapDrawingCache
{
	private class StiCacheItem
	{
		public object Image { get; set; }

		public Size Size { get; set; }

		public string Latitude { get; set; }

		public bool IsDesigning { get; set; }

		public string Longitude { get; set; }
	}

	private static Dictionary<StiMap, StiCacheItem> lastImageCache = new Dictionary<StiMap, StiCacheItem>();

	internal static void Clear()
	{
		lastImageCache.Clear();
	}

	internal static void RemoveImage(StiMap map)
	{
		lock (lastImageCache)
		{
			if (lastImageCache.ContainsKey(map))
			{
				lastImageCache.Remove(map);
			}
		}
	}

	internal static object GetLastImage(StiMap map)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		lock (lastImageCache)
		{
			if (!lastImageCache.ContainsKey(map))
			{
				if (!map.IsDesigning && map.MapImage != null)
				{
					try
					{
						return StiImageConverter.StringToImage(map.MapImage);
					}
					catch
					{
					}
				}
				return null;
			}
			StiCacheItem stiCacheItem = lastImageCache[map];
			if (stiCacheItem.IsDesigning == map.IsDesigning && !(stiCacheItem.Latitude != map.Latitude) && !(stiCacheItem.Longitude != map.Longitude))
			{
				Size size = stiCacheItem.Size;
				RectangleD clientRectangle = map.ClientRectangle;
				SizeD size2 = ((RectangleD)(ref clientRectangle)).Size;
				if (!(size != ((SizeD)(ref size2)).ToSize()))
				{
					return stiCacheItem.Image;
				}
			}
			lastImageCache.Remove(map);
			return null;
		}
	}

	internal static void StoreLastImage(StiMap map, object image)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		lock (lastImageCache)
		{
			StiCacheItem obj = new StiCacheItem
			{
				Image = image
			};
			RectangleD clientRectangle = map.ClientRectangle;
			SizeD size = ((RectangleD)(ref clientRectangle)).Size;
			obj.Size = ((SizeD)(ref size)).ToSize();
			obj.IsDesigning = map.IsDesigning;
			obj.Latitude = map.Latitude;
			obj.Longitude = map.Longitude;
			StiCacheItem value = obj;
			lastImageCache[map] = value;
			Bitmap val = (Bitmap)((image is Bitmap) ? image : null);
			if (val != null)
			{
				try
				{
					map.MapImage = StiImageConverter.ImageToString((Image)(object)val, false);
					return;
				}
				catch
				{
					return;
				}
			}
		}
	}
}
