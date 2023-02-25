FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as builder
WORKDIR /app
COPY /src .
RUN dotnet restore
RUN dotnet publish --no-restore -c release -o /out
COPY entrypoint.sh /out/entrypoint.sh

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine as runtime
COPY --from=builder /out /app
RUN chmod +x /app/entrypoint.sh
