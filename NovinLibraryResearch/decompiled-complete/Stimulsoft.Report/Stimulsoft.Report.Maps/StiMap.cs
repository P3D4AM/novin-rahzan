using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Maps;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiMap.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catInfographics.png")]
[StiGdiPainter(typeof(StiMapGdiPainter))]
[StiDesigner("Stimulsoft.Report.Map.Design.StiMapDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Report.Painters.StiMapWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfMapDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV2Builder(typeof(StiMapV2Builder))]
[StiV1Builder(typeof(StiMapV1Builder))]
[StiV2Builder(typeof(StiMapV2Builder))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiDataSource))]
public class StiMap : StiComponent, IStiExportImageExtended, IStiExportImage, IStiBorder
{
	private bool isMapDataChanged;

	private List<StiMapData> hashData;

	private string mapData;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiMap;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	public StiBorder Border { get; set; } = new StiBorder();


	public override int ToolboxPosition => 220;

	public override string LocalizedCategory => StiLocalization.Get("Components", "StiMap");

	public override string LocalizedName => StiLocalization.Get("Components", "StiMap");

	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 240.0, 240.0);

	[Browsable(false)]
	public override string ComponentStyle
	{
		get
		{
			return base.ComponentStyle;
		}
		set
		{
			base.ComponentStyle = value;
		}
	}

	public override RectangleD ClientRectangle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.ClientRectangle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.ClientRectangle = value;
			StiMapDrawingCache.RemoveImage(this);
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property KeyDataColumn")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string DataColumnKey
	{
		get
		{
			return null;
		}
		set
		{
			KeyDataColumn = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property NameDataColumn")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string DataColumnName
	{
		get
		{
			return null;
		}
		set
		{
			NameDataColumn = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property ValueDataColumn")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string DataColumnValue
	{
		get
		{
			return null;
		}
		set
		{
			ValueDataColumn = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property GroupDataColumn")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string DataColumnGroup
	{
		get
		{
			return null;
		}
		set
		{
			GroupDataColumn = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property ColorDataColumn")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string DataColumnColor
	{
		get
		{
			return null;
		}
		set
		{
			ColorDataColumn = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShortValue { get; set; } = true;


	[StiBrowsable(false)]
	[Browsable(false)]
	[StiSerializable]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[StiSerializable]
	[DefaultValue(StiMapStyleIdent.Style25)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiMapStyleIdent MapStyle { get; set; } = StiMapStyleIdent.Style25;


	[StiSerializable]
	[DefaultValue(StiMapSource.Manual)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiMapSource DataFrom { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool Stretch { get; set; } = true;


	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ColorEach { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[StiBrowsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ShowValue { get; set; } = true;


	[StiBrowsable(false)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Obsolete("MapID property is obsolete. Please use MapIdent property.")]
	public StiMapID MapID
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			MapIdent = value.ToString();
		}
	}

	[StiSerializable]
	[StiBrowsable(false)]
	[Browsable(false)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string MapIdent { get; set; } = "USA";


	[Browsable(false)]
	[StiSerializable]
	public string Language { get; set; }

	[DefaultValue(false)]
	[StiBrowsable(false)]
	[Browsable(false)]
	public bool ShowLegend { get; set; }

	[StiSerializable]
	[DefaultValue(StiDisplayNameType.Full)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiDisplayNameType DisplayNameType { get; set; } = StiDisplayNameType.Full;


	[StiSerializable]
	[StiBrowsable(false)]
	[Browsable(false)]
	[DefaultValue(StiMapType.Individual)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiMapType MapType { get; set; } = StiMapType.Individual;


	[StiSerializable]
	[Browsable(false)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string MapData
	{
		get
		{
			return mapData;
		}
		set
		{
			mapData = value;
			IsHashDataEmpty = true;
			isMapDataChanged = true;
			hashData = null;
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	public string KeyDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	public string NameDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	public string ValueDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	public string GroupDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	public string ColorDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	public string Latitude { get; set; } = string.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	public string Longitude { get; set; } = string.Empty;


	[StiNonSerialized]
	[Browsable(false)]
	public bool IsHashDataEmpty { get; private set; } = true;


	[StiSerializable]
	[StiBrowsable(false)]
	[Browsable(false)]
	[DefaultValue(StiMapMode.Choropleth)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiMapMode MapMode { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiBrowsable(false)]
	[Browsable(false)]
	public string MapImage { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public string PushPins { get; set; }

	[Description("Internal use only.")]
	internal StiDataTable DataTable { get; set; }

	[Description("Internal use only.")]
	internal bool ShowBubble { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public override bool UseParentStyles
	{
		get
		{
			return base.UseParentStyles;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiInteraction Interaction
	{
		get
		{
			return base.Interaction;
		}
		set
		{
			base.Interaction = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Invalid comparison between Unknown and I4
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "UseParentStyles");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "Interaction");
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyEnum(val, "MapStyle", (Enum)MapStyle, (Enum)StiMapStyleIdent.Style25);
		JsonExtensions.AddPropertyEnum(val, "DataFrom", (Enum)DataFrom, (Enum)StiMapSource.Manual);
		JsonExtensions.AddPropertyBool(val, "ColorEach", ColorEach, false, false);
		JsonExtensions.AddPropertyBool(val, "Stretch", Stretch, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowValue", ShowValue, true, false);
		JsonExtensions.AddPropertyBool(val, "ShortValue", ShortValue, true, false);
		JsonExtensions.AddPropertyEnum(val, "ShowName", (Enum)DisplayNameType, (Enum)StiDisplayNameType.Full);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "MapIdent", MapIdent);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Language", Language);
		JsonExtensions.AddPropertyEnum(val, "MapMode", (Enum)MapMode, (Enum)StiMapMode.Choropleth);
		JsonExtensions.AddPropertyEnum(val, "MapType", (Enum)MapType, (Enum)StiMapType.Individual);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "MapData", MapData);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "KeyDataColumn", KeyDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "NameDataColumn", NameDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueDataColumn", ValueDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "GroupDataColumn", GroupDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ColorDataColumn", ColorDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Latitude", Latitude);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Longitude", Longitude);
		if ((int)mode == 1)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "MapImage", MapImage);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "PushPins", PushPins);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "MapStyle":
				MapStyle = StiJsonReportObjectHelper.DeserializeEnum<StiMapStyleIdent>(item);
				break;
			case "DataFrom":
				DataFrom = StiJsonReportObjectHelper.DeserializeEnum<StiMapSource>(item);
				break;
			case "Stretch":
				Stretch = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowValue":
				ShowValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ColorEach":
				ColorEach = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShortValue":
				ShortValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowName":
				DisplayNameType = StiJsonReportObjectHelper.DeserializeEnum<StiDisplayNameType>(item);
				break;
			case "MapID":
			case "MapIdent":
				MapIdent = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Language":
				Language = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MapMode":
				MapMode = StiJsonReportObjectHelper.DeserializeEnum<StiMapMode>(item);
				break;
			case "MapType":
				MapType = StiJsonReportObjectHelper.DeserializeEnum<StiMapType>(item);
				break;
			case "MapData":
				mapData = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "KeyDataColumn":
			case "DataColumnKey":
				KeyDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "NameDataColumn":
			case "DataColumnName":
				NameDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValueDataColumn":
			case "DataColumnValue":
				ValueDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "GroupDataColumn":
			case "DataColumnGroup":
				GroupDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ColorDataColumn":
			case "DataColumnColor":
				ColorDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Latitude":
				Latitude = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Longitude":
				Longitude = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MapImage":
				MapImage = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "PushPins":
				PushPins = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Invalid comparison between Unknown and I4
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.MapEditor() };
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, value);
		value = (((int)level != 0) ? new StiPropertyObject[3]
		{
			propertiesHelper.MapType(),
			propertiesHelper.ShowValue(),
			propertiesHelper.Stretch()
		} : new StiPropertyObject[2]
		{
			propertiesHelper.ShowValue(),
			propertiesHelper.Stretch()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Map, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.MapKeyDataColumn(),
			propertiesHelper.MapNameDataColumn(),
			propertiesHelper.MapValueDataColumn(),
			propertiesHelper.MapGroupDataColumn(),
			propertiesHelper.MapColorDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = (((int)level != 0) ? new StiPropertyObject[6]
		{
			propertiesHelper.Left(),
			propertiesHelper.Top(),
			propertiesHelper.Width(),
			propertiesHelper.Height(),
			propertiesHelper.MinSize(),
			propertiesHelper.MaxSize()
		} : new StiPropertyObject[4]
		{
			propertiesHelper.Left(),
			propertiesHelper.Top(),
			propertiesHelper.Width(),
			propertiesHelper.Height()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Position, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.Border(),
			propertiesHelper.Conditions()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = (((int)level == 0) ? new StiPropertyObject[1] { propertiesHelper.Enabled() } : (((int)level != 1) ? new StiPropertyObject[6]
		{
			propertiesHelper.AnchorMode(),
			propertiesHelper.DockStyle(),
			propertiesHelper.Enabled(),
			propertiesHelper.Printable(),
			propertiesHelper.PrintOn(),
			propertiesHelper.ShiftMode()
		} : new StiPropertyObject[5]
		{
			propertiesHelper.AnchorMode(),
			propertiesHelper.DockStyle(),
			propertiesHelper.Enabled(),
			propertiesHelper.PrintOn(),
			propertiesHelper.ShiftMode()
		}));
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		value = (((int)level == 0) ? new StiPropertyObject[1] { propertiesHelper.Name() } : (((int)level != 1) ? new StiPropertyObject[5]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.Restrictions(),
			propertiesHelper.Locked(),
			propertiesHelper.Linked()
		} : new StiPropertyObject[2]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias()
		}));
		stiPropertyCollection.Add(StiPropertyCategories.Design, value);
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone(bool cloneProperties)
	{
		StiMap stiMap = (StiMap)base.Clone(cloneProperties);
		if (hashData != null)
		{
			stiMap.hashData = new List<StiMapData>();
			foreach (StiMapData hashDatum in hashData)
			{
				stiMap.hashData.Add(hashDatum.Clone());
			}
		}
		return stiMap;
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		if (painter is StiMapGdiPainter)
		{
			((StiMapGdiPainter)painter).UseBackground = false;
		}
		return painter.GetImage(this, ref zoom, format);
	}

	public override bool IsExportAsImage(StiExportFormat format)
	{
		if (MapMode != StiMapMode.Online)
		{
			return format != StiExportFormat.Pdf;
		}
		return true;
	}

	public override StiComponent CreateNew()
	{
		return new StiMap();
	}

	public static List<StiMapData> GetMapDataInternal(StiReport report, string mapData, string mapID, string lang)
	{
		bool isHashDataEmpty;
		return GetMapDataInternal(report, mapData, mapID, lang, out isHashDataEmpty);
	}

	public static List<StiMapData> GetDefaultMapData(StiReport report, string mapID, string lang)
	{
		List<StiMapData> list = new List<StiMapData>();
		byte b = 10;
		bool flag = true;
		int num = 0;
		Color[] colors = StiMapHelper.GetColors();
		List<Color> list2 = new List<Color>();
		Color[] array = colors;
		foreach (Color item in array)
		{
			list2.Add(item);
		}
		StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(report, mapID, lang);
		foreach (string key in stiMapSvgContainer.HashPaths.Keys)
		{
			StiMapData stiMapData = new StiMapData(key)
			{
				Color = ColorTranslator.ToHtml(list2[num])
			};
			StiMapSvg stiMapSvg = stiMapSvgContainer.HashPaths[key];
			if (stiMapSvg != null)
			{
				stiMapData.Name = stiMapSvg.EnglishName;
			}
			list.Add(stiMapData);
			num++;
			if (num < list2.Count)
			{
				continue;
			}
			num = 0;
			if (flag)
			{
				Color[] array2 = colors;
				foreach (Color color in array2)
				{
					list2.Add(StiColorUtils.Dark(color, b));
				}
			}
			else
			{
				Color[] array3 = colors;
				foreach (Color color2 in array3)
				{
					list2.Add(StiColorUtils.Light(color2, b));
				}
				b += 10;
			}
			flag = !flag;
		}
		return list;
	}

	internal static List<StiMapData> GetMapDataInternal(StiReport report, string mapData, string mapID, string lang, out bool isHashDataEmpty)
	{
		isHashDataEmpty = true;
		List<StiMapData> list = new List<StiMapData>();
		if (!string.IsNullOrEmpty(mapData))
		{
			StiJsonHelper.LoadFromJsonString(mapData, (object)list);
		}
		StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(report, mapID, lang);
		if (stiMapSvgContainer != null)
		{
			if (list.Count > 0)
			{
				int num = 0;
				while (num < list.Count)
				{
					StiMapData stiMapData = list[num];
					if (string.IsNullOrEmpty(stiMapData.Key) || !stiMapSvgContainer.HashPaths.ContainsKey(stiMapData.Key))
					{
						list.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
				if (stiMapSvgContainer.HashPaths.Count != list.Count)
				{
					foreach (string key in stiMapSvgContainer.HashPaths.Keys)
					{
						if (list.FirstOrDefault((StiMapData x) => x.Key == key) == null)
						{
							StiMapData item = new StiMapData(key);
							list.Add(item);
						}
					}
				}
				foreach (StiMapData item2 in list)
				{
					if (stiMapSvgContainer.HashPaths.TryGetValue(item2.Key, out var value))
					{
						item2.Name = value.EnglishName;
					}
				}
				foreach (StiMapData item3 in list)
				{
					if (!string.IsNullOrEmpty(item3.Color) || !string.IsNullOrEmpty(item3.Group) || !string.IsNullOrEmpty(item3.Value))
					{
						isHashDataEmpty = false;
						break;
					}
				}
			}
			else
			{
				byte b = 10;
				bool flag = true;
				int num2 = 0;
				Color[] colors = StiMapHelper.GetColors();
				List<Color> list2 = new List<Color>();
				list2.AddRange(colors);
				foreach (string key2 in stiMapSvgContainer.HashPaths.Keys)
				{
					StiMapData stiMapData2 = new StiMapData(key2)
					{
						Color = ColorTranslator.ToHtml(list2[num2])
					};
					StiMapSvg stiMapSvg = stiMapSvgContainer.HashPaths[key2];
					if (stiMapSvg != null)
					{
						stiMapData2.Name = stiMapSvg.EnglishName;
					}
					list.Add(stiMapData2);
					num2++;
					if (num2 < list2.Count)
					{
						continue;
					}
					num2 = 0;
					if (flag)
					{
						Color[] array = colors;
						foreach (Color color in array)
						{
							list2.Add(StiColorUtils.Dark(color, b));
						}
					}
					else
					{
						Color[] array2 = colors;
						foreach (Color color2 in array2)
						{
							list2.Add(StiColorUtils.Light(color2, b));
						}
						b += 10;
					}
					flag = !flag;
				}
			}
		}
		return list;
	}

	public List<StiMapData> GetMapData()
	{
		if (hashData != null && hashData.Count > 0 && isMapDataChanged)
		{
			return hashData;
		}
		IsHashDataEmpty = true;
		bool isHashDataEmpty;
		List<StiMapData> mapDataInternal = GetMapDataInternal(Report, mapData, MapIdent, Language, out isHashDataEmpty);
		if (!isHashDataEmpty)
		{
			IsHashDataEmpty = false;
		}
		isMapDataChanged = true;
		hashData = mapDataInternal;
		return mapDataInternal;
	}

	public Color[] GetCurrentStyleColors()
	{
		if (!string.IsNullOrEmpty(ComponentStyle) && Report.Styles[ComponentStyle] is StiMapStyle stiMapStyle)
		{
			return stiMapStyle.Colors;
		}
		return GetStyleColors(MapStyle);
	}

	public static Color[] GetStyleColors(StiMapStyleIdent style)
	{
		return GetMapStyle(style).Colors;
	}

	internal StiBrush GetStyleBackground()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		if (!string.IsNullOrEmpty(ComponentStyle) && Report.Styles[ComponentStyle] is StiMapStyle stiMapStyle)
		{
			return (StiBrush)new StiSolidBrush(stiMapStyle.BackColor);
		}
		return (StiBrush)new StiSolidBrush(GetMapStyle(MapStyle).BackColor);
	}

	public static StiMapStyle GetMapStyle(StiMap map)
	{
		StiMapStyle stiMapStyle = null;
		if (!string.IsNullOrEmpty(map.ComponentStyle))
		{
			stiMapStyle = map.Report.Styles[map.ComponentStyle] as StiMapStyle;
		}
		if (stiMapStyle == null)
		{
			stiMapStyle = GetMapStyle(map.MapStyle);
		}
		return stiMapStyle;
	}

	public static StiMapStyle GetMapStyle(StiMapStyleIdent style)
	{
		return style switch
		{
			StiMapStyleIdent.Style21 => new StiMap21StyleFX(), 
			StiMapStyleIdent.Style24 => new StiMap24StyleFX(), 
			StiMapStyleIdent.Style25 => new StiMap25StyleFX(), 
			StiMapStyleIdent.Style26 => new StiMap26StyleFX(), 
			StiMapStyleIdent.Style27 => new StiMap27StyleFX(), 
			StiMapStyleIdent.Style28 => new StiMap28StyleFX(), 
			StiMapStyleIdent.Style29 => new StiMap29StyleFX(), 
			StiMapStyleIdent.Style30 => new StiMap30StyleFX(), 
			StiMapStyleIdent.Style31 => new StiMap31StyleFX(), 
			StiMapStyleIdent.Style32 => new StiMap32StyleFX(), 
			StiMapStyleIdent.Style33 => new StiMap33StyleFX(), 
			StiMapStyleIdent.Style34 => new StiMap34StyleFX(), 
			StiMapStyleIdent.Style35 => new StiMap35StyleFX(), 
			_ => throw new Exception("Style is not supported!"), 
		};
	}

	public StiMap()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiMap(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
