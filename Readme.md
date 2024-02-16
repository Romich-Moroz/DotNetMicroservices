# How to set up testing environment

### 1. Setting up database

All you need to do to ensure that database works is to setup connection string
located here: MicroservicesTestTask\MicroservicesTestTask.DataProcessorService\app.config
**Key = SqLiteDatabaseConnectionString, Value = Actual SqLite connection string**


### 2. Setting up docker container for RabbitMQ

Docker script is ready to use as is but if you want to tweak anything its possible.
Location: MicroservicesTestTask\MicroservicesTestTask.RabbitMQ\docker\docker-compose.yml
To run the container you can use the following script:
**MicroservicesTestTask\MicroservicesTestTask.RabbitMQ\docker\run docker container.bat**


### 3. Setting up services
If you are changing anything in docker yml definition for RabbitMQ then you also should configure services to reflect those changes.
It can be done by changing **app.config** for each service that is using RabbitMQ.
(WARNING) This changes are applied at build time. If you want to change configuration 
after build is done then you should change dll.config file inside bin folder.
The following are **RUNTIME** config files
**MicroservicesTestTask.DataProcessorService.dll.config for data processor**
**MicroservicesTestTask.FileParserService.dll.config for file parser**

The following values are configurable:
1. **RabbitMQClientHostName** for remote hostname configuration
2. **RabbitMQClientPort for** remote host port configuration
3. **RabbitMQClientUsername** for username configuration
4. **RabbitMQClientPassword** for password configuration
5. **RabbitMQClientQueue** for queue name configuration

Each service is a console app so no installation required. To start a service you just run it as any other executable.

### 4. Setting up logging
By default each service is logging its actions, if you want to change output location then update the **LogFile** entry inside
**app.config** for required service.

### 5. Running services
First make sure that docker container is running and then run two services by executing their .exe files.
Alternatively it's possible to setup multiple Visual Studio projects to run.
If any errors occur during service runtime they will be printed out in console.