$build = (get-date).ToString("yyyyMMddHHmm");
$acr_loc = "nehsanet-api:$build";
write-host "Building docker API image, bulid:  ${build}";
docker build . -t $acr_loc --platform linux/amd64;

write-host "Pushing API image to Amazon ACR: $acr_loc";
docker push $acr_loc;
