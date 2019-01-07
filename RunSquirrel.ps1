$nupkgs = @(gci .\GrandadAudioPlayer\ -Recurse -Filter GrandadAudioPlayer*.nupkg | ?{ !$_.PSIsContainer } | % { $_.FullName })

if ($nupkgs.length -ne 1) { Write-Warning "More than one nupkg found using first one: $npkgs[0]"}

.\packages\squirrel.windows.1.9.0\tools\Squirrel.exe --no-msi --no-delta --releasify $nupkgs[0]

