FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS base 
WORKDIR /app 
COPY *.csproj ./ 
RUN dotnet restore 
COPY . . 
RUN dotnet publish -c Release -o out 

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime 
WORKDIR /app
COPY --from=base /app/out ./
ENV ASPNETCORE_URLS=http://*:22007 
EXPOSE 22007/tcp 

RUN apk add --no-cache python3 py3-pip

# Install required packages
# Update package lists
COPY dontcheckin.py ./
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt --break-system-packages --no-cache-dir

# copy sql database
COPY game_data.db ./

# Check the installed version of Python
RUN python3 --version

COPY --from=base /app/out ./

# For Comet AI
COPY talk.py /app

# Start the application
ENTRYPOINT ["dotnet", "nehsanet-app.dll"]  
