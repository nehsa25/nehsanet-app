Invoke-nehsaBuildAndPublishAPI;

# FOR AWS LightSail
# $build = (get-date).ToString("yyyyMMddHHmm");
# $acr_loc = "nehsanet-api:$build";
# write-host "Building docker API image, bulid:  ${build}";
# docker build . -t $acr_loc --platform linux/amd64;

# write-host "Pushing API image to Amazon ACR: $acr_loc";
# docker push $acr_loc;

# LOCAL
# $build = (get-date).ToString("yyyyMMddHHmm");

# write-host "Getting API ready...";
# write-host "Building docker API image ${build}";
# docker build . -t nehsa/nehsaapi:$build --platform linux/amd64;
# $addCert = "-e ASPNETCORE_Kestrel__Certificates__Default__Password='password' -e ASPNETCORE_Kestrel__Certificates__Default__Path='/https/api.nehsa.net.pfx'  -v d:/data/certs:/https/";
# $cmd = "docker run -it -p 22007:22007 -e ASPNETCORE_URLS='https://+:22007' -e ASPNETCORE_HTTPS_PORT=22007 $addCert -v d:/data/mud_images:/app/wwwroot/ nehsa/nehsaapi:${build}";
 
# write-host 'To run:';
# write-host $cmd;

# # output to file
# $cmd | out-file -filepath ./run-api.ps1;
# write-host 'To run in the future: ./run-api.ps1';

# write-host "Done!";