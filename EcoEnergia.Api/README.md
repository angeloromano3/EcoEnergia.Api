# Eco Energia API

API REST em ASP.NET Core (.NET 10) para monitoramento de consumo de energia — Desafio FIAP (tema ESG: eficiência energética).

## Tecnologias
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core + Oracle (migrations, code first)
- Docker
- xUnit

## Como rodar

### 1. Configurar o banco (cada um usa a PRÓPRIA conta Oracle FIAP)
No Visual Studio: botão direito no projeto **EcoEnergia.Api** → **Manage User Secrets** e cole, trocando pelo seu RM e senha:

```json
{
  "ConnectionStrings": {
    "OracleFIAP": "User Id=rmSEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

A senha fica fora do projeto e nunca vai para o GitHub.

### 2. Criar as tabelas
Tools → NuGet Package Manager → Package Manager Console:

```
Update-Database
```

### 3. Rodar
- Perfil **Container (Dockerfile)** (Docker Desktop aberto): API em `http://localhost:8080/api`
- Perfil **http** (sem Docker): API em `http://localhost:5109/api`

Rodando o contêiner por fora do Visual Studio, passe a conexão por variável de ambiente:

```
docker run -p 8080:8080 -e ConnectionStrings__OracleFIAP="User Id=...;Password=...;Data Source=..." ecoenergia-api
```

## Insomnia
- `base_url` = `http://localhost:8080/api`
- Em Preferences → General → Request / Response, desmarque **Validate certificates**.

## Endpoints prontos
| Método | Rota | Descrição |
|---|---|---|
| GET | /api/unidades?pagina=1&tamanho=10 | Lista paginada |
| GET | /api/unidades/{id} | Busca por id |
| POST | /api/unidades | Cria |
| PUT | /api/unidades/{id} | Atualiza |
| DELETE | /api/unidades/{id} | Remove |

## Como contribuir
Veja o [CONTRIBUTING.md](CONTRIBUTING.md).