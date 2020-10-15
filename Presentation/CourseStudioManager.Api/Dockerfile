FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
# Copy everything and build
COPY . ./
RUN dotnet restore
RUN dotnet publish ./Presentation/CourseStudioManager.Api/CourseStudioManager.Api.csproj -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/Presentation/CourseStudioManager.Api/out .
ENTRYPOINT ["dotnet", "CourseStudioManager.Api.dll"]