using System.Collections;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class KeyList : IKeyList
{
	private enum KeyStates
	{
		BookMark = 1,
		Search,
		Outline,
		Select,
		BookMarkEx,
		SelectEx
	}

	private Hashtable states;

	private SyntaxEdit owner;

	private EventHandlers handlers;

	public EventHandlers Handlers => handlers;

	public KeyListData[] EventData
	{
		get
		{
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			foreach (Hashtable value in states.Values)
			{
				num += value.Values.Count;
			}
			KeyListData[] array = new KeyListData[num];
			int num2 = 0;
			foreach (Hashtable value2 in states.Values)
			{
				foreach (KeyListData value3 in value2.Values)
				{
					array[num2] = new KeyListData(value3.KeyData, value3.Action, value3.ActionEx, value3.Param, value3.State, value3.LeaveState);
					num2++;
				}
			}
			return array;
		}
	}

	public KeyList(SyntaxEdit owner)
	{
		this.owner = owner;
		handlers = new EventHandlers(owner);
		states = new Hashtable();
		Init();
	}

	private KeyListData GetKeyData(Keys keyData, int state)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		KeyListData keyListData = null;
		Hashtable hashtable = (Hashtable)states[state];
		if (hashtable != null)
		{
			keyListData = (KeyListData)hashtable[keyData];
		}
		if (keyListData == null)
		{
			hashtable = (Hashtable)states[-1];
			if (hashtable != null)
			{
				keyListData = (KeyListData)hashtable[keyData];
			}
		}
		return keyListData;
	}

	private void Init()
	{
		InitStdKeys();
		InitWordKeys();
		InitCodeCompletionKeys();
		InitMovements();
		InitSelection();
		InitOutlining();
		InitBookMarks();
		InitSearch();
		InitClipboard();
		InitMacros();
	}

	private void InitCodeCompletionKeys()
	{
		Add((Keys)262183, handlers.completeWordEvent);
		Add((Keys)131104, handlers.completeWordEvent);
		Add((Keys)196640, handlers.parameterInfoEvent);
		Add((Keys)196640, handlers.parameterInfoEvent);
		Add((Keys)131148, handlers.listMembersEvent, 1, 0);
		Add((Keys)76, handlers.listMembersEvent, 1, 0);
		Add((Keys)131145, handlers.quickInfoEvent, 1, 0);
		Add((Keys)73, handlers.quickInfoEvent, 1, 0);
		Add((Keys)131160, handlers.codeSnippetsEvent, 1, 0);
		Add((Keys)88, handlers.codeSnippetsEvent, 1, 0);
	}

	private void InitStdKeys()
	{
		Add((Keys)46, handlers.deleteRightEvent);
		Add((Keys)8, handlers.deleteLeftEvent);
		Add((Keys)65544, handlers.deleteLeftEvent);
		Add((Keys)131080, handlers.deleteWordLeftEvent);
		Add((Keys)131118, handlers.deleteWordRightEvent);
		Add((Keys)13, handlers.newLineEvent);
		Add((Keys)131085, handlers.newLineAboveEvent);
		Add((Keys)196621, handlers.newLineBelowEvent);
		Add((Keys)65549, handlers.newLineEvent);
		Add((Keys)27, handlers.processEscapeEvent);
		Add((Keys)9, handlers.processTabEvent);
		Add((Keys)65545, handlers.processShiftTabEvent);
		Add((Keys)262152, handlers.undoEvent);
		Add((Keys)327688, handlers.redoEvent);
		Add((Keys)131162, handlers.undoEvent);
		Add((Keys)196698, handlers.redoEvent);
		Add((Keys)45, handlers.toggleOverwriteEvent);
	}

	private void InitWordKeys()
	{
		Add((Keys)131156, handlers.charTransposeEvent);
		Add((Keys)196692, handlers.wordTransposeEvent);
		Add((Keys)327764, handlers.lineTransposeEvent);
	}

	private void InitBookMarks()
	{
		Add((Keys)131147, null, 0, 1);
		Add((Keys)131138, null, 0, 5);
		Add((Keys)131147, handlers.toggleBookMarkEvent, 1, 0);
		Add((Keys)131150, handlers.nextBookMarkEvent, 1, 0);
		Add((Keys)131152, handlers.prevBookMarkEvent, 1, 0);
		Add((Keys)78, handlers.nextBookMarkEvent, 1, 0);
		Add((Keys)80, handlers.prevBookMarkEvent, 1, 0);
		Add((Keys)75, handlers.toggleBookMarkEvent, 1, 0);
		Add((Keys)78, handlers.nextBookMarkEvent, 5, 0);
		Add((Keys)131150, handlers.nextBookMarkEvent, 5, 0);
		Add((Keys)80, handlers.prevBookMarkEvent, 5, 0);
		Add((Keys)131152, handlers.prevBookMarkEvent, 5, 0);
		Add((Keys)131120, handlers.toggleBookMarkEventEx, 0, 1, 0);
		Add((Keys)131121, handlers.toggleBookMarkEventEx, 1, 1, 0);
		Add((Keys)131122, handlers.toggleBookMarkEventEx, 2, 1, 0);
		Add((Keys)131123, handlers.toggleBookMarkEventEx, 3, 1, 0);
		Add((Keys)131124, handlers.toggleBookMarkEventEx, 4, 1, 0);
		Add((Keys)131125, handlers.toggleBookMarkEventEx, 5, 1, 0);
		Add((Keys)131126, handlers.toggleBookMarkEventEx, 6, 1, 0);
		Add((Keys)131127, handlers.toggleBookMarkEventEx, 7, 1, 0);
		Add((Keys)131128, handlers.toggleBookMarkEventEx, 8, 1, 0);
		Add((Keys)131129, handlers.toggleBookMarkEventEx, 9, 1, 0);
		Add((Keys)48, handlers.toggleBookMarkEventEx, 0, 1, 0);
		Add((Keys)49, handlers.toggleBookMarkEventEx, 1, 1, 0);
		Add((Keys)50, handlers.toggleBookMarkEventEx, 2, 1, 0);
		Add((Keys)51, handlers.toggleBookMarkEventEx, 3, 1, 0);
		Add((Keys)52, handlers.toggleBookMarkEventEx, 4, 1, 0);
		Add((Keys)53, handlers.toggleBookMarkEventEx, 5, 1, 0);
		Add((Keys)54, handlers.toggleBookMarkEventEx, 6, 1, 0);
		Add((Keys)55, handlers.toggleBookMarkEventEx, 7, 1, 0);
		Add((Keys)56, handlers.toggleBookMarkEventEx, 8, 1, 0);
		Add((Keys)57, handlers.toggleBookMarkEventEx, 9, 1, 0);
		Add((Keys)131120, handlers.moveBookMarkEvent, 0);
		Add((Keys)131121, handlers.moveBookMarkEvent, 1);
		Add((Keys)131122, handlers.moveBookMarkEvent, 2);
		Add((Keys)131123, handlers.moveBookMarkEvent, 3);
		Add((Keys)131124, handlers.moveBookMarkEvent, 4);
		Add((Keys)131125, handlers.moveBookMarkEvent, 5);
		Add((Keys)131126, handlers.moveBookMarkEvent, 6);
		Add((Keys)131127, handlers.moveBookMarkEvent, 7);
		Add((Keys)131128, handlers.moveBookMarkEvent, 8);
		Add((Keys)131129, handlers.moveBookMarkEvent, 9);
	}

	private void InitMovements()
	{
		Add((Keys)38, handlers.moveLineUpEvent);
		Add((Keys)40, handlers.moveLineDownEvent);
		Add((Keys)37, handlers.moveCharLeftEvent);
		Add((Keys)39, handlers.moveCharRightEvent);
		Add((Keys)33, handlers.movePageUpEvent);
		Add((Keys)34, handlers.movePageDownEvent);
		Add((Keys)131105, handlers.moveScreenTopEvent);
		Add((Keys)131106, handlers.moveScreenBottomEvent);
		Add((Keys)131108, handlers.moveFileBeginEvent);
		Add((Keys)131107, handlers.moveFileEndEvent);
		Add((Keys)36, handlers.moveLineBeginEvent);
		Add((Keys)35, handlers.moveLineEndEvent);
		Add((Keys)131109, handlers.moveWordLeftEvent);
		Add((Keys)131111, handlers.moveWordRightEvent);
		Add((Keys)131110, handlers.scrollLineUpEvent);
		Add((Keys)131112, handlers.scrollLineDownEvent);
		Add((Keys)131293, handlers.moveToBraceEvent);
		Add((Keys)196829, handlers.moveToBraceExtendEvent);
	}

	private void InitOutlining()
	{
		Add((Keys)131149, null, 0, 3);
		Add((Keys)131148, handlers.toggleOutliningEvent, 3, 0);
		Add((Keys)131149, handlers.toggleOutliningSelectionEvent, 3, 0);
		Add((Keys)131151, handlers.collapseToDefinitionsEvent, 3, 0);
	}

	private void InitSelection()
	{
		Add((Keys)131141, null, 0, 6);
		Add((Keys)65574, handlers.selectLineUpEvent, SelectionType.Stream);
		Add((Keys)65576, handlers.selectLineDownEvent, SelectionType.Stream);
		Add((Keys)65573, handlers.selectCharLeftEvent, SelectionType.Stream);
		Add((Keys)65575, handlers.selectCharRightEvent, SelectionType.Stream);
		Add((Keys)65569, handlers.selectPageUpEvent, SelectionType.Stream);
		Add((Keys)65570, handlers.selectPageDownEvent, SelectionType.Stream);
		Add((Keys)196641, handlers.selectScreenTopEvent, SelectionType.Stream);
		Add((Keys)196642, handlers.selectScreenBottomEvent, SelectionType.Stream);
		Add((Keys)196644, handlers.selectFileBeginEvent, SelectionType.Stream);
		Add((Keys)196643, handlers.selectFileEndEvent, SelectionType.Stream);
		Add((Keys)65572, handlers.selectLineBeginEvent, SelectionType.Stream);
		Add((Keys)65571, handlers.selectLineEndEvent, SelectionType.Stream);
		Add((Keys)196645, handlers.selectWordLeftEvent, SelectionType.Stream);
		Add((Keys)196647, handlers.selectWordRightEvent, SelectionType.Stream);
		Add((Keys)131159, handlers.selectWordEvent);
		Add((Keys)131137, handlers.selectAllEvent);
		Add((Keys)131153, handlers.tabifyEvent);
		Add((Keys)196689, handlers.unTabifyEvent);
		Add((Keys)131157, handlers.lowerCaseEvent);
		Add((Keys)196693, handlers.upperCaseEvent);
		Add((Keys)196675, handlers.capitalizeEvent);
		Add((Keys)131292, handlers.deleteWhiteSpaceEvent, 1, 0);
		Add((Keys)131139, handlers.commentSelectionEvent, 1, 0);
		Add((Keys)131157, handlers.uncommentSelectionEvent, 1, 0);
		Add((Keys)67, handlers.commentSelectionEvent, 6, 0);
		Add((Keys)67, handlers.commentSelectionEvent, 6, 0);
		Add((Keys)131139, handlers.commentSelectionEvent, 6, 0);
		Add((Keys)85, handlers.uncommentSelectionEvent, 6, 0);
		Add((Keys)131157, handlers.uncommentSelectionEvent, 6, 0);
		Add((Keys)327718, handlers.selectLineUpEvent, SelectionType.Block);
		Add((Keys)327720, handlers.selectLineDownEvent, SelectionType.Block);
		Add((Keys)327717, handlers.selectCharLeftEvent, SelectionType.Block);
		Add((Keys)327719, handlers.selectCharRightEvent, SelectionType.Block);
		Add((Keys)327713, handlers.selectPageUpEvent, SelectionType.Block);
		Add((Keys)327714, handlers.selectPageDownEvent, SelectionType.Block);
		Add((Keys)458788, handlers.selectFileBeginEvent, SelectionType.Block);
		Add((Keys)458787, handlers.selectFileEndEvent, SelectionType.Block);
		Add((Keys)327716, handlers.selectLineBeginEvent, SelectionType.Block);
		Add((Keys)327715, handlers.selectLineEndEvent, SelectionType.Block);
		Add((Keys)458789, handlers.selectWordLeftEvent, SelectionType.Block);
		Add((Keys)458791, handlers.selectWordRightEvent, SelectionType.Block);
		Add((Keys)458785, handlers.selectScreenTopEvent, SelectionType.Block);
		Add((Keys)458786, handlers.selectScreenBottomEvent, SelectionType.Block);
		Add((Keys)131154, null, 0, 4);
		Add((Keys)131152, handlers.swapAnchorEvent, 4, 0);
		Add((Keys)131142, handlers.formatSelectionEvent, 1, 0);
		Add((Keys)131140, handlers.formatDocumentEvent, 1, 0);
	}

	private void InitSearch()
	{
		Add((Keys)131142, handlers.findEvent);
		Add((Keys)114, handlers.findNextEvent);
		Add((Keys)131186, handlers.findNextSelectedEvent);
		Add((Keys)65650, handlers.findPreviousEvent);
		Add((Keys)196722, handlers.findPreviousSelectedEvent);
		Add((Keys)131144, handlers.replaceEvent);
		Add((Keys)131143, handlers.gotolineEvent);
		Add((Keys)131145, handlers.initIncrementalSearchEvent);
		Add((Keys)196681, handlers.initReverseIncrementalSearchEvent);
		Add((Keys)262258, null, 0, 2);
		Add((Keys)72, handlers.toggleHiddenTextEvent, 2, 0);
		Add((Keys)67, handlers.toggleMatchCaseEvent, 2, 0);
		Add((Keys)82, handlers.toggleRegularExpressionsEvent, 2, 0);
		Add((Keys)66, handlers.toggleSearchUpEvent, 2, 0);
		Add((Keys)87, handlers.toggleWholeWordEvent, 2, 0);
	}

	private void InitClipboard()
	{
		Add((Keys)131139, handlers.copyEvent);
		Add((Keys)131117, handlers.copyEvent);
		Add((Keys)131160, handlers.cutEvent);
		Add((Keys)65582, handlers.cutEvent);
		Add((Keys)131158, handlers.pasteEvent);
		Add((Keys)65581, handlers.pasteEvent);
		Add((Keys)131148, handlers.lineCutEvent);
		Add((Keys)196684, handlers.lineDeleteEvent);
	}

	private void InitMacros()
	{
		Add((Keys)196690, handlers.toggleMacroRecording);
		Add((Keys)196688, handlers.playBackMacro);
	}

	public void Add(Keys keyData, KeyEvent action, KeyEventEx actionEx, object param, int state, int leaveState)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Hashtable hashtable = (Hashtable)states[state];
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			states.Add(state, hashtable);
		}
		hashtable[keyData] = new KeyListData(keyData, action, actionEx, param, state, leaveState);
	}

	public void Add(Keys keyData, KeyEventEx action, object param, int state, int leaveState)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Add(keyData, null, action, param, state, leaveState);
	}

	public void Add(Keys keyData, KeyEvent action, int state, int leaveState)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Add(keyData, action, null, null, state, leaveState);
	}

	public void Add(Keys keyData, KeyEventEx action, object param)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Add(keyData, null, action, param, -1, 0);
	}

	public void Add(Keys keyData, KeyEvent action)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Add(keyData, action, null, null, -1, 0);
	}

	public void AddNormal(Keys keyData, KeyEventEx action, object param)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Add(keyData, null, action, param, 0, 0);
	}

	public void AddNormal(Keys keyData, KeyEvent action)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Add(keyData, action, null, null, 0, 0);
	}

	public void Remove(Keys keyData, int state)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		((Hashtable)states[state])?.Remove(keyData);
	}

	public void Remove(Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Remove(keyData, -1);
	}

	public void Clear()
	{
		states.Clear();
	}

	public bool FindKey(Keys keyData, int state)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetKeyData(keyData, state) != null;
	}

	public bool ExecuteKey(Keys keyData, ref int state)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		KeyListData keyData2 = GetKeyData(keyData, state);
		if (keyData2 != null)
		{
			if (owner.MacroRecording)
			{
				owner.RecordKeyData(new MacroKeyListData(keyData2.KeyData, keyData2.Action, keyData2.ActionEx, keyData2.Param, keyData2.State, keyData2.LeaveState));
			}
			if (keyData2.Action != null)
			{
				keyData2.Action();
			}
			else if (keyData2.ActionEx != null)
			{
				keyData2.ActionEx(keyData2.Param);
			}
			state = keyData2.LeaveState;
			return true;
		}
		return false;
	}
}
