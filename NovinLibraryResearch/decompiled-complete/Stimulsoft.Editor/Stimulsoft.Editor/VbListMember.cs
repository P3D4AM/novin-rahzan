using System.Globalization;

namespace Stimulsoft.Editor;

public class VbListMember : ListMember
{
	public VbListMember()
	{
	}

	public VbListMember(IListMembers owner)
		: base(owner)
	{
	}

	public override void UpdateParamText()
	{
		if (Parameters == null)
		{
			return;
		}
		IParameterMember[] array = Parameters;
		foreach (IParameterMember parameterMember in array)
		{
			string text = parameterMember.Qualifier.ToLower(CultureInfo.InvariantCulture);
			ParameterModifer parameterModifer = parameterMember.Modifiers;
			if (text.IndexOf("byval") >= 0)
			{
				parameterModifer |= ParameterModifer.In;
			}
			if (text.IndexOf("byref") >= 0)
			{
				parameterModifer |= ParameterModifer.Retval;
			}
			if (text.IndexOf("optional") >= 0)
			{
				parameterModifer |= ParameterModifer.Optional;
			}
			string text2 = parameterMember.Name;
			string text3 = parameterMember.DataType;
			if (text3 != string.Empty)
			{
				if (text3[text3.Length - 1] == '&')
				{
					text3 = text3.Substring(0, text3.Length - 1);
					parameterModifer |= ParameterModifer.Retval;
				}
				text2 = text2 + " As " + text3;
			}
			if ((parameterModifer & ParameterModifer.In) != 0)
			{
				text2 = "ByVal " + text2;
			}
			else if ((parameterModifer & ParameterModifer.Out) != 0 || (parameterModifer & ParameterModifer.Retval) != 0)
			{
				text2 = "ByRef " + text2;
			}
			if ((parameterModifer & ParameterModifer.Optional) != 0)
			{
				text2 = "[" + text2 + "]";
			}
			parameterMember.Text = text2;
		}
	}

	public override string GetOwnDescription()
	{
		string text = base.OwnDescriptionText;
		if (text == null || text == string.Empty)
		{
			text = ((base.Qualifier + " " + DisplayText + " ").Trim() + ParamText).Trim();
			if (base.DataType != string.Empty)
			{
				text = text + " As " + base.DataType;
			}
			if (base.Overloads > 0)
			{
				text = text + " " + string.Format(StringConsts.MethodOverloads, base.Overloads);
			}
		}
		return text.Trim();
	}

	public override string GetParamText(bool useFormatting)
	{
		string text = base.GetParamText(useFormatting);
		if (text != null && text != string.Empty)
		{
			return text;
		}
		text = string.Empty;
		if (Parameters != null)
		{
			if (base.Owner != null)
			{
				useFormatting &= base.Owner.UseHtmlFormatting;
			}
			for (int i = 0; i < Parameters.Length; i++)
			{
				IParameterMember parameterMember = Parameters[i];
				string text2 = ((i == base.CurrentParamIndex && useFormatting) ? (SyntaxConsts.DefaultBoldTag + parameterMember.Text + SyntaxConsts.DefaultBoldEndTag) : parameterMember.Text);
				text = ((text != string.Empty) ? (text + ", " + text2) : text2);
			}
			text = "(" + text + ")";
		}
		return text;
	}
}
