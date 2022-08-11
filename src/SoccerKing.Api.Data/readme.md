# Context
- MyContext: Define as tabelas baseado nas entidades vindas do Domain;
- ContextFactory: Criar a conexão com base de dados para realizar as Migrations;
# Implementations
# Mapping
- Mapeia as configurações de como as entidades serão representadas no banco de dados;
# Migrations
- Armazena as migrations já feitas ou que serão feitas;
# Repository
- Encapsula a lógica de acesso aos dados, agindo como intermediário entre o Domain e o mapeamento de dados;