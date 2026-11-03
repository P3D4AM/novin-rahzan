using System.Drawing;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public class StiVariableLoader
{
	public static Image GetImage(StiReport report, string name)
	{
		StiVariable stiVariable = report.Dictionary.Variables[name];
		if (stiVariable == null)
		{
			return null;
		}
		object valueObject = stiVariable.ValueObject;
		return (Image)((valueObject is Image) ? valueObject : null);
	}
}
