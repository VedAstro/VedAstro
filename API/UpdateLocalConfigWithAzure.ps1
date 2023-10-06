az login

az functionapp config appsettings list --name VedAstroAPI --resource-group VedAstro --query "[].{name:name, value:value}" --output json > local.settings.latest.json