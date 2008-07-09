msbuild ..\trunk\Source\Moq.csproj /p:Configuration=Release

xcopy ..\trunk\Source\bin\Release\Moq*.* ..\drops\Current\ /Y
xcopy ..\trunk\License.txt "..\drops\Current\" /Y /I
pushd ..\drops\Current\
ren License.txt "Moq License.txt" 
popd
"Sandcastle Help File Builder\SandcastleBuilderConsole.exe" ..\trunk\Moq.shfb -include=Moq,MockBehavior -exclude=Moq,MockBehavior,Normal -exclude=Moq,MockBehavior,Relaxed -exclude=Moq,IMocked

xcopy Help\Moq.chm ..\drops\Current\ /Y 