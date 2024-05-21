
$build = (get-date).ToString("yyyyMMddHHmm");
$localBuild = "./dist/nehsanet/browser";
$version = (get-date).ToString("yyyy-MM-dd HH:mm");
$full_version = "<!-- ${version} -->";
$index_location = "$localBuild/index.html";

$val = read-host "Publish UI, API, Both? [UI/api/both]";
if ($val.ToLower() -eq '' -or $val.ToLower() -eq 'ui') {
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

if ($val.ToLower() -eq 'both' -or $val.ToLower() -eq 'api') {
    write-host "Getting API ready...";
    write-host "Building docker API image ${build}";
    docker build . -t nehsa/nehsaapi:$build --platform linux/amd64;
}

write-host "Done!";