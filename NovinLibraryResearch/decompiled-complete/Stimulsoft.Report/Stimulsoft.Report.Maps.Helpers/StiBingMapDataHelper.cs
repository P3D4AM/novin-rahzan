using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Maps.Helpers;

public static class StiBingMapDataHelper
{
	public static void CreateImageBackground(StiMap map, List<string> pushPins, Size size)
	{
		BackgroundWorker backgroundWorker = new BackgroundWorker();
		backgroundWorker.DoWork += delegate
		{
			try
			{
				Bitmap image = StiBingMapHelper.GetImage(size, pushPins);
				StiMapDrawingCache.StoreLastImage(map, image);
				StiComponentProgressHelper.Remove((IStiAppComponent)(object)map, false);
			}
			catch
			{
				StiComponentProgressHelper.Remove((IStiAppComponent)(object)map, true);
			}
		};
		StiComponentProgressHelper.Add((IStiAppComponent)(object)map);
		backgroundWorker.RunWorkerAsync();
	}
}
