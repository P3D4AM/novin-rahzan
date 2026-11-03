using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.PropertyGridInternal;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Surface;

namespace Stimulsoft.Base.Design;

public class StiPropertiesTab : PropertiesTab
{
	private class OrderData : IComparable
	{
		private int position;

		public int CategoryPosition { get; set; }

		public string Name { get; }

		public string CategoryName { get; }

		public int CompareTo(object obj)
		{
			OrderData orderData = obj as OrderData;
			if (orderData.CategoryPosition == CategoryPosition)
			{
				if (orderData.position == position)
				{
					return string.CompareOrdinal(Name, orderData.Name);
				}
				return -orderData.position.CompareTo(position);
			}
			return -orderData.CategoryPosition.CompareTo(CategoryPosition);
		}

		public OrderData(string categoryName, string name, int position, int categoryPosition)
		{
			this.position = position;
			CategoryPosition = categoryPosition;
			Name = name;
			CategoryName = categoryName;
		}
	}

	private class CategoryData : IComparable
	{
		private int position;

		public string CategoryName { get; }

		public int CompareTo(object obj)
		{
			CategoryData categoryData = obj as CategoryData;
			if (categoryData.position == position)
			{
				return string.CompareOrdinal(CategoryName, categoryData.CategoryName);
			}
			return -categoryData.position.CompareTo(position);
		}

		public CategoryData(string categoryName, int position)
		{
			CategoryName = categoryName;
			this.position = position;
		}
	}

	internal PropertyGrid PropertyGrid { get; set; }

	public static bool ShowAllPropertiesInWpf { get; set; }

	public override Bitmap Bitmap => StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Office2013.Properties.png");

	public override string TabName => StiLocalization.Get("Report", "PropertiesTab");

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
	{
		PropertyDescriptorCollection properties = ((PropertyTab)this).GetProperties(component, attributes);
		if (PropertyGrid.SelectedObjects != null && PropertyGrid.SelectedObjects.Length == 1)
		{
			return properties;
		}
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		foreach (PropertyDescriptor item in properties)
		{
			StiPropertyDescriptor val = (StiPropertyDescriptor)(object)((item is StiPropertyDescriptor) ? item : null);
			if (val != null)
			{
				list.Add(new Tuple<string, string>(val.LocalizedName, ((MemberDescriptor)(object)val).Name));
			}
			else
			{
				list.Add(new Tuple<string, string>(item.Name, item.Name));
			}
		}
		list.Sort();
		List<string> list2 = new List<string>();
		foreach (Tuple<string, string> item2 in list)
		{
			list2.Add(item2.Item2);
		}
		return properties.Sort(list2.ToArray());
	}

	public override bool CanExtend(object extendee)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return GetPropertiesFromComponent(component, PropertyGrid.SelectedObjects, PropertyGrid.PropertySort, (StiGuiMode)0);
	}

	public static PropertyDescriptorCollection GetPropertiesFromComponent(object component, object[] selectedObjects, PropertySort propertySort, StiGuiMode guiMode)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		PropertyDescriptorCollection propertiesFromComponentInternal = GetPropertiesFromComponentInternal(component, selectedObjects, propertySort, guiMode);
		StiOptions.Engine.GlobalEvents.InvokeGetProperties(component, new StiGetPropertiesEventArgs(selectedObjects, propertiesFromComponentInternal, propertySort, guiMode));
		return propertiesFromComponentInternal;
	}

	private static PropertyDescriptorCollection GetPropertiesFromComponentInternal(object component, object[] selectedObjects, PropertySort propertySort, StiGuiMode guiMode)
	{
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if (component is StiDataBindingsCollection)
		{
			return ProcessDataBindingsProperties(component);
		}
		List<StiPropertyDescriptor> properties = new List<StiPropertyDescriptor>();
		PropertyDescriptorCollection properties2 = TypeDescriptor.GetProperties(component);
		foreach (PropertyDescriptor item in properties2)
		{
			if (CheckAllowedProperty(item, component) || CheckPropertyLevel(item) || CheckEngineVersion(item, component) || CheckGuiMode(item, guiMode))
			{
				continue;
			}
			if (StiOptions.Designer.UseGlobalizationManager)
			{
				if (item.Name == "Text" && component is StiText stiText && !string.IsNullOrWhiteSpace(stiText.GlobalizedName) && stiText.Report != null && stiText.Report.GlobalizationManager != null && !(stiText.Report.GlobalizationManager is StiNullGlobalizationManager))
				{
					try
					{
						string @string = stiText.Report.GlobalizationManager.GetString(stiText.GlobalizedName);
						if (@string != null)
						{
							continue;
						}
					}
					catch
					{
					}
				}
				if (item.Name == "Image" && component is StiImage stiImage && !string.IsNullOrWhiteSpace(stiImage.GlobalizedName) && stiImage.Report != null && stiImage.Report.GlobalizationManager != null && !(stiImage.Report.GlobalizationManager is StiNullGlobalizationManager))
				{
					try
					{
						object @object = stiImage.Report.GlobalizationManager.GetObject(stiImage.GlobalizedName);
						if (@object != null)
						{
							continue;
						}
					}
					catch
					{
					}
				}
			}
			ProcessBrowsableAttribute(component, item, properties);
		}
		properties = ProcessReportProperties(component, properties);
		properties = ProcessStyleProperties(component, properties);
		properties = ProcessCrossProperties(component, properties);
		properties = ProcessDashboardElementProperties(selectedObjects, component, properties);
		PropertyDescriptor[] properties3;
		if (selectedObjects != null && selectedObjects.Length > 1)
		{
			properties3 = (PropertyDescriptor[])(object)properties.OrderBy((StiPropertyDescriptor x) => x.LocalizedName).ToArray();
			return new PropertyDescriptorCollection(properties3);
		}
		properties3 = (PropertyDescriptor[])(object)properties.ToArray();
		return SortProperties(component, propertySort, new PropertyDescriptorCollection(properties3));
	}

	private static List<StiPropertyDescriptor> ProcessReportProperties(object component, List<StiPropertyDescriptor> properties)
	{
		if (component is StiReport stiReport && stiReport.Pages.ContainsOnlyDashboards)
		{
			RemoveProperty("CacheAllData", properties);
			RemoveProperty("ConvertNulls", properties);
			RemoveProperty("ReportCacheMode", properties);
			RemoveProperty("RetrieveOnlyUsedData", properties);
			RemoveProperty("CachedTotals", properties);
			RemoveProperty("Collate", properties);
			RemoveProperty("EngineVersion", properties);
			RemoveProperty("NumberOfPass", properties);
			RemoveProperty("ReportUnit", properties);
			RemoveProperty("ScriptLanguage", properties);
			RemoveProperty("StopBeforePage", properties);
			RemoveProperty("PreviewMode", properties);
			RemoveProperty("PrinterSettings", properties);
		}
		return properties;
	}

	private static List<StiPropertyDescriptor> ProcessCrossProperties(object component, List<StiPropertyDescriptor> properties)
	{
		if (component is StiCrossSummary stiCrossSummary)
		{
			if (stiCrossSummary.Summary == StiSummaryType.Image)
			{
				RemoveProperty("AllowHtmlTags", properties);
				RemoveProperty("Angle", properties);
				RemoveProperty("Font", properties);
				RemoveProperty("HideZeros", properties);
				RemoveProperty("HorAlignment", properties);
				RemoveProperty("VertAlignment", properties);
				RemoveProperty("Margins", properties);
				RemoveProperty("TextFormat", properties);
				RemoveProperty("TextQuality", properties);
				RemoveProperty("TextBrush", properties);
				RemoveProperty("TextOptions", properties);
				RemoveProperty("WordWrap", properties);
			}
			else
			{
				RemoveProperty("AspectRatio", properties);
				RemoveProperty("ImageHorAlignment", properties);
				RemoveProperty("ImageVertAlignment", properties);
				RemoveProperty("Stretch", properties);
			}
		}
		if (component is StiCrossTab stiCrossTab)
		{
			if (!stiCrossTab.Wrap)
			{
				RemoveProperty("WrapGap", properties);
			}
			return properties;
		}
		return properties;
	}

	private static List<StiPropertyDescriptor> ProcessDashboardElementProperties(object[] selectedObjects, object component, List<StiPropertyDescriptor> properties)
	{
		if (component is IStiGaugeElement)
		{
			if (((IStiGaugeElement)component).CalculationMode == StiGaugeCalculationMode.Auto)
			{
				RemoveProperty("Minimum", properties);
				RemoveProperty("Maximum", properties);
			}
		}
		else if (component is IStiButtonElement stiButtonElement)
		{
			if (stiButtonElement.Type == StiButtonType.Button)
			{
				RemoveProperty("Group", properties);
				RemoveProperty("Checked", properties);
			}
			else if (stiButtonElement.Type == StiButtonType.CheckBox)
			{
				RemoveProperty("Group", properties);
			}
		}
		else if (component is IStiIndicatorElement)
		{
			bool flag = true;
			bool flag2 = false;
			IStiIndicatorElement stiIndicatorElement = (IStiIndicatorElement)component;
			if (stiIndicatorElement.GetSeries() != null || stiIndicatorElement.DataMode == StiDataMode.ManuallyEnteringData)
			{
				flag = false;
			}
			if (stiIndicatorElement.GetTarget() != null || stiIndicatorElement.DataMode == StiDataMode.ManuallyEnteringData)
			{
				flag2 = true;
			}
			if (!flag)
			{
				RemoveProperty("IconAlignment", properties);
			}
			if (!flag2)
			{
				RemoveProperty("TargetMode", properties);
			}
		}
		else if (component is StiChart)
		{
			StiChart stiChart = component as StiChart;
			if (!(stiChart.Area is IStiAxisArea))
			{
				RemoveProperty("XAxis", properties);
				RemoveProperty("XTopAxis", properties);
				RemoveProperty("YAxis", properties);
				RemoveProperty("YRightAxis", properties);
			}
			if (!(stiChart.Area is StiAxisArea))
			{
				RemoveProperty("ConstantLines", properties);
			}
		}
		else if (component is IStiChartElement)
		{
			IStiChartElement stiChartElement = component as IStiChartElement;
			if (!stiChartElement.IsAxisAreaChart)
			{
				RemoveProperty("ConstantLines", properties);
			}
			if (!stiChartElement.IsLinesChart)
			{
				RemoveProperty("Marker", properties);
			}
			if (!stiChartElement.IsParetoChart)
			{
				RemoveProperty("ParetoSeriesColors", properties);
			}
			if (!stiChartElement.IsPie3dChart)
			{
				RemoveProperty("Options3D", properties);
			}
			RemoveProperty("EndValues", properties);
		}
		else if (component is IStiChartArea)
		{
			object obj = selectedObjects?.Where((object o) => o is IStiChartElement).FirstOrDefault();
			if (obj != null && !(obj as IStiChartElement).IsAxisAreaChart)
			{
				RemoveProperty("ReverseHor", properties);
				RemoveProperty("ReverseVert", properties);
				RemoveProperty("GridLinesHor", properties);
				RemoveProperty("GridLinesVert", properties);
				RemoveProperty("InterlacingHor", properties);
				RemoveProperty("InterlacingVert", properties);
				RemoveProperty("XTopAxis", properties);
				RemoveProperty("YRightAxis", properties);
			}
			if (obj != null && !(obj as IStiChartElement).IsAxisAreaChart && !(obj as IStiChartElement).IsRadarChart)
			{
				RemoveProperty("XAxis", properties);
				RemoveProperty("YAxis", properties);
			}
		}
		else if (component is IStiYChartAxis)
		{
			object obj2 = selectedObjects?.Where((object o) => o is IStiChartElement).FirstOrDefault();
			if ((obj2 as IStiChartElement).IsRadarChart)
			{
				RemoveProperty("Title", properties);
				RemoveProperty("StartFromZero", properties);
			}
		}
		else if (component is IStiXChartAxis)
		{
			object obj3 = selectedObjects?.Where((object o) => o is IStiChartElement).FirstOrDefault();
			if ((obj3 as IStiChartElement).IsRadarChart)
			{
				RemoveProperty("Range", properties);
				RemoveProperty("ShowEdgeValues", properties);
				RemoveProperty("StartFromZero", properties);
				RemoveProperty("Title", properties);
			}
		}
		else if (component is IStiChartAxisLabels)
		{
			object obj4 = selectedObjects?.Where((object o) => o is IStiChartElement).FirstOrDefault();
			if ((obj4 as IStiChartElement).IsRadarChart)
			{
				RemoveProperty("Angle", properties);
				RemoveProperty("Placement", properties);
				RemoveProperty("TextAlignment", properties);
			}
		}
		else if (component is IStiChartLabels)
		{
			IStiChartLabels stiChartLabels = component as IStiChartLabels;
			if (stiChartLabels.Position == StiChartLabelsPosition.TwoColumns)
			{
				RemoveProperty("AutoRotate", properties);
			}
		}
		else if (component is IStiChartLegendLabels)
		{
			if (!(selectedObjects?.All((object c) => c is IStiChartElement && (((IStiChartElement)c).IsPieChart || ((IStiChartElement)c).IsPie3dChart || ((IStiChartElement)c).IsDoughnutChart))).GetValueOrDefault(false))
			{
				RemoveProperty("ValueType", properties);
			}
		}
		else if (component is IStiRegionMapElement)
		{
			IStiRegionMapElement stiRegionMapElement = (IStiRegionMapElement)component;
			if (stiRegionMapElement.DataFrom == StiMapSource.Manual)
			{
				RemoveProperty("KeyMeter", properties);
				RemoveProperty("NameMeter", properties);
				RemoveProperty("ValueMeter", properties);
				RemoveProperty("GroupMeter", properties);
				RemoveProperty("ColorMeter", properties);
			}
		}
		else if (component is IStiSurface)
		{
			IStiSurface stiSurface = (IStiSurface)component;
			if (stiSurface.IsDesktopSurfaceSelected)
			{
				RemoveProperty("DeviceWidth", properties);
			}
		}
		else if (component is StiAzureBlobStorageDatabase)
		{
			StiAzureBlobStorageDatabase stiAzureBlobStorageDatabase = component as StiAzureBlobStorageDatabase;
			if (!stiAzureBlobStorageDatabase.BlobContentType.Equals("CSV"))
			{
				RemoveProperty("CodePage", properties);
				RemoveProperty("Delimiter", properties);
			}
			if (!stiAzureBlobStorageDatabase.BlobContentType.Equals("Excel"))
			{
				RemoveProperty("FirstRowIsHeader", properties);
			}
		}
		return properties;
	}

	private static List<StiPropertyDescriptor> ProcessStyleProperties(object component, List<StiPropertyDescriptor> properties)
	{
		if (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles)
		{
			return properties;
		}
		if (component == null)
		{
			return properties;
		}
		if (component is StiChart { AllowApplyStyle: not false })
		{
			RemoveProperty("Brush", properties);
		}
		if (component is StiComponent)
		{
			if (!(((StiComponent)component).GetComponentStyle() is StiStyle stiStyle))
			{
				return properties;
			}
			if (stiStyle.AllowUseBorderFormatting && stiStyle.AllowUseBorderSides)
			{
				RemoveProperty("Border", properties);
			}
			if (stiStyle.AllowUseBrush)
			{
				RemoveProperty("Brush", properties);
			}
			if (stiStyle.AllowUseFont)
			{
				RemoveProperty("Font", properties);
			}
			if (stiStyle.AllowUseHorAlignment)
			{
				RemoveProperty("HorAlignment", properties);
			}
			if (stiStyle.AllowUseVertAlignment)
			{
				RemoveProperty("VertAlignment", properties);
			}
			if (stiStyle.AllowUseTextBrush)
			{
				RemoveProperty("TextBrush", properties);
			}
			if (stiStyle.AllowUseImage)
			{
				RemoveProperty("Image", properties);
			}
			return properties;
		}
		StiSeries stiSeries = component as StiSeries;
		if (stiSeries is StiPieSeries stiPieSeries)
		{
			if (stiSeries.AllowApplyStyle)
			{
				RemoveProperty("BorderThickness", properties);
				RemoveProperty("ShowShadow", properties);
				RemoveProperty("Lighting", properties);
			}
			if (stiPieSeries.AllowApplyBorderColor)
			{
				RemoveProperty("BorderColor", properties);
			}
			if (stiPieSeries.AllowApplyBrush)
			{
				RemoveProperty("Brush", properties);
			}
			return properties;
		}
		if (stiSeries != null)
		{
			if (stiSeries.AllowApplyStyle)
			{
				RemoveProperty("Brush", properties);
				RemoveProperty("BorderColor", properties);
				RemoveProperty("BorderThickness", properties);
				RemoveProperty("ShowShadow", properties);
				RemoveProperty("Lighting", properties);
				if (!(stiSeries is IStiParetoSeries))
				{
					RemoveProperty("LineColor", properties);
				}
			}
			if (stiSeries is IStiAllowApplyBrushNegative && !((IStiAllowApplyBrushNegative)stiSeries).AllowApplyBrushNegative)
			{
				RemoveProperty("BrushNegative", properties);
			}
			if (stiSeries is IStiAllowApplyColorNegative && !((IStiAllowApplyColorNegative)stiSeries).AllowApplyColorNegative)
			{
				RemoveProperty("LineColorNegative", properties);
			}
			if (stiSeries is IStiParetoSeries && !((IStiParetoSeries)stiSeries).AllowApplyLineColor)
			{
				RemoveProperty("LineColor", properties);
			}
			return properties;
		}
		if (component is StiTrendLine { AllowApplyStyle: not false })
		{
			RemoveProperty("LineColor", properties);
			RemoveProperty("ShowShadow", properties);
			return properties;
		}
		if (component is StiSeriesLabels { AllowApplyStyle: not false })
		{
			RemoveProperty("LineColor", properties);
			RemoveProperty("Antialiasing", properties);
			RemoveProperty("LabelColor", properties);
			RemoveProperty("BorderColor", properties);
			RemoveProperty("Brush", properties);
			RemoveProperty("Font", properties);
			return properties;
		}
		if (component is StiChartTable { AllowApplyStyle: not false })
		{
			RemoveProperty("TextColor", properties);
			RemoveProperty("GridLineColor", properties);
			RemoveProperty("Font", properties);
		}
		if (component is StiConstantLines { AllowApplyStyle: not false })
		{
			RemoveProperty("Antialiasing", properties);
			RemoveProperty("Font", properties);
			RemoveProperty("LineColor", properties);
			return properties;
		}
		if (component is StiStrips { AllowApplyStyle: not false })
		{
			RemoveProperty("StripBrush", properties);
			RemoveProperty("Antialiasing", properties);
			RemoveProperty("Font", properties);
			RemoveProperty("TitleColor", properties);
			return properties;
		}
		if (component is StiAxis { AllowApplyStyle: not false })
		{
			RemoveProperty("LineColor", properties);
			RemoveProperty("MinorColor", properties);
			RemoveProperty("Style", properties);
			RemoveProperty("MinorStyle", properties);
			return properties;
		}
		if (component is StiRadarAxis { AllowApplyStyle: not false })
		{
			RemoveProperty("LineColor", properties);
			return properties;
		}
		if (component is StiInterlacing { AllowApplyStyle: not false })
		{
			RemoveProperty("InterlacedBrush", properties);
			RemoveProperty("Visible", properties);
			return properties;
		}
		if (component is StiGridLines { AllowApplyStyle: not false })
		{
			RemoveProperty("Color", properties);
			RemoveProperty("MinorColor", properties);
			RemoveProperty("Style", properties);
			RemoveProperty("MinorStyle", properties);
			return properties;
		}
		if (component is StiRadarGridLines { AllowApplyStyle: not false })
		{
			RemoveProperty("Color", properties);
			return properties;
		}
		if (component is StiLegend { AllowApplyStyle: not false })
		{
			RemoveProperty("ShowShadow", properties);
			RemoveProperty("BorderColor", properties);
			RemoveProperty("Brush", properties);
			RemoveProperty("TitleColor", properties);
			RemoveProperty("LabelsColor", properties);
			RemoveProperty("TitleFont", properties);
			RemoveProperty("Font", properties);
			return properties;
		}
		if (component is StiArea { AllowApplyStyle: not false })
		{
			RemoveProperty("ShowShadow", properties);
			RemoveProperty("BorderColor", properties);
			RemoveProperty("Brush", properties);
			return properties;
		}
		if (component is StiChartTitle { AllowApplyStyle: not false })
		{
			RemoveProperty("Antialiasing", properties);
			RemoveProperty("Brush", properties);
			RemoveProperty("Font", properties);
			return properties;
		}
		return properties;
	}

	private static void RemoveProperty(string name, List<StiPropertyDescriptor> props)
	{
		for (int i = 0; i < props.Count; i++)
		{
			StiPropertyDescriptor val = props[i];
			if (((MemberDescriptor)(object)val).Name == name)
			{
				props.RemoveAt(i);
				break;
			}
		}
	}

	private static PropertyDescriptorCollection SortProperties(object component, PropertySort propertySort, PropertyDescriptorCollection properties)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		if ((int)propertySort != 3 && (int)propertySort != 2)
		{
			return properties;
		}
		List<OrderData> list = new List<OrderData>();
		Hashtable hashtable = new Hashtable();
		foreach (PropertyDescriptor property in properties)
		{
			if (StiPropertyGrid.IsAllowedProperty(component.GetType(), property.Name))
			{
				Attribute? attribute = property.Attributes[typeof(StiOrderAttribute)];
				StiOrderAttribute val = (StiOrderAttribute)(object)((attribute is StiOrderAttribute) ? attribute : null);
				Attribute? attribute2 = property.Attributes[typeof(StiCategoryAttribute)];
				StiCategoryAttribute val2 = (StiCategoryAttribute)(object)((attribute2 is StiCategoryAttribute) ? attribute2 : null);
				string text = ((val2 != null) ? val2.Category : "Misc");
				int num = StiCategoryIndexHelper.GetCategoryIndex(text);
				if (text == "Design" && component is StiReport)
				{
					num = 0;
				}
				int position = ((val != null) ? val.Position : 0);
				if (hashtable[text] == null && num != int.MaxValue)
				{
					hashtable[text] = num;
				}
				list.Add(new OrderData(text, property.Name, position, num));
			}
		}
		string[] array = new string[hashtable.Count];
		hashtable.Keys.CopyTo(array, 0);
		List<CategoryData> list2 = new List<CategoryData>();
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			list2.Add(new CategoryData(text2, (int)hashtable[text2]));
		}
		list2.Sort();
		int num2 = 1;
		foreach (CategoryData item in list2)
		{
			hashtable[item.CategoryName] = num2++;
		}
		int num3 = num2;
		foreach (OrderData item2 in list)
		{
			int val3 = num2;
			if (hashtable[item2.CategoryName] != null)
			{
				val3 = (int)hashtable[item2.CategoryName];
			}
			else
			{
				hashtable[item2.CategoryName] = num2;
				num2++;
			}
			num3 = Math.Max(val3, num3);
		}
		foreach (OrderData item3 in list)
		{
			int num4 = num2;
			if (hashtable[item3.CategoryName] != null)
			{
				num4 = (int)hashtable[item3.CategoryName];
			}
			else
			{
				hashtable[item3.CategoryName] = num2;
				num2++;
			}
			((StiPropertyDescriptor)properties[item3.Name]).Level = num4;
			((StiPropertyDescriptor)properties[item3.Name]).MaxLevel = num3;
			item3.CategoryPosition = num4;
		}
		list.Sort();
		if (component is Font)
		{
			properties = properties.Sort(new string[6] { "Name", "Size", "Bold", "Italic", "Underline", "Strikeout" });
		}
		else
		{
			string[] names = list.Select((OrderData orderData) => orderData.Name).ToArray();
			properties = properties.Sort(names);
		}
		return properties;
	}

	private static PropertyDescriptorCollection ProcessDataBindingsProperties(object component)
	{
		PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
		StiDataBindingsCollection stiDataBindingsCollection = component as StiDataBindingsCollection;
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(stiDataBindingsCollection.Control);
		foreach (PropertyDescriptor item in properties)
		{
			if (StiPropertyGrid.IsAllowedProperty(component.GetType(), item.Name) && !(item.Attributes[typeof(BindableAttribute)] is BindableAttribute { Bindable: false }))
			{
				propertyDescriptorCollection.Add((PropertyDescriptor)(object)new StiDataBindingPropertyDescriptor(item));
			}
		}
		return propertyDescriptorCollection;
	}

	private static void ProcessBrowsableAttribute(object component, PropertyDescriptor prop, List<StiPropertyDescriptor> properties)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		if (component is IStiReport)
		{
			Attribute? attribute = prop.Attributes[typeof(StiBrowsableAttribute)];
			StiBrowsableAttribute val = (StiBrowsableAttribute)(object)((attribute is StiBrowsableAttribute) ? attribute : null);
			if (val == null || val.Browsable)
			{
				properties.Add(new StiPropertyDescriptor(prop));
			}
		}
		else if (!(prop.Attributes[typeof(BrowsableAttribute)] is BrowsableAttribute { Browsable: false }))
		{
			properties.Add(new StiPropertyDescriptor(prop));
		}
	}

	private static bool CheckGuiMode(PropertyDescriptor prop, StiGuiMode guiMode)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Attribute? attribute = prop.Attributes[typeof(StiGuiModeAttribute)];
		StiGuiModeAttribute val = (StiGuiModeAttribute)(object)((attribute is StiGuiModeAttribute) ? attribute : null);
		if (val != null && val.Mode != guiMode)
		{
			return !ShowAllPropertiesInWpf;
		}
		return false;
	}

	private static bool CheckEngineVersion(PropertyDescriptor prop, object component)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		if (component is IStiEngineVersionProperty || component is IStiReportProperty)
		{
			IStiEngineVersionProperty val = (IStiEngineVersionProperty)((component is IStiEngineVersionProperty) ? component : null);
			if (component is IStiReportProperty)
			{
				object report = ((IStiReportProperty)component).GetReport();
				val = (IStiEngineVersionProperty)((report is IStiEngineVersionProperty) ? report : null);
			}
			if (val == null)
			{
				return true;
			}
			Attribute? attribute = prop.Attributes[typeof(StiEngineAttribute)];
			StiEngineAttribute val2 = (StiEngineAttribute)(object)((attribute is StiEngineAttribute) ? attribute : null);
			if (val2 != null && val2.Version != val.EngineVersion)
			{
				return (int)val2.Version > 0;
			}
			return false;
		}
		return false;
	}

	private static bool CheckAllowedProperty(PropertyDescriptor prop, object component)
	{
		return !StiPropertyGrid.IsAllowedProperty(component.GetType(), prop.Name);
	}

	private static bool CheckPropertyLevel(PropertyDescriptor prop)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		if (StiPropertyGridOptions.PropertyLevel != (StiLevel?)0 && StiPropertyGridOptions.PropertyLevel != (StiLevel?)1)
		{
			return false;
		}
		if (!(prop.Attributes[typeof(StiPropertyLevelAttribute)] is StiPropertyLevelAttribute stiPropertyLevelAttribute))
		{
			return false;
		}
		if (StiPropertyGridOptions.PropertyLevel == (StiLevel?)0)
		{
			if ((int)stiPropertyLevelAttribute.Level == 2 || (int)stiPropertyLevelAttribute.Level == 1)
			{
				return true;
			}
		}
		else if (StiPropertyGridOptions.PropertyLevel == (StiLevel?)1 && (int)stiPropertyLevelAttribute.Level == 2)
		{
			return true;
		}
		return false;
	}
}
