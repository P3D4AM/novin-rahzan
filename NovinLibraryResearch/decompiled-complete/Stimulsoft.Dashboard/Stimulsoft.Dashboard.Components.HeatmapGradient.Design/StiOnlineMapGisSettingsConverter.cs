using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.HeatmapGradient.Design;

public class StiOnlineMapGisSettingsConverter : StiOrderConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return "(GIS)";
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiOnlineMapGisSettings stiOnlineMapGisSettings = value as StiOnlineMapGisSettings;
			Type[] types = new Type[6]
			{
				typeof(Color),
				typeof(double),
				typeof(bool),
				typeof(StiLanguageType),
				typeof(StiFontIcons),
				typeof(Color)
			};
			ConstructorInfo constructor = typeof(StiOnlineMapGisSettings).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[6] { stiOnlineMapGisSettings.Color, stiOnlineMapGisSettings.Size, stiOnlineMapGisSettings.ShowPlacemark, stiOnlineMapGisSettings.Language, stiOnlineMapGisSettings.Icon, stiOnlineMapGisSettings.IconColor };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
