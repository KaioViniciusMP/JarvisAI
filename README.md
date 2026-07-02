# 🤖 JarvisAI — Assistente Inteligente para Siri

JarvisAI é um assistente inteligente que utiliza a Siri apenas como interface de voz, enquanto toda a inteligência fica em uma API desenvolvida em ASP.NET Core 9. Ele compreende linguagem natural, executa ações, utiliza ferramentas, possui memória e conversa por voz.

---

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Configuração](#configuração)
- [Rodando o Projeto](#rodando-o-projeto)
- [Ferramentas Disponíveis](#ferramentas-disponíveis)
- [Integração com iPhone e Siri](#integração-com-iphone-e-siri)
- [Endpoints](#endpoints)
- [Roadmap](#roadmap)

---

## 🧠 Visão Geral

```
iPhone (Siri)
    ↓ HTTP
JarvisAI API (.NET 9)
    ↓
Groq (IA na nuvem — rápida e gratuita)
    ↓
Ferramentas (Clima, Calculadora, Pesquisa, etc.)
    ↓
SQLite (Memória de conversas)
```

---

## 🛠️ Tecnologias

- **ASP.NET Core 9** — Minimal APIs
- **Entity Framework Core** — ORM
- **SQLite** — Banco de dados local
- **Groq** — IA na nuvem (gratuito e rápido)
- **OpenAI SDK** — Comunicação com Groq
- **OpenWeatherMap** — API de clima
- **DuckDuckGo** — Pesquisa na internet
- **Siri Shortcuts** — Interface de voz no iPhone

---

## 📁 Estrutura do Projeto

```
JarvisAI/
├── JarvisAI.Api/               # Endpoints e configuração da API
│   ├── Endpoints/
│   │   └── ChatEndpoints.cs
│   ├── appsettings.json
│   ├── appsettings.Local.json  # (não versionado - criar localmente)
│   └── Program.cs
│
├── JarvisAI.Application/       # Interfaces e DTOs
│   ├── DTOs/
│   │   ├── ChatRequest.cs
│   │   └── ChatResponse.cs
│   └── Interfaces/
│       ├── IChatService.cs
│       └── IToolService.cs
│
├── JarvisAI.Domain/            # Entidades e contratos
│   ├── Entities/
│   │   └── Conversation.cs
│   └── Interfaces/
│       └── ITool.cs
│
├── JarvisAI.Infrastructure/    # Implementações
│   ├── Data/
│   │   └── JarvisDbContext.cs
│   ├── Services/
│   │   ├── ChatService.cs
│   │   └── ToolService.cs
│   └── Tools/
│       ├── TimeTool.cs
│       ├── CalculatorTool.cs
│       ├── WeatherTool.cs
│       └── SearchTool.cs
│
├── mobile/                     # Atalhos do iPhone (Siri Shortcuts)
├── docs/                       # Documentação
└── docker/                     # Configurações Docker
```

---

## ✅ Pré-requisitos

Antes de começar, você vai precisar de:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- iPhone com iOS 16+ (para integração com Siri)
- Conta gratuita no [Groq](https://console.groq.com)
- Conta gratuita no [OpenWeatherMap](https://openweathermap.org/api)

---

## ⚙️ Configuração

### 1. Clonar o repositório

```bash
git clone https://github.com/KaioViniciusMP/JarvisAI.git
cd JarvisAI
```

### 2. Criar o arquivo de configuração local

Crie o arquivo `jarvis-api/JarvisAI/JarvisAI/appsettings.Local.json` com suas chaves:

```json
{
  "Groq": {
    "ApiKey": "sua-chave-do-groq-aqui"
  },
  "OpenWeather": {
    "ApiKey": "sua-chave-do-openweather-aqui"
  }
}
```

> ⚠️ Este arquivo está no `.gitignore` e nunca deve ser versionado.

### 3. Obter as chaves de API

**Groq (IA):**
1. Acesse [console.groq.com](https://console.groq.com)
2. Crie uma conta gratuita
3. Vá em **API Keys → Create API Key**
4. Copie a chave e cole no `appsettings.Local.json`

**OpenWeatherMap (Clima):**
1. Acesse [openweathermap.org](https://openweathermap.org)
2. Crie uma conta gratuita
3. Vá em **API Keys** e copie a chave padrão
4. Cole no `appsettings.Local.json`

> ⚠️ A chave do OpenWeatherMap pode demorar até 2 horas para ativar após a criação.

---

## ▶️ Rodando o Projeto

### Via Visual Studio

1. Abra a solution `JarvisAI.slnx`
2. Selecione o perfil **http** no botão de execução
3. Pressione **F5**
4. A API estará disponível em `http://localhost:5114`

### Via terminal

```bash
cd jarvis-api/JarvisAI/JarvisAI
dotnet run --urls "http://0.0.0.0:5114"
```

### Aplicar as migrations do banco

Na primeira execução, aplique as migrations via Package Manager Console no Visual Studio:

```powershell
Update-Database -Project JarvisAI.Infraestructure -StartupProject JarvisAI
```

---

## 🧰 Ferramentas Disponíveis

| Ferramenta | Descrição | Exemplo de uso |
|---|---|---|
| `TimeTool` | Retorna data e hora atual | "Que horas são?" |
| `CalculatorTool` | Realiza cálculos matemáticos | "Quanto é 250 * 87?" |
| `WeatherTool` | Consulta o clima de uma cidade | "Como está o clima em São Paulo?" |
| `SearchTool` | Pesquisa na internet via DuckDuckGo | "Pesquise sobre IA" |
| `NewsTool` | Busca notícias recentes | "Quais são as últimas notícias?" |
| `ReminderTool` | Cria e lista lembretes | "Me lembre de tomar água às 14h" |

---

## 💡 Endpoints

### `POST /chat`
Envia uma mensagem para o Jarvis.

**Request:**
```json
{
  "message": "Olá Jarvis, qual é meu nome?"
}
```

**Response:**
```json
{
  "response": "Olá! Seu nome é Kaio."
}
```

---

### `GET /tools`
Lista todas as ferramentas disponíveis.

**Response:**
```json
[
  "TimeTool: Retorna a data e hora atual",
  "CalculatorTool: Realiza cálculos matemáticos",
  "WeatherTool: Consulta o clima de uma cidade",
  "SearchTool: Pesquisa informações na internet"
]
```

---

### `POST /tools/{toolName}`
Executa uma ferramenta diretamente.

**Exemplo — Calculadora:**
```
POST /tools/CalculatorTool
```
```json
{
  "message": "250 * 87"
}
```

**Response:**
```json
{
  "response": "Resultado: 21750"
}
```

---

## 📱 Integração com iPhone e Siri

Para conversar com o Jarvis usando a Siri, siga os passos abaixo:

### Pré-requisitos

- iPhone e notebook na **mesma rede Wi-Fi**
- API rodando e acessível pelo IP local
- Descobrir o IP local do notebook: `ipconfig` → **Endereço IPv4**

### Criando o Atalho no iPhone

1. Abra o app **Atalhos** no iPhone
2. Toque em **+** para criar um novo atalho
3. Adicione as seguintes ações na ordem:

**Ação 1 — Pedir Entrada**
```
Pedir: Texto
Com: O que deseja perguntar ao Jarvis?
```

**Ação 2 — Obter Conteúdo da URL**
```
URL: http://SEU_IP_LOCAL:5114/chat
Método: POST
Corpo da Solicitação: JSON
  └── Adicionar campo: Texto
        Chave: message
        Valor: [variável] Pedir Entrada
```

**Ação 3 — Obter Valor do Dicionário**
```
Obter: Valor
Para: response
Em: [variável] Conteúdos do URL
```

**Ação 4 — Retornar Resultado**
```
Resultado: [variável] Valor do Dicionário
```

4. Nomeie o atalho como **Jarvis**
5. Salve

### Acionando pela Siri

Fale para a Siri:
```
"Ei Siri, Jarvis"
```

A Siri vai executar o atalho, perguntar o que você quer e ler a resposta com sua própria voz.

> ⚠️ Se a Siri disser que deu erro, verifique se a API está rodando e se o iPhone está na mesma rede Wi-Fi do notebook.

---

## 🗺️ Roadmap

- [x] Fase 1 — Backend base com endpoint `/chat`
- [x] Fase 2 — Integração com IA (Groq)
- [x] Fase 3 — Integração com Siri via Shortcuts
- [x] Fase 4 — Memória de conversas com SQLite
- [x] Fase 5 — Sistema de ferramentas (Tools)
- [x] Fase 6 — Function Calling automático
- [x] Fase 7 — Ferramentas do dia a dia (Clima, Pesquisa, Notícias, Lembretes)
- [ ] Fase 8 — Controle do computador
- [ ] Fase 9 — Memória inteligente
- [ ] Fase 10 — MCP (Model Context Protocol)
- [ ] Fase 11 — Agente autônomo
- [ ] Fase 12 — Dashboard, autenticação e Docker

---

## 📄 Licença

MIT License — sinta-se à vontade para usar e modificar.
