# SGC Backend - Sistema De Gerenciamento Comercial

SGC é um sistema online para gerenciamento de lojas comerciais, composto basicamente de três módulos: 
**Módulo de Clientes, Módulo de Produtos, Módulo de Vendas.** Cada um dos módulos foi desenvolvido tendo em mente as necessidades das áreas a quais se direcionavam.


## Arquitetura do Sistema
Em termos de aplicação, embora esta esteja condensada em um único projeto para facilitar o desenvolvimento, a arquitetura do sistema foi baseada na arquitetura de 
microserviços, de modo que os três principais módulos do sistema (citados na seção anterior) não possuem dependências entre si, se utilizando de mensageria
quando necessitam notificar sobre alterações que ocorreram em seus domínios.

Além destes, o sistema possui ainda dois outros projetos: O projeto **Aplicação**, composto pela API e Serviços que consomem as mensagens publicadas via mensageria,
e o projeto **Core**, composto por classes e interfaces implementadas pelos módulos de negócios (Cliente, Produto e Venda) do sistema. Este último é composto por:

1. Commands : Interfaces para implementação dos comandos (requisições dos usuários) e sua execução.
2. EventMessages: Interfaces e Classe utilizada na implementação de eventos (notificações de operação realizada) e sua execução.
3. Infrastructure: Interfaces e classes utilizadas na implementação da camada de repositório e dados (Unit Of Work e Database Context).
4. MessageBroker: Interfaces e implementação dos componentes utilizados no disparo e recebimento de mensagens (Publisher e Subscriber).

Os módulos de negócios estão divididos em camadas:

1. Camada de Domínio: Encapsula toda a lógica de negócios para o módulo. Em nosso sistema, é subdividido em:
   1. Contém as entidades e objetos de valores necessários ao módulo. 
   2. Commands: Implementação dos comandos que podem ser executados pelo domínio.
   3. Events: Implementação das mensagens de eventos que são enviados ou recebidos pelo domínio através de mensageria.
   4. Repository: Interface do repositório para as chamadas feitas ao banco de dados.
2. Camada de Infraestrutura (Infrastructure): Encapsula as classes necessárias para implementação do padrão Unit Of Work/Repository e a implementação da classe
de acesso à base de dados.
3. Query: Encapsula os DTOs (Data Objects) que representam a entidade para a camada de visualização (API) e o serviço de consultas ao banco de dados.

O diagrama da arquitetura do sistema pode ser observado abaixo:
<DIAGRAMA>

## Módulos do Sistema

### Módulo de Clientes
1. Consulta de Clientes
2. Cadastro de Clientes
3. Atualização de Cadastro de Clientes
4. Gerenciamento de Status (Ativo/Inativo)

As operações 2, 3 e 4 publicam mensagens em canais específicos notificando a alteração de registros.

### Módulo de Produtos
1. Consulta de Produtos
2. Cadastro de Produtos
3. Atualização de Cadastro de Produtos
4. Gerenciamento de Status (Ativo/Inativo)
5. Gerenciamento de Estoque

As operações 2, 3, 4 e 5 publicam mensagens em canais específicos notificando a alteração de registros.

### Módulo de Vendas
O módulo de vendas, como o próprio nome sugere, é o responsável pelo gerenciamento e execução das operações de venda. O processo consiste basicamente de:

1. Dar entrada em um registro de venda (informando o código do cliente ao qual a venda está sendo feita)
2. Adicionar produtos a uma venda
3. Editar dados de venda (desconto aplicado, forma de pagamento)
4. Processar a venda

A operação de venda, no entanto, depende dos produtos e clientes que são gerenciados pelos outros dois módulos. A fim de evitar o acoplamento entre estes,
o módulo de venda possui em seu banco de dados as informações de produtos e clientes que são espelhadas de seus respectivos módulos. Os produtos e clientes são 
atualizados ou adicionados via eventos. Por exemplo, uma ação de cadastro de cliente:

1. Cliente cadastrado no módulo de Clientes.
2. Evento de Notificação de Cliente Cadastrado é disparado.
3. Consumidor recebe a mensagem e a repassa ao EventHandler do módulo de Vendas (ClienteAtualizadoEvent).
4. O cliente é registrado/atualizado no banco de dados do módulo de vendas.

Dessa forma, nenhum comando pode ser executado no módulo de vendas para cadastrar/atualizar/ativar/inativar clientes ou produtos, visto que essa é uma responsabilidade
de seus próprios módulos, e a sincronização desses dados é realizada via mensageria, como já fora mencionado. Esse modelo isola os módulos e, em um posterior
desenvolvimento que venha a separar os módulos em sistemas que operam independentemente, o Módulo de Vendas poderá continuar operando mesmo que o sistema de Clientes 
e/ou Produtos esteja indisponível.

Outro ponto a ser ressaltado: para que a operação de venda aconteça com sucesso, o registro da venda deve passar por todas as validações que estão definidas como
regras de negócio na entidade Venda. O fluxo do processamento de uma venda (confirmação por parte do usuário e aprovação por parte do sistema) pode ser observado abaixo:
<FLUXO>

## Dependências e Ferramentas Utilizadas















