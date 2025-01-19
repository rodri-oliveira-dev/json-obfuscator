# Configuração inicial
$StartDate = "2024-12-01"
$EndDate = (Get-Date).AddYears(1).ToString("yyyy-MM-dd")
$CurrentDate = [datetime]$StartDate

# Lista de arquivos que podem ser modificados
$Files = @("README.md", "setup-hooks.bat", "setup-hooks.sh", "test.runsettings", "requirements.txt")
# Mensagens realistas de commits
$Messages = @("Corrigindo bug no módulo X",
             "Atualizando README",
             "Refatorando função Y",
             "Adicionando testes unitários",
             "Aprimorando performance",
             "Atualizando dependências",
             "Adicionando funcionalidade Z",
             "Removendo código desnecessário")

# Simula horários de trabalho (9h às 18h)
$WorkStartHour = 9
$WorkEndHour = 18

# Garantir que o próprio script não seja rastreado
if (!(Test-Path ".gitignore")) {
    Set-Content -Path ".gitignore" -Value "git_commit_simulation.ps1"
    git add .gitignore
    git commit -m "Adicionando git_commit_simulation.ps1 ao .gitignore"
}

while ($CurrentDate -le [datetime]$EndDate) {
    $DayOfWeek = $CurrentDate.DayOfWeek

    if ($DayOfWeek -eq "Saturday" -or $DayOfWeek -eq "Sunday") {
        if ((Get-Random -Minimum 1 -Maximum 10) -ne 1) {
            $CurrentDate = $CurrentDate.AddDays(1)
            continue
        }
    }

    $RandomCommits = Get-Random -Minimum 3 -Maximum 8

    for ($i = 1; $i -le $RandomCommits; $i++) {
        $Hour = Get-Random -Minimum $WorkStartHour -Maximum ($WorkEndHour + 1)
        $Minute = Get-Random -Minimum 0 -Maximum 60
        $Second = Get-Random -Minimum 0 -Maximum 60

        $IsoDate = $CurrentDate.AddHours($Hour - $CurrentDate.Hour).AddMinutes($Minute).AddSeconds($Second).ToString("yyyy-MM-ddTHH:mm:sszzz")

        $SelectedFile = $Files | Get-Random
        $CommitMessage = $Messages | Get-Random

        Write-Host "Modificando arquivo: $SelectedFile em $IsoDate"
        Add-Content -Path $SelectedFile -Value "/* Modificação única: $CommitMessage em $IsoDate - $(Get-Random) */"

        Write-Host "Adicionando arquivo ao Git..."
        git add $SelectedFile

        Write-Host "Criando commit..."
        git commit -m "$CommitMessage" --date="$IsoDate"

        Write-Host "Pausando por alguns segundos..."
        Start-Sleep -Seconds (Get-Random -Minimum 1 -Maximum 5)
    }

    Write-Host "Avançando para o próximo dia..."
    $CurrentDate = $CurrentDate.AddDays(1)
}
