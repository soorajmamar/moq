msbuild ..\trunk\Extensions\Contrib\Source\Moq.Contrib.csproj /p:Configuration=Release

copy ..\trunk\Extensions\Contrib\Source\bin\Release\Moq*.* ..\drops\Current
"Sandcastle Help File Builder\SandcastleBuilderConsole.exe" ..\trunk\Moq.shfb -include=Moq,MockBehavior -exclude=Moq,MockBehavior,Normal -exclude=Moq,MockBehavior,Relaxed -exclude=Moq,IMocked

copy Help\Moq.chm ..\drops\Current\Moq.chm /Y