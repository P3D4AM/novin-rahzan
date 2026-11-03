using System;
using System.CodeDom;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomStandalone
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report, object standaloneReportType, CodeTypeDeclaration typeDeclaration, string name)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_009f: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00f1: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0139: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		if (standaloneReportType != null)
		{
			CodeMemberMethod val = new CodeMemberMethod
			{
				Attributes = (MemberAttributes)24579,
				Name = "Main"
			};
			val.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string[]), "args"));
			((CodeTypeMember)val).CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(STAThreadAttribute))));
			serializator.Members.Add((CodeTypeMember)(object)val);
			val.Statements.Add((CodeStatement)new CodeVariableDeclarationStatement(new CodeTypeReference(serializator.GetReportTypeName(name)), "report", (CodeExpression)new CodeObjectCreateExpression(serializator.GetReportTypeName(name), (CodeExpression[])(object)new CodeExpression[0])));
			CodeMethodInvokeExpression val2 = null;
			switch ((StiStandaloneReportType)standaloneReportType)
			{
			case StiStandaloneReportType.Show:
				val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeVariableReferenceExpression("report"), "Show", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)true) });
				break;
			case StiStandaloneReportType.Print:
				val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeVariableReferenceExpression("report"), "Print", (CodeExpression[])(object)new CodeExpression[0]);
				break;
			case StiStandaloneReportType.ShowWithWpf:
				val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeVariableReferenceExpression("report"), "ShowWithWpf", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)true) });
				break;
			case StiStandaloneReportType.PrintWithWpf:
				val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeVariableReferenceExpression("report"), "PrintWithWpf", (CodeExpression[])(object)new CodeExpression[0]);
				break;
			}
			val.Statements.Add((CodeExpression)(object)val2);
		}
	}
}
