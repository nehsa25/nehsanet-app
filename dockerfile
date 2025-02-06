# Use the official .NET Core SDK as a parent image
<<<<<<< HEAD
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS base 
ARG TARGETARCH
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY *.csproj ./
RUN dotnet restore -a $TARGETARCH

# Copy the rest of the application code
COPY . .

# Publish the application
RUN dotnet publish -c Release -o out -a $TARGETARCH
=======
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS base 
WORKDIR /app 
COPY *.csproj ./ 
RUN dotnet restore 
COPY . . 
RUN dotnet publish -c Release -o out 
>>>>>>> efa91381c05b159c88b5743dcb8f1e96b040ceb3

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime 
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

# Check the installed version of Python
RUN python3 --version

COPY --from=base /app/out ./

# For Comet AI
COPY talk.py /app

# Start the application
ENTRYPOINT ["dotnet", "nehsanet-app.dll"]  
