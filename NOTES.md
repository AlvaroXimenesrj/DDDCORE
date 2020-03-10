# Anotações

## 04 - Classe Startup e Middlewares

Middlewares: pipeline entre a request e a response.

OWIN: Open Web Interface for .NET

<http://owin.org/spec/spec/owin-1.0.0.html>

Especificação de como separar a aplicação do web-server.

Interface entre Web Host e ASP.NET Components:

- *Environment Dictionary:* IDictionary<string, object>
- *Application Delegate:* Func<IDictionary<string, object>, Task>

System.Web estava fortemente ligado ao IIS.

Exemplos de middleware:

- MVC Core
- Identity
- Logging

Interfaces:

- *IHostingEnvironment:* Fornece informações sobre o ambiente de hospedagem Web em que uma aplicação está sendo executada.
- *IConfigurationRoot:* Representa a raiz de uma hierarquia (Microsoft.Extensions.Configuration.IConfiguration.
- *IServiceCollection:* Representa um contrato para uma coleção de serviços. Essa interface é estendida por classes que implementam Middlewares através do recurso de injeção de dependência.
- *IApplicationBuilder:* Representa um contrato para classes que irão prover mecanismos de configuração de um Middleware. Essa interface é estendida por classes que implementam métodos de configuração de cada Middleware.
- *ILoggerFactory:* Representa um tipo usado para configurar o log de sistema e criar instâncias da classe ILogger. É utilizada com o objetivo de capturar dados para log.

Demo WebApplication Core:

*User Secrets:* install-package Microsoft.Extensions.Configuration.UserSecrets

Ao clicar com o botão direito no projeto, tem a opção "Manage User Secrets". Abrirá um arquivo Json que pode armazenar chave e valor.

Configurar no startup:

```bash
//User Secrets
if (env.IsDevelopment())
{
    builder.AddUserSecrets<Startup>();
}

//Recuperar a chave
var email = Configuration["Email"];
```

Utilizar o dot.net cli para gerenciar as chaves (verificar porque não funcionou no 2.0):

- dotnet user-secrets set Email teste@teste.com.br
- dornet user-secrets list

Host != Server

Host: o que você vai fazer para a aplicação rodar. O Kestrel representa o host.
Server: o IIS servirá de proxy reverso.

Dentro do projeto tem um arquivo launchSettings.json que possui todos os profiles de execução do projeto.

Cada profile define um environment. Essa propriedade do environment pode ser utilizada na definição do layout por exemplo:

```bash
<environment include="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.css" />
    <link rel="stylesheet" href="~/css/site.css" />
</environment>
<environment exclude="Development">
    <link rel="stylesheet" href="https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/css/bootstrap.min.css"
            asp-fallback-href="~/lib/bootstrap/dist/css/bootstrap.min.css"
            asp-fallback-test-class="sr-only" asp-fallback-test-property="position" asp-fallback-test-value="absolute" />
    <link rel="stylesheet" href="~/css/site.min.css" asp-append-version="true" />
</environment>
```

## 05 - Padrão MVC e Funcionalidades do ASPNET MVC Core

IActionResult pode retornar várias opções.

HTTP verbos básicos:

- get: Pede uma informação ao server. É feito através da URL.
- post: envia informações ao servidor (formulários).
- put: similar ao post, envia informações ao servidor. É utilizado para atualizar informações existentes. Utilizado em APIs Rest, MVC não.
- delete: solicita a exclusão de informação no servidor através da URL indicada. Utilizado em APIs Rest, MVC não.

Rotas por atributo:

```bash
[Route("")]
[Route("dashboard/tela-inicial")]
[Route("dashboard/tela-inicial/{id:int}/{valor:guid}")]
```

Para os parâmetros você pode definir o tipo do parâmetro e até mesmo definir uma expressão regular para o parâmetro ser aceito.

DTO: Data Transfer Objects. Diminuir o número de requisições no servidor.

Uma View só pode renderizar uma Model, dessa forma um DTO é necessário (classe ViewModel).

Razor Views: motor de renderização do MVC. Podemos pensar em Views fortemente tipadas. Razor transforma as Views em arquivos HTML puros para a interpretação dos browsers.

Tag Helpers: são os recursos do Razer para geração de HTML. Antes era algo como @Html.EditFor e agora ficou "label asp-for". Muito mais amigável.

View components: possuem processamento server-side independente e podem realizar ações como obter dados de uma tabela e exibir valor manipulados. Ex: componentizar um recurso de página como um carrinho de compras.

ViewComponent: uma classe e sempre deverá ter um método Invoke que retorna um IViewComponentResult.

Todo ViewComponent deverá ter uma View que estará na pasta Shared -> Components -> e o nome do seu component.

## 06 - Apresentando o Entity Framework Core

ORM: Object-relational mapping

EF Core x EF 6.x: EF Core ainda está imaturo, porém está pronto para utilização.

Data annotations não é obrigatório e dependendo do código pode deixar o código poluído.

Fluent API é uma solução para o data annotations.

Data annotations: mais aplicável nas validações do front-end.

Fluent API: mais aplicável na definição do modelo para o EF Core.

## 07 - Identity 3.0 e Segurança no ASPNET Core

- ASP.NET Identity
- Configuração
- Autenticação
- Autorização
- Claims
- Roles
- Regras de autorização customizadas

É possível customizar a tabela AspNetUsers, porém é recomendado criar uma tabela de negócio para armazenar os dados dos clientes.

Podemos controlar o acesso as cotrollers incluindo as anotações de Authorize, porém essa abordagem é muito verbosa (magic strings).

Na classe startup podemos definir o acesso baseado em Claims (definir uma policy).

Claims e Roles ficam no cookie na máquina do usuário.

Não tem uma tabela AspNetClaims, o ideal seria criar uma tabela para armazenar as claims e depois relacioná-las por roles na AspNetRoleClaims.

<https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity>

## 08 - Ferramentas de Front-End

NPM: Node Package Manager.

Bower: gerenciador de pacotes front-end.

GRUNT: Task Runner.

<https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier>

<http://cmder.net/>

Yeaoman: ajudará na criação dos nossos projetos.

## 09 - Iniciando o desenvolvimento da aplicação Eventos.IO

- Criar blank solution;
- Criar diretório src (código fonte);
- Criar diretório tests (classes de teste);
- Criar diretório docs (documentação);
- Criar diretório sql (guardar algum script);

No Visual Studio, criar solution folders:

- 1 - Presentation
- 2 - Services
- 3 - Application
- 4 - Domain
- 5 - Infra
- 5.1 - Data
- 5.2 - CrossCutting

Criar um arquivo global.json com o seguinte conteúdo:

```bash
{
  "projects": [ "src", "tests" ],
  "sdk": {
    "version": "2.0.2"
  }
}
```

## 10 - Modelando o domínio - Entidades e Validações

![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/Responsabilidadedascamadas.PNG)

Criação do projeto de domínio e do projeto de domínio core.

Para a validação do domínio utilizaremos o FluentValidation <https://github.com/JeremySkinner/FluentValidation>.

## 11 - Revisitando o Domínio - Implementando CQRS

![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/CQRS.PNG)

Entidade: possui entidade e pode ser representada por um registro no banco de dados. Exemplo: categoria.

Agregados: é uma entidade que possui várias entidades que se comporta como uma só. Exemplo: evento.

Para cada raiz de agregação, apenas uma classe de repositório.

*CQRS:* Command Query Responsibility Segregation

Um command nada mais é que um Dto que será persistido no banco de dados.

Um command precisa de um timestamp para sabermos quando o comando foi executado.

O projeto Domain.Core é o Shared Kernel.

Pasta events: todo evento nada mais é que uma mensagem.

UnitOfWork:

Consistência eventual: trabalha com filas.

Event sourcing: salvar os eventos numa base. Guardar todos os passos do evento enquanto ele existir.

- Comando é enviado.
- Evento é lançado.

Construtores: manter apenas um público. Utilizar factories para atender todas as necessidades.

## 12 - Tornando o domínio mapeavel para o banco de dados

Raiz de agregação Evento que possui uma entidade Endereço.

## 13 - Criando a camada de dados

Utilizando o Core 2.0, ao executar o add-migration Initial foi gerado o seguinte erro:

The property 'ValidationFailure.AttemptedValue' could not be mapped, because it is of type 'object' which is not a supported primitive type or a valid entity type. Either explicitly map this property, or ignore it using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.

Esse erro é referente a propriedade ValidationResult que já estamos ignorando da seguinte forma:

```bash
modelBuilder.Entity<Evento>().Ignore(e => e.ValidationResult);
modelBuilder.Entity<Endereco>().Ignore(e => e.ValidationResult);
```

!!!Divetta!!!
Para resolver o problema tive que usar o data annotation '[NotMapped]' na propriedade na classe Entity:

```bash
[NotMapped]
public ValidationResult ValidationResult { get; protected set; }
```

Ou seja, não funciona somente com Fluent API.

Quando tiver mais de um DbContext na solution você deve especificar o nome da seguinte forma:

```bash
update-database -Context EventosContext
```

## 14 - Criando a camada de Aplicação

Se quiser simplificar o seu modelo, a camada de aplicação poderia ser substituída pela Controller do MVC, é o que ocorre com SPA por exemplo. Quem assume o papel de application é a WebApi.

A camada de aplicação que orquestra a execução dos métodos no domínio.

Como a camada de aplicação está fortemente ligada a camada de apresentação, é recomendável que se crie uma camada de aplicação para cada front-end.

![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/ApplicationLayer.PNG)

Application layer: dependente dos casos de uso (Data Transfer Objects - ViewModels, Application Services).
Application services: conversão das Dtos e execução dos serviços de domínio.

![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/CamadasNegocio.PNG)

Domain layer: independe de casos de uso (Domain Model, Domain Services).
No nosso projeto o CQRS está fazendo o papel de Domain Services.

Criaremos mapeamentos de:

- Domain para ViewModel
- ViewModel para Command

## 15 - Desenvolvendo o projeto MVC (Camada de Apresentação)

Nesse video foi criado o projeto de Injeção de Dependência.

Injeção de dependência (DI) é uma técnica para a obtenção de um acoplamento fraco entre objetos e seus colaboradores ou dependências. Em vez de diretamente a instanciação de parceiros, ou usando referências estáticas, os objetos que precisa de uma classe para realizar suas ações são fornecidos para a classe de alguma forma. Geralmente, classes irão declarar suas dependências por meio de seu construtor, possibilitando que siga a princípio de dependências explícitas. Essa abordagem é conhecida como "injeção de construtor".

Quando um sistema é projetado para usar a DI, com muitas classes solicitando suas dependências por meio de seu construtor (ou propriedades), é útil ter uma classe dedicada à criação dessas classes com suas dependências associadas. Essas classes são chamadas de contêineres, ou, mais especificamente, inversão de controle (IoC) contêineres ou contêineres de injeção de dependência (DI).

Um contêiner é essencialmente uma fábrica que é responsável por fornecer instâncias dos tipos que são solicitados a partir dele. Se um determinado tipo declarou que ele tem dependências, e o contêiner tiver sido configurado para fornecer os tipos de dependência, ele criará as dependências como parte da criação da instância solicitada. Dessa forma, os gráficos de dependência complexos podem ser fornecidos para classes sem a necessidade de qualquer construção de objeto inserido no código. Além de criar objetos com suas dependências, contêineres normalmente gerenciar tempos de vida de objeto dentro do aplicativo.

Diferença nos tipos:

- AddTransient: objects are always different; a new instance is provided to every controller and every service.
- AddScoped: objects are the same within a request, but different across different requests.
- AddSingleton: objects are the same for every object and every request (regardless of whether an instance is provided in ConfigureServices).

Consistência imediata: o InMemoryBus está ok.

Consistência eventual: você precisaria implementar algum tipo de fila.

!!!Divetta!!!
Tive alguns problemas com o Automapper na versão do Core 2.0 na ação de atualização do evento, mais especificamente no comando de atualizar endereço. Fiz o mesmo procedimento do RegistrarEvento:

```bash
CreateMap<EventoViewModel, AtualizarEventoCommand>()
    .ConstructUsing(c => new AtualizarEventoCommand(c.Id, c.Nome, c.DescricaoCurta, c.DescricaoLonga, c.DataInicio, c.DataFim, c.Gratuito, c.Valor, c.Online, c.NomeEmpresa, c.OrganizadorId, c.CategoriaId,
        new AtualizarEnderecoEventoCommand(c.Endereco.Id, c.Endereco.Logradouro, c.Endereco.Numero, c.Endereco.Complemento, c.Endereco.Bairro, c.Endereco.CEP, c.Endereco.Cidade, c.Endereco.Estado, c.Id)));
```

## 16 - Complementando as funcionalidades de Apresentação

Implementação do Dapper: micro ORM. <https://github.com/StackExchange/Dapper/>

Vantagens:

- Não coloca no cache.
- Não faz tracking.

Performance quase do ADO.NET.

Exemplo de implementação usando join:

```bash
public override Evento ObterPorId(Guid id)
{
    var sql = @"SELECT * FROM Eventos E " +
                "LEFT JOIN Enderecos EN " +
                "ON E.Id = EN.EventoId " +
                "WHERE E.Id = @uid";

    //<Evento, Endereco, Evento>
    //Primeiro objeto, segundo objeto e o de retorno
    var evento = Db.Database.GetDbConnection().Query<Evento, Endereco, Evento>(sql,
        (e, en) =>
        {
            if (en != null)
                e.AtribuirEndereco(en);

            return e;
        }, new { uid = id });

    return evento.FirstOrDefault();
}
```

Criar um ViewComponent para exibir as notificações geradas.

```bash
    public class SummaryViewComponent : ViewComponent
    {
        private readonly IDomainNotificationHandler<DomainNotification> _notifications;

        public SummaryViewComponent(IDomainNotificationHandler<DomainNotification> notifications)
        {
            _notifications = notifications;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notifications.GetNotifications());
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Value));

            return View();
        }
    }
```

A view desse ViewComponent será assim:

```bash
@if (ViewData.ModelState.ErrorCount > 0)
{
    <div class="alert alert-danger">
        <button type="button" class="close" data-dismiss="alert">×</button>
        <h3 id="msgRetorno">Opa! Alguma coisa não deu certo:</h3>
        <div asp-validation-summary="All" class="text-danger"></div>
    </div>
}
```

Para utilizar esse ViewComponent com TagHelper, precisamos definir no _ViewImports:

```bash
@addTagHelper"*, Eventos.IO.Site"
```

Utilizando o TagHelper:

```bash
<vc:summary />
```

Implementar toaster para feedback para o usuário. <http://codeseven.github.io/toastr/demo.html>

É possível incluir um arquivo de resource para traduzir as mensagens no front-end <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization>. Porém, como a aplicação não será multi-idioma, não utilizaremos esse recurso. Para simplificar no caso do campo valor:

```bash
<input asp-for="Valor" data-val-number="O valor está em formato inválido" class="form-control" />
```

Criando um método de extensão para obter o Guid do usuário logado:

```bash
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }
    }
```

## 17 - Finalizando as funcionalidades de Apresentação

O Identity foi movido para um projeto dentro do projeto de CrossCutting.

É possível injetar diretamente na View:

```bash
@inject IUser user
```

## 18 - Tratamento e Log de Erros e Auditoria no MVC

!!!Divetta!!!
Como estou usando o .net core 2.0, algumas propriedades foram removidas (Claims) <https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x>.

Verificar conceito de policy e claim.

403: sem acesso.
401: não logado.

Log provider para gravar os logs no banco de dados ou qualquer outro lugar. Para aplicações grandes, recomendo a utilização de um serviço para isso, a mais conhecida <https://elmah.io/>

Verificar alternativa para o elmah on premises.

## 19 - Camada de Serviços ASPNET WebAPI

- Rest vs SOAP

![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/RestVsSOAP.PNG)

- Serviços REST
- Controller unificada
- Criando minha primeira API REST
- Segurança
- Serviços em uma aplicação SPA
- Abstraindo responsabilidades com MediatR

Implementação da classe RouteConvention para criar convenção dos nomes das apis, ou seja, não será mais necessário decorar nossas controllers com "/api" e depois com "/v1" ou a versão desejada. Essa classe fará esse trabalho para nós. Nossas controllers terão um parâmetro a mais chamado "version" que indicará a versão requisitada pelo cliente da Api.

Configuração do Swagger.

Tendo uma propriedade RequestDelegate, quando for registrado será considerado um middleware.

Além disso, deverá ter um método invoke.

Verificar como fazer na versão 2.0: options.Cookies.ApplicationCookie.AutomaticChallenge = false; Esse trecho não existe mais no 2.0. Basta remover do startup app.UseIdentity();

<https://Auth0.com>

<https://identityserver.io>

```bash
//Cross origin request => por padrão desabilitado
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
    //Exemplos
    //c.WithOrigins("www.eventos.io,www.site.com");
    //c.WithMethods("POST");
});
```

## 20 - Introdução ao Angular

Apresentação sobre o Angular e escolha do Angular CLI.
![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/20_AngularLinguagens.PNG)

![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/20_CriandoAplicacao.PNG)

## 21 - Angular Startup Template

![Alt Text](https://raw.githubusercontent.com/divetta/CursoAspNetCoreAngular/master/_imagens/21_ArquiteturaAplicacao.PNG)

ngx-bootstrap

Link sobre explicação sobre o package-lock.json <https://www.codeproject.com/Articles/1202361/What-is-package-lock-json-file-in-Node-NPM>

## 22 - Rotas e recursos de navegação do Angular

Dicas de layout:

<https://bootsnipp.com/>

<https://bulma.io/>

<https://placeholder.com/>

Sobre o serviço SEO, na versão do video ainda era possível incluir referência para uma pasta src de um pacote. Na versão atual não é possível.

Atualizei o serviço para utilizar o serviço Meta que possibilita a manipulação dos itens.

Publicação:

```bash
ng b -prod
```

Para rodar no IIS será necessário instalar o IIS URL Rewriter <https://www.iis.net/downloads/microsoft/url-rewrite>.

Além disso iremos adicionar um arquivo web.config na raiz do site da seguinte forma:

```bash
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<system.webServer>
		<rewrite>
			<rules>
				<rule name="Angular" stopProcessing="true">
					<match url=".*" />
					<conditions logicalGrouping="MatchAll">
						<add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
						<add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
					</conditions>
					<action type="Rewrite" url="/" />
				</rule>
			</rules>
		</rewrite>
	</system.webServer>
</configuration>
```