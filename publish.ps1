
$localBuild = "./nehsanet/dist/nehsanet/browser";
$build = (get-date).ToString("yyyyMMddHHmm");
$version = (get-date).ToString("yyyy-MM-dd HH:mm");
$full_version = "<!-- ${version} -->";
$index_location = "$localBuild/index.html";

$val = read-host "Apply build number ${version}? [Y/n]";
if ($val.ToLower() -eq '' -or $val.ToLower() -eq 'y') {
    write-host "Updating version.ts";
    set-content -path "./nehsanet/src/version.ts" -value "export const version = { number: '$version' }";
}

write-host "Removing old files at $localBuild";
remove-item $localBuild/* -recurse;

write-host "Running: ng build --configuration production";
set-location ./nehsanet; ng build --configuration production; set-location ..;

write-host "Updating index.html and version.ts with version info: ${version}";
add-content -path $index_location -value $full_version;

write-host "Building docker image ${build}";
docker build . -t nehsa/nehsanet:$build --platform linux/amd64;

write-host "Pushing image to DockerHub...";
docker push nehsa/nehsanet:$build;

write-host "Ready for deployment.";