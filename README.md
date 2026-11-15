# Educa+ — Sistema Web de Monitoramento e Prevenção da Evasão Escolar

Sistema web desenvolvido em **C# com ASP.NET Core MVC**, utilizando SQL Server Express como banco de dados, com o objetivo de monitorar indicadores acadêmicos e auxiliar na identificação precoce de alunos em risco de evasão escolar.

O projeto contempla gerenciamento de alunos e turmas, registro de frequência, lançamento de notas, classificação automática de risco e acompanhamento de intervenções pedagógicas.

---

## Objetivo

O Educa+ foi criado para centralizar informações de frequência e desempenho acadêmico, facilitando o acompanhamento dos estudantes pelas equipes educacionais.

A partir dos dados registrados, o sistema identifica situações que exigem atenção, permitindo que a Coordenação e a Psicopedagogia adotem medidas preventivas antes que o aluno interrompa sua trajetória escolar.

---

## Tecnologias utilizadas

- C# e .NET 10
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server Express
- ASP.NET Core Identity
- HTML5
- CSS3
- JavaScript
- Bootstrap

---

## Motivo da escolha da infraestrutura

### C# + ASP.NET Core MVC

O ASP.NET Core MVC foi escolhido pela possibilidade de estruturar o sistema em camadas, separando interfaces, regras de negócio e acesso aos dados.

O framework oferece recursos para:

- Gerenciamento de rotas e Controllers;
- Desenvolvimento de interfaces Razor;
- Validação de formulários;
- Autenticação e controle de acesso;
- Integração com bancos de dados relacionais.

### SQL Server Express

O SQL Server Express foi utilizado por ser uma solução relacional adequada para o armazenamento dos registros acadêmicos.

Sua utilização permite relacionar alunos, turmas, disciplinas, aulas, notas e frequências, garantindo maior organização e integridade das informações.

### Entity Framework Core

O Entity Framework Core foi utilizado para realizar a comunicação entre o sistema e o SQL Server.

A abordagem **Database First** permitiu gerar os modelos C# a partir da estrutura do banco de dados existente.

### ASP.NET Core Identity

O Identity foi utilizado para implementar login, gerenciamento de usuários e permissões de acesso, garantindo que cada perfil utilize apenas as funcionalidades autorizadas.

### HTML5, CSS3 e Bootstrap

O frontend foi desenvolvido utilizando Razor Views, HTML5, CSS3 e Bootstrap, proporcionando interfaces organizadas, intuitivas e responsivas.

---

## Funcionalidades

### Coordenação

- Login administrativo;
- Cadastro e gerenciamento de turmas;
- Cadastro e gerenciamento de alunos;
- Cadastro e gerenciamento de disciplinas;
- Gerenciamento de aulas e avaliações;
- Visualização dos indicadores acadêmicos;
- Monitoramento dos riscos de evasão;
- Registro e acompanhamento de intervenções;
- Cadastro de usuários e definição de perfis.

### Professor

- Login com perfil específico;
- Consulta das aulas cadastradas;
- Registro de presença e falta dos alunos;
- Atualização de chamadas;
- Consulta das avaliações;
- Lançamento e atualização de notas.

### Psicopedagogia

- Login com perfil específico;
- Acesso ao painel de monitoramento;
- Identificação de alunos em situação de risco;
- Consulta de indicadores de frequência e desempenho;
- Registro de ocorrências;
- Cadastro de ações preventivas;
- Acompanhamento do status das intervenções.

---

## Classificação de risco de evasão

O sistema utiliza regras de negócio baseadas na frequência e na média acadêmica dos alunos.

| Classificação | Critério |
|---|---|
| Estável | Frequência ≥ 80% e média ≥ 6,0 |
| Atenção | Frequência entre 75% e menos de 80%, ou média entre 5,0 e menos de 6,0 |
| Crítico | Frequência inferior a 75% ou média inferior a 5,0 |
| Sem dados | Informações insuficientes para classificação |

O nível Crítico prevalece sobre Atenção quando o aluno apresenta indicadores conflitantes.

Essas classificações permitem identificar estudantes que podem necessitar de acompanhamento pedagógico.

---

## Banco de dados

O sistema utiliza oito tabelas acadêmicas principais:

- Turmas
- Alunos
- Disciplinas
- Aulas
- Frequencias
- Avaliacoes
- Notas
- OcorrenciasRisco

Além disso, possui tabelas específicas do ASP.NET Core Identity para autenticação e gerenciamento dos perfis de acesso.

---

## Testes e validação

Para validar o funcionamento do Educa+, foi utilizada uma base fictícia de 100 alunos, com diferentes registros de notas e frequências.

Foram realizados **15 casos de teste funcionais**, contemplando autenticação, permissões, registros acadêmicos, classificação de risco e acompanhamento pedagógico.

**Resultado:** 15 testes executados e aprovados, correspondendo a 100% de aprovação na homologação funcional.

---

## Possíveis melhorias futuras

- Integração com sistemas acadêmicos externos;
- Notificações automáticas sobre situações de risco;
- Análises históricas da evolução acadêmica;
- Ampliação dos indicadores de acompanhamento escolar;
- Publicação do sistema em ambiente de nuvem.

---

## Contexto acadêmico

O Educa+ foi desenvolvido no âmbito do **Projeto Integrador II**, com foco na aplicação de conhecimentos de Engenharia de Software, Banco de Dados e Desenvolvimento Web para apoiar ações de prevenção à evasão escolar.
