OutFile "Apprenda-NetApp-Installer-v1.0.exe"

InstallDir "C:\Apprenda\Addons\Netapp"

Section 

SetOutPath $InstallDir

File test.txt

WriteUninstaller $InstallDir\Uninstall-Apprenda-NetApp-Integration.exe

SectionEnd

Section "Uninstall"


SectionEnd