Function AddOrUpdateWebConfig {
    [CmdletBinding()]
    
    param([string]$key,[string]$value)
    
    $webConfig = "..\..\MSIPackaging\BuildTools\Build\Dist\webroot\web.config"
    $doc = (Get-Content $webConfig) -as [Xml]

    $item = $doc.configuration.appSettings.add | Where-Object {$_.Key -eq "$key" }
    if ($item -ne $null){
        $item.value = "$value"
    }else {
        $newAppSetting = $doc.CreateElement("add")
        $doc.configuration.appSettings.AppendChild($newAppSetting)
        $newAppSetting.SetAttribute("key","$key");
        $newAppSetting.SetAttribute("value","$value");
    }

    $doc.Save($webConfig)
}