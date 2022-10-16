FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/PhotoStock/FreeCourse.Services.PhotoStock/*.csproj Services/PhotoStock/FreeCourse.Services.PhotoStock/
RUN dotnet restore Services/PhotoStock/FreeCourse.Services.PhotoStock/*
COPY . .
RUN dotnet publish ervices/PhotoStock/FreeCourse.Services.PhotoStock/* -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "FreeCourse.Services.PhotoStock.dll" ]