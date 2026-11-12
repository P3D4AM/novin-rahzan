using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiDashboardInteractionConverter : TypeConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
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
			if (((context?.Instance is IStiElementInteraction stiElementInteraction) ? stiElementInteraction.DashboardInteraction : null) == null)
			{
				return "(" + Loc.Get("FormStyleDesigner", "NotSpecified") + ")";
			}
			return "(" + Loc.GetMain("Interaction") + ")";
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiTableColumnDashboardInteraction stiTableColumnDashboardInteraction = value as StiTableColumnDashboardInteraction;
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
			object[] objs = new object[7] { stiTableColumnDashboardInteraction.OnHover, stiTableColumnDashboardInteraction.OnClick, stiTableColumnDashboardInteraction.HyperlinkDestination, stiTableColumnDashboardInteraction.ToolTip, stiTableColumnDashboardInteraction.Hyperlink, stiTableColumnDashboardInteraction.DrillDownPageKey, stiTableColumnDashboardInteraction.DrillDownParameters };
			ConstructorInfo constructor = typeof(StiDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
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
