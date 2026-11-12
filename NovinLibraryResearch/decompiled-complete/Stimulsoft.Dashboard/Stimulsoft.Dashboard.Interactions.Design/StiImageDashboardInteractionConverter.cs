using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiImageDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiImageDashboardInteraction stiImageDashboardInteraction = value as StiImageDashboardInteraction;
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
			object[] objs = new object[7] { stiImageDashboardInteraction.OnHover, stiImageDashboardInteraction.OnClick, stiImageDashboardInteraction.HyperlinkDestination, stiImageDashboardInteraction.ToolTip, stiImageDashboardInteraction.Hyperlink, stiImageDashboardInteraction.DrillDownPageKey, stiImageDashboardInteraction.DrillDownParameters };
			ConstructorInfo constructor = typeof(StiImageDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
