FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

ARG CONFIGURATION="Release"

ENV CONFIGURATION=$CONFIGURATION

RUN curl -o /usr/local/share/ca-certificates/verisign.crt -SsL https://crt.sh/?d=1039083 && update-ca-certificates

FROM base as restore

WORKDIR /sln

COPY src/Usadosbr.Contas.Core/Usadosbr.Contas.Core.csproj Usadosbr.Contas.Core/
COPY src/Usadosbr.Contas.Core/ Usadosbr.Contas.Core/

COPY src/Usadosbr.Contas.WebApi/Usadosbr.Contas.WebApi.csproj Usadosbr.Contas.WebApi/
COPY src/Usadosbr.Contas.WebApi/ Usadosbr.Contas.WebApi/

COPY src/Usadosbr.Contas.Migrations/Usadosbr.Contas.Migrations.csproj Usadosbr.Contas.Migrations/
COPY src/Usadosbr.Contas.Migrations/ Usadosbr.Contas.Migrations/

RUN dotnet restore Usadosbr.Contas.Migrations/

FROM restore as build

WORKDIR Usadosbr.Contas.Migrations

RUN dotnet build Usadosbr.Contas.Migrations.csproj -c ${CONFIGURATION}
RUN dotnet publish Usadosbr.Contas.Migrations.csproj --configuration ${CONFIGURATION} --no-build -o /app/tools

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS final

COPY --from=build /app/tools /app/tools

WORKDIR /app/tools

ENTRYPOINT ["dotnet", "Usadosbr.Contas.Migrations.dll"]
