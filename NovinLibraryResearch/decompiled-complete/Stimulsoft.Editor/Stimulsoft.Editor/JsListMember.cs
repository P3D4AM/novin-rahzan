namespace Stimulsoft.Editor;

public class JsListMember : ListMember
{
	public JsListMember()
	{
	}

	public JsListMember(IListMembers owner)
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
			string text = (parameterMember.Qualifier + " " + parameterMember.Name).Trim();
			if (parameterMember.DataType != string.Empty)
			{
				text = text + " : " + parameterMember.DataType;
			}
			parameterMember.Text = text;
		}
	}

	public override string GetOwnDescription()
	{
		string text = base.OwnDescriptionText;
		if (text == null || text == string.Empty)
		{
			text = ((base.Qualifier + " " + DisplayText).Trim() + " " + ParamText).Trim();
			if (base.DataType != null && base.DataType != string.Empty)
			{
				text = text + " : " + base.DataType;
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
