FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY TodoApi/*.csproj ./TodoApi/
RUN dotnet restore

# copy everything else and build app
COPY TodoApi/. ./TodoApi/
WORKDIR /app/TodoApi
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/TodoApi/out ./
ENTRYPOINT ["dotnet", "TodoApi.dll"]