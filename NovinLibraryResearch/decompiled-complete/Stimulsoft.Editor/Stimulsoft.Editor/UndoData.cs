using System.Drawing;

namespace Stimulsoft.Editor;

public class UndoData
{
	public UndoOperation Operation;

	public UpdateReason Reason;

	public int UpdateCount;

	public UndoFlags UndoFlag;

	public object Data;

	public Point Position;

	public UndoData(UndoOperation operation, object data)
	{
		Operation = operation;
		Data = data;
		Reason = UpdateReason.Other;
	}
}
