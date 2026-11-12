using System;
using System.Reflection;
using Microsoft.SqlServer.Management.Sdk.Differencing.SPI;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal class SmoAvaliablePropertyProvider : AvailablePropertyValueProvider
{
	public override bool IsGraphSupported(ISfcSimpleNode node)
	{
		if (node == null)
		{
			throw new ArgumentNullException();
		}
		if (node.ObjectReference is SqlSmoObject && ((SqlSmoObject)node.ObjectReference).IsDesignMode)
		{
			return true;
		}
		return false;
	}

	public override bool IsValueAvailable(ISfcSimpleNode node, string propName)
	{
		if (node == null || string.IsNullOrEmpty(propName))
		{
			throw new ArgumentNullException();
		}
		if (node.ObjectReference is SqlSmoObject)
		{
			IAlienObject alienObject = node.ObjectReference as IAlienObject;
			try
			{
				Type propertyType = alienObject.GetPropertyType(propName);
				if (alienObject.GetPropertyValue(propName, propertyType) == null)
				{
					return false;
				}
			}
			catch (TargetInvocationException)
			{
				return false;
			}
		}
		return true;
	}
}
