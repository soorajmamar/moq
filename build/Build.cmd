msbuild ..\trunk\Source\Moq.csproj /p:Configuration=Release
msbuild ..\trunk\Extensions\Contrib\Source\Moq.Contrib.csproj /p:Configuration=Release
copy ..\trunk\Source\bin\Release\Moq.* ..\drops\
copy ..\trunk\Extensions\Contrib\Source\bin\Release\Moq.Contrib.* ..\drops\
"Sandcastle Help File Builder\SandcastleBuilderConsole.exe" ..\trunk\Moq.shfb -include=Moq,MockBehavior -exclude=Moq,MockBehavior,Normal -exclude=Moq,MockBehavior,Relaxed -exclude=Moq,IMock
copy Help\Moq.chm ..\drops\Moq.chm /Y