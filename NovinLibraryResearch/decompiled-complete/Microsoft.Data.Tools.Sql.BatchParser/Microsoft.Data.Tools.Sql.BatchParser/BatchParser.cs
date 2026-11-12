using System;
using System.IO;

namespace Microsoft.Data.Tools.Sql.BatchParser;

internal class BatchParser : ICommandHandler, IVariableResolver
{
	public delegate void HaltParserDelegate();

	public delegate void ScriptMessageDelegate(string message);

	public delegate void ScriptErrorDelegate(string message, ScriptMessageType messageType);

	public delegate bool ExecuteDelegate(string batchScript, int num, int lineNumber);

	protected ScriptMessageDelegate _scriptMessageDelegate;

	protected ScriptErrorDelegate _scriptErrorDelegate;

	protected ExecuteDelegate _executeDelegate;

	protected HaltParserDelegate _haltParserDelegate;

	private int _startingLine;

	protected bool _variableSubstitutionDisabled;

	public ScriptMessageDelegate Message
	{
		get
		{
			return _scriptMessageDelegate;
		}
		set
		{
			_scriptMessageDelegate = value;
		}
	}

	public ScriptErrorDelegate ErrorMessage
	{
		get
		{
			return _scriptErrorDelegate;
		}
		set
		{
			_scriptErrorDelegate = value;
		}
	}

	public ExecuteDelegate Execute
	{
		get
		{
			return _executeDelegate;
		}
		set
		{
			_executeDelegate = value;
		}
	}

	public HaltParserDelegate HaltParser
	{
		get
		{
			return _haltParserDelegate;
		}
		set
		{
			_haltParserDelegate = value;
		}
	}

	public int StartingLine
	{
		get
		{
			return _startingLine;
		}
		set
		{
			_startingLine = value;
		}
	}

	public BatchParserAction Go(TextBlock batch, int repeatCount)
	{
		batch.GetText(!_variableSubstitutionDisabled, out var text, out var lineInfo);
		bool flag = false;
		if (_executeDelegate != null)
		{
			flag = _executeDelegate(text, repeatCount, lineInfo.GetStreamPositionForOffset(0).Line + _startingLine - 1);
		}
		if (!flag)
		{
			return BatchParserAction.Abort;
		}
		return BatchParserAction.Continue;
	}

	protected void RaiseScriptError(string message, ScriptMessageType messageType)
	{
		if (_scriptErrorDelegate != null)
		{
			_scriptErrorDelegate(message, messageType);
		}
	}

	protected void RaiseScriptMessage(string message)
	{
		if (_scriptMessageDelegate != null)
		{
			_scriptMessageDelegate(message);
		}
	}

	protected void RaiseHaltParser()
	{
		if (_haltParserDelegate != null)
		{
			_haltParserDelegate();
		}
	}

	public virtual BatchParserAction OnError(Token token, OnErrorAction action)
	{
		throw new NotImplementedException("The method or operation is not implemented.");
	}

	public virtual BatchParserAction Include(TextBlock filename, out TextReader stream, out string newFilename)
	{
		throw new NotImplementedException("The method or operation is not implemented.");
	}

	public virtual string GetVariable(PositionStruct pos, string name)
	{
		throw new NotImplementedException("The method or operation is not implemented.");
	}

	public virtual void SetVariable(PositionStruct pos, string name, string value)
	{
		throw new NotImplementedException("The method or operation is not implemented.");
	}

	internal void DisableVariableSubstitution()
	{
		_variableSubstitutionDisabled = true;
	}
}
