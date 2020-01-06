param($AppName, $JobName, $ResourceGroup)
az webapp webjob triggered run --name $AppName --resource-group $ResourceGroup --webjob-name $JobName