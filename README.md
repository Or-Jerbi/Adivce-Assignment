# Advice Assignment

This is a full stack elevator management system built with ASP.NET Core for the backend and React for the frontend.

The system allows users to register, create buildings, and simulate elevator behavior in real-time using SignalR. Each building includes a single elevator, and users can request elevator calls between floors.

## Project Structure

```
Advice-Assignment/
├── server/        # Backend - ASP.NET Core Web API
│   ├── Controllers/
│   ├── DAL/
│   ├── DTOs/
│   ├── Hubs/
│   ├── Services/
│   ├── Program.cs
│   └── ...
├── client/                   # Frontend - React application
│   ├── src/
│   ├── public/
│   ├── package.json
│   └── ...
```

Note: the actual folder names are `AdviceAssignement` (for the server) and `advice` (for the client), but we'll refer to them here as `server/` and `client/` for clarity.

## How to Run the Project

### 1. Clone the Repository

### 2. Run the Backend (ASP.NET Core)

```bash
cd AdviceAssignement
dotnet restore
dotnet run
```

### 3. Run the Frontend (React)

```bash
cd advice
npm install
npm start
```

### 4. Configure Environment Variables

```
REACT_APP_BACKEND_URL=https://localhost:7266
```

## Features

- User registration and login with JWT
- Create and manage buildings
- Elevator simulation with real-time updates using SignalR
- Direction-aware elevator call logic
- Separated front-end and back-end architecture

## Technologies Used

- Backend: ASP.NET Core, SignalR, Entity Framework Core, Dapper
- Frontend: React, Axios, SignalR JavaScript Client
- Database: SQL Server

## Notes

- Make sure SQL Server is installed and the connection string is configured correctly.
- The `.env` file should not contain secrets if pushed to a public repository.
- The backend and frontend are completely decoupled and can be deployed separately.

### What I would improve with more time

- A more robust and secure authentication system (password encryption, validation, better error handling)
- Cleaner, more consistent codebase and structure
- Improved UI/UX design and responsiveness
- More intuitive navigation flow between screens
- Unit and integration tests


<img width="1851" height="898" alt="image" src="https://github.com/user-attachments/assets/4810c2ab-6742-4265-87ab-eb2f45a69d6a" />

<img width="1829" height="917" alt="image" src="https://github.com/user-attachments/assets/b8eeffaa-db73-44ad-a31c-ef84fb812552" />
