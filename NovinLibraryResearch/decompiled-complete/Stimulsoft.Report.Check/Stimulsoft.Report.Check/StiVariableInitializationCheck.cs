using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiVariableInitializationCheck : StiVariableCheck
{
	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckVariable", "StiVariableInitializationCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckVariable", "StiVariableInitializationCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool CheckInitialization(StiVariable variable)
	{
		decimal result;
		return !decimal.TryParse(variable.Value, out result);
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		StiVariable stiVariable = obj as StiVariable;
		if (stiVariable?.Type != null && stiVariable.InitBy == StiVariableInitBy.Value && !string.IsNullOrWhiteSpace(stiVariable.Value) && stiVariable.Type.IsNumericType() && CheckInitialization(stiVariable))
		{
			StiVariableInitializationCheck stiVariableInitializationCheck = new StiVariableInitializationCheck();
			stiVariableInitializationCheck.Element = obj;
			stiVariableInitializationCheck.Actions.Add(new StiEditPropertyAction());
			return stiVariableInitializationCheck;
		}
		return null;
	}
}
