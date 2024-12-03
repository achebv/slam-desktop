
$scriptDirectory = $PSScriptRoot

# Installation Steps

# Step 1: Create a folder "work" in C:\
$workFolderPath = "C:\work"

if (-not (Test-Path $workFolderPath -PathType Container)) {
    New-Item -Path $workFolderPath -ItemType Directory
    Write-Host "Step 1: Created 'work' folder in C:\"
} else {
    Write-Host "Step 1: 'work' folder in C:\ already exists."
}

# Step 2: Unzip "planner.zip" to C:\work\planner
$plannerZipPath = Join-Path $scriptDirectory "planner.zip"

$plannerExtractPath = Join-Path $workFolderPath "planner"

if (Test-Path $plannerZipPath -PathType Leaf) {
    Expand-Archive -Path $plannerZipPath -DestinationPath $plannerExtractPath -Force
    Write-Host "Step 2: Unzipped 'planner.zip' to C:\work\planner"
} else {
    Write-Host "Step 2: 'planner.zip' not found in C:\"
}

# Step 3: Create a desktop shortcut
$shortcutPath = [System.IO.Path]::Combine([System.Environment]::GetFolderPath("Desktop"), "AmazonDeliveryPlanner.lnk")
$exePath = Join-Path $plannerExtractPath "Debug\AmazonDeliveryPlanner.exe"
if (Test-Path $exePath -PathType Leaf) {
    $WScriptShell = New-Object -ComObject WScript.Shell
    $Shortcut = $WScriptShell.CreateShortcut($shortcutPath)
    $Shortcut.TargetPath = $exePath
    $Shortcut.Save()
    Write-Host "Step 3: Created desktop shortcut to 'AmazonDeliveryPlanner.exe'"
} else {
    Write-Host "Step 3: 'AmazonDeliveryPlanner.exe' not found in '$plannerExtractPath\Debug'"
}
