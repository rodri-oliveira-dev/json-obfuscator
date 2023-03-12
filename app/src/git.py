import git

try:
    # Abre o repositório em um diretório local já existente
    repo = git.Repo('/caminho/para/diretorio/local')

    if repo.is_dirty():
        return

    repo.git.checkout('master')
    # Verifica se há atualizações no branch remoto antes de fazer o pull
    remote = repo.remotes.origin
    if commits_behind := list(repo.iter_commits('HEAD..origin/master')):
        print(f'Existem {len(commits_behind)} commits atrasados')
        repo.git.pull(line_callback=lambda line: print(
            line.split()[0]) if line.startswith(('Receiving', 'Resolving')) else None)

except git.GitCommandError:
    print('Ocorreu um erro ao buscar atualizações do repositório remoto')
