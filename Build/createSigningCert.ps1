# https://gallery.technet.microsoft.com/scriptcenter/Self-signed-certificate-5920a7c6#content
Import-Module .\tools\New-SelfSignedCertificateEx.ps1 
New-SelfSignedCertificateEx -Subject "CN=Test Code Signing" -StoreLocation "CurrentUser" -KeyUsage DigitalSignature -NotAfter (Get-Date).AddMonths(13) -EKU "Code Signing"

# Check to see if new cert exist
Set-Location Cert:\CurrentUser\My
Get-ChildItem | Format-Table Subject, FriendlyName, Thumbprint -AutoSize