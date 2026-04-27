# Start TaskTracker (run when you want to use the app)
param(
    [string]$ResourceGroup = "YOUR_RESOURCE_GROUP",
    [string]$ContainerApp  = "tasktracker-api",
    [string]$DbServer      = "YOUR_POSTGRES_SERVER_NAME"
)

Write-Host "Starting TaskTracker..." -ForegroundColor Cyan

Write-Host "  Starting PostgreSQL database..." -ForegroundColor Yellow
az postgres flexible-server start --name $DbServer --resource-group $ResourceGroup

Write-Host "  Scaling up Container App..." -ForegroundColor Yellow
az containerapp update --name $ContainerApp --resource-group $ResourceGroup --min-replicas 1

$url = az containerapp show --name $ContainerApp --resource-group $ResourceGroup `
    --query properties.configuration.ingress.fqdn -o tsv

Write-Host ""
Write-Host "TaskTracker is starting up!" -ForegroundColor Green
Write-Host "API URL: https://$url" -ForegroundColor Green
Write-Host "(Wait ~60 seconds for the database to fully start)" -ForegroundColor Gray
