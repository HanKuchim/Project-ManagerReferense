# Project Manager

Project Manager is a web-based project management application built with ASP.NET Core MVC.

The application allows users to create projects, invite team members, assign tasks, and track task progress within collaborative teams.

## Features

* User registration and authentication
* Project creation and management
* Role-based project membership (Admin, Moderator, User)
* Task creation and hierarchical subtasks
* Task assignment to project members
* Task status tracking
* Project invitation system
* Personal dashboard with assigned tasks

## Technologies

* C#
* ASP.NET Core MVC
* Entity Framework Core
* ASP.NET Identity
* SQL Server
* Razor Views
* Bootstrap

## Architecture

The application follows the MVC architectural pattern.

Controllers handle user requests, Models represent the domain entities and database schema, and Views render the user interface using Razor templates.

Entity Framework Core is used as the ORM for database access and migrations.

## Database

The database includes the following main entities:

* Users
* Projects
* ProjectMembers
* ProjectRoles
* Tasks
* TaskAssignments
* Comments
* ProjectInvitations

## Authentication

User authentication and authorization are implemented using ASP.NET Identity.
