FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Azure.CognitiveService.Client.BlazorApp/Azure.CognitiveService.Client.BlazorApp.csproj", "src/Azure.CognitiveService.Client.BlazorApp/"]
RUN dotnet restore "src/Azure.CognitiveService.Client.BlazorApp/Azure.CognitiveService.Client.BlazorApp.csproj"
COPY . .
WORKDIR "/src/src/Azure.CognitiveService.Client.BlazorApp"
RUN dotnet build "Azure.CognitiveService.Client.BlazorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Azure.CognitiveService.Client.BlazorApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Azure.CognitiveService.Client.BlazorApp.dll"]
