  powershell -f build.ps1 -Configuration Debug -CodeDirectory "src" -TestsDirectory "test" -PackageOutputDirectory "Build" -Package "dotnet-test-mspec" -Version "1-local-version"
