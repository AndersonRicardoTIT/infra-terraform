# Usadosbr.Contas

Olá 👋, segue algumas instruções para subir o projeto.

Se você ainda não tem um checkout você pode clonar o projeto com

`git clone https://github.com/usadosbr/usadosbr.contas.git`

Para rodar a aplicação você vai precisar do [.NET Core 5](https://dotnet.microsoft.com/download?WT.mc_id=dotnet-35129-website) e do [docker](https://docs.docker.com/get-docker/) instalados.

Atualmente a solução está dividida em 3 projetos

- WebApi: Sobe a pipeline HTTP e a injeção de dependencias.
- Migrations: É uma ferramenta CLI que migra o nosso banco de dados.
- Core: Toda a lógica de infra e de negócio.

O projeto utiliza o [docker compose](docker-compose.yaml) 
para subir os recursos durante desenvolvimento na raiz do projeto use

`docker compose up`

Caso queria rodar a infra necessária para a API mas rodar a API dentro da sua IDE você pode usar

`docker compose up --build migrations`

E em seguidar usar a sua IDE para subir a API.



