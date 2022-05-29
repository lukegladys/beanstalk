FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Beanstalk.App/Beanstalk.App.csproj", "Beanstalk.App/"]
RUN dotnet restore "Beanstalk.App/Beanstalk.App.csproj"
COPY . .
WORKDIR "/src/Beanstalk.App"
RUN dotnet build "Beanstalk.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Beanstalk.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beanstalk.App.dll"]
