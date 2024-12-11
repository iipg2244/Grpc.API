FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG USERNAME=non-root-user
ARG USER_UID=1000
ARG USER_GID=$USER_UID

# Add the certificates
COPY ["./docker-compose/certificates/client-api.crt", "/usr/local/share/ca-certificates/"]
RUN chmod 644 /usr/local/share/ca-certificates/client-api.crt && update-ca-certificates --fresh

# Create the user
RUN groupadd --gid "$USER_GID" "$USERNAME" \
    && useradd --uid "$USER_UID" --gid "$USER_GID" -m "$USERNAME" \
    #
    # [Optional] Add sudo support. Omit if you don't need to install software after connecting.
    && apt-get update \
    && apt-get install --no-install-recommends -y sudo \
    && apt-get install --no-install-recommends -y curl \
    && echo "$USERNAME" ALL=\(root\) NOPASSWD:ALL > "/etc/sudoers.d/$USERNAME" \
    && chmod 0440 "/etc/sudoers.d/$USERNAME" \
    && apt-get clean

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR "/build"
COPY ["./server/Grpc.API/Grpc.API.csproj", "./server/NuGet.Config", "./server/global.json", "./Grpc.API/"]
RUN dotnet restore "./Grpc.API/Grpc.API.csproj" --configfile "./Grpc.API/NuGet.Config" --packages .nuget

COPY ["./server/Grpc.API/", "./Grpc.API/"]
RUN dotnet publish "./Grpc.API/Grpc.API.csproj" -c "Release" -o "/app" --no-restore

WORKDIR "/"
RUN rm -r "build"

FROM base
USER $USERNAME
WORKDIR "/app"
EXPOSE 8002
EXPOSE 44302
COPY --from=build "/app" "."

ENTRYPOINT ["dotnet", "Grpc.API.dll"]
