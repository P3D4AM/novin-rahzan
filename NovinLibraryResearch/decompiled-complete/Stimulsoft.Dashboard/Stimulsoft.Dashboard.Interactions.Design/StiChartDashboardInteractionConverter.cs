using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiChartDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiChartDashboardInteraction stiChartDashboardInteraction = value as StiChartDashboardInteraction;
			Type[] types = new Type[15]
			{
				typeof(StiInteractionOnHover),
				typeof(StiInteractionOnClick),
				typeof(StiInteractionOpenHyperlinkDestination),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(List<StiDashboardDrillDownParameter>),
				typeof(bool),
				typeof(bool),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			object[] objs = new object[15]
			{
				stiChartDashboardInteraction.OnHover, stiChartDashboardInteraction.OnClick, stiChartDashboardInteraction.HyperlinkDestination, stiChartDashboardInteraction.ToolTip, stiChartDashboardInteraction.Hyperlink, stiChartDashboardInteraction.DrillDownPageKey, stiChartDashboardInteraction.DrillDownParameters, stiChartDashboardInteraction.AllowUserDrillDown, stiChartDashboardInteraction.AllowUserSorting, stiChartDashboardInteraction.FileName,
				stiChartDashboardInteraction.HeaderText, stiChartDashboardInteraction.FooterText, stiChartDashboardInteraction.ShowFullScreenButton, stiChartDashboardInteraction.ShowSaveButton, stiChartDashboardInteraction.ShowViewDataButton
			};
			ConstructorInfo constructor = typeof(StiChartDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
