FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

COPY *.sln .

# Копируем csproj каждый в свою папку
COPY */*.csproj .
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore --runtime linux-x64 --force --force-evaluate -v q --property WarningLevel=0 /clp:ErrorsOnly

COPY . .
RUN dotnet publish --runtime linux-x64 --self-contained --configuration Release --no-restore -v q --property WarningLevel=0 /clp:ErrorsOnly

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run
# curl нужен для хелсчеков
RUN apt-get -y update &&\
    apt-get install -y curl
WORKDIR /app
COPY --from=build ./build/Web/bin/Release/net8\.0/linux-x64/publish/ .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "Web.dll"]