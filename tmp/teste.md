## **Pré-requisitos para Executar o Projeto**

Antes de executar o projeto, certifique-se de que você possui os seguintes requisitos instalados e configurados em sua máquina:

### **1. .NET SDK 8**
- Este projeto foi desenvolvido utilizando o **.NET 8**.
- Você pode verificar se o .NET SDK está instalado e qual versão está disponível utilizando o comando:
  ```bash
  dotnet --version
  ```
  - **Se não estiver instalado**: Faça o download da versão correta do .NET SDK diretamente no site oficial: [Download .NET 8](https://dotnet.microsoft.com/download).

---

### **2. IDE Recomendada (Opcional)**
Embora seja possível executar o projeto diretamente via linha de comando, o uso de uma IDE pode facilitar o desenvolvimento, depuração e navegação pelo código. As IDEs recomendadas são:

- **Visual Studio** (Windows):  
  - Certifique-se de que a **versão 2022** ou superior está instalada.
  - Instale o *workload* "Desenvolvimento ASP.NET e Web".
  
- **Visual Studio Code** (Windows, macOS, Linux):  
  - Instale a extensão **C# Dev Kit** ou **OmniSharp** para suporte ao .NET.
  
- **JetBrains Rider** (Windows, macOS, Linux):  
  - Uma IDE completa para desenvolvimento .NET, especialmente útil se você já utiliza outros produtos JetBrains.

---

### **3. Sistema Operacional Suportado**
- Este projeto pode ser executado em qualquer sistema operacional suportado pelo .NET 8, como **Windows**, **macOS**, e **Linux**.



*Após instalar os pré-requisitos acima, você estará pronto para configurar e executar o projeto. Para mais detalhes sobre como executar ou testar o projeto, consulte as seções correspondentes neste README.*
 

## **Como Executar os Testes via CLI**

Os testes podem ser executados facilmente pela linha de comando utilizando o .NET CLI. Aqui estão os passos:

1. **Navegue até o diretório raiz do projeto** (onde está localizada a solução `.sln`):
   ```bash
   cd caminho/para/o/projeto
   ```

2. **Execute os testes**:
   ```bash
   dotnet test
   ```

   Isso compilará a solução, executará todos os testes definidos nos projetos de teste, e exibirá os resultados diretamente no terminal.



## **Git Hooks Integrados**

Este projeto possui dois *git hooks* que são instalados automaticamente na primeira vez que o projeto recebe build. Aqui está como cada um deles funciona e os benefícios que trazem:

### **1. Pré-push: Build e Testes Automáticos**

- **Funcionamento**:
  Este hook é acionado antes de cada *push*. Ele:
  1. Compila o projeto utilizando o comando `dotnet build`.
  2. Executa todos os testes definidos na solução (`dotnet test`).

- **Vantagens**:
  - **Garante qualidade**: Evita enviar código quebrado ou que falhe nos testes para o repositório remoto.
  - **Economiza tempo**: Identifica problemas antes de serem enviados, reduzindo a necessidade de correções posteriores.

### **2. Pós-merge: Restore Automático**

- **Funcionamento**:
  Este hook é acionado automaticamente após um *merge*. Ele executa o comando `dotnet restore` na solução, garantindo que todas as dependências estejam atualizadas.

- **Vantagens**:
  - **Produtividade**: Automatiza o processo de restauração, garantindo que você não esqueça de atualizar suas dependências após integrar alterações.

Sim, vale muito a pena mencionar o uso do **.editorconfig** no seu README, especialmente se ele estiver configurado para padronizar o código e melhorar a colaboração em equipe. Adicionar uma seção sobre isso ajuda os desenvolvedores a entenderem como o código deve ser formatado e mostra um cuidado com a qualidade do projeto.

## **Padronização de Código com .editorconfig**

Este projeto utiliza o arquivo **.editorconfig** para garantir uma padronização de estilo e formatação de código entre os desenvolvedores, independentemente da IDE ou editor de texto utilizado.

- **O que é .editorconfig?**  
  O [**.editorconfig**](https://editorconfig.org/) é um arquivo de configuração usado para definir regras de estilo de código, como espaçamento, indentação, e fim de linha. Ele é suportado por diversas IDEs e editores, como Visual Studio, Visual Studio Code, Rider, e muitos outros.

- **Principais Regras Configuradas no Projeto**:
  - **Indentação**: Utiliza espaços ao invés de tabs, com 4 espaços por nível de indentação.
  - **Fim de linha**: Unix (`LF`) para compatibilidade multiplataforma.
  - **Encoding**: UTF-8 para todos os arquivos.
  - **Regras de C#**:
    - Espaço antes e após operadores (`=`, `+`, etc.).
    - Remoção de linhas em branco desnecessárias.
    - Uso de `var` conforme apropriado.

- **Como Ele Funciona?**  
  Ao abrir ou editar arquivos do projeto em uma IDE compatível, o **.editorconfig** aplica automaticamente as regras configuradas, garantindo que o código esteja consistente.

- **Benefícios**:
  - **Consistência**: Todo o time segue o mesmo padrão de formatação, reduzindo divergências durante revisões de código.
  - **Automatização**: Com suporte direto na IDE, as regras são aplicadas automaticamente, minimizando esforço manual.
  - **Compatibilidade**: Garante que o código seja formatado corretamente, independentemente do sistema operacional ou IDE utilizada.

- **Como Ativar o Suporte ao .editorconfig?**
  - **Visual Studio**: Suporte nativo (versões 2019 ou superiores).
  - **Visual Studio Code**: Certifique-se de que as extensões relacionadas ao C# e ao **EditorConfig** estão instaladas.
  - **Rider**: Suporte nativo ao abrir o projeto.

Se quiser modificar ou expandir as regras de estilo, basta editar o arquivo `.editorconfig` na raiz do projeto.


## **Tratamento de Erros no Padrão ProblemDetails**

O projeto utiliza o padrão **ProblemDetails** para padronizar e simplificar o retorno de erros nas APIs. 

- **O que é ProblemDetails?**  
  É uma especificação definida na [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807) que descreve como fornecer respostas HTTP de erro de forma estruturada e consistente.

- **Como funciona no projeto?**  
  Sempre que ocorre um erro (por exemplo, validações, exceções não tratadas ou problemas específicos de negócio), o sistema retorna um objeto no formato JSON com as seguintes informações:

  ```json
  {
    "type": "https://suadocumentacao.com/errors/erro-especifico",
    "title": "Erro durante a operação",
    "status": 400,
    "detail": "Descrição detalhada do erro.",
    "instance": "/api/endpoint-especifico"
  }
  ```

  - `type`: Um link para mais informações sobre o tipo de erro.
  - `title`: Um resumo do erro.
  - `status`: O código de status HTTP.
  - `detail`: Uma descrição detalhada do problema (útil para debug).
  - `instance`: A URL ou caminho do recurso que causou o erro.

- **Vantagens de usar ProblemDetails**:
  - **Consistência**: Todas os endpoints ds API retornam erros no mesmo formato.
  - **Facilidade de depuração**: Informações claras sobre o que deu errado e onde.
  - **Melhor comunicação com o cliente**: Os consumidores da API conseguem interpretar e tratar os erros de forma eficiente.

