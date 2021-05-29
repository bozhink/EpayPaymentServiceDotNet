#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Web/EpayPaymentServiceDotNet.Web.PaymentsServiceApi/EpayPaymentServiceDotNet.Web.PaymentsServiceApi.csproj", "Web/EpayPaymentServiceDotNet.Web.PaymentsServiceApi/"]
RUN dotnet restore "Web/EpayPaymentServiceDotNet.Web.PaymentsServiceApi/EpayPaymentServiceDotNet.Web.PaymentsServiceApi.csproj"
COPY . .
WORKDIR "/src/Web/EpayPaymentServiceDotNet.Web.PaymentsServiceApi"
RUN dotnet build "EpayPaymentServiceDotNet.Web.PaymentsServiceApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EpayPaymentServiceDotNet.Web.PaymentsServiceApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EpayPaymentServiceDotNet.Web.PaymentsServiceApi.dll"]