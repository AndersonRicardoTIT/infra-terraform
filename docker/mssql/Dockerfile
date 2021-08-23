FROM mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04

USER root

RUN mkdir -p /var/opt/mssql/data
RUN mkdir -p /var/opt/mssql/log
RUN mkdir -p /var/opt/mssql/secrets

RUN chgrp -R 0 /var/opt/mssql
RUN chmod -R g=u /var/opt/mssql

RUN chown -R mssql:0 /var/opt/mssql

USER mssql

ENTRYPOINT ./opt/mssql/bin/sqlservr
