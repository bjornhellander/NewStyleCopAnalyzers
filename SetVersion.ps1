param([String]$version) 

function Update-Nuspec-File {
    param ($FileName)

    $content = [System.IO.File]::ReadAllText($FileName) -replace '(?<=<Version>).*(?=</Version>)', $version
    $encoding = New-Object System.Text.UTF8Encoding $True
    [System.IO.File]::WriteAllText($FileName, $content ,$encoding)
}

Update-Nuspec-File -FileName "StyleCop.Analyzers\\StyleCop.Analyzers.CodeFixes\\StyleCop.Analyzers.nuspec"

git commit -m "Set version $version" .
git tag -a v$version -m "Version $version"
