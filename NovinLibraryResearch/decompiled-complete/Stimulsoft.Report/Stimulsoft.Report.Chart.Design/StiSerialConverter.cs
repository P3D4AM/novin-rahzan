using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart.Design;

public class StiSerialConverter : StiUniversalConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}
}
