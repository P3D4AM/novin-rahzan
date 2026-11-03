using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomRelations
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report)
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0246: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Expected O, but got Unknown
		//IL_028d: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04ce: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Expected O, but got Unknown
		//IL_050b: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_0511: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Expected O, but got Unknown
		if (report.Dictionary.Relations.Count <= 0)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		foreach (StiDataRelation relation in report.Dictionary.Relations)
		{
			if (relation.ParentSource == null || relation.ChildSource == null)
			{
				continue;
			}
			string parentRelationName = StiCodeDomSerializator.GetParentRelationName(report, relation.Name);
			string text = parentRelationName;
			int num = 1;
			while (hashtable[text] != null)
			{
				text = parentRelationName + num++;
			}
			parentRelationName = text;
			hashtable[parentRelationName] = parentRelationName;
			CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[relation.ParentColumns.Length];
			int num2 = 0;
			string[] parentColumns = relation.ParentColumns;
			foreach (string text2 in parentColumns)
			{
				array[num2++] = (CodeExpression)new CodePrimitiveExpression((object)text2);
			}
			CodeExpression[] array2 = (CodeExpression[])(object)new CodeExpression[relation.ChildColumns.Length];
			num2 = 0;
			string[] childColumns = relation.ChildColumns;
			foreach (string text3 in childColumns)
			{
				array2[num2++] = (CodeExpression)new CodePrimitiveExpression((object)text3);
			}
			List<CodeExpression> list = new List<CodeExpression>();
			list.Add((CodeExpression)new CodePrimitiveExpression((object)relation.NameInSource));
			list.Add((CodeExpression)new CodePrimitiveExpression((object)relation.Name));
			list.Add((CodeExpression)new CodePrimitiveExpression((object)relation.Alias));
			list.Add((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), StiNameValidator.CorrectName(relation.ParentSource.Name, report)));
			list.Add((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), StiNameValidator.CorrectName(relation.ChildSource.Name, report)));
			list.Add((CodeExpression)new CodeArrayCreateExpression(typeof(string), array));
			list.Add((CodeExpression)new CodeArrayCreateExpression(typeof(string), array2));
			List<CodeExpression> list2 = list;
			if (!string.IsNullOrWhiteSpace(relation.Key))
			{
				list2.Add((CodeExpression)new CodePrimitiveExpression((object)relation.Key));
			}
			if (relation.Active)
			{
				list2.Add((CodeExpression)new CodePrimitiveExpression((object)relation.Active));
			}
			serializator.memberMethod.Statements.Insert(0, (CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parentRelationName), (CodeExpression)new CodeObjectCreateExpression(new CodeTypeReference(relation.GetType()), list2.ToArray())));
			serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "Relations"), "Add", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parentRelationName) }));
			CodeMemberField val = new CodeMemberField(relation.GetType(), parentRelationName)
			{
				Attributes = (MemberAttributes)24576
			};
			if (Array.IndexOf(StiCodeDomSerializator.ReservedWords, parentRelationName) != -1)
			{
				((CodeTypeMember)val).Attributes = (MemberAttributes)(((CodeTypeMember)val).Attributes | 0x10);
			}
			serializator.Members.Insert(0, (CodeTypeMember)(object)val);
			CodeTypeDeclaration val2 = new CodeTypeDeclaration(parentRelationName + "Relation");
			CodeConstructor val3 = new CodeConstructor
			{
				Attributes = (MemberAttributes)24576
			};
			((CodeMemberMethod)val3).Parameters.Add(new CodeParameterDeclarationExpression(typeof(StiDataRow), "dataRow"));
			val3.BaseConstructorArgs.Add((CodeExpression)new CodeArgumentReferenceExpression("dataRow"));
			val2.Members.Add((CodeTypeMember)(object)val3);
			((CodeTypeMember)val2).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionStart("Relation " + parentRelationName));
			((CodeTypeMember)val2).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionEnd("Relation " + parentRelationName));
			val2.IsClass = true;
			val2.BaseTypes.Add(typeof(StiDataRow));
			val2.TypeAttributes = TypeAttributes.Public;
			List<CodeExpression> list3 = new List<CodeExpression>();
			foreach (StiDataColumn column in relation.ParentSource.Columns)
			{
				list3.Add(serializator.GetObjectCreateExpression(column.GetType(), column));
				CodeMemberProperty columnProperty = serializator.GetColumnProperty(column, buildRelations: true);
				if (column.Name == "Dictionary" || column.Name == "Row")
				{
					((CodeTypeMember)columnProperty).Attributes = (MemberAttributes)(((CodeTypeMember)columnProperty).Attributes | 0x10);
				}
				val2.Members.Add((CodeTypeMember)(object)columnProperty);
			}
			StiDataRelationsCollection parentRelations = relation.ParentSource.GetParentRelations();
			foreach (StiDataRelation item in parentRelations)
			{
				string parentRelationName2 = StiCodeDomSerializator.GetParentRelationName(report, item.Name);
				CodeMemberProperty val4 = new CodeMemberProperty
				{
					Attributes = (MemberAttributes)24576,
					HasGet = true,
					Name = StiNameValidator.CorrectName(item.Name, report),
					Type = new CodeTypeReference(parentRelationName2 + "Relation")
				};
				CodeObjectCreateExpression val5 = new CodeObjectCreateExpression(new CodeTypeReference(parentRelationName2 + "Relation"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "GetParentData", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)item.NameInSource) }) });
				val4.GetStatements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)(object)val5));
				val2.Members.Add((CodeTypeMember)(object)val4);
			}
			serializator.Members.Add((CodeTypeMember)(object)val2);
		}
	}
}
