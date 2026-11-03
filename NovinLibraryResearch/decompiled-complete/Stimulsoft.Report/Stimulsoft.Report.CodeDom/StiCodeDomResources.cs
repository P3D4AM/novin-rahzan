using System.CodeDom;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomResources
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		if (report.Dictionary.Resources.Count <= 0)
		{
			return;
		}
		foreach (StiResource resource in report.Dictionary.Resources)
		{
			if (!string.IsNullOrEmpty(resource.Name))
			{
				CodeObjectCreateExpression val = new CodeObjectCreateExpression(resource.GetType(), serializator.GetArguments(resource.GetType(), resource));
				serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "Resources"), "Add", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)val }));
			}
		}
	}
}
