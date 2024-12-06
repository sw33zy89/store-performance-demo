# Put common setup steps in an initial stage
FROM mcr.microsoft.com/mssql/server:2019-latest AS setup
ENV MSSQL_PID=Developer
ENV SA_PASSWORD=Passw@rd
ENV ACCEPT_EULA=Y
ENV DB_NAME=PerformanceTestDb

CMD /opt/mssql/bin/sqlservr & \
    sleep 30s && \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -Q "CREATE DATABASE $DB_NAME;" && \
    tail -f /dev/null

EXPOSE 1433