FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000 7288

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MealMate.PL/MealMate.PL.csproj", "MealMate.PL/"]
RUN dotnet restore "MealMate.PL/MealMate.PL.csproj"
COPY . .
WORKDIR "/src/MealMate.PL"
RUN dotnet build "MealMate.PL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MealMate.PL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MealMate.PL.dll"]
