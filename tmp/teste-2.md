# Testes Automatizados no Projeto

Este projeto é construído com foco em uma estratégia eficiente de testes, priorizando a execução de **testes isolados e de integração controlada**. Ao eliminar dependências externas reais, como bancos de dados e APIs, os testes garantem um ambiente previsível, acelerando o desenvolvimento e identificando problemas com maior precisão. Essa abordagem reduz a complexidade e os custos associados à configuração e manutenção de ambientes externos durante os ciclos de teste.

Além disso, a estratégia assegura que as **injeções de dependência** e os **fluxos de dados** dentro do sistema estejam devidamente configurados e funcionando como esperado. Esse controle meticuloso permite detectar falhas em um estágio inicial do desenvolvimento, fornecendo uma base sólida para expandir para testes mais abrangentes e robustos quando necessário, sem comprometer a confiabilidade ou a escalabilidade do sistema.

---

## **Pirâmide de Testes**

A **Pirâmide de Testes** é uma metáfora que descreve como estruturar diferentes tipos de testes em um sistema, equilibrando esforço, custo e cobertura. Ela é composta por três níveis principais:

1. **Testes Unitários**: 
   - Base da pirâmide, com maior número de testes.
   - Focados em pequenas partes do código, como funções ou métodos.
   - Rápidos, isolados e baratos de executar.

2. **Testes de Componentes**:  
   - Focados em validar o comportamento de um conjunto de classes ou módulos que trabalham juntos.  
   - Testam interações internas sem dependências externas reais, utilizando mocks ou stubs quando necessário.  
   - Garantem que os componentes funcionem corretamente em conjunto, isolados do ambiente externo.  

3. **Testes de Integração**:
   - Meio da pirâmide, menor em número que os unitários.
   - Validam a interação entre diferentes módulos ou componentes.
   - Testam como dependências externas (como bancos de dados e APIs) são integradas.

4. **Testes de Interface/End-to-End**:
   - Topo da pirâmide, com menor quantidade de testes.
   - Focados em testar o sistema como um todo, simulando interações reais de usuários.

Neste projeto, as ferramentas e configurações foram implementadas para cobrir até a etapa de **Integração**.

---

## **Ferramentas Utilizadas**

Abaixo estão as tecnologias utilizadas neste projeto, com a explicação de seu papel na pirâmide de testes:

### **1. XUnit**
   - **Descrição**: Framework de testes unitários para .NET.
   - **Uso na Pirâmide**: Testes Unitários.
   - **Função**: Facilita a criação, execução e organização de testes automatizados.
   - **Documentação**: https://xunit.net

### **2. NSubstitute**
   - **Descrição**: Biblioteca para criar mocks e stubs de objetos.
   - **Uso na Pirâmide**: Testes de Componentes.
   - **Função**: Ajuda a simular comportamentos de dependências externas, permitindo o isolamento de componentes.
    **Documentação**: https://nsubstitute.github.io/

### **3. WireMock.Net**
   - **Descrição**: Simula servidores HTTP para substituir dependências reais, como APIs externas.
   - **Uso na Pirâmide**: Testes de Integração.
   - **Função**: Cria mocks de APIs REST para validar a interação de serviços com essas dependências.
   - **Documentação**: https://wiremock.org/docs/solutions/dotnet/

### **4. EF In-Memory**
   - **Descrição**: Provedor de banco de dados in-memory para o Entity Framework.
   - **Uso na Pirâmide**: Testes de Integração.
   - **Função**: Permite testar interações com o banco de dados sem a necessidade de configurar um banco físico.
   - **Documentação**: https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli

### **5. Web Application Factory**
   - **Descrição**: Utilitário para criar instâncias de aplicações web em memória para testes.
   - **Uso na Pirâmide**: Testes de Integração.
   - **Função**: Facilita o carregamento completo de um microserviço, testando a configuração e injeção de dependências.
   - **Documentação**: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0

---

## **Detalhes da Integração**

A camada de integração foi projetada para validar aspectos fundamentais da aplicação, como **configurações de dependência**, **serialização de entrada/saida**, **midlewares** e **interações externas**. Abaixo estão os detalhes das ferramentas utilizadas para essa etapa:

### **1. WireMock.Net**
   - **Uso**: Simula serviços REST externos.
   - **Finalidade**: Testar chamadas HTTP para APIs externas de forma controlada e previsível.
   - **Exemplo**: Configuramos rotas simuladas para garantir que o microserviço consuma dados corretamente de APIs de terceiros.

### **2. EF In-Memory**
   - **Uso**: Substitui o banco de dados real por um em memória.
   - **Finalidade**: Validar a lógica de persistência e consulta de dados sem a necessidade de um banco físico.
   - **Exemplo**: Testamos consultas e operações CRUD para verificar se os repositórios e serviços funcionam como esperado.

### **3. Web Application Factory**
   - **Uso**: Inicializa o microserviço completo em memória.
   - **Finalidade**: Validar configurações, injeções de dependências e a inicialização da aplicação.
   - **Exemplo**: Utilizado para simular um ambiente real, permitindo a execução de testes de ponta a ponta para APIs locais.

---

## **Execução dos Testes**

1. **Configuração Inicial**:
   - Certifique-se de que as dependências do projeto estão instaladas:
     ```bash
     dotnet restore
     ```

2. **Executando os Testes**:
   - Para executar os testes, use:
     ```bash
     dotnet test
     ```

3. **Personalização**:
   - As configurações para o ambiente de teste podem ser ajustadas em `appsettings.Test.json`.

---
