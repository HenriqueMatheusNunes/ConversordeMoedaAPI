🔀 Conversor de Moedas em C# (.NET)

Aplicação de terminal (Console Application) desenvolvida em C# para realizar conversões de moedas utilizando as cotações da API pública AwesomeAPI.

O projeto foi desenvolvido para praticar requisições HTTP, manipulação de dados JSON, utilização de dicionários e tratamento de limitações de APIs externas.

🛠️ Tecnologias Utilizadas
Linguagem: C# (.NET)
Comunicação HTTP: System.Net.Http.HttpClient
Desserialização JSON: System.Text.Json (utilizando Dictionary<string, Informacoes>)
Interface: Console Application (Terminal)

📌 Funcionalidades
Consulta de Cotações: Obtém as cotações atualizadas das moedas diretamente da AwesomeAPI.
Conversão entre Moedas: Realiza conversões utilizando as cotações retornadas pela API.
Tratamento de Limitações da API: Quando a conversão parte do Real (BRL), a aplicação inverte a consulta (ex: USD-BRL) e utiliza o cálculo inverso para realizar a conversão.
Menu Interativo: Permite realizar várias conversões sem precisar reiniciar o programa, utilizando um laço de repetição (while).
Validação de Entradas: Utiliza decimal.TryParse para validar os valores informados pelo usuário.
Prevenção de Conversões Inválidas: Impede conversões entre moedas iguais e utiliza .ToUpper().Trim() para padronizar as entradas.

📂 Estrutura do Projeto
text
ConversordeMoedaAPI/
├── Program.cs          # Fluxo principal, menu, validações e conversões
└── Informacoes.cs      # Modelo para mapear a resposta JSON da API

🚀 Como executar

Pré-requisito: ter o .NET SDK instalado.

bash
# Clonar o repositório
git clone https://github.com/HenriqueMatheusNunes/ConversordeMoedaAPI

# Entrar na pasta do projeto
cd ConversordeMoedaAPI

# Executar a aplicação
dotnet run
