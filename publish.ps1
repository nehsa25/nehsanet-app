
$build = (get-date).ToString("yyyyMMddHHmm");

write-host "Getting API ready...";
write-host "Building docker API image ${build}";
docker build . -t nehsa/nehsaapi:$build --platform linux/amd64;
$addCert = "-e ASPNETCORE_Kestrel__Certificates__Default__Password='password' -e ASPNETCORE_Kestrel__Certificates__Default__Path='/https/api.nehsa.net.pfx'  -v C:\certs:/https/";
$cmd = "docker run -it -p 22007:22007 -e ASPNETCORE_URLS='https://+:22007' -e ASPNETCORE_HTTPS_PORT=22007 $addCert -v C:\src\mud_images:/app/wwwroot/ nehsa/nehsaapi:${build}";
 
write-host 'To run:';
write-host $cmd;

# output to file
$cmd | out-file -filepath ./run-api.ps1;
write-host 'To run in the future: ./run-api.ps1';

write-host "Done!";