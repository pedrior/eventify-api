# Eventify API

An Event Platform API built with ASP.NET Core, EF Core, PostgreSQL, Amazon S3, Amazon Cognito, Clean Architecture, DDD,
CQRS and other technologies.

👉 This project was developed for educational purposes, but feel free to use it as a reference or for any other purpose.

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
