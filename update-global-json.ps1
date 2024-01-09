'{ "sdk": { "version": "' + $args[0] +'", "rollForward": "latestMinor" } }' | Out-File -FilePath global.json -Encoding utf8

dotnet workload install maui macos android ios maccatalyst

maui-check --fix --non-interactive

exit 0