#!/bin/bash

# Pull the latest PostgreSQL image from Docker Hub
docker pull postgres:latest

# Run the PostgreSQL container
docker run --name MI16Postgres -e POSTGRES_PASSWORD=yourpassword -p 5432:5432 -d postgres

echo "PostgreSQL container is running. Access it on localhost:5432"