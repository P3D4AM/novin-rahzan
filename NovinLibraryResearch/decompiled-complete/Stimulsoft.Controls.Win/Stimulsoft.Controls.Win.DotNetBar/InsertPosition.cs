namespace Stimulsoft.Controls.Win.DotNetBar;

public class InsertPosition
{
	public int Position;

	public bool Before;

	public IDesignTimeProvider TargetProvider;

	public InsertPosition()
	{
		Position = -1;
		Before = false;
		TargetProvider = null;
	}

	public InsertPosition(int iPosition, bool bBefore, IDesignTimeProvider target)
	{
		Position = iPosition;
		Before = bBefore;
		TargetProvider = target;
	}

	public override string ToString()
	{
		if (TargetProvider == null)
		{
			return string.Format("TargetProvider={0}, Position={1}, Before={2}", "null", Position, Before);
		}
		return $"TargetProvider={TargetProvider}, Position={Position}, Before={Before}";
	}
}
