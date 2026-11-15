# Educa+ — Sistema Web de Monitoramento e Prevenção da Evasão Escolar

Sistema web desenvolvido em **C# com ASP.NET Core MVC**, utilizando **SQL Server Express** como banco de dados, com o objetivo de apoiar instituições de ensino na identificação de estudantes com sinais de risco de evasão escolar.

O projeto foi desenvolvido no âmbito do **Projeto Integrador II**, considerando o contexto da educação profissional e técnica. A aplicação reúne cadastros acadêmicos, controle de frequência, lançamento de notas, análise de risco e registro de intervenções pedagógicas.

---

## Objetivo

O Educa+ foi criado para centralizar informações acadêmicas que ajudam a identificar precocemente situações de desengajamento escolar.

A partir dos registros de presença e desempenho, o sistema calcula indicadores e classifica os alunos em níveis de risco, oferecendo à Coordenação e à Psicopedagogia uma visão consolidada dos casos que exigem acompanhamento.

A proposta é **apoiar a tomada de decisão**, e não substituir a avaliação realizada pelos profissionais da instituição.

---

## Tecnologias utilizadas

- C# e .NET 10
- ASP.NET Core MVC
- Razor Views, HTML5, CSS3 e JavaScript
- Bootstrap
- SQL Server Express
- Entity Framework Core (Database First)
- ASP.NET Core Identity
- Visual Studio

---

## Motivo da escolha da infraestrutura

### C# + ASP.NET Core MVC

O ASP.NET Core MVC foi escolhido para estruturar a aplicação em camadas, separando as responsabilidades de apresentação, controle das requisições e acesso às informações.

Essa arquitetura facilita a manutenção do código e a organização das funcionalidades, como cadastros, lançamentos acadêmicos e monitoramento dos alunos.

### SQL Server Express

O SQL Server Express foi utilizado para armazenar os dados de turmas, alunos, disciplinas, aulas, avaliações, frequências, notas e ocorrências de risco.

Por ser um banco relacional, permite estabelecer relacionamentos entre os registros e manter a integridade das informações acadêmicas durante os testes do sistema.

### Entity Framework Core

O Entity Framework Core realiza a comunicação entre a aplicação e o banco de dados. No projeto, foi utilizada a abordagem **Database First**, gerando modelos C# a partir da estrutura previamente criada no SQL Server.

### ASP.NET Core Identity

O Identity foi utilizado para implementar autenticação, gerenciamento de usuários e controle de acesso por perfil. O cadastro de novas contas é restrito à Coordenação, e o registro público foi desabilitado.

### HTML5, CSS3, Razor e Bootstrap

Essas tecnologias compõem as interfaces do Educa+, incluindo formulários, tabelas, menus e indicadores. O Bootstrap contribui para a organização visual e a adaptação das telas a diferentes tamanhos de janela.

---

## Perfis de acesso

O sistema possui três perfis:

- **Coordenação:** gerencia turmas, alunos, disciplinas, usuários, aulas, avaliações, monitoramento e ocorrências.
- **Professor:** acessa aulas, registra frequências, consulta avaliações e lança notas.
- **Psicopedagogia:** acessa o monitoramento de risco e registra ou acompanha ocorrências e intervenções.

As permissões são verificadas pela aplicação, inclusive quando alguém tenta acessar uma rota diretamente pela URL.

---

## Funcionalidades

### Início

- Apresentação dos indicadores disponíveis para o perfil autenticado.
- Atalhos para os módulos autorizados.
- Visão resumida dos registros acadêmicos e das ocorrências em acompanhamento.

### Turmas, alunos e disciplinas

- Cadastro, consulta, edição e exclusão de registros.
- Associação de alunos a turmas.
- Associação de disciplinas às respectivas turmas.

### Aulas e frequência

- Cadastro de aulas por disciplina e data.
- Acesso à chamada diretamente pela listagem de aulas.
- Marcação de presença ou falta dos alunos.
- Atualização de chamadas já registradas.
- Cálculo do percentual de frequência com base nos registros existentes.

### Avaliações e notas

- Cadastro de avaliações vinculadas às disciplinas.
- Lançamento de notas por aluno e avaliação.
- Atualização das notas registradas.
- Cálculo da média acadêmica a partir dos lançamentos disponíveis.

### Monitoramento de evasão

- Consulta dos alunos ativos.
- Exibição de frequência, média e classificação de risco.
- Indicadores resumidos de desempenho e risco.
- Pesquisa e filtros para facilitar o acompanhamento.
- Atalho para abertura de ocorrência associada ao aluno.

### Ocorrências e intervenções

- Registro de ocorrências de risco associadas a alunos.
- Descrição do motivo do acompanhamento.
- Registro das ações realizadas e da data de intervenção.
- Controle de status, como **Aberta**, **Em andamento** e **Concluída**.

### Usuários

- Cadastro de contas de Professor e Psicopedagogia pela Coordenação.
- Associação de perfil a cada usuário.
- Login e logout.
- Restrição de acesso conforme as permissões definidas.

---

## Regras de classificação de risco

O Educa+ utiliza regras de negócio baseadas na frequência e na média das notas registradas:

| Classificação | Critério |
|---|---|
| **Estável** | Frequência igual ou superior a 80% **e** média igual ou superior a 6,0. |
| **Atenção** | Frequência de 75% a menos de 80% **ou** média de 5,0 a menos de 6,0, desde que não exista critério crítico. |
| **Crítico** | Frequência inferior a 75% **ou** média inferior a 5,0. |
| **Sem dados** | Ausência de registros suficientes de frequência ou notas para avaliar os dois indicadores. |

A classificação **Crítico** tem prioridade sobre **Atenção**. Os resultados são indicadores de apoio ao acompanhamento pedagógico, não diagnósticos definitivos de evasão.

---

## Estrutura do banco de dados

O modelo acadêmico utiliza oito tabelas principais:

- `Turmas`
- `Alunos`
- `Disciplinas`
- `Aulas`
- `Frequencias`
- `Avaliacoes`
- `Notas`
- `OcorrenciasRisco`

O **ASP.NET Core Identity** utiliza tabelas adicionais de autenticação e perfis, como `AspNetUsers` e `AspNetRoles`.

---

## Como executar localmente

**Pré-requisitos:** Visual Studio ou SDK do .NET 10, SQL Server Express e acesso ao banco `DB_EducaPlus`.

1. Clone o repositório e abra o projeto `EducaPlus` no Visual Studio.
2. Restaure as dependências com `dotnet restore`.
3. Crie ou restaure o banco **`DB_EducaPlus`**, com as tabelas acadêmicas e seus relacionamentos. **As migrações do Identity não criam as oito tabelas acadêmicas**: elas precisam existir previamente, de acordo com o modelo Database First.
4. Configure a conexão `ConnectionStrings:EducaPlus` para sua instância local do SQL Server Express. O ambiente de desenvolvimento original utiliza autenticação integrada do Windows e a instância `.\SQLEXPRESS`.
5. Se necessário, aplique a migração de autenticação com `dotnet ef database update --context EducaPlusIdentityContext`.
6. Para criar a primeira conta de Coordenação em um banco novo, configure temporariamente `BootstrapAdmin:Email` e `BootstrapAdmin:Password` com `dotnet user-secrets set` e execute a aplicação. Depois de confirmar o login, remova esses dois segredos de inicialização.
7. Inicie o projeto pelo Visual Studio ou com `dotnet run` e acesse a URL local indicada no terminal.

> **Importante:** não publique senhas, arquivos de *User Secrets*, backups com dados pessoais ou strings de conexão contendo credenciais no repositório.

A carga de 100 alunos fictícios foi utilizada para homologação e pode ser executada separadamente, após a criação do esquema acadêmico.

---

## Testes e validação

O Educa+ passou por **15 casos de teste funcionais manuais**, abrangendo:

- Autenticação e permissões de acesso — CT01 a CT04.
- Cadastro e frequência escolar — CT05 a CT07.
- Lançamento e validação de notas — CT08 a CT10.
- Classificação automática de risco — CT11 a CT13.
- Monitoramento e intervenções — CT14 e CT15.

**Resultado da homologação:** 15 testes executados e 15 aprovados, utilizando uma base de testes com 100 alunos fictícios.

---

## Escopo e possíveis evoluções

O projeto foi validado como **protótipo funcional em ambiente local de desenvolvimento**, com dados simulados. Não há confirmação de publicação em nuvem ou integração com sistemas acadêmicos externos.

Como possíveis evoluções, destacam-se a integração com diários acadêmicos oficiais, notificações automáticas e recursos adicionais de análise temporal do desempenho escolar. Uma API REST pública ou específica para integrações também pode ser desenvolvida futuramente, mas **não integra o escopo funcional validado nesta versão**.

---

## Contexto acadêmico

Projeto desenvolvido para o **Projeto Integrador II**, com foco no uso da tecnologia como apoio à identificação e à prevenção da evasão escolar na educação profissional e técnica.
