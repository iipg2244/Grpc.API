FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG USERNAME=non-root-user
ARG USER_UID=1000
ARG USER_GID=$USER_UID

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

# ********************************************************
# * Anything else you want to do like clean up goes here *
# ********************************************************

# [Optional] Set the default user. Omit if you want to keep the default as root.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR "/build"
COPY ["./server/Client.API/Client.API.csproj", "./server/NuGet.Config", "./server/global.json", "./Client.API/"]
RUN dotnet restore "./Client.API/Client.API.csproj" --configfile "./Client.API/NuGet.Config" --packages .nuget

COPY ["./server/Client.API/", "./Client.API/"]
RUN dotnet publish "./Client.API/Client.API.csproj" -c "Release" -o "/app" --no-restore

WORKDIR "/"
RUN rm -r "build"

FROM base
COPY ["./docker-compose/certificates/grpc-api.crt", "/usr/local/share/ca-certificates/"]
RUN chmod 644 /usr/local/share/ca-certificates/grpc-api.crt && update-ca-certificates --fresh
USER $USERNAME
WORKDIR "/app"
EXPOSE 8002
EXPOSE 44302
COPY --from=build "/app" "."

ENTRYPOINT ["dotnet", "Client.API.dll"]
