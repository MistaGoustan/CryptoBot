#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TCK.Bot/TCK.Bot.csproj", "TCK.Bot/"]
COPY ["TCK.Bot.Api/TCK.Bot.Api.csproj", "TCK.Bot.Api/"]
COPY ["TCK.Bot.DynamicService/TCK.Bot.DynamicService.csproj", "TCK.Bot.DynamicService/"]
COPY ["TCK.Bot.SignalService/TCK.Bot.SignalService.csproj", "TCK.Bot.SignalService/"]
COPY ["TCK.Common.DependencyInjection/TCK.Common.DependencyInjection.csproj", "TCK.Common.DependencyInjection/"]
COPY ["TCK.Common.WebJobs/TCK.Common.WebJobs.csproj", "TCK.Common.WebJobs/"]
COPY ["TCK.Bot.Data/TCK.Bot.Data.csproj", "TCK.Bot.Data/"]
COPY ["TCK.Exchanges.Binance/TCK.Exchanges.Binance.csproj", "TCK.Exchanges.Binance/"]
COPY ["TCK.Exchanges.Bybit/TCK.Exchanges.Bybit.csproj", "TCK.Exchanges.Bybit/"]
RUN dotnet restore "TCK.Bot.Api/TCK.Bot.Api.csproj"
COPY . .
WORKDIR "/src/TCK.Bot.Api"
RUN dotnet build "TCK.Bot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TCK.Bot.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "TCK.Bot.Api.dll"]