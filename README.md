# Thomas Greg
Teste prático de desenvolvimento

Demonstração de cliente MVC suportado por API RestFul com armazenamento em banco de dados relacional Microsoft MS-SQL.

## Instalação

Para executar é necessário ambiente com Docker Engine versão 24.0.2 or superior em execução.
Para iniciar basta executar docker compose a partir de um prompt PowerShell utilizando o seguinte comando na pasta raiz do projeto: 

```bash
docker compose up -d
```

## Utilização

No mesmo ambiente, inicie seu navegador internet e digite o seguinte endereço:

```bash
http://localhost:4200/
```

## Lista de pendências

▪ Criação de stored procedures para ações de inclusão, alteração e exclusão. \
▪ Revisão e padronização de parametrização em todas as aplicações. \
▪ Implementação de logs em todas as aplicações. \
▪ Refação de código e aplicação de design patterns onde necessários. \
▪ Planejamento de exposição de portas de containers docker. Apenas o cliente MVC é a API devem estar expostos. \
▪ Testes unitários em todas as aplicações. \
▪ Criação de documentação.

## Execução em ambiente de desenvolvimento

Em um esquipamento com Visual Studio 2022 instalado e Docker Engine versão 24.0.2 or superior em execução: \
▪ Clone este repositório. \
▪ Abra a solução thomasgregcorewebapi.sln. \
▪ Abra a solução thomasgregmvc.sln. \
▪ Para executar o SQL Server, a partir de uma janela CMD ou PowerShell execute:

```bash
docker pull dietermarno/thomasgregmssql:data
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=R353t3282@" -p 1433:1433 -d dietermarno/thomasgregmssql:data
```

▪ Execute a aplicação thomasgregcorewebapi utilizando o perfil "Docker". \
▪ Execute a aplicação thomasgregmvc utilizando o perfil "Docker". \
▪ Duas janelas de navegação serão abertas automaticamente, exibindo o mapa de funções da API e o cliente MVC de gerenciamento de registros.
