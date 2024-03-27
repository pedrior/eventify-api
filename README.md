# Eventify API

A REST API built using ASP.NET Core, EF Core, PostgreSQL, AWS, Clean Architecture, DDD, CQRS, and other technologies. This project exemplifies top coding standards, providing a neat, effective, and scalable solution.

👉 This project was developed for educational purposes, but feel free to use it as a reference or for any other purpose.

## :pushpin: Index

- [Requirements](#memo-requirements)
- [Project](#sparkles-project)
- [Getting Started](#runner-getting-started)
- [Endpoints](#globe_with_meridians-endpoints)
- [License](#page_with_curl-license)

## :memo: Requirements

To successfully build and run this project, you will need the following tools properly configured:

- [ASP.NET Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [EF Core CLI Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [Docker](https://www.docker.com/)
- [Amazon S3](https://aws.amazon.com/s3/)
- [Amazon Cognito](https://aws.amazon.com/cognito/)

:information_source: In the `/aws` directory, you will find the `cognito-functions` sln with a Lambda Function implementation to handle the 
AWS Cognito [Pre Sign-Up](https://docs.aws.amazon.com/cognito/latest/developerguide/user-pool-lambda-pre-sign-up.html#user-pool-lambda-pre-sign-up-flows) event.
This Lambda Function is used to automatically confirm the user during registration. Build and deploy this Lambda function to AWS and configure Cognito to trigger it.
Otherwise, manual user confirmation will be required. Also, create the user groups `attendee` and `producer` to manage user roles in the system.

## :sparkles: Project

### :bulb: Overview

Eventify is a simple event platform where producers can create and manage events, and attendees can search for events and book tickets.

### :triangular_ruler: Architecture

The project follows the Clean Architecture principles inspired by [Jason Taylor's Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture),
with some modifications to suit my preferences and needs.

![Clean Architecture Design](./images/clean-architecture.jpg)

## :runner: Getting Started

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
> See [Environment variables to configure the AWS CL](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-envvars.html) for further details.

3. Create a certificate for the API to run on Docker over HTTPS

```shell
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Eventify.Presentation.pfx -p password
dotnet dev-certs https --trust
```

> :warning: Make sure that the certificate name and password match those specified in the `docker-compose.yml` file.
> See [Developing ASP.NET Core Applications with Docker over HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md) for further details.

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

## :globe_with_meridians: Endpoints

### :pushpin: Endpoints Index

- [Account Endpoints](#account)

__Location__

The API is available at `https://localhost:8081/api/{version}`.

__Versioning__

The API is versioned through the URL path. By default, all endpoints use the `v1` version of the API.

__Authorization__

The API utilizes JWT Bearer Authentication for endpoint protection. When making authenticated requests, you need to include the access token in the `Authorization` header with the `Bearer` scheme.

> :information_source: You can obtain an `access_token` by sending a request to the `login` endpoint.

If you fail to provide the access token or provide an invalid one, you will get a `401 Unauthorized` response. If you provide a valid access token but lack authorization for the request, you will receive a `403 Forbidden` response.

```http
Authorization: Bearer {access_token}
```

__Rate Limiting:__

Most endpoints have rate limits to prevent abuse. If you surpass the request limit within a specific time frame, you will get a `429 Too Many Requests` response.

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

The API provides error responses compliant with RFC 7807. The response may contain additional fields to provide a more detailed description of the error.

Examples:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.10",
  "title": "The producer profile already exists",
  "status": 409,
  "traceId": "00-644db4453cc9472b833eb729cb4f5db1-96612bcce84a96ad-00",
  "code": "producer.profile_already_created"
}
```

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "language": [
      "Must be a supported language."
    ],
    "period_start": [
      "Must be in the future."
    ],
    "location_name": [
      "Must not be empty.",
      "Must contain only letters, numbers and special characters."
    ]
  },
  "traceId": "00-204f3dc9742993c27181094fa3f707ff-8b8ec56e88d528e4-00"
}
```

### Account

#### Login

Allows users to authenticate within the system. The response includes the tokens for accessing the protected endpoints.

```http
POST /v1/account/login
```

__Request example:__

```json
{
  "email": "john@doe.com",
  "password": "JohnDoe123"
}
```

__Body Parameters:__

| Parameter | Type   | Description          | Required |
|-----------|--------|----------------------|----------|
| email     | string | The user's email.    | Yes      |
| password  | string | The user's password. | Yes      |

__Expected response:__

200 - OK

```json
{
  "access_token": "eyJraWQiOiI0ZlhUQUtrTlZRcHVZS21zM3oya0g0ZUFGMlFjUUZra2NtY2l6UE1z...",
  "refresh_token": "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ...",
  "expires_in": 3600
}
```

<hr/>

#### Register

Allows users to register an account within the system.

```http
POST /v1/account/register
```

__Request example:__

```json
{
  "email": "john@doe.com",
  "password": "JohnDoe123",
  "given_name": "John",
  "family_name": "Doe",
  "phone_number": "+5581999999999",
  "birth_date": "2000-09-05"
}
```

__Body Parameters:__

| Parameter     | Type   | Description               | Required |
|---------------|--------|---------------------------|----------|
| email         | string | The user's email.         | Yes      |
| password      | string | The user's password.      | Yes      |
| given_name    | string | The user's given name.    | Yes      |
| family_name   | string | The user's family name.   | Yes      |
| phone_number  | string | The user's phone number.  | No       |
| birth_date    | string | The user's birth date.    | No       |

__Expected response__

201 - Created

<hr/>

### Attendee

#### Get Attendee Profile

Retrieves the attendee's profile information.

```http
GET /v1/attendees
```

__Expected response__

200 - OK

```json
{
  "given_name": "John",
  "family_name": "Doe",
  "email": "john@doe.com",
  "phone_number": "+5581999999999",
  "birth_date": "2000-09-05",
  "picture_url": "https://{storage-name}.s3.{storage-region}.amazonaws.com/attendees/{attendee-id}/picture",
  "created_at": "2024-03-17T20:22:20.1354120-03:00",
  "updated_at": "2024-03-22T10:30:41.3698490-03:00"
}
```

<hr/>

## :page_with_curl: License

This project is licensed under the terms of the [MIT](https://github.com/pedrior/eventify-api/blob/master/LICENSE)
license.
