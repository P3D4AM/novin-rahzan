namespace Stimulsoft.Editor;

public class CsListMember : ListMember
{
	public CsListMember()
	{
	}

	public CsListMember(IListMembers owner)
		: base(owner)
	{
	}

	public override string GetOwnDescription()
	{
		string text = base.OwnDescriptionText;
		if (text == null || text == string.Empty)
		{
			text = (base.Qualifier + " " + (base.DataType + " " + DisplayText).Trim().Trim() + " " + ParamText).Trim();
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
