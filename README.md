FreelancerManager - Sistema de Gestão de Freelancer e Emissão de Recibos
https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet
https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp
https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite
https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black

Um sistema Full-Stack desenvolvido para profissionais autônomos (designers, desenvolvedores, etc.) gerenciarem seus clientes, contratos e finanças de forma centralizada, incluindo a emissão automatizada de recibos.

Sobre o Projeto
Este projeto foi construído como a peça central do meu portfólio de desenvolvimento Back-end. O principal objetivo foi aplicar na prática padrões de arquitetura de software utilizados em ambientes corporativos, como Clean Architecture, Domain-Driven Design (DDD) e testes unitários.

O grande diferencial técnico do projeto é o desacoplamento das regras de negócio. O cálculo de impostos, por exemplo, não fica engessado no domínio da aplicação; ele utiliza o Padrão Strategy, permitindo que diferentes regras de dedução (ou até mesmo nenhuma) sejam aplicadas de forma flexível.

Stack de Tecnologias
Back-end:

C# e .NET 10

ASP.NET Core Web API

Entity Framework Core e SQLite (foco em portabilidade zero-config)

Clean Architecture e DDD

xUnit, Moq e FluentAssertions (Testes Unitários)

Front-end:

HTML5 e CSS3

JavaScript Vanilla (Fetch API para consumo dos endpoints e manipulação de DOM)

Arquitetura
A solução está dividida em quatro camadas isoladas:

Domain: O coração do sistema. Contém as entidades (Client, Contract, Receipt), Enums e as Exceções de Domínio. Não possui dependências externas.

Application: Onde ficam os casos de uso, DTOs, Interfaces dos Repositórios e Serviços.

Infrastructure: Implementação do EF Core, mapeamentos Fluent API, configuração do DbContext e os repositórios de dados.

API: Controllers, configuração de Injeção de Dependência, configuração do SQLite e Middlewares globais.

Principais Funcionalidades
Cadastro e listagem de Clientes e Contratos.

Emissão de recibos com cálculo automático do valor líquido (Bruto menos Deduções).

Cálculo de impostos flexível (inserido via porcentagem no ato da emissão do recibo).

Integridade referencial: regras que impedem a exclusão de clientes que possuem contratos ativos vinculados.

Tratamento global de exceções para padronizar os retornos da API (HTTP 400, 409, 500).

Como rodar o projeto
O projeto foi desenhado para ser facilmente testado por qualquer pessoa. O banco de dados SQLite é criado e atualizado automaticamente na primeira execução, sem a necessidade de instalar servidores de banco de dados.

Pré-requisitos:

.NET 10 SDK

Um navegador web moderno

Passo a passo:

1 - Clone o repositório:
git clone https://github.com/wesleyroffline/FreelancerManager.git
cd FreelancerManager
2 - Rode a API (as migrações do banco de dados serão aplicadas automaticamente):
dotnet run --project FreelancerManager.API
3 - A API estará rodando em http://localhost:5148. Você pode acessar a documentação interativa (Swagger) em: http://localhost:5148/swagger.
4 - Para abrir o Front-end, navegue até a pasta Frontend-Web e abra o arquivo index.html abrir com - google chrome(ou navegador de sua preferencia).

Autor
Wesley R Offline
