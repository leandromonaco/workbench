FROM mcr.microsoft.com/windows/nanoserver:1903

ADD ./bin/* /redis/
WORKDIR /redis/
EXPOSE 6379

# Define our command to be run when launching the container
CMD .\redis-server.exe .\redis.windows.conf --port 6379