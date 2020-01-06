##############################
#.SYNOPSIS
# Add or Update a Web configuration AppSetting key
#
#.DESCRIPTION
# Add or Update a Web configuration AppSetting key
#
#.PARAMETER ConfigFilePath
# Path of the configuration file
#
#.PARAMETER key
# The name of the key to add/update
#
#.PARAMETER value
# The value to set
#
#.EXAMPLE
# AddOrUpdateWebConfigAppSetting -ConfigFilePath $Path -Key $key -Value $value
#
#.NOTES
#
##############################
Function AddOrUpdateWebConfigAppSetting {
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)] [string]$ConfigFilePath
       ,[Parameter(Mandatory=$true)] [string]$key
       ,[Parameter(Mandatory=$true)] [string]$value
   )

   $configType = "web.config"
   $configurationElement = "appSettings"
   AddOrUpdateConfigFile -ConfigFilePath $ConfigFilePath -ConfigFileName $ConfigType -ConfigurationElement $configurationElement -key $key -value $value
}

##############################
#.SYNOPSIS
#Short description
#
#.DESCRIPTION
#Long description
#
#.PARAMETER ConfigFilePath
#Parameter description
#
#.PARAMETER ConfigFileName
#Parameter description
#
#.PARAMETER key
#Parameter description
#
#.PARAMETER value
#Parameter description
#
#.EXAMPLE
#An example
#
#.NOTES
#General notes
##############################
Function AddOrUpdateAppConfigAppSetting() {
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)] [string]$ConfigFilePath
       ,[Parameter(Mandatory=$true)] [string]$ConfigFileName
       ,[Parameter(Mandatory=$true)] [string]$key
       ,[Parameter(Mandatory=$true)] [string]$value
   )
   $configurationElement = "appSettings"
   AddOrUpdateConfigFile -ConfigFilePath $ConfigFilePath -ConfigFileName $ConfigFileName -ConfigurationElement $configurationElement -key $key -value $value
}

##############################
#.SYNOPSIS
#Short description
#
#.DESCRIPTION
#Long description
#
#.PARAMETER ConfigFilePath
#Parameter description
#
#.PARAMETER key
#Parameter description
#
#.PARAMETER value
#Parameter description
#
#.EXAMPLE
#An example
#
#.NOTES
#General notes
##############################
Function AddOrUpdateWebConfigConnectionString {
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)] [string]$ConfigFilePath
       ,[Parameter(Mandatory=$true)] [string]$key
       ,[Parameter(Mandatory=$true)] [string]$value
   )

   $configType = "web.config"
   $configurationElement = "connectionStrings"
   AddOrUpdateConfigFile -ConfigFilePath $ConfigFilePath -ConfigFileName $ConfigType -ConfigurationElement $configurationElement -key $key -value $value
}

##############################
#.SYNOPSIS
#Short description
#
#.DESCRIPTION
#Long description
#
#.PARAMETER ConfigFilePath
#Parameter description
#
#.PARAMETER ConfigFileName
#Parameter description
#
#.PARAMETER key
#Parameter description
#
#.PARAMETER value
#Parameter description
#
#.EXAMPLE
#An example
#
#.NOTES
#General notes
##############################
Function AddOrUpdateAppConfigConnectionString() {
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)] [string]$ConfigFilePath
       ,[Parameter(Mandatory=$true)] [string]$ConfigFileName
       ,[Parameter(Mandatory=$true)] [string]$key
       ,[Parameter(Mandatory=$true)] [string]$value
   )
   $configurationElement = "connectionStrings"
   AddOrUpdateConfigFile -ConfigFilePath $ConfigFilePath -ConfigFileName $ConfigFileName -ConfigurationElement $configurationElement -key $key -value $value
}


##############################
#.SYNOPSIS
# Internal function to update or add a configuration element inside a configuration file
#
#.DESCRIPTION
#
#
#.PARAMETER ConfigFilePath
# path of the configuration file
#
#.PARAMETER ConfigFileName
# name of the configuration file
#
#.PARAMETER ConfigurationElement
# The configuration section where to update
#
#.PARAMETER key
# Configuration key name to add/update
#
#.PARAMETER value
# Value to set
#
#.EXAMPLE
# AddOrUpdateConfigFile -ConfigFilePath $ConfigFilePath -ConfigFileName $ConfigFileName -ConfigurationElement $configurationElement -key $key -value $value
#
#.NOTES
# Used internally from all other function in the lib
##############################
Function AddOrUpdateConfigFile {
    [CmdletBinding()]

    param(
         [Parameter(Mandatory=$true)] [string]$ConfigFilePath
        ,[Parameter(Mandatory=$true)] [string]$ConfigFileName
        ,[Parameter(Mandatory=$true)] [string]$ConfigurationElement
        ,[Parameter(Mandatory=$true)] [string]$key
        ,[Parameter(Mandatory=$true)] [string]$value
    )

    $configFile = Join-Path $ConfigFilePath -ChildPath $ConfigFileName
    $doc = (Get-Content $configFile) -as [Xml]

    if($configurationElement -eq "connectionStrings"){
        $item = $doc.configuration.connectionStrings.add | Where-Object { $_.Name -eq "$key"}
        if ($item -ne $null){
            $item.connectionString = $value
        }
    }
    else{
        $item = $doc.configuration.$ConfigurationElement.add | Where-Object {$_.Key -eq "$key" }
        if ($item -ne $null){
            $item.value = "$value"
        }else {
            $newSetting = $doc.CreateElement("add")
            $doc.configuration.$ConfigurationElement.AppendChild($newSetting)
            $newSetting.SetAttribute("key","$key");
            $newSetting.SetAttribute("value","$value");
        }
    }
    $doc.Save($configFile)

}
