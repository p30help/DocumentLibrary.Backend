# Document Library

## Overview
This project is a Document library (which was implemented by .Net8) that users are able to join then store and manage their documents and also share them with other users.

## Prerequisites
- Install .NET SDK 8.0 or later
- Docker Desktop

## Installation

To running this application you should run Back-end and Front-end projects. this part is related to backend side and <a href="https://github.com/p30help/DocumentLibrary.Angular">DocumentLibrary.Angular</a> is related to Front-end side

Okay, you should follow these steps:

### Step 1: Clone the repository

Open the command prompt (``Cmd``) in your desired directory and execute the following command:

``git clone https://github.com/p30help/DocumentLibrary.Backend.git``

### Step 2: Run Docker compose
Execute these commands in command prompt:

``cd DocumentLibrary.Backend``

Then

``docker compose up -d``

After the Docker Compose ran successfully. The ``MinIO`` and ``Sql Server`` will be activated.

PS: ``MinIO`` is a high-performance, S3 compatible object store. (<a href="https://min.io/">Read More</a>)

### Step 3: Run .Net Project
Execute the following commands:

``dotnet restore --source https://api.nuget.org/v3/index.json``

Then

``dotnet run --project DocumentLibrary.WebApi --no-restore``


After the prject ran properly, you can see the Swagger page in ``https://localhost:7090/``

Now the Backend side is Up, you should run the Front-end side as well to complete the scenario. To do that go to <a href="https://github.com/p30help/DocumentLibrary.Angular">DocumentLibrary.Angular</a> and follow the steps.

## Run the tests

To run the unit tests, open a command prompt in the project directory and execute the following commands:

``dotnet restore --source https://api.nuget.org/v3/index.json``

Then

``dotnet test --no-restore``

This will execute all of the unit tests in the project and provide a report of the results.
