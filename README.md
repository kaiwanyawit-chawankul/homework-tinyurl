# url-shortener

[x] create api
[x] fix url for redirect - prefix for shortener
[] fix reference api from vite
[] create db
[] connect with analytic
[] create frontend
[] deploy


api design

host/create
host/id
host/login
host/urls

create(url,SPECIAL_TAG)

https://learn.microsoft.com/en-us/dotnet/aspire/caching/caching-integrations?tabs=dotnet-cli


curl -X POST https://localhost:7073/create -H "Content-Type: application/json" -d '{"url": "your-url-here"}'


https://localhost:7073/scalar/#tag/homework-tinyurlwebapi

Fix cors
https://stackoverflow.com/a/79214366


https://github.com/dotnet/aspire-samples/blob/main/samples/AspireWithJavaScript/AspireJavaScript.Vite/default.conf.template


install aspirate
https://github.com/prom3theu5/aspirational-manifests


use aspirate
inside homework-tinyurl.AppHost
aspirate generate --output-format compose
aspirate build

homework-tinyurl.AppHost/aspirate-output/docker-compose.yaml
update missing config
image: "reactvite:latest"

inside homework-tinyurl.AppHost/aspirate-output/
docker-compose up
