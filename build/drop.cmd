msbuild ..\trunk\Source\Moq.csproj /t:Rebuild /p:Configuration=Release /p:TargetFrameworkVersion=v3.5
del ..\trunk\Source\bin\Release\Moq.dll.* /Y
xcopy ..\trunk\Source\bin\Release\*.* ..\drops\Current\NET35 /Y /I

msbuild ..\trunk\Source.Silverlight\Moq.Silverlight.csproj /t:Rebuild /p:Configuration=Release
xcopy ..\trunk\Source.Silverlight\bin\Release\*.* ..\drops\Current\Silverlight4 /Y /I

msbuild ..\trunk\Source\Moq.csproj /t:Rebuild /p:Configuration=Release
del ..\trunk\Source\bin\Release\Moq.dll.* /Y
xcopy ..\trunk\Source\bin\Release\*.* ..\drops\Current\NET40 /Y /I

xcopy ..\trunk\License.txt "..\drops\Current\" /Y /I
msbuild ..\trunk\Moq.shfbproj

xcopy Help\Moq.chm ..\drops\Current\ /Y 