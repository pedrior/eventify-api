# Eventify API

A REST API built using ASP.NET Core, EF Core, PostgreSQL, AWS, Clean Architecture, DDD, CQRS, and other technologies.
This project exemplifies top coding standards, providing a neat, effective, and scalable solution.

👉 This project was developed for educational purposes, but feel free to use it as a reference or for any other purpose.

## :pushpin: Index

- [Project](#sparkles-project)
- [Getting Started](#runner-getting-started)
- [API](#globe_with_meridians-api)
- [License](#page_with_curl-license)

## :sparkles: Project

### :bulb: Overview

Eventify is a simple event platform where producers can create and manage events, and attendees can search for events
and book tickets.

__Some cool features you will discover here:__

- __Clean Architecture__: The project follows the principles of Clean Architecture, which emphasize separating concerns
  and defining clear responsibilities.
- __DDD__: A software design approach that prioritizes the problem domain over technology, aiming to develop systems
  that align with the language and requirements of the business.
- __CQRS__: An architectural pattern that separates the read and update operations of a data store (in this project,
  encouraged more as a code style, as we use a single database).
- __API Versioning__: The project adopts versioning to facilitate progressive changes while maintaining backward
  compatibility.
- __API Rate Limiting:__ The project adopts rate limiting to prevent request abuses.
- __Authentication and Authorization__: The project implements authentication and authorization with JWT.
- __Unit Testing__: The solution includes individual test projects for unit testing the API layers.
- __Database Transactions__: The project supports database transactions. Each command that implements the ITransactional
  interface is executed in a single transaction.
- __Pipeline Behaviors__: The project implements various Pipeline Behaviors for managing Authorization, Exceptions,
  Logging, Transactions, and Validations.

### :triangular_ruler: Architecture

The project follows the Clean Architecture principles inspired
by [Jason Taylor's Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture),
with some modifications to suit my preferences and needs.

![Clean Architecture Design](./images/clean-architecture.jpg)

## :runner: Getting Started

### :memo: Requirements

To successfully build and run this project, you will need the following tools properly configured:

- [ASP.NET Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [EF Core CLI Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [Docker](https://www.docker.com/)
- [Amazon S3](https://aws.amazon.com/s3/)
- [Amazon Cognito](https://aws.amazon.com/cognito/)

:information_source: In the `/aws` directory, you will find the `cognito-functions` sln with a Lambda Function
implementation to handle the
AWS
Cognito [Pre Sign-Up](https://docs.aws.amazon.com/cognito/latest/developerguide/user-pool-lambda-pre-sign-up.html#user-pool-lambda-pre-sign-up-flows)
event.
This Lambda Function is used to automatically confirm the user during registration. Build and deploy this Lambda
function to AWS and configure Cognito to trigger it.
Otherwise, manual user confirmation will be required. Also, create the user groups `attendee` and `producer` to manage
user roles in the system.

1. Clone the repository

```shell
git clone https://github.com/pedrior/eventify-api.git
cd eventify-api
```

2. Configure the required application settings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your PostgreSQL connection string (Skip if you're using Docker Compose)"
  },
  "AWS": {
    "Region": "Your AWS Region",
    "UserPoolClientId": "Your AWS Cognito User Pool Client Id",
    "UserPoolClientSecret": "Your AWS Cognito User Pool Client Secret",
    "UserPoolId": "Your AWS Cognito User Pool Id"
  },
  "Storage": {
    "Bucket": "Your S3 Bucket Name"
  }
}
```

> :warning: Make sure you have configured the AWS credentials in the environment variables.
>
See [Environment variables to configure the AWS CL](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-envvars.html)
for further details.

3. Create a certificate for the API to run on Docker over HTTPS

```shell
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Eventify.Presentation.pfx -p password
dotnet dev-certs https --trust
```

> :warning: Make sure that the certificate name and password match those specified in the `docker-compose.yml` file.
>
See [Developing ASP.NET Core Applications with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md)
for further details.

4. Create and apply the database migration

> :information_source: In this step, you need to have the database running for the migration to be successful.

```shell
dotnet ef migrations add Initial -s src/Eventify.Presentation -p src/Eventify.Infrastructure -o ./Common/Persistence/Migrations
dotnet ef database update -s src/Eventify.Presentation -p src/Eventify.Infrastructure
```

5. Run with Docker Compose

```shell
docker compose up --build
```

## :globe_with_meridians: API

The API offers endpoints for handling events, tickets, bookings, and users. You can locate all `*.http` samples in the 
`src/Eventify.Presentation/Requests directory`, and execute them using any REST client.

__Location__

The API is available at `https://localhost:8081/api/{version}`.

__Versioning__

The API is versioned through the URL path. By default, all endpoints use the `v1` version of the API.

__Authorization__

The API uses JWT Bearer Authentication for endpoint protection. When making authenticated requests, you need to
include the access token in the `Authorization` header with the `Bearer` scheme.

> :information_source: You can obtain an `access_token` by sending a request to the `/v1/account/login` endpoint.

If you fail to provide the access token or provide an invalid one, you will get a `401 Unauthorized` response. If you
provide a valid access token but lack authorization for the request, you will receive a `403 Forbidden` response.

```http
Authorization: Bearer {access_token}
```

__Rate Limiting:__

Most endpoints have rate limits to prevent abuse. If you surpass the request limit within a specific time frame, you
will get a `429 Too Many Requests` response.

__Responses__

The API uses standard HTTP response codes to indicate the success or failure of an API request.

| Code                        | Description                                                                                     |
|-----------------------------|-------------------------------------------------------------------------------------------------|
| 200 - OK                    | Everything worked as expected.                                                                  |
| 201 - Created               | Everything worked as expected, and as result, has created a new resource.                       |
| 204 - No Content            | Everything worked as expected, but is not going to return any content.                          |
| 400 - Bad Request           | The request was unacceptable, often due to missing a required parameter or malformed parameter. |
| 401 - Unauthorized          | The request requires user authentication.                                                       |
| 403 - Forbidden             | The user is authenticated but not authorized to perform the request.                            |
| 404 - Not Found             | The requested resource doesn’t exist.                                                           |
| 409 - Conflict              | The request could not be completed due to a conflict with the current state of the resource.    |
| 422 - Unprocessable Entity  | The request body was acceptable but unable to be processed, often due to business logic errors. |
| 429 - Too Many Requests     | Too many requests hit the API too quickly.                                                      |
| 500 - Internal Server Error | An unexpected error occurred.                                                                   |

__Errors__

The API provides error responses compliant with RFC 7807. The response may contain additional fields to provide a more
detailed description of the error.

Examples:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.10",
  "title": "Conflict",
  "status": 409,
  "traceId": "00-9544cfe6df9d42a0553f910ca6d228b7-6469fbed19a87208-00",
  "errors": [
    {
      "code": "event.slug_already_exists",
      "message": "The slug is already associated with another event",
      "details": {
        "slug": "awesome-event-slug"
      }
    }
  ]
}
```

Validation errors:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "slug": [
      "Must not be empty.",
      "Must contain only lowercase letters, numbers, dashes and must not start or end with a dash."
    ]
  },
  "traceId": "00-3d6a0b01f3384b9327b7de83c37c8005-792cece6737ab181-00"
}
```

## :page_with_curl: License

This project is licensed under the terms of the [MIT](https://github.com/pedrior/eventify-api/blob/master/LICENSE)
license.
