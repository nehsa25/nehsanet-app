<<<<<<< HEAD

$build = (get-date).ToString("yyyyMMddHHmm");

write-host "Getting API ready...";
write-host "Building docker API image ${build}";
docker build . -t nehsa/nehsaapi:$build --platform linux/amd64;
$addCert = "-e ASPNETCORE_Kestrel__Certificates__Default__Password='password' -e ASPNETCORE_Kestrel__Certificates__Default__Path='/https/api.nehsa.net.pfx'  -v ~/certs:/https/";
$cmd = "docker run -it -p 22007:22007 -e ASPNETCORE_URLS='https://+:22007' -e ASPNETCORE_HTTPS_PORT=22007 $addCert -v ~/mud_images:/app/wwwroot/ nehsa/nehsaapi:${build}";
 
write-host 'To run:';
write-host $cmd;

# output to file
$cmd | out-file -filepath ./run-api.ps1;
write-host 'To run in the future: ./run-api.ps1';

=======

$build = (get-date).ToString("yyyyMMddHHmm");
$acr_loc = "nehsanet-api:$build";
write-host "Getting API ready...";
write-host "Building docker API image, bulid:  ${build}";
docker build . -t $acr_loc --platform linux/amd64;
$addCert = "-e ASPNETCORE_Kestrel__Certificates__Default__Password='password' -e ASPNETCORE_Kestrel__Certificates__Default__Path='/https/api.nehsa.net.pfx'  -v ~/certs:/https/";

write-host "Pushing API image to Amazon ACR: $acr_loc";
docker push $acr_loc;

$cmd = "docker run -it -p 22007:22007 -e ASPNETCORE_URLS='https://+:22007' -e ASPNETCORE_HTTPS_PORT=22007 $addCert -v ~/mud_images:/app/wwwroot/ nehsa/nehsaapi:${build}";
write-host "To run locally: $cmd";

# output to file
$cmd | out-file -filepath ./run-api.ps1;
write-host 'To run in the future: ./run-api.ps1';

>>>>>>> 01be12233c2514241a90e3ebce42145c4e66829f
write-host "Done!";