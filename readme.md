# Teste para desenvolvedor - Ilegra

O sistema desenvolvido com fins de teste para o processo seletivo tem o objetivo analisar dados de venda que irá importar lotes de arquivos e produzir um relatório baseado em informações presentes no mesmo.

## Iniciando

Essas instruções fornecerão uma cópia do projeto em execução na sua máquina local para fins de desenvolvimento e teste. Consulte implantação para obter notas sobre como implantar o projeto em um sistema ativo.

### Pré-requisitos

- Visual Studio 2019 instalado ou Visual Studio Code
- .NET Core 3.1

### Instalação

Para rodar o projeto siga os passos a baixo

1. Clone o repositório através do comando 

```
git clone https://github.com/LucasDiogo96/Ilegra_Analyzer.git
```

2. Acesse o diretório clonado no passo anterior

```
cd Ilegra_Analyzer
```

3. Execute os comandos

```
dotnet build
```
```
dotnet run --project Analyzer.API/Analyzer.API.csproj
```

4. Acesse a URL da demo através do seu browser

```
 https://localhost:5001/hangfire
```
## Notas

 * Para fins de monitoramento dos denominados jobs da aplicação, foi utilizado o Hangfire onde é fornecida uma interface para acompanhamento e histórico dos processos ocorridos de leitura dos arquivos.

 * Com a finalidade de se ter um controle ainda maior sobre a aplicação, o Serilog está sendo utilizado para registrar qualquer erro que ocorra no sistema e salvando-os em uma base NoSQL MongoDB.

  * Para futuras integrações, foi desenvolvido um endpoint assíncrono preparada para receber arquivos e salva-los para recorrente leitura do monitor.
  
  * Ambas as bases NoSQL estão rodando em um cluster em nuvem no MongoDB Atlas


## Bibliotecas de desenvolvimento

* [.Net Code 3.1](https://dotnet.microsoft.com/download) - Framework utilizado para construção da aplicação.
* [MongoDB](https://www.mongodb.com/) - Utilzado como base de logs e de processos para o Hangfire.
* [Hangfire](https://www.hangfire.io/) - Usado para monitorar os processos da aplicação.
* [Serilog](https://serilog.net/) - Utilizado como framework para registro de logs.


## Autor

* **Lucas Diogo da Silva** - [Github](https://github.com/LucasDiogo96)

