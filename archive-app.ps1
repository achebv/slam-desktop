
$scriptDirectory = $PSScriptRoot


# Check if the correct number of arguments is provided
if ($args.Length -ne 1) {
    Write-Host "Usage: .\archive-app.ps1 <environment>"
    exit 1
}

# Get the target database name from the command-line argument
$environment = $args[0]



$deliverPath = "C:\Users\achebv\Dropbox\planner-app"

$sourceFolder = Join-Path $scriptDirectory "AmazonDeliveryPlanner\bin\Debug"

$archiveFileName = if ($environment -eq "prod") {
    "planner.zip"
} else {
    "planner-$environment.zip"
}

$archiveFolder = Join-Path $deliverPath $archiveFileName

$configFileSource = Join-Path $scriptDirectory "AmazonDeliveryPlanner\\ConfigurationTemplate\conf.${environment}.json"
$configFileDestination = Join-Path $sourceFolder "conf.json"

$installFileSrc = Join-Path $scriptDirectory "install-planning.ps1"
$installFileDest = Join-Path $deliverPath "install-planning.ps1"

# Check if the source folder exists
if (Test-Path $sourceFolder -PathType Container) {
    # Create the archive folder if it doesn't exist
    $null = New-Item -Path (Split-Path $archiveFolder) -ItemType Directory -Force
	
    # Copy the config-prod.json file to the project folder
    Copy-Item -Path $configFileSource -Destination $configFileDestination -Force

    # Copy install file
    Copy-Item -Path $installFileSrc -Destination $installFileDest -Force

    # Archive the source folder
    Compress-Archive -Path $sourceFolder -DestinationPath $archiveFolder -Force

    Write-Host "Folder '$sourceFolder' has been archived to '$archiveFolder'."
} else {
    Write-Host "Source folder '$sourceFolder' does not exist."
}

