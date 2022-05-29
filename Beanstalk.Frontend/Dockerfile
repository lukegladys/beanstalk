FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Beanstalk.Frontend/Beanstalk.Frontend.csproj", "Beanstalk.Frontend/"]
RUN dotnet restore "Beanstalk.Frontend/Beanstalk.Frontend.csproj"
COPY . .
WORKDIR "/src/Beanstalk.Frontend"
RUN dotnet build "Beanstalk.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Beanstalk.Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beanstalk.Frontend.dll"]
