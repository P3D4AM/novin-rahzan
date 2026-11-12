using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiPivotTableDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiPivotTableDashboardInteraction stiPivotTableDashboardInteraction = value as StiPivotTableDashboardInteraction;
			Type[] types = new Type[6]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			object[] objs = new object[6] { stiPivotTableDashboardInteraction.FileName, stiPivotTableDashboardInteraction.HeaderText, stiPivotTableDashboardInteraction.FooterText, stiPivotTableDashboardInteraction.ShowFullScreenButton, stiPivotTableDashboardInteraction.ShowSaveButton, stiPivotTableDashboardInteraction.ShowViewDataButton };
			ConstructorInfo constructor = typeof(StiPivotTableDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
