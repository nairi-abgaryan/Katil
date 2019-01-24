FROM microsoft/dotnet:2.1-sdk AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Katil.WebAPI/Katil.WebAPI.csproj Katil.WebAPI/
#COPY Katil.WebAPI.Tests/Katil.WebAPI.Tests.csproj Katil.WebAPI.Tests/
COPY Katil.Test.Moqito/Katil.Test.Moqito.csproj Katil.Test.Moqito/
COPY Katil.Business.Services.Tests/Katil.Business.Services.Tests.csproj Katil.Business.Services.Tests/
COPY Katil.Business.Entities/Katil.Business.Entities.csproj Katil.Business.Entities/
COPY Katil.Common.Utilities/Katil.Common.Utilities.csproj Katil.Common.Utilities/
COPY Katil.Data.Model/Katil.Data.Model.csproj Katil.Data.Model/
COPY Katil.UserResolverService/Katil.UserResolverService.csproj Katil.UserResolverService/
COPY Katil.Scheduler.Task/Katil.Scheduler.Task.csproj Katil.Scheduler.Task/
COPY Katil.Business.Services/Katil.Business.Services.csproj Katil.Business.Services/
COPY Katil.Business.Services.IntegrationEvents/Katil.Business.Services.IntegrationEvents.csproj Katil.Business.Services.IntegrationEvents/
COPY Katil.Data.Repositories/Katil.Data.Repositories.csproj Katil.Data.Repositories/
COPY Katil.Messages/Katil.Messages.csproj Katil.Messages/

WORKDIR /src/Katil.WebAPI
RUN dotnet restore -nowarn:msb3202,nu1503

WORKDIR /src
COPY . .

WORKDIR /src/Katil.WebAPI
RUN dotnet build -c Release -o /app 

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Katil.WebAPI.dll"]
