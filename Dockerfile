FROM ubuntu:latest

USER root

COPY .nuke/temp ~/.nuget/packages

RUN ./build.cmd UnitTest IntegrationTest

ENV TZ=America/Sao_Paulo
