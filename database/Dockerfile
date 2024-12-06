# Put common setup steps in an initial stage
FROM mcr.microsoft.com/mssql/server:2019-latest AS setup
ENV MSSQL_PID=Developer
ENV SA_PASSWORD=Passw@rd
ENV ACCEPT_EULA=Y

# Have a stage specifically to populate the data directory
FROM setup AS data
USER root
# Install Unzip
RUN apt-get update \
    && apt-get install unzip -y

# Install SQLPackage for Linux and make it executable
COPY ["database/sqlpackage-linux-x64-en-16.1.8089.0.zip", "/"]
RUN mkdir /opt/sqlpackage
RUN unzip /sqlpackage-linux-x64-en-16.1.8089.0.zip -d /opt/sqlpackage
RUN chmod 777 -R /opt/sqlpackage
RUN rm /sqlpackage-linux-x64-en-16.1.8089.0.zip

USER mssql
COPY ["database/data/db.bacpac", "/tmp/"]

ARG DBNAME=PerformanceTestDb
# Launch SQL Server, confirm startup is complete, deploy the DACPAC, then terminate SQL Server.
# See https://stackoverflow.com/a/51589787/488695
RUN ( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started" \
    && /opt/sqlpackage/sqlpackage /a:Import /tsn:. /tdn:${DBNAME} /tu:sa /tp:$SA_PASSWORD /tec:false /sf:/tmp/db.bacpac \
    && pkill sqlservr

# Final stage that actually will actually be run.
FROM setup
# Copy the prepopulated data tree, but not the backup file
COPY --from=data /var/opt/mssql /var/opt/mssql
# Use the default USER, CMD, etc. from the base SQL Server image