cd "../Game/Assets/Logic" 

if exist "wsconsole.exe" del "wsconsole.exe"
if exist "UnityIoC.IO.WSConsole.exe" (	
	ren "UnityIoC.IO.WSConsole.exe" "wsconsole.exe""
)
