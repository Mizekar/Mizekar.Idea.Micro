FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 63091
EXPOSE 44399

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Mizekar.Idea.Micro/Mizekar.Idea.Micro.csproj Mizekar.Idea.Micro/
RUN dotnet restore Mizekar.Idea.Micro/Mizekar.Idea.Micro.csproj
COPY . .
WORKDIR /src/Mizekar.Idea.Micro
RUN dotnet build Mizekar.Idea.Micro.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Mizekar.Idea.Micro.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mizekar.Idea.Micro.dll"]
