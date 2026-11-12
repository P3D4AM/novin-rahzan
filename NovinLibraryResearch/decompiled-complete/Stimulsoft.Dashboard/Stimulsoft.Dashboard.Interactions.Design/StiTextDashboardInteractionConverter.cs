using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiTextDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiTextDashboardInteraction stiTextDashboardInteraction = value as StiTextDashboardInteraction;
			Type[] types = new Type[7]
			{
				typeof(StiInteractionOnHover),
				typeof(StiInteractionOnClick),
				typeof(StiInteractionOpenHyperlinkDestination),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(List<StiDashboardDrillDownParameter>)
			};
			object[] objs = new object[7] { stiTextDashboardInteraction.OnHover, stiTextDashboardInteraction.OnClick, stiTextDashboardInteraction.HyperlinkDestination, stiTextDashboardInteraction.ToolTip, stiTextDashboardInteraction.Hyperlink, stiTextDashboardInteraction.DrillDownPageKey, stiTextDashboardInteraction.DrillDownParameters };
			ConstructorInfo constructor = typeof(StiTextDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
