using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Engine;

public class StiMapV2Builder : StiComponentV2Builder
{
	public static StiMap RenderMap(StiMap masterMap)
	{
		StiMap stiMap = (StiMap)((StiService)masterMap).Clone();
		if (!string.IsNullOrEmpty(masterMap.KeyDataColumn))
		{
			object[] datasFromDataColumn = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.KeyDataColumn, null, firstPositionInDataSource: true);
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
				array = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.NameDataColumn, null, firstPositionInDataSource: true);
			}
			if (!string.IsNullOrEmpty(masterMap.ValueDataColumn))
			{
				array2 = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.ValueDataColumn, null, firstPositionInDataSource: true);
			}
			if (!string.IsNullOrEmpty(masterMap.GroupDataColumn))
			{
				array3 = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.GroupDataColumn, null, firstPositionInDataSource: true);
			}
			if (!string.IsNullOrEmpty(masterMap.ColorDataColumn))
			{
				array4 = StiDataColumn.GetDatasFromDataColumn(stiMap.Report.Dictionary, masterMap.ColorDataColumn, null, firstPositionInDataSource: true);
			}
			if (array == null && array2 == null && array3 == null && array4 == null)
			{
				return stiMap;
			}
			List<StiMapData> mapData = stiMap.GetMapData();
			for (int j = 0; j < datasFromDataColumn.Length; j++)
			{
				string text = datasFromDataColumn[j] as string;
				StiMapData stiMapData = null;
				if (!string.IsNullOrEmpty(text))
				{
					List<string> idents = new List<string> { text };
					List<string> mapIdents = new StiMapKeyHelper().GetMapIdents(text);
					if (mapIdents != null)
					{
						idents.AddRange(mapIdents);
					}
					idents = (from i in idents
						where !string.IsNullOrWhiteSpace(i)
						select i?.Replace(" ", "")?.ToLowerInvariant()).Distinct().ToList();
					stiMapData = mapData.FirstOrDefault((StiMapData x) => idents.Any((string i) => x.Key?.Replace(" ", "")?.ToLowerInvariant() == i));
				}
				if (stiMapData == null)
				{
					continue;
				}
				if (array != null && j >= 0 && j < array.Length)
				{
					string text2 = array[j]?.ToString();
					stiMapData.Name = ((!string.IsNullOrEmpty(text2)) ? text2 : null);
				}
				if (array2 != null && j >= 0 && j < array2.Length)
				{
					stiMapData.Value = StiValueHelper.TryToDecimal(array2[j]).ToString();
				}
				if (array3 != null && j >= 0 && j < array3.Length)
				{
					string text3 = array3[j]?.ToString();
					if (!string.IsNullOrEmpty(text3))
					{
						stiMapData.Group = text3;
					}
					else
					{
						stiMapData.Group = null;
					}
				}
				if (array4 != null && j >= 0 && j < array4.Length)
				{
					string text4 = array4[j]?.ToString();
					if (!string.IsNullOrEmpty(text4))
					{
						stiMapData.Color = text4;
					}
					else
					{
						stiMapData.Color = null;
					}
				}
			}
			stiMap.MapData = StiJsonHelper.SaveToJsonString((object)mapData);
		}
		else if (!string.IsNullOrEmpty(stiMap.Latitude) && !string.IsNullOrEmpty(stiMap.Longitude))
		{
			List<string> list = new List<string>();
			StiDataBand stiDataBand = stiMap.Parent as StiDataBand;
			List<object> list2 = null;
			List<object> list3 = null;
			stiMap.PushPins = null;
			if (stiDataBand != null)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(stiMap.Report.Dictionary, stiMap.Latitude);
				object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(stiMap.Report.Dictionary, stiMap.Longitude);
				if (dataFromDataColumn != null && dataFromDataColumn2 != null)
				{
					list2 = new List<object> { dataFromDataColumn };
					list3 = new List<object> { dataFromDataColumn2 };
				}
			}
			else
			{
				list2 = StiDataColumn.GetDataListFromDataColumn(stiMap.Report.Dictionary, stiMap.Latitude);
				list3 = StiDataColumn.GetDataListFromDataColumn(stiMap.Report.Dictionary, stiMap.Longitude);
			}
			if (list2 != null && list3 != null)
			{
				int num = Math.Min(100, Math.Max(list2.Count, list3.Count));
				for (int k = 0; k < num; k++)
				{
					object obj = list2[k];
					object obj2 = list3[k];
					if (obj != null && obj2 != null)
					{
						string text5 = StiValueHelper.TryToDecimal(obj).ToString(CultureInfo.InvariantCulture).Replace(",", ".");
						string text6 = StiValueHelper.TryToDecimal(obj2).ToString(CultureInfo.InvariantCulture).Replace(",", ".");
						list.Add("pp=" + text5 + "," + text6 + ";60");
					}
				}
				stiMap.PushPins = JsonConvert.SerializeObject((object)list.ToArray());
			}
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
