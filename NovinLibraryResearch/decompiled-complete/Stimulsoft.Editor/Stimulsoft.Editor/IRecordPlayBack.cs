using System.IO;

namespace Stimulsoft.Editor;

public interface IRecordPlayBack
{
	bool MacroRecording { get; set; }

	bool MacroSuspendend { get; set; }

	MacroKeyListData[] MacroRecords { get; }

	void PlayBackMacro();

	void StartMacroRecording();

	void StopMacroRecording();

	void PauseMacroRecording();

	void ResumeMacroRecording();

	void ToggleMacroRecording();

	void SaveMacros(Stream stream);

	void SaveMacros(TextWriter writer);

	void SaveMacros(string fileName);

	void LoadMacros(Stream stream);

	void LoadMacros(TextReader reader);

	void LoadMacros(string fileName);
}
