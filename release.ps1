dotnet publish Flow.Launcher.Plugin.NewFile -c Release -r win-x64
Compress-Archive -LiteralPath Flow.Launcher.Plugin.NewFile/bin/Release/win-x64/publish -DestinationPath Flow.Launcher.Plugin.NewFile/bin/NewFile.zip -Force