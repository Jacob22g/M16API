#!/bin/bash

# Pull the latest PostgreSQL image from Docker Hub
docker pull postgres:latest

# Run the PostgreSQL container
docker run --name my_postgres_container -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres

echo "PostgreSQL container is running. Access it on localhost:5432"