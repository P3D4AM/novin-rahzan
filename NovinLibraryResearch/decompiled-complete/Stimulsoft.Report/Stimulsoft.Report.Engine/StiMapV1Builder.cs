using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Engine;

public class StiMapV1Builder : StiComponentV2Builder
{
	public static StiMap RenderMap(StiMap masterMap)
	{
		StiMap stiMap = (StiMap)((StiService)masterMap).Clone();
		if (!string.IsNullOrEmpty(masterMap.KeyDataColumn))
		{
			object[] datasFromDataColumn = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.KeyDataColumn);
			if (datasFromDataColumn == null)
			{
				return stiMap;
			}
			object[] array = null;
			object[] array2 = null;
			object[] array3 = null;
			object[] array4 = null;
			if (!string.IsNullOrEmpty(masterMap.NameDataColumn))
			{
				array = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.NameDataColumn);
			}
			if (!string.IsNullOrEmpty(masterMap.ValueDataColumn))
			{
				array2 = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.ValueDataColumn);
			}
			if (!string.IsNullOrEmpty(masterMap.GroupDataColumn))
			{
				array3 = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.GroupDataColumn);
			}
			if (!string.IsNullOrEmpty(masterMap.ColorDataColumn))
			{
				array4 = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.ColorDataColumn);
			}
			if (array == null && array2 == null && array3 == null && array4 == null)
			{
				return stiMap;
			}
			List<StiMapData> mapData = stiMap.GetMapData();
			for (int i = 0; i < datasFromDataColumn.Length; i++)
			{
				string key = datasFromDataColumn[i] as string;
				StiMapData stiMapData = null;
				if (!string.IsNullOrEmpty(key))
				{
					stiMapData = mapData.FirstOrDefault((StiMapData x) => x.Key == key);
				}
				if (stiMapData == null)
				{
					continue;
				}
				if (array != null)
				{
					string text = array[i] as string;
					if (!string.IsNullOrEmpty(text))
					{
						stiMapData.Name = text;
					}
					else
					{
						stiMapData.Name = null;
					}
				}
				if (array2 != null)
				{
					stiMapData.Value = array2[i] as string;
				}
				if (array3 != null)
				{
					string text2 = array3[i] as string;
					if (!string.IsNullOrEmpty(text2))
					{
						stiMapData.Group = text2;
					}
					else
					{
						stiMapData.Group = null;
					}
				}
				if (array4 != null)
				{
					string text3 = array4[i] as string;
					if (!string.IsNullOrEmpty(text3))
					{
						stiMapData.Color = text3;
					}
					else
					{
						stiMapData.Color = null;
					}
				}
			}
			stiMap.MapData = StiJsonHelper.SaveToJsonString((object)mapData);
		}
		return stiMap;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		if (masterComp.Height > masterComp.Page.Height || masterComp.Height > masterComp.Parent.Height)
		{
			masterComp.Height = Math.Min(masterComp.Page.Height, masterComp.Parent.Height);
		}
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiMap masterMap = masterComp as StiMap;
		return RenderMap(masterMap);
	}
}
