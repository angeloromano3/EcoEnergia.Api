# Como contribuir – Eco Energia API

## Regras da main
- Ninguém sobe código direto na main. Tudo entra por Pull Request (PR).
- Você mesmo faz o merge do seu PR (não precisa de aprovação).
- O merge só libera se o check do GitHub Actions (build) ficar VERDE.
- Sua branch precisa estar atualizada com a main antes do merge.

## Passo a passo toda vez que for codar
1. Atualize a main: `git checkout main` e depois `git pull`
2. Crie sua branch: `git checkout -b feature/nome-da-tarefa`
3. Faça seu código e rode o projeto (F5, perfil **http**).
4. Salve: `git add .` e `git commit -m "o que você fez"`
5. Atualize com a main: `git pull origin main` (resolva conflitos se aparecerem)
6. Suba: `git push -u origin feature/nome-da-tarefa`
7. No GitHub, abra o Pull Request da sua branch para a main.
8. Espere o check:
   - ✅ Verde: clique em **Merge pull request**.
   - ❌ Vermelho: clique em **Details**, copie o erro, corrija e faça novo commit e push na MESMA branch. O check roda de novo sozinho.
9. Depois do merge, mova seu card no Trello para **Concluído**.

## Banco de dados
- Cada um usa a PRÓPRIA conta Oracle da FIAP no `appsettings.Development.json`.
- Esse arquivo NÃO vai para o GitHub. Nunca suba senha.
- Veio migration nova da main? Rode `Update-Database`.
- Mexeu em algum Model? Rode `Add-Migration NomeDaMudanca` e suba a migration junto no PR.

## Se travar
1. Leia o erro inteiro.
2. Cole na sua IA: o erro completo + o arquivo + o que você estava tentando fazer.
3. Ainda travado? Avise no grupo com print.
