
# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=base /app/out ./

# Update package lists
RUN apt update && \
    apt upgrade -y

# Install required packages
RUN apt install -y python3.11
RUN apt install -y python3-pip
COPY dontcheckin.py ./
COPY requirements.txt ./

# # Create a cache volume
VOLUME /root/.cache/pip

RUN pip install -r requirements.txt --break-system-packages --no-cache-dir

# Check the installed version of Python
RUN python3.11 --version

COPY talk.py /app

EXPOSE 22007/tcp

# Start the application
# "dotnet nehsanet-app.dll --server.urls http://*/22007
ENTRYPOINT ["dotnet", "nehsanet-app.dll","--server.urls","http://*/22007"]