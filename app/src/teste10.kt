# Navega até a pasta do repositório Git
cd /caminho/para/o/repositório/git/

# Obter o nome da branch atual
branch_atual=$(git rev-parse --abbrev-ref HEAD)

# Define uma variável para rastrear se o build já foi executado
build_executado=false

# Verifica se há alterações na pasta app
if git status app | grep -qE 'new file|modified|deleted'; then
    echo "Alterações encontradas na pasta app na branch $branch_atual. Executando 'gradlew clean build'."
    ./gradlew clean build
    build_executado=true
else
    echo "Nenhuma alteração encontrada na pasta app na branch $branch_atual."
fi

# Verifica se há commits pendentes de push na branch atual na pasta app, somente se o build não tiver sido executado anteriormente
if ! $build_executado && git fetch && [ -n "$(git diff --name-only --diff-filter=d origin/$branch_atual HEAD -- app)" ]; then
    echo "Commits pendentes encontrados na pasta app na branch $branch_atual. Executando 'gradlew clean build'."
    ./gradlew clean build
else
    echo "Nenhum commit pendente encontrado na pasta app na branch $branch_atual."
fi
