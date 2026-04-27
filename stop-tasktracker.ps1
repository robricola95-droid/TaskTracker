# Stop TaskTracker (run when you're done to save money)
param(
    [string]$ResourceGroup = "YOUR_RESOURCE_GROUP",
    [string]$ContainerApp  = "tasktracker-api",
    [string]$DbServer      = "YOUR_POSTGRES_SERVER_NAME"
)

Write-Host "Stopping TaskTracker..." -ForegroundColor Cyan

Write-Host "  Scaling Container App to zero..." -ForegroundColor Yellow
az containerapp update --name $ContainerApp --resource-group $ResourceGroup --min-replicas 0

Write-Host "  Stopping PostgreSQL database..." -ForegroundColor Yellow
az postgres flexible-server stop --name $DbServer --resource-group $ResourceGroup

Write-Host ""
Write-Host "TaskTracker stopped. Azure billing paused." -ForegroundColor Green
Write-Host "Run .\start-tasktracker.ps1 when you want it back." -ForegroundColor Gray
