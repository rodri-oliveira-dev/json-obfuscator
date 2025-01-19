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

while ($CurrentDate -le [datetime]$EndDate) {
    # Obter o número do dia da semana (1 = Segunda, 7 = Domingo)
    $DayOfWeek = $CurrentDate.DayOfWeek

    if ($DayOfWeek -eq "Saturday" -or $DayOfWeek -eq "Sunday") {
        # Reduz atividade no fim de semana (10% de chance de trabalhar)
        if ((Get-Random -Minimum 1 -Maximum 10) -ne 1) {
            $CurrentDate = $CurrentDate.AddDays(1)
            continue
        }
    }

    # Determina o número de commits no dia (dias úteis têm mais commits)
    $RandomCommits = Get-Random -Minimum 3 -Maximum 8 # 3 a 7 commits em dias úteis

    for ($i = 1; $i -le $RandomCommits; $i++) {
        # Define um horário aleatório durante o expediente
        $Hour = Get-Random -Minimum $WorkStartHour -Maximum ($WorkEndHour + 1)
        $Minute = Get-Random -Minimum 0 -Maximum 60
        $Second = Get-Random -Minimum 0 -Maximum 60

        $IsoDate = $CurrentDate.AddHours($Hour - $CurrentDate.Hour).AddMinutes($Minute).AddSeconds($Second).ToString("yyyy-MM-ddTHH:mm:sszzz")

        # Escolhe um arquivo aleatório para modificar
        $SelectedFile = $Files | Get-Random

        # Escolhe uma mensagem de commit aleatória
        $CommitMessage = $Messages | Get-Random

        # Faz uma alteração no arquivo selecionado
        "/* Modificação: $CommitMessage em $IsoDate */" | Out-File -Append -FilePath $SelectedFile

        # Adiciona e comita as mudanças
        git add $SelectedFile
        git commit -m "$CommitMessage" --date="$IsoDate"

        # Pausa para simular trabalho (1 a 5 minutos)
        Start-Sleep -Seconds (Get-Random -Minimum 60 -Maximum 301)
    }

    # Avança para o próximo dia
    $CurrentDate = $CurrentDate.AddDays(1)
}
