namespace Antlr.Runtime;

public class ParserRuleReturnScope<TToken> : IRuleReturnScope<TToken>, IRuleReturnScope
{
	private TToken _start;

	private TToken _stop;

	public TToken Start
	{
		get
		{
			return _start;
		}
		set
		{
			_start = value;
		}
	}

	public TToken Stop
	{
		get
		{
			return _stop;
		}
		set
		{
			_stop = value;
		}
	}

	object IRuleReturnScope.Start => Start;

	object IRuleReturnScope.Stop => Stop;
}
