using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.TelemetryCore.Internal.Constants;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class ErrorStore : IErrorStore
{
	private readonly List<IPropertyBag> _propertyBagList = new List<IPropertyBag>();

	private readonly object _lockPropertyBagList = new object();

	public void Append(IErrorStore errorStore)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		lock (_lockPropertyBagList)
		{
			_propertyBagList.Clear();
		}
	}

	public IEnumerable<IPropertyBag> GetEventsForUpload()
	{
		lock (_lockPropertyBagList)
		{
			return _propertyBagList;
		}
	}

	public void ReportError(string errorMessage, ErrorType errorType, ErrorSeverity errorSeverity)
	{
		ReportError(errorMessage, errorType, errorSeverity, 1);
	}

	private void ReportError(string errorMessage, ErrorType errorType, ErrorSeverity errorSeverity, int count)
	{
		lock (_lockPropertyBagList)
		{
			if (!UpdateErrorCountIfPreviouslySeen(errorMessage, count))
			{
				DateTime utcNow = DateTime.UtcNow;
				PropertyBag propertyBag = new PropertyBag(EventType.LibraryError, null);
				propertyBag.Add(MatsErrorPropertyNames.TypeConstStrKey, (int)errorType);
				propertyBag.Add(MatsErrorPropertyNames.SeverityConstStrKey, (int)errorSeverity);
				propertyBag.Add(MatsErrorPropertyNames.ErrorMessageConstStrKey, errorMessage);
				propertyBag.Add(MatsErrorPropertyNames.TimestampConstStrKey, DateTimeUtils.GetMillisecondsSinceEpoch(utcNow));
				propertyBag.Add(MatsErrorPropertyNames.CountConstStrKey, count);
				_propertyBagList.Add(propertyBag);
			}
		}
	}

	private bool UpdateErrorCountIfPreviouslySeen(string errorMessage, int additionalCount)
	{
		lock (_lockPropertyBagList)
		{
			try
			{
				foreach (IPropertyBag propertyBag in _propertyBagList)
				{
					PropertyBagContents contents = propertyBag.GetContents();
					if (contents.StringProperties.ContainsKey(MatsErrorPropertyNames.ErrorMessageConstStrKey) && string.Compare(contents.StringProperties[MatsErrorPropertyNames.ErrorMessageConstStrKey], errorMessage, StringComparison.OrdinalIgnoreCase) == 0)
					{
						int num = (contents.IntProperties.ContainsKey(MatsErrorPropertyNames.CountConstStrKey) ? 1 : contents.IntProperties[MatsErrorPropertyNames.CountConstStrKey]);
						num += additionalCount;
						propertyBag.Update(MatsErrorPropertyNames.CountConstStrKey, num);
						return true;
					}
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
