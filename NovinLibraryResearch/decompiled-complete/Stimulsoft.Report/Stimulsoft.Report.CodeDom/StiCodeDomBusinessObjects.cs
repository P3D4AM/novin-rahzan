using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomBusinessObjects
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report)
	{
		foreach (StiBusinessObject businessObject in report.Dictionary.BusinessObjects)
		{
			businessObject.Dictionary = report.Dictionary;
			Serialize(serializator, report, businessObject, isParent: true, null, null);
		}
	}

	private static void Serialize(StiCodeDomSerializator serializator, StiReport report, StiBusinessObject data, bool isParent, CodeTypeDeclaration parentType, CodeConstructor parentConstructor)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009b: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0071: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Expected O, but got Unknown
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_038b: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_034f: Expected O, but got Unknown
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		string text = StiNameValidator.CorrectName(data.Name, report);
		string text2 = "BusinessObject";
		if (parentType != null && text + text2 == ((CodeTypeMember)parentType).Name)
		{
			text2 += "_";
		}
		CodeTypeReference val = new CodeTypeReference(text + text2);
		if (isParent)
		{
			serializator.memberMethod.Statements.Insert(0, (CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), (CodeExpression)new CodeObjectCreateExpression(val, (CodeExpression[])(object)new CodeExpression[0])));
		}
		else
		{
			((CodeMemberMethod)parentConstructor).Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), (CodeExpression)new CodeObjectCreateExpression(val, (CodeExpression[])(object)new CodeExpression[0])));
		}
		CodeTypeDeclaration val2 = new CodeTypeDeclaration(text + text2);
		CodeConstructor val3 = new CodeConstructor
		{
			Attributes = (MemberAttributes)24576
		};
		val3.BaseConstructorArgs.AddRange(new CodeExpressionCollection(serializator.GetArguments(data.GetType(), data)));
		val2.Members.Add((CodeTypeMember)(object)val3);
		((CodeTypeMember)val2).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionStart("BusinessObject " + text));
		((CodeTypeMember)val2).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionEnd("BusinessObject " + text));
		val2.IsClass = true;
		val2.BaseTypes.Add(data.GetType());
		val2.TypeAttributes = TypeAttributes.Public;
		List<CodeExpression> list = new List<CodeExpression>();
		foreach (StiDataColumn column in data.Columns)
		{
			list.Add(serializator.GetObjectCreateExpression(column.GetType(), column));
			CodeMemberProperty columnProperty = serializator.GetColumnProperty(column, buildRelations: false);
			((CodeTypeMember)columnProperty).Attributes = (MemberAttributes)24576;
			if (column.Name == "Name" || column.Name == "Position" || column.Name == "Inherited" || column.Name == "RealCount" || column.Name == "IsBof" || column.Name == "IsEof" || column.Name == "IsEmpty" || column.Name == "Alias")
			{
				((CodeTypeMember)columnProperty).Attributes = (MemberAttributes)(((CodeTypeMember)columnProperty).Attributes | 0x10);
			}
			val2.Members.Add((CodeTypeMember)(object)columnProperty);
		}
		if (list.Count > 0)
		{
			CodeExpression[] array = list.ToArray();
			CodeExpression[] array2 = (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArrayCreateExpression(typeof(StiDataColumn), array) };
			if (isParent)
			{
				serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), "Columns"), "AddRange", array2));
			}
			else
			{
				((CodeMemberMethod)parentConstructor).Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), "Columns"), "AddRange", array2));
			}
		}
		if (isParent)
		{
			serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "BusinessObjects"), "Add", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text) }));
		}
		else
		{
			((CodeMemberMethod)parentConstructor).Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "BusinessObjects"), "Add", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text) }));
		}
		foreach (StiBusinessObject businessObject in data.BusinessObjects)
		{
			Serialize(serializator, report, businessObject, isParent: false, val2, val3);
		}
		CodeMemberField val4 = new CodeMemberField(new CodeTypeReference(((CodeTypeMember)val2).Name), text)
		{
			Attributes = (MemberAttributes)24576
		};
		if (Array.IndexOf(StiCodeDomSerializator.ReservedWords, text) != -1)
		{
			((CodeTypeMember)val4).Attributes = (MemberAttributes)(((CodeTypeMember)val4).Attributes | 0x10);
		}
		if (isParent)
		{
			serializator.Members.Add((CodeTypeMember)(object)val4);
			serializator.Members.Add((CodeTypeMember)(object)val2);
		}
		else
		{
			parentType.Members.Add((CodeTypeMember)(object)val4);
			parentType.Members.Add((CodeTypeMember)(object)val2);
		}
	}
}
