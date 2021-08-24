FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

RUN curl -o /usr/local/share/ca-certificates/verisign.crt -SsL https://crt.sh/?d=1039083 && update-ca-certificates

ARG CONFIGURATION="Debug"
ARG SOLUTION_NAME="Usadosbr.Contas.sln"

ENV CONFIGURATION=$CONFIGURATION
ENV SOLUTION_NAME=$SOLUTION_NAME

FROM base AS restore 

WORKDIR /sln

COPY ./*.sln ./

COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*} && mv $file src/${file%.*}; done

RUN dotnet restore src/Usadosbr.Contas.WebApi

COPY . . 

FROM restore AS build

RUN dotnet build /sln/src/Usadosbr.Contas.WebApi -c ${CONFIGURATION}
RUN dotnet publish /sln/src/Usadosbr.Contas.WebApi --configuration ${CONFIGURATION} --no-build -o /app/www

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

COPY --from=build /app/www /app/www
WORKDIR /app/www
ENTRYPOINT ["dotnet", "Usadosbr.Contas.WebApi.dll"]
