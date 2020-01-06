# When we have a service account, this can be used.
#$username=""
#$password="" | ConvertTo-SecureString -AsPlainText -Force
#$credentials = New-Object System.Management.Automation.PsCredential($username, $password)
$credentials = ""

$envs=@("c592xfglumwb1.ecomqc.tlrg.com","c460zvplumwb2.ecomqc.tlrg.com","411yzjlumwb3.ecomqc.tlrg.com","vswebmipor07.manatron.com")
"Please select the environment(s) to update web.config"
"1. Lumen DEV1"
"2. Lumen DEV2"
"3. Lumen QA"
"4. Riverside DEV"
$envIndex = Read-Host -Prompt "Enter option: "

$server=$envs[$envIndex-1]
$keys=@( "Common.ResourceLocator.Uri","Common.ResourceLocator.Partition","Common.ResourceLocator.ExternalUri")
$values=@("http://localhost/common.resourcelocator/","DEV","http://$server/common.resourcelocator/")
$drive="p"
New-PSDrive -Name $drive -PSProvider "FileSystem" -Root "\\$server\D$" -Credential $credentials
$webConfig = "${drive}:\InetPub\wwwroot\CA-Riverside-QA-SCL\web.config"
$doc = (Get-Content $webConfig) -as [Xml]

for($i = 0; $i -lt $keys.Count; $i++) {
    
    $key = $keys[$i]
    $value = $values[$i]

    $item = $doc.configuration.appSettings.add | Where-Object {$_.Key -eq $key }
    if ($item -ne $null) {

        Write-Output "At index $i Located $key"        

        if ($item.value -ne $value) {
            
            "Not correct value "+ $item.value + ", value should be $value"
            $item.value = $value
        }
        else {
            "Correct value $value found"
        }

    } else {
        "Cannot locate " + $key + " - Creating appsettings"
        $newAppSetting = $doc.CreateElement("add")
        $doc.configuration.appSettings.AppendChild($newAppSetting)
        $newAppSetting.SetAttribute("key",$key);
        $newAppSetting.SetAttribute("value",$value);
    }
}

$doc.Save("\\$server\D$\InetPub\wwwroot\CA-Riverside-QA-SCL\web.config")

Remove-PSDrive $drive