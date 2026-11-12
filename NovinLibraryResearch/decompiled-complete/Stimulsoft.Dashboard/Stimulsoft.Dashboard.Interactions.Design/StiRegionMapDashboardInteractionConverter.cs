using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiRegionMapDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiRegionMapDashboardInteraction stiRegionMapDashboardInteraction = value as StiRegionMapDashboardInteraction;
			Type[] types = new Type[13]
			{
				typeof(StiInteractionOnHover),
				typeof(StiInteractionOnClick),
				typeof(StiInteractionOpenHyperlinkDestination),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(List<StiDashboardDrillDownParameter>),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			object[] objs = new object[13]
			{
				stiRegionMapDashboardInteraction.OnHover, stiRegionMapDashboardInteraction.OnClick, stiRegionMapDashboardInteraction.HyperlinkDestination, stiRegionMapDashboardInteraction.ToolTip, stiRegionMapDashboardInteraction.Hyperlink, stiRegionMapDashboardInteraction.DrillDownPageKey, stiRegionMapDashboardInteraction.DrillDownParameters, stiRegionMapDashboardInteraction.FileName, stiRegionMapDashboardInteraction.HeaderText, stiRegionMapDashboardInteraction.FooterText,
				stiRegionMapDashboardInteraction.ShowFullScreenButton, stiRegionMapDashboardInteraction.ShowSaveButton, stiRegionMapDashboardInteraction.ShowViewDataButton
			};
			ConstructorInfo constructor = typeof(StiRegionMapDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
