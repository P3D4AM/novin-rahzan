using System;
using System.Globalization;

namespace Stimulsoft.Report.Events;

public class StiGetCulturesEventArgs : EventArgs
{
	public CultureInfo[] Cultures { get; set; }
}
