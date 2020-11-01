dotnet restore
dotnet lambda package --configuration release --framework netcoreapp3.1 --output-package build/deploy-package.zip

npm run deploy
