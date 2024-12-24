#!/bin/bash

# Start the frontend
echo "Starting frontend..."
(cd client && pnpm dev) &

# Start the backend
echo "Starting backend API..."
(cd web-api && dotnet watch --launch-profile https)

wait