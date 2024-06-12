
$build = (get-date).ToString("yyyyMMddHHmm");
$localBuild = "./dist/nehsanet/browser";
$version = (get-date).ToString("yyyy-MM-dd HH:mm");
$full_version = "<!-- ${version} -->";
$index_location = "$localBuild/index.html";

$val = read-host "Publish UI, API, Both? [UI/api/both]";
if ($val.ToLower() -eq 'both' -or $val.ToLower() -eq 'api') {
    write-host "Getting API ready...";
    write-host "Building docker API image ${build}";
    docker build . -t nehsa/nehsaapi:$build --platform linux/amd64;
    $cmd = "docker run -it -p 22007:22007 -e ASPNETCORE_URLS='https://+:22007' -e ASPNETCORE_HTTPS_PORT=22007 -e ASPNETCORE_Kestrel__Certificates__Default__Password='password' -e ASPNETCORE_Kestrel__Certificates__Default__Path='/https/api.nehsa.net.pfx'  -v C:\certs:/https/ nehsa/nehsaapi:${build}";
    write-host 'To run:';
    write-host $cmd;

    # output to file
    $cmd | out-file -filepath ./run-api.ps1;
    write-host 'To run in the future: ./run-api.ps1';
}

if ($val.ToLower() -eq '' -or $val.ToLower() -eq 'ui' -or $val.ToLower() -eq 'both') {
    set-location ./nehsanet; 

    $val = read-host "Apply build number ${version}? [Y/n]";
    if ($val.ToLower() -eq '' -or $val.ToLower() -eq 'y') {
        write-host "Updating version.ts";
        set-content -path "./src/version.ts" -value "export const version = { number: '$version' }";
    }
    
    write-host "Removing old files at $localBuild";
    remove-item $localBuild/* -recurse;
    
    write-host "Running: ng build --configuration production";
    ng build --configuration production;
    
    write-host "Updating index.html and version.ts with version info: ${version}";
    add-content -path $index_location -value $full_version;
    
    write-host "Building docker UI image ${build}";
    docker build . -t nehsa/nehsanet:$build --platform linux/amd64;
    
    write-host "Pushing image to DockerHub...";
    docker push nehsa/nehsanet:$build;
    
    write-host "UI ready for deployment.";

    set-location ..
}

write-host "Done!";