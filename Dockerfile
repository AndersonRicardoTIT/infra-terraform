FROM gradle:jre11

USER root

RUN mkdir /api && apt-get update && apt-get install vim telnet wget -y

WORKDIR /API
ENV TZ=America/Sao_Paulo
