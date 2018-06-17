FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 63091
EXPOSE 44399

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Mizekar.Micro.Idea/Mizekar.Micro.Idea.csproj Mizekar.Micro.Idea/
RUN dotnet restore Mizekar.Micro.Idea/Mizekar.Micro.Idea.csproj
COPY . .
WORKDIR /src/Mizekar.Micro.Idea
RUN dotnet build Mizekar.Micro.Idea.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Mizekar.Micro.Idea.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mizekar.Micro.Idea.dll"]
