function xmlPoke($file, $xpath, $value){
    $filePath = $file.FullName

    [xml] $fileXml = Get-Content $filePath
    $node = $fileXml.SelectSingleNode($xpath)
    if ($node) { 
        $node.InnerText = $value
        $fileXml.Save($filePath)
    }
}

if ($env:APPVEYOR_BUILD_VERSION){
    xmlPoke (Get-ChildItem ".\postmark.nuspec") "/package/metadata/version" $env:APPVEYOR_BUILD_VERSION
}