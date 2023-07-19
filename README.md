# Desafio Thomas Greg
Teste prático Thomas Greg

Demonstração de cliente MVC suportado por API RestFul com armazenamento em banco de dados relacional Microsoft MS-SQL.

## Installation

Para executar é necessário ambiente com Docker Engine version 24.0.2 or higher em execução.
Para iniciar basta executar docker compose a partir de um prompt PowerShell utilizando o seguinte comando na pasta raiz do projeto: 

```bash
docker compose up -d
```

## Utilização

No mesmo ambiente, inicie seu navegador internet e digite o seguinte endereço:

```bash
http://localhost:4200
```

## Lista de pendências

Contemplar ação de exclusão de endereços ao excluir um cliente.\
Ações e views para gerenciamento da lista de endereços a partir das telas de inclusão e alteração.\
Criação de stored procedures para ações de inclusão, alteração e exclusão.\ 
Verificar motivo da falha de execução da API em container através de docker compose.\
Revisão e padronização de parametrização em todas as aplicações.\
Impolementação de logs em todas as aplicações.\
Refação de código e aplicação de design patterns onde necessários.\
Planejamento de exposição de portas de containers docker. Apenas o cliente MVC é a API devem estar expostos.\
Testes unitários em todas as aplicações.\
Criação de documentação.
