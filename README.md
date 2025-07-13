# 🚀 Funcionalidades

- 📁 Cadastro de projetos e tarefas  
- ✅ Atualização de status e histórico de alterações  
- 💬 Comentários em tarefas  
- 📊 Relatórios de produtividade  
- 🔐 Controle por papéis (ex: gerente)  

# Endpoints Principais

1. Listagem de Projetos - listar todos os projetos do usuário  
2. Visualização de Tarefas - visualizar todas as tarefas de um projeto específico  
3. Criação de Projetos - criar um novo projeto  
4. Criação de Tarefas - adicionar uma nova tarefa a um projeto  
5. Atualização de Tarefas - atualizar o status ou detalhes de uma tarefa  
6. Remoção de Tarefas - remover uma tarefa de um projeto  

---

# 🛠️ Tecnologias

- .NET 8 (ASP.NET Core)  
- Entity Framework Core  
- SQLite  
- Docker  
- Swagger (para testes)  
- xUnit (para testes unitários)  

---

# 📦 Como rodar com Docker

1. Compile a imagem:

    docker pull renankwn/taskmanagerapi-renanaugusto

2. Execute o container:

    docker run -d -p 5000:8080 --name taskmanagerapi-renanaugusto renankwn/taskmanagerapi-renanaugusto:latest

3. Acesse no navegador:

    http://localhost:5000/swagger

---

# 🧪 Rodando os testes

Para rodar os testes unitários localmente (fora do Docker):

    dotnet test

---

# 📂 Estrutura de Pastas

    TaskManagerAPI/
    ├── Controllers/       # Controllers da API (Projetos, Tarefas, Relatórios)
    ├── Models/            # Entidades e enums
    ├── DTOs/              # Objetos de transferência de dados
    ├── Data/              # AppDbContext
    ├── Program.cs         # Inicialização da aplicação
    ├── Dockerfile         # Docker build script
    └── README.md          # Você está aqui

---

# 📝 Notas

- A API usa SQLite como banco de dados leve, armazenado no próprio container.  
- Um projeto pode ter até 20 tarefas.  
- Não é possível deletar um projeto com tarefas pendentes.  
- Prioridade de tarefas não pode ser alterada após criação.  

---

# 🔍 Fase 2: Perguntas para Refinamento

1. Um projeto pode ter múltiplos responsáveis?  
2. Haverá convites ou permissões para colaboradores externos?  
3. Os papéis são fixos (ex: gerente, colaborador) ou configuráveis?  
4. Que tipo de métricas os relatórios devem exibir (por usuário, por projeto)?  
5. Os relatórios devem ser exportáveis (PDF, Excel)?  
6. Devemos registrar todo tipo de alteração nas tarefas (título, prazo, etc.)?  
7. Comentários geram notificações? Quem pode comentar?  
8. O limite de 20 tarefas por projeto será fixo ou configurável?  
9. A prioridade será sempre imutável após criação?  
10. Há intenção de integrar com ferramentas como Trello, Slack, etc.?  
11. Haverá versão mobile ou front-end web no futuro?  

---

# 🧠 Fase 3: Melhorias e Visão de Futuro

## Arquitetura

- Aplicar Clean Architecture ou Onion Architecture  
- Separar serviços de negócio (Services) e repositórios  
- Centralizar validações com FluentValidation  

## Cloud e Escalabilidade

- Utilizar volumes no Docker ou migrar para banco como PostgreSQL  
- Configuração via .env com IConfiguration para cloud readiness  
- Criar CI/CD com GitHub Actions e deploy para Azure/AWS  
- Adicionar endpoints de health check para uso com Kubernetes  

## Funcionalidades e UX

- Implementar autenticação via JWT ou OAuth2  
- Controle de acesso baseado em claims  
- Rate limiting com AspNetCoreRateLimit  
- Registro estruturado com Serilog ou Seq  

## Testes

- Cobertura de testes com coverlet e ReportGenerator  
- Testes de integração com banco em memória ou TestContainers  
- Testes de contrato entre serviços (ex: usando Pact)

## Internacionalização

- Adicionar suporte a múltiplos idiomas com IStringLocalizer  
- Suporte a regionalização de datas e mensagens  

---

# 📫 Contato

Desenvolvido por Renan Augusto Ferreira  
📧 renanaugustokwn@outlook.com  
🔗 https://www.linkedin.com/in/renan-augusto-kwn
Git: https://github.com/RenanAugustoKwn/TaskManagerAPI-Renan
