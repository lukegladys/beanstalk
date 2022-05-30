FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Beanstalk.Backend/Beanstalk.Backend.csproj", "Beanstalk.Backend/"]
RUN dotnet restore "Beanstalk.Backend/Beanstalk.Backend.csproj"
COPY . .
WORKDIR "/src/Beanstalk.Backend"
RUN dotnet build "Beanstalk.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Beanstalk.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beanstalk.Backend.dll"]
