Param()

# import class and function
$ScriptPath = $PSScriptRoot
$DlibDotNetRoot = Split-Path $ScriptPath -Parent
$NugetPath = Join-Path $DlibDotNetRoot "nuget" | `
             Join-Path -ChildPath "BuildUtils.ps1"
import-module $NugetPath -function *

$OperatingSystem="win"

# Store current directory
$Current = Get-Location
$DlibDotNetRoot = (Split-Path (Get-Location) -Parent)
$DlibDotNetSourceRoot = Join-Path $DlibDotNetRoot src

$BuildSourceHash = [Config]::GetBinaryLibraryWindowsHash()

$IntelMKLDir = $env:MKL_WIN
if ([string]::IsNullOrEmpty($IntelMKLDir))
{
   Write-Host "Environmental Value 'MKL_WIN' is not defined." -ForegroundColor Yellow
}

if ($IntelMKLDir -And !(Test-Path $IntelMKLDir))
{
   Write-Host "Environmental Value 'MKL_WIN' does not exist." -ForegroundColor Yellow
}

$BuildTargets = @()
$BuildTargets += New-Object PSObject -Property @{Target = "cpu";  Architecture = 64; RID = "$OperatingSystem-x64";   CUDA = 0   }

foreach ($BuildTarget in $BuildTargets)
{
   $target = $BuildTarget.Target
   $architecture = $BuildTarget.Architecture
   $rid = $BuildTarget.RID
   $cudaVersion = $BuildTarget.CUDA
   $option = ""

   $Config = [Config]::new("Release", $target, $architecture, "uwp", $option)
   $libraryDir = Join-Path "artifacts" $Config.GetArtifactDirectoryName()
   $build = $Config.GetBuildDirectoryName($OperatingSystem)

   foreach ($key in $BuildSourceHash.keys)
   {
      $srcDir = Join-Path $DlibDotNetSourceRoot $key

      # Move to build target directory
      Set-Location -Path $srcDir

      $arc = $Config.GetArchitectureName()
      Write-Host "Build $key [$arc] for $target" -ForegroundColor Green
      Build -Config $Config

      if ($lastexitcode -ne 0)
      {
         Set-Location -Path $Current
         exit -1
      }
   }

   # Copy output binary
   foreach ($key in $BuildSourceHash.keys)
   {
      $srcDir = Join-Path $DlibDotNetSourceRoot $key
      $dll = $BuildSourceHash[$key]
      $dstDir = Join-Path $Current $libraryDir

      CopyToArtifact -configuration "Release" -srcDir $srcDir -build $build -libraryName $dll -dstDir $dstDir -rid $rid
   }
}

# Move to Root directory 
Set-Location -Path $Current